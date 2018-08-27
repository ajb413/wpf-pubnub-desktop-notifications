using Newtonsoft.Json;
using PubnubApi;
using System;
using System.Windows;
using Toast;

namespace WPF_PubNub.Model
{
    public class PubNubHelper
    {
        Pubnub pubnub;
        private readonly string ChannelName = "win-notification";

        //Init and Subscribe PubNub using your PublishKey and SubscribeKey
        public void Init()
        {
            //Init
            PNConfiguration pnConfiguration = new PNConfiguration
            {
                PublishKey = "pub-c-ae8e586a-3d53-40e4-b799-55645e796bb6",
                SubscribeKey = "sub-c-373b79a4-a116-11e8-96b6-ea0414d4b606",
                Secure = true
            };
            pubnub = new Pubnub(pnConfiguration);

            //Subscribe
            pubnub.Subscribe<string>()
           .Channels(new string[] {
               ChannelName
           })
           .WithPresence()
           .Execute();
        }

        //Publish the message
        public void Publish()
        {
            //Buils JSON Message using JSON data type
            JsonMsg Person = new JsonMsg
            {
                Name = "John",
                Description = "Description",
                Date = DateTime.Now.ToString()
            };

            //Convert the data type to string
            string arrayMessage = JsonConvert.SerializeObject(Person);

            pubnub.Publish()
                .Channel(ChannelName)
                .Message(arrayMessage)
                .Async(new PNPublishResultExt((result, status) => {}));
        }

        //listen to the channel
        public void Listen()
        {
            SubscribeCallbackExt listenerSubscribeCallack = new SubscribeCallbackExt(
            (pubnubObj, message) => {

                //Call the notification windows from the UI thread
                Application.Current.Dispatcher.Invoke(new Action(() => { 
                    //Show the message as a WPF window message like WIN-10 toast
                    NotificationWindow ts = new NotificationWindow();


                    //Convert the message to JSON data type
                    JsonMsg bsObj = JsonConvert.DeserializeObject<JsonMsg>(message.Message.ToString());

                    //Build string from the JSON data type message
                    string messageBoxText = "Name: " + bsObj.Name + Environment.NewLine + "Description: " + bsObj.Description + Environment.NewLine + "Date: " + bsObj.Date;
                    ts.NotifText.Text = messageBoxText;
                    ts.Show();
                }));
            },
            (pubnubObj, presence) => {
                // handle incoming presence data
            },
            (pubnubObj, status) => {
                // the status object returned is always related to subscribe but could contain
                // information about subscribe, heartbeat, or errors
                // use the PNOperationType to switch on different options
            });
 
            pubnub.AddListener(listenerSubscribeCallack);
        }
    }
}
