function CheckNum()
{
    var objRegex=/[\d]/ig;//數字的正則表達式
    if (String.fromCharCode(event.keyCode).match(objRegex) == null && event.keyCode!=9)
    ReturnFalse();
}

function CheckDecimal(el, ev)
{
    //if(isNaN(String.fromCharCode(event.keyCode)))
    //8：退格键、46：delete、37-40： 方向键
//48-57：小键盘区的数字、96-105：主键盘区的数字
//110、190：小键盘区和主键盘区的小数
//189、109：小键盘区和主键盘区的负号
    var event = ev || window.event;                             //IE、FF下获取事件对象
    var currentKey = event.charCode||event.keyCode;             //IE、FF下获取键盘码
    
    //小数点处理
    if (currentKey == 110 || currentKey == 190) {
        if (el.value.indexOf(".")>=0) 
            ReturnFalse();

    } else 
        if (currentKey!=8 && currentKey !=9 && currentKey != 46 && (currentKey<37 || currentKey>40) && (currentKey<48 || currentKey>57) && (currentKey<96 || currentKey>105))
            ReturnFalse();

}

function ReturnFalse()
{
    if (window.event)                       //IE
                event.returnValue=false;                 //e.returnValue = false;效果相同.
            else                                    //Firefox
                event.preventDefault();
}