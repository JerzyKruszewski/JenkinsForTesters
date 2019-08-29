using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsForTesters.Core
{
    /// <summary>
    ///     configuration information
    /// </summary>
    public class Config
    {
        /// <summary>
        ///     Jenkins server adress
        /// </summary>
        public string IPAdress { get; set; }

        /// <summary>
        ///     Jenkins server port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     Login aka. Username
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///     Jenkins API Token. Used for authorization.
        /// </summary>
        public string Token { get; set; }
    }
}
