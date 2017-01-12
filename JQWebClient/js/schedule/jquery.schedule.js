$.extend({
    minutesToString: function (value) {
        if (value) {
            var hour = Math.floor(value / 60).toString();
            if (hour.length == 1) {
                hour = "0" + hour;
            }
            var minute = Math.floor(value % 60).toString();
            if (minute.length == 1) {
                minute = "0" + minute;
            }
            return hour + ":" + minute;
        }
        else {
            return "00:00";
        }
    },
    stringToMinutes: function (value) {
        if (value) {
            var intValue = parseInt(value.toString().replace(':', ''), 10);
            return Math.floor(intValue / 100) * 60 + intValue % 100;
        }
        else {
            return 0;
        }
    },
    parseDate: function (value) {
        if (!value) {
            return new Date();
        }
        else {
            var date = value.toString().split(/[T\s]/)[0].replace(/[\/-]/g, '');
            if (date.length == 8) {
                var year = eval(date.substring(0, 4));
                var month = eval(date.substring(4, 6));
                var day = eval(date.substring(6, 8));
                return new Date(year, month - 1, day);
            }
            return new Date();
        }
    }
});

$.fn.schedule = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.schedule.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            if (!$(this).hasClass($.fn.schedule.foo)) {
                $(this).addClass($.fn.schedule.foo)
            }
            $(this).schedule('initialize', options);
        });
    }
};

$.fn.schedule.foo = "info-schedule";

$.fn.schedule.defaults =
{
    todayText: 'Today',
    weekText: 'Week',
    monthText: 'Month',
    refreshText: 'Refresh',
    allDayText: '<All Day>',
    loadingText: 'loading...',
    dateArray: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']
}

