using System.Collections.Generic;

namespace GithubUsers.Models
{
    public class GithubUser : SearchStatus
    {
        public string UserName { get; set; }
        public string Avatar_url { get; set; }
        public string Location { get; set; }
        public List<GithubUserRepos> Repos { get; set; }
    }

    public class GithubUserRepos
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int Stargazers_count { get; set; }
    }
}
