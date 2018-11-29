using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using LG.Threads;

namespace LG.Logging
{
    public partial class Logger
    {
        static AsyncLock myLock = new AsyncLock();

        static public void ShowMessage(Exception ex)
        {
            string inMes = ex.InnerException != null ? ex.InnerException.Message : "";
            MessageDialog msgdlg = new MessageDialog(ex.Message + " " + inMes, "Error");
            msgdlg.Commands.Add(new UICommand("Ok", null, null));
            msgdlg.ShowAsync();
        }

        async  static public Task ShowMessageAndLog(Exception ex)
        {
            string inMes = ex.InnerException != null ? ex.InnerException.Message : "";
            MessageDialog msgdlg = new MessageDialog(ex.Message + " " + inMes, "Error");
            msgdlg.Commands.Add(new UICommand("Ok", null, null));
            await msgdlg.ShowAsync();
            Logger.Log(ex.Message + " " + inMes + " Trace: " + ex.StackTrace);
          
        }
    }

}
