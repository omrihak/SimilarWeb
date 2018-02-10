using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWebAPI.Models
{
    public class Follower : ICachable
    {
        public string FollowingUserName { get; set;  }
        public string FollowedUserName { get; set; }
        public DateTime DateTime { get; set; }

        public Follower(string followingUserName, string followedUserName, DateTime dateTime)
        {
            FollowingUserName = followingUserName;
            FollowedUserName = followedUserName;
            DateTime = dateTime;
        }

        public DateTime GetDateTime()
        {
            return DateTime;
        }
    }
}
