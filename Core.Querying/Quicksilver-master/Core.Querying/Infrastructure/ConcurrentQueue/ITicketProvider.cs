using System;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 4014

namespace Core.Querying.Infrastructure.ConcurrentQueue
{
    public interface ITicketProvider
    {
        /// <summary>
        ///     Asynchronously waits for an available ticket, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <returns></returns>
        Task WaitAsync(CancellationToken cancellationToken);
    }

    internal class SelfDestructingTicketProvider : ITicketProvider
    {
        private readonly TimeSpan _ticketLifetime;
        private readonly SemaphoreSlim _ticketPool;

        public SelfDestructingTicketProvider(int maxTicketCount, TimeSpan ticketLifetime)
        {
            _ticketLifetime = ticketLifetime;
            _ticketPool = new SemaphoreSlim(maxTicketCount, maxTicketCount);
        }

        public async Task WaitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _ticketPool.WaitAsync(cancellationToken).ConfigureAwait(false);

            // TODO improve: let ASP.NET runtime be aware of outstanding works
            Task.Factory.StartNew(ReturnTicket).Unwrap();
        }

        private async Task ReturnTicket()
        {
            await Task.Delay(_ticketLifetime).ConfigureAwait(false);
            _ticketPool.Release();
        }
    }
}