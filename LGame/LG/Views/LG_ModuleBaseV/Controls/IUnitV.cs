using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace LG.Views
{
    public interface IUnitV
    {
        event ShouldBeRemoved OnShouldBeRemoved;
    }

    public delegate void ShouldBeRemoved(UIElement sender);
}
