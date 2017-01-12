$(document).ready(function () {
    $("." + $.fn.officeplate.foo).officeplate({});
});
//bar chart------------------------------------------------------------------------------------------------------------------------
$.fn.officeplate = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.officeplate.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            var options = methodName;
            $(this).officeplate('initialize', options);
            if (!$(this).hasClass($.fn.officeplate.foo)) {
                $(this).addClass($.fn.officeplate.foo)
            }
        });
    }
};

$.fn.officeplate.foo = 'info-officeplate';

$.fn.officeplate.defaults = {
};

$.fn.officeplate.methods = {
    initialize: function (jq, options) {
        jq.each(function () {
            var officeOptions = new Object();
            var htmlOptions = getInfolightOption($(this)); //html option
            for (var property in htmlOptions) {
                officeOptions[property] = htmlOptions[property];
            }
            $(this).data('options', officeOptions);

            if (officeOptions.alwaysClose) {
                $(this).officeplate('options').whereString = '1=0';
            }
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
    export: function (jq) {
        if (jq.length > 0) {
            var options = $(jq[0]).officeplate('options');
            var officeOptions = new Object();
            for (var property in options) {
                if (property != "dataSourceCollection")
                    officeOptions[property] = options[property];
                else {
                    var datasourcec = options['dataSourceCollection'];
                    for (var i = 0; i < datasourcec.length; i++) {
                        var datasource = datasourcec[i];
                        var obj = new Object();
                        if (datasource.whereItems != null) {
                            for (var j = 0; j < datasource.whereItems.length;j++) {
                                var where = datasource.whereItems[j];
                                var filed = where.field;
                                var value = where.value;
                                var condition = where.condition;
                                if (value != undefined) {
                                    if (value.indexOf("client[") == 0) {
                                        var methodName = value.replace("client[", "").replace("]", "");
                                        where.value = eval(methodName).call();
                                    }
                                    else if (value.indexOf("remote[") == 0) {
                                        var methods = new Object();
                                        var methodName = defaultValue.replace("remote[", "").replace("]", "");
                                        methods[field] = methodName;
                                        var defaultObjs = getDefault($.toJSONString(methods));
                                        var defaultObj = $.parseJSON(defaultObjs);
                                        for (var property in defaultObj) {
                                            where.value = defaultObj[property];
                                        }
                                    }
                                    else {
                                    }
                                }
                            }
                        }
                        if (i == 0 && options.whereString != "") {
                            datasource.whereString = options.whereString;
                        }
                    }
                    officeOptions[property] = $.toJSONString(options['dataSourceCollection']);
                }
            }
            officeOptions.mode = 'office';
            $.ajax({
                type: "POST",
                dataType: 'text',
                url: window.currentUrl,
                data: officeOptions,
                cache: false,
                async: false,
                success: function (data) {
                    window.open('../handler/JqFileHandler.ashx?File=' + data, 'download');
                    data.responseText = "";
                },
                error: function (data) {
                    alert(data);
                }
            });
        }
    },
    setWhere: function (jq, where) {
        jq.each(function () {
            $(this).officeplate('options').whereString = where;
        });
    }
};
//--------------------------------------------------------------------------------------------------------------------------------
