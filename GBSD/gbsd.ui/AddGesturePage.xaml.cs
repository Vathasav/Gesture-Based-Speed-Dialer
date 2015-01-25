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
using System.Windows.Navigation;
using Microsoft.Devices.Sensors;
using Microsoft.Devices;
using System.IO.IsolatedStorage;

namespace GestureTest
{
    public partial class AddGesturePage : PhoneApplicationPage
    {
        public static ContactData gestureData { get; set; }

        public string gesture { get; set; }

        private string displayName { get; set; }

        private string displayNumber { get; set; }

        private string  gestureToBeEdited { get; set; }

        //Accelerometer accelerometer;

        MoveGesture gestureControl;// = new MoveGesture(Dispatcher);

        public AddGesturePage()
        {
            gestureControl = new MoveGesture();// new MoveGesture(Dispatcher);

            InitializeComponent();
            gestureControl.onSingleGestureEvent += onGesture_Event;
            
          
        }

        ~AddGesturePage()
        {
            gestureControl.onMultiGesturesEvent += onGesture_Event; 
        }

        /*void startAccelerometer()
        {
            if (accelerometer == null)
            {
                // Instantiate the Accelerometer.
                accelerometer = new Accelerometer();
                //time between updates
                //    accelerometer.TimeBetweenUpdates = TimeSpan.FromSeconds(2);
                accelerometer.CurrentValueChanged +=
                    new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);

                try
                {

                    accelerometer.Start();
                    Debug.writeToIO("Accelerometer started");
                }
                catch (InvalidOperationException exception)
                {
                    Debug.writeToIO( exception.Message);
                }
            }
            else
                accelerometer.Start();
        }*/

        void onGesture_Event(object sender, CustomEventArgs e)
        {
            Dispatcher.BeginInvoke(() => updateCircles2(e.Gesture));
        }

        private void updateCircles2(String gesture)
        {
            var uri = NavigationService.CurrentSource;
            String uriString = uri.ToString();

            if (uriString.Contains("/gbsd.ui/AddGesturePage.xaml"))
            {

                // Pass the AccelerometerReading.
                //MoveGesture recordReading = new MoveGesture(e.Acceleration);


                //string gesture = gestureArg;// "";
                if (String.Equals(gesture, ContactData.LeftGesture))//recordReading.isLeftTilt())
                {
                    //gesture = ContactData.LeftGesture;
                    this.resetEllipsesColor();

                    if (GesContactsDB.getInstance().getContactData(gesture) == null)
                    {

                        this.leftEllipse.Fill = new SolidColorBrush(Colors.Green);

                    }
                    else
                    {

                        this.leftEllipse.Fill = new SolidColorBrush(Colors.Red);


                    }

                    updateRecordedGesture(ContactData.LeftGesture);

                }

                else if (String.Equals(gesture,ContactData.RightGesture))//recordReading.isRightTilt())
                {
                    //gesture = ContactData.RightGesture;
                    this.resetEllipsesColor();
                    if (GesContactsDB.getInstance().getContactData(gesture) == null)
                    {

                        this.rightEllipse.Fill = new SolidColorBrush(Colors.Green);

                    }
                    else
                    {

                        this.rightEllipse.Fill = new SolidColorBrush(Colors.Red);


                    }
                    updateRecordedGesture(ContactData.RightGesture);

                }

                else if (String.Equals(gesture,ContactData.UpGesture))//recordReading.isUpTilt())
                {
                    //gesture = ContactData.UpGesture;
                    this.resetEllipsesColor();

                    if (GesContactsDB.getInstance().getContactData(gesture) == null)
                    {

                        this.upEllipse.Fill = new SolidColorBrush(Colors.Green);

                    }
                    else
                    {

                        this.upEllipse.Fill = new SolidColorBrush(Colors.Red);


                    }
                    updateRecordedGesture(ContactData.UpGesture);


                }

                else if (String.Equals(gesture,ContactData.DownGesture))//recordReading.isDownTilt())
                {

                    //gesture = ContactData.DownGesture;
                    this.resetEllipsesColor();

                    if (GesContactsDB.getInstance().getContactData(gesture) == null)
                    {

                        this.downEllipse.Fill = new SolidColorBrush(Colors.Green);

                    }
                    else
                    {

                        this.downEllipse.Fill = new SolidColorBrush(Colors.Red);


                    }
                    updateRecordedGesture(ContactData.DownGesture);
                }


                /*if (!String.IsNullOrEmpty(gesture))
                {
                    if (GesContactsDB.getInstance().getContactData(gesture) == null)
                    {
                        this.resetEllipsesColor();
                        this.leftEllipse.Fill = new SolidColorBrush(Colors.Green);
                        updateRecordedGesture(ContactData.LeftGesture);
                    }
                    else
                    {
                        this.resetEllipsesColor();
                        this.leftEllipse.Fill = new SolidColorBrush(Colors.Red);
                        updateRecordedGesture(ContactData.LeftGesture);
                 
                    }
                }*/






            }
        }



