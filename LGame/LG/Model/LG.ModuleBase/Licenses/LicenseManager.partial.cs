using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Foundation;
#if DEBUG
using CurrentAppStore = Windows.ApplicationModel.Store.CurrentAppSimulator;
#else
using CurrentAppStore = Windows.ApplicationModel.Store.CurrentApp;
#endif

namespace LG.Models.Licenses
{
    public partial class LicenseManager
    {
        async public static Task BuyApplication()
        {
            if (!CurrentAppStore.LicenseInformation.IsActive && CurrentAppStore.LicenseInformation.IsTrial)
            {
                try
                {
                    MessageDialog msgdlg = new MessageDialog("Learn & Game trial period expired, purchase is required.", "Purchase");
                    msgdlg.Commands.Add(new UICommand("Ok", null, 1));
                    msgdlg.Commands.Add(new UICommand("Cancel", null, 2));
                    IUICommand asyncOp = await msgdlg.ShowAsync();
                    if ((int)asyncOp.Id == 1)
                    {
                        string w = await CurrentAppStore.RequestAppPurchaseAsync(true);
                        if (CurrentAppStore.LicenseInformation.IsActive)
                        {
                            msgdlg = new MessageDialog("Thanks for purchasing Learn & Game", "Thanks");
                            msgdlg.ShowAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.Logger.ShowMessageAndLog(ex);
                }
            }
        }
        async public static Task<bool> BuyThemes(List<Topic> tops)
        {
            try
            {
                if (CurrentAppStore.LicenseInformation.IsActive && CurrentAppStore.LicenseInformation.IsTrial)
                {
                    MessageDialog msgdlg = new MessageDialog("Product purchase is required", "Purchase");
                    msgdlg.Commands.Add(new UICommand("Ok", null, 1));
                    msgdlg.Commands.Add(new UICommand("Cancel", null, 2));
                    IUICommand asyncOp = await msgdlg.ShowAsync();
                    if ((int)asyncOp.Id == 1)
                    {
                        string w = await CurrentAppStore.RequestAppPurchaseAsync(true);
                        if (!String.IsNullOrEmpty(w))
                        {
                            msgdlg.Content = "Themes purchase is required (6 themes are being bought)";
                            asyncOp = await msgdlg.ShowAsync();
                            if ((int)asyncOp.Id == 1)
                            {
                                string s = await CurrentAppStore.RequestProductPurchaseAsync(tops[0].LicenseInfo.ID, true);
                                return !String.IsNullOrEmpty(s);
                            }
                          
                        }
                    }
                }
                else
                {
                    MessageDialog msgdlg = new MessageDialog("Themes purchase is required (6 themes are being bought)", "Purchase");
                    msgdlg.Commands.Add(new UICommand("Ok", null, 1));
                    msgdlg.Commands.Add(new UICommand("Cancel", null, 2));
                    IUICommand asyncOp = await msgdlg.ShowAsync();
                    if ((int)asyncOp.Id == 1)
                    {
                        string s = await CurrentAppStore.RequestProductPurchaseAsync(tops[0].LicenseInfo.ID, true);
                        return !String.IsNullOrEmpty(s);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logging.Logger.ShowMessageAndLog(ex);
                return false;
            }
        }
        public static bool IsAppActive()
        {
            return CurrentAppStore.LicenseInformation.IsActive;
        }
        private static bool Lic1_20IsActive()
        {
            return true;// CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic1_20].IsActive;
        }

        private static bool Lic21_26IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic21_26].IsActive;
        }

        private static bool Lic27_32IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic27_32].IsActive;
        }
        private static bool Lic33_38IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic33_38].IsActive;
        }
        private static bool Lic39_44IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic39_44].IsActive;
        }
        private static bool Lic45_50IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic45_50].IsActive;
        }
        private static bool Lic51_56IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic51_56].IsActive;
        }
        private static bool Lic57_62IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic57_62].IsActive;
        }
        private static bool Lic63_68IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic63_68].IsActive;
        }
        private static bool Lic69_74IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic69_74].IsActive;
        }
        private static bool Lic75_80IsActive()
        {
            return CurrentAppStore.LicenseInformation.ProductLicenses[LicenseManager.Lic75_80].IsActive;
        }
    }
}
