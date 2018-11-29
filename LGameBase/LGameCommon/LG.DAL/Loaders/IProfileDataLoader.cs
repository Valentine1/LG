using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LG.Data
{
    public interface IProfileDataLoader
    {
        Task<int> AddProfile(ProfileDAL p);
        Task<bool> UpdateProfile(ProfileDAL prof);
        Task<bool> DeleteProfile(int pid, Language lang);
        Task<bool?> CheckIfBecomesOrRemainsAllSeeing(Language lang, int profId, Theme theme, int bTime);
        Task<LevelProfiles> GetProfiles(Language lang, Theme th, int levNo, int skip, int take);
        Task<bool> SaveLevelHierarchy(ProfileDAL prof, Theme th, Language lang);
    }
}
