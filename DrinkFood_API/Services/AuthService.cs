using DataBase.Entities;
using DrinkFood_API.Exceptions;
using DrinkFood_API.Model;
using DrinkFood_API.Service;
using DrinkFood_API.Utility;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text;

namespace DrinkFood_API.Services
{
    public class AuthService : BaseService
    {
        private readonly string _key;

        public Guid UserID { get; set; }

        public AuthService(IServiceProvider provider) : base()
        {
            provider.Inject(this);
        }

        /// <summary>
        /// 檢查Token是否正確 (Request版)
        /// </summary>
        public bool CheckToken(HttpRequest Request, bool tokenIsNormal = true)
        {
            string HeaderToken = Request.Headers["Authorization"].FirstOrDefault() ?? throw new ApiException("缺少Token", 401);
            return CheckToken(HeaderToken, tokenIsNormal);
        }

        /// <summary>
        /// 檢查Token是否正確 (string版)
        /// </summary>
        public bool CheckToken(string Token, bool tokenIsNormal = true)
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                throw new ApiException("Token不可為空字串", 401);
            }
            return CheckTokenLogic(SplitSpaceToken(Token), tokenIsNormal);
        }

        #region CheckToken 邏輯

        /// <summary>
        /// 解析Token中含有空白字元的情況
        /// </summary>
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
        private bool CheckTokenLogic(string token, bool tokenIsNormal)
        {
            string[] split = token.Split('.');
            string iv = split[0];
            string encrypt = split[1];
            string signature = split[2];

            //檢查簽章是否正確
            if (signature != TokenCrypto.ComputeHMACSHA256(iv + "." + encrypt, _key.Substring(0, 64)))
            {
                throw new ApiException("缺少Token", 401);
            }

            //使用 AES 解密 Payload
            string base64 = TokenCrypto.AESDecrypt(encrypt, _key.Substring(0, 16), iv);
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
                UserId = payload.UserId;
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

        public void CreateToken(Guid accountID, Action<string> Callback)
        {
            DateTime TokenCreateDate = DateTime.Now;
            Payload payload = new()
            {
                UserId = accountID,
                CreateDate = TokenCreateDate
            };

            string json = JsonConvert.SerializeObject(payload);
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            string iv = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);

            //使用 AES 加密 Payload
            string encrypt = TokenCrypto.AESEncrypt(base64, _key.Substring(0, 16), iv);

            //取得簽章
            string signature = TokenCrypto.ComputeHMACSHA256(iv + "." + encrypt, _key.Substring(0, 64));

            Callback?.Invoke(iv + "." + encrypt + "." + signature);
        }
    }
}
