<%@ WebHandler Language="C#" Class="PushHandler" %>

using System;
using System.Web;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using System.Configuration;
using System.Xml.Linq;
using JdSoft.Apple.Apns.Notifications;

public class PushHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    static string ret = "";
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        ret = "";
        var users = context.Request.Form["to"];
        var server_api_key = context.Request.Form["server_api_key"];
        var p12_file_path = context.Request.Form["p12_file_path"];
        //A6-Sensitive Data Exposure
        //var p12_file_password = context.Request.Form["p12_file_password"];
        var apn_type = context.Request.Form["apn_type"];
        var subject = context.Request.Form["subject"];
        var body = context.Request.Form["body"];
        var listID = context.Request.Form["listID"];
        var flowpath = context.Request.Form["flowpath"];
        var regIDs = new System.Collections.Generic.List<string>();
        var tokenIDs = new System.Collections.Generic.List<string>();
        if (!string.IsNullOrEmpty(users))
        {
            var list = new System.Collections.Generic.List<string>(users.Split(';'));
            regIDs = EFClientTools.ClientUtility.Client.GetRegIDs(list, EFClientTools.ClientUtility.ClientInfo.SDDeveloperID);
            tokenIDs = EFClientTools.ClientUtility.Client.GetTokenIDs(list, EFClientTools.ClientUtility.ClientInfo.SDDeveloperID);
            EFClientTools.ClientUtility.Client.SendMessage(EFClientTools.ClientUtility.ClientInfo, list, subject, body);
        }
        else
        {
            regIDs = new System.Collections.Generic.List<string>(context.Request.Form["regIDs"].Split(';'));
            tokenIDs = new System.Collections.Generic.List<string>(context.Request.Form["tokenIDs"].Split(';'));
        }
        if (regIDs.Count > 0)
        {
            for (int i = 0; i < regIDs.Count; i++)
            {
                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format("http://www.infolight.com/new/ashx/PushHandler.ashx?RegID={0}&subject={1}&body={2}", regIDs[i], subject, body));
                //var res = req.GetResponse();
                //StreamReader sr = new StreamReader(res.GetResponseStream());
                //string result = sr.ReadToEnd();
                //RegID 是接收 Android 裝置上傳 GCM 註冊成功的 RegID
                //完整的應用應該是在 APP 對 google-cloud-messaging(GCM) 服務註冊成功時, 會收到REGID ,  
                //APP 對自己的網站上傳 GCM 服務傳回來的 RegID ,RegID 長達 162 字元
                //然後自己的網站收到此 RegID 後,如果您想將 RegID 儲存於資料庫系統內，
                //則您需要建立一個 table 存放 Android 裝置傳上來的 RegID，存放 RegID 的欄位長度最好大於 162 字元
                //因為以 Android 設備爆炸性成長的速度來看，如果愈來愈多開發人員採用 GCM，那麼 RegID 長度勢必再增加...
                //然後自己的網站在需要通知 APP 某訊息時, 再從 資料表讀出 RegID 
                //當您的網站要發送訊息給有安裝您 APP 的 Android 裝置時，您的 server 是將訊息發送給 Google GCM server，
                //由 Google GCM server 再將訊息轉發給您 指定的 RegID。
                //而當 Android 裝置解除安裝您的 app 時，Google GCM server 並不會立即通知您的網站，
                //而是在下一次您發送訊息給該 Android 裝置時，Google GCM server 才會回應給您的網站一個錯誤json如下，
                //  
                //{
                //  "multicast_id":575?????46362470343,
                //  "success":117,
                //  "failure":83,
                //  "canonical_ids":0,
                //  "results":[
                //              {"error":"NotRegistered"},
                //              {"error":"NotRegistered"},
                //              {"message_id":"0:13957176817?????%fcbf9d11f9fd7ecd"},

                //              ...
                //              ...

                //              {"message_id":"0:13957176817?????%fcbf9d11f9fd7ecd"}
                //           ]
                //}
                //錯誤的內容是該裝置並未註冊，所以您的網站要在此時將該裝置的 regId 從您的資料庫中刪除。 
                //此範例已簡化為如果網站有收到一個 RegID , 就對這個 RegID 發一個 "Hello GCM!" 的訊息
                //準備對GCM Server發出Http post
                string API_Key = server_api_key;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
                request.Method = "POST";
                request.ContentType = "application/json;charset=utf-8;";
                request.Headers.Add(string.Format("Authorization: key={0}", API_Key));
                var postData =
                new
                {
                    data = new
                    {
                        message = subject, //message等等這些 tag 要讓前端開發人員知道
                        body = body,
                        title = subject,
                        listID = listID,
                        flowpath = flowpath
                    },
                    registration_ids = regIDs.ToArray()
                };
                string p = JsonConvert.SerializeObject(postData);//將Linq to json轉為字串
                byte[] byteArray = Encoding.UTF8.GetBytes(p);//要發送的字串轉為byte[]
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                //發出Request
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responseStr = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                response.Close();
                context.Response.Write("{\"ret\":\"OK\"}");
            }
        }
        if (tokenIDs.Count > 0)
        {

            //for (int i = 0; i < tokenIDs.Count; i++)
            //{
            //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(string.Format("http://www.infolight.com/new/ashx/PushHandler.ashx?token={0}&subject={1}&body={2}", tokenIDs[i], subject, body));
            //    var res = req.GetResponse();
            //    StreamReader sr = new StreamReader(res.GetResponseStream());
            //    string result = sr.ReadToEnd();
            //}
            //return;
            //Variables you may need to edit:
            //---------------------------------

            //True if you are using sandbox certificate, or false if using production

            bool sandbox = false;
            if (apn_type.ToLower() == "debug")
            {
                sandbox = true;
            }

            //Put your device token in here
            //string testDeviceToken = token;

            //Put your PKCS12 .p12 or .pfx filename here.
            // Assumes it is in the same directory as your app
            string p12File = p12_file_path;

            //This is the password that you protected your p12File 
            //  If you did not use a password, set it as null or an empty string
            //A6-Sensitive Data Exposure
            //string p12FilePassword = p12_file_password;

            //Number of notifications to send
            //int count = 1;

            //Number of milliseconds to wait in between sending notifications in the loop
            // This is just to demonstrate that the APNS connection stays alive between messages
            //int sleepBetweenNotifications = 15000;


            //Actual Code starts below:
            //--------------------------------

            string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);
            //A6-Sensitive Data Exposure
            //NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);
            NotificationService service = new NotificationService(sandbox, p12Filename, context.Request.Form["p12_file_password"], 1);
           

            service.SendRetries = 5; //5 retries before generating notificationfailed event
            service.ReconnectDelay = 5000; //5 seconds

            service.Error += new NotificationService.OnError(service_Error);
            service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);

            service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);
            service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);
            service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);
            service.Connecting += new NotificationService.OnConnecting(service_Connecting);
            service.Connected += new NotificationService.OnConnected(service_Connected);
            service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);

            for (int i = 0; i < tokenIDs.Count; i++)
            {
                //Create a new notification to send
                Notification alertNotification = new Notification(tokenIDs[i]);

                alertNotification.Payload.Alert.Body = body;
                //alertNotification.Payload.Sound = "default";
                //alertNotification.Payload.Badge = i;

                //Queue the notification to be sent
                if (service.QueueNotification(alertNotification))
                    ret += "Notification Queued!";
                else
                    ret += "Notification Failed to be Queued!";

                //Sleep in between each message
                //if (true)
                //{
                //    ret += "Sleeping " + sleepBetweenNotifications + " milliseconds before next Notification...";
                //    System.Threading.Thread.Sleep(sleepBetweenNotifications);
                //}
            }

            ret += "Cleaning Up...";

            //First, close the service.  
            //This ensures any queued notifications get sent befor the connections are closed
            service.Close();

            //Clean up
            service.Dispose();
            ret = HttpUtility.HtmlEncode(ret);
            context.Response.Write("{\"ret\":\"" + ret + "\"}");
        }
        else
        {
            context.Response.Write("{\"ret\":\"None RegID or none token\"}");
        }
    }
    static void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
    {
        ret += string.Format("Bad Device Token: {0}", ex.Message);
    }

    static void service_Disconnected(object sender)
    {
        ret +=  "Disconnected...";
    }

    static void service_Connected(object sender)
    {
        ret +=  "Connected...";
    }

    static void service_Connecting(object sender)
    {
        ret +=  "Connecting...";
    }

    static void service_NotificationTooLong(object sender, NotificationLengthException ex)
    {
        ret +=  string.Format("Notification Too Long: {0}", ex.Notification.ToString());
    }

    static void service_NotificationSuccess(object sender, Notification notification)
    {
        ret +=  string.Format("Notification Success: {0}", notification.ToString());
    }

    static void service_NotificationFailed(object sender, Notification notification)
    {
        ret +=  string.Format("Notification Failed: {0}", notification.ToString());
    }

    static void service_Error(object sender, Exception ex)
    {
        ret +=  string.Format("Error: {0}", ex.Message);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}