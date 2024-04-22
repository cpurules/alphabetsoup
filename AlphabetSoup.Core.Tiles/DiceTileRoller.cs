using AlphabetSoup.Core.Tiles.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphabetSoup.Core.Tiles
{
    public class DiceTileRoller : ITileRoller
    {

        private IEnumerable<string> _faces;

        public DiceTileRoller(IEnumerable<string> faces)
        {
            _faces = faces;
        }

        public string Roll()
        {
            return _faces.GetRandom();
        }
    }
}
