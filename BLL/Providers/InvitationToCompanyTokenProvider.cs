using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BLL.Providers
{
    public class InvitationToCompanyTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public InvitationToCompanyTokenProvider(
            IDataProtectionProvider dataProtectionProvider, 
            IOptions<InvitationToCompanyTokenProviderOptions> options, 
            ILogger<DataProtectorTokenProvider<TUser>> logger) 
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
}
