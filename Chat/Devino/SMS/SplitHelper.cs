using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devino.SMS
{
    public static class SplitHelper
    {
        public static List<List<string>> SplitMessages(List<string> source, int count)
        {
            var size = source.Count / count;
            var result = source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            return result;
        }
    }
}
