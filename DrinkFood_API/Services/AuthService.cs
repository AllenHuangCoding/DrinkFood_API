using CodeShare.Libs.BaseProject;
using Newtonsoft.Json;
using System.Text;

namespace DrinkFood_API.Services
{
    public class AuthService : BaseService
    {
        public Guid UserID { get; set; }

        public AuthService(IServiceProvider provider) : base(provider)
        {
            provider.Inject(this);
        }

        
    }
}
