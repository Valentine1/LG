using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Common;

namespace LG.Models
{
    public partial class AssetM : BaseBlock
    {
        public event StartPositionOnMoveAreaChanged OnStartPositionOnMoveAreaChanged;

        public Flow DataFlow { get; set; }
        public int ID { get; set; }
        public string TextValue { get; set; }
        public int ColumnPositionNumber { get; set; }
        public int RowPositionNumber { get; set; }

        private Point _startPositionOnMoveArea;
        public Point StartPositionOnMoveArea
        {
            get
            {
                return _startPositionOnMoveArea;
            }
            set
            {
                _startPositionOnMoveArea = value;
                if (this.OnStartPositionOnMoveAreaChanged != null)
                {
                    this.OnStartPositionOnMoveAreaChanged(_startPositionOnMoveArea);
                }
            }
        }

        private BaseControls _controls;
        public virtual BaseControls Controls
        {
            get
            {
                if (_controls == null)
                {
                    _controls = new AssetControls(this);
                }
                return _controls;
            }
        }

    }

    public delegate void StartPositionOnMoveAreaChanged(Point sp);

    public enum Flow { FromModelToVM, FromVMToModel }
}
