using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicTacToeMax.WebClient.ViewModels;
using System.Net.Http;
using Newtonsoft.Json;

namespace TicTacToeMax.WebClient.Controllers
{
    public class GameController : BaseController
    {
        public async Task<IActionResult> Join(string id)
        {
            SetDefaultHeaderFromSession();

            var values = new Dictionary<string, string>
            {
                { "gameId", id}
            };
            var content = new FormUrlEncodedContent(values);
            var joinResponse = await Requester.PostAsync(BaseUri + "join", content);
            var gameAsJson = await joinResponse.Content.ReadAsStringAsync();
            return RedirectToAction("Board", new { gameAsJson = gameAsJson });
        }


        public IActionResult Board(string gameAsJson)
        {
            var game = JsonConvert.DeserializeObject<Game>(gameAsJson);
            if (!game.Users.ContainsKey("2"))
                game.Users.Add("2", new DTOs.JoinResponse() { Username = "No opponent yet"});

            // TODO: Implement Web Soket connction to play on board
            
            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            SetDefaultHeaderFromSession();
            var createResponse = await Requester.PostAsync(BaseUri + "create", new FormUrlEncodedContent(new Dictionary<string, string>()));
            var gameStr = await createResponse.Content.ReadAsStringAsync();

            return RedirectToAction("Board", new { gameJson = gameStr });
        }
    }
}
