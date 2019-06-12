using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFunction
{
    class ApplicationHelper
    {
        private static bool IsStarted = false;
        private static object _syncLock = new object();
        ///<summary>
        /// Sets up the app before running any other code
        /// </summary>

        public static void Startup()
        {
            if (!IsStarted)
            {
                lock (_syncLock)
                {
                    if (!IsStarted)
                    {
                        AssemblyBindingRedirectorHelper.ConfigureBindingRedirects();
                        IsStarted = true;
                    }
                }
            }
        }
    }
}
