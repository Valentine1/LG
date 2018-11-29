using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LG.Data;

namespace LGservices
{
    public class WebProfileDataLoader : IProfileDataLoader
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

        async public Task<int> AddProfile(ProfileDAL p)
        {
            return await this.ThemesUpdateClient.UploadProfile(p);
        }

        async public Task<bool?> CheckIfBecomesOrRemainsAllSeeing(Language lang, int profId, Theme theme, int bTime)
        {
            return await this.ThemesUpdateClient.WsCheckIfBecomesOrRemainsAllSeeing(lang, profId, theme.Name, bTime);
        }

        async public Task<bool> SaveLevelHierarchy(ProfileDAL prof, Theme th, Language lang)
        {
           return  await this.ThemesUpdateClient.WsUpdateProfileLevel(lang, prof, th);
        }

        public Task<LevelProfiles> GetProfiles(Language lang, Theme th, int levNo, int skip, int take)
        {
            return this.ThemesUpdateClient.GetProfiles(lang, th.Name, levNo, skip, take);
        }


        async public Task<bool> UpdateProfile(ProfileDAL prof)
        {
           return await this.ThemesUpdateClient.UpdateProfile(prof);
        }
        async public Task<bool> DeleteProfile(int pid, Language lang)
        {
           return await this.ThemesUpdateClient.DeleteProfile(pid, lang);
        }
       
    }
}
