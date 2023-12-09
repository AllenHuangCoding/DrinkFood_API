using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CodeShare.Libs.BaseProject
{
    public class LineNotify
    {
        private readonly string _client_id;

        private readonly string _client_secret;

        private readonly string _redirect_uri;

        public LineNotify(string client_id, string client_secret, string redirect_uri)
        {
            if (!string.IsNullOrWhiteSpace(client_id))
            {
                throw new ArgumentNullException(nameof(client_id));
            }

            if (!string.IsNullOrWhiteSpace(client_secret))
            {
                throw new ArgumentNullException(nameof(client_secret));
            }

            if (!string.IsNullOrWhiteSpace(redirect_uri))
            {
                throw new ArgumentNullException(nameof(redirect_uri));
            }

            _client_id = client_id;
            _client_secret = client_secret;
            _redirect_uri = redirect_uri;
        }

        public string GetAccessToken(string code)
        {
            //建立 HttpClient
            HttpClient client = new HttpClient();
            string requestString = $"grant_type=authorization_code&code={code}&redirect_uri={_redirect_uri}&client_id={_client_id}&client_secret={_client_secret}";

            // 將轉為 string 的 json 依編碼並指定 content type 存為 httpcontent
            HttpContent contentPost = new StringContent(requestString, Encoding.UTF8, "application/x-www-form-urlencoded");
            // 發出 post 並取得結果
            HttpResponseMessage response = client.PostAsync("https://notify-bot.line.me/oauth/token", contentPost).GetAwaiter().GetResult();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ApiException("找不到Access Token", 400);
            }

            //取得Line ID(access token)
            string content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["access_token"];
        }

        public async Task Notify(string accessToken, string message)
        {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            HttpContent content = new StringContent($"message={message}", Encoding.UTF8, "application/x-www-form-urlencoded");
            _ = await httpClient.PostAsync("https://notify-api.line.me/api/notify", content);
        }
    }
}
