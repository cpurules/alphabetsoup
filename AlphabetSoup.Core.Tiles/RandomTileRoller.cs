using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphabetSoup.Core.Tiles
{
    public class RandomTileRoller : ITileRoller
    {
        public string Roll()
        {
            var s = Convert.ToChar(new Random().NextInt64(65, 91)).ToString();
            return s.Equals("Q") ? "QU" : s;
        }
    }
}
