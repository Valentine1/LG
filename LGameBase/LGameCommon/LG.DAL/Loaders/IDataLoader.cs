using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LG.Common;

namespace LG.Data
{
    public interface IDataLoader
    {
        Task AddTheme(Theme theme, Language lang);
        Task AddWord(uint themeId, Word w, Language lang);
        Task<List<Language>> GetLanguages();
        Task<List<Theme>> GetThemesByLang(Language lang, int profId);
        Task<List<Theme>> GetThemesByLang(Language lang, int profId, List<int> ids);
        Task<Theme> GetThemeById(Language lang, int profId, int themeId);
        Task<List<Theme>> GetThemesByIds(Language lang, List<int> themeId);
        Task<List<Word>> GetPreviewThemeWordsById(Language lang, int themeId);
        Task<List<Theme>> GetThemesByLangWithoutChildren(Language lang);
        Task AddThemes(List<Theme> themes, Language lang);
        Task UpdateThemeIsResourcesLoaded(Theme theme);
        Task UpdateThemesIsResourcesLoaded(List<Theme> themes);
        Task UpdateThemeIsPreviewsLoaded(Theme th);
        Task UpdateThemesWordsAndIsPreviewMode(List<Theme> themes, Language lang);

        Task<Dictionary<int, Word>> GetWords(Language lang);
        Task<List<Word>> GetWordsByTheme(int themeId, Language lang, int skip, int take);

        Task<List<Level>> GetAllHierarchyLevels();
        Task<Level> GetLevelByNumber(int no);

        Task<List<ProfileDAL>> GetProfiles();
        Task<int> GetLastUserProfileId();
        Task AddProfileWithReadyId(ProfileDAL prof);
        Task CheckIfExistAndAddProfileTheme(Language lang, int profId, int themeId);
      
        Task SetLastUser(int id);
        Task SetLastTheme(int pid, int tid);
        Task<TimeRange> GetBestTime(Language lang, int id, int themeId);
    }
}
