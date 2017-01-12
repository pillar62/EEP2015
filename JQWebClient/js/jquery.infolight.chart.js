$(document).ready(function () {
    $("." + $.fn.plotbarchart.foo).plotbarchart({});
    $("." + $.fn.plotpiechart.foo).plotpiechart({});
    $("." + $.fn.plotlinechart.foo).plotlinechart({});
    $("." + $.fn.plotDashBoard.foo).plotDashBoard({});
    $(window).bind("orientationchange resize pageshow", fixgeometry);
});
function fixgeometry() {
    var viewport_width = $(window).width();
    $("." + $.fn.plotbarchart.foo).each(function () {
        var plotwidth = $(this).width();
        var options = $(this).plotbarchart('options');
        if (plotwidth > 700 && viewport_width < 560 && options.width >700) {
            $(this).plotbarchart('resize',true);
        }
        if (plotwidth < 700 && viewport_width > 560 && options.width > 700) {
            $(this).plotbarchart('resize', false);
        }
    });
    $("." + $.fn.plotlinechart.foo).each(function () {
        var plotwidth = $(this).width();
        var options = $(this).plotlinechart('options');
        if (plotwidth > 700 && viewport_width < 560 && options.width > 700) {
            $(this).plotlinechart('resize', true);
        }
        if (plotwidth < 700 && viewport_width > 560 && options.width > 700) {
            $(this).plotlinechart('resize', false);
        }
    });
    $("." + $.fn.plotpiechart.foo).each(function () {
        var plotwidth = $(this).width();
        var options = $(this).plotpiechart('options');
        if (plotwidth > 700 && viewport_width < 560 && options.width > 700) {
            $(this).plotpiechart('resize', true);
        }
        if (plotwidth < 700 && viewport_width > 560 && options.width > 700) {
            $(this).plotpiechart('resize', false);
        }
    });
    $("." + $.fn.plotDashBoard.foo).each(function () {
        var plotwidth = $(this).width();
        var options = $(this).plotDashBoard('options');
        if (plotwidth > 700 && viewport_width < 560 && options.width > 700) {
            $(this).plotDashBoard('resize', true);
        }
        if (plotwidth < 700 && viewport_width > 560 && options.width > 700) {
            $(this).plotDashBoard('resize', false);
        }
    });

}
//--------------------------------------------------------------------------------------------------------------------------------
$.fn.plotpiechart = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.plotpiechart.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).plotpiechart('initialize', options);
            if (!$(this).hasClass($.fn.plotpiechart.foo)) {
                $(this).addClass($.fn.plotpiechart.foo)
            }
        });
    }
};

$.fn.plotpiechart.foo = 'info-plotpiechart';

$.fn.plotpiechart.defaults = {
    title:"",
    radius: undefined,
    labelShow: false,
    legendShow: true,
    sliceMargin: 2,
    fill: true,
    shadowDepth:5,
    lineWidth: undefined,
    labelStyle: 'percent'
};

