using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;

namespace LG.ViewModels
{
    public class WatchVM : BaseNotify
    {
        private string _hours;
        public string Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = value;
                this.NotifyPropertyChanged("Hours");
            }
        }

        private string _minutes;
        public string Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                _minutes = value;
                this.NotifyPropertyChanged("Minutes");
            }
        }

        private string _seconds;
        public string Seconds
        {
            get
            {
                return _seconds;
            }
            set
            {
                _seconds = value;
                this.NotifyPropertyChanged("Seconds");
            }
        }

        private string _milliSeconds;
        public string MilliSeconds
        {
            get
            {
                return _milliSeconds;
            }
            set
            {
                _milliSeconds = value;
                this.NotifyPropertyChanged("MilliSeconds");
            }
        }
        public WatchVM()
        {
        }
        public WatchVM(TimeRange tr)
        {
            if (tr != null)
            {
                this.Hours = tr.Span.Hours.ToString();
                this.Minutes = tr.Span.Minutes.ToString("D2");
                this.Seconds = tr.Span.Seconds.ToString("D2");
                this.MilliSeconds = tr.Span.Milliseconds.ToString("D3");
            }
        }
        public void ReInit(TimeRange tr)
        {
            if (tr != null)
            {
                this.Hours = tr.Span.Hours.ToString();
                this.Minutes = tr.Span.Minutes.ToString("D2");
                this.Seconds = tr.Span.Seconds.ToString("D2");
                this.MilliSeconds = tr.Span.Milliseconds.ToString("D3");
            }
        }

    }
}
