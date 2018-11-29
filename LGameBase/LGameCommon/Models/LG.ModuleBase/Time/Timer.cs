using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public class Timer
    {
        public event Ticked OnTicked;
        public event TickedStatefull OnTickedStatefull;
        private double Interval;
        private double TimeElapsed;
        private double _totalTimeElapsed;
        public double TotalTimeElapsed
        {
            get
            {
                return _totalTimeElapsed;
            }
        }
        private object State { get; set; }

        public Timer()
        {
        }
        public Timer(double interval)
        {
            this.Interval = interval;
        }
        public void Start()
        {
            this.TimeElapsed = 0;
            this._totalTimeElapsed = 0;
            Clock.OnTimeElapsedChanged += Clock_OnTimeElapsedChanged;
        }
        public void SetInterval(double interval)
        {
            this.Interval = interval;
        }
        public void Start(object state)
        {
            this.State = state;
            this.TimeElapsed = 0;
            this._totalTimeElapsed = 0;
            Clock.OnTimeElapsedChanged += Clock_OnTimeElapsedStatefullChanged;
        }
        public void Stop()
        {
            Clock.OnTimeElapsedChanged -= Clock_OnTimeElapsedChanged;
            Clock.OnTimeElapsedChanged -= Clock_OnTimeElapsedStatefullChanged;
        }
        private void Clock_OnTimeElapsedChanged(double totalTime, double deltaTime)
        {
            this.TimeElapsed = this.TimeElapsed + deltaTime;
            this._totalTimeElapsed = this._totalTimeElapsed + deltaTime;
            if (this.TimeElapsed >= this.Interval)
            {
                if (this.OnTicked != null)
                {
                    this.OnTicked();
                }
                double dif = this.TimeElapsed - this.Interval;
                this.TimeElapsed = dif;
                this._totalTimeElapsed += dif;
            }
        }

        private void Clock_OnTimeElapsedStatefullChanged(double totalTime, double deltaTime)
        {
            this.TimeElapsed = this.TimeElapsed + deltaTime;
            this._totalTimeElapsed = this._totalTimeElapsed + deltaTime;
            if (this.TimeElapsed >= this.Interval)
            {
                Clock.OnTimeElapsedChanged -= Clock_OnTimeElapsedStatefullChanged;
                if (this.OnTickedStatefull != null)
                {
                    this.OnTickedStatefull(this.State);
                }
                double dif = this.TimeElapsed - this.Interval;
                this.TimeElapsed = dif;
                this._totalTimeElapsed += dif;
            }
        }
    }

    public delegate void Ticked();
    public delegate void TickedStatefull(object state);
}
