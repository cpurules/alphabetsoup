using AlphabetSoup.Core.Tiles;

namespace AlphabetSoup.Core.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dice = new List<List<string>>
            {
                new List<string> { "A", "A", "E", "E", "G", "N" },
                new List<string> { "A", "B", "B", "J", "O", "O" },
                new List<string> { "A", "C", "H", "O", "P", "S" },
                new List<string> { "A", "F" ,"F", "K", "P", "S" },
                new List<string> { "A", "O", "O", "T", "T", "W" },
                new List<string> { "C", "I", "M", "O", "T", "U" },
                new List<string> { "D", "E", "I", "R", "L", "X" },
                new List<string> { "D", "E", "L", "R", "V", "Y" },
                new List<string> { "D", "I", "S", "T", "T", "Y" },
                new List<string> { "E", "E", "G", "H", "N", "W" },
                new List<string> { "E", "E", "I", "N", "S", "U" },
                new List<string> { "E", "H", "R", "T", "V", "W" },
                new List<string> { "E", "I", "O", "S", "S", "T" },
                new List<string> { "E", "L", "R", "T", "T", "Y" },
                new List<string> { "H", "I", "M", "N", "U", "QU" },
                new List<string> { "H", "L", "N", "N", "R", "Z" }
            }.Select(x => new DiceTileRoller(x));


            var board = new Board(dice);

            //Console.Write(board.ToString());

            var g = new Game();
            g.Start();
            return;

            while (true)
            {
                var input = Console.ReadLine().Trim().ToUpper();
                if (input.Equals(""))
                    break;

                Console.WriteLine($"{input} - {board.ContainsText(input)}");
            }

            return;

            var tiles = new List<string> {  "P", "L", "G", "M",
                                            "B", "T", "I", "L",
                                            "R", "N", "A", "U",
                                            "E", "E", "R", "Y"
            }.Select((face, idx) => new Tile(face, idx));

            var toTest = new List<string>
            {
                "NEAR",
                "RANT",
                "GLARE",
                "YARN",
                "TAIL",
                "LANE",
                "URN",
                "MINE"
            };
            foreach (var x in toTest) Console.WriteLine($"{x} - {board.ContainsText(x)}");

            var toFail = new List<string>
            {
                "RARE", // re-uses R in middle of word
                "BRUIN", // R->U crosses sides of the board
                "GRAIN", // G->R crosses sides of the board
                "GILL", // re-uses same tile
                "PNMR" // nonsense random tiles
            };
            foreach (var x in toFail) Console.WriteLine($"{x} - {board.ContainsText(x)}");
        }
    }
}
