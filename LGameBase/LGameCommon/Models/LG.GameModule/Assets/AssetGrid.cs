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
    public abstract class AssetGrid : BaseBlock
    {
        public event AssetsReached4 OnAssetsReached4;

        private ObservableCollection<AssetM> _assets;
        public ObservableCollection<AssetM> Assets
        {
            get
            {
                if (_assets == null)
                {
                    _assets = new ObservableCollection<AssetM>();
                    _assets.CollectionChanged += Assets_CollectionChanged;
                }
                return _assets;
            }
        }

        private readonly int? _columns;
        protected int? Columns
        {
            get
            {
                return _columns;
            }
        }

        protected Random Rand = new Random();
        public bool IsAssetsReached4;

        public AssetGrid()
        {
        }
        public AssetGrid(int col)
        {
            _columns = col;
        }

        public virtual void TimeElapses(double roundedDeltaTime)
        {
            foreach (AssetM ass in this.Assets)
            {
                ass.Controls.TimeElapses(roundedDeltaTime);
            }
        }
        public virtual void DeleteAllAssets()
        {
            foreach (AssetM ass in this.Assets)
            {
                ass.DeleteItself();
            }
            this.Assets.Clear();
            this._assets = null;
        }
        public void DeleteAsset(AssetM asset)
        {
            this.RemoveAssetFromCollections(asset);
            asset.DeleteItself();
        }
        public virtual void RemoveAssetFromCollections(AssetM asset)
        {
            this.Assets.Remove(asset);
        }
        public void RemoveAssetsOutOfScope()
        {
            List<AssetM> assForDelete = new List<AssetM>();
            for (int i = 0; i < this.Assets.Count; i++)
            {
                if (this.CheckIfAssetIsOut(this.Assets[i]))
                {
                    assForDelete.Add(this.Assets[i]);
                }
            }
            foreach (AssetM ass in assForDelete)
            {
                this.DeleteAsset(ass);
            }
        }
        protected virtual bool CheckIfAssetIsOut(AssetM ass)
        {
            if ((ass.StartPosition.Y + ass.PositionY) > (SpaceParams.SpaceHeight))
            {
                return true;
            }
            return false;
        }

        private void Assets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (this.Assets.Count == 4)
                {
                    this.IsAssetsReached4 = true;
                    if (this.OnAssetsReached4 != null)
                    {
                        this.OnAssetsReached4();
                    }
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (this.Assets.Count == 3)
                {
                    this.IsAssetsReached4 = false;
                }
            }
        }
        public override void DeleteItself()
        {
            base.DeleteItself();
        }
        public override void DetachEvents()
        {
            base.DetachEvents();
            Assets.CollectionChanged -= Assets_CollectionChanged;
        }
    }

    public delegate void AssetsReached4();



}
