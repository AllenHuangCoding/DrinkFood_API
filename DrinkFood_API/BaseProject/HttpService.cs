using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace Service.HTTP
{
    public delegate void LogFunc(Exception ex , string RequestString);


    public class HttpService 
    {
        public HttpClient Client { get; set; } = new();

        public string BaseUrl { get; set; }

        public LogFunc DoLogFunc { get; set; }

        public T Get<T>(string EndPoint, Dictionary<string, string> Dict)
        {
            // 使用 HttpUtility.ParseQueryString 方法將 Dictionary<string, string> 轉換為 NameValueCollection 物件
            var nvc = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in Dict)
            {
                nvc[item.Key] = item.Value;
            }

            // 使用 NameValueCollection.ToString 方法將 NameValueCollection 物件轉換為 GET 網址字串
            string queryString = nvc.ToString();

            var response = Client.GetAsync(BaseUrl + EndPoint + "?" + queryString).Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            return default;
        }

        public HttpResponseMessage File(string EndPoint, Dictionary<string, string> Dict)
        {
            // 使用 HttpUtility.ParseQueryString 方法將 Dictionary<string, string> 轉換為 NameValueCollection 物件
            var nvc = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in Dict)
            {
                nvc[item.Key] = item.Value;
            }

            // 使用 NameValueCollection.ToString 方法將 NameValueCollection 物件轉換為 GET 網址字串
            string queryString = nvc.ToString();

            var response = Client.GetAsync(BaseUrl + EndPoint + "?" + queryString).Result;
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            return default;
        }

        public T Post<T>(string EndPoint, object Param)
        {
            try
            {
                var response = Client.PostAsync(BaseUrl + EndPoint, new StringContent(JsonConvert.SerializeObject(Param), Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                if (DoLogFunc != null)
                {
                    DoLogFunc(ex, JsonConvert.SerializeObject(Param));
                }
            }
            return default;
        }

        public (T1, T2) Post<T1, T2>(string EndPoint, object Param)
        {
            var response = Client.PostAsync(BaseUrl + EndPoint, new StringContent(JsonConvert.SerializeObject(Param), Encoding.UTF8, "application/json")).Result;
            if (response.IsSuccessStatusCode)
            {
                return (JsonConvert.DeserializeObject<T1>(response.Content.ReadAsStringAsync().Result), default(T2));
            }
            return (default(T1), JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result));
        }

        public T Put<T>(string EndPoint, object Param)
        {
            var response = Client.PutAsync(BaseUrl + EndPoint, new StringContent(JsonConvert.SerializeObject(Param), Encoding.UTF8, "application/json")).Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            return default;
        }

        public T Delete<T>(string EndPoint)
        {
            var response = Client.DeleteAsync(BaseUrl + EndPoint).Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            return default;
        }
    }
}
