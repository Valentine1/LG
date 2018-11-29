using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGservices.ThemesUpdaterProxy;

namespace LGservices
{
    public class Converter
    {
        public List<LG.Data.Theme> ConvertToThemes(List<Theme> wsThemes)
        {
            List<LG.Data.Theme> themes = new List<LG.Data.Theme>();
            foreach (Theme wsTh in wsThemes)
            {
                LG.Data.Theme th = this.ConvertToTheme(wsTh);
                themes.Add(th);
            }
            return themes;
        }

        public LG.Data.Theme ConvertToTheme(Theme wsTh)
        {

            LG.Data.Theme th = new LG.Data.Theme()
            {
                ID = wsTh.ID,
                Name = wsTh.Name,
                Left = wsTh.Left,
                Top = wsTh.Top,
                Width = wsTh.Width,
                Height = wsTh.Height,
                PathData = wsTh.PathData,
                NameInLetters = this.ConvertToNameInLetters(wsTh.NameInLetters),
                NativeName = this.GetName(wsTh.NameInLetters),
                IsResourcesLoaded = false,
                Words = this.ConvertToWords(wsTh.Words)
            };
            return th;
        }

        public Language ConvertToWsLanguage(LG.Data.Language l)
        {
            Language WsLang = new Language() { ID = l.ID, Name = l.Name };
            return WsLang;
        }

        public ProfileDAL ConvertToWsProfile(LG.Data.ProfileDAL pdal)
        {
            ProfileDAL p = new ProfileDAL()
            {
                ID = pdal.ID,
                Name = pdal.Name,
                Password = pdal.Password,
                TimePlayed = pdal.TimePlayed,
                ContactInfo = pdal.ContactInfo,
                HasPhoto = pdal.HasPhoto,
                Picture = pdal.Picture,
                PictureUrl = pdal.PictureUrl
            };
            return p;
        }

        public Theme ConvertToWsTheme(LG.Data.Theme th)
        {
            Theme wsTh = new Theme()
            {
                Name = th.Name,
                HLevel = new Level()
                {
                    BestTimeResult = th.HLevel.BestTimeResult,
                    Number = th.HLevel.Number
                }
            };
            return wsTh;
        }

        public LG.Data.LevelProfiles ConvertToLevelProfiles(LevelProfiles lp)
        {
            LG.Data.LevelProfiles lps = new LG.Data.LevelProfiles(){ TotalLevelPlayers = lp.TotalLevelPlayers};
            lps.Profiles = new List<LG.Data.ProfileDAL>();
            foreach (ProfileDAL pd in lp.Profiles)
            {
                lps.Profiles.Add(this.ConvertToProfile(pd));
            }
            return lps;
        }

        private LG.Data.ProfileDAL ConvertToProfile(LGservices.ThemesUpdaterProxy.ProfileDAL pd)
        {
            LG.Data.ProfileDAL p = new LG.Data.ProfileDAL()
            {
                ID = pd.ID,
                Name = pd.Name,
                PictureUrl = pd.PictureUrl,
                ContactInfo = pd.ContactInfo,
                Type = 1,
                TimePlayed = pd.TimePlayed,
                BestTime = pd.BestTime,
                HasPhoto = pd.HasPhoto
            };
            return p;
        }

        private List<LG.Data.Symbol> ConvertToNameInLetters(List<Symbol> wsSymbols)
        {
            List<LG.Data.Symbol> symbols = new List<LG.Data.Symbol>();
            if (wsSymbols != null)
            {
                foreach (Symbol wsSym in wsSymbols)
                {
                    LG.Data.Symbol symbol = this.ConvertToSymbol(wsSym);
                    symbols.Add(symbol);
                }
            }
            return symbols;
        }
        private string GetName(List<Symbol> wsSymbols)
        {
            StringBuilder sb = new StringBuilder();
            if (wsSymbols != null)
            {
                foreach (Symbol wsSym in wsSymbols)
                {
                    sb.Append(wsSym.Value);
                }
            }
            return sb.ToString();
        }
        private LG.Data.Symbol ConvertToSymbol(Symbol wsSymb)
        {
            LG.Data.Symbol symbol = new LG.Data.Symbol() { Value = wsSymb.Value, Left = wsSymb.Left, Top = wsSymb.Top, Rotation = wsSymb.Rotation };
            return symbol;
        }

        private List<LG.Data.Word> ConvertToWords(List<Word> wsWords)
        {
            List<LG.Data.Word> words = new List<LG.Data.Word>();
            foreach (Word wsW in wsWords)
            {
                LG.Data.Word w = this.ConvertToWord(wsW);
                words.Add(w);
            }
            return words;
        }

        private LG.Data.Word ConvertToWord(Word wsW)
        {
            LG.Data.Word w = new LG.Data.Word()
            {
                ID = wsW.ID,
                PictureUrl = wsW.PictureUrl,
                Left = wsW.Left,
                Top = wsW.Top,
                Rotation = wsW.Rotation,
                BackColor = this.ConvertToColor(wsW.BackColor),
                BorderColor = this.ConvertToColor(wsW.BorderColor),
                LeftColor = this.ConvertToColor(wsW.LeftColor),
                TopColor = this.ConvertToColor(wsW.TopColor),
                LeftTopColor = this.ConvertToColor(wsW.LeftTopColor),
                Value = wsW.Value,
                AudioUrl = wsW.AudioUrl
            };
            return w;
        }

        private LG.Common.Color ConvertToColor(Color wsClr)
        {
            LG.Common.Color clr = new LG.Common.Color() { A = wsClr.A, B = wsClr.B, G = wsClr.G, R = wsClr.R };
            return clr;
        }
    }
}
