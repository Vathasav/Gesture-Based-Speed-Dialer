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
using System.Collections.Generic;
using System.IO.IsolatedStorage;

namespace GestureTest
{
    public class SettingsDB
    {

        public static SettingsDB gesContactDB = null;
        public static SettingsDB getInstance()
        {
            if (gesContactDB == null)
            {
                gesContactDB = new SettingsDB();
            }

            return gesContactDB;
        }


        private  Dictionary<string, ContactData> gestureToContactMap = new Dictionary<string,ContactData>();
        public const string DeviceDbKey = "GestureContactDB";
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        /*public void initializeDB(Dictionary<string, ContactData> db)
        {
            gestureToContactMap = db;
        }*/


        public Boolean add(string gesture, ContactData contact)
        {
            if (!gestureToContactMap.ContainsKey(gesture))
            {
                gestureToContactMap.Add(gesture, contact);
                return true;
            }

            return false;
        }

        public Boolean remove(string gesture)
        {
            return gestureToContactMap.Remove(gesture);            
        }

        public Boolean modify(string gesture, ContactData contact)
        {
            this.remove(gesture);
            return this.add(gesture, contact);
        }

        public ContactData getContactData(string gesture)
        {
            if (gestureToContactMap.ContainsKey(gesture))
            {
                return gestureToContactMap[gesture];
            }

            return null;
        }

        public int getSize()
        {
            return gestureToContactMap.Count;
        }

        public void load()
        {
            if (settings.Contains(DeviceDbKey))
            {
                gestureToContactMap = ((Dictionary<string, ContactData>)settings[DeviceDbKey]);
            }        
        }


        public void save()
        {
            if (settings.Contains(DeviceDbKey))
            {
                settings[DeviceDbKey] = gestureToContactMap;
            }
            else
            {
                settings.Add(DeviceDbKey, gestureToContactMap);
            }

            settings.Save();
        
        }

        public List<ContactData> getContactsList()
        {
            ContactData[] contactsCopy = new ContactData[gestureToContactMap.Count];
            
            gestureToContactMap.Values.CopyTo(contactsCopy, 0);
            return new List<ContactData>(contactsCopy);
        }
 



    }
}
