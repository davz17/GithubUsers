using GithubUsers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GithubUsers.Services;

namespace GithubUsers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGithubService _githubService;

        public HomeController(ILogger<HomeController> logger, IGithubService githubService)
        {
            _logger = logger;
            _githubService = githubService;
        }

        public IActionResult Index(string userName)
        {
            if (!Request.QueryString.HasValue)
            {
                return View();
            }

            if (string.IsNullOrEmpty(userName))
            {
                return View(new GithubUser()
                {
                    RequestedString = string.Empty
                });
            }

            //get github user details
            var githubUser = _githubService.GetUserAsync(userName).Result;
            if (!string.IsNullOrEmpty(githubUser.ErrorMessage))
            {
                return View(new GithubUser()
                {
                    Error = githubUser.ErrorMessage
                });
            }
                
            //get github repos
            var githubUserRepos = _githubService.GetRepos(userName).Result;
            
            //take 5 repos with the highest stargazer_count
            var topFiveRepos = githubUserRepos.Repos.OrderByDescending(x => x.Stargazers_count)
                                                    .Take(5)
                                                    .ToList();

            var repos = new List<GithubUserRepos>();
            topFiveRepos.ForEach(item =>
            {
                repos.Add(new GithubUserRepos()
                {
                    Name = item.Name,
                    Url = item.Html_url,
                    Description = item.Description,
                    Stargazers_count = item.Stargazers_count
                });
            });

            var model = new GithubUser()
            {
                UserName = githubUser.Login,
                Location = githubUser.Location,
                Avatar_url = githubUser.Avatar_url,
                Repos = repos
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
