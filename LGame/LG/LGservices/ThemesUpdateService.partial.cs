using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ComponentModel;
using Windows.UI.Popups;
using LG.Logging;
using LGservices.ThemesUpdaterProxy;

namespace LGservices
{
    public partial class ThemesUpdateService
    {
        async public Task<string> CatchFileStorageUrl()
        {
            LGservices.ThemesUpdaterProxy.GetFileStorageBaseUrlResponse fu = await ServiceClient.GetFileStorageBaseUrlAsync();
            return fu.Body.GetFileStorageBaseUrlResult;
        }

        async public Task<string> CatchThemesUpdaterWebServiceUrl()
        {
            LGservices.ThemesUpdaterProxy.GetWebThemeUpdaterUrlResponse ur = await ServiceClient.GetWebThemeUpdaterUrlAsync();
            return ur.Body.GetWebThemeUpdaterUrlResult;
        }
        async public Task ReportErrorToServer(string errorMes)
        {
            try
            {
                await ServiceClient.LogErrorToServeAsync(errorMes);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
        async public Task<List<LG.Data.Theme>> CatchNewAvailableThemes(LG.Data.Language l, int themeId, int takeNo)
        {
            Converter conv = new Converter();
            LGservices.ThemesUpdaterProxy.GetNewThemesResponse tr = await this.ServiceClient.GetNewThemesAsync(conv.ConvertToWsLanguage(l), themeId, takeNo);
            if (!String.IsNullOrEmpty(tr.Body.errMessage))
            {
                throw new Exception(tr.Body.errMessage);
            }
            List<Theme> themes = tr.Body.GetNewThemesResult;
            return conv.ConvertToThemes(themes);
        }

        async public Task<List<LG.Data.Theme>> CatchFullThemesData(LG.Data.Language l, List<int> ids)
        {
            Converter conv = new Converter();
            ArrayOfInt ai = new ArrayOfInt();
            ai.AddRange(ids);
            LGservices.ThemesUpdaterProxy.GetFullThemesDataResponse tr = await this.ServiceClient.GetFullThemesDataAsync(conv.ConvertToWsLanguage(l), ai);
            if (!String.IsNullOrEmpty(tr.Body.errMessage))
            {
                throw new Exception(tr.Body.errMessage);
            }
            List<Theme> themes = tr.Body.GetFullThemesDataResult;
            return conv.ConvertToThemes(themes);
        }

        async public Task<int> UploadProfile(LG.Data.ProfileDAL pdal)
        {
            Converter conv = new Converter();
            ProfileDAL p = conv.ConvertToWsProfile(pdal);
            LGservices.ThemesUpdaterProxy.AddProfileResponse pr = await this.ServiceClient.AddProfileAsync(p);
            if (!String.IsNullOrEmpty(pr.Body.errMessage))
            {
                throw new Exception(pr.Body.errMessage);
            }
            return pr.Body.AddProfileResult;
        }

        async public Task<bool> WsUpdateProfileLevel(LG.Data.Language l, LG.Data.ProfileDAL prof, LG.Data.Theme th)
        {
            try
            {
                Converter conv = new Converter();
                LGservices.ThemesUpdaterProxy.UpdateProfileLevelResponse plr = await this.ServiceClient.UpdateProfileLevelAsync(conv.ConvertToWsProfile(prof), conv.ConvertToWsTheme(th),
                                                                                                                                conv.ConvertToWsLanguage(l));
                if (!String.IsNullOrEmpty(plr.Body.errMessage))
                {
                    throw new Exception(plr.Body.errMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ShowMessageAndLog(ex);
                return false;
            }
        }

        async public Task<bool?> WsCheckIfBecomesOrRemainsAllSeeing(LG.Data.Language l, int profId, string thName, int bTime)
        {
            try
            {
                Converter conv = new Converter();
                LGservices.ThemesUpdaterProxy.CheckIfBecomesOrRemainsAllSeeingResponse asr = await this.ServiceClient.CheckIfBecomesOrRemainsAllSeeingAsync(conv.ConvertToWsLanguage(l), profId, thName, bTime);
                if (!String.IsNullOrEmpty(asr.Body.errMessage))
                {
                    throw new Exception(asr.Body.errMessage);
                }
                return asr.Body.CheckIfBecomesOrRemainsAllSeeingResult;
            }
            catch (Exception ex)
            {
                Logger.ShowMessageAndLog(ex);
                return null;
            }
        }

        async public Task<LG.Data.LevelProfiles> GetProfiles(LG.Data.Language lang, string thName, int levNo, int skip, int take)
        {
            try
            {
                Converter conv = new Converter();
                LGservices.ThemesUpdaterProxy.GetProfilesResponse pr = await this.ServiceClient.GetProfilesAsync(conv.ConvertToWsLanguage(lang), thName, levNo, skip, take);
                if (!String.IsNullOrEmpty(pr.Body.errMessage))
                {
                    throw new Exception(pr.Body.errMessage);
                }
                LevelProfiles lp = pr.Body.GetProfilesResult;
                return conv.ConvertToLevelProfiles(lp);
            }
            catch (Exception ex)
            {
                Logger.ShowMessageAndLog(ex);
                return null;
            }
        }

        async public Task<bool> UpdateProfile(LG.Data.ProfileDAL prof)
        {
            try
            {
                Converter conv = new Converter();
                LGservices.ThemesUpdaterProxy.UpdateProfileResponse pr = await this.ServiceClient.UpdateProfileAsync(conv.ConvertToWsProfile(prof));
                if (!String.IsNullOrEmpty(pr.Body.errMessage))
                {
                    throw new Exception(pr.Body.errMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.ShowMessageAndLog(ex);
                return false;
            }
        }

        async public Task<bool> DeleteProfile(int pid, LG.Data.Language lang)
        {
            try
            {
                Converter conv = new Converter();
                LGservices.ThemesUpdaterProxy.DeleteProfileResponse dpr = await this.ServiceClient.DeleteProfileAsync(pid, conv.ConvertToWsLanguage(lang));
                return true;
            }
            catch (Exception ex)
            {
                Logger.ShowMessageAndLog(ex);
                return false;
            }
        }
    }
}
