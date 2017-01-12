function _dtpBlur(params) { 
    var dtp = document.getElementById(params['dtp']);
    var traDate = params['traDate'];
    var dateformat = params['dateformat'];
    var msg = params['msg'];

    if (!checkdate(dtp.value, traDate, dateformat, msg)) {
        if (dateformat == 'varChar') {
           var value = dtp.value;
           if (value != '') {
               if (value.length == 8) {
                   var year = value.substring(0, 4);
                   var month = value.substring(4, 6);
                   var day = value.substring(6, 8);
                   var formatValue = year + '/' + month + '/' + day;
                   if (!checkdate(formatValue, false, 'yyyyMMdd', msg)) {
                       alert(msg);
                       dtp.focus();
                       dtp.value = '';
                   }
                   else {
                       dtp.value = formatValue;
                   }
               }
               else {
                   alert(msg);
                   dtp.focus();
                   dtp.value = '';
               }
           }
        }
       else {
            alert(msg);
            dtp.focus();
            dtp.value = '';
        }
    }

   
}

function LeapYear(intYear) {
    if (intYear % 100 == 0) {
        if (intYear % 400 == 0) { return true; }
    }
    else {
        if ((intYear % 4) == 0) { return true; }
    }
    return false;
}

function checkdate(inpar, traDate, dateformat, message) {
    var flagM = false;
    var flagY = false;
    var flagD = false;
    var getdate = inpar
    if (getdate != '') {
        var datepart = getdate.split('');
        var syear = '', smonth = '', sday = '';
        var part1 = '', part2 = '', part3 = '';
        var shour = '', sminute = '', ssecond = '';
        var part4 = '', part5 = '', part6 = '';
        var f = 0;
//        for (var i = 0; i < datepart.length; i++) {
//            var j = datepart[i];
//            if (parseInt(j) <= 9 && parseInt(j) >= 0) {
//                if (f == 0) {
//                    syear += j;
//                }
//                else if (f == 1) {
//                    smonth += j;
//                }
//                else if (f == 2) {
//                    sday += j;
//                }
//            }
//            else {
//                f++;
//            }
        //        }
        for (var i = 0; i < datepart.length; i++) {
            var j = datepart[i];
            if (parseInt(j) <= 9 && parseInt(j) >= 0) {
                if (f == 0) {
                    part1 += j;
                }
                else if (f == 1) {
                    part2 += j;
                }
                else if (f == 2) {
                    part3 += j;
                }
                else if (f == 3) {
                    part4 += j;
                }
                else if (f == 4) {
                    part5 += j;
                }
                else if (f == 5) {
                    part6 += j;
                }
            }
            else {
                f++;
//                if(f == 3)
//                {
//                    alert(message);
//                    return false;
//                }
            }
        }
        if (f != 2 && f != 5) {
            //alert(message);
            return false;
        }

        if (f == 5) {
            shour = part4;
            sminute = part5;
            ssecond = part6;
            if (shour.indexOf('0') == 0) {
                shour = shour.substring(1);
            }
            if (sminute.indexOf('0') == 0) {
                sminute = sminute.substring(1);
            }
            if (ssecond.indexOf('0') == 0) {
                ssecond = ssecond.substring(1);
            }
            var hour = parseInt(shour);
            var minute = parseInt(sminute);
            var second = parseInt(ssecond);
            //alert(hour + "." + minute + "." + second);
            if (hour >= 24 || minute > 60 || second >=60) {
                //alert(message);
                return false;
            }
        }

        if (traDate) {
            syear = part1;
            smonth = part2;
            sday = part3;
        }
        else if(dateformat == 'ddMMyyyy'){
            syear = part3;
            smonth = part2;
            sday = part1;
        }
        else if(dateformat == 'MMddyyyy'){
            syear = part3;
            smonth = part1;
            sday = part2;
        
        }
        else{
            syear = part1;
            smonth = part2;
            sday = part3;
        }
        
        var year = parseInt(syear);
        if (traDate) {
            year = year + 1911;
        }
        if (smonth.indexOf('0') == 0) {
            smonth = smonth.substring(1);
        }
        if (sday.indexOf('0') == 0) {
            sday = sday.substring(1);
        }
        var month = parseInt(smonth);
        var day = parseInt(sday);

        //判断年份是否格式正确
        if (year <= 9999 && year >= 1) { flagY = true; }
        // 判断月份是否格式正确
        if (month <= 12 && month >= 1) { flagM = true; }
        // 判断4,6,9,11月份
        if (month == 4 || month == 6 || month == 9 || month == 11) {
            if (day <= 30 && day >= 1) {
                flagD = true;
            }
        }
        // 判断2月份
        else if (month == 2) {
            if (LeapYear(year)) {
                if (day <= 29 && day >= 1) { flagD = true; }
            }
            else {
                if (day <= 28 && day >= 1) { flagD = true; }
            }
        }
        // 判断1,3,5,7,8,10,12月份
        else {
            if (day <= 31 && day >= 1) {
                flagD = true;
            }
        }
        if (flagY == false || flagM == false || flagD == false) {
            //alert(message);
        }
        return (flagY && flagM && flagD);
    }
    else { return true; }
}