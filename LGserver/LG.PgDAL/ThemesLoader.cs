using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using LG.Data;
using LG.Common;

namespace LG.PgDAL
{
    public class ThemesLoader
    {
        public List<Theme> GetNewThemes(Language lang, int startId, int take)
        {
            List<Theme> themes = null;
            switch (lang.ID)
            {
                case 1:
                    themes = GetNewThemesEnglish(startId, take);
                    break;
                case 2:
                    break;
            }
            return themes;
        }
        public List<Theme> GetThemesWordsData(Language lang, List<int> ids)
        {
            List<Theme> themes = null;
            switch (lang.ID)
            {
                case 1:
                    themes = GetThemesWordsDataEnglish(ids);
                    break;
                case 2:
                    break;
            }
            return themes;
        }

        private List<Theme> GetNewThemesEnglish(int startId, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                List<Theme> themes = (from th in lg.themes
                                      where th.id >= startId
                                      select new Theme()
                                      {
                                          ID = th.id,
                                          Name = th.Name,
                                          Width = (double)th.Width,
                                          Height = (double)th.Height,
                                          Left = (double)th.Left,
                                          Top = (double)th.Top,
                                          PathData = th.Data,
                                      }).Take(take).ToList();
                foreach (Theme th in themes)
                {
                    th.NameInLetters = (from thLet in lg.themes_english_letters
                                        where (int)thLet.tid == th.ID
                                        select new Symbol()
                                        {
                                            Value = thLet.letter,
                                            Left = (double)thLet.left,
                                            Top = (double)thLet.top,
                                            Rotation = (double)thLet.rotation
                                        }).ToList();
                    th.Words = (from thPic in lg.themes_english_pictures
                                where (int)thPic.tid == th.ID
                                join w in lg.words on thPic.wid equals w.id
                                select new Word()
                                {
                                    ID = (int)w.id,
                                    PictureUrl = w.pic_url,
                                    Left = (double)thPic.left,
                                    Top = (double)thPic.top,
                                    Rotation = (double)thPic.rotation,
                                    BackColor = new Color() { HexString = w.back_clr },
                                    BorderColor = new Color() { HexString = w.border_clr },
                                    LeftColor = new Color() { HexString = w.left_clr },
                                    TopColor = new Color() { HexString = w.top_clr },
                                    LeftTopColor = new Color() { HexString = w.left_top_clr }

                                }).ToList();
                }
                return themes.ToList();

            }
        }
        private List<Theme> GetThemesWordsDataEnglish(List<int> ids)
        {
            List<Theme> fullThemes = new List<Theme>();
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                foreach (int id in ids)
                {
                    Theme th = new Theme() { ID = id };
                    fullThemes.Add(th);
                    th.Words = (from w in lg.words
                                where (int)w.tid == id
                                join ew in lg.words_english on w.id equals ew.wid
                                select new Word()
                                {
                                    ID = (int)w.id,
                                    PictureUrl = w.pic_url,
                                    BackColor = new Color() { HexString = w.back_clr },
                                    BorderColor = new Color() { HexString = w.border_clr },
                                    LeftColor = new Color() { HexString = w.left_clr },
                                    TopColor = new Color() { HexString = w.top_clr },
                                    LeftTopColor = new Color() { HexString = w.left_top_clr },
                                    Value = ew.value,
                                    AudioUrl = ew.audio_url
                                }).ToList();
                }
            }
            return fullThemes;
        }

        public int AddProfile(ProfileDAL prof)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profile p = new profile()
                {
                    name = this.ReEncode(prof.Name),
                    pic_url = prof.PictureUrl,
                    pass = this.ReEncode(prof.Password),
                    contact_info = this.ReEncode(prof.ContactInfo),
                    time = prof.TimePlayed,
                    is_photo = prof.HasPhoto
                };
                lg.profiles.AddObject(p);
                lg.SaveChanges();
                return p.id;
            }
        }
        public void UpdateProfile(ProfileDAL prof)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profile pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();

                pr.name =this.ReEncode(prof.Name);
                pr.pic_url = prof.PictureUrl;
                pr.pass = this.ReEncode(prof.Password);
                pr.is_photo = prof.HasPhoto;
                pr.contact_info =this.ReEncode(prof.ContactInfo);
                lg.SaveChanges();
            }
        }


        private string ReEncode(string s)
        {
            byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(s);
            return System.Text.Encoding.GetEncoding(1252).GetString(b);
        }

        private string Decode(string s)
        {
            byte[] b = System.Text.Encoding.GetEncoding(1252).GetBytes(s);
            return System.Text.Encoding.UTF8.GetString(b);
        }
        public string DeleteProfile(int pid, Language lang)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profile pr = (from p in lg.profiles where p.id == pid select p).SingleOrDefault();
                string picUrl = pr.pic_url;
                lg.DeleteObject(pr);
                this.DeleteProfilesEnglishWeather(pid, lg);
                this.DeleteProfilesEnglishBody(pid, lg);
                this.DeleteProfilesEnglishPlant1(pid, lg);
                this.DeleteProfilesEnglishPlant2(pid, lg);
                this.DeleteProfilesEnglishNature1(pid, lg);
                this.DeleteProfilesEnglishCity(pid, lg);
                this.DeleteProfilesEnglishHouse1(pid, lg);
                this.DeleteProfilesEnglishNature2(pid, lg);
                this.DeleteProfilesEnglishAnimal1(pid, lg);
                this.DeleteProfilesEnglishKitchen(pid, lg);
                lg.SaveChanges();
                return picUrl;
            }
        }

        #region Delete from all English Profile_Themes
        private void DeleteProfilesEnglishWeather(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_weather where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_weather pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishBody(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_body where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_body pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishPlant1(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_plant1 where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_plant1 pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishPlant2(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_plant2 where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_plant2 pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishNature1(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_nature1 where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_nature1 pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishCity(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_city where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_city pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishHouse1(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_house1 where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_house1 pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishNature2(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_nature2 where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_nature2 pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishAnimal1(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_animal1 where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_animal1 pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        private void DeleteProfilesEnglishKitchen(int pid, db_lgameEntities lg)
        {
            var pts = (from pew in lg.profiles_english_kitchen where pew.pid == pid select pew).ToList();
            if (pts != null)
            {
                foreach (profiles_english_kitchen pew in pts)
                {
                    lg.DeleteObject(pew);
                }
            }
        }
        #endregion

        public bool CheckIfBecomesOrRemainsAllSeeing(Language lang, int profId, string thName, int bTime)
        {
            switch (lang.ID)
            {
                case 1:
                    switch (thName)
                    {
                        case "weather":
                            return CheckAllSeeingEnglishWeather(profId, bTime);
                        case "body":
                            return CheckAllSeeingEnglishBody(profId, bTime);
                        case "plant1":
                            return CheckAllSeeingEnglishPlant1(profId, bTime);
                        case "plant2":
                            return CheckAllSeeingEnglishPlant2(profId, bTime);
                        case "nature1":
                            return CheckAllSeeingEnglishNature1(profId, bTime);
                        case "city":
                            return CheckAllSeeingEnglishCity(profId, bTime);
                        case "house1":
                            return CheckAllSeeingEnglishHouse1(profId, bTime);
                        case "nature2":
                            return CheckAllSeeingEnglishNature2(profId, bTime);
                        case "animal1":
                            return CheckAllSeeingEnglishAnimal1(profId, bTime);
                        case "kitchen":
                            return CheckAllSeeingEnglishKitchen(profId, bTime);
                    }
                    break;
            }
            return false;
        }
        #region Check if All Seeing in all English Profile_Themes
        private bool CheckAllSeeingEnglishWeather(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_weather pew = (from p in lg.profiles_english_weather where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishBody(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_body pew = (from p in lg.profiles_english_body where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishPlant1(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_plant1 pew = (from p in lg.profiles_english_plant1 where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishPlant2(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_plant2 pew = (from p in lg.profiles_english_plant2 where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishNature1(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_nature1 pew = (from p in lg.profiles_english_nature1 where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishCity(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_city pew = (from p in lg.profiles_english_city where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishHouse1(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_house1 pew = (from p in lg.profiles_english_house1 where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishNature2(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_nature2 pew = (from p in lg.profiles_english_nature2 where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishAnimal1(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_animal1 pew = (from p in lg.profiles_english_animal1 where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        private bool CheckAllSeeingEnglishKitchen(int profId, int bTime)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                profiles_english_kitchen pew = (from p in lg.profiles_english_kitchen where p.lev_no == 12 orderby p.best_time select p).Take(1).SingleOrDefault();

                if (bTime < pew.best_time)
                {
                    return true;
                }
                if (bTime >= pew.best_time && profId == pew.pid)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        public void UpdateProfileLevel(Language lang, ProfileDAL prof, Theme th)
        {

            switch (lang.ID)
            {
                case 1:
                    switch (th.Name)
                    {
                        case "weather":
                            UpdateProfLevEnglishWeather(prof, th);
                            break;
                        case "body":
                            UpdateProfLevEnglishBody(prof, th);
                            break;
                        case "plant1":
                            UpdateProfLevEnglishPlant1(prof, th);
                            break;
                        case "plant2":
                            UpdateProfLevEnglishPlant2(prof, th);
                            break;
                        case "nature1":
                            UpdateProfLevEnglishNature1(prof, th);
                            break;
                        case "city":
                            UpdateProfLevEnglishCity(prof, th);
                            break;
                        case "house1":
                            UpdateProfLevEnglishHouse1(prof, th);
                            break;
                        case "nature2":
                            UpdateProfLevEnglishNature2(prof, th);
                            break;
                        case "animal1":
                            UpdateProfLevEnglishAnimal1(prof, th);
                            break;
                        case "kitchen":
                            UpdateProfLevEnglishKitchen(prof, th);
                            break;
                    }
                    break;
            }
        }
        #region Update all English Profile_Themes
        private void UpdateProfLevEnglishWeather(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_weather
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_weather pew = new profiles_english_weather()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_weather.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishBody(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_body
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_body pew = new profiles_english_body()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_body.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishPlant1(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_plant1
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_plant1 pew = new profiles_english_plant1()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_plant1.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishPlant2(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_plant2
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_plant2 pew = new profiles_english_plant2()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_plant2.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishNature1(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_nature1
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_nature1 pew = new profiles_english_nature1()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_nature1.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishCity(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_city
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_city pew = new profiles_english_city()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_city.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishHouse1(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_house1
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_house1 pew = new profiles_english_house1()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_house1.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishNature2(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_nature2
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_nature2 pew = new profiles_english_nature2()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_nature2.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishAnimal1(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_animal1
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_animal1 pew = new profiles_english_animal1()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_animal1.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        private void UpdateProfLevEnglishKitchen(ProfileDAL prof, Theme th)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profTheme = (from pt in lg.profiles_english_kitchen
                                 where pt.pid == prof.ID
                                 select pt).SingleOrDefault();
                if (profTheme == null)
                {
                    profiles_english_kitchen pew = new profiles_english_kitchen()
                    {
                        pid = prof.ID,
                        lev_no = th.HLevel.Number,
                        best_time = th.HLevel.BestTimeResult
                    };
                    lg.profiles_english_kitchen.AddObject(pew);
                }
                else
                {
                    profTheme.lev_no = th.HLevel.Number;
                    profTheme.best_time = th.HLevel.BestTimeResult;
                }

                var pr = (from p in lg.profiles where p.id == prof.ID select p).SingleOrDefault();
                pr.time = prof.TimePlayed;

                lg.SaveChanges();
            }
        }
        #endregion

        public LevelProfiles GetProfiles(Language lang, string thName, int levNo, int skip, int take)
        {
            switch (lang.ID)
            {
                case 1:
                    switch (thName)
                    {
                        case "weather":
                            return GetProfilesEnglishWeather(levNo, skip, take);
                        case "body":
                            return GetProfilesEnglishBody(levNo, skip, take);
                        case "plant1":
                            return GetProfilesEnglishPlant1(levNo, skip, take);
                        case "plant2":
                            return GetProfilesEnglishPlant2(levNo, skip, take);
                        case "nature1":
                            return GetProfilesEnglishNature1(levNo, skip, take);
                        case "city":
                            return GetProfilesEnglishCity(levNo, skip, take);
                        case "house1":
                            return GetProfilesEnglishHouse1(levNo, skip, take);
                        case "nature2":
                            return GetProfilesEnglishNature2(levNo, skip, take);
                        case "animal1":
                            return GetProfilesEnglishAnimal1(levNo, skip, take);
                        case "kitchen":
                            return GetProfilesEnglishKitchen(levNo, skip, take);
                    }
                    break;
            }
            return null;


        }
        #region Get from all English Profiles_Themes
        public LevelProfiles GetProfilesEnglishWeather(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles =( (from p in lg.profiles
                                join pt in lg.profiles_english_weather on p.id equals pt.pid
                                where pt.lev_no == levNo
                                orderby pt.best_time
                                select new ProfileDAL()
                                    {
                                        ID = p.id,
                                        Name = p.name,
                                        PictureUrl = p.pic_url,
                                        ContactInfo = p.contact_info,
                                        Type = 1,
                                        TimePlayed = p.time,
                                        BestTime = pt.best_time,
                                        HasPhoto = p.is_photo
                                    }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_weather where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishBody(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_body on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_body where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishPlant1(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_plant1 on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_plant1 where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishPlant2(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_plant2 on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_plant2 where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishNature1(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_nature1 on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_nature1 where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishCity(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_city on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_city where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishHouse1(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_house1 on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_house1 where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishNature2(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_nature2 on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_nature2 where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishAnimal1(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_animal1 on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_animal1 where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        public LevelProfiles GetProfilesEnglishKitchen(int levNo, int skip, int take)
        {
            using (db_lgameEntities lg = new db_lgameEntities())
            {
                var profiles = ((from p in lg.profiles
                                 join pt in lg.profiles_english_kitchen on p.id equals pt.pid
                                 where pt.lev_no == levNo
                                 orderby pt.best_time
                                 select new ProfileDAL()
                                 {
                                     ID = p.id,
                                     Name = p.name,
                                     PictureUrl = p.pic_url,
                                     ContactInfo = p.contact_info,
                                     Type = 1,
                                     TimePlayed = p.time,
                                     BestTime = pt.best_time,
                                     HasPhoto = p.is_photo
                                 }).Skip(skip).Take(take)).ToList();
                foreach (ProfileDAL pd in profiles)
                {
                    pd.Name = this.Decode(pd.Name);
                    pd.ContactInfo = this.Decode(pd.ContactInfo);
                }
                int totalLevNo = (from p in lg.profiles_english_kitchen where p.lev_no == levNo select p).Count();
                LevelProfiles lp = new LevelProfiles() { Profiles = profiles.ToList(), TotalLevelPlayers = totalLevNo };
                return lp;
            }
        }
        #endregion
    }
}
