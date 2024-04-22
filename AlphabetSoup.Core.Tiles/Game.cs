using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphabetSoup.Core.Tiles
{
    public class Game
    {
        public List<Player> Players { get; } = new List<Player>();
        private Board _board;
        private bool _started = false;
        private bool _ended = false;

        public Game()
        {

        }

        public void AddPlayer(Player player) => this.Players.Add(player);

        public void Start()
        {
            _board = new Board(new RandomTileRoller(), 16);
            _started = true;

            Console.WriteLine(_board.ToString());
        }
    }
}
