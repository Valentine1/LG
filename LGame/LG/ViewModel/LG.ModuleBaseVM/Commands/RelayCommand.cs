using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LG.ViewModels.Commands
{
    public class RelayCommand : ICommand
    {
        private Action _handler;

        public RelayCommand(Action handler)
        {

            _handler = handler;

        }

        private bool _isEnabled;
        public bool IsEnabled
        {

            get { return _isEnabled; }

            set
            {

                _isEnabled = value;

            }

        }

        public bool CanExecute(object parameter)
        {

            return IsEnabled;

        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {

            _handler();

        }

    }
}