$.fn.plotpiechart.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var chartOptions = new Object();
            chartOptions.title = $.fn.plotpiechart.defaults.title;      //default option
            chartOptions.radius = $.fn.plotpiechart.defaults.radius;      //default option
            chartOptions.labelShow = $.fn.plotpiechart.defaults.labelShow;
            chartOptions.labelRadius = $.fn.plotpiechart.defaults.labelRadius;
            chartOptions.backgroundOpacity = $.fn.plotpiechart.defaults.backgroundOpacity;
            chartOptions.legendShow = $.fn.plotpiechart.defaults.legendShow;

            var htmlOptions = getInfolightOption($(this)); //html option
            for (var property in htmlOptions) {
                chartOptions[property] = htmlOptions[property];
            }
            chartOptions.whereString = $(this).plotpiechart('getWhereItem');
            $(this).data('options', chartOptions);

            var title = chartOptions.title;

            if (chartOptions.alwaysClose) {
                $(this).plotpiechart('options').whereString = '1=0';
            }
            if (chartOptions.renderObjectID != undefined && chartOptions.renderObjectID != "") {
                if ($('#' + chartOptions.renderObjectID) != undefined) {
                    $(this).appendTo($('#' + chartOptions.renderObjectID));
                }
            }
            $(this).plotpiechart('load');

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
    load: function (jq) {
        jq.each(function () {
            var options = $(this).plotpiechart('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                if (options.onBeforeLoad != undefined) {
                    options.onBeforeLoad.call(this);
                }
                var queryWord = new Object();
                if (options.whereString != undefined) {
                    queryWord.whereString = options.whereString;
                }
                var data = $(this).plotpiechart('loadData');
                var truedata = data.data;
                var showFields = data.showFields;
                var plotOptions = $(this).plotpiechart('getplotOptions', { showFields: showFields });
                $(this).data('plotOptions', plotOptions);
                $(this).data('plotdata', truedata);
                if (truedata.length > 0) {
                    var plot = $.jqplot($(this).attr('id'), [truedata], plotOptions);
                    $(this).data('plot', plot);
                    if (options.onClick != undefined) {
                        $(this).bind('jqplotDataClick', eval(options.onClick));
                    }
                }
            }
        });
    },
    loadData: function (jq) {
        var data = [];
        var showFields = [];
        if (jq.length > 0) {
            var options = $(jq[0]).plotpiechart('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                var url = getRemoteUrl(options.remoteName, options.tableName, false); //呼叫Server取得數據的url
                var queryWord = new Object();
                if (options.whereString != undefined) {
                    queryWord.whereString = options.whereString;
                }
                $.ajax({
                    type: "POST",
                    url: url,
                    data: "queryWord=" + $.toJSONString(queryWord),
                    cache: false,
                    async: false,
                    success: function (returndata) {
                        if (returndata != null) {
                            $(jq[0]).data('data', returndata);
                            var rows = $.parseJSON(returndata)
                            for (var i = 0; i < rows.length; i++) {
                                data[i] = [rows[i][options.keyField], rows[i][options.valueField]];
                                if (options.labelStyle == "labelshowfield") {
                                    showFields[i] = [rows[i][options.labelShowField]];
                                }
                            }
                        }
                    },
                    error: function () {
                        //alert('error');
                    }
                });
            }
            return { data: data, showFields: showFields };
        }
    },
    getplotOptions: function (jq,options) {
        if (jq.length > 0) {
            var plotpiechart = jq[0];
            var title = $(plotpiechart).plotpiechart('options').title;
            var radius = $(plotpiechart).plotpiechart('options').radius;
            var labelShow = $(plotpiechart).plotpiechart('options').labelShow;
            var legendShow = $(plotpiechart).plotpiechart('options').legendShow;
            var legendLocation = $(plotpiechart).plotpiechart('options').legendLocation;
            var legendPlacement = $(plotpiechart).plotpiechart('options').legendPlacement;
            var labelStyle = $(plotpiechart).plotpiechart('options').labelStyle;
            var sliceMargin = $(plotpiechart).plotpiechart('options').sliceMargin;
            var fill = $(plotpiechart).plotpiechart('options').fill;
            var shadowDepth = $(plotpiechart).plotpiechart('options').shadowDepth;
            var lineWidth = $(plotpiechart).plotpiechart('options').lineWidth;

            if (labelStyle == 'key') {
                labelStyle = "label";
            }
            else if (labelStyle == "labelshowfield") {
                labelStyle = options.showFields;
            }

            var options = {
                title:title,
                seriesDefaults: {
                    // Make this a pie chart.
                    renderer: jQuery.jqplot.PieRenderer,
                    rendererOptions: {
                        // Put data labels on the pie slices.
                        // By default, labels show the percentage of the slice.
                        diameter:radius,
                        sliceMargin: sliceMargin,
                        showDataLabels: labelShow,
                        fill: fill,
                        dataLabels :labelStyle,
                        shadowDepth: shadowDepth,
                        lineWidth: lineWidth
                    }
                }
            }
            if (legendShow) {
                var legend = {
                    show: legendShow,
                    location: legendLocation,
                    placement: legendPlacement
                }
                options.legend = legend;
            }

            return options;
        }
        return [];
    },
    resize: function (jq, m) {
        if (jq.length > 0) {
            var options = $(jq[0]).plotpiechart('options');
            if (m) {
                var data = $(jq[0]).data();
                if (data != undefined) {
                    var plot = data.plot;
                    if (plot != undefined) {
                        $(jq[0]).width(options.width / 2);
                        //plot._width = plotwidth / 2;
                        plot.destroy();
                        var plotnew = $.jqplot($(jq[0]).attr('id'), [data.plotdata], data.plotOptions);
                        $(jq[0]).data('plot', plotnew);
                    }
                }
            }
            else {
                var data = $(jq[0]).data();
                if (data != undefined) {
                    var plot = data.plot;
                    if (plot != undefined) {
                        $(jq[0]).width(options.width);
                        plot.destroy();
                        var plotnew = $.jqplot($(jq[0]).attr('id'), [data.plotdata], data.plotOptions);
                        $(jq[0]).data('plot', plotnew);
                    }
                }
            }
        }
    },
    getData: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('data') == undefined) {
                return [];
            }
            return $(jq[0]).data('data');
        }
        return [];
    },
    getWhereItem: function (jq, op) {
        if (jq.length > 0) {
            var options = new Object();
            if (op != undefined) {
                options = op;
            }
            else {
                options = getInfolightOption($(jq[0]));
            }
            if (options.whereItems != undefined) {
                var where = '';
                var wheremethods = new Object();
                var hasRemote = false;
                for (var i = 0; i < options.whereItems.length; i++) {
                    var field = options.whereItems[i].field;
                    var value = options.whereItems[i].value;
                    var condition = options.whereItems[i].condition;
                    var realvalue = "";
                    if (condition == undefined)
                        chartOptions = "=";

                    if (value.indexOf("client[") == 0) {
                        var methodName = value.replace("client[", "").replace("]", "");
                        realvalue = eval(methodName).call();
                    }
                    else if (value.indexOf("remote[") == 0) {
                        var methodName = value.replace("remote[", "").replace("]", "");
                        wheremethods[field] = methodName;
                        $.ajax({
                            type: "POST",
                            url: window.currentUrl,
                            data: "mode=default&method=" + $.toJSONString(wheremethods),
                            cache: false,
                            async: false,
                            success: function (data) {
                                realvalue = data;
                            }, error: function (data) {
                                realvalue = null;
                            }
                        });
                    }
                    else {
                        realvalue = value;
                    }
                    if (where != "") where += " and ";
                    if (condition == "%") {
                        where += field + " like " + "'%" + realvalue + "'";
                    }
                    else if (condition == "%%") {
                        where += field + " like " + "'%" + realvalue + "%'";
                    }
                    else {
                        where += field +" " + condition +  " '" + realvalue + "'";
                    }
                }
                //if (hasRemote) {
                //    var whereObjs = getDefault($.toJSONString(wheremethods));
                //    var whereObj = $.parseJSON(whereObjs);
                //    for (var property in whereObj) {
                //        whereObject[property] = whereObj[property];
                //    }
                //}
                return where;
            }
        }
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            $(this).plotpiechart('options').whereString = where;
            var olddata = $(this).data();
            if (olddata != undefined && olddata.plot != undefined)
                olddata.plot.destroy();
            var options = $(this).plotpiechart('options');
            var data = $(this).plotpiechart('loadData');
            var truedata = data.data;
            var showFields = data.showFields;
            var plotOptions = $(this).plotpiechart('getplotOptions', { showFields: showFields });
            $(this).data('plotOptions', plotOptions);
            $(this).data('plotdata', truedata);
            if (truedata.length > 0) {
                var plot = $.jqplot($(this).attr('id'), [truedata], plotOptions);
                $(this).data('plot', plot);
                if (options.onClick != undefined) {
                    $(this).bind('jqplotDataClick', eval(options.onClick));
                }
            }
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------

//Line chart------------------------------------------------------------------------------------------------------------------------
$.fn.plotlinechart = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.plotlinechart.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).plotlinechart('initialize', options);
            if (!$(this).hasClass($.fn.plotlinechart.foo)) {
                $(this).addClass($.fn.plotlinechart.foo)
            }
        });
    }
};

