using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class LetterVM : AssetVM
    {
        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                this.NotifyPropertyChanged("Value");
            }
        }

        public LetterVM(Letter let): base(let)
        {
            this.Value = let.TextValue;
        }
    }
}
