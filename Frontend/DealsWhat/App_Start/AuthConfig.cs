using Microsoft.Web.WebPages.OAuth;

namespace DealsWhat
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            OAuthWebSecurity.RegisterMicrosoftClient(
                clientId: "0000000040154D20",
                clientSecret: "TORcakjT8UhYbPkC-icFTsrmJHBLttwS");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient("360180917522724", "d373f3029d094f058acc3e97f5d4096d");
            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "360181580855991",
            //    appSecret: "fefe43ecb138330cccb98b7c9a372340");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
