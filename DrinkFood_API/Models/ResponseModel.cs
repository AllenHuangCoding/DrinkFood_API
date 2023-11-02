namespace DrinkFood_API.Model
{
    #region 單筆回傳
    public class ResponseModel
    {
        /// <summary>
        /// 回傳代碼
        /// </summary>
        public int Code { get; set; } = 200;
        /// <summary>
        /// 要求是否成功
        /// </summary>
        public bool Success { get; set; } = true;
        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = "執行成功";


        public ResponseModel()
        {

        }
        public ResponseModel(bool success)
        {
            Success = success;
        }
        public ResponseModel(string message)
        {
            Message = message;
        }
        public ResponseModel(bool success, int code, string message)
        {
            Success = success;
            Code = code;
            Message = message;
        }

        public ResponseModel(SimpleResult simpleResult)
        {
            Success = simpleResult.Success;
            Code = simpleResult.Code;
            Message = simpleResult.ShowMessage ?? simpleResult.Message;
        }
    }
    #endregion

    #region 陣列回傳
    /// <summary>
    ///回應資料時的基礎 Response 物件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseData<T> : ResponseModel
    {
        /// <summary>
        /// 總筆數
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 回傳的資料
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 建立一個  Response 物件
        /// </summary>
        /// <param name="data"></param>
        public ResponseData(T data)
        {
            Data = data;
        }

        /// <summary>
        /// 建立一個 成功 Response 物件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        public ResponseData(T data, int count)
        {
            Data = data;
            Count = (data == null) ? 0 : count;
        }

        /// <summary>
        /// 建立一個 Response 物件
        /// </summary>
        /// <param name="success"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseData(bool success, int code, string message)
        {
            Success = success;
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 建立一個 Response 物件
        /// </summary>
        /// <param name="success"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public ResponseData(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        /// <summary>
        /// 建立一個 Response 物件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="message"></param>
        public ResponseData(T data, int count, string message)
        {
            Data = data;
            Count = (data == null) ? 0 : count;
            Message = message;
        }

        /// <summary>
        /// 建立一個 Response 物件
        /// </summary>
        /// <param name="success"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="message"></param>
        public ResponseData(bool success, T data, int count, string message)
        {
            Success = success;
            Data = data;
            Count = (data == null) ? 0 : count;
            Message = message;
        }

        /// <summary>
        /// 建立一個 ResponseData 物件
        /// </summary>
        /// <param name="success"></param>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ResponseData(bool success, T data, int count, int code, string message)
        {
            Success = success;
            Data = data;
            Count = (data == null) ? 0 : count;
            Message = message;
            Code = code;
        }

        public ResponseData(SimpleResult simpleResult, T data, int count) : base(simpleResult)
        {
            Data = data;
            Count = (data == null) ? 0 : count;
        }
    }
    #endregion

    #region 簡易暫存回傳訊息
    public class SimpleResult
    {
        public bool Success { get; set; } = true;
        public int Code { get; set; } = 200;
        public int Count { get; set; }
        public string Message { get; set; } = "執行成功";
        public object Data { get; set; } 
        public string ShowMessage { get; set; }
        public Exception Exception { get; set; }
    }
    #endregion
}
