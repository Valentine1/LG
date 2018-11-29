using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Common;
using LG.Data;

namespace LG.XmlData
{
    //<ProfTh PId="1" TId="1" LevNo="1" bTime="65235" />
    public class ProfileTheme
    {
        public int PId { get; set; }
        public int TId { get; set; }
        public int LevNo { get; set; }
        public int bTime { get; set; }
    }

    public partial class DataLoader : IDataLoader, IProfileDataLoader
    {
        private string DirPath { get; set; }
        public DataLoader(string dirPath)
        {
            DirPath = dirPath;
        }

        async public Task<List<Language>> GetLanguages()
        {
            XDocument xDoc = await this.LoadAsync(this.DirPath + "\\" + DataNames.Languages);
            var lanuages = from lang in xDoc.Descendants("Lang")
                           select new Language()
                           {
                               ID = (uint)lang.Attributes("Id").FirstOrDefault(),
                               Name = (string)lang.Attributes("Name").FirstOrDefault()
                           };
            return lanuages.ToList();
        }

        async public Task<List<Theme>> GetThemesByLang(Language lang, int profId)
        {
            XDocument xDocThemes = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            XDocument xDocThemesToLanuages = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLanguages);
            XDocument xDocThemesLang = await this.LoadAsync(this.DirPath + "\\" + Path.GetFileNameWithoutExtension(DataNames.Themes) + lang.Name + ".xml");
            XDocument xDocThemesLicences = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLicences);
            XDocument xDocHier = await this.LoadAsync(this.DirPath + "\\" + DataNames.Hierarchy);
            var profThemes = (from p in xDocThProf.Descendants("ProfTh") where (int)p.Attribute("PId") == profId select p);

