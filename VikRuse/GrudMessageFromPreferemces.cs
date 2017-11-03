using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.IO;
using Newtonsoft.Json;

namespace VikRuse
{
  public class GrudMessageFromPreferemces
    {
        private static string mDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string mMessageFilename = Path.Combine(mDocuments, "MessageFromApi.txt");

        private Message message;

        public Message GetMessageFromPreferencesInPhone()
        {
            string messageAsString = string.Empty;

            try
            {
                messageAsString = File.ReadAllText(mMessageFilename);
                

                if (messageAsString == null || messageAsString == string.Empty)
                {
                    return new Message();
                }
                else
                {
                    var message = JsonConvert.DeserializeObject<Message>(messageAsString);
                  
                }

                if (message == null)
                {
                    message = new Message();
                }
            }
            catch (Exception e)
            {

                if (messageAsString == null || messageAsString == string.Empty)
                {
                    return new Message();
                }
                else
                {
                    var message = JsonConvert.DeserializeObject<Message>(messageAsString);
                }

                if (message == null)
                {
                    message = new Message();
                }
            }

            return message;

            //// get shared preferences
            //ISharedPreferences pref = Application.Context.GetSharedPreferences("PREFERENCE_NAME", FileCreationMode.Private);

            //// read exisiting value
            //var messageAsString = pref.GetString("MessageFromApi", null);

            //// if preferences return null, initialize listOfCustomers
            //if (messageAsString == null)
            //{
            //    return new Message();
            //}

            //var message = JsonConvert.DeserializeObject<Message>(messageAsString);

            //if (message == null)
            //    return new Message();

            //return message;
        }

        public void SaveMessageInPhone(Message newMessage)
        {
            var messageAsString = JsonConvert.SerializeObject(newMessage);

            File.WriteAllText(mMessageFilename, messageAsString);

        }
    }
}