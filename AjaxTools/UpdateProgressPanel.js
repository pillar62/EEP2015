var originalBodyClass = '';

Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(
function(sender, e) {
    originalBodyClass = document.body.className;
    document.body.className = 'mainDivUpdating';
});

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(
function(sender, e) {
    document.body.className = originalBodyClass;
});

//function SetUpdateProgressPanelSytle(pan, size) {
//    var panel = $get(pan);
//    if (panel) {
//        var width = pan['width'] ? pan['width'] : 100;
//        var height = pan['height'] ? pan['height'] : 50;
//        panel.style.position = 'absolute';
//        panel.style.width = width;
//        panel.style.height = height;
//        panel.style.left = (document.documentElement.clientWidth - width) / 2;
//        panel.style.top = (document.documentElement.clientHeight - height) / 2;
//    }
//}