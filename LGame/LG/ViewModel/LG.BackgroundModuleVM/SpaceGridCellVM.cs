using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class SpaceGridCellVM : AssetVM
    {
        private double _opacity;
        public double Opacity
        {
            get
            {
               return _opacity;
            }
            set
            {
                _opacity = value;
                NotifyPropertyChanged("Opacity");
            }
        }

        private ObservableCollection<AssetVM> _stars;
        public ObservableCollection<AssetVM> Stars
        {
            get
            {
                if (_stars == null)
                {
                    _stars = new ObservableCollection<AssetVM>();
                }
                return _stars;
            }
        }

        private ObservableCollection<BlinkStarVM> _blinkStarsVMs;
        public ObservableCollection<BlinkStarVM> BlinkStarsVMs
        {
            get
            {
                if (_blinkStarsVMs == null)
                {
                    _blinkStarsVMs = new ObservableCollection<BlinkStarVM>();
                }
                return _blinkStarsVMs;
            }
        }

        public SpaceGridCellVM(SpaceGridCell cell) : base(cell)
        {
            foreach (AssetM star in cell.Stars)
            {
                AssetVM svm = new AssetVM(star);
                this.Stars.Add(svm);
            }
            foreach (BlinkStar bstar in cell.BlinkingStars)
            {
                BlinkStarVM bsvm = new BlinkStarVM(bstar);
                this.BlinkStarsVMs.Add(bsvm);
            }
            cell.Stars.CollectionChanged += Stars_CollectionChanged;
        }

        private void Stars_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AssetM star = (AssetM)e.NewItems[0];
                AssetVM svm = new AssetVM(star);
                this.Stars.Add(svm);
            }
        }
    }
}