     public void updateRecordedGesture(String gesture)
     {
         
         this.gesture = gesture;
     }
        public void resetEllipsesColor(){
            this.downEllipse.Fill = new SolidColorBrush(Colors.White);
            this.upEllipse.Fill = new SolidColorBrush(Colors.White);
            this.leftEllipse.Fill = new SolidColorBrush(Colors.White);
            this.rightEllipse.Fill = new SolidColorBrush(Colors.White);
        }



 
        private bool checkGestureNumber(String number)
        {
            
                foreach (var eachGesture in GesContactsDB.getInstance().getContactsList())
                {
                    if (eachGesture.Number.Equals(number))
                        return true;
                }
           
          return false;
        }
       
      
        private void AddGesturePage_Loaded(object sender, RoutedEventArgs e)
        {
            this.displayName = NavigationContext.QueryString["Name"];
            this.displayNumber = NavigationContext.QueryString["Number"];

            // if the passing string contains + in the beginning navigation context
            // is replacing with whitespace " "

            //removing whitespace and adding "+"
            int index = displayNumber.IndexOf(" ", 0, displayNumber.Length);

            if (index == 0)
            {
                this.displayNumber = "+" + displayNumber.TrimStart();
            }
            gestureToBeEdited = null;
            if (NavigationContext.QueryString.Keys.Contains("Gesture"))
            {
                gestureToBeEdited = NavigationContext.QueryString["Gesture"];
            }



            this.textToDisplay.Text = "Select a gesture for\n" + this.displayName + "\nby tilting the phone";
            gestureControl.startAccelerometer();

            

        }

    
       private void appbar_saveclick(object sender, EventArgs e)
       {
           if (String.IsNullOrEmpty(gesture))
           {

               MessageBox.Show("Please record a new gesture", "Info", MessageBoxButton.OK
                   );

           }

           if (!String.IsNullOrEmpty(gesture))
           {

               if (GesContactsDB.getInstance().getContactData(gesture) == null)
               {
                   if (gestureToBeEdited != null)
                   {
                       GesContactsDB.getInstance().remove(gestureToBeEdited);
                   }
                       String action = "";
                       gestureData = new ContactData() { DisplayName = this.displayName, Number = this.displayNumber, Gesture = this.gesture, Action = action };
                       GesContactsDB.getInstance().add(this.gesture, gestureData);
                       GesContactsDB.getInstance().save();
                   

                   gestureControl.stopAccelerometer();

                   NavigationService.GoBack();
               }
               else
               {
                   MessageBox.Show("Gesture already exists", "Info", MessageBoxButton.OK
                   );
               }

           }
       }

       private void appbar_cancelclick(object sender, EventArgs e)
       {
           NavigationService.GoBack();
       }

     
    }





    /*   private void updateEllipse(string gesture)
       {
           if (gesture == ContactData.LeftGesture)
               this.leftEllipse.Fill = new SolidColorBrush(Colors.Green);
           else if (gesture == ContactData.RightGesture)
               this.rightEllipse.Fill = new SolidColorBrush(Colors.Green);
           else if (gesture == ContactData.UpGesture)
               this.upEllipse.Fill = new SolidColorBrush(Colors.Green);
           else if (gesture == ContactData.DownGesture)
               this.downEllipse.Fill = new SolidColorBrush(Colors.Green);
            
       }
        void addGesturePage_readingChanged(object sender,
 Microsoft.Phone.Applications.Common.AccelerometerHelperReadingEventArgs e)
        {
         Dispatcher.BeginInvoke(() => updateCircles2(e));
        }

        
      protected override void OnNavigatedFrom(NavigationEventArgs e)
       {
           // NavigationEventArgs returns destination page "DestinationPage"
           GestureMainPage dPage = e.Content as GestureMainPage;
           if (dPage != null)
           {
               // Change property of destination page 
               dPage.gestureData_i = gestureData;
           }
          // Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.ReadingChanged -= addGesturePage_readingChanged;
         //  Microsoft.Phone.Applications.Common.AccelerometerHelper.Instance.Active = true;
       }*/

     
}