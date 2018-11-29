using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class Watch
    {
        public event TimePassed OnTimePassed;

        private double WatchedTimeElapsed { get; set; }

        private int _minutes;
        public int Minutes
        {
            get
            {
                return _minutes;
            }
        }

        private int _seconds;
        public int Seconds
        {
            get
            {
                return _seconds;
            }
        }

        private int _milliSeconds;
        public int MilliSeconds
        {
            get
            {
                return _milliSeconds;
            }
        }

        public int TotalMilliSeconds
        {
            get
            {
                return this.MilliSeconds + this.Seconds * 1000 + this.Minutes * 60 * 1000;
            }
        }

        public void Start()
        {
            this.WatchedTimeElapsed = 0;
            Clock.OnTimeElapsedChanged += Clock_OnTimeElapsedChanged;
        }
        public void Stop()
        {
            Clock.OnTimeElapsedChanged -= Clock_OnTimeElapsedChanged;
        }

        private void Clock_OnTimeElapsedChanged(double totalTime, double deltaTime)
        {
            this.WatchedTimeElapsed += deltaTime;
            double rest = this.WatchedTimeElapsed;
            if (rest >= 60000)
            {
                _minutes = (int)(rest / 60000);
                rest = rest % 60000;
            }

            if (rest >= 1000)
            {
                _seconds = (int)(rest / 1000);
                rest = rest % 1000;
            }
            _milliSeconds = (int)rest;

            if (this.OnTimePassed != null)
            {
               this.OnTimePassed(this);
            }
        }
    }

    public delegate void TimePassed(Watch w);
}
