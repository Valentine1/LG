using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public abstract class ModuleVM :UnitVM
    {
        protected Module ModelM
        {
            get
            {
                return (Module)this.UnitM;
            }
        }

        public ModuleVM(Module mod):base(mod)
        {

        }

        public void NavigateTo(NavigationCommand com)
        {
            switch (com)
            {
                case NavigationCommand.Play:
                    ModelM.NavigateTo(ModuleType.Game);
                    break;
                case NavigationCommand.SelectTheme:
                    ModelM.NavigateTo(ModuleType.Themes);
                    break;
                case NavigationCommand.Hierarchy:
                    ModelM.NavigateTo(ModuleType.Hierarchy);
                    break;
            }
        }
    }

    public enum NavigationCommand { StartPage, Play, SelectTheme, Settings, Hierarchy }
}
