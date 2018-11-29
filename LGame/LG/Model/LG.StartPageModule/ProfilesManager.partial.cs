using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using LG.Common;
using LG.Data;


namespace LG.Models
{
    public partial class ProfilesManager
    {
        async public Task Initialize()
        {
            float w = GlobalGameParams.WindowWidth * StartPageParams.ProfilesWidth1920 / 1920f;
            this.ProfilesPanel.BlockSize = new LG.Common.Size() { Width = w, Height = w * StartPageParams.ProfilesHeight1920 / StartPageParams.ProfilesWidth1920 };
            await this.RefreshProfiles();
        }

        async public Task RegisterProfileLocally(Profile prof)
        {
            ProfileDAL pdal = new ProfileDAL() { Name = prof.Name, LastThemeID = 1 };
            int id = await Module.GetProfLoader(prof.Type).AddProfile(pdal);
            pdal.ID = id;
            prof.ID = id;
            if (prof.PictureStream != null)
            {
                pdal.PictureUrl = prof.ID + ".png";
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("profilepictures\\" + pdal.PictureUrl, CreationCollisionOption.ReplaceExisting);
                await SaveBitmapToFile(file, prof);
                pdal.HasPhoto = true;
            }
            else
            {
                pdal.HasPhoto = false;
            }
            await Module.GetProfLoader(prof.Type).UpdateProfile(pdal);
            await Module.Loader.SetLastUser(pdal.ID);
          
        }

        async public Task RegisterProfileOnServer(Profile prof)
        {
            ProfileDAL pdal = new ProfileDAL();
            pdal.Name = prof.Name;
            pdal.Password = prof.Password;
            pdal.ContactInfo = prof.ContactInfo == null ? "" : prof.ContactInfo;
            pdal.TimePlayed = 0;
            pdal.HasPhoto = false;
            pdal.Type = prof.Type == ProfileType.Internet ? 1 : 0;
            if (prof.PictureStream != null)
            {
                pdal.HasPhoto = true;
                pdal.PictureUrl = System.Guid.NewGuid().ToString() + ".jpg";
                pdal.Picture = await this.SaveBitmapToBytes(prof);
            }

            pdal.ID = await Module.GetProfLoader(prof.Type).AddProfile(pdal);

            await Module.Loader.AddProfileWithReadyId(pdal);
            if (pdal.HasPhoto)
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("profilepictures\\" + pdal.PictureUrl, CreationCollisionOption.ReplaceExisting);
                await SaveBitmapToFile(file, prof);
            }
            await Module.Loader.SetLastUser(pdal.ID);
        }

        async private Task RefreshProfiles()
        {
            List<ProfileDAL> profilesD = await Module.Loader.GetProfiles();
            int pid = await Module.Loader.GetLastUserProfileId();
            pid = pid == 0 ? -1 : pid;
            foreach (Profile p in this.Profiles)
            {
                p.DeleteItself();
            }
            this.Profiles.Clear();
            this.SelectedProfile = null;
            foreach (ProfileDAL pd in profilesD)
            {
                Profile p = new Profile(pd, false);
                this.Profiles.Add(p);
                if (p.ID == pid)
                {
                    this.SelectedProfile = p;
                }
            }
            if (this.SelectedProfile == null)
            {
                this.SelectedProfile = this.Profiles[0];
            }
        }

        async private Task UpdateProfileLocally(Profile p)
        {
            ProfileDAL pdal = new ProfileDAL() { ID = p.ID, Name = p.Name, ContactInfo = p.ContactInfo, Password = p.Password, PictureUrl = p.PicUrl, HasPhoto = p.HasPhoto };
            if (p.PictureStream != null)
            {
                if (!p.HasPhoto)
                {
                    pdal.HasPhoto = true;
                    pdal.PictureUrl = System.Guid.NewGuid().ToString() + ".jpg";
                }
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("profilepictures\\" + pdal.PictureUrl, CreationCollisionOption.ReplaceExisting);
                await SaveBitmapToFile(file, p);
            }
            await Module.GetProfLoader(ProfileType.Local).UpdateProfile(pdal);
            await Module.Loader.SetLastUser(pdal.ID);
        }

