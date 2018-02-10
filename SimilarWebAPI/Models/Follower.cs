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
        public DateTime CreationDateTime { get; set; }

        public Follower(string followingUserName, string followedUserName, DateTime dateTime)
        {
            FollowingUserName = followingUserName;
            FollowedUserName = followedUserName;
            CreationDateTime = dateTime;
        }

        public DateTime GetDateTime()
        {
            return CreationDateTime;
        }
    }
}
