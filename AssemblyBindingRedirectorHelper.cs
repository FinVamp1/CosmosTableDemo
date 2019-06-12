using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;

namespace TestFunction
{
    ///<summary>
    /// Manages "BindingRedirects" for the given set of assemblies.
    /// </summary>
    public static class AssemblyBindingRedirectorHelper
    {
        ///<summary>
        /// Reads the "BindingRedirects" field from the app settings and applies the redirection on the
        /// specified assemblies
        /// </summary>

        public static void ConfigureBindingRedirects()
        {
            var redirects = GetBindingRedirects();
            redirects.ForEach(RedirectAssembly);
        }

        private static List<BindingRedirect> GetBindingRedirects()
        {
            var redirects = new List<BindingRedirect>();
            var bindingRedirectListJson = Environment.GetEnvironmentVariable("BindingRedirects");
            if (!string.IsNullOrEmpty(bindingRedirectListJson))
            {
                redirects = JsonConvert.DeserializeObject<List<BindingRedirect>>(bindingRedirectListJson);
            }
            return redirects;
        }

        private static void RedirectAssembly(BindingRedirect bindingRedirect)
        {
            ResolveEventHandler handler = null;
            handler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                if (requestedAssembly.Name != bindingRedirect.ShortName)
                {
                    return null;
                }
                var targetPublicKeyToken = new AssemblyName("x, PublicKeyToken=" + bindingRedirect.PublicKeyToken).GetPublicKeyToken();
                requestedAssembly.SetPublicKeyToken(targetPublicKeyToken);
                requestedAssembly.Version = new Version(bindingRedirect.RedirectToVersion);
                requestedAssembly.CultureInfo = CultureInfo.InvariantCulture;
                AppDomain.CurrentDomain.AssemblyResolve -= handler;
                return Assembly.Load(requestedAssembly);
            };
            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }

        public class BindingRedirect
        {
            public string ShortName { get; set; }
            public string PublicKeyToken { get; set; }
            public string RedirectToVersion { get; set; }
        }
    }
}
