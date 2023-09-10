using Microsoft.AspNetCore.Identity;

namespace BLL.Providers
{
    /// <summary>
    /// Options for configuring the custom token provider for handling invitation tokens related to company access.
    /// </summary>
    public class InvitationToCompanyTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        /// <summary>
        /// The name of the custom token provider.
        /// </summary>
        public static readonly string TokenProvider = "InvitationToCompanyTokenProvider";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvitationToCompanyTokenProviderOptions"/> class with default settings.
        /// </summary>
        public InvitationToCompanyTokenProviderOptions()
        {
            Name = TokenProvider;
            TokenLifespan = TimeSpan.FromMinutes(30);
        }
    }
}
