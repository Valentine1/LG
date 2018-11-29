using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public static class Clock
    {
        public static event TimeElapsedChanged OnTimeElapsedChanged;
        public static double TimeElapsed { get; set; }

        public static void TimeElapses(double deltaTime)
        {
            Clock.TimeElapsed += deltaTime;
            if (Clock.OnTimeElapsedChanged != null)
            {
                Clock.OnTimeElapsedChanged(Clock.TimeElapsed, deltaTime);
            }
        }
    }

    public delegate void TimeElapsedChanged(double totalTime, double deltaTime);
}
