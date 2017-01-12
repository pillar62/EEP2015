// http://go.microsoft.com/fwlink/?LinkID=290993&clcid=0x804

var GCM_SENDER_ID = 'EEPApp'; // Replace with your own ID.
var pushNotification;
var mobileServiceClient;
document.addEventListener("deviceready", function () {
    alert(device.uuid);
    mobileServiceClient = new WindowsAzure.MobileServiceClient(
                    "https://eeptest.azure-mobile.net/",
                    "YzTOLwlVJgtPmftvsUrBUUQtPfCxFw53");
    // Define the PushPlugin.
    pushNotification = window.plugins.pushNotification;
    // Platform-specific registrations.
    if (device.platform == 'android' || device.platform == 'Android') {
        // Register with GCM for Android apps.
        pushNotification.register(successHandler, errorHandler,
          {
              "senderID": GCM_SENDER_ID,
              "ecb": "onGcmNotification"
          });
    }
});

// Handle a GCM notification.
function onGcmNotification(e) {
    switch (e.event) {
        case 'registered':
            // Handle the registration.
            if (e.regid.length > 0) {
                console.log("gcm id " + e.regid);
                if (mobileServiceClient) {
                    // Template registration.
                    var template = "{ \"data\" : {\"message\":\"$(message)\"}}";
                    // Register for notifications.
                    mobileServiceClient.push.gcm.registerTemplate(e.regid,
                      "myTemplate", template, null)
                      .done(function () {
                          alert('Registered template with Azure!');
                      }).fail(function (error) {
                          alert('Failed registering with Azure: ' + error);
                      });
                }
            }
            break;
        case 'message':
            if (e.foreground) {
                // Handle the received notification when the app is running
                // and display the alert message.
                alert(e.payload.message);
                // Reload the items list.
                refreshTodoItems();
            }
            break;
        case 'error':
            alert('Google Cloud Messaging error: ' + e.message);
            break;
        default:
            alert('An unknown GCM event has occurred');
            break;
    }
}
