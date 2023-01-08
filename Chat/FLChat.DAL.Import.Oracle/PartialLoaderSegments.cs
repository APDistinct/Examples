using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Oracle
{
    public class PartialLoaderSegments : PartialLoader<Tuple<int, IEnumerable<string>>>
    {
        private Dictionary<string, Guid> _segments;
        public int Inserted { get; private set; } = 0;
        public int Deleted { get; private set; } = 0;

        public PartialLoaderSegments(string connString, int consNumber) 
            : base(new SegmentSource(), connString, consNumber) {
        }

        public event EventHandler<List<string>> OnNewSegments;

        protected override void BeforeFirstImport(SourceDT<Tuple<int, IEnumerable<string>>> source) {
            using (ChatEntities entities = new ChatEntities()) {
                _segments = ImportSegmentsBatch.LoadSegmentsIds(
                    entities, 
                    (source as SegmentSource).SegmentNames, 
                    out List<string> inserted);
                if (inserted != null)
                    OnNewSegments?.Invoke(this, inserted);
                
            }
        }

        protected override void Import(SourceDT<Tuple<int, IEnumerable<string>>> source) {
            using (ChatEntities entities = new ChatEntities()) {
                var result = entities.ImportSegmentsBatch(source, _segments);
                Inserted += result.Inserted;
                Deleted += result.Deleted;
            }
        }
    }
}
