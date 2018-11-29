using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LG.XmlData;
using LG.Data;

namespace LG.pgDB_manipulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  AddThemes();
           // AddWords();
            AddEnglishWords();
        }
        void AddThemes()
        {
            DataLoaderFactory f = new DataLoaderFactory();
            IDataLoader loader = f.GetDataLoader(@"D:\JohnyPc\action\sketches\LGserver\LG.pgDB_manipulator\DLLs\data");
            Task<Theme> tt1 = loader.GetThemeById(new Language() { ID = 1, Name = "English" }, 2,10);
            tt1.ContinueWith(cont);
        }
        void AddWords()
        {
            DataLoaderFactory f = new DataLoaderFactory();
            IDataLoader loader = f.GetDataLoader(@"D:\JohnyPc\action\sketches\LGserver\LG.pgDB_manipulator\DLLs\data");
            Task<List<Word>> tt1 = loader.GetWordsByTheme(10, new Language() { ID = 1, Name = "English" });
            tt1.ContinueWith(InsertWords);
        }

        void AddEnglishWords()
        {
            DataLoaderFactory f = new DataLoaderFactory();
            IDataLoader loader = f.GetDataLoader(@"D:\JohnyPc\action\sketches\LGserver\LG.pgDB_manipulator\DLLs\data");
            Task<List<Word>> tt1 = loader.GetWordsByTheme(10, new Language() { ID = 1, Name = "English" });
            tt1.ContinueWith(InsertEnglishWords);
        }





        void InsertWords(Task<List<Word>> tt)
        {
            List<Word> words = tt.Result;

            using (Entities lg = new Entities())
            {
                foreach (Word w in words)
                {
                    word wd = new word()
                    {
                        id = (int)w.ID,
                        tid = 10,
                        pic_url = w.PictureUrl,
                        back_clr = w.BackColor.ToHexString().Substring(3),
                        border_clr = w.BorderColor.ToHexString().Substring(3),
                        left_clr = w.LeftColor.ToHexString().Substring(3),
                        top_clr = w.TopColor.ToHexString().Substring(3),
                        left_top_clr = w.LeftTopColor.ToHexString().Substring(3)
                    };
                    lg.words.AddObject(wd);
                }
                lg.SaveChanges();
            }
        }
    
        void cont(Task<Theme> tt)
        {
            Theme t = tt.Result;

            using (Entities lg = new Entities())
            {
                theme newTheme = new theme()
                {
                    id = (int)t.ID,
                    Name = t.Name,
                    Left = (int)t.Left,
                    Top = (int)t.Top,
                    Width = (int)t.Width,
                    Height = (int)t.Height,
                    Data = t.PathData
                };
                lg.themes.AddObject(newTheme);
                lg.SaveChanges();

                foreach (Symbol sb in t.NameInLetters)
                {
                    themes_english_letters let = new themes_english_letters() { tid = (int)t.ID, letter = sb.Value, left = (int)sb.Left, top = (int)sb.Top, rotation = (int)sb.Rotation };
                    lg.themes_english_letters.AddObject(let);
                }
                lg.SaveChanges();
                foreach (Word w in t.Words)
                {
                    themes_english_pictures pic = new themes_english_pictures() { tid = (int)t.ID,  wid  =(int) w.ID, left = (int)w.Left, top = (int)w.Top, rotation = (int)w.Rotation };

                    lg.themes_english_pictures.AddObject(pic);
                }
                lg.SaveChanges();
            }
        }

        void InsertEnglishWords(Task<List<Word>> tt)
        {
            List<Word> words = tt.Result;

            using (Entities lg = new Entities())
            {
                foreach (Word w in words)
                {
                    words_english we = new words_english() {    wid = (int)w.ID,
                                                                value = w.Value,
                                                                audio_url = w.AudioUrl
                                                            };
                    lg.words_english.AddObject(we);
                }
                lg.SaveChanges();
            }
        }
    }
}
