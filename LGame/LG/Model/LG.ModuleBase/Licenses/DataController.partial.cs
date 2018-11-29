using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.Web.AtomPub;
using Windows.ApplicationModel;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography.Certificates;
using Windows.Security.Cryptography.DataProtection;
using LG.Common;
using LG.Data;
using LG.Logging;

namespace LG.Models.Licenses
{
    public partial class DataController
    {
        async public Task CheckDataIntegrity()
        {
            await this.CheckAndCopyProfilesPictures();
            await this.CheckAndCopySettingsFile();
            await this.CopyXmlDataLocalIfNecessary();
        }
        async public Task CheckResourcesIntegrity()
        {
            try
            {
                await Settings.Initialize();
                await this.CheckWebServiceUrlAndUpdateIfNecessary();
                List<Theme> themes = await Module.Loader.GetThemesByLangWithoutChildren(GlobalGameParams.AppLang);
                StorageFolder destFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("images", CreationCollisionOption.OpenIfExists);
                for (int i = 0; i < themes.Count; i++)
                {
                    themes[i].LicenseInfo = LicenseManager.GetLicenseInfo(themes[i].ID);
                    if (themes[i].ID <= LicenseManager.BaseThemesCount && !themes[i].IsResourcesLoaded)
                    {
                        await this.CopyResourcesLocally(GlobalGameParams.AppLang, themes[i]);
                        await Module.Loader.UpdateThemeIsResourcesLoaded(themes[i]);
                    }
                    else if (themes[i].ID > LicenseManager.BaseThemesCount && !themes[i].LicenseInfo.IsActive)
                    {
                        if (!themes[i].IsPreviewsLoaded)
                        {
                            //load 3 preview pictures from file server
                            StorageFolder destFolderTh = await destFolder.CreateFolderAsync(themes[i].Name, CreationCollisionOption.OpenIfExists);
                            themes[i].Words = await Module.Loader.GetPreviewThemeWordsById(GlobalGameParams.AppLang, themes[i].ID);
                            await this.LoadPreviewResources(destFolderTh, themes[i]);
                            await Module.Loader.UpdateThemeIsPreviewsLoaded(themes[i]);
                        }
                    }
                    else if (themes[i].ID > LicenseManager.BaseThemesCount && themes[i].LicenseInfo.IsActive)
                    {
                        List<Theme> fullThemes = null;
                        bool shouldIncrement = false;
                        if (themes[i].IsPreviewMode)
                        {
                            //update words xml from server db
                            fullThemes = await this.LoadFullThemeData(LicenseManager.GetThemesIdsByLic(themes[i].LicenseInfo));
                            await Module.Loader.UpdateThemesWordsAndIsPreviewMode(fullThemes, GlobalGameParams.AppLang);
                            shouldIncrement = true;
                        }
                        else
                        {
                            fullThemes = await Module.Loader.GetThemesByIds(GlobalGameParams.AppLang, LicenseManager.GetThemesIdsByLic(themes[i].LicenseInfo));
                        }
                        if (!themes[i].IsResourcesLoaded)
                        {
                            // load all pictures and audios from file server
                            await this.LoadFullResources(fullThemes);
                            await Module.Loader.UpdateThemesIsResourcesLoaded(fullThemes);
                            shouldIncrement = true;
                        }
                        if (shouldIncrement)
                        {
                            i += 6;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ShowMessageAndLog(ex);
            }
        }
        async public Task<List<int>> LoadNewAvailableThemesFromServer(int startThemeId, int takeNo)
        {
            List<Theme> newThemes = await Module.GetThemeLoader().GetNewAvailableThemes(GlobalGameParams.AppLang, startThemeId, takeNo);
            if (newThemes.Count > 0)
            {
                if (LicenseManager.GetLicenseInfo(newThemes[0].ID).IsActive)
                {
                    List<Theme> ts = await this.LoadThemesPreviewFromServer(startThemeId, takeNo);
                    return await LoadFullThemesAndResources(ts);
                }
                else
                {
                    List<Theme> ts = await this.LoadThemesPreviewFromServer(startThemeId, takeNo);
                    return (from t in ts select t.ID).ToList();
                }
            }
            return null;
        }
        async public Task<List<int>> LoadFullThemesAndResources(List<Topic> topics)
        {
            List<int> ids = (from t in topics select t.ID).ToList();
            //update words xml from server db
            List<Theme> fullThemes = await this.LoadFullThemeData(ids);
            foreach (Theme t in fullThemes)
            {
                t.Name = (from top in topics where top.ID == t.ID select top.TextValue).FirstOrDefault();
            }

            await Module.Loader.UpdateThemesWordsAndIsPreviewMode(fullThemes, GlobalGameParams.AppLang);
            // load all pictures and audios from file server
            await this.LoadFullResources(fullThemes);
            await Module.Loader.UpdateThemesIsResourcesLoaded(fullThemes);
            return (from t in fullThemes select t.ID).ToList();
        }

        async private Task<List<int>> LoadFullThemesAndResources(List<Theme> themes)
        {
            List<int> ids = (from t in themes select t.ID).ToList();
            //update words xml from server db
            List<Theme> fullThemes = await this.LoadFullThemeData(ids);
            foreach (Theme t in fullThemes)
            {
                t.Name = (from top in themes where top.ID == t.ID select top.Name).FirstOrDefault();
            }
            await Module.Loader.UpdateThemesWordsAndIsPreviewMode(fullThemes, GlobalGameParams.AppLang);
            // load all pictures and audios from file server
            await this.LoadFullResources(fullThemes);
            await Module.Loader.UpdateThemesIsResourcesLoaded(fullThemes);
            return (from t in fullThemes select t.ID).ToList();
        }
        async private Task<List<Theme>> LoadThemesPreviewFromServer(int startThemeId, int takeNo)
        {
            List<Theme> newThemes = await Module.GetThemeLoader().GetNewAvailableThemes(GlobalGameParams.AppLang, startThemeId, takeNo);
            if (newThemes.Count > 0)
            {
                StorageFolder destFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("images", CreationCollisionOption.OpenIfExists);
                foreach (Theme th in newThemes)
                {
                    StorageFolder destFolderTh = await destFolder.CreateFolderAsync(th.Name, CreationCollisionOption.OpenIfExists);
                    await LoadPreviewResources(destFolderTh, th);
                }
                await Module.Loader.AddThemes(newThemes, GlobalGameParams.AppLang);
                return newThemes;
            }
            return null;
        }
        async private Task LoadPreviewResources(StorageFolder destFolder, Theme th)
        {
            await this.LoadPictureResources(destFolder, th);
        }
        async private Task LoadFullResources(List<Theme> themes)
        {
            StorageFolder destImFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("images", CreationCollisionOption.OpenIfExists);
            StorageFolder destFolderSounds = await ApplicationData.Current.LocalFolder.CreateFolderAsync("sounds", CreationCollisionOption.OpenIfExists);
            destFolderSounds = await destFolderSounds.CreateFolderAsync(GlobalGameParams.AppLang.Name, CreationCollisionOption.OpenIfExists);
            foreach (Theme th in themes)
            {
                StorageFolder destFolderTh = await destImFolder.CreateFolderAsync(th.Name, CreationCollisionOption.OpenIfExists);
                await this.LoadPictureResources(destFolderTh, th);
                StorageFolder destFolderSoundsTheme = await destFolderSounds.CreateFolderAsync(th.Name, CreationCollisionOption.OpenIfExists);
                await this.LoadAudioResources(destFolderSoundsTheme, th);
            }
        }
        async private Task<List<Theme>> LoadFullThemeData(List<int> ids)
        {
            return await Module.GetThemeLoader().GetFullThemesData(GlobalGameParams.AppLang, ids);
        }

        async private Task CheckWebServiceUrlAndUpdateIfNecessary()
        {
            string url = await Module.GetThemeLoader().GetThemesUpdaterWebServiceUrl();
           if (Settings.ThemesUpdaterWebServiceUrl != url)
           {
               Settings.UpdateThemesUpdaterWebServiceUrlEntry(url);
               Module.RefreshThemeLoader(url);
           }
        }
        async private Task CheckAndCopyProfilesPictures()
        {
            bool profPicExist = false;
            try
            {
                await ApplicationData.Current.LocalFolder.GetFolderAsync("profilepictures");
                profPicExist = true;
            }
            catch (FileNotFoundException ex)
            {
                profPicExist = false;
            }
            if (!profPicExist)
            {
                StorageFolder destFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("profilepictures", CreationCollisionOption.OpenIfExists);
                StorageFolder sourceRootFolder = await Package.Current.InstalledLocation.GetFolderAsync("Images");
                StorageFile phIm = await sourceRootFolder.GetFileAsync("NoPhoto.png");
                await phIm.CopyAsync(destFolder);
            }
        }
        async private Task CheckAndCopySettingsFile()
        {
            bool setExist = false;
            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync("Settings.xml");
                setExist = true;
            }
            catch (FileNotFoundException ex)
            {
                setExist = false;
            }
            if (!setExist)
            {
                StorageFile set =  await Package.Current.InstalledLocation.GetFileAsync("Settings.xml");
                await set.CopyAsync(ApplicationData.Current.LocalFolder);
            }
        }

        async private Task CopyXmlDataLocalIfNecessary()
        {
            if (await this.CheckIfXmlDataExistsLocallyAlready())
            {
                return;
            }
            else
            {
                StorageFolder toFold = null;
                bool catchEx = false;
                try
                {
                    toFold = await ApplicationData.Current.LocalFolder.CreateFolderAsync("data", CreationCollisionOption.ReplaceExisting);
                    await ApplicationData.Current.LocalFolder.CreateFolderAsync("images", CreationCollisionOption.ReplaceExisting);
                }
                catch (UnauthorizedAccessException ex)
                {
                    catchEx = true;
                }
                if (catchEx)
                {
                    toFold = await ApplicationData.Current.LocalFolder.CreateFolderAsync("data", CreationCollisionOption.ReplaceExisting);
                }
                await this.CopyXmlData(toFold);
            }
        }
        async private Task CopyResourcesLocally(Language lang, Theme th)
        {
            StorageFolder sourceRootFolder = await Package.Current.InstalledLocation.GetFolderAsync("Images");
            StorageFolder sourceFolder = await sourceRootFolder.GetFolderAsync("images");
            sourceFolder = await sourceFolder.GetFolderAsync(th.Name);

            StorageFolder destFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("images", CreationCollisionOption.OpenIfExists);
            destFolder = await destFolder.CreateFolderAsync(th.Name, CreationCollisionOption.OpenIfExists);

            await CopyMediaLocally(sourceFolder, destFolder, ".jpg");

            sourceFolder = await sourceRootFolder.GetFolderAsync("sounds");
            sourceFolder = await sourceFolder.GetFolderAsync(lang.Name);
            sourceFolder = await sourceFolder.GetFolderAsync(th.Name);

            destFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("sounds", CreationCollisionOption.OpenIfExists);
            destFolder = await destFolder.CreateFolderAsync(lang.Name, CreationCollisionOption.OpenIfExists);
            destFolder = await destFolder.CreateFolderAsync(th.Name, CreationCollisionOption.OpenIfExists);

            await CopyMediaLocally(sourceFolder, destFolder, ".mp3");
        }
        async private Task CopyMediaLocally(StorageFolder fromFolder, StorageFolder toFolder, string ext)
        {
            QueryOptions qopt = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() { ext });
            StorageFileQueryResult r = fromFolder.CreateFileQueryWithOptions(qopt);
            IReadOnlyList<StorageFile> files = await r.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                file.CopyAsync(toFolder);
                //IRandomAccessStream ras = await file.OpenAsync(FileAccessMode.Read);
                //byte[] b = new byte[ras.Size];
                //IBuffer ibuf = await ras.ReadAsync(CryptographicBuffer.CreateFromByteArray(b), (uint)ras.Size, InputStreamOptions.None);
                //StorageFile fileEncrypted = await toFolder.CreateFileAsync(file.Name, CreationCollisionOption.ReplaceExisting);
                //using (IRandomAccessStream rasw = await fileEncrypted.OpenAsync(FileAccessMode.ReadWrite))
                //{
                //    IOutputStream outs = rasw.GetOutputStreamAt(0);
                //    await outs.WriteAsync(ibuf);
                //    bool suc = await outs.FlushAsync();
                //}
            }
        }
        async private Task CopyXmlData(StorageFolder ToFold)
        {
            QueryOptions qopt = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() { ".xml" });
            StorageFolder dataFolder = await Package.Current.InstalledLocation.GetFolderAsync("data");
            StorageFileQueryResult r = dataFolder.CreateFileQueryWithOptions(qopt);
            IReadOnlyList<StorageFile> xmls = await r.GetFilesAsync();
            foreach (StorageFile xml in xmls)
            {
                IRandomAccessStream ras = await xml.OpenAsync(FileAccessMode.Read);
                byte[] b = new byte[ras.Size];
                IBuffer ibuf = await ras.ReadAsync(CryptographicBuffer.CreateFromByteArray(b), (uint)ras.Size, InputStreamOptions.None);
                StorageFile xmlFileDest = await ToFold.CreateFileAsync(xml.Name, CreationCollisionOption.ReplaceExisting);

                using (IRandomAccessStream rasw = await xmlFileDest.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await rasw.WriteAsync(ibuf);
                    rasw.Seek(0);
                    await rasw.FlushAsync();
                }

            }
        }

