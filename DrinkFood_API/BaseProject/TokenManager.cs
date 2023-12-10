using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace CodeShare.Libs.BaseProject
{
    /// <summary>
    /// 檢查Token資料庫邏輯的委派結果
    /// </summary>
    /// <param name="token"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    public delegate bool TokenLogicDelegate(string token, Payload payload);

    /// <summary>
    /// 檢查Token資料庫邏輯的Interface
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// 使用者ID
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 檢查Token資料庫邏輯
        /// </summary>
        /// <param name="token"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public bool CheckTokenLogic(string token, Payload payload);

        /// <summary>
        /// 檢查管理員身分 (非管理員噴出ApiException)
        /// </summary>
        public void CheckAdmin();
    }

    /// <summary>
    /// Token內藏的使用者資訊
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        public Guid UserID { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        public Payload()
        {
            CreateDate = DateTime.Now;
        }
    }

    /// <summary>
    /// Token管理
    /// </summary>
    public class TokenManager
    {
        [Inject] private readonly IConfiguration _configuration;

        /// <summary>
        /// 設定檔中的加密Key
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// 建構元
        /// </summary>
        /// <param name="provider"></param>
        public TokenManager(IServiceProvider provider) 
        { 
            provider.Inject(this);

            if (_configuration!.GetSection("Token")["Key"] == null)
            {
                throw new ArgumentException("appsettings Token Key");
            }

            _key = _configuration!.GetSection("Token")["Key"]!.ToString();
        }

        /// <summary>
        /// 檢查Token是否正確 (Request版)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public bool Check(HttpRequest request, TokenLogicDelegate callback)
        {
            // 從Request中取出欄位交給字串版Check判斷
            string HeaderToken = request.Headers["Authorization"].FirstOrDefault() ?? throw new ApiException("缺少Token", 401);
            return Check(HeaderToken, callback);
        }

        /// <summary>
        /// 檢查Token是否正確 (string版)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public bool Check(string token, TokenLogicDelegate callback)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ApiException("Token不可為空字串", 401);
            }
            return Decode(token.Replace("Bearer ", "").Replace(" ", "+"), callback);
        }

        #region 私有方法：Decode

        /// <summary>
        /// Token解析
        /// </summary>
        /// <param name="token"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        private bool Decode(string token, TokenLogicDelegate callback)
        {
            string[] split = token.Split('.');
            string iv = split[0];
            string encrypt = split[1];
            string signature = split[2];

            //檢查簽章是否正確
            if (signature != ComputeHMACSHA256(iv + "." + encrypt, _key.Substring(0, 64)))
            {
                throw new ApiException("Token格式錯誤", 401);
            }

            //使用 AES 解密 Payload
            string base64 = AESDecrypt(encrypt, _key.Substring(0, 16), iv);
            string json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            Payload payload = JsonConvert.DeserializeObject<Payload>(json);
            return callback.Invoke(token, payload);
        }

        #endregion

        /// <summary>
        /// 產生Token
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="callback"></param>
        public void Create(Guid accountID, Action<string> callback)
        {
            DateTime TokenCreateDate = DateTime.Now;
            Payload payload = new()
            {
                UserID = accountID,
                CreateDate = TokenCreateDate
            };

            string json = JsonConvert.SerializeObject(payload);
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            string iv = accountID.ToString().Replace("-", "").Substring(0, 16);

            //使用 AES 加密 Payload
            string encrypt = AESEncrypt(base64, _key.Substring(0, 16), iv);

            //取得簽章
            string signature = ComputeHMACSHA256(iv + "." + encrypt, _key.Substring(0, 64));

            callback?.Invoke(iv + "." + encrypt + "." + signature);
        }

        #region 私有方法：加解密、產生雜湊

        /// <summary>
        /// 產生 HMACSHA256 雜湊
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string ComputeHMACSHA256(string data, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (HMACSHA256 hmacSHA = new HMACSHA256(keyBytes))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] hash = hmacSHA.ComputeHash(dataBytes, 0, dataBytes.Length);
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static string AESEncrypt(string data, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] encrypt = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                return Convert.ToBase64String(encrypt);
            }
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static string AESDecrypt(string data, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] dataBytes = Convert.FromBase64String(data);
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] decrypt = decryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                return Encoding.UTF8.GetString(decrypt);
            }
        }

        #endregion
    }
}
