function registerDevice() {
    var xhr = new XMLHttpRequest();
    xhr.addEventListener("load", function () {
        var parser = new DOMParser();
        var doc = parser.parseFromString(xhr.responseText, "application/xml");
        var sender_id = doc.getElementsByTagName("sender_id")[0].textContent;
        var server_api_key = doc.getElementsByTagName("server_api_key")[0].textContent;
        var p12_file_path = doc.getElementsByTagName("p12_file_path")[0].textContent;
        var p12_file_password = doc.getElementsByTagName("p12_file_password")[0].textContent;
        var apn_acvive = doc.getElementsByTagName("apn_acvive")[0].textContent;
        var apn_type = doc.getElementsByTagName("apn_type")[0].textContent;
        var gcm_active = doc.getElementsByTagName("gcm_active")[0].textContent;
        //alert('sender_id:' + sender_id);
        window.sessionStorage.setItem('sender_id', sender_id);
        window.sessionStorage.setItem('server_api_key', server_api_key);
        window.sessionStorage.setItem('p12_file_path', p12_file_path);
        window.sessionStorage.setItem('p12_file_password', p12_file_password);
        window.sessionStorage.setItem('apn_acvive', apn_acvive);
        window.sessionStorage.setItem('apn_type', apn_type);
        window.sessionStorage.setItem('gcm_active', gcm_active);
        if (device.platform == 'android' || device.platform == 'Android' || device.platform == "amazon-fireos") {
            if (gcm_active == "true") {
                var pushNotification = window.plugins.pushNotification;
                pushNotification.register(
                successHandler,
                errorHandler,
                {
                    "senderID": sender_id,
                    "ecb": "onNotification"
                });
            }
        }
        else {
            //alert("OS:" + device.platform);
            if (apn_acvive == "true") {
                var pushNotification = window.plugins.pushNotification;
                pushNotification.register(
                tokenHandler,
                errorHandler,
                {
                    "badge": "false",
                    "sound": "false",
                    "alert": "true",
                    "ecb": "onNotificationAPN"
                });
            }
        }
    });
    var path = 'eepApp.xml';
    xhr.open("get", path, true);
    xhr.send();
}


// result contains any message sent from the plugin call
function successHandler(result) {
    //alert('successHandler-result = ' + result);
}

// result contains any error description text returned from the plugin call
function errorHandler(error) {
    alert('errorHandler-error = ' + error);
}

function tokenHandler(result) {
    // Your iOS push server needs to know the token before it can push to this device
    // here is where you might want to send it the token for later use.
    // alert('tokenID:' + result);
    $('.info-logon').logon('registerDevice', { tokenID: result });
}

window.onNotification = function (e) {
    switch (e.event) {
        case 'registered':
            if (e.regid.length > 0) {
                //alert('regID:'+ e.regid);
                $('.info-logon').logon('registerDevice', { regID: e.regid });
                // Your GCM push server needs to know the regID before it can push to this device
                // here is where you might want to send it the regID for later use.
            }
            break;

        case 'message':
            //alert('Got Google Cloud Messaging');
            // if this flag is set, this notification happened while we were in the foreground.
            // you might want to play a sound to get the user's attention, throw up a dialog, etc.
            if (e.foreground) {
                // on Android soundname is outside the payload.
                // On Amazon FireOS all custom attributes are contained within payload
                //var soundfile = e.soundname || e.payload.sound;
                // if the notification contains a soundname, play it.
                // var my_media = new Media("/android_asset/www/" + soundfile);
                // my_media.play();
                //EEPApp是目前在前景執行的APP
                alert('EEPApp是目前在前景執行的APP\n\n' + e.payload.title + ':\n\n' + e.payload.body);
                if (e.payload.listID) {
                    //loading
                    var url = webSiteUrl + '/handler/SystemHandle_Flow2.ashx';
                    $.mobile.loading('show', { theme: 'b', text: $.fn.datagrid.defaults.loadingMessage, textVisible: true });
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: { Type: 'ToDoList', listID: e.payload.listID },
                        cache: false,
                        async: true,
                        success: function (data) {
                            var rows = eval('(' + data + ')');
                            var path = [];
                            for (var i = 0; i < rows.length; i++) {
                                path.push(rows[i].FLOWPATH);
                                if (rows[i].FLOWPATH == e.payload.flowpath) {
                                    var selectedRow = rows[i];
                                    var urlParam = "?IsWorkflow=1";
                                    for (var field in selectedRow) {
                                        urlParam += "&" + field + "=" + selectedRow[field];
                                    }
                                    var url = selectedRow.WEBFORM_NAME.replace(".", "/") + ".html" + urlParam;
                                    if (selectedRow.FORM_NAME && selectedRow.FORM_NAME.indexOf('M:') == 0) {
                                        url = selectedRow.FORM_NAME.substring(2).replace(".", "/") + ".html" + urlParam;
                                    }
                                    window.sessionStorage.setItem('flowrow', $.toJSONString(selectedRow));
                                    window.location.href = url;
                                }
                            }
                            //alert(rows.length + '#' + e.payload.listID + '#' + e.payload.flowPath + '#' + path.join(','));
                        },
                        error: function (data) {
                            alert(data.responseText);
                        },
                        complete: function () {
                            $.mobile.loading('hide');
                        }
                    });

                }

            }
            else {
                // otherwise we were launched because the user touched a notification in the notification tray.
                //EEPApp不是目前在前景執行的APP
                if (e.coldstart) {
                    alert('EEPApp新執行' + e.payload.title + ':\n\n' + e.payload.body);
                }
                else {
                    alert('EEPApp是在背景執行的APP,現在回覆到前景執行\n\n' + e.payload.title + ':\n\n' + e.payload.body);
                }
            }
            //以下這段沒有反應,所以取消了
            //navigator.notification.beep(2); //2 - > times: The number of times to repeat the beep. (Number)
            //navigator.notification.alert(
            //    e.payload.body,   // message
            //    function () { },   // callback function name
            //    e.payload.title,  // title
            //    'Done'            // buttonName
            //);            
            break;
        case 'error':
            break;
        default:
            break;
    }
};

window.onNotificationAPN = function (e) {
    //alert('got Apple Push Notification');
    //以下這段沒有反應,所以取消了
    //if (event.alert) {
    //    navigator.notification.alert(e.alert);
    //};
    alert(e.alert);
};