$.fn.plotlinechart.foo = 'info-plotlinechart';

$.fn.plotlinechart.defaults = {
    title: "",
    fill: false,
    legendShow: true,
};

$.fn.plotlinechart.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var chartOptions = new Object();
            chartOptions.title = $.fn.plotlinechart.defaults.title;      //default option
            chartOptions.fill = $.fn.plotlinechart.defaults.fill;
            chartOptions.legendShow = $.fn.plotlinechart.defaults.legendShow;

            var htmlOptions = getInfolightOption($(this)); //html option
            for (var property in htmlOptions) {
                chartOptions[property] = htmlOptions[property];
            }
            chartOptions.whereString = $(this).plotlinechart('getWhereItem');
            $(this).data('options', chartOptions);

            var title = chartOptions.title;

            if (chartOptions.alwaysClose) {
                $(this).plotlinechart('options').whereString = '1=0';
            }
            if (chartOptions.renderObjectID != undefined && chartOptions.renderObjectID != "") {
                if ($('#' + chartOptions.renderObjectID) != undefined) {
                    $(this).appendTo($('#' + chartOptions.renderObjectID));
                }
            }
            $(this).plotlinechart('load');

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
    load: function (jq) {
        jq.each(function () {
            var options = $(this).plotlinechart('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                if (options.onBeforeLoad != undefined) {
                    options.onBeforeLoad.call(this);
                }
                var queryWord = new Object();
                if (options.whereString != undefined) {
                    queryWord.whereString = options.whereString;
                }
                var datalist = $(this).plotlinechart('loadData');
                var data = datalist.data;
                var ticks = datalist.ticks;
                var series = datalist.series;
                var plotOptions = $(this).plotlinechart('getplotOptions', { ticks: ticks, series: series });

                $(this).data('plotOptions', plotOptions);
                $(this).data('plotdata', data);

                if (data.length > 0) {
                    var plot = $.jqplot($(this).attr('id'), data, plotOptions);
                    $(this).data('plot', plot);
                    if (options.onClick != undefined) {
                        $(this).bind('jqplotDataClick', eval(options.onClick));
                    }
                }
            }
        });
    },
    loadData: function (jq) {
        var data = [];
        var ticks = [];
        var series = [];
        if (jq.length > 0) {
            var options = $(jq[0]).plotlinechart('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                var url = getRemoteUrl(options.remoteName, options.tableName, false); //呼叫Server取得數據的url
                var queryWord = new Object();
                if (options.whereString != undefined) {
                    queryWord.whereString = options.whereString;
                }
                $.ajax({
                    type: "POST",
                    url: url,
                    data: "queryWord=" + $.toJSONString(queryWord),
                    cache: false,
                    async: false,
                    success: function (returndata) {
                        if (returndata != null) {
                            $(jq[0]).data('data', returndata);
                            var dataFields = options.dataFields;
                            var rows = $.parseJSON(returndata);
                            if (rows.length > 0) {
                                for (var i = 0; i < rows.length; i++) {
                                    //ticks[i] = [rows[i][options.keyField], rows[i][options.keyShowField]];
                                    ticks.push(rows[i][options.keyField]);
                                }
                                for (var j = 0; j < dataFields.length; j++) {
                                    var data2 = [];
                                    var datafieldoption = dataFields[j];
                                    series[j] = {
                                        label: datafieldoption.captionFieldName == "" ? datafieldoption.caption : rows[0][datafieldoption.captionFieldName],
                                        lineWidth: datafieldoption.lineWidth,
                                        markerOptions: { style: datafieldoption.markerStyle },
                                        pointLabels: { show: datafieldoption.showPointLabels }
                                    };
                                    for (var i = 0; i < rows.length; i++) {
                                        data2[i] = rows[i][datafieldoption.fieldName]; //給data數組賦值
                                        //data2[i] = [rows[i][options.keyField],rows[i][datafieldoption.fieldName]]; //給data數組賦值
                                    }
                                    data.push(data2);
                                }
                            }
                        }
                    },
                    error: function () {
                        //alert('error');
                    }
                });
            }
            return { data: data, ticks: ticks, series: series };
        }
    },
    getplotOptions: function (jq, ticksandseries) {
        var ticks = ticksandseries.ticks;
        var series = ticksandseries.series;
        if (jq.length > 0) {
            var plotlinechart = jq[0];
            var title = $(plotlinechart).plotlinechart('options').title;
            var legendShow = $(plotlinechart).plotlinechart('options').legendShow;
            var legendLocation = $(plotlinechart).plotlinechart('options').legendLocation;
            var legendPlacement = $(plotlinechart).plotlinechart('options').legendPlacement;

            var options = {
                title: title,
                series: series,
                axes: {
                    xaxis: {
                        //label: "X Axis",
                        renderer: $.jqplot.CategoryAxisRenderer, //x轴绘制方式
                        ticks: ticks,
                        pad: 0
                    },
                    yaxis: {
                        //label: "Y Axis"
                    }
                }
            }
            if (legendShow) {
                var legend = {
                    show: legendShow,
                    location: legendLocation,
                    placement: legendPlacement
                }
                options.legend = legend;
            }

            return options;
        }
        return [];
    },
    getData: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('data') == undefined) {
                return [];
            }
            return $(jq[0]).data('data');
        }
        return [];
    },
    resize: function (jq, m) {
        if (jq.length > 0) {
            var options = $(jq[0]).plotlinechart('options');
            if (m) {
                var data = $(jq[0]).data();
                if (data != undefined) {
                    var plot = data.plot;
                    if (plot != undefined) {
                        $(jq[0]).width(options.width / 2);
                        //plot._width = plotwidth / 2;
                        plot.destroy();
                        var plotnew = $.jqplot($(jq[0]).attr('id'), data.plotdata, data.plotOptions);
                        $(jq[0]).data('plot', plotnew);
                    }
                }
            }
            else {
                var data = $(jq[0]).data();
                if (data != undefined) {
                    var plot = data.plot;
                    if (plot != undefined) {
                        $(jq[0]).width(options.width);
                        plot.destroy();
                        var plotnew = $.jqplot($(jq[0]).attr('id'), data.plotdata, data.plotOptions);
                        $(jq[0]).data('plot', plotnew);
                    }
                }
            }
        }
    },
    getWhereItem: function (jq, op) {
        if (jq.length > 0) {
            var options = new Object();
            if (op != undefined) {
                options = op;
            }
            else {
                options = getInfolightOption($(jq[0]));
            }
            if (options.whereItems != undefined) {
                var where = '';
                var wheremethods = new Object();
                var hasRemote = false;
                for (var i = 0; i < options.whereItems.length; i++) {
                    var field = options.whereItems[i].field;
                    var value = options.whereItems[i].value;
                    var condition = options.whereItems[i].condition;
                    var realvalue = "";
                    if (condition == undefined)
                        chartOptions = "=";

                    if (value.indexOf("client[") == 0) {
                        var methodName = value.replace("client[", "").replace("]", "");
                        realvalue = eval(methodName).call();
                    }
                    else if (value.indexOf("remote[") == 0) {
                        var methodName = value.replace("remote[", "").replace("]", "");
                        wheremethods[field] = methodName;
                        $.ajax({
                            type: "POST",
                            url: window.currentUrl,
                            data: "mode=default&method=" + $.toJSONString(wheremethods),
                            cache: false,
                            async: false,
                            success: function (data) {
                                realvalue = data;
                            }, error: function (data) {
                                realvalue = null;
                            }
                        });
                    }
                    else {
                        realvalue = value;
                    }
                    if (where != "") where += " and ";
                    if (condition == "%") {
                        where += field + " like " + "'%" + realvalue + "'";
                    }
                    else if (condition == "%%") {
                        where += field + " like " + "'%" + realvalue + "%'";
                    }
                    else {
                        where += field + " " + condition + " '" + realvalue + "'";
                    }
                }
                //if (hasRemote) {
                //    var whereObjs = getDefault($.toJSONString(wheremethods));
                //    var whereObj = $.parseJSON(whereObjs);
                //    for (var property in whereObj) {
                //        whereObject[property] = whereObj[property];
                //    }
                //}
                return where;
            }
        }
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            $(this).plotlinechart('options').whereString = where;
            var olddata = $(this).data();
            if (olddata != undefined && olddata.plot != undefined)
                olddata.plot.destroy();
            var options = $(this).plotlinechart('options');
            var datalist = $(this).plotlinechart('loadData');
            var data = datalist.data;
            var ticks = datalist.ticks;
            var series = datalist.series;
            var plotOptions = $(this).plotlinechart('getplotOptions', { ticks: ticks, series: series });

            $(this).data('plotOptions', plotOptions);
            $(this).data('plotdata', data);

            if (data.length > 0) {
                var plot = $.jqplot($(this).attr('id'), data, plotOptions);
                $(this).data('plot', plot);
                if (options.onClick != undefined) {
                    $(this).bind('jqplotDataClick', eval(options.onClick));
                }
            }
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
//bar chart------------------------------------------------------------------------------------------------------------------------
$.fn.plotbarchart = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.plotbarchart.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).plotbarchart('initialize', options);
            if (!$(this).hasClass($.fn.plotbarchart.foo)) {
                $(this).addClass($.fn.plotbarchart.foo)
            }
        });
    }
};

