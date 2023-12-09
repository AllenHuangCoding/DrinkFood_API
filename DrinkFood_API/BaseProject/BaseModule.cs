//using Newtonsoft.Json;
//using Service.HTTP;
//using System.Net.Http.Headers;

//namespace CodeShare.Libs.BaseProject.Context
//{
//    /// <summary>
//    /// 呼叫對外系統基礎模組
//    /// </summary>
//    public class BaseModule
//    {
//        /// <summary>
//        /// Http服務
//        /// </summary>
//        protected HttpService _httpService { get; set; }

//        /// <summary>
//        /// 設定檔
//        /// </summary>
//        protected IConfiguration _configuration { get; set; }

//        /// <summary>
//        /// 資料庫
//        /// </summary>
//        protected EFContext _context { get; set; }

//        /// <summary>
//        /// 權限服務
//        /// </summary>
//        protected PermissionService _permissionService { get; set; }

//        /// <summary>
//        /// 建構元 (給EPA的ResponseHandler用多傳PermissionService)
//        /// </summary>
//        public BaseModule(EFContext context, PermissionService permissionService, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//            _permissionService = permissionService;
//            _httpService = new HttpService();
//        }

//        /// <summary>
//        /// 建構元
//        /// </summary>
//        public BaseModule(EFContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//            _httpService = new HttpService();
//        }

//        /// <summary>
//        /// 建構元
//        /// </summary>
//        public BaseModule()
//        {
//            _httpService = new HttpService();
//        }

//        /// <summary>
//        /// 設定Token
//        /// </summary>
//        protected void SetToken(TokenTypes Type, string Token)
//        {
//            switch (Type)
//            {
//                case TokenTypes.Bearer:
//                    _httpService.Client.DefaultRequestHeaders.Accept.Clear();
//                    _httpService.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
//                    break;
//                case TokenTypes.EPA:
//                    _httpService.Client.DefaultRequestHeaders.Accept.Clear();
//                    _httpService.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//                    _httpService.Client.DefaultRequestHeaders.Add("Authorization", "AuthCode " + Token);
//                    break;
//            }
//        }

//        protected void ResponseHandler(string RequestName, object Param, HttpResponseMessage GetResponse, Action<Stream> Callback)
//        {
//            if (GetResponse != null && GetResponse.IsSuccessStatusCode)
//            {
//                using (var stream = GetResponse.Content.ReadAsStream())
//                {
//                    Callback.Invoke(stream);
//                }
//            }
//        }

//        /// <summary>
//        /// WaaS Log 紀錄
//        /// </summary>
//        protected void ResponseHandler<T>(string RequestName, object Param, BaseWaaSApiResponseModel<T> GetResponse, Action<T> Callback)
//        {
//            if (GetResponse != null && GetResponse.Success && GetResponse.Data != null)
//            {
//                Callback.Invoke(GetResponse.Data);
//            }
//            else if (GetResponse != null && !GetResponse.Success) 
//            {
//                throw new ApiException(GetResponse.Message , GetResponse.Code);
//            }
//        }


//        /// <summary>
//        /// 大云環保平台API的回傳資料格式解析
//        /// </summary>
//        protected void ResponseHandler<T>(string RequestName, object Param, BaseChaseApiResponseModel<T> GetResponse, Action<T> Callback)
//        {
//            ChaseRecord chaseRecord = new()
//            {
//                Chase_account_uuid = "System",
//                Chase_request = JsonConvert.SerializeObject(Param),
//                Chase_response_code = "200",
//                Chase_response = JsonConvert.SerializeObject(GetResponse)
//            };

//            // 清除資料庫目前咬住的資料
//            _context.DetachAllEntities();

//            if (GetResponse != null && GetResponse.Success)
//            {
//                chaseRecord.Chase_event = RequestName;
//                _context.ChaseRecord.Add(chaseRecord);
//                _context.SaveChanges();

//                if (GetResponse.Data != null)
//                {
//                    Callback.Invoke(GetResponse.Data);
//                }
//            }
//            else
//            {
//                chaseRecord.Chase_event = "發生錯誤:" + RequestName;
//                chaseRecord.Chase_response_code = "400";
//                _context.ChaseRecord.Add(chaseRecord);
//                _context.SaveChanges();
//            }
//        }

//        /// <summary>
//        /// 環保署開放資料API的回傳資料格式解析
//        /// </summary>
//        protected void ResponseHandler<T>(string RequestName, object Param, BaseEpaOpenDataApiResponseModel<T> GetResponse, Action<T> Callback)
//        {
//            if (GetResponse != null)
//            {
//                if (GetResponse.records != null)
//                {
//                    Callback.Invoke(GetResponse.records);
//                }
//            }
//        }

//        /// <summary>
//        /// 環保署申報系統API的回傳資料格式解析
//        /// </summary>
//        protected void ResponseHandler<T>(string RequestName, object Param, BaseEpaApiResponseModel<T> GetResponse, Action<T> Callback, Action<string> ErrorCallback)
//        {
//            if (GetResponse != null)
//            {
//                if (GetResponse.ErrorCode == "0")
//                {
//                    _context.DetachAllEntities();
//                    EPARecord EPARecord = new EPARecord()
//                    {
//                        EPA_account_uuid = _permissionService.AccountID.ToString(),
//                        EPA_request = JsonConvert.SerializeObject(Param),
//                        EPA_response_code = GetResponse.ErrorCode,
//                        EPA_event = RequestName,
//                        EPA_response = JsonConvert.SerializeObject(GetResponse)
//                    };
//                    _context.EPARecord.Add(EPARecord);
//                    _context.SaveChanges();

//                    Callback.Invoke(GetResponse.ClearDoc);
//                }
//                else 
//                {
//                    _context.DetachAllEntities();
//                    EPARecord EPARecord = new EPARecord()
//                    {
//                        EPA_account_uuid = _permissionService.AccountID.ToString() ,
//                        EPA_request = JsonConvert.SerializeObject(Param),
//                        EPA_response_code = "400",
//                        EPA_event = RequestName,
//                        EPA_response = JsonConvert.SerializeObject(GetResponse)
//                    };
//                    _context.EPARecord.Add(EPARecord);
//                    _context.SaveChanges();

//                    ErrorCallback.Invoke(GetResponse.ErrorMsg);
//                }
//            }
//        }
//    }
//}
