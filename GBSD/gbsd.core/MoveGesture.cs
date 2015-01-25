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
using Microsoft.Devices.Sensors;
using System.Windows.Threading;
using System.Windows.Navigation;


namespace GestureTest
{
    //[msdn]
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(string s)
        {
            gesture = s;
        }
        private string gesture;

        public string Gesture
        {
            get { return gesture; }
            set { gesture = value; }
        }
    }

    

    public class MoveGesture
    {
        // [msdn] Declare the event using EventHandler<T>
        public event EventHandler<CustomEventArgs> onMultiGesturesEvent;
        public event EventHandler<CustomEventArgs> onSingleGestureEvent;

        long previousGestureTimeStamp = 0;
        string previousGesture = "";
        //string Gesture = "";

        public Accelerometer accelerometer=new Accelerometer();// { get; set; }
        TiltGestures tiltGestures = new TiltGestures();
        DispatcherTimer timer = new DispatcherTimer();

        //time constants
        const int MultiMovesGestureDuration = 1000;
        const int AccelerometerDataDelay = 450;

        const long IdleTimeLimit = 700;

        Dispatcher dispatcher;


        public MoveGesture()
        {
            //dispatcher = disp;
            //time between updates
            //    accelerometer.TimeBetweenUpdates = TimeSpan.FromSeconds(2);
            accelerometer.CurrentValueChanged +=
                new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);


            //timer.Interval = new TimeSpan(0, 0, 0,0, MultiMovesGestureDuration);
            //timer.Tick += onTimerTick;

        }

      /*  public MoveGesture(Dispatcher  disp)
        {
            dispatcher = disp;
            //time between updates
            //    accelerometer.TimeBetweenUpdates = TimeSpan.FromSeconds(2);
            accelerometer.CurrentValueChanged +=
                new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);


            //timer.Interval = new TimeSpan(0, 0, 0,0, MultiMovesGestureDuration);
            //timer.Tick += onTimerTick;

        }*/

       

         ~MoveGesture()
        {
            accelerometer.Stop();
            accelerometer.CurrentValueChanged -=
               new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
            //timer.Tick -= onTimerTick;
            
        }

         Boolean firstGesture = true;
         Boolean secondGesture = false;
         Boolean thirdGesture = false;

         string firstGes = "";
         string secondGes = "";
         string thridGes = "";

         private void clearGesturesHistory()
         {
             previousGesture = "";

             firstGes = "";
             secondGes = "";
             thridGes = "";

             firstGesture = true;
             secondGesture = false;
             thirdGesture = false;

             if (timer.IsEnabled)
             {
                 timer.Stop();
             }

         }

         

         private Boolean isMovePartOfMultiMovesGesture()
         {
             
             long nowTimeStamp = getTimeMiliseconds();
             long idleTime = nowTimeStamp - previousGestureTimeStamp;

             if (idleTime > IdleTimeLimit)
             {
                 
                 Debug.writeToIO("Not a Multi move-Gesture.");
                 return false;
             }

             return true;           
         }

         void onTimerTick(object sender, EventArgs e)
         {
             timer.Stop();

             if (thirdGesture)
             {
                 if (String.Equals(firstGes, thridGes) && String.Equals(secondGes, ContactData.NoGesture))
                 {
                     Debug.writeToIO("-----------Recorded multi-gesture::--" + firstGes);
                     onMultiGestures(new CustomEventArgs(firstGes));
                 }
                 else
                 {
                     Debug.writeToIO("------------first and third gesture does not match");
                         
                                             
                 }


             }
             else
             {
                 Debug.writeToIO("----------------- Three gestures not happend");
             }

             clearGesturesHistory();

         }

         void firstGestureMove()
         {
             if (timer.IsEnabled)
             {
                 timer.Stop();
             }
             else
             {
                 timer.Start();
             }
         }

         long resetHistoryTimeKeeper = 0;
         bool executing = false;

         void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
         {
             /*dispatcher.BeginInvoke(() => UpdateUI2(e.SensorReading));
         }

        void UpdateUI2(AccelerometerReading e)
             {*/
             //var uri = NavigationService.CurrentSource;
             //String uriString = uri.ToString();

             // Call UpdateUI on the UI thread and pass the AccelerometerReading.
             //if (uriString.Contains("/GestureMainPage.xaml"))
             //{
             //  Vector3 acceleration = e.AverageAcceleration.;
             //Debug.writeToIO("Acceleration X value" + e.Acceleration.X);
             //Debug.writeToIO("Acceleration Y value" + e.Acceleration.Y);
             //Debug.writeToIO("Acceleration Z value" + e.Acceleration.Z);

             //MoveGesture accelerationRead = new MoveGesture(e.Acceleration
             //  );


             //this.accelReading = e.SensorReading.Acceleration;

             if (!executing)//one thread at a time
             {
                 tiltGestures.accelReading = e.SensorReading.Acceleration;

                 if (isGesture())
                 {
                     onSingleGesture(new CustomEventArgs(getGesture()));
                 }


                 //if (isGesture())
                 //{

                 //single move gesture

                 Debug.writeToIO("Gesture detected");
                 string recordGesture = "";
                 recordGesture = getGesture();

                 string previousGes = previousGesture;
                 string currentGesture = recordGesture;
                 previousGesture = currentGesture;

                 long previousTS = previousGestureTimeStamp;
                 long nowTimeStamp = getTimeMiliseconds();
                 previousGestureTimeStamp = nowTimeStamp;
                 long timeSpan = nowTimeStamp - previousTS;

                 const long GestureGapLimit = 200;

                 Debug.writeToIO("Recorded gesture::--" + recordGesture);
                 if (previousGes.Equals(currentGesture) && timeSpan <= GestureGapLimit)
                 {
                     Debug.writeToIO("Same Gesture : " + recordGesture);
                     resetHistoryTimeKeeper += timeSpan;

                     if (((double)resetHistoryTimeKeeper) >= (3.8 * ((double)GestureGapLimit)))
                     {
                         resetHistoryTimeKeeper = 0;
                         Debug.writeToIO("History Reset Occured");

                         firstGes = "";
                         secondGes = "";

                     }
                     else
                     {
                         Debug.writeToIO("History Reset not Occured");
                     }

                     return;
                 }




                 firstGes = secondGes;
                 secondGes = thridGes;
                 thridGes = currentGesture;

                 Debug.writeToIO2("Ges Stream:" + firstGes + " , " + secondGes + " , " + thridGes);

                 if (thridGes.Equals(firstGes) && isGesture())
                 {
                     firstGes = "";
                     secondGes = "";
                     onMultiGestures(new CustomEventArgs(recordGesture));
                 }

                 /*if (previousGes.Equals(currentGesture))
                 {
                     Debug.writeToIO("Timespan::--" + timeSpan);
                     if (timeSpan > 700 && timeSpan < 1200)//double click
                     {

                         Debug.writeToIO("Recorded gesture::--" + recordGesture);
                         onMultiGestures(new CustomEventArgs(recordGesture));

                         //performAction(recordGesture);
                     }
                 }
                 // }
                 else
                 {
                     Debug.writeToIO("Not a Gesture.");
                     long nowTimeStamp = getTimeMiliseconds();
                     long idleTime = nowTimeStamp - previousGestureTimeStamp;

                     if (idleTime > 700)
                     {
                         previousGesture = "";
                     }
                 }*/

                 executing = false;
             }

         }




            

        

  /*      void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            dispatcher.BeginInvoke(() => UpdateUI2(e.SensorReading));
        }

        void UpdateUI2(AccelerometerReading e)
        {
            //var uri = NavigationService.CurrentSource;
            //String uriString = uri.ToString();

            // Call UpdateUI on the UI thread and pass the AccelerometerReading.
            //if (uriString.Contains("/GestureMainPage.xaml"))
            //{
            //  Vector3 acceleration = e.AverageAcceleration.;
            //Debug.writeToIO("Acceleration X value" + e.Acceleration.X);
            //Debug.writeToIO("Acceleration Y value" + e.Acceleration.Y);
            //Debug.writeToIO("Acceleration Z value" + e.Acceleration.Z);

            //MoveGesture accelerationRead = new MoveGesture(e.Acceleration
            //  );


            //this.accelReading = e.SensorReading.Acceleration;
            tiltGestures.accelReading = e.Acceleration;


            if (isGesture())
            {

                //single move gesture
                Debug.writeToIO("Gesture detected");
                string currentGesture = getGesture();
                onSingleGesture(new CustomEventArgs(currentGesture));

                //long previousTS = previousGestureTimeStamp;
                //long nowTimeStamp = getTimeMiliseconds();



                if (firstGesture)
                {
                    Debug.writeToIO("First gesture::" + currentGesture);

                    firstGesture = false;
                    secondGesture = true;

                    firstGes = currentGesture;



                    firstGestureMove();

                }
                else if (isMovePartOfMultiMovesGesture())
                {

                    if (secondGesture)
                    {
                        secondGesture = false;
                        thirdGesture = true;

                        secondGes = currentGesture;

                        Debug.writeToIO("Second gesture::" + firstGes + " , " + currentGesture);

                    }
                    else if (thirdGesture)
                    {
                        thirdGesture = false;
                        firstGesture = true;

                        thridGes = currentGesture;

                        Debug.writeToIO("Third gesture::" + firstGes + " , " + secondGes + " , " + currentGesture);
                    }

                }

                else
                {
                    clearGesturesHistory();
                }




                /*        string recordGesture = "";
                        recordGesture = getGesture();

                    

                        string previousGes = previousGesture;
                        string currentGesture = recordGesture;
                        previousGesture = currentGesture;

                        long previousTS = previousGestureTimeStamp;
                        long nowTimeStamp = getTimeMiliseconds();
                        previousGestureTimeStamp = nowTimeStamp;
                        long timeSpan = nowTimeStamp - previousTS;


                        if (previousGes.Equals(currentGesture))
                        {
                            Debug.writeToIO("Timespan::--" + timeSpan);
                            if (timeSpan > 700 && timeSpan < 1200)//double click
                            {

                                Debug.writeToIO("Recorded gesture::--" + recordGesture);
                                onMultiGestures(new CustomEventArgs(recordGesture));

                                //performAction(recordGesture);
                            }
                        }
                    }
                else
                {
                    Debug.writeToIO("Not a Gesture.");
                    long nowTimeStamp = getTimeMiliseconds();
                    long idleTime = nowTimeStamp - previousGestureTimeStamp;

                    if (idleTime > 700)
                    {
                        previousGesture = "";
                    }
                }



                previousGestureTimeStamp = getTimeMiliseconds();
            }




            //}

        }*/

        private long getTimeMiliseconds()
        {
            return DateTime.Now.Ticks / 10000;
        }

        public void startAccelerometer()
        {
                
                //TimeSpan delay = new TimeSpan(0, 0, 0, 0, AccelerometerDataDelay);
                //accelerometer.TimeBetweenUpdates = delay;


                

                try
                {

                    accelerometer.Start();

                    Debug.writeToIO("Accelerometer started");
                }
                catch (InvalidOperationException ex)
                {

                }
          
        }


        //[msdn]
        protected virtual void onMultiGestures(CustomEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = onMultiGesturesEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                //e.Gesture += String.Format(" at {0}", DateTime.Now.ToString());

                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        //[msdn]
        protected virtual void onSingleGesture(CustomEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = onSingleGestureEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                //e.Gesture += String.Format(" at {0}", DateTime.Now.ToString());

                // Use the () operator to raise the event.
                handler(this, e);
            }
        }


        

       

        public Boolean isGesture()
        {
            string recordGesture = "";

            if (tiltGestures.isLeftTilt())
            {
                recordGesture = ContactData.LeftGesture;


            }
            else if (tiltGestures.isRightTilt())
            {

                recordGesture = ContactData.RightGesture;

            }
            else if (tiltGestures.isDownTilt())
            {
                recordGesture = ContactData.DownGesture;


            }
            else if (tiltGestures.isUpTilt())
            {
                recordGesture = ContactData.UpGesture;
            }
            /*else 
            {
                return true;
            }*/

            if (!String.IsNullOrEmpty(recordGesture))
            {
                return true;
            }

            return false;

        }

        public string getGesture()
        {
            string recordGesture = "";

            if (tiltGestures.isLeftTilt())
            {
                recordGesture = ContactData.LeftGesture;


            }
            else if (tiltGestures.isRightTilt())
            {

                recordGesture = ContactData.RightGesture;

            }
            else if (tiltGestures.isDownTilt())
            {
                recordGesture = ContactData.DownGesture;


            }
            else if (tiltGestures.isUpTilt())
            {
                recordGesture = ContactData.UpGesture;
            }
            else
            {
                recordGesture = ContactData.NoGesture;
            }

            return recordGesture;

        }



        public void stopAccelerometer()
        {
            if (accelerometer != null)
            {
                // Instantiate the Accelerometer.
                //accelerometer = new Accelerometer();
                //TimeSpan delay = new TimeSpan(0, 0, 0, 0, 400);
                //accelerometer.TimeBetweenUpdates = delay;


                //time between updates
                //    accelerometer.TimeBetweenUpdates = TimeSpan.FromSeconds(2);
               //accelerometer.CurrentValueChanged -=
                 //   new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);

                try
                {

                    accelerometer.Stop();

                    Debug.writeToIO("Accelerometer stopped");
                }
                catch (InvalidOperationException ex)
                {
                    
                }
            }
            
        }
    }
}
