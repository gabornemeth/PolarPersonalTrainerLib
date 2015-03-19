using System;
using System.Collections.Generic;
using System.Text;

namespace PolarPersonalTrainerLib
{
    /// <summary>
    /// Unit system
    /// </summary>
    public enum Units
    {
        Metric,
        Imperial
    }

    /// <summary>
    /// User settings available at /user/settings/index.ftl
    /// </summary>
    public class PPTUser
    {
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Nickname
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// Unit system used
        /// </summary>
        public Units Units { get; set; }
        /// <summary>
        /// Date format
        /// </summary>
        public string DateFormat { get; set; }
    }
}
