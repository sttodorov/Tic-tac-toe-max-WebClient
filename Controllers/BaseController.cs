using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TicTacToeMax.WebClient.Controllers
{
    public class BaseController : Controller
    {
        public HttpClient Requester { get; set; }

        protected string BaseUri = "http://localhost:1234/";

        public BaseController()
        {
            this.Requester = new HttpClient();
        }

        protected void SetDefaultHeaderFromSession()
        {
            string username = GetValueFromSession("username");
            if (string.IsNullOrEmpty(username))
                return;

            string token = GetValueFromSession(username);
            if (!Requester.DefaultRequestHeaders.Contains("Authorization"))
                Requester.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        protected string GetValueFromSession(string key)
        {
            var valueArray = new byte[200];
            if (HttpContext.Session.TryGetValue(key, out valueArray))
            {
                return Encoding.ASCII.GetString(valueArray);
            }
            return null;
        }

        ~BaseController()
        {
            this.Requester.Dispose();
        }
    }
}
