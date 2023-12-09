//namespace CodeShare.Libs.BaseProject.Context
//{
//    /// <summary>
//    /// Token類型列舉
//    /// </summary>
//    public enum TokenTypes
//    {
//        /// <summary>
//        /// Bearer
//        /// </summary>
//        Bearer,

//        /// <summary>
//        /// 環保署
//        /// </summary>
//        EPA,
//    }

//    /// <summary>
//    /// 呼叫對外系統基礎類型
//    /// </summary>
//    public class BaseContext
//    {
//        /// <summary>
//        /// 設定檔
//        /// </summary>
//        protected IConfiguration _configuration { get; set; }

//        /// <summary>
//        /// 資料庫
//        /// </summary>
//        public EFContext _context { get; set; }

//        /// <summary>
//        /// 權限服務
//        /// </summary>
//        public PermissionService _permissionService { get; set; }
        
//        /// <summary>
//        /// 建構元 (給EPA的ResponseHandler用多傳PermissionService)
//        /// </summary>
//        public BaseContext(EFContext context, PermissionService PermissionService, IConfiguration configuration)
//        {
//            _context = context;
//            _permissionService = PermissionService;
//            _configuration = configuration;
//        }

//        /// <summary>
//        /// 建構元
//        /// </summary>
//        public BaseContext(EFContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }

//        /// <summary>
//        /// 建構元
//        /// </summary>
//        public BaseContext()
//        {

//        }
//    }
//}
