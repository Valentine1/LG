using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Common
{
    public class TimeRange
    {
        private TimeSpan _span;
        public TimeSpan Span
        {
            get
            {
                return _span;
            }
            set
            {
                _span = value;
            }
        }

        int rest = 0;
        int hours = 0;
        int minutes = 0;
        int seconds = 0;
        int millisecs = 0;

        public TimeRange(Int64 ms)
        {
            MakeSpan(ms);
            _span = new TimeSpan(0, hours, minutes, seconds, millisecs);
        }

        private void MakeSpan(Int64 ms)
        {
            if (ms >= 3600000)
            {
                hours = (int)(ms / 3600000);
                rest =(int)(ms % 3600000);
                MakeSpan(rest);
            }
            else if (ms >= 60000)
            {
                minutes = (int)(ms / 60000);
                rest = (int)(ms % 60000);
                MakeSpan(rest);
            }
            else if (ms >= 1000)
            {
                seconds = (int)(ms / 1000);
                millisecs = (int)(ms % 1000);
            }
        }
    }
}
