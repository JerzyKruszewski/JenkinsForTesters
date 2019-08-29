using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsForTesters.Core
{
    /// <summary>
    ///     needed information about job
    /// </summary>
    public class Job
    {
        /// <summary>
        ///     Forced by Jenkins api/json
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        ///     Job name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Job url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Job color:
        ///     red - fail
        ///     blue - pass
        /// </summary>
        public string Color { get; set; }
    }
}
