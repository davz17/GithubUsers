using System;

namespace GithubUsers.Models
{
    public class GithubUserClientResponse : GithubResponseStatus
    {
        public string Login { get; set; }
        public string Avatar_url { get; set; }
        public string Location { get; set; }
        public string Repos_url { get; set; }
    }
}
    