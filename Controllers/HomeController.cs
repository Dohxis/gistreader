using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace GistReader.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
            return View();
        }

        public async Task<object> Search(string username) {
            try {
                var client = new GitHubClient(new ProductHeaderValue("GistReader"));
                var user = await client.User.Get(username);
                var gists = await client.Gist.GetAllForUser(username);
                
                ViewData["Username"] = "@" + username;
                ViewData["FullName"] = user.Name;
                ViewData["Avatar"] = user.AvatarUrl;
                ViewData["Gists"] = gists.ToList();

                //foreach(var g in gists)
                //if(g.Description.Equals("") ) Console.WriteLine("'yeah " + g.Description.GetType() + "'");

                return View();

            } catch(Exception e) {
                TempData["error"] = e.Message;
                return e.Message;
            }
        }

        public async Task<object> Read(string id) {
            try {

                var client = new GitHubClient(new ProductHeaderValue("GistReader"));
                var gists = await client.Gist.Get(id);

                ViewData["Gist"] = gists;
                ViewData["Files"] = gists.Files.ToArray();

                foreach(var g in gists.Files.ToArray()){
                    Console.WriteLine(g.Value.Content);
                }

            } catch (Exception e) {
                return e.Message;
            }
            return View();
        }

    }
}
