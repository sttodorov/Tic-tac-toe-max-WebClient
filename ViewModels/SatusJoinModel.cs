using System.Collections.Generic;
using TicTacToeMax.WebClient.DTOs;

namespace TicTacToeMax.WebClient.ViewModels
{
    public class SatusJoinModel
    {
        public StatusResponse Status { get; set; }

        public IEnumerable<JoinResponse> Games { get; set; }
    }
}
