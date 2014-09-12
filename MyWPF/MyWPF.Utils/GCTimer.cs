using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace MyWPF.Utils
{
    public class GCTimer: IDisposable
    {
        private DispatcherTimer _timer;

        public GCTimer()
        {
            _timer = new DispatcherTimer {Interval = TimeSpan.FromMinutes(10)};
            //默认10分钟回收一次
        }

        public TimeSpan Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        public void Start()
        {
            _timer.Start();
            _timer.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            this.Collect();
        }

        public void Collect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Tick -= OnTick;
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= OnTick;
            _timer = null;
        }
    }
}
