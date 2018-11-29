using System;
using System.Net;
using System.Windows;
using System.ServiceModel;
using LG.Common;

namespace LGservices
{
    public class ServiceConnection
    {
        private static BasicHttpBinding _bind;
        public static BasicHttpBinding Bind
        {
            get
            {
                if (_bind == null)
                {
                    _bind = new BasicHttpBinding();
                    _bind.MaxBufferSize = 2147483647;
                    _bind.MaxReceivedMessageSize = 2147483647;
                    _bind.CloseTimeout = new TimeSpan(0, 2, 0);
                    _bind.OpenTimeout = new TimeSpan(0, 2, 0);
                    _bind.ReceiveTimeout = new TimeSpan(0, 1, 30);
                    _bind.SendTimeout = new TimeSpan(0, 1, 30);
                }
                return _bind;
            }
        }
        private static EndpointAddress _endpoint;
        public static EndpointAddress TUEndpoint
        {
            get
            {
                if (_endpoint == null)
                {
                    _endpoint = new EndpointAddress(Settings.ThemesUpdaterWebServiceUrl);
                }
                return _endpoint;
            }
        }
        //public static string ThemesUpdateServiceUrl = "http://learnenglishgames.net/ThemesUpdater.asmx";
        public static string ThemesUpdateServiceUrl{get;set;} // = "http://localhost/LGserver/ThemesUpdater.asmx";
        //public static string ThemesUpdateServiceUrl = "http://54.191.255.226/ThemesUpdater.asmx";

        public static void ResetEndpoint()
        {
            _endpoint = null;
        }
    }
}
