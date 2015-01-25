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
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Phone.Tasks;

namespace GestureTest
{
    public partial class MainPage : PhoneApplicationPage
    {

        Accelerometer accelerometer;
        PhoneCallTask phoneTask = null;// Constructor
       PhoneNumberChooserTask phoneNumberChooserTask;
        // Constructor
        public MainPage()
        {
            InitializeComponent();


             phoneNumberChooserTask = new PhoneNumberChooserTask();
             phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);
            phoneTask = new PhoneCallTask();

            // Set the data context of the listbox control to the sample data
          
          

            //  this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }


        void startAccelerometer(){
            if (accelerometer == null)
            {
                // Instantiate the Accelerometer.
                accelerometer = new Accelerometer();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                accelerometer.CurrentValueChanged +=
                    new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);

                try
                {

                    accelerometer.Start();
                   Debug.writeToIO("Accelerometer started");
                }
                catch (InvalidOperationException ex)
                {

                }
            }
            }


        void phoneNumberChooserTask_Completed(object sender, PhoneNumberResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //Code to start a new phone call using the retrieved phone number.
                phoneTask.DisplayName = e.DisplayName;
                phoneTask.PhoneNumber = e.PhoneNumber;



                phoneTask.Show();
            }
        }

        private void UpdateUI(AccelerometerReading accelerometerReading)
        {
            Vector3 acceleration = accelerometerReading.Acceleration;
            Debug.writeToIO("Acceleration x value"+acceleration.X);
            if (acceleration.X > 0.5)
            {
                // button1.Click;

                try
                {

                    accelerometer.Stop();


                    PhoneCallTask MyPCT = new PhoneCallTask();

                    MyPCT.DisplayName = nametextBox.Text; // "Amit Maheshwari";

                    MyPCT.PhoneNumber = numbertextBox.Text;//"9412331585";

                    MyPCT.Show();
                    // phoneNumberChooserTask = new PhoneNumberChooserTask();
                     //phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);
               //     phoneTask.DisplayName = nametextBox.Text;
                 //   phoneTask.PhoneNumber = numbertextBox.Text;



                   // phoneTask.Show();
                    
                   
                }
                catch (Exception ex)
                {

                }
            }

        }
        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            // Call UpdateUI on the UI thread and pass the AccelerometerReading.
            Dispatcher.BeginInvoke(() => UpdateUI(e.SensorReading));


        }

      
        private void button1_Click(object sender, RoutedEventArgs e)
        {

            startAccelerometer();

            



           // phoneNumberChooserTask = new PhoneNumberChooserTask();
             //        phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);


            //phoneNumberChooserTask.Show();
           // PanoramaView.DefaultItem = PanoramaView.Items[0];
            //  if (accelerometer != null)
            // {

            // Stop the accelerometer.
            // accelerometer.Stop();
            // statusTextBlock.Text = "accelerometer stopped.";
            //}


        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/gbsd.ui/page1.xaml", UriKind.Relative));
        }

        
    }
}