$.fn.plotbarchart.foo = 'info-plotbarchart';

$.fn.plotbarchart.defaults = {
    title: "",
    Label: "",
    barWidth: 20,
    labelShow: true,
    legendShow: true,
    stack: false
};

$.fn.plotbarchart.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var chartOptions = new Object();
            chartOptions.title = $.fn.plotbarchart.defaults.title;      //default option
            chartOptions.Label = $.fn.plotbarchart.defaults.Label;
            chartOptions.labelShow = $.fn.plotbarchart.defaults.labelShow;
            chartOptions.barWidth = $.fn.plotbarchart.defaults.barWidth;
            chartOptions.legendShow = $.fn.plotbarchart.defaults.legendShow;
            chartOptions.stack = $.fn.plotbarchart.defaults.stack;

            var htmlOptions = getInfolightOption($(this)); //html option
            for (var property in htmlOptions) {
                chartOptions[property] = htmlOptions[property];
            }
            chartOptions.whereString = $(this).plotbarchart('getWhereItem');
            $(this).data('options', chartOptions);

            var title = chartOptions.title;

            if (chartOptions.alwaysClose) {
                $(this).plotbarchart('options').whereString = '1=0';
            }
            if (chartOptions.renderObjectID != undefined && chartOptions.renderObjectID != "") {
                if ($('#' + chartOptions.renderObjectID) != undefined) {
                    $(this).appendTo($('#' + chartOptions.renderObjectID));
                }
            }
            $(this).plotbarchart('load');

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
    load: function (jq) {
        jq.each(function () {
            var options = $(this).plotbarchart('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                if (options.onBeforeLoad != undefined) {
                    options.onBeforeLoad.call(this);
                }
                var queryWord = new Object();
                if (options.whereString != undefined) {
                    queryWord.whereString = options.whereString;
                }
                var datalist = $(this).plotbarchart('loadData');
                var data = datalist.data;
                var ticks = datalist.ticks;
                var series = datalist.series;

                var plotOptions = $(this).plotbarchart('getplotOptions', { ticks: ticks, series: series });

                $(this).data('plotOptions', plotOptions);
                $(this).data('plotdata', data);

                if (data.length > 0) {
                    var plot = $.jqplot($(this).attr('id'), data, plotOptions);
                    $(this).data('plot', plot);
                    if (options.onClick != undefined) {
                        $(this).bind('jqplotDataClick', eval(options.onClick));
                    }
                }
            }
        });
    },
    loadData: function (jq) {
        var data = [];
        var ticks = [];
        var series = [];
        if (jq.length > 0) {
            var options = $(jq[0]).plotbarchart('options');
            if (options.remoteName != undefined && options.remoteName != "" && options.tableName != undefined) {
                var url = getRemoteUrl(options.remoteName, options.tableName, false); //呼叫Server取得數據的url
                var queryWord = new Object();
                if (options.whereString != undefined) {
                    queryWord.whereString = options.whereString;
                }
                $.ajax({
                    type: "POST",
                    url: url,
                    data: "queryWord=" + $.toJSONString(queryWord),
                    cache: false,
                    async: false,
                    success: function (returndata) {
                        if (returndata != null && returndata != []) {
                            $(jq[0]).data('data', returndata);
                            var dataFields = options.dataFields;
                            var rows = $.parseJSON(returndata);
                            if (rows.length > 0) {
                                for (var i = 0; i < rows.length; i++) {
                                    ticks.push(rows[i][options.keyField]);
                                }
                                for (var j = 0; j < dataFields.length; j++) {
                                    var data2 = [];
                                    var datafieldoption = dataFields[j];
                                    series[j] = { label: datafieldoption.captionFieldName == "" ? datafieldoption.caption : rows[0][datafieldoption.captionFieldName] };
                                    for (var i = 0; i < rows.length; i++) {
                                        //data2[i] = [rows[i][options.keyField], rows[i][datafieldoption.fieldName]]; //給data數組賦值
                                        data2.push(rows[i][datafieldoption.fieldName]); //給data數組賦值
                                    }
                                    data[j] = data2
                                }
                            }
                        }
                    },
                    error: function () {
                        //alert('error');
                    }
                });
            }
            return { data: data, ticks: ticks, series: series };
        }
    },
    getplotOptions: function (jq, ticksandseries) {
        var ticks = ticksandseries.ticks;
        var series = ticksandseries.series;
        if (jq.length > 0) {
            var plotbarchart = jq[0];
            var title = $(plotbarchart).plotbarchart('options').title;
            var labelShow = $(plotbarchart).plotbarchart('options').labelShow;
            var barWidth = $(plotbarchart).plotbarchart('options').barWidth;
            var legendShow = $(plotbarchart).plotbarchart('options').legendShow;
            var legendLocation = $(plotbarchart).plotbarchart('options').legendLocation;
            var legendPlacement = $(plotbarchart).plotbarchart('options').legendPlacement;
            var keyField = $(plotbarchart).plotbarchart('options').keyField;
            var valueField = $(plotbarchart).plotbarchart('options').valueField;
            var labelStyle = $(plotbarchart).plotbarchart('options').labelStyle;
            var stack = $(plotbarchart).plotbarchart('options').stack;
            var pointLabels = $(plotbarchart).plotbarchart('options').pointLabels;

                
            var options = {
                title:title,
                stackSeries: stack,
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer, //使用柱状图表示
                    rendererOptions: {
                        barWidth: barWidth,
                        barMargin: 10   //柱状体组之间间隔
                    },
                    pointLabels: { show: pointLabels }
                }, axes: {
                    xaxis: {
                        ticks: ticks,
                        renderer: $.jqplot.CategoryAxisRenderer //x轴绘制方式
                    }, yaxis: {
                        padMin: 0
                    }
                },
                series: series  
            }
            if (legendShow)
            {
                var legend ={
                    show: legendShow,
                    location: legendLocation,
                    placement: legendPlacement
                }
                options.legend = legend;
            }
            return options;
        }
        return [];
    },
    resize: function (jq, m) {
        if (jq.length > 0) {
            var options = $(jq[0]).plotbarchart('options');
            if (m) {
                var data = $(jq[0]).data();
                if (data != undefined) {
                    var plot = data.plot;
                    if (plot != undefined) {
                        $(jq[0]).width(options.width / 2);
                        //plot._width = plotwidth / 2;
                        plot.destroy();
                        var plotnew = $.jqplot($(jq[0]).attr('id'), data.plotdata, data.plotOptions);
                        $(jq[0]).data('plot', plotnew);
                    }
                }
            }
            else {
                var data = $(jq[0]).data();
                if (data != undefined) {
                    var plot = data.plot;
                    if (plot != undefined) {
                        $(jq[0]).width(options.width);
                        plot.destroy();
                        var plotnew = $.jqplot($(jq[0]).attr('id'), data.plotdata, data.plotOptions);
                        $(jq[0]).data('plot', plotnew);
                    }
                }
            }
        }
    },
    getData: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('data') == undefined) {
                return [];
            }
            return $(jq[0]).data('data');
        }
        return [];
    },
    getWhereItem: function (jq, op) {
        if (jq.length > 0) {
            var options = new Object();
            if (op != undefined) {
                options = op;
            }
            else {
                options = getInfolightOption($(jq[0]));
            }
            if (options.whereItems != undefined) {
                var where = '';
                var wheremethods = new Object();
                var hasRemote = false;
                for (var i = 0; i < options.whereItems.length; i++) {
                    var field = options.whereItems[i].field;
                    var value = options.whereItems[i].value;
                    var condition = options.whereItems[i].condition;
                    var realvalue = "";
                    if (condition == undefined)
                        chartOptions = "=";

                    if (value.indexOf("client[") == 0) {
                        var methodName = value.replace("client[", "").replace("]", "");
                        realvalue = eval(methodName).call();
                    }
                    else if (value.indexOf("remote[") == 0) {
                        var methodName = value.replace("remote[", "").replace("]", "");
                        wheremethods[field] = methodName;
                        $.ajax({
                            type: "POST",
                            url: window.currentUrl,
                            data: "mode=default&method=" + $.toJSONString(wheremethods),
                            cache: false,
                            async: false,
                            success: function (data) {
                                realvalue = data;
                            }, error: function (data) {
                                realvalue = null;
                            }
                        });
                    }
                    else {
                        realvalue = value;
                    }
                    if (where != "") where += " and ";
                    if (condition == "%") {
                        where += field + " like " + "'%" + realvalue + "'";
                    }
                    else if (condition == "%%") {
                        where += field + " like " + "'%" + realvalue + "%'";
                    }
                    else {
                        where += field + " " + condition + " '" + realvalue + "'";
                    }
                }
                //if (hasRemote) {
                //    var whereObjs = getDefault($.toJSONString(wheremethods));
                //    var whereObj = $.parseJSON(whereObjs);
                //    for (var property in whereObj) {
                //        whereObject[property] = whereObj[property];
                //    }
                //}
                return where;
            }
        }
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            $(this).plotbarchart('options').whereString = where;
            var olddata = $(this).data();
            if (olddata != undefined && olddata.plot != undefined)
                olddata.plot.destroy();
            var options = $(this).plotbarchart('options');
            var datalist = $(this).plotbarchart('loadData');
            var data = datalist.data;
            var ticks = datalist.ticks;
            var series = datalist.series;

            var plotOptions = $(this).plotbarchart('getplotOptions', { ticks: ticks, series: series });

            $(this).data('plotOptions', plotOptions);
            $(this).data('plotdata', data);

            if (data.length > 0) {
                var plot = $.jqplot($(this).attr('id'), data, plotOptions);
                $(this).data('plot', plot);
                if (options.onClick != undefined) {
                    $(this).bind('jqplotDataClick', eval(options.onClick));
                }
            }
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
//dashboard  MeterGauge chart ------------------------------------------------------------------------------------------------------------------------
$.fn.plotDashBoard = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.plotDashBoard.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).plotDashBoard('initialize', options);
            if (!$(this).hasClass($.fn.plotDashBoard.foo)) {
                $(this).addClass($.fn.plotDashBoard.foo)
            }
        });
    }
};

