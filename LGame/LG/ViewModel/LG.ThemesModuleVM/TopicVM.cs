using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class TopicVM : AssetVM
    {
        private ObservableCollection<PictureBoxM> _pictures;
        public ObservableCollection<PictureBoxM> Pictures
        {
            get
            {
                if (_pictures == null)
                {
                    _pictures = new ObservableCollection<PictureBoxM>();
                }
                return _pictures;
            }
            set
            {
                _pictures = value;
            }
        }

        private ObservableCollection<Letter> _nameInLetters;
        public ObservableCollection<Letter> NameInLetters
        {
            get
            {
                if (_nameInLetters == null)
                {
                    _nameInLetters = new ObservableCollection<Letter>();
                }
                return _nameInLetters;
            }
            set
            {
                _nameInLetters = value;
            }
        }

        public string Name { get; set; }
        public string PathData { get; set; }

        public TopicVM(Topic top) : base(top)
        {

        }
    }
}
