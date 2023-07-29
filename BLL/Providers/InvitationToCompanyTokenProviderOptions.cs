using Microsoft.AspNetCore.Identity;

namespace BLL.Providers
{
    public class InvitationToCompanyTokenProviderOptions : DataProtectionTokenProviderOptions
    {

        public static readonly string TokenProvider = "InvitationToCompanyTokenProvider";

        public InvitationToCompanyTokenProviderOptions()
        {
            Name = TokenProvider;
            TokenLifespan = TimeSpan.FromMinutes(30);
        }

    }
}
