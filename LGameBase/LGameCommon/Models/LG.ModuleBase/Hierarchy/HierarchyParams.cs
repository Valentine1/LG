using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public static class HierarchyParams
    {
        public static double TopProfilesWidth1920 { get; set; }
        public static double TopProfilesMaxDif { get; set; }

        public static double PyramidRightSpace { get; set; }
        public static double PyramidTopSpace { get; set; }

        public static double PyramidStepWidth { get; set; }
        public static double PyramidStepHeight { get; set; }
        public static double PyramidAvatarHeight { get; set; }


        static HierarchyParams()
        {
            HierarchyParams.Initialize();
        }

        public static void Initialize()
        {
            HierarchyParams.TopProfilesWidth1920 = 602;
            HierarchyParams.TopProfilesMaxDif = 896;

            HierarchyParams.PyramidRightSpace = GlobalGameParams.WindowWidth / 5d;
            HierarchyParams.PyramidStepWidth = (GlobalGameParams.WindowWidth - HierarchyParams.PyramidRightSpace) / 12d;

            HierarchyParams.PyramidTopSpace = GlobalGameParams.WindowHeight * 0.2;
            HierarchyParams.PyramidStepHeight = (GlobalGameParams.WindowHeight - HierarchyParams.PyramidTopSpace) / 12d;

            HierarchyParams.PyramidAvatarHeight = HierarchyParams.PyramidStepHeight * 2.4;
        }
    }
}
