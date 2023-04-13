using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace TuyaCoreController.Enums
{
    /// <summary>
    /// Device online status Enum
    /// </summary>
    public enum OnlineState
    {
        /// <summary>
        /// Unknown state
        /// </summary>
        [Display(Name = "Unknown")]
        Unknown = 0,
        /// <summary>
        /// Offline state
        /// </summary>
        [Display(Name = "Offline")]
        Offline = 1,
        /// <summary>
        /// Online state
        /// </summary>
        [Display(Name = "Online")]
        Online = 2,
        /// <summary>
        /// Online via Gateway
        /// </summary>
        [Display(Name = "Online via Gate")]
        Online_via_Gate = 3
    }
}
