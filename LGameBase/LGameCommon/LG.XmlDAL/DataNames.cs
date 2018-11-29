using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Data
{
    public static class DataNames
    {
        private static string _languages;
        public static string Languages { get { return _languages; } }

        private static string _licenses;
        public static string Licenses { get { return _licenses; } }

        private static string _themes;
        public static string Themes { get { return _themes; } }

        private static string _themesLanguages;
        public static string ThemesLanguages { get { return _themesLanguages; } }

        private static string _themesLicenses;
        public static string ThemesLicences { get { return _themesLicenses; } }

        private static string _words;
        public static string Words { get { return _words; } }

        private static string _hierarchy;
        public static string Hierarchy { get { return _hierarchy; } }

        private static string _themeHierarchy;
        public static string ThemeHierarchy { get { return _themeHierarchy; } }

        private static string _profiles;
        public static string Profiles { get { return _profiles; } }

        private static string _profilesThemes;
        public static string ProfilesThemes { get { return _profilesThemes; } }
        static DataNames()
        {
            DataNames._languages = "Languages.xml";
            DataNames._licenses = "Licenses.xml";
            DataNames._themes = "Themes.xml";
            DataNames._themesLanguages = "ThemeLanguages.xml";
            DataNames._themesLicenses = "ThemesLicences.xml";
            DataNames._words = "Words";
            DataNames._hierarchy = "Hierarchy.xml";
            DataNames._themeHierarchy = "ThemeHierarchy.xml";
            DataNames._profiles = "Profiles.xml";
            DataNames._profilesThemes = "ProfilesThemes";
        }

    }
}
