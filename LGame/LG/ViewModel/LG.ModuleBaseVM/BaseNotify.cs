using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace LG.ViewModels
{
    public class BaseNotify : DependencyObject, INotifyPropertyChanged
    {
        public FlowDirection FlowDirectionVM { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public enum FlowDirection { FromVMtoView, FromViewToVM }
}
