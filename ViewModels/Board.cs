using System.Collections.Generic;

namespace TicTacToeMax.WebClient.ViewModels
{
    public class Board
    {
        public Dictionary<int, Dictionary<int, SmallBoard>> MyProperty { get; set; }

        public const string StillPlaying = "Still playing";

        public const string Draw = "Draw";

        public const string WonByO = "Won by O";

        public const string WonByX = "Won by X";
    }
}
