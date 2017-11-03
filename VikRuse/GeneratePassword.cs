using System;

namespace VikRuse
{
    public class GeneratePassword
    {
        public string secretPass = "vasP#df40176_gooW";

        public string GetDeviceName()
        {
            // var ds = DateTime.Now.Ticks;

            //string manufacturer = null ;//Build.Manufacturer;

            //string model = null;// Build.Model;

            var device = UIKit.UIDevice.CurrentDevice;

            string model = device.SystemName;  /// .name

            string manufacturer = device.Model;


            if (model.StartsWith(manufacturer))
            {
                return model;
                //  return capitalize(model);
            }
            else
            {
                return manufacturer + model;
                // return capitalize(manufacturer) + " " + model; //"Samsung GT-N8010"             }         }
            }
        }

        public string GetDateTimeTiks()
        {
            var currentTiks = DateTime.Now.Ticks;

            return currentTiks.ToString();

        }
    }
}