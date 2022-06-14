using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GithubUsers.Models;
using GithubUsers.Settings;

namespace GithubUsers.Services
{
    public interface IGithubService
    {
        Task<GithubUserClientResponse> GetUserAsync(string userName);
        Task<GithubUserClientRepoResponse> GetRepos(string userName);
    }

    public class GithubService : IGithubService
    {
        private GithubConfiguration _githubConfiguration;

        public GithubService(IOptions<GithubConfiguration> githubConfiguration) 
        {
            _githubConfiguration = githubConfiguration.Value;
        }

        public async Task<GithubUserClientResponse> GetUserAsync(string userName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");//Set the User Agent to "request"

                    var response = await client.GetAsync($"{_githubConfiguration.UserLink}{userName}");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<GithubUserClientResponse>(responseBody);

                        return responseJson;
                    }
                    else
                    {
                        return new GithubUserClientResponse()
                        {
                            ErrorMessage = $"{userName} {response.ReasonPhrase}"
                        };
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return new GithubUserClientResponse()
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<GithubUserClientRepoResponse> GetRepos(string userName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");//Set the User Agent to "request"

                    var response = await client.GetAsync($"{_githubConfiguration.UserLink}{userName}/repos");

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseJson = JsonConvert.DeserializeObject<List<GithubUserClientRepo>>(responseBody);

                        return new GithubUserClientRepoResponse()
                        {
                            Repos = responseJson
                        };
                    }
                    else
                    {
                        return new GithubUserClientRepoResponse()
                        {
                            ErrorMessage = $"{response.ReasonPhrase}"
                        };
                    }

                    
                }
            }
            catch (Exception ex)
            {
                return new GithubUserClientRepoResponse()
                {
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
