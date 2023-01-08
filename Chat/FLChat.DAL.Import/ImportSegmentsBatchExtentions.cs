using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import
{
    public static class ImportSegmentsBatchExtentions
    {
        public static ImportSegmentsBatch.Result ImportSegmentsBatch(
            this ChatEntities entities, 
            IImportSource<Tuple<int, IEnumerable<string>>> users,
            Dictionary<string, Guid> segmentNames) {

            using (ImportSegmentsBatch import = new ImportSegmentsBatch(segmentNames)) {
                foreach (var item in users) {
                    import.Add(item.Item1, item.Item2);
                }
                return import.Execute(entities);
            }
        }
    }
}