$.fn.schedule.methods =
{
    initialize: function (jq, options) {
        $(jq).each(function () {
            var scheduleOptions =
            {
                mode: 'monthly',
                date: new Date(),
                dayHourFrom: 8,
                dayHourTo: 24,
                interval: 1,
                weekSummary: true
            };
            if (options) {
                for (var prop in options) {
                    scheduleOptions[prop] = options[prop];
                }
            }
            $(this).data('options', scheduleOptions);
            $(this).schedule('createLayout');
            $(this).schedule('load');
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    createMenu: function (jq) {
        $(jq).each(function () {
            $(".schedule-menu", this).remove();
            var menu = $("<div class='schedule-menu'></div>").appendTo(this);
            $("<a class='schedule-menuitem'>" + $.fn.schedule.defaults.todayText + "</a>").appendTo(menu).linkbutton({ iconCls: 'icon-today', plain: true }).click(function () {
                if ($(this).linkbutton('options').disabled) {
                    return;
                }
                $(this).closest("." + $.fn.schedule.foo).schedule('date', new Date());
            });
            var showStyle = $(this).schedule('options').showStyle;
            if (showStyle == 'both' || showStyle == 'weekly') {
                $("<a class='schedule-menuitem'>" + $.fn.schedule.defaults.weekText + "</a>").appendTo(menu).linkbutton({ iconCls: 'icon-week', plain: true }).click(function () {
                    if ($(this).linkbutton('options').disabled) {
                        return;
                    }
                    $(this).closest("." + $.fn.schedule.foo).schedule('mode', 'weekly');
                });
            }
            if (showStyle == 'both' || showStyle == 'monthly') {
                $("<a class='schedule-menuitem'>" + $.fn.schedule.defaults.monthText + "</a>").appendTo(menu).linkbutton({ iconCls: 'icon-month', plain: true }).click(function () {
                    if ($(this).linkbutton('options').disabled) {
                        return;
                    }
                    $(this).closest("." + $.fn.schedule.foo).schedule('mode', 'monthly');
                });
            }
            $("<a class='schedule-menuitem'>" + $.fn.schedule.defaults.refreshText + "</a>").appendTo(menu).linkbutton({ iconCls: 'icon-refresh', plain: true }).click(function () {
                if ($(this).linkbutton('options').disabled) {
                    return;
                }
                $(this).closest("." + $.fn.schedule.foo).schedule('load');
            });
            $("<a class='schedule-menuitem'>Previous</a>").appendTo(menu).linkbutton({ iconCls: 'pagination-prev', plain: true, text: '' }).click(function () {
                if ($(this).linkbutton('options').disabled) {
                    return;
                }
                $(this).closest("." + $.fn.schedule.foo).schedule('previousPage');
            });
            $("<a class='schedule-menuitem'>Next</a>").appendTo(menu).linkbutton({ iconCls: 'pagination-next', plain: true, text: '' }).click(function () {
                if ($(this).linkbutton('options').disabled) {
                    return;
                }
                $(this).closest("." + $.fn.schedule.foo).schedule('nextPage');
            });
            $("<a class='schedule-label schedule-menuitem'></a>").appendTo(menu).linkbutton({ plain: true }).click(function () {
                if ($(this).linkbutton('options').disabled) {
                    return;
                }
                $(this).closest("." + $.fn.schedule.foo).schedule('disableMenu');
                var x = $(this).offset().left;
                var y = $(this).offset().top + $(this).height();
                var currentDate = $(this).closest("." + $.fn.schedule.foo).schedule('date');
                var year = currentDate.getFullYear();
                var month = currentDate.getMonth() + 1;
                $("<div style='width:180px;height:180px;'></div>").appendTo($(this).closest("." + $.fn.schedule.foo))
                .offset({ left: x, top: y })
                .calendar({
                    year: year,
                    month: month,
                    current: currentDate,
                    onSelect: function (date) {
                        $(this).closest("." + $.fn.schedule.foo).schedule('date', date);
                        $(this).hide();
                        $(this).closest("." + $.fn.schedule.foo).schedule('enableMenu');
                        $(this).closest("." + $.fn.schedule.foo).find("div.calendar").remove();
                    }
                })
                .show();
            });
            $("<a class='schedule-loadingitem'>" + $.fn.schedule.defaults.loadingText + "</a>").appendTo(menu).linkbutton({ iconCls: 'icon-loading', plain: true }).hide();


            //            $(document).bind('contextmenu', function (e) {
            //                var srcElement = $(e.srcElement);
            //                if (srcElement.hasClass('schedule-contextmenu') || srcElement.hasClass('schedule-item') || srcElement.closest(".schedule-item").length > 0) {
            //                    e.preventDefault();
            //                }

            //            });
            $("<div class='schedule-contextmenu' style='width:120px;'><div data-options=\"iconCls:'icon-cancel'\">Delete</div></div>").appendTo(this)
            .data('schedule', $(this))
            .menu({
                onClick: function (item) {
                    //...
                    var scheduleItem = $(this).data('item');
                    if (scheduleItem.hasClass('schedule-detail-item')) {
                        scheduleItem = scheduleItem.data('parentItem');
                    }
                    var index = $(scheduleItem).index();
                    var item = $(scheduleItem).parent().data('items').splice(index, 1)[0];  //从原来的cell中删除掉
                    var schedule = $(this).data('schedule');
                    if (schedule.schedule('options').onItemRemoved) {
                        schedule.schedule('options').onItemRemoved.call(schedule, {
                            item: item
                        });
                    }

                    $(this).data('schedule').schedule('refreshCell', $(scheduleItem).parent());             //源cell刷新
                }
            });
        });
    },
    createTable: function (jq) {
        $(jq).each(function () {
            $(".schedule-table", this).remove();
            $(".schedule-detail-item", this).remove();
            var mode = $(this).schedule('mode');
            var table = $("<table class='schedule-table'></table>").appendTo(this);
            table.width($(this).width()); //ie 取不到宽度
            if (mode == 'weekly') {
                var currentDate = $(this).schedule('date');
                var head = $("<thead></thead>").appendTo(table);
                var headRow = $("<tr class='schedule-header-row'></tr>").appendTo(head);
                var rowHeaderCell = $("<th></th>").appendTo(headRow);
                rowHeaderCell.width(50);
                var dates = $.fn.schedule.defaults.dateArray;
                for (var i = 0; i < 7; i++) {
                    var cell = $("<th>" + dates[i] + "</th>").appendTo(headRow);
                    cell.width((table.width() - 50) / 7);
                }
                var body = $("<tbody></tbody>").appendTo(table);
                var currentDate = $(this).schedule('date');
                var date = currentDate.addDays(0 - currentDate.getDay());
                $(this).schedule('options').dateFrom = date;
                var dateRow = $("<tr class='schedule-date-row'></tr>").appendTo(body);
                var row = $("<tr class='schedule-row'></tr>").appendTo(body);
                if (!$(this).schedule('options').weekSummary) {
                    row.css('height', '0px');
                }
                else if ($(this).schedule('options').monthHeight) {
                    row.css('height', $(this).schedule('options').monthHeight + 'px');
                }
                $("<td class='schedule-cell' rowspan='2'></td>").appendTo(dateRow);
                for (var i = 0; i < 7; i++) {
                    var dateCell = $("<td class='schedule-cell'>" + (date.getMonth() + 1) + "/" + date.getDate() + "</td>").appendTo(dateRow);
                    var cell = $("<td class='schedule-cell'></td>").appendTo(row)
                    .data("date", date);
                    if (i == 6) {
                        dateCell.addClass("schedule-last-cell");
                        cell.addClass("schedule-last-cell");
                    }
                    date = date.addDays(1);
                }
                $(this).schedule('options').dateTo = date.addDays(-1);
                var dateFrom = $(this).schedule('options').dateFrom;
                var dateTo = $(this).schedule('options').dateTo;

                //detail table
                var dayHourFrom = $(this).schedule('options').dayHourFrom;
                var dayHourTo = $(this).schedule('options').dayHourTo;
                var interval = $(this).schedule('options').interval;
                $('.schedule-label', this).linkbutton({ text: dateFrom.toLocaleDateString() + " - " + dateTo.toLocaleDateString() });

                for (var i = dayHourFrom * 2; i < dayHourTo * 2; i++) {
                    var detailRow = $("<tr class='schedule-detail-row'></tr>").appendTo(body);
                    if ($(this).schedule('options').weekHeight) {
                        detailRow.css('height', $(this).schedule('options').weekHeight + 'px');
                    }
                    if (i % (2 * interval) == 0) {
                        var timeHeader = i / 2 + ":00";
                        var onTimeFormat = $(this).schedule('options').onTimeFormat;
                        if (onTimeFormat) {
                            timeHeader = onTimeFormat.call(this, timeHeader);
                        }

                        $("<td class='schedule-cell' rowspan='" + 2 * interval + "'>" + timeHeader + "</td>").appendTo(detailRow);
                    }
                    var date = $(this).schedule('options').dateFrom;
                    var time = i * 30;

                    for (var j = 0; j < 7; j++) {
                        var cell = $("<td class='schedule-cell'></td>").appendTo(detailRow)
                        .data("date", date)
                        .data("time", time)

                        if (i % (2 * interval) != (2 * interval) - 1) {
                            if (i != dayHourTo * 2 - 1) {
                                cell.addClass("schedule-odd-cell");
                            }
                        }

                        if (j == 6) {
                            cell.addClass("schedule-last-cell");
                        }
                        date = date.addDays(1);
                    }
                }
            }
            else if (mode == 'monthly') {
                var head = $("<thead></thead>").appendTo(table);
                var headRow = $("<tr class='schedule-header-row'></tr>").appendTo(head);
                var dates = $.fn.schedule.defaults.dateArray;
                for (var i = 0; i < 7; i++) {
                    var cell = $("<th>" + dates[i] + "</th>").appendTo(headRow);
                    cell.width(table.width() / 7);
                }
                var body = $("<tbody></tbody>").appendTo(table);
                var currentDate = $(this).schedule('date');
                var year = currentDate.getFullYear();
                var month = currentDate.getMonth();
                var date = new Date(year, month, 1);
                date = date.addDays(0 - date.getDay());
                $(this).schedule('options').dateFrom = date;
                for (var i = 0; i < 6; i++) {
                    var dateRow = $("<tr class='schedule-date-row'></tr>").appendTo(body);
                    var row = $("<tr class='schedule-row'></tr>").appendTo(body);
                    if ($(this).schedule('options').monthHeight) {
                        row.css('height', $(this).schedule('options').monthHeight + 'px');
                    }
                    for (var j = 0; j < 7; j++) {
                        var dateCell = $("<td class='schedule-cell'>" + (date.getMonth() + 1) + "/" + date.getDate() + "</td>").appendTo(dateRow);
                        var cell = $("<td class='schedule-cell'></td>").appendTo(row)
                        .data("date", date);
                        if (j == 6) {
                            dateCell.addClass("schedule-last-cell");
                            cell.addClass("schedule-last-cell");
                        }
                        date = date.addDays(1);
                    }
                    if (date.getMonth() != month) {
                        $(this).schedule('options').dateTo = date.addDays(-1);
                        break;
                    }
                }
            }
            $('.schedule-row>.schedule-cell', this).droppable({
                accept: ".schedule-general-item,.schedule-new-item",
                onDragEnter: function (e, source) {
                    if ($(source).parent() && $(source).parent()[0] == this) { }
                    else {
                        if ($(source).hasClass("schedule-period-item")) {
                            var schedule = $(this).closest("." + $.fn.schedule.foo);
                            var options = schedule.schedule('options');
                            var dateField = options.dateField;
                            var dateToField = options.dateToField;
                            var index = $(source).index();
                            var item = $(source).parent().data('items')[index];
                            var cellDate = $(this).data('date');
                            if ($(source).hasClass("schedule-period-left")) {
                                var dateTo = $.parseDate(item[dateToField]);
                                if (cellDate.compareDate(dateTo) <= 0) {
                                    $(this).addClass('schedule-drag-over');
                                }
                            }
                            else if ($(source).hasClass("schedule-period-right")) {
                                var date = $.parseDate(item[dateField]);
                                if (cellDate.compareDate(date) >= 0) {
                                    $(this).addClass('schedule-drag-over');
                                }
                            }
                            else {
                                $(this).addClass('schedule-drag-over');
                            }
                        }
                        else {
                            $(this).addClass('schedule-drag-over');
                        }
                    }
                },
                onDragLeave: function (e, source) {
                    $(this).removeClass('schedule-drag-over');
                },
                onDrop: function (e, source) {
                    if ($(source).parent() && $(source).parent()[0] == this) { }
                    else if ($(source).hasClass("schedule-period-item")) {
                        $(this).removeClass('schedule-drag-over');
                        var schedule = $(this).closest("." + $.fn.schedule.foo);
                        var options = schedule.schedule('options');
                        var dateField = options.dateField;
                        var dateToField = options.dateToField;
                        var index = $(source).index();
                        var item = $(source).parent().data('items')[index];
                        var cellDate = $(this).data('date');
                        var oldDate = $.parseDate(item[dateField]);
                        var oldDateTo = $.parseDate(item[dateToField]);
                        var dateFrom = schedule.schedule('options').dateFrom;
                        var dateTo = schedule.schedule('options').dateTo;
                        var newDate = oldDate;
                        var newDateTo = oldDateTo;
                        var changedArgs = new Object();
                        changedArgs.item = item;
                        if ($(source).hasClass("schedule-period-left")) {
                            //                            if (cellDate.compareDate(oldDateTo) <= 0) {
                            //                                newDate = cellDate;
                            //                                changedArgs.date = cellDate;
                            //                            }
                            //                            else {
                            //                                return;
                            //                            }
                            //更改为只能拖动第一天
                            newDate = cellDate;
                            changedArgs.date = cellDate;
                            newDateTo = new Date(newDate.getTime() - oldDate.getTime() + oldDateTo.getTime());
                            changedArgs.dateTo = newDateTo;
                        }
                        else if ($(source).hasClass("schedule-period-right")) {
                            if (cellDate.compareDate(oldDate) >= 0) {
                                newDateTo = cellDate;
                                changedArgs.dateTo = cellDate;
                            }
                            else {
                                return;
                            }
                        }
                        else {
                            //                            if (cellDate.compareDate(oldDate) > 0) {
                            //                                newDateTo = cellDate;
                            //                                changedArgs.dateTo = cellDate;
                            //                            }
                            //                            else {
                            //                                newDate = cellDate;
                            //                                changedArgs.date = cellDate;
                            //                            }
                            newDate = cellDate;
                            changedArgs.date = cellDate;
                            newDateTo = cellDate;
                            changedArgs.dateTo = cellDate;

                        }

                        var date = new Date(oldDate.getTime());
                        if (date.compareDate(dateFrom) < 0) {
                            date = new Date(dateFrom.getTime());
                        }
                        for (var i = 0; i < 6 * 7; i++) {
                            if (date.compareDate(oldDateTo) <= 0 && date.compareDate(dateTo) <= 0) {
                                var sourceCell = schedule.schedule('getCell', date);
                                var index = sourceCell.data('items').indexOf(item);               //从原来的cell中删除掉
                                sourceCell.data('items').splice(index, 1);
                                schedule.schedule('refreshCell', sourceCell);                     //源cell刷新
                                date = date.addDays(1);
                            }
                            else {
                                break;
                            }
                        }

                        if (schedule.schedule('options').onItemChanged) {
                            schedule.schedule('options').onItemChanged.call(schedule, changedArgs);
                        }
                        else {
                            item[dateField] = newDate.Format();
                            item[dateToField] = newDateTo.Format();
                        }

                        date = new Date(newDate.getTime());
                        if (date.compareDate(dateFrom) < 0) {
                            date = new Date(dateFrom.getTime());
                        }
                        for (var i = 0; i < 6 * 7; i++) {
                            if (date.compareDate(newDateTo) <= 0 && date.compareDate(dateTo) <= 0) {
                                var targetCell = schedule.schedule('getCell', date);
                                targetCell.data('items').push(item);                              //加到新的cell里
                                schedule.schedule('refreshCell', targetCell);                     //目的cell刷新
                                date = date.addDays(1);
                            }
                            else {
                                break;
                            }
                        }
                    }
                    else {
                        $(this).removeClass('schedule-drag-over');
                        if ($(source).hasClass('schedule-new-item')) {
                            var schedule = $(this).closest("." + $.fn.schedule.foo);
                            var cellDate = $(this).data('date');
                            if (schedule.schedule('options').onItemDroped) {
                                schedule.schedule('options').onItemDroped.call(schedule, {
                                    item: source,
                                    date: cellDate
                                });
                            }
                        }
                        else {
                            var schedule = $(this).closest("." + $.fn.schedule.foo);
                            var index = $(source).index();
                            var item = $(source).parent().data('items').splice(index, 1)[0];  //从原来的cell中删除掉
                            schedule.schedule('refreshCell', $(source).parent());             //源cell刷新
                            var cellDate = $(this).data('date');
                            var options = $(schedule).schedule('options');
                            var dateField = options.dateField;
                            if (schedule.schedule('options').onItemChanged) {
                                schedule.schedule('options').onItemChanged.call(schedule, {
                                    item: item,
                                    date: cellDate
                                });
                            }
                            else {
                                item[dateField] = cellDate.Format();
                            }
                            $(this).data('items').push(item);                                 //加到新的cell里
                            schedule.schedule('refreshCell', this);                           //目的cell刷新
                        }
                    }
                }
            });
            $('.schedule-detail-row>.schedule-cell', this).droppable({
                accept: ".schedule-detail-item,.schedule-new-item",
                onDragEnter: function (e, source) {
                    if ($(source).parent() && $(source).parent()[0] == this) { }
                    else {
                        $(this).addClass('schedule-drag-over');
                    }
                },
                onDragLeave: function (e, source) {
                    $(this).removeClass('schedule-drag-over');
                },
                onDrop: function (e, source) {
                    $(this).removeClass('schedule-drag-over');
                    if ($(source).hasClass('schedule-new-item')) {
                        var schedule = $(this).closest("." + $.fn.schedule.foo);
                        var cellDate = $(this).data('date');
                        var cellTimeMinutes = $(this).data('time');
                        if (schedule.schedule('options').onItemDroped) {
                            schedule.schedule('options').onItemDroped.call(schedule, {
                                item: source,
                                date: cellDate,
                                time: cellTimeMinutes
                            });
                        }
                    }
                    else {
                        var schedule = $(this).closest("." + $.fn.schedule.foo);
                        var index = $(source).data("parentItem").index();
                        var sourceCell = $(source).data("parentItem").parent()
                        var item = sourceCell.data('items').splice(index, 1)[0]
                        schedule.schedule('refreshCell', sourceCell);

                        var cellDate = $(this).data('date');
                        var cellTimeMinutes = $(this).data('time');
                        var options = $(schedule).schedule('options');

                        var timeFromField = options.timeFromField;
                        var timeToField = options.timeToField;
                        var timeFromMinutes = $.stringToMinutes(item[timeFromField]);
                        var timeToMinutes = $.stringToMinutes(item[timeToField]);

                        var dayHourToMinutes = $.stringToMinutes(schedule.schedule('options').dayHourTo * 100);

                        var targetTimeToMinutes = timeToMinutes - timeFromMinutes + cellTimeMinutes;
                        if (targetTimeToMinutes > dayHourToMinutes) {
                            targetTimeToMinutes = dayHourToMinutes;
                        }

                        if (schedule.schedule('options').onItemChanged) {
                            schedule.schedule('options').onItemChanged.call(schedule, {
                                item: item,
                                date: cellDate,
                                dateTo: cellDate,
                                timeFrom: $.minutesToString(cellTimeMinutes),
                                timeTo: $.minutesToString(targetTimeToMinutes)
                            });
                        }
                        else {
                            item[timeFromField] = $.minutesToString(cellTimeMinutes);
                            item[timeToField] = $.minutesToString(targetTimeToMinutes);
                        }
                        var targetCell = schedule.schedule('getCell', cellDate);
                        targetCell.data('items').push(item);                              //加到新的cell里
                        schedule.schedule('refreshCell', targetCell);                     //目的cell刷新
                    }
                }
            });

            var dateFrom = $(this).schedule('options').dateFrom;
            var dateTo = $(this).schedule('options').dateTo;
            $('.schedule-label', this).linkbutton({ text: dateFrom.toLocaleDateString() + " - " + dateTo.toLocaleDateString() });
        });

    },
    createLayout: function (jq) {
        $(jq).empty();
        $(jq).each(function () {
            var options = $(this).schedule('options');
            if (options.width) {
                $(this).width(options.width);
            }
            if (options.height) {
                $(this).height(options.height);
            }
            $(this).schedule('createMenu');
        });
    },
    date: function (jq, value) {
        if (value) {
            $(jq).each(function () {
                var currentDate = $(this).schedule('options').date;
                if (currentDate.toDateString() != value.toDateString()) {
                    $(this).schedule('options').date = value;
                    $(this).schedule('load');
                }
            });
        }
        else {
            return $(jq).schedule('options').date;
        }
    },
    mode: function (jq, value) {
        if (value) {
            $(jq).each(function () {
                var currentMode = $(this).schedule('options').mode;
                if (currentMode != value) {
                    $(this).schedule('options').mode = value;
                    $(this).schedule('load');
                }
            });
        }
        else {
            return $(jq).schedule('options').mode;
        }
    },
    refreshCell: function (jq, cell) {
        $(jq).each(function () {
            var mode = $(this).schedule('mode');
            var options = $(this).schedule('options');
            var dateField = options.dateField;
            var dateToField = options.dateToField;
            var timeFromField = options.timeFromField;
            var timeToField = options.timeToField;
            var titleField = options.titleField;
            var tipField = options.tipField;
            var table = $(".schedule-table, this");
            $(cell).empty();                                      //删除所有的item
            var cellDate = $(cell).data('date');
            $(".schedule-detail-item", this).each(function () {   //删除所有的detail item
                var date = $(this).data('date');
                if (date.equalDate(cellDate)) {
                    $(this).remove();
                }
            });
            var items = $(cell).data('items');
            if (items) {
                //sort
                items.sort(function (item1, item2) {
                    var timeFrom1 = 2400;
                    var timeFrom2 = 2400;
                    if (dateToField && item1[dateToField]) {
                        timeFrom1 = -1;
                    }
                    else if (item1[timeFromField]) {
                        timeFrom1 = parseInt(item1[timeFromField].toString().replace(':', ''), 10);
                    }
                    if (dateToField && item2[dateToField]) {
                        timeFrom2 = -1;
                    }
                    else if (item2[timeFromField]) {
                        timeFrom2 = parseInt(item2[timeFromField].toString().replace(':', ''), 10);
                    }
                    return timeFrom1 - timeFrom2;
                });

                for (var i = 0; i < items.length; i++) {
                    var tooltips = [];
                    var itemClass = "schedule-day-item";
                    if (items[i][timeFromField] && items[i][timeToField]) {
                        var timeFrom = items[i][timeFromField].toString();
                        var timeTo = items[i][timeToField].toString();
                        if (parseInt(timeFrom.replace(':', ''), 10) < 1200) {
                            itemClass = "schedule-am-item";
                        }
                        else {
                            itemClass = "schedule-pm-item";
                        }
                        if (timeFrom.length == 4) {
                            timeFrom = timeFrom.substring(0, 2) + ":" + timeFrom.substring(2, 4);
                        }
                        if (timeTo.length == 4) {
                            timeTo = timeTo.substring(0, 2) + ":" + timeTo.substring(2, 4);
                        }
                        tooltips.push(timeFrom + "-" + timeTo);
                    }
                    else {
                        tooltips.push($.fn.schedule.defaults.allDayText);
                    }
                    if (dateToField && items[i][dateToField]) {
                        itemClass = "schedule-period-item";
                    }
                    if (options.onItemFormating) {
                        var e = {
                            item: items[i],
                            index: i,
                            itemClass: itemClass
                        }
                        options.onItemFormating.call(this, e);
                        itemClass = e.itemClass;
                    }
                    tooltips.push(items[i][titleField]);
                    tooltips.push(items[i][tipField]);
                    var scheduleItem = $("<div class='schedule-item schedule-general-item'><p>" + items[i][titleField] + "</p><div>")
                    .addClass(itemClass)
                    .attr('title', tooltips.join("\r\n"));
                    if (mode == 'monthly' || options.weekSummary) {
                        scheduleItem.appendTo(cell);
                    }

                    if (dateToField && items[i][dateToField]) {    //这些class用于拖动不能修改
                        var date = $.parseDate(items[i][dateField]);
                        var dateTo = $.parseDate(items[i][dateToField]);
                        if (date.compareDate(cellDate) < 0 && dateTo.compareDate(cellDate) > 0) {
                            scheduleItem.addClass("schedule-period-middle");
                        }
                        else if (date.compareDate(cellDate) == 0 && dateTo.compareDate(cellDate) > 0) {
                            scheduleItem.addClass("schedule-period-left");
                        }
                        else if (date.compareDate(cellDate) < 0 && dateTo.compareDate(cellDate) == 0) {
                            scheduleItem.addClass("schedule-period-right");
                        }
                    }

                    var width = table.width() / 7;
                    if (mode == 'weekly') {
                        width = (table.width() - 50) / 7;
                    }
                    scheduleItem.width(width);
                    if (options.editable) {
                        //if (!scheduleItem.hasClass("schedule-period-middle")) {   //period 中段不能拖动
                        if (!scheduleItem.hasClass("schedule-period-middle") && !scheduleItem.hasClass("schedule-period-right")) {   //period 前段才能拖动
                            scheduleItem.draggable({
                                handle: "p",
                                revert: true,
                                onBeforeDrag: function (e) {
                                    if (e.button != 0) {
                                        return false;
                                    }
                                }
                            });
                        }

                        scheduleItem.bind("contextmenu", function (e) {
                            $(".schedule-contextmenu").data('item', $(this))
                                .menu('show', {
                                    left: e.pageX ? e.pageX : e.x,
                                    top: e.pageY ? e.pageY : e.y
                                });
                            e.preventDefault();
                        });

                        //                        scheduleItem.mouseup(function () {
                        //                            if (event.which == 3) {
                        //                                $(".schedule-contextmenu").data('item', $(this))
                        //                                .menu('show', {
                        //                                    left: event.pageX,
                        //                                    top: event.pageY
                        //                                });
                        //                            }
                        //                        });
                    }
                    if (options.onSelected) {
                        scheduleItem.bind("click", function () {
                            var index = $(this).index();
                            var item = $(this).parent().data('items')[index];
                            options.onSelected.call(this, item);
                        });
                    }
                    //detailItem
                    if (mode == 'weekly') {
                        if (items[i][timeFromField] && items[i][timeToField] && $(this).schedule('options').dayHourFrom < $(this).schedule('options').dayHourTo) {
                            var itemIndex = 0;
                            var itemCount = 0;
                            for (var j = 0; j < items.length; j++) {
                                if (items[i][timeFromField] == items[j][timeFromField] && items[i][timeToField] == items[j][timeToField]) {
                                    if (j < i) {
                                        itemIndex++;
                                    }
                                    itemCount++;
                                }
                            }

                            var baseTop = $(cell).offset().top + $(cell).height();
                            //换算
                            var perHourHeight = $(".schedule-detail-row", this).height() * 2;
                            //for ie8 
                            var browser = navigator.appName
                            var b_version = navigator.appVersion
                            var version = b_version.split(";");
                            var trim_Version = version[1].replace(/[ ]/g, "");
                            if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE8.0") {
                                perHourHeight = $(".schedule-detail-row", this)[0].clientHeight * 2;
                            }
                            //end
                            var dayHourFromMinutes = $.stringToMinutes($(this).schedule('options').dayHourFrom * 100);
                            var dayHourToMinutes = $.stringToMinutes($(this).schedule('options').dayHourTo * 100);
                            var timeFromMinutes = $.stringToMinutes(items[i][timeFromField]);
                            var timeToMinutes = $.stringToMinutes(items[i][timeToField]);
                            if (timeFromMinutes < dayHourFromMinutes) {
                                timeFromMinutes = dayHourFromMinutes;
                            }
                            if (timeToMinutes > dayHourToMinutes) {
                                timeToMinutes = dayHourToMinutes;
                            }
                            var top = baseTop + perHourHeight / 60.0 * (timeFromMinutes - dayHourFromMinutes);
                            var left = $(cell).offset().left + 2;
                            var itemWidth = width - 4;
                            if (itemCount > 1) {
                                itemWidth = (width - 4) / itemCount;
                                left = $(cell).offset().left + 2 + itemIndex * itemWidth;
                            }
                            var height = perHourHeight / 60.0 * (timeToMinutes - timeFromMinutes);
                            var minHeight = perHourHeight * 1.5 + 1; //再小就和拖动冲突了 border为1
                            var maxHeight = perHourHeight / 60.0 * (dayHourToMinutes - timeFromMinutes);
                            var detailItem = $("<div class='schedule-item schedule-detail-item'><p>" + items[i][titleField] + "</p><div>")
                            .appendTo(this)
                            .addClass(itemClass);
                            //                            if (options.editable) {
                            detailItem.css('position', 'absolute'); //设定绝对位置，防止拖拉后位置的变化
                            //                            }
                            //.css('position', 'absolute')       
                            detailItem.attr('title', tooltips.join("\r\n"))
                            .data('date', $(cell).data('date')) //用来识别,删除
                            .data('parentItem', scheduleItem)   //用来找到主cell
                            .data('item', items[i])
                            .width(itemWidth)
                            .height(height)
                            .offset({ left: left, top: top });
                            if (dateToField && items[i][dateToField]
                             && (!$.parseDate(items[i][dateField]).equalDate(cellDate) || !$.parseDate(items[i][dateToField]).equalDate(cellDate))) {

                            }
                            else if (options.editable) {

                                detailItem.draggable({
                                    handle: "p",
                                    revert: true,
                                    onBeforeDrag: function (e) {
                                        if (e.button != 0) {
                                            return false;
                                        }
                                    }
                                })
                                .bind("contextmenu", function (e) {
                                    $(".schedule-contextmenu").data('item', $(this))
                                        .menu('show', {
                                            left: e.pageX ? e.pageX : e.x,
                                            top: e.pageY ? e.pageY : e.y
                                        });
                                    e.preventDefault();
                                });
                                //                                .mouseup(function () {
                                //                                    if (event.which == 3) {
                                //                                        $(".schedule-contextmenu").data('item', $(this))
                                //                                        .menu('show', {
                                //                                            left: event.pageX,
                                //                                            top: event.pageY
                                //                                        });
                                //                                    }
                                //                                });
                                if (height >= perHourHeight * 1.5) {
                                    detailItem.resizable({
                                        handles: "s",
                                        minHeight: minHeight,
                                        maxHeight: maxHeight,
                                        onStopResize: function () {
                                            var newHeight = $(this).height();
                                            var schedule = $(this).closest("." + $.fn.schedule.foo);
                                            var index = $(this).data("parentItem").index();
                                            var sourceCell = $(this).data("parentItem").parent()
                                            var item = sourceCell.data('items')[index];
                                            var options = $(schedule).schedule('options');

                                            var timeFromField = options.timeFromField;
                                            var timeToField = options.timeToField;
                                            var timeFromMinutes = $.stringToMinutes(item[timeFromField]);
                                            var timeToMinutes = timeFromMinutes + newHeight / perHourHeight * 60;

                                            item[timeToField] = $.minutesToString(timeToMinutes);

                                            if (schedule.schedule('options').onItemChanged) {
                                                schedule.schedule('options').onItemChanged.call(schedule, {
                                                    item: item,
                                                    timeTo: $.minutesToString(timeToMinutes)
                                                });
                                            }
                                            schedule.schedule('refreshCell', sourceCell);
                                        }
                                    });
                                }
                            }
                            if (options.onSelected) {
                                detailItem.bind("click", function () {
                                    var item = $(this).data('item');
                                    options.onSelected.call(this, item);
                                });
                            }
                        }
                    }
                }
            }
        });
    },
    getCell: function (jq, data) {
        var cell = $(".schedule-row>.schedule-cell", jq).filter(function () {
            var cellDate = $(this).data('date');
            if (cellDate.getFullYear() == data.getFullYear() && cellDate.getMonth() == data.getMonth() && cellDate.getDate() == data.getDate()) {
                return true;
            }
            else {
                return false;
            }
        });
        return cell;
    },
    loadData: function (jq, data) {
        $(jq).each(function () {
            var options = $(this).schedule('options');
            var dateField = options.dateField;
            var dateToField = options.dateToField;
            $(".schedule-row>.schedule-cell", this).each(function () {
                var cellDate = $(this).data('date');
                var items = [];
                for (var i = 0; i < data.length; i++) {
                    var rowData = data[i];
                    if (rowData[dateField]) {
                        var date = $.parseDate(rowData[dateField]);
                        if (date.equalDate(cellDate)) {
                            items.push(rowData); //day
                        }
                        else if (dateToField && rowData[dateToField]) {
                            var dateTo = $.parseDate(rowData[dateToField]);
                            if (date.compareDate(cellDate) <= 0 && dateTo.compareDate(cellDate) >= 0) {
                                items.push(rowData); //period
                            }
                        }
                    }
                }
                $(this).data('items', items);
                $(this).closest("." + $.fn.schedule.foo).schedule('refreshCell', this);
            });

        });
    },
    load: function (jq) {
        $(jq).each(function () {
            $(this).schedule('createTable');
            var schedule = $(this);
            var options = $(this).schedule('options');
            if (options.url) {
                if (options.onBeforeLoad) {
                    options.onBeforeLoad.call(this);
                }
                var queryWord = $(this).schedule('options').queryWord;
                schedule.schedule('disableMenu');
                schedule.find(".schedule-menu .schedule-loadingitem").show();
                $.ajax({
                    type: "POST",
                    dataType: 'json',
                    url: options.url,
                    data: { queryWord: $.toJSONString(queryWord) },
                    cache: false,
                    async: true,
                    success: function (data) {
                        schedule.schedule('loadData', data);
                    },
                    complete: function () {
                        schedule.schedule('enableMenu');
                        schedule.find(".schedule-menu .schedule-loadingitem").hide();
                    }
                });
            }
            else { //测试数据
                var data = [
                {
                    date: '20160721',
                    timeFrom: '20:00',
                    timeTo: '24:00',
                    title: '圣诞节',
                    tip: ''
                },
                {
                    date: '20160723',
                    timeFrom: '08:00',
                    timeTo: '12:00',
                    title: '元旦早上',
                    tip: ''
                },
                {
                    date: '20160328',
                    timeFrom: '20:00',
                    timeTo: '24:00',
                    title: '元旦晚上',
                    tip: ''
                },
                {
                    date: '20160329',
                    timeFrom: '',
                    timeTo: '',
                    title: '元旦',
                    tip: ''
                },
                {
                    date: '20160127',
                    timeFrom: '20:00',
                    timeTo: '24:00',
                    title: '情人节',
                    tip: ''
                },
                {
                    date: '20160128',
                    timeFrom: '20:00',
                    timeTo: '24:00',
                    title: '妇女节',
                    tip: ''
                }
                ];
                $(this).schedule('loadData', data);
            }
        });
    },
    previousPage: function (jq) {
        $(jq).each(function () {
            var currentDate = $(this).schedule('date');
            var mode = $(this).schedule('mode');
            if (mode == 'weekly') {
                $(this).schedule('date', currentDate.addDays(-7))
            }
            else {
                var year = currentDate.getFullYear();
                var month = currentDate.getMonth();
                if (month > 0) {
                    $(this).schedule('date', new Date(year, month - 1, 1));
                }
                else {
                    $(this).schedule('date', new Date(year - 1, 11, 1));
                }
            }
            var onPrevious = $(this).schedule('options').onPrevious;
            if (onPrevious) {
                onPrevious.call(this);
            }
        });
    },
    nextPage: function (jq) {
        $(jq).each(function () {
            var currentDate = $(this).schedule('date');
            var mode = $(this).schedule('mode');
            if (mode == 'weekly') {
                $(this).schedule('date', currentDate.addDays(7))
            }
            else {
                var year = currentDate.getFullYear();
                var month = currentDate.getMonth();
                if (month < 11) {
                    $(this).schedule('date', new Date(year, month + 1, 1));
                }
                else {
                    $(this).schedule('date', new Date(year + 1, 0, 1));
                }
            }
            var onNext = $(this).schedule('options').onNext;
            if (onNext) {
                onNext.call(this);
            }
        });
    },
    enableMenu: function (jq) {
        $(jq).find(".schedule-menu .schedule-menuitem").linkbutton('enable');
    },
    disableMenu: function (jq) {
        $(jq).find(".schedule-menu .schedule-menuitem").linkbutton('disable');
    }
};

Date.prototype.equalDate = function (date) {
    return this.compareDate(date) == 0;
};

Date.prototype.compareDate = function (date) {
    this.setHours(0);
    this.setMinutes(0);
    this.setSeconds(0);
    this.setMilliseconds(0);
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);
    return this.getTime() - date.getTime();
};

Date.prototype.addDays = function (value) {
    return new Date(this.getTime() + value * 1000 * 3600 * 24);
};

Date.prototype.format = function (format) {
    var y = this.getFullYear();
    var m = this.getMonth() + 1;
    var d = this.getDate();
    return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
};

Array.prototype.indexOf = function (obj) {
    if (obj) {
        for (var i = 0; i < this.length; i++) {
            var item = this[i];
            if (typeof object == "object" && typeof item == "object") {
                var equal = true;
                for (var p in obj) {
                    if (obj[p] != item[p]) {
                        equal = false;
                    }
                }
                if (equal) {
                    return i;
                }
            }
            else {
                if (obj == item) {
                    return i;
                }
            }
        }
    }
    return -1;
};