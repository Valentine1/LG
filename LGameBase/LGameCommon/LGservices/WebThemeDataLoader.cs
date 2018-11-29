using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Data;

namespace LGservices
{
    public class WebThemeDataLoader : IThemeDataLoader
    {
        private ThemesUpdateService _themesUpdateClient;
        public ThemesUpdateService ThemesUpdateClient
        {
            get
            {
                if (_themesUpdateClient == null)
                {
                    _themesUpdateClient = new ThemesUpdateService();
                }
                return _themesUpdateClient;
            }
        }

        async public Task<string> GetFileStorageUrl()
        {
            return await ThemesUpdateClient.CatchFileStorageUrl();
        }

        async public Task ReportErrorToServer(string errorMes)
        {
             await ThemesUpdateClient.ReportErrorToServer(errorMes);
        }


        async public Task<List<Theme>> GetNewAvailableThemes(Language l, int themeId, int takeNo)
        {
            return await this.ThemesUpdateClient.CatchNewAvailableThemes(l, themeId, takeNo);
        }

        async public Task<List<Theme>> GetFullThemesData(Language l, List<int> ids)
        {
            return await this.ThemesUpdateClient.CatchFullThemesData(l, ids);
        }

        async public Task<string> GetThemesUpdaterWebServiceUrl()
        {
            return await ThemesUpdateClient.CatchThemesUpdaterWebServiceUrl();
        }
    }
}
