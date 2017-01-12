function setTraDate(id) {
    var calendar = $get(id);
    var sDate = calendar.getElementsByTagName('td')[2].innerHTML;
    var datepart = sDate.split('');
    var syear = '', sother = '';
    var yearOver = false;
    for (var i = 0; i < datepart.length; i++) {
        var j = datepart[i];
        if (!yearOver) {
            if (parseInt(j) <= 9 && parseInt(j) >= 0) {
                syear += j;
            }
            else {
                yearOver = true; 
                sother += j;
            }
        }
        else {
            sother += j;
        }
    }
    var year = parseInt(syear) - 1911;
    calendar.getElementsByTagName('td')[2].innerHTML = year.toString() + sother;
}

function _dtpBlur1(params) {
    var dtp = document.getElementById(params['dtp']);
    var traDate = params['traDate'];
    var msg = params['msg'];
    if (!checkdate(dtp.value.trim(), traDate, msg)) {
        dtp.focus();
        dtp.value = '';
    }
}

function _dtpChange1(dtp) {
    var dtp = document.getElementById(dtp);
    if (dtp) {
        var value = dtp.value;
        if (value === '') {
            dtp.value = ' ';
        }
    }
}