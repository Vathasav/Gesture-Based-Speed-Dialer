
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
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Notification;
using System.Windows.Navigation;
using Microsoft.Xna.Framework;
using Microsoft.Devices;
using System.Windows.Threading;
using GestureTest.gbsd.core;


namespace GestureTest
{
    public partial class GestureMainPage : PhoneApplicationPage
    {

        PhoneCallTask phoneTask = new PhoneCallTask();
        SmsComposeTask smsComposeTask = new SmsComposeTask();
        PhoneNumberChooserTask phoneNumberChooserTask = new PhoneNumberChooserTask();
        public static MoveGesture gestureControl;
        LocationInfo locationTask = new LocationInfo();

        //public static Accelerometer accelerometer { get; set; }






        //private string gesture { get; set; }

        public ContactData gestureData_i { get; set; }

        public GestureMainPage()
        {
            gestureControl = new MoveGesture();//(Dispatcher);

            InitializeComponent();

            InitializeSettings();

            (Application.Current as App).RootFrame.Obscured += OnObscured;
            (Application.Current as App).RootFrame.Unobscured += OnUnobscured;

            Debug.writeToIO("GesMainPage Constructor");
            phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);
            gestureControl.onMultiGesturesEvent += onMultiGestures_Event;

        }

        ~GestureMainPage()
        {
            phoneNumberChooserTask.Completed -= new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);
            gestureControl.onMultiGesturesEvent -= onMultiGestures_Event;

        }

        void OnObscured(object sender, ObscuredEventArgs e)
        {
            //Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.Active = false;
            Debug.writeToIO("Obscured");
            //accelerometer.Stop();

        }

        void OnUnobscured(object sender, EventArgs e)
        {
            gestureControl.startAccelerometer();
            // Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.Active = true;

        }

        void phoneNumberChooserTask_Completed(object sender, PhoneNumberResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {

                String displayName = e.DisplayName;
                Debug.writeToIO("Name: " + e.DisplayName);
                String number = "" + e.PhoneNumber;
                Debug.writeToIO("Phone Number: " + e.PhoneNumber);


                Navigate(displayName, number);

            }


        }

        private void vibrateFeedback()
        {
            VibrateController.Default.Start(TimeSpan.FromMilliseconds(100));

        }







        private void performAction(string gesture)
        {
            /*long previousTS = previousTimeStamp;
            long nowTimeStamp = getTimeMiliseconds();
            
            previousTimeStamp = nowTimeStamp;

           if (nowTimeStamp - previousTS < 500)
            {
                return;
            }*/



            if (GesContactsDB.getInstance().getContactData(gesture) != null)
            {
                gestureControl.stopAccelerometer();
                vibrateFeedback();


                // Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.Active = false;

                ContactData gestureData = GesContactsDB.getInstance().getContactData(gesture);

                if (gestureData == null)
                {
                    return;
                }

                if (gestureData.settings.connectSetting == ContactSettings.CallContact)
                {
                    phoneTask.DisplayName = gestureData.DisplayName;
                    phoneTask.PhoneNumber = gestureData.Number;
                    phoneTask.Show();

                }
                else if (gestureData.settings.connectSetting == ContactSettings.WriteMessage)
                {
                    smsComposeTask.To = gestureData.DisplayName + "<" + gestureData.Number + ">"; // Mention here the phone number to whom the sms is to be sent
                    smsComposeTask.Body = ""; // the string containing the sms body
                    smsComposeTask.Show();
                }
                else if (gestureData.settings.connectSetting == ContactSettings.SendDistressMessage)
                {
                    gestureData_i = gestureData;
                    //LocationInfo location = new LocationInfo();
                    this.statusBarTB.Text = "Please wait, getting GPS location data...";
                    locationTask.onLocationDataAvailableEvent += onLocationDataAvailable;
                    locationTask.start();

                    /*string latitude = locationTask.latitude;
                    string longitude = locationTask.longitude;

                    string location = "location:{" + latitude + "," + longitude + "}";
                    smsComposeTask.To = gestureData.DisplayName + "<" + gestureData.Number + ">"; // Mention here the phone number to whom the sms is to be sent
                    smsComposeTask.Body = "I need help, "+location; // the string containing the sms body
                    smsComposeTask.Show();*/

                  //  locationTask.stopWatcher();
                }

            }


        }


        void Navigate(String displayName, String number)
        {
            gestureControl.stopAccelerometer();

            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/gbsd.ui/AddGesturePage.xaml?Name=" + displayName + "&Number=" + number, UriKind.Relative));
            });

        }


        private void InitializeSettings()
        {

            GesContactsDB.getInstance().load();
            this.listBox.ItemsSource = GesContactsDB.getInstance().getContactsList();

        }



        private void saveData()
        {

            GesContactsDB.getInstance().save();

        }










        private void create_click(object sender, RoutedEventArgs e)
        {


            phoneNumberChooserTask.Show();


        }


        private void GestureMainPage_Loaded(object sender, RoutedEventArgs e)
        {

            gestureControl.startAccelerometer();

            this.listBox.ItemsSource = GesContactsDB.getInstance().getContactsList();

        }

        void onMultiGestures_Event(object sender, CustomEventArgs e)
        {
            Dispatcher.BeginInvoke(() => performAction(e.Gesture));
            //updateCircles2(e.Gesture));
        }

        /*  private void deletegesture_click(object sender, RoutedEventArgs e)
          {
              try
              {
                  // int selectedIndex = YourListBoxItemCollection.IndexOf((sender as MenuItem).DataContext)
                  //ListBoxItem contextMenuListItem = this.listBox.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;
                  //ContactData gestureData = (ContactData)contextMenuListItem.DataContext;

                  object item = ((ListBox)sender).SelectedItem;

                  ContactData gestureData = null;

                  if (item is ContactData)
                  {
                      gestureData = (ContactData)item;
                      GesContactsDB.getInstance().remove(gestureData.Gesture);
                      GesContactsDB.getInstance().save();

                      this.listBox.ItemsSource = GesContactsDB.getInstance().getContactsList();
                  }

              }
              catch (NullReferenceException exception)
              {
                  Debug.writeToIO(exception.Message);
              }
          }

 

          private void editgesture_Click(object sender, RoutedEventArgs e)
          {
              if (sender != null)
              {
                  //ListBoxItem contextMenuListItem = this.listBox.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;
                  //ContactData gestureData_l = (ContactData)contextMenuListItem.DataContext;
                  object item = ((ListBox)sender).SelectedItem;

                  ContactData gestureData_l = null;

                  if (item is ContactData)
                  {
                      gestureData_l = (ContactData)item;
                      NavigationService.Navigate(new Uri("/AddGesturePage.xaml?Name=" + gestureData_l.DisplayName + "&Number=" + gestureData_l.Number + "&Gesture=" + gestureData_l.Gesture, UriKind.Relative));
                  }
                

                
              }
          }
          */
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {

        }

        private void barSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/gbsd.ui/Settings.xaml", UriKind.Relative));
        }

        private void barAdd_Click(object sender, EventArgs e)
        {
            phoneNumberChooserTask.Show();
        }

        private void MenuButton_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void contactsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = ((ListBox)sender).SelectedItem;
            //object value = ((ListBox)sender).SelectedValue;
            ContactData contactData;

            if (item is ContactData)
            {
                contactData = ((ContactData)item);
                NavigationService.Navigate(new Uri("/gbsd.ui/settings.xaml?Gesture=" + contactData.Gesture + "&Name=" + contactData.DisplayName, UriKind.Relative));
            }


        }


        public void onLocationDataAvailable(object sender,LocationEventArgs args)
        {
            string latitude = locationTask.latitude;
            string longitude = locationTask.longitude;
            this.statusBarTB.Text = "Data acquired...";

            locationTask.stopWatcher();

            String location = "";
            if (!String.IsNullOrEmpty(latitude) && !String.IsNullOrEmpty(longitude))
            {
                location = "location:{" + latitude + "," + longitude + "}";
            }
            smsComposeTask.To = gestureData_i.DisplayName + "<" + gestureData_i.Number + ">"; // Mention here the phone number to whom the sms is to be sent
            smsComposeTask.Body = "I need help, " + location; // the string containing the sms body
            try
            {
                smsComposeTask.Show();
            }
            catch (InvalidOperationException exception)
            {
                
            }
            this.statusBarTB.Text = "";
        }

    }
    /*        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // NavigationEventArgs returns destination page "DestinationPage"
            AddGesturePage addPage = e.Content as AddGesturePage;
            if (addPage != null)
            {
                // Change property of destination page 
               // addPage.accelerometer = accelerometer;
            }

        }
       void startAccelerometerHelper()
        {
          

            if (!Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.Active)
            {
               // Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.ReadingChanged -= Accelerometer_ReadingChanged;
                Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.ReadingChanged += Accelerometer_ReadingChanged;
                Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.Active = true;
            }
           
        }*/
}