using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public interface ISendingConveyor
    {
        void Send(CancellationToken ct);
    }

    public static class ISendingConveyorExtentions
    {
        /// <summary>
        /// Perform endlessly sending
        /// </summary>
        /// <param name="conv">Conveyor</param>
        /// <param name="delay">Delay time between two consecutive call Send method</param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="onException">Call when when conveyor throws exception</param>
        public static void SendEndlessly(this ISendingConveyor conv, TimeSpan delay, CancellationToken ct, EventHandler<Exception> onException) {
            while (true) {
                if (ct.IsCancellationRequested)
                    return;
                try {
                    conv.Send(ct);
                } catch (OperationCanceledException) {
                    return;
                } catch (Exception e) {
                    onException(conv, e);
                }
                Task.Delay(delay).Wait();
            }
        }
    }
}
