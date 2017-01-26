using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using TicTacToeMax.WebClient.DTOs;
using System.Text;
using TicTacToeMax.WebClient.ViewModels;

namespace TicTacToeMax.WebClient.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("/home")]
        public async Task<IActionResult> Home()
        {
            SetDefaultHeaderFromSession();
            var statusResponse = await Requester.GetAsync(BaseUri + "status");
            var status = JsonConvert.DeserializeObject<StatusResponse>(await statusResponse.Content.ReadAsStringAsync());

            var joinResponse = await Requester.GetAsync(BaseUri + "join");
            var games = JsonConvert.DeserializeObject<JoinResponse[]>(await joinResponse.Content.ReadAsStringAsync());

            ViewBag.Username = GetValueFromSession("username");
            return View(new SatusJoinModel() { Status = status, Games = games });
        }

        [HttpPost]
        [Route("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register(RegisterViewModel data)
        {
            if (string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Password))
            {
                return View("Index", data);
            }

            var values = new Dictionary<string, string>
                {
                   { "username", data.Email },
                   { "password", data.Password },
                   { "confirmPassword", data.Password }
                };

            var content = new FormUrlEncodedContent(values);
            var registerResponse = await Requester.PostAsync(BaseUri + "register", content);

            // TODO: Add error data
            if (registerResponse.StatusCode != System.Net.HttpStatusCode.Created)
            {
                return View("Index", data);
            }
            if (!await SendLoginRequest(data))
            {
                return View("Index", data);
            }

            return RedirectToAction("Home", new { username = data.Email });
        }

        private async Task<bool> SendLoginRequest(RegisterViewModel data)
        {
            var values = new Dictionary<string, string>
                {
                   { "username", data.Email },
                   { "password", data.Password }
                };

            var content = new FormUrlEncodedContent(values);
            var loginResponse = await Requester.PostAsync(BaseUri + "login", content);
            if (loginResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            var token = JsonConvert.DeserializeObject<LoginResponse>(await loginResponse.Content.ReadAsStringAsync()).Token;
            HttpContext.Session.Set("username", Encoding.ASCII.GetBytes(data.Email));
            HttpContext.Session.Set(data.Email, Encoding.ASCII.GetBytes(token));

            return true;
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(RegisterViewModel data)
        {
            // TODO: Add error data.
            if (string.IsNullOrWhiteSpace(data.Email) || string.IsNullOrWhiteSpace(data.Password))
            {
                return View("Index", data);
            }

            if (!await SendLoginRequest(data))
            {
                return View("Index", data);
            }

            return RedirectToAction("Home", new { username = data.Email });
        }

        [HttpPost]
        [Route("/fblogin")]
        public async Task<ActionResult> LoginFb(string fbApiToken)
        {
            if (string.IsNullOrWhiteSpace(fbApiToken))
            {
                return View("Index");
            }

            var values = new Dictionary<string, string>
            {
                { "token", fbApiToken}
            };

            var loginResponse = await Requester.PostAsync(BaseUri + "fblogin", new FormUrlEncodedContent(values));
            if (loginResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return View("Index");
            }

            var resp = JsonConvert.DeserializeObject<LoginResponse>(await loginResponse.Content.ReadAsStringAsync());
            HttpContext.Session.Set("username", Encoding.ASCII.GetBytes(resp.Username));
            HttpContext.Session.Set(resp.Username, Encoding.ASCII.GetBytes(resp.Token));

            return RedirectToAction("Home", new { username = resp.Username });
        }
    }
}
