using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsForTesters.Core
{
    /// <summary>
    ///     needed information about an account
    /// </summary>
    public class Account
    {
        /// <summary>
        ///     Forced by Jenkins api/json
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        ///     List of jobs available in account
        /// </summary>
        public List<Job> Jobs { get; set; }
    }
}
