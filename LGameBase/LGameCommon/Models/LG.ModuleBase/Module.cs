using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public abstract partial class Module : Unit
    {
        public event NavigateTo OnNavigateTo;
        public event ProgressShowStart OnProgressShowStart;
        public event ProgressShowEnd OnProgressShowEnd;

        public ModuleNavigationMethod NavigationMethod { get; set; }
        public double Opacity { get; set; }
        public ModuleType Type { get; set; }

        private static DataLoaderFactory _loaderFactory;
        private static DataLoaderFactory LoaderFactory
        {
            get
            {
                if (_loaderFactory == null)
                {
                    _loaderFactory = new DataLoaderFactory();
                }
                return _loaderFactory;
            }
        }

        private static IDataLoader _loader;
        public static IDataLoader Loader
        {
            get
            {
                if (_loader == null)
                {
                    _loader = LoaderFactory.GetDataLoader(Module.LocalStoaragePath + "\\data");
                }
                return _loader;
            }
        }

        private static IProfileDataLoader _profLoader;
        public static IProfileDataLoader GetProfLoader(ProfileType ptype)
        {
            _profLoader = LoaderFactory.GetProfileDataLoader(ptype, Module.LocalStoaragePath + "\\data");
            return _profLoader;
        }

        private static IThemeDataLoader _themeLoader;
        public static IThemeDataLoader GetThemeLoader()
        {
            if (_themeLoader == null)
            {
                _themeLoader = LoaderFactory.GetThemeDataLoader();
            }
            return _themeLoader;
        }

        public Module()
        {
        }

        public virtual void TimeElapses(int roundedDeltaTime, double deltaTime)
        {

        }
        async public virtual Task Initialize()
        {

        }

        public virtual void PostInitialize()
        {
        }

        public void NavigateTo(ModuleType modType)
        {
            if (this.OnNavigateTo != null)
            {
                this.OnNavigateTo(modType);
            }
        }
        public void StartProgressBar(string mes)
        {
            if (this.OnProgressShowStart != null)
            {
                this.OnProgressShowStart(mes);
            }
        }
        public void EndProgressBar()
        {
            if (this.OnProgressShowEnd != null)
            {
                this.OnProgressShowEnd();
            }
        }
        public static void RefreshThemeLoader(string url)
        {
            _themeLoader = null;
            LoaderFactory.ResetServiceConnection();
        }
    }

    public delegate void NavigateTo(ModuleType modType);
    public delegate void ProgressShowStart(string mes);
    public delegate void ProgressShowEnd();

    public enum ModuleNavigationMethod { Transition, Shuffle }
    public enum ModuleType { StartPage, Game, Themes, Hierarchy, Settings }

}
