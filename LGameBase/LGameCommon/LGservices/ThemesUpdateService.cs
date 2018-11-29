using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ComponentModel;
using LGservices.ThemesUpdaterProxy;
using LG.Data;

namespace LGservices
{
    public partial class ThemesUpdateService 
    {
        private ThemesUpdaterSoapClient _serviceClient;
        private ThemesUpdaterSoapClient ServiceClient
        {
            get
            {
                if (_serviceClient == null)
                {
                    _serviceClient = new ThemesUpdaterSoapClient(ServiceConnection.Bind, ServiceConnection.TUEndpoint);
                }
                return _serviceClient;
            }
        }

        //public static Task<string> DownloadDataTask(ThemesUpdaterSoapClient webClient)
        //{
        //    var tcs = new TaskCompletionSource<string>(TaskCreationOptions.None);

            
        //    EventHandler<HelloWorldCompletedEventArgs> handler = null;
        //    handler = (sender, e) => TransferCompletion(tcs, e, () => (e as HelloWorldCompletedEventArgs).Result, () => webClient.HelloWorldCompleted -= handler);
        //    webClient.HelloWorldCompleted += handler;

        //    try
        //    {
        //        webClient.HelloWorldAsync();
        //    }
        //    catch
        //    {
        //        webClient.HelloWorldCompleted -= handler;
        //        tcs.TrySetCanceled();
        //        throw;
        //    }
        //    return tcs.Task;
        //}

        //private static void TransferCompletion<T>(TaskCompletionSource<T> tcs, AsyncCompletedEventArgs e, Func<T> getResult, Action unregisterHandler)
        //{
        //    if (e.UserState == tcs)
        //    {
        //        if (e.Cancelled)
        //        {
        //            tcs.TrySetCanceled();
        //        }
        //        else if (e.Error != null)
        //        {
        //            tcs.TrySetException(e.Error);
        //        }
        //        else
        //        {
        //            tcs.TrySetResult(getResult());
        //        }
        //        if (unregisterHandler != null)
        //        {
        //            unregisterHandler();
        //        }
        //    }
        //}

    }
}
