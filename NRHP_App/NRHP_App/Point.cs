using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace NRHP_App
{
    class Point : Pin
    {
        public string RefNum { get; set; }
        public string Category { get; set; }
    }
}
