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
using System.Device.Location;

namespace GestureTest.gbsd.core
{
    public class LocationEventArgs : EventArgs
    {
        public LocationEventArgs(double lat, double lon)
        {
            latitude=lat;
            longitude = lon;
        }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }


    public class LocationInfo
    {

        public event EventHandler<LocationEventArgs> onLocationDataAvailableEvent;

        //[msdn]
        protected virtual void onLocationDataAvailable(LocationEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<LocationEventArgs> handler = onLocationDataAvailableEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                //e.Gesture += String.Format(" at {0}", DateTime.Now.ToString());

                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        GeoCoordinateWatcher watcher;

        public string latitude { get; set; }

        public string longitude { get; set; }

        public LocationInfo()
        {

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High); // using high accuracy
            watcher.MovementThreshold = 20; // use MovementThreshold to ignore noise in the signal
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            
          //  watcher.Start();
        }

        ~LocationInfo()
        {
            watcher.StatusChanged -= new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged -= new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
          //  watcher.Stop();
        }
        public void start()
        {
            watcher.Start();
        }

        // Event handler for the GeoCoordinateWatcher.StatusChanged event.
        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled or unsupported.
                    // Check to see whether the user has disabled the Location Service.
                    if (watcher.Permission == GeoPositionPermission.Denied)
                    {
                        // The user has disabled the Location Service on their device.
                        MessageBox.Show("Location service is disabled on the device.");

                        onLocationDataAvailable(new LocationEventArgs(0, 0));
                    }
                    else
                    {
                        MessageBox.Show("Location is not functioning on this device");

                    }
                    break;

                case GeoPositionStatus.Initializing:
                    // The Location Service is initializing.
                    // Disable the Start Location button.

                    break;

                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    // Alert the user and enable the Stop Location button.
                    // MessageBox.Show("location data is not available.");

                    break;

                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    // Show the current position and enable the Stop Location button.
                    //   MessageBox.Show("location data is not available.");
                  //  MessageBox.Show("getting location data");
                    

                    break;
            }
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (watcher.Status.Equals(GeoPositionStatus.Ready) )
            {
               
              
                latitude = e.Position.Location.Latitude.ToString();
                longitude = e.Position.Location.Longitude.ToString();
                
                
                if (!String.IsNullOrEmpty(latitude) && !String.IsNullOrEmpty(longitude))
                {

                    onLocationDataAvailable(new LocationEventArgs(e.Position.Location.Latitude, e.Position.Location.Longitude));
                }
            }
        }

        public void stopWatcher()
        {
            watcher.Stop();
        }
    }
}
