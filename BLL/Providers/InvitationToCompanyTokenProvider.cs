using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BLL.Providers
{
    /// <summary>
    /// Provides a custom token provider for handling invitation tokens related to company access.
    /// </summary>
    /// <typeparam name="TUser">The type of user associated with the token provider.</typeparam>
    public class InvitationToCompanyTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvitationToCompanyTokenProvider{TUser}"/> class.
        /// </summary>
        /// <param name="dataProtectionProvider">The data protection provider used for token generation.</param>
        /// <param name="options">The options for configuring the token provider.</param>
        /// <param name="logger">The logger used for logging token provider activities.</param>
        public InvitationToCompanyTokenProvider(
            IDataProtectionProvider dataProtectionProvider, 
            IOptions<InvitationToCompanyTokenProviderOptions> options, 
            ILogger<DataProtectorTokenProvider<TUser>> logger) 
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
}
