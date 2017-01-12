// 如需空白範本的簡介，請參閱下列文件: 
// http://go.microsoft.com/fwlink/?LinkID=397704
// 若要針對在 Ripple 或 Android 裝置/模擬器上載入的頁面，偵錯程式碼: 請啟動您的應用程式，設定中斷點，
// 然後在 JavaScript 主控台中執行 "window.location.reload()"。
//var ak = 'fF3EzrOwkhrd7fZWVzhQp2ah';
var ak = "IP7GW6lGbeOUZGp0ggpyPKtM";
var currentLat;
var currentLon;
var baiduLat;
var baiduLon;

$(document).ready(function (e) {
    
})
function setMap(id) {
    getLocation(); //定位 
}
function getLocation() {
    if (navigator.geolocation) {
        alert('abce');
        navigator.geolocation.getCurrentPosition(translatePoint);
    }
    else { alert("Geolocation is not supported by this browser."); }
}
function translatePoint(position) {
    alert(position.coords.latitude);
    currentLat = position.coords.latitude;
    currentLon = position.coords.longitude;
    if (typeof (BMap) != undefined) {
        var gpsPoint = new BMap.Point(currentLon, currentLat);
        BMap.Convertor.translate(gpsPoint, 2, initMap2); //转换坐标 
    }
}
function initMap2(point2) {
    alert(point2.lat);
    //var url2 = "http://api.map.baidu.com/geocoder/v2/?ak=" + ak + "&callback=renderReverse&location=121.4383,31.220&output=json&pois=1";
    //var url2 = "http://api.map.baidu.com/geocoder/v2/?ak=" + ak + "&callback=renderReverse&location=31.220,121.4383&output=json&pois=1";
    var url2 = "http://api.map.baidu.com/geocoder/v2/?ak=" + ak + "&callback=renderReverse&location=" + currentLon + "," + currentLat + "&output=json&pois=1";
    $.ajax({
        type: "GET",
        url: url2,
        cache: false,
        async: false,
        dataType: 'jsonp',
        success: function (data) {
            if (data.status == 0) {
                var address = data.result.formatted_address;
                var map = $('#JQPlace1');
                //listview
                var ul = $("ul", map)[0];
                $(ul).html("");
                for (var i = 0; i < data.result.pois.length; i++) {
                    var s = data.result.pois[i];
                    var name = s.name;
                    var addr = s.addr;
                    var direction = s.direction;
                    var distance = s.distance;
                    var poiType = s.poiType;
                    var cp = s.cp;
                    var point = s.point;
                    var li = $('<li></li>').appendTo(ul);
                    var a = $('<a href="#">' + name + '</a>').appendTo(li);
                    var p1 = $('<p>地址信息:' + addr + ' 类型:' + poiType + '</p>').appendTo(li);
                    $('<p>方向:' + direction + ' 距离:' + distance + '</p>').appendTo(li);
                    //$('<p>坐标:' + point + '</p>').appendTo(li);
                    //$('<p>数据来源:' + cp + '</p>').appendTo(li);
                }
                //$(ul).delegate('li', 'click', function () {
                //   alert($(this).val());
                //});
                $(ul).on('tap', 'li', function () {
                    $("input", "#JQPlace1").val($('a', this).html());
                });
                //end listview
                $.mobile.changePage(map, { transition: "slidedown", role: "page" });

                //map
                var geomap = $("#JQPlace1_Map", map);
                geomap.height(300);
                geomap.width(300);
                geomap.css("display", "inline-block");
                var id = geomap.attr('id');
                var map = new BMap.Map(id);
                //map.centerAndZoom(new BMap.Point(data.result.location.lng, data.result.location.lat), 17);
                map.centerAndZoom(point2, 15);
                
                var point = map.getCenter();
                //map.setCenter(new BMap.Point(data.result.location.lng, data.result.location.lat));
                //endmap
                for (var i = 0; i < data.result.pois.length; i++) {
                    var s = data.result.pois[i];
                    var point = s.point;
                    var marker = new BMap.Marker(point);
                    map.addOverlay(marker);
                }
            }

        }, error: function (data) {
            alert(data);
        }
    });
}