using System;
using System.Web;

namespace ab.Mvc
{
    public class Identity
    {
        /// <summary>
        /// A default identity that is based on the user first, if they are identifiable,
        /// and then the request's anonymous ID, which assumes use of ASP.NET's anonymous
        /// authentication module, which is part of the default HttpModule pipeline and is
        /// enabled with this bit of code:
        /// <code>
        /// <system.web>
        ///     <anonymousIdentification enabled="true" />
        /// </system.web>
        /// </code>
        /// If both of these identity methods fail, then the IP address is tried.
        /// </summary>
        public static Func<string> Default
        {
            get
            {
                return () =>
                {
                    var context = HttpContext.Current;
                    if (context == null) return null;
                    var identity = context.User != null && context.User.Identity.IsAuthenticated
                                       ? context.User.Identity.Name
                                       : context.Request.AnonymousID;
                    if(identity == null)
                    {
                        var request = context.Request;
                        if (request.IsLocal) return "127.0.0.1";
                        var proxy = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (!string.IsNullOrWhiteSpace(proxy))
                        {
                            var ipArray = proxy.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (ipArray.Length > 0)
                            {
                                return ipArray[0];
                            }
                        }
                        return request.UserHostAddress;
                    }
                    return identity;
                };
            }
        }
    }
}