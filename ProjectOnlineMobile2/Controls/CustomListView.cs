using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.Controls
{
    public class CustomListView : ListView
    {
        public CustomListView()
        {
        }

        public CustomListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
        }
    }
}
