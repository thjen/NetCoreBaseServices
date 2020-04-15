using BaseArchitect.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseArchitect.CacheManager
{
    public class ProfileCaching
    {
        private static Dictionary<int, UserProfile> dctProfile = new Dictionary<int, UserProfile>();

        public static void CacheProfile(UserProfile profile)
        {
            if (profile == null)
            {
                return;
            }
            else
            {
                if (dctProfile.ContainsKey(profile.AccountID))
                {
                    dctProfile[profile.AccountID] = profile;
                }
                else
                {
                    dctProfile.Add(profile.AccountID, profile);
                }
            }

        }

        public static void RemoveProfileFromCache(UserProfile profile)
        {
            if (profile == null)
            {
                return;
            }
            else
            {
                if (dctProfile.ContainsKey(profile.AccountID))
                {
                    dctProfile.Remove(profile.AccountID);
                }
            }
        }

        public static UserProfile GetProfile(int empID)
        {
            if (dctProfile.ContainsKey(empID))
            {
                return dctProfile[empID];
            }
            else
            {
                return null;
            }
        }
    }
}
