using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public static class BackgroundParams
    {
        public static event CellHeightChanged OnCellHeightChanged;

        private static int _rowsCount;
        public static int RowsCount
        {
            get
            {
                return _rowsCount;
            }
        }

        private static int _cellsCount;
        public static int CellsCount
        {
            get
            {
                return _cellsCount;
            }
        }

        private static double _cellWidth;
        public static double CellWidth
        {
            get
            {
                return _cellWidth;
            }
        }

        private static double _cellHeight;
        public static double CellHeight
        {
            get
            {
                return _cellHeight;
            }
        }

        private static double _speed;
        public static double Speed
        {
            get
            {
                return _speed;
            }
        }

        static BackgroundParams()
        {
            _rowsCount = 5;
            _cellsCount = 5;
            _speed = 0.005;
        }

        public static void Initialize()
        {
            _cellWidth = GlobalGameParams.WindowWidth / BackgroundParams.CellsCount;
            _cellHeight = GlobalGameParams.WindowHeight / (BackgroundParams.RowsCount-1);
            if (BackgroundParams.OnCellHeightChanged != null)
            {
                BackgroundParams.OnCellHeightChanged(_cellHeight);
            }
        }
    }

    public delegate void CellHeightChanged(double ch);
}
