using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AlphabetSoup.Core.Tiles.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var r = new Random();
            List<T> shuffled = source.ToList();
            int n = shuffled.Count;
            while (n > 1)
            {
                n--;
                int k = r.Next(n + 1);
                T val = shuffled[k];
                shuffled[k] = shuffled[n];
                shuffled[n] = val;
            }
            return shuffled;
        }

        public static T GetRandom<T>(this IEnumerable<T> source)
        {
            var r = new Random();
            var l = source.ToList();
            return l[r.Next(l.Count)];
        }

        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }
    }
}
