using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using LG.Models;

namespace LG.ViewModels
{
    public class ShipControlsVM : UnitVM
    {
        public event LeftWasDown OnLeftWasDown;
        public event LeftWasUp OnLeftWasUp;

        public event RightWasDown OnRightWasDown;
        public event RightWasUp OnRightWasUp;

        #region model
        private ShipControls ShipControlsM { get; set; }

        #endregion

        public event PlayChargerEmptySound OnPlayChargerEmptySound;
        public event ShipJittered OnShipJittered;
      
        private IndicatorVM _ammoCountIndicatorVM;
        public IndicatorVM AmmoCountIndicatorVM
        {
            get
            {
                return _ammoCountIndicatorVM;
            }
        }

        private StarShipVM _shipVM;
        public StarShipVM ShipVM
        {
            get
            {
                return _shipVM;
            }
        }

        private CrossHairVM _targetCrossHairVM;
        public CrossHairVM TargetCrossHairVM
        {
            get
            {
                return _targetCrossHairVM;
            }
            set
            {
                _targetCrossHairVM = value;
                NotifyPropertyChanged("TargetCrossHairVM");
            }
        }

        private int _panelHeight;
        public int PanelHeight
        {
            get
            {
                return _panelHeight;
            }
            set
            {
                _panelHeight = value;
                this.NotifyPropertyChanged("PanelHeight");
            }
        }

        private bool _isMovingLeft;
        public bool IsMovingLeft
        {
            get
            {
                return _isMovingLeft;
            }
            set
            {
                _isMovingLeft = value;
                if (_isMovingLeft)
                {
                    if (this.OnLeftWasDown != null)
                    {
                        this.OnLeftWasDown();
                    }
                }
                else
                {
                    if (this.OnLeftWasUp != null)
                    {
                        this.OnLeftWasUp();
                    }
                }
                this.ShipControlsM.IsMovingLeft = value;
            }
        }
        private bool _isMovingRight;
        public bool IsMovingRight
        {
            get
            {
                return _isMovingRight;
            }
            set
            {
                _isMovingRight = value;
                if (_isMovingRight)
                {
                    if (this.OnRightWasDown != null)
                    {
                        this.OnRightWasDown();
                    }
                }
                else
                {
                    if (this.OnRightWasUp != null)
                    {
                        this.OnRightWasUp();
                    }
                }
                this.ShipControlsM.IsMovingRight = value;
            }
        }

        public ShipControlsVM(ShipControls contrls):base(contrls)
        {
            this.ShipControlsM = contrls;
            this._shipVM = new StarShipVM(contrls.Ship);
            this._targetCrossHairVM = new CrossHairVM(contrls.TargetCrossHair);
            this._ammoCountIndicatorVM = new IndicatorVM(contrls.AmmoCountIndicator);
            ShipControlsM.OnTriedFireWhenChargerEmpty += contrls_OnTriedFireWhenChargerEmpty;
            ShipControlsM.OnShipJittered += contrls_OnShipJittered;
            this.ShipControlsM.OnPanelHeightChanged += ShipControlsM_OnPanelHeightChanged;
        }
     
        private void ShipControlsM_OnPanelHeightChanged(int panelHeight)
        {
            this.PanelHeight = panelHeight;
        }

        public void Fire()
        {
            this.ShipControlsM.Fire();
        }

        private void contrls_OnTriedFireWhenChargerEmpty()
        {
            if (this.OnPlayChargerEmptySound != null)
            {
                this.OnPlayChargerEmptySound();
            }
        }

        private void contrls_OnShipJittered()
        {
            if (OnShipJittered != null)
            {
                OnShipJittered();
            }
        }

        public void DetachEvents()
        {
            ShipControlsM.OnTriedFireWhenChargerEmpty -= contrls_OnTriedFireWhenChargerEmpty;
            ShipControlsM.OnShipJittered -= contrls_OnShipJittered;
            this.ShipControlsM.OnPanelHeightChanged -= ShipControlsM_OnPanelHeightChanged;
            ShipControlsM = null;
        }

    }

    public delegate void PlayChargerEmptySound();
    public delegate void ShipJittered();
    public delegate void LeftWasDown();
    public delegate void LeftWasUp();
    public delegate void RightWasDown();
    public delegate void RightWasUp();
}
