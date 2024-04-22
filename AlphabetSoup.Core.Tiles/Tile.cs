using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace AlphabetSoup.Core.Tiles
{
    public class Tile
    {
        public string Text { get; }
        public int BoardIndex { get; }

        private HashSet<Tile> _connectedTiles = new HashSet<Tile>();

        public Tile(string text, int boardIndex)
        {
            Text = text;
            BoardIndex = boardIndex;
        }

        public void ConnectTile(Tile other)
        {
            _connectedTiles.Add(other);
            other._connectedTiles.Add(this);
        }

        public HashSet<Tile> ConnectedTiles()
        {
            return new HashSet<Tile>(_connectedTiles);
        }
    }
}
