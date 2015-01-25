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
    public class ContactData
    {

        public const string DownGesture = "/Images/arrow.down.png";
        public const string UpGesture = "/Images/arrow.up.png";
        public const string LeftGesture = "/Images/arrow.left.png";
        public const string RightGesture = "/Images/arrow.right.png";
        public const string NoGesture = "NoGesture";



        public string DisplayName { get; set; }

        public string Number { get; set; }

        public string Gesture { get; set; }

        public string Action { get; set; }        

        public ContactSettings settings { get; set; }

        public ContactData()
        {
            settings = new ContactSettings();
            settings.connectSetting = ContactSettings.CallContact;
        }



    }
}
