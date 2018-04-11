using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ProjectOnlineMobile2.Services;

namespace ProjectOnlineMobile2.Android
{
    public sealed class Singleton
    {
        private static volatile Singleton _instance;
        private static readonly object _syncLock = new object();

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_syncLock)
                {
                    if(_instance == null)
                    {
                        _instance = new Singleton();
                    }
                }

                return _instance;
            }
        }

        public TokenService TokenServices = new TokenService();
    }
}