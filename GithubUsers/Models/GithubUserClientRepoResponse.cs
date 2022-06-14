using System.Collections.Generic;

namespace GithubUsers.Models
{
    public class GithubUserClientRepoResponse : GithubResponseStatus
    {
        public List<GithubUserClientRepo> Repos { get; set; }
    }

    public class GithubUserClientRepo
    {
        public string Name { get; set; }
        public string Html_url { get; set; }
        public string Description { get; set; }
        public int Stargazers_count { get; set; }
    }
}
