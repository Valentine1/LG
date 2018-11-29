using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.System;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LG.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LG.Views
{
    public sealed partial class ShipControlsV : UserControl
    {
        private ShipControlsVM ControlsVM { get; set; }

        public ShipControlsV()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        public void Initialize(ShipControlsVM controlsVM)
        {
            this.DataContext = controlsVM;
            this.ControlsVM = controlsVM;
            this.ControlsVM.OnItselfDeleted += controlsVM_OnItselfDeleted;
        }

        private bool WasSpacePressed;
        private bool WasRightKeyPressed;
        private bool WasLeftKeyPressed;
        private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Right)
            {
                this.WasRightKeyPressed = false;
                this.ControlsVM.IsMovingRight = false;
                btDirRight.EmulateUp();
            }
            else if (args.VirtualKey == VirtualKey.Left)
            {
                this.WasLeftKeyPressed = false;
                this.ControlsVM.IsMovingLeft = false;
                btDirLeft.EmulateUp();
            }
            else if (args.VirtualKey == VirtualKey.Space)
            {
                this.WasSpacePressed = false;
                btFire.EmulateUp();
            }
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Right)
            {
                if (!this.WasRightKeyPressed)
                {
                    this.WasRightKeyPressed = true;
                    this.ControlsVM.IsMovingRight = true;
                    btDirRight.EmulateDown();
                }
            }
            else if (args.VirtualKey == VirtualKey.Left)
            {
                if (!this.WasLeftKeyPressed)
                {
                    this.WasLeftKeyPressed = true;
                    this.ControlsVM.IsMovingLeft = true;
                    btDirLeft.EmulateDown();
                }
            }
            else if (args.VirtualKey == VirtualKey.Space)
            {
                if (!this.WasSpacePressed)
                {
                    this.WasSpacePressed = true;
                    this.ControlsVM.Fire();
                    btFire.EmulateDown();
                }
            }
        }

        private void btDirLeft_OnDirectionButtonUp()
        {
            this.ControlsVM.IsMovingLeft = false;
        }
        private void btDirLeft_OnDirectionButtonDown()
        {
            this.ControlsVM.IsMovingLeft = true;
        }

        private void btDirRight_OnDirectionButtonUp()
        {
            this.ControlsVM.IsMovingRight = false;
        }
        private void btDirRight_OnDirectionButtonDown()
        {
            this.ControlsVM.IsMovingRight = true;
        }

        private void btFire_OnDirectionButtonUp()
        {
            this.WasSpacePressed = false;
        }
        private void btFire_OnDirectionButtonDown()
        {
            if (!this.WasSpacePressed)
            {
                this.WasSpacePressed = true;
                this.ControlsVM.Fire();
            }
        }
        private void controlsVM_OnItselfDeleted(UnitVM uvm)
        {
            this.DetachEvents();
            ControlsVM = null;
        }

        public void DetachEvents()
        {
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp -= CoreWindow_KeyUp;
        }
    }
}
