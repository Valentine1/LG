using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace LG.Data
{
    public interface IThemeDataLoader
    {
        Task<string> GetThemesUpdaterWebServiceUrl();
        Task<string> GetFileStorageUrl();
        Task<List<LG.Data.Theme>> GetNewAvailableThemes(LG.Data.Language l, int themeId, int takeNo);
        Task<List<LG.Data.Theme>> GetFullThemesData(LG.Data.Language l, List<int> ids);
        Task ReportErrorToServer(string errorMes);
    }
}
