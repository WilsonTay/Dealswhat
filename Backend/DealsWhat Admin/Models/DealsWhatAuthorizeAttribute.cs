using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace DealsWhat_Admin.Models
{
    public class DealsWhatAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return true;
            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity;

            if (claimsIdentity != null)
            {
                var claims = claimsIdentity.Claims;

                var emailClaim =
                    claims.FirstOrDefault(
                        c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"));

                if (emailClaim != null && emailClaim.Value.Equals("taykahhoe@gmail.com"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}