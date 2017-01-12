/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

Infolight.Default = function() {
    return {
        isSysDefaultValue: function(value) {
            if (typeof value !== "string") {
                return false;
            }
            var lv = value.toLowerCase();
            return (lv === '_usercode'    ||
                    lv === '_username'    ||
                    lv === '_solution'    ||
                    lv === '_database'    ||
                    lv === '_sitecode'    ||
                    lv === '_ipaddress'   ||
                    lv === '_language'    ||
                    lv === '_today'       ||
                    lv === '_sysdate'     ||
                    lv === '_servertoday' ||
                    lv === '_firstday'    ||
                    lv === '_lastday'     ||
                    lv === '_firstdaylm'  ||
                    lv === '_lastdaylm'   ||
                    lv === '_firstdayty'  ||
                    lv === '_lastdayty'   ||
                    lv === '_firstdayly'  ||
                    lv === '_lastdayly');
        },
        setDefaultValues: function(defMethods, callBackMethod, scope) {
            Ext.Ajax.request({
                url: '../ExtJs/infolight/ExtDefault.ashx',
                method: 'POST',
                params: {
                    methods: Ext.encode(defMethods)
                },
                success: function(response, option) {
                    if (response.responseText) {
                        var result = Ext.decode(response.responseText);
                        if (result.success === true) {
                            var defValues = result.defObj;
                            callBackMethod.apply(this, [defValues]);
                        }
                        else if (result.success === false) {
                            Infolight.Exception.throwEx(response.responseText);
                        }
                    }
                },
                scope: scope || this
            });
        }
    };
} ();