        async private Task<bool> UpdateProfileOnServer(Profile p)
        {
            ProfileDAL pdal = new ProfileDAL() { ID = p.ID, Name = p.Name, ContactInfo = p.ContactInfo, Password = p.Password, PictureUrl = p.PicUrl, HasPhoto = p.HasPhoto };
            if (p.PictureStream != null)
            {
                if (!p.HasPhoto)
                {
                    pdal.HasPhoto = true;
                    pdal.PictureUrl = System.Guid.NewGuid().ToString() + ".jpg";
                }
                pdal.Picture = await this.SaveBitmapToBytes(p);
            }
           return await Module.GetProfLoader(ProfileType.Internet).UpdateProfile(pdal);
        }

        async private Task DeleteSelectedProfileLocally()
        {
            await Module.GetProfLoader(ProfileType.Local).DeleteProfile(this.SelectedProfile.ID, GlobalGameParams.AppLang);
        }
        async private Task<bool> DeleteSelectedProfileOnServer()
        {
            return await Module.GetProfLoader(ProfileType.Internet).DeleteProfile(this.SelectedProfile.ID, GlobalGameParams.AppLang);
        }
        async private Task<byte[]> SaveBitmapToBytes(Profile p)
        {
            using (InMemoryRandomAccessStream memStream = new InMemoryRandomAccessStream())
            {
                byte[] buffer = null;
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(p.PictureStream);
                PixelDataProvider prov = await decoder.GetPixelDataAsync();
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, memStream);
                uint pwidth = (uint)p.PictureSource.PixelWidth;
                uint pheight = (uint)p.PictureSource.PixelHeight;
                double r = (double)pheight / (double)pwidth;
                if (r < 96 / 72)
                {
                    pheight = 96;
                    pwidth = (uint)Math.Round(pheight / r, 0);
                }
                else
                {
                    pwidth = 72;
                    pheight = (uint)Math.Round((double)pwidth * r, 0);
                }
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.BitmapTransform.ScaledHeight = pheight;
                encoder.BitmapTransform.ScaledWidth = pwidth;
                encoder.SetPixelData(decoder.BitmapPixelFormat, BitmapAlphaMode.Premultiplied,
                                      (uint)p.PictureSource.PixelWidth, (uint)p.PictureSource.PixelHeight, decoder.DpiX, decoder.DpiY, prov.DetachPixelData());
                await encoder.FlushAsync();
                Stream strm = memStream.AsStreamForRead();
                strm.Position = 0;
                buffer = new byte[memStream.Size];
                int bytesRead;
                while ((bytesRead = strm.Read(buffer, 0, buffer.Length)) != 0)
                {

                }
                return buffer;

            }
        }

        async private Task SaveBitmapToFile(StorageFile file, Profile p)
        {
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(p.PictureStream);
                PixelDataProvider prov = await decoder.GetPixelDataAsync();
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                uint pwidth = (uint)p.PictureSource.PixelWidth;
                uint pheight = (uint)p.PictureSource.PixelHeight;
                double r = (double)pheight / (double)pwidth;
                if (r < 96 / 72)
                {
                    pheight = 96;
                    pwidth = (uint)Math.Round(pheight / r, 0);
                }
                else
                {
                    pwidth = 72;
                    pheight = (uint)Math.Round((double)pwidth * r, 0);
                }
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.BitmapTransform.ScaledHeight = pheight;
                encoder.BitmapTransform.ScaledWidth = pwidth;
                encoder.SetPixelData(decoder.BitmapPixelFormat, BitmapAlphaMode.Premultiplied,
                                      (uint)p.PictureSource.PixelWidth, (uint)p.PictureSource.PixelHeight, decoder.DpiX, decoder.DpiY, prov.DetachPixelData());
                await encoder.FlushAsync();

            }
        }

    }
}
