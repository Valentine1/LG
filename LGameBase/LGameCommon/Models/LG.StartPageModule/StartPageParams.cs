using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public static class StartPageParams
    {
        private static float _mainMenuWidth1920;
        public static float MainMenuWidth1920
        {
            get
            {
                return _mainMenuWidth1920;
            }
        }

        private static float _mainMenuHeight1920;
        public static float MainMenuHeight1920
        {
            get
            {
                return _mainMenuHeight1920;
            }
        }

        private static int _profilesWidth1920;
        public static int ProfilesWidth1920
        {
            get
            {
                return _profilesWidth1920;
            }
        }

        private static int _profilesHeight1920;
        public static int ProfilesHeight1920
        {
            get
            {
                return _profilesHeight1920;
            }
        }

        private static int _profilesRegisterLocalHeight1920;
        public static int ProfilesRegisterLocalHeight1920
        {
            get
            {
                return _profilesRegisterLocalHeight1920;
            }
        }

        private static int _profilesRegisterOnInternetHeight1920;
        public static int ProfilesRegisterOnInternetHeight1920
        {
            get
            {
                return _profilesRegisterOnInternetHeight1920;
            }
        }

        public static void Initialize()
        {
            _mainMenuWidth1920 = 1057;
            _mainMenuHeight1920 = 382;

            _profilesWidth1920 = 230;
            _profilesHeight1920 = 176;
            _profilesRegisterLocalHeight1920 = 307;
            _profilesRegisterOnInternetHeight1920 = 429;
        }
    }
}
