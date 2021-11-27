using Domain.Enums;
using System.Collections.Generic;

namespace Infrastructure.Common.Models
{
    public class EmailSettings
    {
        public string host { get; set; }
        public int port { get; set; }
        public bool useSsl { get; set; }
        public List<EmailInfo> Emails { get; set; }
    }

    public class EmailInfo
    {
        public string name { get; set; }
        public FromEmail from { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}