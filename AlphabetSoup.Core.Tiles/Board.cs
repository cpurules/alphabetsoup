using AlphabetSoup.Core.Tiles.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphabetSoup.Core.Tiles
{
    public class Board
    {
        private HashSet<Tile> _tiles;
        public int SquareSize { get { return (int)Math.Sqrt(_tiles.Count); } }

        public Board(IEnumerable<Tile> tiles)
        {
            if (Math.Sqrt(tiles.Count()) % 1 > 0)
            {
                throw new ArgumentException("length is not valid for square board", "tiles");
            }

            _tiles = new HashSet<Tile>(tiles);

            ConnectTiles();
        }

        public Board(ITileRoller roller, int count)
            : this(Enumerable.Repeat(roller, count), count) { }

        public Board(IEnumerable<ITileRoller> rollers)
            : this(rollers.Select((roller, idx) => new Tile(roller.Roll(), idx))) { }

        public Board(IEnumerable<ITileRoller> rollers, int count)
        {
            if (Math.Sqrt(count) % 1 > 0)
                throw new ArgumentException("count is not valid for square board", "count");

            _tiles = new HashSet<Tile>();
            for (int i = 0; i < count; i++) _tiles.Add(new Tile(rollers.GetRandom().Roll(), i));

            ConnectTiles();
        }

        private void ConnectTiles()
        {
            // We work with the tiles in a List since we're using the indexes in a lot of places here
            var tiles = new List<Tile>(_tiles.OrderBy(tile => tile.BoardIndex));

            // We can connect to the tile above if we are not on the top row
            // Tile above is index - SquareSize
            foreach (var tile in tiles.Skip(SquareSize))
                tile.ConnectTile(tiles[tile.BoardIndex - SquareSize]);

            // We can connect to the tile below if we are not in the last row
            foreach (var tile in tiles.SkipLast(SquareSize))
                tile.ConnectTile(tiles[tile.BoardIndex + SquareSize]);

            // We can connect to the tile left if we are not in the first column
            // First column has indices mod SquareSize = 0
            // Tile left is index - 1
            foreach (var tile in tiles.Where(tile => tile.BoardIndex % SquareSize > 0))
                tile.ConnectTile(tiles[tile.BoardIndex - 1]);

            // We can connect to the tile right if we are not in the last column
            // Last column has indices + 1 mod SquareSize = 0
            // Tile right is index + 1
            foreach (var tile in tiles.Where(tile => (tile.BoardIndex + 1) % SquareSize > 0))
                tile.ConnectTile(tiles[tile.BoardIndex + 1]);

            // We can connect to the tile above-right if we are not in the first row AND we are not in the last column
            // Tile above-right is index - SquareSize + 1
            foreach (var tile in tiles.Skip(SquareSize)
                                        .Where(tile => (tile.BoardIndex + 1) % SquareSize > 0))
                tile.ConnectTile(tiles[tile.BoardIndex - SquareSize + 1]);

            // We can connect to the tile below-right if we are not in the last row AND we are not in the last column
            // Tile below-right is index + SquareSize + 1
            foreach (var tile in tiles.SkipLast(SquareSize)
                                        .Where(tile => (tile.BoardIndex + 1) % SquareSize > 0))
                tile.ConnectTile(tiles[tile.BoardIndex + SquareSize + 1]);

            // We can connect to the tile below-left if we are not in the last row AND we are not in the first column
            // Tile below-left is index + SquareSize - 1
            foreach (var tile in tiles.SkipLast(SquareSize)
                                        .Where(tile => tile.BoardIndex % SquareSize > 0))
                tile.ConnectTile(tiles[tile.BoardIndex + SquareSize - 1]);

            // Finally..
            // We can connect to the tile above-left if we are not in the first row AND we are not in the first column
            // Tile above-left is index - SquareSize - 1
            foreach (var tile in tiles.Skip(SquareSize)
                                        .Where(tile => tile.BoardIndex % SquareSize > 0))
                tile.ConnectTile(tiles[tile.BoardIndex - SquareSize - 1]);
        }

        public bool ContainsText(string text)
        {
            var startTiles = _tiles.Where(tile => text.StartsWith(tile.Text));
            return startTiles.Any(tile => ContainsTextStartingAt(text, tile));
        }

        private bool ContainsTextStartingAt(string text, Tile startTile)
        {
            return ContainsTextStartingAt(text, startTile, new HashSet<Tile>());
        }

        private bool ContainsTextStartingAt(string text, Tile startTile, HashSet<Tile> visitedTiles)
        {
            if (!text.StartsWith(startTile.Text)) return false;

            var textRest = text.Substring(startTile.Text.Length);
            if (textRest.Length == 0) return true; // we're done

            var nextTiles = startTile.ConnectedTiles().Where(tile => !visitedTiles.Contains(tile) && textRest.StartsWith(tile.Text));
            if (nextTiles.Count() == 0) return false;

            var newVisited = new HashSet<Tile>(visitedTiles)
            {
                startTile
            };
            return nextTiles.Any(tile => ContainsTextStartingAt(textRest, tile, newVisited));
        }

        public override string ToString()
        {
            var board = new StringBuilder();
            var orderedTiles = _tiles.OrderBy(tile => tile.BoardIndex);

            // minimum of 3 spaces per tile, more if a tile has more letters (shouldn't happen)
            var tileWidth = Math.Max(3, _tiles.Max(x => x.Text.Length));

            // top border
            // + on both sides
            // SquareSize x tileWidth ---
            // plus an additional - between each tile (so SquareSize - 1)
            board.AppendLine("+" + string.Concat(Enumerable.Repeat("-", (tileWidth + 1) * SquareSize - 1)) + "+");

            // rows of tiles
            // | on both sides
            // padded text in tile
            // plus an additional | between each tile
            for (int row = 0; row < SquareSize; row++)
            {
                var rowTiles = orderedTiles.Skip(row * SquareSize).Take(SquareSize);
                board.Append("|");
                foreach (var tile in rowTiles)
                {
                    var tileText = tile.Text;
                    var spaces = tileWidth - tileText.Length;
                    if (spaces % 2 == 0)
                    {
                        var space = spaces / 2;
                        var spaceText = string.Concat(Enumerable.Repeat(" ", space));
                        tileText = string.Concat(spaceText, tileText, spaceText);
                    }
                    else
                    {
                        var space1 = spaces / 2;
                        var space2 = spaces - space1;
                        tileText = string.Concat(
                            string.Concat(Enumerable.Repeat(" ", space1)),
                            tileText,
                            string.Concat(Enumerable.Repeat(" ", space2)));
                    }
                    board.Append(tileText);
                    board.Append("|"); // this also handles our final pipe
                }
                board.AppendLine();

                // border between rows, if not the final row
                if (row < SquareSize - 1)
                {
                    board.AppendLine("|" + string.Concat(Enumerable.Repeat("-", (tileWidth + 1) * SquareSize - 1)) + "|");
                }
            }

            // bottom border, same as top
            board.AppendLine("+" + string.Concat(Enumerable.Repeat("-", (tileWidth + 1) * SquareSize - 1)) + "+");

            return board.ToString();
        }
    }
}
