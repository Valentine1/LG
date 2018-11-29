using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;
using LG.Data;

namespace LG.Models
{
    public partial class PictureBoxInitializer : AssetInitializer
    {
        private List<Word> Words
        {
            get;
            set;
        }
        private Random Rand = new Random();

        async public Task LoadWords()
        {
            DataLoaderFactory fac = new DataLoaderFactory();

            IDataLoader loader = fac.GetDataLoader(Module.LocalStoaragePath + "\\data");

            int take; 
            switch (Game.GameTopic.HierarchyLevel.LevelNo)
            {
                case 1:
                    take = 6;
                    break;
                case 2:
                    take = 10;
                    break;
                case 3:
                    take = 14;
                    break;
                case 4:
                    take = 17;
                    break;
                default:
                    take = 20;
                    break;
            }

            this.Words = await loader.GetWordsByTheme(Game.GameTopic.ID, GlobalGameParams.AppLang, 0, take);
        }

        public void InitWithValues(PictureBoxM pic)
        {
            Word w = this.GetRandomWord();
            pic.WordID = w.ID;
            pic.ID = ++Incrementer;
            pic.TextValue = w.Value;
            pic.BoxColors = new PictureBoxColors()
            {
                BackColor = w.BackColor,
                BorderColor = w.BorderColor,
                LeftColor = w.LeftColor,
                LeftTopColor = w.LeftTopColor,
                TopColor = w.TopColor
            };
            this.AssignPictureBitmap(pic);
            this.AssignAudioStream(pic);
        }

        private Word GetRandomWord()
        {
            switch (Game.GameTopic.HierarchyLevel.LevelNo)
            {
                case 1:
                    return Words[Rand.Next(0, 6)];
                case 2:
                    if (Game.CoveredDistancePercantage < 50)
                    {
                        return Words[Rand.Next(6, 10)];
                    }
                    else
                    {
                        return Words[Rand.Next(0, 10)];
                    }
                case 3:
                    if (Game.CoveredDistancePercantage < 50)
                    {
                        return Words[Rand.Next(10, 14)];
                    }
                    else if (Game.CoveredDistancePercantage < 75)
                    {
                        return Words[Rand.Next(5, 14)];
                    }
                    else
                    {
                        return Words[Rand.Next(0, 14)];
                    }
                case 4:
                    if (Game.CoveredDistancePercantage < 50)
                    {
                        return Words[Rand.Next(14, 17)];
                    }
                    else if (Game.CoveredDistancePercantage < 75)
                    {
                        return Words[Rand.Next(10, 17)];
                    }
                    else if (Game.CoveredDistancePercantage < 87)
                    {
                        return Words[Rand.Next(5, 17)];
                    }
                    else 
                    {
                        return Words[Rand.Next(0, 17)];
                    }
                case 5:
                    if (Game.CoveredDistancePercantage < 50)
                    {
                        return Words[Rand.Next(17, 20)];
                    }
                    else if (Game.CoveredDistancePercantage < 75)
                    {
                        return Words[Rand.Next(10, 20)];
                    }
                    else if (Game.CoveredDistancePercantage < 87)
                    {
                        return Words[Rand.Next(5, 20)];
                    }
                    else
                    {
                        return Words[Rand.Next(0, 20)];
                    }
                default:
                    return Words[Rand.Next(0, Words.Count)];
            }
        }
    }
}




