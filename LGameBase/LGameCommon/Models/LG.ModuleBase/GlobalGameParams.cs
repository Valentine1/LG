using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Data;

namespace LG.Models
{
    public static class GlobalGameParams
    {
        private static Language _appLang;
        public static Language AppLang
        {
            get
            {
                if (_appLang == null)
                {
                    _appLang = new Language() { ID = 1, Name = "English" };
                }
                return _appLang;
            }
        }

        private static MessageConstants _messages;
        public static MessageConstants Messages
        {
            get
            {
                if (_messages == null)
                {
                    _messages = new MessageConstants(AppLang);
                }
                return _messages;
            }
        }

        private static string _fileStorageUrl;
        async public static Task<string> FileStorageUrl()
        {
            if (_fileStorageUrl == null)
            {
                _fileStorageUrl = await Module.GetThemeLoader().GetFileStorageUrl();
            }
            return _fileStorageUrl;
        }

        public static int WindowWidth { get; set; }
        public static int WindowHeight { get; set; }
        public static int WindowHeightMin { get; set; }
        public static ScreenOrientation Orientation { get; set; }

        private static int _globalInfoPanelHeight;
        public static int GlobalInfoPanelHeight
        {
            get
            {
                return _globalInfoPanelHeight;
            }
            set
            {
                _globalInfoPanelHeight = value;
            }
        }

        public static double GlobalInfoPanelAspectRatio { get; set; }

        public static void Initialize()
        {
            GlobalGameParams.WindowHeightMin = 768;
            GlobalGameParams.GlobalInfoPanelAspectRatio = 0.382653;
        }
    }
    public enum ScreenOrientation { Landscape, Portrait }

    public delegate void GlobalInfoPanelHeightChanged();
}
