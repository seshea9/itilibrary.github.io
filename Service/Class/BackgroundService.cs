using System.Security.Cryptography.Xml;

namespace AutoRunSetTime.NewFolder
{
    public abstract class BackgroundService : IHostedService
    {
        private Task _exeucutingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private DateTime runAt = DateTime.Today + new TimeSpan(13, 33, 20);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _exeucutingTask = ExecuteAsync(_stoppingCts.Token);
            if (_exeucutingTask.IsCompleted)
            {
                return _exeucutingTask;
            }
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken token)
        {
            //if (DateTime.Now >= runAt)
            //{
            //    await Process();
            //}
            //else
            //{
                do
                {
                    var delay = runAt-DateTime.Now;
                    
                    await Task.Delay(20000, token);
                    await Process();
                } while (!token.IsCancellationRequested);
            //}
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if(_exeucutingTask == null)
            {
                return ;
            }
            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_exeucutingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
        protected abstract Task Process();
    }
}
