using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace CodeShare.Libs.BaseProject
{
    public class TokenManager
    {
        [Inject] private readonly IConfiguration _configuration;

        /// <summary>
        /// 設定檔中的加密Key
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public TokenManager(IServiceProvider provider) 
        { 
            provider.Inject(this);

            _key = _configuration!.GetSection("Token")["Key"]!.ToString();
        }

        /// <summary>
        /// 檢查Token是否正確 (Request版)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="tokenIsNormal"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public bool Check(HttpRequest request, bool tokenIsNormal = true)
        {
            // 從Request中取出欄位交給字串版Check判斷
            string HeaderToken = request.Headers["Authorization"].FirstOrDefault() ?? throw new ApiException("缺少Token", 401);
            return Check(HeaderToken, tokenIsNormal);
        }

        /// <summary>
        /// 檢查Token是否正確 (string版)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenIsNormal"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public bool Check(string token, bool tokenIsNormal = true)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ApiException("Token不可為空字串", 401);
            }
            return CheckLogic(SplitSpaceToken(token), tokenIsNormal);
        }

        #region CheckToken 邏輯

        /// <summary>
        /// 解析Token中含有空白字元的情況
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        private static string SplitSpaceToken(string Token)
        {
            if (!string.IsNullOrWhiteSpace(Token))
            {
                var SplitToken = Token.Split("Bearer ").ToList();
                if (SplitToken.Count >= 2)
                {
                    return SplitToken[1].Replace(" ", "+");
                }
                else
                {
                    return Token.Replace(" ", "+");
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 檢查Token邏輯
        /// </summary>
        /// <param name="Token"></param>
        /// <param name="tokenIsNormal"></param>
        /// <returns></returns>
        private bool CheckLogic(string token, bool tokenIsNormal)
        {
            string[] split = token.Split('.');
            string iv = split[0];
            string encrypt = split[1];
            string signature = split[2];

            //檢查簽章是否正確
            if (signature != ComputeHMACSHA256(iv + "." + encrypt, _key.Substring(0, 64)))
            {
                throw new ApiException("缺少Token", 401);
            }

            //使用 AES 解密 Payload
            string base64 = AESDecrypt(encrypt, _key.Substring(0, 16), iv);
            string json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            Payload payload = JsonConvert.DeserializeObject<Payload>(json);


            //驗證時效性

            /*
            AccountLogin LoginRecord = _context.AccountLogin.Where(a =>
                a.AL_token == string.Format("Bearer {0}", token) &&
                a.AL_normal == tokenIsNormal
            ).FirstOrDefault();

            if (LoginRecord != null)
            {
                // 如果已經超過時限，則回傳過期
                if (LoginRecord.AL_validity < DateTime.Now)
                {
                    SimpleResult = new SimpleResult()
                    {
                        Success = false,
                        Message = "連線已逾時，請重新登入"
                    };
                    return false;
                }
                else
                {
                    if (tokenIsNormal)
                    {
                        List<AccountLogin> LoginRecords = _context.AccountLogin.Where(a => a.AL_validity >= DateTime.Now).ToList();

                        //因為登入時候會自動創建相同create時間的不同系統多個token
                        //故將token依照時間做分群後取得現在群裡面有原先token的組別再一口氣加2小時的驗證時間
                        LoginRecords = LoginRecords.GroupBy(x => x.AL_create).Where(x => x.Select(x => x.AL_id).Contains(LoginRecord.AL_id)).FirstOrDefault().Select(x => { return x; }).ToList();
                        LoginRecords = LoginRecords.Select(x =>
                        {
                            x.AL_validity = DateTime.Now;
                            x.AL_validity = LoginRecord.AL_validity.AddHours(2);
                            x.AL_update = DateTime.Now;
                            return x;
                        }).ToList();

                        LoginRecordList = LoginRecords;
                        ProductID = LoginRecord.AL_product_id;
                    }
                    else
                    {
                        // 忘記密碼Token呼叫API: 已驗證成功，砍掉30分鐘的暫時性Token
                        _context.AccountLogin.Remove(LoginRecord);
                        _context.SaveChanges();
                    }
                }
                IsAdmin = _context.NewAccount.Where(x => x.A_id == UserId).Select(x => x.A_isAdmin).FirstOrDefault();
                IsSystem = _context.NewAccount.Where(x => x.A_id == UserId).Select(x => x.A_type == "System").FirstOrDefault();

                SimpleResult = new SimpleResult()
                {
                    Success = true
                };
                return true;
            }
            else
            {
                SimpleResult = new SimpleResult()
                {
                    Success = false,
                    Message = "Token驗證失敗"
                };
                return false;
            }
            */
            return true;
        }

        #endregion

        public void Create(Guid accountID, Action<string> callback)
        {
            DateTime TokenCreateDate = DateTime.Now;
            Payload payload = new()
            {
                UserId = accountID,
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

        #region 私有方法：檢查設定檔key、加解密、產生雜湊

        /// <summary>
        /// 產生 HMACSHA256 雜湊
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string ComputeHMACSHA256(string data, string key)
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
        private string AESEncrypt(string data, string key, string iv)
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
        private string AESDecrypt(string data, string key, string iv)
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
