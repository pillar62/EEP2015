/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

Infolight.FormHelper = function() {
    Ext.Ajax.request({
        url: "../ExtJs/infolight/ExtGetSysMessage.ashx",
        method: "POST",
        params: { type: 'form' },
        success: function(response, option) {
            var result = Ext.decode(response.responseText);
            if (result.success === true) {
                Infolight.FormHelper.msgValidFail = result.msgValidFail;
                Infolight.FormHelper.msgSaveDetails = result.msgSaveDetails;
            }
            else if (result.success === false) {
                Infolight.Exception.throwEx(response.responseText);
            }
        }
    });
    return {
        msgValidFail: '',
        msgSaveDetails: '',
        setRefButtonEnable: function(field, enable) {
            var refButton = Ext.getCmp('refBtn' + field.name);
            if (refButton) {
                if (enable) {
                    refButton.enable();
                }
                else {
                    refButton.disable();
                }
            }
        },
        refButtonClick: function() {
            if (this.updatePanId && this.targetPan) {
                Infolight.createModalPan(this.targetPan);
                __doPostBack(this.updatePanId);
            }
        }
    };
} ();