            var themes = (from th in xDocThemes.Descendants("Theme")
                          join tProf in profThemes on (int)th.Attribute("Id") equals (int)tProf.Attribute("TId") into ttprof
                          from ttp in ttprof.DefaultIfEmpty()
                          join hier in xDocHier.Descendants("Level") on (ttp == null ? 1 : (int)ttp.Attribute("LevNo")) equals (int)hier.Attribute("No")
                          join tLic in xDocThemesLicences.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tLic.Attribute("TId")
                          join tToLangs in xDocThemesToLanuages.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tToLangs.Attribute("ThId")
                          join tLangs in xDocThemesLang.Descendants("Theme") on (uint)th.Attribute("Id") equals (uint)tLangs.Attribute("TId")
                          where (uint)tToLangs.Attribute("LId") == lang.ID && (uint)tLic.Attribute("LId") == lang.ID
                          select new Theme()
                          {
                              ID = (int)th.Attributes("Id").FirstOrDefault(),
                              Name = (string)th.Attributes("Name").FirstOrDefault(),
                              IsResourcesLoaded = (int)th.Attributes("IsLoaded").FirstOrDefault() == 1 ? true : false,
                              IsPreviewsLoaded = (int)th.Attributes("IsPreviewLoaded").FirstOrDefault() == 1 ? true : false,
                              NativeName = (string)tLangs.Attributes("Name").FirstOrDefault(),
                              HLevel = new Level()
                                               {
                                                   Name = (string)hier.Attribute("Name"),
                                                   Number = (int)hier.Attribute("No"),
                                                   BestTimeResult = (ttp == null ? 0 : (int)ttp.Attribute("bTime")),
                                                   PictureUrl = (string)hier.Attribute("PicUrl"),
                                                   TopColor = new Color((string)hier.Attribute("TopClr")),
                                                   BottomColor = new Color((string)hier.Attribute("BottomClr"))
                                               },
                              Width = (double)th.Attributes("Width").FirstOrDefault(),
                              Height = (double)th.Attributes("Height").FirstOrDefault(),
                              Left = (double)th.Attributes("Left").FirstOrDefault(),
                              Top = (double)th.Attributes("Top").FirstOrDefault(),
                              PathData = (string)th.Attributes("Data").FirstOrDefault(),
                              //LicenseInfo = new License() { ID = (uint)lic.Attribute("Id"), Name = (string)lic.Attribute("Name") },
                              NameInLetters = tLangs.Descendants("Letter").Select(a => new Symbol()
                              {
                                  Value = (string)a.Attribute("Val"),
                                  Left = (double)a.Attribute("Left"),
                                  Top = (double)a.Attribute("Top"),
                                  Rotation = (double)a.Attribute("Rot")
                              }).ToList(),
                              Words = tLangs.Descendants("Pic").Select(w => new Word()
                             {
                                 ID = (int)w.Attribute("WId"),
                                 Left = (double)w.Attribute("Left"),
                                 Top = (double)w.Attribute("Top"),
                                 Rotation = (double)w.Attribute("Rot")
                             }).ToList()
                          }).ToList();
            return themes.ToList();
        }
        async public Task<List<Theme>> GetThemesByLang(Language lang, int profId, List<int> ids)
        {
            XDocument xDocThemes = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            XDocument xDocThemesToLanuages = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLanguages);
            XDocument xDocThemesLang = await this.LoadAsync(this.DirPath + "\\" + Path.GetFileNameWithoutExtension(DataNames.Themes) + lang.Name + ".xml");
            XDocument xDocThemesLicences = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLicences);
            XDocument xDocHier = await this.LoadAsync(this.DirPath + "\\" + DataNames.Hierarchy);
            var profThemes = (from p in xDocThProf.Descendants("ProfTh") where (int)p.Attribute("PId") == profId select p);

            var themes = (from th in xDocThemes.Descendants("Theme")
                          join tProf in profThemes on (int)th.Attribute("Id") equals (int)tProf.Attribute("TId") into ttprof
                          where ids.Contains((int)th.Attribute("Id"))
                          from ttp in ttprof.DefaultIfEmpty()
                          join hier in xDocHier.Descendants("Level") on (ttp == null ? 1 : (int)ttp.Attribute("LevNo")) equals (int)hier.Attribute("No")
                          join tLic in xDocThemesLicences.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tLic.Attribute("TId")
                          //join lic in xDocLicences.Descendants("Lic") on (uint)tLic.Attribute("LId") equals (uint)lic.Attribute("Id")
                          join tToLangs in xDocThemesToLanuages.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tToLangs.Attribute("ThId")
                          join tLangs in xDocThemesLang.Descendants("Theme") on (uint)th.Attribute("Id") equals (uint)tLangs.Attribute("TId")
                          where (uint)tToLangs.Attribute("LId") == lang.ID && (uint)tLic.Attribute("LId") == lang.ID
                          select new Theme()
                          {
                              ID = (int)th.Attributes("Id").FirstOrDefault(),
                              Name = (string)th.Attributes("Name").FirstOrDefault(),
                              IsResourcesLoaded = (int)th.Attributes("IsLoaded").FirstOrDefault() == 1 ? true : false,
                              IsPreviewsLoaded = (int)th.Attributes("IsPreviewLoaded").FirstOrDefault() == 1 ? true : false,
                              NativeName = (string)tLangs.Attributes("Name").FirstOrDefault(),
                              HLevel = new Level()
                              {
                                  Name = (string)hier.Attribute("Name"),
                                  Number = (int)hier.Attribute("No"),
                                  BestTimeResult = (ttp == null ? 0 : (int)ttp.Attribute("bTime")),
                                  PictureUrl = (string)hier.Attribute("PicUrl"),
                                  TopColor = new Color((string)hier.Attribute("TopClr")),
                                  BottomColor = new Color((string)hier.Attribute("BottomClr"))
                              },
                              Width = (double)th.Attributes("Width").FirstOrDefault(),
                              Height = (double)th.Attributes("Height").FirstOrDefault(),
                              Left = (double)th.Attributes("Left").FirstOrDefault(),
                              Top = (double)th.Attributes("Top").FirstOrDefault(),
                              PathData = (string)th.Attributes("Data").FirstOrDefault(),
                              //LicenseInfo = new License() { ID = (uint)lic.Attribute("Id"), Name = (string)lic.Attribute("Name") },
                              NameInLetters = tLangs.Descendants("Letter").Select(a => new Symbol()
                              {
                                  Value = (string)a.Attribute("Val"),
                                  Left = (double)a.Attribute("Left"),
                                  Top = (double)a.Attribute("Top"),
                                  Rotation = (double)a.Attribute("Rot")
                              }).ToList(),
                              Words = tLangs.Descendants("Pic").Select(w => new Word()
                              {
                                  ID = (int)w.Attribute("WId"),
                                  Left = (double)w.Attribute("Left"),
                                  Top = (double)w.Attribute("Top"),
                                  Rotation = (double)w.Attribute("Rot")
                              }).ToList()
                          }).ToList();
            return themes.ToList();
        }
        async public Task<Theme> GetThemeById(Language lang, int profId, int themeId)
        {
            XDocument xDocThemes = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            XDocument xDocThemesToLanuages = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLanguages);
            XDocument xDocThemesLangs = await this.LoadAsync(this.DirPath + "\\" + Path.GetFileNameWithoutExtension(DataNames.Themes) + lang.Name + ".xml");
            XDocument xDocThemesLicences = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLicences);
            XDocument xDocHier = await this.LoadAsync(this.DirPath + "\\" + DataNames.Hierarchy);

            var prof = (from p in xDocThProf.Descendants("ProfTh") where (int)p.Attribute("PId") == profId && (int)p.Attribute("TId") == themeId select p).ToList(); ;
            if (prof.Count == 0)
            {
                var themes = (from th in xDocThemes.Descendants("Theme")
                              join hier in xDocHier.Descendants("Level") on 1 equals (int)hier.Attribute("No")
                              join tLic in xDocThemesLicences.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tLic.Attribute("TId")
                              join tToLangs in xDocThemesToLanuages.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tToLangs.Attribute("ThId")
                              join tLangs in xDocThemesLangs.Descendants("Theme") on (uint)th.Attribute("Id") equals (uint)tLangs.Attribute("TId")
                              where (uint)tToLangs.Attribute("LId") == lang.ID && (int)th.Attribute("Id") == themeId && (uint)tLic.Attribute("LId") == lang.ID
                              select new Theme()
                              {
                                  ID = (int)th.Attributes("Id").FirstOrDefault(),
                                  Name = (string)th.Attributes("Name").FirstOrDefault(),
                                  IsResourcesLoaded = (int)th.Attributes("IsLoaded").FirstOrDefault() == 1 ? true : false,
                                  NativeName = (string)tLangs.Attributes("Name").FirstOrDefault(),
                                  HLevel = new Level()
                                  {
                                      Name = (string)hier.Attribute("Name"),
                                      Number = (int)hier.Attribute("No"),
                                      BestTimeResult = 0,
                                      PictureUrl = (string)hier.Attribute("PicUrl"),
                                      TopColor = new Color((string)hier.Attribute("TopClr")),
                                      BottomColor = new Color((string)hier.Attribute("BottomClr"))
                                  },
                                  Width = (double)th.Attributes("Width").FirstOrDefault(),
                                  Height = (double)th.Attributes("Height").FirstOrDefault(),
                                  Left = (double)th.Attributes("Left").FirstOrDefault(),
                                  Top = (double)th.Attributes("Top").FirstOrDefault(),
                                  PathData = (string)th.Attributes("Data").FirstOrDefault(),
                                  // LicenseInfo = new License() { ID = (uint)lic.Attribute("Id"), Name = (string)lic.Attribute("Name") },
                                  NameInLetters = tLangs.Descendants("Letter").Select(a => new Symbol()
                                  {
                                      Value = (string)a.Attribute("Val"),
                                      Left = (double)a.Attribute("Left"),
                                      Top = (double)a.Attribute("Top"),
                                      Rotation = (double)a.Attribute("Rot")
                                  }).ToList(),
                                  Words = tLangs.Descendants("Pic").Select(w => new Word()
                                  {
                                      ID = (int)w.Attribute("WId"),
                                      Left = (double)w.Attribute("Left"),
                                      Top = (double)w.Attribute("Top"),
                                      Rotation = (double)w.Attribute("Rot")
                                  }).ToList()
                              }).ToList();
                return themes.FirstOrDefault();
            }
            else
            {
                var themes = (from th in xDocThemes.Descendants("Theme")
                              join tProf in prof on (int)th.Attribute("Id") equals (int)tProf.Attribute("TId")
                              join hier in xDocHier.Descendants("Level") on ((int)tProf.Attribute("LevNo")) equals (int)hier.Attribute("No")
                              join tLic in xDocThemesLicences.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tLic.Attribute("TId")
                              join tToLangs in xDocThemesToLanuages.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tToLangs.Attribute("ThId")
                              join tLangs in xDocThemesLangs.Descendants("Theme") on (uint)th.Attribute("Id") equals (uint)tLangs.Attribute("TId")
                              where (uint)tToLangs.Attribute("LId") == lang.ID && (int)th.Attribute("Id") == themeId && (uint)tLic.Attribute("LId") == lang.ID
                              select new Theme()
                              {
                                  ID = (int)th.Attributes("Id").FirstOrDefault(),
                                  Name = (string)th.Attributes("Name").FirstOrDefault(),
                                  IsResourcesLoaded = (int)th.Attributes("IsLoaded").FirstOrDefault() == 1 ? true : false,
                                  NativeName = (string)tLangs.Attributes("Name").FirstOrDefault(),
                                  HLevel = new Level()
                                  {
                                      Name = (string)hier.Attribute("Name"),
                                      Number = (int)hier.Attribute("No"),
                                      BestTimeResult = ((int)tProf.Attribute("bTime")),
                                      PictureUrl = (string)hier.Attribute("PicUrl"),
                                      TopColor = new Color((string)hier.Attribute("TopClr")),
                                      BottomColor = new Color((string)hier.Attribute("BottomClr"))
                                  },
                                  Width = (double)th.Attributes("Width").FirstOrDefault(),
                                  Height = (double)th.Attributes("Height").FirstOrDefault(),
                                  Left = (double)th.Attributes("Left").FirstOrDefault(),
                                  Top = (double)th.Attributes("Top").FirstOrDefault(),
                                  PathData = (string)th.Attributes("Data").FirstOrDefault(),
                                  // LicenseInfo = new License() { ID = (uint)lic.Attribute("Id"), Name = (string)lic.Attribute("Name") },
                                  NameInLetters = tLangs.Descendants("Letter").Select(a => new Symbol()
                                  {
                                      Value = (string)a.Attribute("Val"),
                                      Left = (double)a.Attribute("Left"),
                                      Top = (double)a.Attribute("Top"),
                                      Rotation = (double)a.Attribute("Rot")
                                  }).ToList(),
                                  Words = tLangs.Descendants("Pic").Select(w => new Word()
                                  {
                                      ID = (int)w.Attribute("WId"),
                                      Left = (double)w.Attribute("Left"),
                                      Top = (double)w.Attribute("Top"),
                                      Rotation = (double)w.Attribute("Rot")
                                  }).ToList()
                              }).ToList();
                return themes.FirstOrDefault();
            }
        }

        async public Task<List<Theme>> GetThemesByIds(Language lang, List<int> themeIds)
        {
            XDocument xDocThemes = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            XDocument xDocThemesToLanuages = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLanguages);
            XDocument xDocThemesLicences = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLicences);
            XDocument xDocWords = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");
            XDocument xDocWordsLang = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + lang.Name + ".xml");

            var themes = (from th in xDocThemes.Descendants("Theme")
                          join tLic in xDocThemesLicences.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tLic.Attribute("TId")
                          join tToLangs in xDocThemesToLanuages.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tToLangs.Attribute("ThId")
                          join wt in xDocWords.Descendants("Theme") on (int)th.Attribute("Id") equals (int)wt.Attribute("TId")
                          join wet in xDocWordsLang.Descendants("Theme") on (int)wt.Attribute("TId") equals (int)wet.Attribute("TId")
                          where (uint)tToLangs.Attribute("LId") == lang.ID && themeIds.Contains((int)th.Attribute("Id")) && (uint)tLic.Attribute("LId") == lang.ID
                          select new Theme()
                          {
                              ID = (int)th.Attributes("Id").FirstOrDefault(),
                              Name = (string)th.Attributes("Name").FirstOrDefault(),
                              IsPreviewMode = (int)th.Attributes("IsPreviewMode").FirstOrDefault() == 1 ? true : false,
                              IsPreviewsLoaded = (int)th.Attributes("IsPreviewLoaded").FirstOrDefault() == 1 ? true : false,
                              IsResourcesLoaded = (int)th.Attributes("IsLoaded").FirstOrDefault() == 1 ? true : false,
                              Words = (from w in wt.Descendants("Word")
                                       join we in wet.Descendants("W") on w.Attribute("Id") equals we.Attribute("TId")
                                       select new Word()
                                       {
                                           PictureUrl = (string)w.Attribute("PicUrl"),
                                           AudioUrl = (string)we.Attribute("AudioUrl")
                                       }
                                       ).ToList()
                          }
                         ).ToList();

            return themes;
        }
        async public Task<List<Word>> GetPreviewThemeWordsById(Language lang, int themeId)
        {
            XDocument xDocThemes = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            XDocument xDocThemesToLanuages = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLanguages);
            XDocument xDocThemesLangs = await this.LoadAsync(this.DirPath + "\\" + Path.GetFileNameWithoutExtension(DataNames.Themes) + lang.Name + ".xml");
            XDocument xDocThemesLicences = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLicences);
            XDocument xDocWords = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");

            List<int> wids = (from tLangs in xDocThemesLangs.Descendants("Theme")
                              where (int)tLangs.Attribute("TId") == themeId
                              select tLangs.Descendants("Pic").Select(w => (int)w.Attribute("WId")
                                                         ).ToList()).FirstOrDefault();
            var tw = xDocWords.Descendants("Theme").Where(t => (int)t.Attribute("TId") == themeId).Select(t => t);
            List<Word> words = tw.Descendants("Word").Where(w => wids.Contains((int)w.Attribute("Id"))).Select(w => new Word()
            {
                PictureUrl = (string)w.Attribute("PicUrl")
            }).ToList();


            return words;
        }
        async public Task<List<Theme>> GetThemesByLangWithoutChildren(Language lang)
        {
            XDocument xDocThemes = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            XDocument xDocThemesToLanuages = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLanguages);
            var themes = from th in xDocThemes.Descendants("Theme")
                         join tToLangs in xDocThemesToLanuages.Descendants("TL") on (uint)th.Attribute("Id") equals (uint)tToLangs.Attribute("ThId")
                         where (uint)tToLangs.Attribute("LId") == lang.ID
                         select new Theme()
                         {
                             ID = (int)th.Attributes("Id").FirstOrDefault(),
                             Name = (string)th.Attributes("Name").FirstOrDefault(),
                             IsPreviewMode = (int)th.Attributes("IsPreviewMode").FirstOrDefault() == 1 ? true : false,
                             IsPreviewsLoaded = (int)th.Attributes("IsPreviewLoaded").FirstOrDefault() == 1 ? true : false,
                             IsResourcesLoaded = (int)th.Attributes("IsLoaded").FirstOrDefault() == 1 ? true : false
                         };

            return themes.ToList();
        }

        async public Task AddTheme(Theme theme, Language lang)
        {
            XDocument xDoc = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            xDoc.Element("Themes").Add(new XElement("Theme", new XAttribute("Id", theme.ID.ToString()), new XAttribute("Name", theme.Name)));
            await this.Save(xDoc, this.DirPath + "\\" + DataNames.Themes); //<TL ThId="2" LId="1" />
            XDocument xDocThemesLang = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLanguages);
            xDocThemesLang.Element("ThemesToLanguages").Add(new XElement("TL", new XAttribute("ThId", theme.ID.ToString()), new XAttribute("LId", lang.ID)));
            await this.Save(xDocThemesLang, this.DirPath + "\\" + DataNames.ThemesLanguages);
            XDocument xDocWords = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");
            xDocWords.Element("Themes").Add(new XElement("Theme", new XAttribute("TId", theme.ID.ToString())));
            await this.Save(xDocWords, this.DirPath + "\\" + DataNames.Words + ".xml");

            XDocument xDocWordsLang = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + lang.Name + ".xml");
            xDocWordsLang.Element("WordsTo" + lang.Name).Add(new XElement("Theme", new XAttribute("TId", theme.ID.ToString())));
            await this.Save(xDocWordsLang, this.DirPath + "\\" + DataNames.Words + lang.Name + ".xml");
        }
        async public Task AddThemes(List<Theme> themes, Language lang)
        {
            XDocument xDoc = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            XDocument xDocThemesToLang = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLanguages);
            XDocument xDocThLang = await this.LoadAsync(this.DirPath + "\\" + Path.GetFileNameWithoutExtension(DataNames.Themes) + lang.Name + ".xml");
            XDocument xDocThemesLicences = await this.LoadAsync(this.DirPath + "\\" + DataNames.ThemesLicences);
            XDocument xDocWords = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");

            foreach (Theme th in themes)
            {
                xDoc.Element("Themes").Add(new XElement("Theme", new XAttribute("Id", th.ID.ToString()),
                                                                 new XAttribute("Name", th.Name),
                                                                 new XAttribute("Width", th.Width.ToString()),
                                                                 new XAttribute("Height", th.Height.ToString()),
                                                                 new XAttribute("Left", th.Left.ToString()),
                                                                 new XAttribute("Top", th.Top.ToString()),
                                                                 new XAttribute("Data", th.PathData),
                                                                 new XAttribute("IsPreviewMode", 1),
                                                                 new XAttribute("IsPreviewLoaded", 1),
                                                                 new XAttribute("IsLoaded", 0)));
                xDocThemesToLang.Element("ThemesToLanguages").Add(new XElement("TL", new XAttribute("ThId", th.ID.ToString()),
                                                                                     new XAttribute("LId", lang.ID)));
                xDocThemesLicences.Element("ThemesToLicences").Add(new XElement("TL", new XAttribute("TId", th.ID.ToString()),
                                                                                      new XAttribute("LId", lang.ID)));

                XElement thElem = new XElement("Theme", new XAttribute("TId", th.ID.ToString()),
                                                        new XAttribute("Name", th.NativeName));

                foreach (Symbol symb in th.NameInLetters)
                {
                    thElem.Add(new XElement("Letter", new XAttribute("Val", symb.Value),
                                                      new XAttribute("Left", symb.Left.ToString()),
                                                      new XAttribute("Top", symb.Top.ToString()),
                                                      new XAttribute("Rot", symb.Rotation.ToString())));
                }
                foreach (Word w in th.Words)
                {

                    thElem.Add(new XElement("Pic", new XAttribute("WId", w.ID.ToString()),
                                                   new XAttribute("Left", w.Left.ToString()),
                                                   new XAttribute("Top", w.Top.ToString()),
                                                   new XAttribute("Rot", w.Rotation.ToString())));
                }

                xDocThLang.Element("Themes").Add(thElem);

                XElement thElem2 = new XElement("Theme", new XAttribute("TId", th.ID.ToString()));
                foreach (Word w in th.Words)
                {

                    thElem2.Add(new XElement("Word", new XAttribute("Id", w.ID.ToString()),
                                                   new XAttribute("PicUrl", w.PictureUrl),
                                                   new XAttribute("BackClr", w.BackColor.ToHexString()),
                                                   new XAttribute("BorderClr", w.BorderColor.ToHexString()),
                                                   new XAttribute("LeftClr", w.LeftColor.ToHexString()),
                                                   new XAttribute("TopClr", w.TopColor.ToHexString()),
                                                   new XAttribute("LeftTopClr", w.LeftTopColor.ToHexString())));
                }
                xDocWords.Element("Themes").Add(thElem2);
            }

            await this.Save(xDoc, this.DirPath + "\\" + DataNames.Themes);
            await this.Save(xDocThemesToLang, this.DirPath + "\\" + DataNames.ThemesLanguages);
            await this.Save(xDocThLang, this.DirPath + "\\" + Path.GetFileNameWithoutExtension(DataNames.Themes) + lang.Name + ".xml");
            await this.Save(xDocThemesLicences, this.DirPath + "\\" + DataNames.ThemesLicences);
            await this.Save(xDocWords, this.DirPath + "\\" + DataNames.Words + ".xml");
        }
        async public Task UpdateThemeIsResourcesLoaded(Theme th)
        {
            XDocument xDocThemes = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            var theme = (from t in xDocThemes.Descendants("Theme") where (int)t.Attribute("Id") == th.ID select t).FirstOrDefault();
            theme.Attribute("IsLoaded").Value = "1";
            await this.Save(xDocThemes, this.DirPath + "\\" + DataNames.Themes);
        }
        async public Task UpdateThemesIsResourcesLoaded(List<Theme> themes)
        {
            XDocument xDoc = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            List<int> ids = (from t in themes select t.ID).ToList();
            var ethemes = (from t in xDoc.Descendants("Theme") where ids.Contains((int)t.Attribute("Id")) select t).ToList();

            foreach (XElement th in ethemes)
            {
                th.Attribute("IsLoaded").Value = "1";
            }
            await this.Save(xDoc, this.DirPath + "\\" + DataNames.Themes);
        }
        async public Task UpdateThemeIsPreviewsLoaded(Theme th)
        {
            XDocument xDocThemes = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            var theme = (from t in xDocThemes.Descendants("Theme") where (int)t.Attribute("Id") == th.ID select t).FirstOrDefault();
            theme.Attribute("IsPreviewLoaded").Value = "1";
            await this.Save(xDocThemes, this.DirPath + "\\" + DataNames.Themes);
        }
        async public Task UpdateThemesWordsAndIsPreviewMode(List<Theme> themes, Language lang)
        {
            XDocument xDoc = await this.LoadAsync(this.DirPath + "\\" + DataNames.Themes);
            XDocument xDocWords = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");
            XDocument xDocWordsLang = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + lang.Name + ".xml");
            List<int> ids = (from t in themes select t.ID).ToList();
            var ethemes = (from t in xDoc.Descendants("Theme") where ids.Contains((int)t.Attribute("Id")) select t).ToList();

            foreach (XElement th in ethemes)
            {
                th.Attribute("IsPreviewMode").Value = "0";
            }

            var ethemewords = (from t in xDocWords.Descendants("Theme") where ids.Contains((int)t.Attribute("TId")) select t).ToList();
            foreach (XElement ethw in ethemewords)
            {
                ethw.Elements().Remove();
                Theme th = (from t in themes where t.ID == (int)ethw.Attribute("TId") select t).FirstOrDefault();
                XElement ethwordseng = new XElement("Theme", new XAttribute("TId", th.ID.ToString()));
                foreach (Word w in th.Words)
                {
                    ethw.Add(new XElement("Word", new XAttribute("Id", w.ID.ToString()),
                                                    new XAttribute("PicUrl", w.PictureUrl),
                                                    new XAttribute("BackClr", w.BackColor.ToHexString()),
                                                    new XAttribute("BorderClr", w.BorderColor.ToHexString()),
                                                    new XAttribute("LeftClr", w.LeftColor.ToHexString()),
                                                    new XAttribute("TopClr", w.TopColor.ToHexString()),
                                                    new XAttribute("LeftTopClr", w.LeftTopColor.ToHexString())));
                    ethwordseng.Add(new XElement("W", new XAttribute("WId", w.ID.ToString()),
                                                      new XAttribute("Val", w.Value),
                                                      new XAttribute("AudioUrl", w.AudioUrl)));
                }
                xDocWordsLang.Element("WordsTo" + lang.Name).Add(ethwordseng);
            }
            await this.Save(xDocWords, this.DirPath + "\\" + DataNames.Words + ".xml");
            await this.Save(xDocWordsLang, this.DirPath + "\\" + DataNames.Words + lang.Name + ".xml");
            await this.Save(xDoc, this.DirPath + "\\" + DataNames.Themes);
        }

        async public Task<Dictionary<int, Word>> GetWords(Language lang)
        {
            XDocument xDocTh = await this.LoadAsync(this.DirPath + "\\" + Path.GetFileNameWithoutExtension(DataNames.Themes) + lang.Name + ".xml");
            XDocument xDocW = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");

            var data = from p in xDocTh.Descendants("Pic")
                       join w in xDocW.Descendants("Word") on (string)p.Attribute("WId") equals (string)w.Attribute("Id")
                       select new Word()
                       {
                           ID = (int)p.Attribute("WId"),
                           PictureUrl = (string)w.Attribute("PicUrl"),
                           BackColor = new Color((string)w.Attribute("BackClr")),
                           BorderColor = new Color((string)w.Attribute("BorderClr")),
                           LeftColor = new Color((string)w.Attribute("LeftClr")),
                           TopColor = new Color((string)w.Attribute("TopClr")),
                           LeftTopColor = new Color((string)w.Attribute("LeftTopClr")),
                       };

            return data.ToDictionary<Word, int>(w => w.ID);
        }
        async public Task<List<Word>> GetWordsByTheme(int themeId, Language lang, int skip, int take)
        {
            XDocument xDocWordsLang = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + lang.Name + ".xml");
            XDocument xDocWords = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");
            var th = (from t in xDocWords.Descendants("Theme") where (uint)t.Attribute("TId") == themeId select t).FirstOrDefault();
            var thwordsLang = (from t in xDocWordsLang.Descendants("Theme") where (uint)t.Attribute("TId") == themeId select t).FirstOrDefault();
            var data = (from w in th.Descendants("Word")
                        join wl in thwordsLang.Descendants("W") on (uint)w.Attribute("Id") equals (uint)wl.Attribute("WId")
                        select new Word()
                        {
                            ID = (int)w.Attribute("Id"),
                            Value = (string)wl.Attribute("Val"),
                            AudioUrl = (string)wl.Attribute("AudioUrl"),
                            PictureUrl = (string)w.Attribute("PicUrl"),
                            BackColor = new Color((string)w.Attribute("BackClr")),
                            BorderColor = new Color((string)w.Attribute("BorderClr")),
                            LeftColor = new Color((string)w.Attribute("LeftClr")),
                            TopColor = new Color((string)w.Attribute("TopClr")),
                            LeftTopColor = new Color((string)w.Attribute("LeftTopClr")),

                        }).Skip(skip).Take(take);

            return data.ToList();
        }
        async public Task<int> GetWordsCount()
        {
            XDocument xDocW = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");

            var count = (from w in xDocW.Descendants("Word")
                         select w).Count();

            return count;
        }
        async public Task AddWord(uint themeId, Word w, Language lang)
        {
            XDocument xDocWordsLang = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + lang.Name + ".xml");
            XDocument xDocWords = await this.LoadAsync(this.DirPath + "\\" + DataNames.Words + ".xml");
            var themeW = (from item in xDocWords.Descendants("Theme") where (uint)item.Attribute("TId") == themeId select item).FirstOrDefault();
            if (themeW != null)
            {
                themeW.Add(new XElement("Word", new XAttribute("Id", w.ID.ToString()),
                                                new XAttribute("PicUrl", w.PictureUrl),
                                                new XAttribute("BackClr", w.BackColor.ToHexString()),
                                                new XAttribute("BorderClr", w.BorderColor.ToHexString()),
                                                new XAttribute("LeftClr", w.LeftColor.ToHexString()),
                                                new XAttribute("TopClr", w.TopColor.ToHexString()),
                                                new XAttribute("LeftTopClr", w.LeftTopColor.ToHexString())));
            }
            var themeWL = (from item in xDocWordsLang.Descendants("Theme") where (uint)item.Attribute("TId") == themeId select item).FirstOrDefault();
            if (themeWL != null)
            {
                themeWL.Add(new XElement("W", new XAttribute("WId", w.ID.ToString()),
                                               new XAttribute("Val", w.Value),
                                               new XAttribute("AudioUrl", w.AudioUrl)));
            }

            this.Save(xDocWords, this.DirPath + "\\" + DataNames.Words + ".xml");
            this.Save(xDocWordsLang, this.DirPath + "\\" + DataNames.Words + lang.Name + ".xml");
        }

        async public Task<List<Level>> GetAllHierarchyLevels()
        {
            XDocument xDocHier = await this.LoadAsync(this.DirPath + "\\" + DataNames.Hierarchy);

            var levels = from hier in xDocHier.Descendants("Level")
                         select new Level()
                         {
                             Name = (string)hier.Attribute("Name"),
                             Number = (int)hier.Attribute("No"),
                             //  BestTimeResult = (int)th.Attributes("bTime").FirstOrDefault(),
                             PictureUrl = (string)hier.Attribute("PicUrl"),
                             TopColor = new Color((string)hier.Attribute("TopClr")),
                             BottomColor = new Color((string)hier.Attribute("BottomClr"))
                         };
            return levels.ToList();
        }
        async public Task<Level> GetLevelByNumber(int no)
        {
            XDocument xDocHier = await this.LoadAsync(this.DirPath + "\\" + DataNames.Hierarchy);
            var lev = (from hier in xDocHier.Descendants("Level")
                       where (int)hier.Attribute("No") == no
                       select
                           new Level()
                           {
                               Name = (string)hier.Attribute("Name"),
                               Number = (int)hier.Attribute("No"),
                               PictureUrl = (string)hier.Attribute("PicUrl"),
                               TopColor = new Color((string)hier.Attribute("TopClr")),
                               BottomColor = new Color((string)hier.Attribute("BottomClr"))
                           }).SingleOrDefault();
            return lev;
        }
        async public Task<bool> SaveLevelHierarchy(ProfileDAL prof, Theme th, Language lang)
        {
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            if (th.HLevel.Number == 1 || th.HLevel.Number == 2)
            {
                var profTheme = (from pt in xDocThProf.Descendants("ProfTh")
                                 where (int)pt.Attribute("PId") == prof.ID && (int)pt.Attribute("TId") == th.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    var profThemes = (from pt in xDocThProf.Descendants("ProfilesThemes") select pt).FirstOrDefault();
                    profThemes.Add(new XElement("ProfTh", new XAttribute("PId", prof.ID),
                                                       new XAttribute("TId", th.ID),
                                                       new XAttribute("LevNo", th.HLevel.Number),
                                                       new XAttribute("bTime", th.HLevel.BestTimeResult)
                                                  ));
                }
                else
                {
                    profTheme.Attribute("LevNo").Value = th.HLevel.Number.ToString();
                    profTheme.Attribute("bTime").Value = th.HLevel.BestTimeResult.ToString();
                }
            }
            else
            {
                var profTheme = (from t in xDocThProf.Descendants("ProfTh")
                                 where (int)t.Attribute("PId") == prof.ID && (uint)t.Attribute("TId") == th.ID
                                 select t).SingleOrDefault();
                profTheme.Attribute("LevNo").Value = th.HLevel.Number.ToString();
                profTheme.Attribute("bTime").Value = th.HLevel.BestTimeResult.ToString();
            }
            this.Save(xDocThProf, this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            XDocument xDocProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            var pr = (from p in xDocProf.Descendants("Profile") where (int)p.Attribute("Id") == prof.ID select p).SingleOrDefault();
            pr.Attribute("Time").Value = prof.TimePlayed.ToString();
            this.Save(xDocProf, this.DirPath + "\\" + DataNames.Profiles);
            return true;
        }

        async public Task<List<ProfileDAL>> GetProfiles()
        {
            XDocument xDocProfiles = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            var profiles = from p in xDocProfiles.Descendants("Profile")
                           select new ProfileDAL()
                           {
                               ID = (int)p.Attribute("Id"),
                               Name = (string)p.Attribute("Name"),
                               PictureUrl = (string)p.Attribute("PicUrl"),
                               Type = (int)p.Attribute("Type"),
                               Password = (string)p.Attribute("Pass"),
                               ContactInfo = (string)p.Attribute("ContactInfo"),
                               LastThemeID = (int)p.Attribute("LastTh"),
                               TimePlayed = (int)p.Attribute("Time"),
                               HasPhoto = (int)p.Attribute("IsPhoto") == 1 ? true : false
                           };

            return profiles.ToList();

        }
        async public Task<LevelProfiles> GetProfiles(Language lang, Theme th, int levNo, int skip, int take)
        {
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            XDocument xDocProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            var profiles = (from pt in xDocThProf.Descendants("ProfTh")
                            join p in xDocProf.Descendants("Profile") on (int)pt.Attribute("PId") equals (int)p.Attribute("Id")
                            where (int)pt.Attribute("TId") == th.ID && (int)pt.Attribute("LevNo") == levNo && (int)p.Attribute("Type") == 0
                            orderby (int)pt.Attribute("bTime")
                            select new ProfileDAL()
                            {
                                ID = (int)p.Attribute("Id"),
                                Name = (string)p.Attribute("Name"),
                                PictureUrl = (string)p.Attribute("PicUrl"),
                                ContactInfo = (string)p.Attribute("ContactInfo"),
                                Type = (int)p.Attribute("Type"),
                                TimePlayed = (int)p.Attribute("Time"),
                                BestTime = (int)pt.Attribute("bTime"),
                                HasPhoto = (int)p.Attribute("IsPhoto") == 1 ? true : false
                            }).Skip(skip).Take(take);
            int totalNo = (from pt in xDocThProf.Descendants("ProfTh")
                           where (int)pt.Attribute("TId") == th.ID && (int)pt.Attribute("LevNo") == levNo
                           select pt).Count();
            LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalNo };
            return lp;
        }

        async public Task<int> GetLastUserProfileId()
        {
            XDocument xDocProfiles = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            var lastuser = from u in xDocProfiles.Descendants("LastUser") select (int)u.Attributes("PId").FirstOrDefault();
            return lastuser.ToList()[0];

        }

        async public Task<int> AddProfile(ProfileDAL prof)
        {
            XDocument xDocProfiles = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            int minId = (from p in xDocProfiles.Descendants("Profile") select (int)p.Attribute("Id")).Min();

            var profiles = (from p in xDocProfiles.Descendants("Profiles") select p).FirstOrDefault();
            if (profiles != null)
            {
                profiles.Add(new XElement("Profile", new XAttribute("Id", (--minId).ToString()),
                                               new XAttribute("Name", prof.Name),
                                               new XAttribute("PicUrl", "NoPhoto.png"),
                                               new XAttribute("Type", prof.Type.ToString()),
                                               new XAttribute("Pass", prof.Password == null ? "" : prof.Password),
                                               new XAttribute("ContactInfo", ""),
                                               new XAttribute("LastTh", prof.LastThemeID.ToString()),
                                               new XAttribute("Time", prof.TimePlayed.ToString()),
                                               new XAttribute("IsPhoto", prof.HasPhoto ? "1" : "0")
                                          ));
            }
            await this.Save(xDocProfiles, this.DirPath + "\\" + DataNames.Profiles);
            return minId;
        }
        async public Task AddProfileWithReadyId(ProfileDAL prof)
        {
            XDocument xDocProfiles = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);

            var profiles = (from p in xDocProfiles.Descendants("Profiles") select p).FirstOrDefault();
            if (profiles != null)
            {
                profiles.Add(new XElement("Profile", new XAttribute("Id", prof.ID.ToString()),
                                               new XAttribute("Name", prof.Name),
                                               new XAttribute("PicUrl", prof.HasPhoto ? prof.PictureUrl : ""),
                                               new XAttribute("Type", prof.Type.ToString()),
                                               new XAttribute("Pass", prof.Password == null ? "" : prof.Password),
                                               new XAttribute("ContactInfo", prof.ContactInfo == null ? "" : prof.ContactInfo),
                                               new XAttribute("LastTh", prof.LastThemeID.ToString()),
                                               new XAttribute("Time", prof.TimePlayed.ToString()),
                                               new XAttribute("IsPhoto", prof.HasPhoto ? "1" : "0")
                                          ));
            }
            await this.Save(xDocProfiles, this.DirPath + "\\" + DataNames.Profiles);
        }
        async public Task<bool> UpdateProfile(ProfileDAL prof)
        {
            XDocument xDocProfiles = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            var profile = (from p in xDocProfiles.Descendants("Profile") where (int)p.Attribute("Id") == prof.ID select p).FirstOrDefault();
            profile.Attribute("Name").Value = prof.Name;
            profile.Attribute("Pass").Value = prof.Password == null ? "" : prof.Password;
            profile.Attribute("ContactInfo").Value = prof.ContactInfo == null ? "" : prof.ContactInfo;
            profile.Attribute("PicUrl").Value = prof.PictureUrl == null ? "" : prof.PictureUrl;
            profile.Attribute("IsPhoto").Value = prof.HasPhoto ? "1" : "0";
            await this.Save(xDocProfiles, this.DirPath + "\\" + DataNames.Profiles);
            return true;
        }
        async public Task<bool> DeleteProfile(int pid, Language lang)
        {
            XDocument xDocProfiles = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            var prof = (from p in xDocProfiles.Descendants("Profile") where (int)p.Attribute("Id") == pid select p).FirstOrDefault();
            prof.Remove();
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            var pthemes = (from pt in xDocThProf.Descendants("ProfTh") where (int)pt.Attribute("PId") == pid select pt).ToList();
            foreach (XElement e in pthemes)
            {
                e.Remove();
            }
            await this.Save(xDocProfiles, this.DirPath + "\\" + DataNames.Profiles);
            await this.Save(xDocThProf, this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            return true;
        }
        async public Task CheckIfExistAndAddProfileTheme(Language lang, int profId, int themeId)
        {
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            var profTheme = (from pt in xDocThProf.Descendants("ProfTh")
                             where (int)pt.Attribute("PId") == profId && (int)pt.Attribute("TId") == themeId
                             select pt).SingleOrDefault();
            if (profTheme == null)
            {
                this.AddProfileTheme(new ProfileTheme() { PId = profId, TId = themeId, LevNo = 1, bTime = 0 }, lang);
            }
        }
        async public Task<bool?> CheckIfBecomesOrRemainsAllSeeing(Language lang, int profId, Theme theme, int bTime)
        {
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            ProfileDAL p = (from pt in xDocThProf.Descendants("ProfTh")
                            where (int)pt.Attribute("TId") == theme.ID && (int)pt.Attribute("LevNo") == 12
                            orderby (int)pt.Attribute("bTime")
                            select new ProfileDAL()
                             {
                                 ID = (int)pt.Attribute("PId"),
                                 BestTime = (int)pt.Attribute("bTime"),
                             }).Take(1).SingleOrDefault();

            if (bTime < p.BestTime)
            {
                return true;
            }
            if (bTime >= p.BestTime && profId == p.ID)
            {
                return true;
            }
            return false;
        }
        async public Task AddProfileTheme(ProfileTheme profTheme, Language lang)
        {
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            var profThemes = (from pt in xDocThProf.Descendants("ProfilesThemes") select pt).FirstOrDefault();
            profThemes.Add(new XElement("ProfTh", new XAttribute("PId", profTheme.PId),
                                               new XAttribute("TId", profTheme.TId),
                                               new XAttribute("LevNo", profTheme.LevNo),
                                               new XAttribute("bTime", profTheme.bTime)
                                          ));
            this.Save(xDocThProf, this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
        }
        async public Task SetLastUser(int id)
        {
            XDocument xDocProfiles = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            var LastUser = (from p in xDocProfiles.Descendants("LastUser") select p).FirstOrDefault();
            LastUser.Attribute("PId").Value = id.ToString();
            await this.Save(xDocProfiles, this.DirPath + "\\" + DataNames.Profiles);
        }
        async public Task SetLastTheme(int pid, int tid)
        {
            XDocument xDocProfiles = await this.LoadAsync(this.DirPath + "\\" + DataNames.Profiles);
            var prof = (from p in xDocProfiles.Descendants("Profile") where (int)p.Attribute("Id") == pid select p).SingleOrDefault();
            prof.Attribute("LastTh").Value = tid.ToString();
            this.Save(xDocProfiles, this.DirPath + "\\" + DataNames.Profiles);
        }
        async public Task<TimeRange> GetBestTime(Language lang, int profId, int themeId)
        {
            XDocument xDocThProf = await this.LoadAsync(this.DirPath + "\\" + DataNames.ProfilesThemes + lang.Name + ".xml");
            var t = (from pt in xDocThProf.Descendants("ProfTh")
                     where (uint)pt.Attribute("TId") == themeId && (int)pt.Attribute("PId") == profId
                     select (int)pt.Attribute("bTime")).FirstOrDefault();
            TimeRange tr = new TimeRange(t);
            return tr;
        }


    }
}