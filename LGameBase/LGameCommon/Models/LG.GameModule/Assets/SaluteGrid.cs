using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using LG.Common;

namespace LG.Models
{
    public class SaluteGrid : AssetGrid
    {
        private ObservableCollection<AnimatedAssetInitializer> _animAssetIniter;
        private ObservableCollection<AnimatedAssetInitializer> AnimAssetIniters
        {
            get
            {
                if (_animAssetIniter == null)
                {
                    _animAssetIniter = new ObservableCollection<AnimatedAssetInitializer>();
                }
                return _animAssetIniter;
            }
        }

        private readonly int _rows;
        protected int Rows
        {
            get
            {
                return _rows;
            }
        }

        public SaluteGrid(int col, int rows) : base(col)
        {
            _rows = rows;
        }

        public void Initialize(List<AnimatedAssetSource> saluteSources)
        {
            foreach (AnimatedAssetSource aas in saluteSources)
            {
                AnimatedAssetInitializer initer = new AnimatedAssetInitializer();
                initer.OnImageSourcesInitialized += initer_OnImageSourcesInitialized;
                initer.InitializeBitmapSources(aas);
            }
        }

        public void StartSalute()
        {
            Monitor.Enter(this.AnimAssetIniters);
            if (this.AnimAssetIniters.Count == 0)
            {
                this.AnimAssetIniters.CollectionChanged += AnimAssetIniters_CollectionChanged;
                Monitor.Exit(this.AnimAssetIniters);
            }
            else
            {
                Monitor.Exit(this.AnimAssetIniters);
                this.Salute();
            }

        }
        public void StopSalute()
        {
            this.DeleteAllAssets();
        }

        private void initer_OnImageSourcesInitialized(AnimatedAssetInitializer initer)
        {
            initer.OnImageSourcesInitialized -= initer_OnImageSourcesInitialized;
            Monitor.Enter(this.AnimAssetIniters);
            this.AnimAssetIniters.Add(initer);
            Monitor.Exit(this.AnimAssetIniters);
        }
        private void Salute()
        {
            for (int i = 0; i < this.Columns; i++)
            {
                for (int j = 0; j < this.Rows; j++)
                {
                    this.CreateAssetAndSetInitialPos(i, j);
                    this.CreateAssetAndSetInitialPos(i, j);
                }
            }
        }

        protected void CreateAssetAndSetInitialPos(int col, int row)
        {
            AnimatedAssetInitializer saluteIniter = this.GetRandomSaluteIniter();
            AnimatedAssetM sal = new SaluteM(AnimationBehavior.OneTime);

            sal.OnAnimationEnded += sal_OnAnimationEnded;

            sal.StartPosition = this.CalcInitialPosition(col, row);
            sal.ColumnPositionNumber = col;
            sal.RowPositionNumber = row;

            saluteIniter.InitializeAnimatedAsset(sal, false, true);
            this.Assets.Add(sal);
        }

        private void sal_OnAnimationEnded(AnimatedAssetM asset)
        {
            asset.OnAnimationEnded -= sal_OnAnimationEnded;
            this.CreateAssetAndSetInitialPos(asset.ColumnPositionNumber, asset.RowPositionNumber);
            this.DeleteAsset(asset);
        }

        private void AnimAssetIniters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                this.AnimAssetIniters.CollectionChanged -= AnimAssetIniters_CollectionChanged;
                this.Salute();
            }
        }

        private AnimatedAssetInitializer GetRandomSaluteIniter()
        {
            int i = this.Rand.Next(0, this.AnimAssetIniters.Count);
            return this.AnimAssetIniters[i];
        }
        private Point CalcInitialPosition(int col, int row)
        {
            int x = SpaceParams.SpaceWidth / (int)this.Columns * col;
            int y = SpaceParams.SpaceHeight / (int)this.Rows * row;
            x = this.Rand.Next(x, x + SpaceParams.SpaceWidth / (int)this.Columns);
            y = this.Rand.Next(y, y + SpaceParams.SpaceHeight / (int)this.Rows);
            return new Point() { X = x, Y = y };
        }

        public override void DeleteItself()
        {
            base.DeleteItself();
            foreach (AnimatedAssetInitializer aai in this.AnimAssetIniters)
            {
                aai.OnImageSourcesInitialized -= initer_OnImageSourcesInitialized;
                aai.DeleteItself();
            }
            this.AnimAssetIniters.Clear();
        }

        public override void DetachEvents()
        {
            base.DetachEvents();
            this.AnimAssetIniters.CollectionChanged -= AnimAssetIniters_CollectionChanged;
        }
    }
}
