using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Data;

namespace LG.Models.Licenses
{
    public static partial class LicenseManager
    {
        public static int BaseThemesCount = 10;

        public static string Lic1_20
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "1_20";
            }

        }
        public static string Lic21_26
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "21_26";
            }

        }
        public static string Lic27_32
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "27_32";
            }

        }
        public static string Lic33_38
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "33_38";
            }

        }
        public static string Lic39_44
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "39_44";
            }

        }
        public static string Lic45_50
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "45_50";
            }

        }
        public static string Lic51_56
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "51_56";
            }

        }
        public static string Lic57_62
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "57_62";
            }

        }
        public static string Lic63_68
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "63_68";
            }

        }
        public static string Lic69_74
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "69_74";
            }

        }
        public static string Lic75_80
        {
            get
            {
                return GlobalGameParams.AppLang.Name + "75_80";
            }

        }
        public static License GetLicenseInfo(int thId)
        {
            License lic = new License();
            if (LicenseManager.BelongsToLic1_20(thId))
            {
                lic.ID = LicenseManager.Lic1_20;
                lic.IsActive = LicenseManager.Lic1_20IsActive();
            }
            else if (LicenseManager.BelongsToLic21_26(thId))
            {
                lic.ID = LicenseManager.Lic21_26;
                lic.IsActive = LicenseManager.Lic21_26IsActive();
            }
            else if (LicenseManager.BelongsToLic27_32(thId))
            {
                lic.ID = LicenseManager.Lic27_32;
                lic.IsActive = LicenseManager.Lic27_32IsActive();
            }
            else if (LicenseManager.BelongsToLic33_38(thId))
            {
                lic.ID = LicenseManager.Lic33_38;
                lic.IsActive = LicenseManager.Lic33_38IsActive();
            }
            else if (LicenseManager.BelongsToLic39_44(thId))
            {
                lic.ID = LicenseManager.Lic39_44;
                lic.IsActive = LicenseManager.Lic39_44IsActive();
            }
            else if (LicenseManager.BelongsToLic45_50(thId))
            {
                lic.ID = LicenseManager.Lic45_50;
                lic.IsActive = LicenseManager.Lic45_50IsActive();
            }
            else if (LicenseManager.BelongsToLic51_56(thId))
            {
                lic.ID = LicenseManager.Lic51_56;
                lic.IsActive = LicenseManager.Lic51_56IsActive();
            }
            else if (LicenseManager.BelongsToLic57_62(thId))
            {
                lic.ID = LicenseManager.Lic57_62;
                lic.IsActive = LicenseManager.Lic57_62IsActive();
            }
            else if (LicenseManager.BelongsToLic63_68(thId))
            {
                lic.ID = LicenseManager.Lic63_68;
                lic.IsActive = LicenseManager.Lic63_68IsActive();
            }
            else if (LicenseManager.BelongsToLic69_74(thId))
            {
                lic.ID = LicenseManager.Lic69_74;
                lic.IsActive = LicenseManager.Lic69_74IsActive();
            }
            else if (LicenseManager.BelongsToLic75_80(thId))
            {
                lic.ID = LicenseManager.Lic75_80;
                lic.IsActive = LicenseManager.Lic75_80IsActive();
            }
            return lic;
        }

        public static List<int> GetThemesIdsByLic(License lic)
        {
            List<int> ids = null;
            if (lic.ID == LicenseManager.Lic21_26)
            {
                return ids = new List<int>() { 2, 3 };
            }
            else if (lic.ID == LicenseManager.Lic27_32)
            {
                return ids = new List<int>() { 4, 6 };
            }
            return ids;
        }

        private static bool BelongsToLic1_20(int thId)
        {
            return thId <= 20;
        }

        private static bool BelongsToLic21_26(int thId)
        {
            return thId >= 21 && thId <= 26;
        }
        private static bool BelongsToLic27_32(int thId)
        {
            return thId >= 27 && thId <= 32;
        }
        private static bool BelongsToLic33_38(int thId)
        {
            return thId >= 33 && thId <= 38;
        }
        private static bool BelongsToLic39_44(int thId)
        {
            return thId >= 39 && thId <= 44;
        }
        private static bool BelongsToLic45_50(int thId)
        {
            return thId >= 45 && thId <= 50;
        }
        private static bool BelongsToLic51_56(int thId)
        {
            return thId >= 51 && thId <= 56;
        }
        private static bool BelongsToLic57_62(int thId)
        {
            return thId >= 57 && thId <= 62;
        }
        private static bool BelongsToLic63_68(int thId)
        {
            return thId >= 63 && thId <= 68;
        }
        private static bool BelongsToLic69_74(int thId)
        {
            return thId >= 69 && thId <= 74;
        }
        private static bool BelongsToLic75_80(int thId)
        {
            return thId >= 75 && thId <= 80;
        }
    }
}
