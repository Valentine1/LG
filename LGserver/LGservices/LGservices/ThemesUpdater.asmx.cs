using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using LG.Common;
using LG.Data;
using LG.PgDAL;
using Logging;

namespace LGservices
{
    /// <summary>
    /// Summary description for ThemesUpdater
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
   
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ThemesUpdater : System.Web.Services.WebService
    {
        private WebLogger _wlogger;
        private WebLogger wlogger
        {
            get
            {
                if (HttpContext.Current.Application["wlog"] == null)
                {
                    _wlogger = new WebLogger(Server.MapPath("Logs.txt"), true);
                    HttpContext.Current.Application["wlog"] = _wlogger;
                }
                return (WebLogger)HttpContext.Current.Application["wlog"];
            }
        }

        private WebLogger _wErrorlogger;
        private WebLogger wErrorlogger
        {
            get
            {
                if (HttpContext.Current.Application["werrorlog"] == null)
                {
                    _wErrorlogger = new WebLogger(Server.MapPath("LogsError.txt"), true);
                    HttpContext.Current.Application["werrorlog"] = _wErrorlogger;
                }
                return (WebLogger)HttpContext.Current.Application["werrorlog"];
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetWebThemeUpdaterUrl()
        {
            try
            {
                string s = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                HttpContext.Current.Application.Lock();
                wlogger.LogWithTimeMark(s);
                HttpContext.Current.Application.UnLock();
                //wlogger.CloseLog();
            }
            catch (Exception ex)
            {

            }
            return "http://learnenglishgames.net/ThemesUpdater.asmx";
          // return "http://localhost/LGserver/ThemesUpdater.asmx";
        }
        [WebMethod(EnableSession = true)]
        public void LogErrorToServe(string s)
        {
            try
            {
                string addr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                HttpContext.Current.Application.Lock();
                wErrorlogger.LogWithTimeMark(addr + ": " + s);
                HttpContext.Current.Application.UnLock();
                //wlogger.CloseLog();
            }
            catch (Exception ex)
            {

            }
        }
        [WebMethod]
        public string GetFileStorageBaseUrl()
        {
           return "http://learnenglishgames.net";
          // return "http://localhost/LGserver";
        }
        public void ProcessRequest(HttpContext context)
        {
         
        }
        [WebMethod]
        public List<Theme> GetNewThemes(Language lang, int themeId, int take, out string errMessage)
        {
            errMessage = "";
            try
            {
                ThemesLoader loader = new ThemesLoader();
                List<Theme> themes = loader.GetNewThemes(lang, themeId, take);
                return themes;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return null;
            }
        }

        [WebMethod]
        public List<Theme> GetFullThemesData(Language lang, List<int> ids, out string errMessage)
        {
            errMessage = "";
            try
            {
                ThemesLoader loader = new ThemesLoader();
                List<Theme> themes = loader.GetThemesWordsData(lang, ids);
                return themes;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return null;
            }
        }
        [WebMethod]
        public int AddProfile(ProfileDAL pdal, out string errMessage)
        {
            errMessage = "";
            try
            {
                ThemesLoader loader = new ThemesLoader();
                int id = loader.AddProfile(pdal);
                if (pdal.HasPhoto)
                {
                    MemoryStream mem = new MemoryStream(pdal.Picture);
                    AmazonServices.AmazonS3 s3 = new AmazonServices.AmazonS3();
                    s3.AddFileToStorage(Context.Server.MapPath("profilepictures"), mem, pdal.PictureUrl);
                }
                return id;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return 0;
            }
        }
        [WebMethod]
        public void UpdateProfile(ProfileDAL pdal, out string errMessage)
        {
            errMessage = "";
            try
            {
                ThemesLoader loader = new ThemesLoader();
                loader.UpdateProfile(pdal);
                if (pdal.Picture != null)
                {
                    MemoryStream mem = new MemoryStream(pdal.Picture);
                    AmazonServices.AmazonS3 s3 = new AmazonServices.AmazonS3();
                    s3.AddFileToStorage(Context.Server.MapPath("profilepictures"), mem, pdal.PictureUrl);
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
            }
        }
        [WebMethod]
        public void DeleteProfile(int pid, Language lang, out string errMessage)
        {
            errMessage = "";
            try
            {
                ThemesLoader loader = new ThemesLoader();
                string picUrl = loader.DeleteProfile(pid, lang);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
            }
        }
        [WebMethod]
        public bool CheckIfBecomesOrRemainsAllSeeing(Language lang, int profId, string thName, int bTime, out string errMessage)
        {
            errMessage = "";
            try
            {
                ThemesLoader loader = new ThemesLoader();
                return loader.CheckIfBecomesOrRemainsAllSeeing(lang, profId, thName, bTime);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        [WebMethod]
        public bool UpdateProfileLevel(ProfileDAL prof, Theme th, Language lang, out string errMessage)
        {
            errMessage = "";
            try
            {
                ThemesLoader loader = new ThemesLoader();
                loader.UpdateProfileLevel(lang, prof, th);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        [WebMethod]
        public LevelProfiles GetProfiles(Language lang, string thName, int levNo, int skip, int take, out string errMessage)
        {
            errMessage = "";
            try
            {
                ThemesLoader loader = new ThemesLoader();
                return loader.GetProfiles(lang, thName, levNo, skip, take);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return null;
            }
        }
    }
}
