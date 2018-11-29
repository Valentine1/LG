using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public abstract class AssetColumnsGrid : AssetGrid
    {
        private Dictionary<int, List<AssetM>> _assetColumns;
        protected Dictionary<int, List<AssetM>> AssetColumns
        {
            get
            {
                if (_assetColumns == null)
                {
                    _assetColumns = new Dictionary<int, List<AssetM>>();
                    int col = this.Columns == null ? SpaceParams.Columns : (int)this.Columns;
                    for (int i = 0; i < col; i++)
                    {
                        _assetColumns[i] = new List<AssetM>();
                    }
                }
                return _assetColumns;
            }
        }

        public int AllCreatedLinesCount { get; set; }
        public static int AssetIdIncrementer { get; set; }

        public AssetColumnsGrid()
        {

        }
        public AssetColumnsGrid(int col):base(col)
        {
        }

        public void CreateAssetLine()
        {
            AssetM ass = this.CreateAssetAndSetInitialPos();
            this.Assets.Add(ass);
        }
        public AssetM DetectCollisionWithStarShip(StarShipM ss)
        {
            for (int i = 0; i < AssetColumns.Count; i++)
            {
                if (AssetColumns[i].Count > 0)
                {
                    int indexLast = AssetColumns[i].Count - 1;
                    AssetM ass = AssetColumns[i][indexLast];
                    if (ass.IntersectsNotRotatedWithNotRotated(ss))
                    {
                        return ass;
                    }
                }
            }
            return null;
        }
        public AssetM DetectCollisionWithBullet(BulletM bul)
        {
            for (int i = 0; i < AssetColumns.Count; i++)
            {
                if (AssetColumns[i].Count > 0)
                {
                    int indexLast = AssetColumns[i].Count - 1;
                    AssetM ass = AssetColumns[i][indexLast];
                    double ax = ass.StartPosition.X + ass.PositionX;
                    double ay = ass.StartPosition.Y + ass.PositionY;
                    double bx = bul.StartPosition.X + bul.PositionX;
                    double by = bul.StartPosition.Y + bul.PositionY;
                    if ((bx > ax && bx < ax + ass.BlockSize.Width) && (by < ay && by > ay - ass.BlockSize.Height))
                    {
                        return ass;
                    }
                }
            }
            return null;
        }
        public AssetM RemoveAssetsFromInvisibleSpace()
        {
            for (int i = 0; i < this.AssetColumns.Count; i++)
            {
                List<AssetM> assForDelete = new List<AssetM>();
                foreach (AssetM ass in this.AssetColumns[i])
                {
                    if (this.CheckIfAssetIsOut(ass))
                    {
                        assForDelete.Add(ass);
                    }
                }
                foreach (AssetM ass in assForDelete)
                {
                    this.DeleteAsset(ass);
                    return ass;
                }
            }
            return null;
        }
        public override void DeleteAllAssets()
        {
            base.DeleteAllAssets();
            for (int i = 0; i < this.AssetColumns.Count; i++)
            {
                this.AssetColumns[i].Clear();
            }
        }
        protected override bool CheckIfAssetIsOut(AssetM ass)
        {
            if ((ass.StartPosition.Y + ass.PositionY) > (SpaceParams.SpaceHeight - ass.BlockSize.Height))
            {
                return true;
            }
            return false;
        }
        protected virtual AssetM CreateAssetAndSetInitialPos()
        {
            return null;
        }
        public AssetM GetLastPicture(int col)
        {
            int count = this.AssetColumns[col].Count;
            if (count == 0)
            {
                return null;
            }
            else
            {
               AssetM a = this.AssetColumns[col][count - 1];
               if (a.PositionY > SpaceParams.PictureBlockHeight)
               {
                   return a;
               }
               else
               {
                   return null;
               }
            }
        }
        public override void RemoveAssetFromCollections(AssetM asset)
        {
            base.RemoveAssetFromCollections(asset);
            this.AssetColumns[asset.ColumnPositionNumber].Remove(asset);
        }
    }

    
  
}