        async private Task<bool> CheckIfXmlDataExistsLocallyAlready()
        {
            try
            {
                StorageFolder sf = await ApplicationData.Current.LocalFolder.GetFolderAsync(@"data\");
                QueryOptions qopt = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() { ".xml" });
                StorageFileQueryResult r = sf.CreateFileQueryWithOptions(qopt);
                IReadOnlyList<StorageFile> xmls = await r.GetFilesAsync();
                return xmls.Count >= 12;
            }
            catch (FileNotFoundException ex)
            {
                return false;
            }
            catch (Exception ex1)
            {
                throw ex1;
            }
        }

        async private Task LoadPictureResources(StorageFolder destFolder, Theme th)
        {
            foreach (Word w in th.Words)
            {
                DataReader dr = await GetFileByUrl(await GlobalGameParams.FileStorageUrl() + "/images/" + th.Name + "/" + w.PictureUrl);
                StorageFile file = await destFolder.CreateFileAsync(w.PictureUrl, CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream ras = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    uint ui = 0;
                    while ((ui = await dr.LoadAsync(4096)) > 0)
                    {
                        await ras.WriteAsync(dr.ReadBuffer(ui));
                    }
                    ras.Seek(0);
                    await ras.FlushAsync();
                }
            }
        }
        async private Task LoadAudioResources(StorageFolder destFolder, Theme th)
        {
            foreach (Word w in th.Words)
            {
                DataReader dr = await GetFileByUrl(await GlobalGameParams.FileStorageUrl() + "/sounds/" + GlobalGameParams.AppLang.Name + "/" + th.Name + "/" + w.AudioUrl);
                StorageFile file = await destFolder.CreateFileAsync(w.AudioUrl, CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream ras = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    uint ui = 0;
                    while ((ui = await dr.LoadAsync(4096)) > 0)
                    {
                        await ras.WriteAsync(dr.ReadBuffer(ui));
                    }
                    ras.Seek(0);
                    await ras.FlushAsync();
                }
            }
        }
        async private Task<DataReader> GetFileByUrl(string resUrl)
        {
            AtomPubClient c = new AtomPubClient();
            IInputStream inStr = await c.RetrieveMediaResourceAsync(new Uri(resUrl, UriKind.Absolute));
            DataReader dr = new DataReader(inStr);//(stream);
            return dr;
        }
    }
}

