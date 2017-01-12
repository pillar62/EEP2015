if ($.fn.pagination){
	$.fn.pagination.defaults.beforePageText = '第';
	$.fn.pagination.defaults.afterPageText = '共{pages}頁';
	$.fn.pagination.defaults.displayMsg = '顯示{from}到{to},共{total}記錄';
}
if ($.fn.datagrid){
    $.fn.datagrid.defaults.loadMsg = '正在處理，請稍待。。。';
    $.fn.datagrid.defaults.lockMsg = '正在修改這筆資料，請稍待。。。';
    $.fn.datagrid.defaults.lockDeletedMsg = '這筆資料已經被刪除，請刷新。。。';
}
if ($.fn.treegrid && $.fn.datagrid){
	$.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
}
if ($.messager){
	$.messager.defaults.ok = '確定';
	$.messager.defaults.cancel = '取消';
}
if ($.fn.validatebox){
	$.fn.validatebox.defaults.missingMessage = '該輸入項為必輸項';
	$.fn.validatebox.defaults.rules.email.message = '請輸入有效的電子郵件地址';
	$.fn.validatebox.defaults.rules.url.message = '請輸入有效的URL地址';
	$.fn.validatebox.defaults.rules.length.message = '輸入內容長度必須介於{0}和{1}之間';
	//$.fn.validatebox.defaults.rules.remote.message = '請修正此欄位';
}
if ($.fn.numberbox){
	$.fn.numberbox.defaults.missingMessage = '該輸入項為必輸項';
}
if ($.fn.combobox){
	$.fn.combobox.defaults.missingMessage = '該輸入項為必輸項';
}
if ($.fn.combotree){
	$.fn.combotree.defaults.missingMessage = '該輸入項為必輸項';
}
if ($.fn.combogrid){
	$.fn.combogrid.defaults.missingMessage = '該輸入項為必輸項';
}
if ($.fn.timespinner){
    $.fn.timespinner.defaults.missingMessage = '該輸入項為必輸項';
}
if ($.fn.calendar){
	$.fn.calendar.defaults.weeks = ['日','一','二','三','四','五','六'];
	$.fn.calendar.defaults.months = ['一月','二月','三月','四月','五月','六月','七月','八月','九月','十月','十一月','十二月'];
}
if ($.fn.datebox){
	$.fn.datebox.defaults.currentText = '今天';
	$.fn.datebox.defaults.closeText = '關閉';
	$.fn.datebox.defaults.okText = '確定';
	$.fn.datebox.defaults.missingMessage = '該輸入項為必輸項';
	$.fn.datebox.defaults.formatter = function (date) {
	    var y = date.getFullYear();
	    var m = date.getMonth() + 1;
	    var d = date.getDate();
	    return y + '.' + (m < 10 ? ('0' + m) : m) + '.' + (d < 10 ? ('0' + d) : d);
	};
	$.fn.datebox.defaults.parser = function (s) {
	    if (!s) return new Date();

	    var ss = s.split(/[T\s]/);
	    var sd = ss[0].split(/[.\/-]/);
	    //var ss = s.split('.');
	    var y = parseInt(sd[0], 10);
	    var M = parseInt(sd[1], 10);
	    var d = parseInt(sd[2], 10);

	    if (ss.length == 1) {
	        if (!isNaN(y) && !isNaN(M) && !isNaN(d)) {
	            return new Date(y, M - 1, d);
	        } else {
	            return new Date();
	        }
	    }
	    else {
	        var st = ss[1].split(':');
	        //var ss = s.split('.');
	        var h = parseInt(st[0], 10);
	        var m = parseInt(st[1], 10);
	        var s = parseInt(st[2], 10);
	        if (!isNaN(y) && !isNaN(M) && !isNaN(d)) {
	            return new Date(y, M - 1, d, h, m, s);
	        } else {
	            return new Date();
	        }
	    }
	};
}
if ($.fn.datetimebox && $.fn.datebox){
	$.extend($.fn.datetimebox.defaults,{
		currentText: $.fn.datebox.defaults.currentText,
		closeText: $.fn.datebox.defaults.closeText,
		okText: $.fn.datebox.defaults.okText,
		missingMessage: $.fn.datebox.defaults.missingMessage
	});
}

if ($.fn.schedule) {
    $.fn.schedule.defaults.todayText = '今天';
    $.fn.schedule.defaults.weekText = '周';
    $.fn.schedule.defaults.monthText = '月';
    $.fn.schedule.defaults.refreshText = '刷新';
    $.fn.schedule.defaults.loadingText = '載入數據中...';
    $.fn.schedule.defaults.dateArray = ['日', '一', '二', '三', '四', '五', '六'];
}
