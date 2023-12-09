using CodeShare.Libs.BaseProject;

namespace DrinkFood_API.Utility
{
    public class LineService : BaseService
    {
        private readonly LineNotify line;

        public LineService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);

            // 產生Line Notify類別
            line = new(
                _configuration.GetSection("Line")["client_id"]!,
                _configuration.GetSection("Line")["client_secret"]!,
                _configuration.GetSection("Line")["redirect_uri"]!
            );
        }

        public string GetAccessToken(string code)
        {
            return line.GetAccessToken(code);
        }

        public void CreateLunchOrderNotify()
        {

        }

        public void CreateDrinkOrderNotify()
        {

        }
    }
}
