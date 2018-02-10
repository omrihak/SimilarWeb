using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWebAPI.Models
{
    public class Message : ICachable
    {
        public string UserName { get; set; }
        public string MessageText { get; set; }
        public DateTime DateTime { get; set; }

        public Message(string userName, string messageText, DateTime dateTime)
        {
            this.UserName = userName;
            this.MessageText = messageText;
            this.DateTime = dateTime;
        }

        public DateTime GetDateTime()
        {
            return DateTime;
        }
    }
}
