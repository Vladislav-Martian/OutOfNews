using System;

namespace OutOfNews.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Pass { get; set; }
        
        public string Name { get; set; } = null;
        public DateTime? BornDate { get; set; } = null;

        public int UserConfigId { get; set; }
        public UserConfig UserConfig { get; set; }

        #region Tough model methods

        public bool IsAdult(int minimal = 16, bool alter = false)
        {
            if (BornDate == null) return alter; // Use config Restrictions.UnauthorizedAdult
            var diff = DateTime.Now - BornDate;
            return ((double)minimal * 365.25) < diff.Value.TotalDays;
        }

        public string GetNameToShow()
        {
            if (UserConfig != null && UserConfig.UseNickname && !string.IsNullOrEmpty(UserConfig.Nickname))
            {
                return UserConfig.Nickname;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                return Name;
            }

            return Mail;
        }

        #endregion
    }
}