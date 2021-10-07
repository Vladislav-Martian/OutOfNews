using System;
using Microsoft.AspNetCore.Identity;

namespace OutOfNews.Models
{
    public class User: IdentityUser
    {

        /// <summary>
        /// Alternative name, that can be shown as name.
        /// </summary>
        public string NickName { get; set; } = null;
        
        /// <summary>
        /// Born date, for managing NSFW content.
        /// </summary>
        public DateTime? Born { get; set; } = null;
        

        #region Methods

        public bool IsAdult(int minimal = 16, bool alter = false)
        {
            if (Born == null) return alter; // Use config Restrictions.UnauthorizedAdult
            var diff = DateTime.Now - Born.Value;
            return ((double)minimal * 365.25) < diff.TotalDays;
        }

        public string GetNameToShow()
        {
            if (!string.IsNullOrEmpty(NickName))
            {
                return NickName;
            }
            return !string.IsNullOrEmpty(UserName) ? UserName : Email;
        }

        #endregion
    }
}