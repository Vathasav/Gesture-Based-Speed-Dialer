using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace GestureTest
{
    public partial class Settings : PhoneApplicationPage
    {
        //string[] options = { "Call the phone numbers", "Send a distress message", "Write a custom message" };

        ContactData selectedContactData = null;

        public Settings()
        {
            InitializeComponent();
            //this.settingsListBox.ItemsSource = options;
        }

        private void SettingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //NavigationService.GoBack();
        }

        private void settingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            string name = NavigationContext.QueryString["Name"];
            string gesture = NavigationContext.QueryString["Gesture"];

            selectedContactData= GesContactsDB.getInstance().getContactData(gesture);
            
        }

        private void callNumber_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (selectedContactData != null)
            {
                selectedContactData.settings.connectSetting = ContactSettings.CallContact;
                GesContactsDB.getInstance().save();
                NavigationService.GoBack();
            }
        }

        private void writeMessage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (selectedContactData != null)
            {
                selectedContactData.settings.connectSetting = ContactSettings.WriteMessage;
                GesContactsDB.getInstance().save();
                NavigationService.GoBack();
            }
        }

        private void distressMessage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (selectedContactData != null)
            {
                selectedContactData.settings.connectSetting = ContactSettings.SendDistressMessage;
                GesContactsDB.getInstance().save();
                NavigationService.GoBack();
            }
        }

        
        private void deleteGesture_Tap(object sender, RoutedEventArgs e)
        {
            
                if (selectedContactData != null)
                {

                    GesContactsDB.getInstance().remove(selectedContactData.Gesture);
                    GesContactsDB.getInstance().save();
                } 
                NavigationService.GoBack();
        }



        private void editGesture_Tap(object sender, RoutedEventArgs e)
        {
            if (selectedContactData != null)
            {

                NavigationService.Navigate(new Uri("/gbsd.ui/AddGesturePage.xaml?Name=" + selectedContactData.DisplayName + "&Number=" + selectedContactData.Number + "&Gesture=" + selectedContactData.Gesture, UriKind.Relative));
            }
        }
        

        
    }
}