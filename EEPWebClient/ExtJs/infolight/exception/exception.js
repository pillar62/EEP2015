/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

Infolight.Exception = function() {
    var formPanel = new Ext.FormPanel({
        frame: true,
        labelAlign: 'top',
        items:
        [
            {
                fieldLabel: 'the error message',
                xtype: 'textarea',
                labelStyle: 'font-weight:bold;',
                name: 'message',
                width: 470,
                height: 100,
                readOnly: true
            },
            {
                fieldLabel: 'the error stack trace',
                xtype: 'textarea',
                labelStyle: 'font-weight:bold;',
                name: 'stack',
                width: 470,
                height: 200,
                readOnly: true
            }
        ]
    });
    var win = new Ext.Window({
        closable: true,
        resizable: false,
        modal: true,
        shadow: true,
        width: 500,
        height: 400,
        shadowOffset: 8,
        title: 'error!',
        layout: 'fit',
        closeAction: 'hide',
        items: [formPanel]
    });
    return {
        throwEx: function(exceptionText) {
            var exception = Ext.decode(exceptionText);
            if (exception.warning) {
                Ext.Msg.alert('', exception.warning);
            }
            else {
                if (exception.message == '75FF57F7-7AC0-43c8-9454-C92B4A2723BB') {
                    document.location = '../Timeout.aspx';
                }
                else {
                    win.show(null, function() {
                        if (exceptionText) {
                            formPanel.getForm().setValues(exception);
                        }
                    });
                }
            }
        }
    };
} ();