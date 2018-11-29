using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.Search;
using LG.Common;


namespace LG.Models
{
    public class BackSpaceGrid : BaseBlock
    {
        static private Random _rand;
        static internal Random Rand
        {
            get
            {
                if (_rand == null)
                {
                    _rand = new Random();
                }
                return _rand;
            }
        }
        static private Dictionary<int, SpaceGridCell> _cellsStore;
        static internal Dictionary<int,SpaceGridCell> CellsStore
        {
            get
            {
                if (_cellsStore == null)
                {
                    _cellsStore = new Dictionary<int, SpaceGridCell>();
                }
                return _cellsStore;
            }
        }

        static private List<int> _cellsStoreIndexes;
        static internal List<int> CellsStoreIndexes
        {
            get
            {
                if (_cellsStoreIndexes == null)
                {
                    _cellsStoreIndexes = new List<int>();
                }
                return _cellsStoreIndexes;
            }
        }

        static private Dictionary<int, SpaceGridCellNebula> _cellsNebulaStore;
        static internal Dictionary<int, SpaceGridCellNebula> CellsNebulaStore
        {
            get
            {
                if (_cellsNebulaStore == null)
                {
                    _cellsNebulaStore = new Dictionary<int, SpaceGridCellNebula>();
                }
                return _cellsNebulaStore;
            }
        }

        static private List<int> _cellsNebulaStoreIndexes;
        static internal List<int> CellsNebulaStoreIndexes
        {
            get
            {
                if (_cellsNebulaStoreIndexes == null)
                {
                    _cellsNebulaStoreIndexes = new List<int>();
                }
                return _cellsNebulaStoreIndexes;
            }
        }

        static private List<AssetInitializer> _smallStarsInitializers;
        static internal List<AssetInitializer> SmallStarsInitializers
        {
            get
            {
                if (_smallStarsInitializers == null)
                {
                    _smallStarsInitializers = new List<AssetInitializer>();
                }
                return _smallStarsInitializers;
            }
        }

        static private List<AssetInitializer> _bigStarsInitializers;
        static internal List<AssetInitializer> BigStarsInitializers
        {
            get
            {
                if (_bigStarsInitializers == null)
                {
                    _bigStarsInitializers = new List<AssetInitializer>();
                }
                return _bigStarsInitializers;
            }
        }

        static private List<AssetInitializer> _blinkingStarsInitializers;
        static internal List<AssetInitializer> BlinkingStarsInitializers
        {
            get
            {
                if (_blinkingStarsInitializers == null)
                {
                    _blinkingStarsInitializers = new List<AssetInitializer>();
                }
                return _blinkingStarsInitializers;
            }
        }

        static private List<AssetInitializer> _nebulasInitializers;
        static internal List<AssetInitializer> NebulasInitializers
        {
            get
            {
                if (_nebulasInitializers == null)
                {
                    _nebulasInitializers = new List<AssetInitializer>();
                }
                return _nebulasInitializers;
            }
        }

        private ObservableCollection<SpaceGridRow> _spaceRows;
        public ObservableCollection<SpaceGridRow> SpaceRows
        {
            get
            {
                if (_spaceRows == null)
                {
                    _spaceRows = new ObservableCollection<SpaceGridRow>();
                }
                return _spaceRows;
            }
        }

        private Queue<SpaceGridRow> _spaceRowsQueue;
        internal Queue<SpaceGridRow> SpaceRowsQueue
        {
            get
            {
                if (_spaceRowsQueue == null)
                {
                    _spaceRowsQueue = new Queue<SpaceGridRow>();
                }
                return _spaceRowsQueue;
            }
        }

        public void TimeElapses(double deltaTime)
        {
            this.PositionY = this.PositionY + (deltaTime * BackgroundParams.Speed);
            if (this.HasLastRowDisappeared())
            {
                this.ReArrangeRows();
            }
        }

        public void Initialize()
        {
            for (int i = BackgroundParams.RowsCount-1; i >=0; i--)
            {
                SpaceGridRow row = new SpaceGridRow();
                row.StartPosition = new Point() { Y = i * BackgroundParams.CellHeight };
                this.SpaceRows.Add(row);
                this.SpaceRowsQueue.Enqueue(row);
            }
        }

        async public void PostInitialize()
        {
            await CreateCelestialObjectsInitializers();
            this.FillCellsStore();
            this.FillRows();
        }
        async private Task CreateCelestialObjectsInitializers()
        {
            StorageFolder imFolder = await Package.Current.InstalledLocation.GetFolderAsync("Images");
            StorageFolder bFolder = await imFolder.GetFolderAsync("backspace");
            StorageFolder sFolder = await bFolder.GetFolderAsync("smallstars");
            await this.CreateInitializers(BackSpaceGrid.SmallStarsInitializers, sFolder);

            StorageFolder bigFolder = await bFolder.GetFolderAsync("bigstars");
            await this.CreateInitializers(BackSpaceGrid.BigStarsInitializers, bigFolder);

            StorageFolder blinkFolder = await bFolder.GetFolderAsync("blinkstars");
            await this.CreateInitializers(BackSpaceGrid.BlinkingStarsInitializers, blinkFolder);

            StorageFolder nebFolder = await bFolder.GetFolderAsync("nebulas");
            await this.CreateInitializers(BackSpaceGrid.NebulasInitializers, nebFolder);
        }

        async private Task CreateInitializers(List<AssetInitializer> initers, StorageFolder folder)
        {
            QueryOptions qopt = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() { ".png" });
            StorageFileQueryResult r = folder.CreateFileQueryWithOptions(qopt);
            IReadOnlyList<StorageFile> ims = await r.GetFilesAsync();
            foreach (StorageFile im in ims)
            {
                AssetInitializer ai = new AssetInitializer();
                await ai.InitializeBitmapSource(im);
                initers.Add(ai);
            }
        }

        private void FillCellsStore()
        {
            for (int i = 0; i < BackgroundParams.CellsCount * BackgroundParams.RowsCount; i++)
            {
                SpaceGridCell cell = SpaceGridCell.CreateCell();
                BackSpaceGrid.CellsStore.Add(cell.ID,cell);
            }

            foreach (AssetInitializer ai in BackSpaceGrid.NebulasInitializers)
            {
                SpaceGridCellNebula cell = SpaceGridCell.CreateCell(ai);
                BackSpaceGrid.CellsNebulaStore.Add(cell.ID, cell);
            }
        }
        private void FillRows()
        {
            foreach (SpaceGridRow row in this.SpaceRows)
            {
                row.FillWithCells();
            }
        }
        private bool HasLastRowDisappeared()
        {
            if ((this.PositionY + (this.SpaceRowsQueue.Peek().StartPosition.Y + this.SpaceRowsQueue.Peek().PositionY) > BackgroundParams.CellHeight * BackgroundParams.RowsCount))
            {
                return true;
            }

            return false;
        }

        private void ReArrangeRows()
        {
            SpaceGridRow r = this.SpaceRowsQueue.Dequeue();
            r.DisposeCells();
            r.PositionY = r.PositionY - BackgroundParams.CellHeight * BackgroundParams.RowsCount;
            r.FillWithCells();
            this.SpaceRowsQueue.Enqueue(r);
        }
    }
}
