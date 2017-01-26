using System.Collections.Generic;
using TicTacToeMax.WebClient.DTOs;

namespace TicTacToeMax.WebClient.ViewModels
{
    public class Game
    {
        public string _Id { get; set; }

        public Dictionary<int, Dictionary<int, SmallBoard>> Board { get; set; }

        public string GameResult { get; set; }

        public int CurrentPlayingBoardRow { get; set; }

        public int CurrentPlayingBoardCol { get; set; }

        public string CurrentPlayerSymbol { get; set; }

        public Dictionary<string, JoinResponse> Users { get; set; }
    }
}
