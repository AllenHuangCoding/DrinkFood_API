namespace CodeShare.Libs.BaseProject
{
    public class BaseService
    {
        [Inject] protected IConfiguration _configuration;

        public BaseService(IServiceProvider provider)
        {
            provider.Inject(this);
        }
    }
}
