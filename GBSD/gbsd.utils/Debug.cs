using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GestureTest
{
    public static class Debug
    {
        public static void writeToIO(string msg)
        {
            //System.Diagnostics.Debug.WriteLine(msg);
        }


        public static void writeToIO2(string p)
        {
            System.Diagnostics.Debug.WriteLine(p);
        }
    }
}
