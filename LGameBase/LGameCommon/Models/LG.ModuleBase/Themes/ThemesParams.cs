using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public static class ThemesParams
    {
        public static double LegendWidth { get; set; }

        public static double LegendHeight { get; set; }
        public static double LegendWidthFor1920 { get; set; }
        public static double LegendWidthFor1024 { get; set; }

        public static double AvatarScale { get; set; }
        public static double AvatarBorderWidth { get; set; }
        public static double AvatarBorderHeight { get; set; }
        public static double AvatarHeightToWidthRatio { get; set; }

        public static double AvatarOffsetY { get; set; }
        public static double AvatarOffsetYFor1920 { get; set; }
        public static double AvatarOffsetXFor1024 { get; set; }
        public static double AvatarOffsetX { get; set; }
        public static double AvatarOffsetXFor1920 { get; set; }
        public static double AvatarOffsetYFor1024 { get; set; }


        public static void Initialize()
        {
            ThemesParams.LegendWidthFor1920 = 480;//356;
            ThemesParams.LegendWidthFor1024 = 190;
            ThemesParams.AvatarHeightToWidthRatio = 1.74;

            ThemesParams.AvatarOffsetYFor1920 = 83;
            ThemesParams.AvatarOffsetXFor1920 = 25;

            ThemesParams.AvatarOffsetXFor1024 = 12;
            ThemesParams.AvatarOffsetYFor1024 = 50;
        }
    }
}
