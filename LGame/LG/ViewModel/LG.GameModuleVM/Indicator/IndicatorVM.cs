using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Models;

namespace LG.ViewModels
{
    public class IndicatorVM : UnitVM
    {
        public event StartDecreaseMoveAnimation OnStartDecreaseMoveAnimation;
        public event OpacityChanged OnOpacityChanged;
        private Indicator IndicatorM { get; set; }

        private ObservableCollection<StripeVM> _stripes;
        public ObservableCollection<StripeVM> Stripes
        {
            get
            {
                if (_stripes == null)
                {
                    _stripes = new ObservableCollection<StripeVM>();
                }
                return _stripes;
            }
        }

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
                if (OnOpacityChanged != null)
                {
                    OnOpacityChanged(_opacity);
                }
                this.NotifyPropertyChanged("Opacity");
            }
        }

        public IndicatorVM(Indicator indic)
            : base(indic)
        {
            this.IndicatorM = indic;
            this.Opacity = indic.Opacity;
            foreach (Stripe str in indic.Panel)
            {
                StripeVM svm = new StripeVM(str);
                svm.OnItselfDeleted += svm_OnItselfDeleted;
                Stripes.Add(svm);
            }

            if (indic is IndicatorMoving)
            {
                ((IndicatorMoving)indic).OnDecreaseBegin += IndicatorVM_OnDecreaseBegin;
                indic.OnOpacityChanged += indic_OnOpacityChanged;
            }
        }

        void svm_OnItselfDeleted(UnitVM uvm)
        {
            uvm.OnItselfDeleted -= svm_OnItselfDeleted;
            Stripes.Remove(uvm as StripeVM);
        }

        private void IndicatorVM_OnDecreaseBegin()
        {
            if (this.OnStartDecreaseMoveAnimation != null)
            {
                this.OnStartDecreaseMoveAnimation();
            }
        }
        private void indic_OnOpacityChanged(double op)
        {
            this.Opacity = op;
        }
        public void DecreaseMoveAnimationEnded()
        {
            ((IndicatorMoving)this.IndicatorM).EndDecrease();
        }

        public override void DeleteItself(Unit m)
        {
            base.DeleteItself(m);
        }
        public override void DetachEvents(Unit m)
        {
            base.DetachEvents(m);
            if (m is IndicatorMoving)
            {
                ((IndicatorMoving)m).OnDecreaseBegin += IndicatorVM_OnDecreaseBegin;
                ((IndicatorMoving)m).OnOpacityChanged += indic_OnOpacityChanged;
            }
        }
    }

    public delegate void StartDecreaseMoveAnimation();
    public delegate void OpacityChanged(double opacity);
}