$.fn.plotDashBoard.foo = 'info-plotDashBoard';

$.fn.plotDashBoard.defaults = {
    labelHeightAdjust: 0,
    ticks: [0, 20, 40, 60, 80, 100, 120],
    labelPosition: 'bottom',
    intervals: [40, 80, 120],   //取2,3,4这三个刻度值作为分隔线；该值的取定须参照表盘刻度值范围而定  
    intervalColors: ['#66cc66', '#E7E658', '#cc6666'],  //分别为上面分隔的域指定表示的颜色  
    labelColor:'black'
};

$.fn.plotDashBoard.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var chartOptions = new Object();
            chartOptions.labelHeightAdjust = $.fn.plotDashBoard.defaults.labelHeightAdjust;
            chartOptions.ticks = $.fn.plotDashBoard.defaults.ticks;
            chartOptions.labelPosition = $.fn.plotDashBoard.defaults.labelPosition;
            chartOptions.intervals = $.fn.plotDashBoard.defaults.intervals;
            chartOptions.intervalColors = $.fn.plotDashBoard.defaults.intervalColors;
            chartOptions.labelColor = $.fn.plotDashBoard.defaults.labelColor;

            var htmlOptions = getInfolightOption($(this)); //html option
            for (var property in htmlOptions) {
                chartOptions[property] = htmlOptions[property];
            }
            $(this).data('options', chartOptions);

            if (chartOptions.renderObjectID != undefined && chartOptions.renderObjectID != "") {
                if ($('#' + chartOptions.renderObjectID) != undefined) {
                    $(this).appendTo($('#' + chartOptions.renderObjectID));
                }
            }
            $(this).plotDashBoard('load');
            $('.jqplot-meterGauge-label',$(this)).css('color',$(this).plotDashBoard('options').labelColor);
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
    load: function (jq) {
        jq.each(function () {
            var options = $(this).plotDashBoard('options');
            if (options.onBeforeLoad != undefined) {
                options.onBeforeLoad.call(this,options);
            }
            var datalist = [];
            datalist.push([options.value]);
            var data = datalist;

            var plotOptions = $(this).plotDashBoard('getplotOptions');

            $(this).data('plotOptions', plotOptions);
            $(this).data('plotdata', data);

            if (data.length > 0) {
                var plot = $.jqplot($(this).attr('id'), data, plotOptions);
                $(this).data('plot', plot);
                if (options.onClick != undefined) {
                    $(this).bind('jqplotDataClick', eval(options.onClick));
                }
            }
        });
    },
    getplotOptions: function (jq) {
        if (jq.length > 0) {
            var plotDashBoard = jq[0];
            var labelHeightAdjust = $(plotDashBoard).plotDashBoard('options').labelHeightAdjust;
            var ticks = $(plotDashBoard).plotDashBoard('options').ticks;
            var labelPosition = $(plotDashBoard).plotDashBoard('options').labelPosition;
            var intervals = $(plotDashBoard).plotDashBoard('options').intervals;
            var intervalColors = $(plotDashBoard).plotDashBoard('options').intervalColors;
            var label = $(plotDashBoard).plotDashBoard('options').label;
            var background = $(plotDashBoard).plotDashBoard('options').background;
            var ringColor = $(plotDashBoard).plotDashBoard('options').ringColor;
            var tickColor = $(plotDashBoard).plotDashBoard('options').tickColor;
            var ringWidth = $(plotDashBoard).plotDashBoard('options').ringWidth;

            var options = {
                seriesDefaults: {
                    renderer: $.jqplot.MeterGaugeRenderer,
                    rendererOptions: {
                        labelHeightAdjust: labelHeightAdjust,
                        ticks: ticks,
                        background: background,
                        ringColor: ringColor,
                        tickColor: tickColor,
                        ringWidth: ringWidth,
                        labelPosition: labelPosition,
                        intervals: intervals,
                        intervalColors: intervalColors,
                        label: label
                    }
                },
                grid: {
                    drawBorder: false,
                    shadow: false,
                    background: 'transparent'
                }
            }
            //if (legendShow) {
            //    var legend = {
            //        show: legendShow,
            //        location: legendLocation,
            //        placement: legendPlacement
            //    }
            //    options.legend = legend;
            //}
            return options;
        }
        return [];
    },
    resize: function (jq, m) {
        if (jq.length > 0) {
            var options = $(jq[0]).plotDashBoard('options');
            if (m) {
                var data = $(jq[0]).data();
                if (data != undefined) {
                    var plot = data.plot;
                    if (plot != undefined) {
                        $(jq[0]).width(options.width / 2);
                        //plot._width = plotwidth / 2;
                        plot.destroy();
                        var plotnew = $.jqplot($(jq[0]).attr('id'), data.plotdata, data.plotOptions);
                        $(jq[0]).data('plot', plotnew);
                    }
                }
            }
            else {
                var data = $(jq[0]).data();
                if (data != undefined) {
                    var plot = data.plot;
                    if (plot != undefined) {
                        $(jq[0]).width(options.width);
                        plot.destroy();
                        var plotnew = $.jqplot($(jq[0]).attr('id'), data.plotdata, data.plotOptions);
                        $(jq[0]).data('plot', plotnew);
                    }
                }
            }
        }
    },
    getData: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('data') == undefined) {
                return [];
            }
            return $(jq[0]).data('data');
        }
        return [];
    },
    setValue: function (jq,value) {
        jq.each(function () {
            var options = $(this).plotDashBoard('options');
            options.value = value;
            $(this).plotDashBoard('load');
            $('.jqplot-meterGauge-label', $(this)).css('color', $(this).plotDashBoard('options').labelColor);
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
