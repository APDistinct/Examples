using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Oracle
{
    public abstract class PartialLoader<T> : IDisposable where T : class {
        private OracleConnection _conn = null;
        private bool _sourceOwner;

        public PartialLoader(SourceDT<T> source, string connString, int consNumber, bool sourceOwner = true) {
            Source = source;
            ConnString = connString;
            ConsNumber = consNumber;
            _sourceOwner = sourceOwner;
        }

        public SourceDT<T> Source { get; }
        public string ConnString { get; }
        public int ConsNumber { get; }
        public int RecordsLimit { get; set; } = 1000;

        public class LoadingArg
        {
            public int Iteration { get; set; }
        }

        public class LoadExceptionArg {
            public int TryCount { get; set; }
            public Exception Exception { get; set; }
        }

        public class LoadCompleteArg
        {
            public int Iteration { get; set; }
            public int RowCount { get; set; }
        }

        public event EventHandler<LoadingArg> OnLoading;
        public event EventHandler<LoadExceptionArg> OnLoadError;
        public event EventHandler<LoadCompleteArg> OnLoadComplete;

        public void Load() {
            bool exhausted = false;
            int iteration = 0;
            while (exhausted == false) {
                //log.Info($"loading from partner: {((iteration * RecordsLimit) + 1).ToString()}-{((iteration + 1) * RecordsLimit).ToString()} records");
                OnLoading?.Invoke(this, new LoadingArg() { Iteration = iteration });

                Source.Load(
                    ConnString, ref _conn,
                    ConsNumber, (iteration * RecordsLimit) + 1, (iteration + 1) * RecordsLimit,
                        OnLoadException);

                OnLoadComplete?.Invoke(this, new LoadCompleteArg() { Iteration = iteration, RowCount = Source.RowCount });
                //log.Info($"row count: {source.RowCount.ToString()}");

                if (Source.RowCount > 0) {
                    if (iteration == 0)
                        BeforeFirstImport(Source);
                    Import(Source);
                }

                exhausted = Source.RowCount < RecordsLimit;
                iteration += 1;
            }
            Complete(Source);
        }

        protected abstract void Import(SourceDT<T> source);

        protected virtual void BeforeFirstImport(SourceDT<T> source) { }

        protected virtual void Complete(SourceDT<T> source) { }

        private bool OnLoadException(OracleException e, int tryCount) {
            //log.Error($"#{tryCount.ToString()} loading error: {e.Message}");
            OnLoadError?.Invoke(this, new LoadExceptionArg() { Exception = e, TryCount = tryCount });
            return true;
        }

        public virtual void Dispose() {
            _conn?.Dispose();
            if (_sourceOwner)
                Source.Dispose();
        }
    }
}
