using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LG.Models
{
    public static class SpaceParams
    {
        public static event InfoBoardWidthChanged OnInfoBoardWidthChanged;
        public static event BlockSpeedChanged OnBlockSpeedChanged;
        public static event ShipSpeedChanged OnShipSpeedChanged;

        public static int SpaceWidth { get; set; }
        public static int SpaceHeight { get; set; }

        public static double SpaceHeightRatioTo900 { get; set; }
        public static double TimeForAssetСapture = 500; //ms
        public static int RightPanelWidthMin { get; set; }

        private static double _spaceAspectRatio;
        public static double SpaceAspectRatio
        {
            get
            {
                return _spaceAspectRatio;
            }
        }

        private static double _bigWayDistance;
        public static double BigWayDistance
        {
            get
            {
                return _bigWayDistance * SpaceParams.SpaceHeightRatioTo900;
            }
            set
            {
                _bigWayDistance = value;
            }
        }

        public static int BottomSpaceMin
        {
            get
            {
                return (int)Math.Round(GlobalGameParams.WindowHeight / 6d);
            }
        }
        public static int BottomSpace { get; set; }

        public static int PictureBlockWidth { get; set; }
        public static int PictureBlockHeight { get; set; }

        public static int AssetBlockWidth { get; set; }
        public static int AssetBlockHeight { get; set; }

        public static int Columns { get; set; }
        public static int Rows { get; set; }

        private static double _minBlockSpeed;
        public static double MinBlockSpeed
        {
            get
            {
                if (SpaceParams.SpaceHeightRatioTo900 != 0)
                {
                    return _minBlockSpeed * SpaceParams.SpaceHeightRatioTo900;
                }
                return _minBlockSpeed;
            }
        }

        private static double _blockSpeedStepPerLevel;
        public static double BlockSpeedStepPerLevel
        {
            get
            {
                if (SpaceParams.SpaceHeightRatioTo900 != 0)
                {
                    return _blockSpeedStepPerLevel * SpaceParams.SpaceHeightRatioTo900;
                }
                return _blockSpeedStepPerLevel;
            }
        }

        private static double _blockSpeed;
        public static double BlockSpeed
        {
            get
            {
                return _blockSpeed;
            }
        }

        public static void ChangeBlockSpeed(double delta, bool isSmallChange)
        {
            _blockSpeed += delta;
            if (SpaceParams.OnBlockSpeedChanged != null)
            {
                SpaceParams.OnBlockSpeedChanged(SpaceParams.BlockSpeed, _blockSpeed, isSmallChange);
            }
        }
        private static double _bulletSpeed;
        public static double BulletSpeed
        {
            get
            {
                return _bulletSpeed * SpaceParams.SpaceHeightRatioTo900;
            }
            set
            {
                _bulletSpeed = value;
            }
        }

        private static double _shipSpeed;
        public static double ShipSpeed
        {
            get
            {
                return _shipSpeed * SpaceParams.SpaceHeightRatioTo900;
            }
            set
            {
                _shipSpeed = value;
                if (SpaceParams.OnShipSpeedChanged != null)
                {
                    SpaceParams.OnShipSpeedChanged(_shipSpeed);
                }
            }
        }

        #region Info Board

        private static int _infoBoardWidth;
        public static int InfoBoardWidth
        {
            get
            {
                return _infoBoardWidth;
            }
            set
            {
                _infoBoardWidth = value;
                if (OnInfoBoardWidthChanged != null)
                {
                    OnInfoBoardWidthChanged(_infoBoardWidth);
                }
            }
        }

        private static int _infoBoardHeight;
        public static int InfoBoardHeight
        {
            get
            {
                return _infoBoardHeight;
            }
            set
            {
                _infoBoardHeight = value;
                if (OnInfoBoardWidthChanged != null)
                {
                    OnInfoBoardWidthChanged(_infoBoardWidth);
                }
            }
        }

        private static double _spaceMapHeight900;
        public static double SpaceMapHeight900
        {
            get
            {
                return _spaceMapHeight900;
            }
        }

        private static double _spaceMapBottomHeightForShip;
        public static double SpaceMapBottomHeightForShip
        {
            get
            {
                return _spaceMapBottomHeightForShip;
            }
        }

        private static double _spaceMapWidth;
        public static double SpaceMapWidth
        {
            get
            {
                return _spaceMapWidth;
            }
        }
        #endregion

        public static void Initialize()
        {
            SpaceParams.RightPanelWidthMin = 221;
            SpaceParams._spaceMapWidth = 141;
            SpaceParams._spaceAspectRatio = 0.5625;
            SpaceParams._spaceMapHeight900 = 480;
            SpaceParams._spaceMapBottomHeightForShip = 20;

            SpaceParams.Columns = 8;
            SpaceParams.Rows = 6;

            _blockSpeed = 0;
            _minBlockSpeed = 0.02;   //   pixels/ms
           // _blockSpeedStepPerLevel = 0.012;
            _blockSpeedStepPerLevel = 0.01;
            _bigWayDistance = 27700;
            SpaceParams.BulletSpeed = 1.5;
            //SpaceParams.ShipSpeed = 1.25;
            double spaceWidth = GlobalGameParams.WindowWidth - SpaceParams.RightPanelWidthMin;
            int blockWidth = (int)Math.Round(spaceWidth / SpaceParams.Columns);
            int blockHeight = (int)Math.Round(blockWidth * 0.75d, 0);
            int spaceHeight = blockHeight * SpaceParams.Rows;
            int bottomSpace = GlobalGameParams.WindowHeight - spaceHeight;
            SpaceParams.InfoBoardWidth = SpaceParams.RightPanelWidthMin;
            SpaceParams.BottomSpace = bottomSpace;
            if (bottomSpace < SpaceParams.BottomSpaceMin)
            {
                spaceHeight = GlobalGameParams.WindowHeight - SpaceParams.BottomSpaceMin;
                blockHeight = (int)Math.Round(spaceHeight / (double)SpaceParams.Rows);
                blockWidth = (int)Math.Round(blockHeight / 0.75d, 0);
                int rightPanel = GlobalGameParams.WindowWidth - (blockWidth * SpaceParams.Columns);
                SpaceParams.BottomSpace = GlobalGameParams.WindowHeight - (int)spaceHeight;
                SpaceParams.InfoBoardWidth = rightPanel;
            }
            SpaceParams.SpaceHeightRatioTo900 = (double)spaceHeight / 900d;
            SpaceParams.PictureBlockHeight = blockHeight;
            SpaceParams.PictureBlockWidth = blockWidth;
            SpaceParams.AssetBlockWidth = (int)Math.Round(SpaceParams.PictureBlockWidth * 0.4);
            SpaceParams.AssetBlockHeight = (int)Math.Round((double)(SpaceParams.AssetBlockWidth * 140 / 160), 0);
            SpaceParams.SpaceWidth = blockWidth * SpaceParams.Columns;
            SpaceParams.SpaceHeight = blockHeight * SpaceParams.Rows;
        }
        public static void ResetValues()
        {
            _blockSpeed = 0;
        }
    }
}
public delegate void InfoBoardWidthChanged(int width);
public delegate void BlockSpeedChanged(double ajustedSpeed, double absoluteSpeed, bool isSmallChange);
public delegate void ShipSpeedChanged(double speed);
