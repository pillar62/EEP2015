Ext.ns('Infolight');

Infolight = function() {
    if (Ext.isIE6 === false) {
        Ext.QuickTips.init();
    }
    return {
        convertDate: function(dateString) {
            var dt = Ext.Date.parse(dateString, 'Y-m-d');
            if (dt == undefined) {
                dt = Ext.Date.parse(dateString, 'Y/m/d');
            }
            if (dt == undefined) {
                dt = Ext.Date.parse(dateString, 'Y-n-j');
            }
            if (dt == undefined) {
                dt = Ext.Date.parse(dateString, 'Y/n/j');
            }
            return dt;
        },
        setExtToolItemEnable: function(items, id, enable) {
            var btn = items.find(function(item) {
                return item.id === id;
            });
            if (btn) {
                if (enable === true) {
                    btn.enable();
                }
                else if (enable === false) {
                    btn.disable();
                }
            }
        },
        createModalPan: function(targetPan) {
            var trigger = String.format('_{0}Trigger', targetPan);
            if (!Ext.get(trigger)) {
                Ext.DomHelper.append(document.body, {
                    tag: 'input',
                    id: trigger,
                    type: 'button',
                    style: 'display:none'
                });
                var handlerButton = $get(trigger);
                if (handlerButton) {
                    $create(AjaxControlToolkit.ModalPopupBehavior, {
                        "BackgroundCssClass": 'ajaxmodalpan_modalBackground',
                        "Drag": false,
                        "PopupControlID": targetPan,
                        "id": String.format('{0}behavior', targetPan)
                    }, null, null, handlerButton);
                }
            }
        }
    };
} ();