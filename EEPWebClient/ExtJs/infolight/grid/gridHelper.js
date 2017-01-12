/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>
Ext.Loader.setConfig({
    enabled: true
});
Ext.Loader.setPath('Ext.ux', '../../ux');
Ext.require([
    'Ext.util.*',
    'Ext.state.*',
    'Ext.ux.RowExpander',
    'Ext.ux.PreviewPlugin',
    'Ext.ux.CheckColumn'
]);

Infolight.GridHelper = function () {
    Ext.Ajax.request({
        url: "../ExtJs/infolight/ExtGetSysMessage.ashx",
        method: "POST",
        params: { type: 'grid' },
        success: function (response, option) {
            var result = Ext.decode(response.responseText);
            if (result.success === true) {
                Infolight.GridHelper.msgSureDelete = result.msgSureDelete;
                Infolight.GridHelper.msgNonSelToDelete = result.msgNonSelToDelete;
                Infolight.GridHelper.msgNonSelToEdit = result.msgNonSelToEdit;
                Infolight.GridHelper.msgPersonalSaved = result.msgPersonalSaved;
                Infolight.GridHelper.msgPersonalLoaded = result.msgPersonalLoaded;
            }
            else if (result.success === false) {
                Infolight.Exception.throwEx(response.responseText);
            }
        }
    });
    return {
        msgSureDelete: '',
        msgNonSelToDelete: '',
        msgNonSelToEdit: '',
        msgPersonalSaved: '',
        msgPersonalLoaded: '',
        createGrid: function (config) {
            var store = Ext.create('Ext.data.JsonStore', config.storeConfig);
            store = Ext.apply(store, { keys: config.keys });
            if (config.allowPage === true) {
                store.pageSize = config.pageConfig.pageSize;
                //store.pageSize = 25;
                store.getProxy().extraParams = Ext.apply(store.getProxy().extraParams, {
                    allowPage :config.allowPage,
                    //start: 0,
                    //limit: config.pageConfig.pageSize
                });
                store.loadPage(1);
                //page bar
                config.pageConfig.store = store;
                var pagingBar = new Ext.PagingToolbar(config.pageConfig);
                var pageBar = Ext.create('Ext.PagingToolbar', config.pageConfig);
                config.gridConfig.bbar = pagingBar;
            }
            else if (config.allowPage === false) {
                store.load();
            }
            config.gridConfig = Ext.apply(config.gridConfig, {
                store: store,
                columns: config.columns,
                layout: 'fit',
                border: true,
                
            });
            if (config.expandRowTemplateHtml != "" && config.expandRowTemplateHtml != null)
            {
                Ext.apply(config.gridConfig, {
                    plugins: [{
                        ptype: 'rowexpander',
                        rowBodyTpl: [
                            config.expandRowTemplateHtml,
                        ]

                    }]
                });
            }
            var grid = new Infolight.Grid(config.gridConfig);
            grid = Ext.apply(grid, {
                valids: config.validArray,
                allowPage: config.allowPage,
                keys: config.keys,
                autoApply: config.autoApply,
                focusEventHandlers: config.focusEventHandlers,
                leaveEventHandlers: config.leaveEventHandlers,
                eidtFormViewID: config.eidtFormViewID,
                editFormViewConfig: config.editFormViewConfig,
                editFormViewWidth: config.editFormViewWidth,
                editFormViewHeght: config.editFormViewHeight,
                editFormViewKeyFields: config.editFormViewKeyFields,
                editFormViewFields: config.editFormViewFields,
                editFormViewValids: config.editFormViewValids,
                editFormViewDefaultValues:config.editFormViewDefaultValues,
                editFormViewFocusEventHandlers:config.editFormViewFocusEventHandlers,
                editFormViewLeaveEventHandlers: config.editFormViewLeaveEventHandlers
            });
            if (!config.isView) {
                var rowEditing = Ext.create('Ext.grid.plugin.RowEditing', {
                    clicksToMoveEditor: 2,
                    autoCancel: true
                });
                grid.initPlugin(rowEditing);
            }
            grid.on('beforeedit', function (editor, e) {
                if(e && editor)
                    grid.setNavState('editing');
            });
            if (config.editPan) {
                Infolight.createModalPan(config.editPan);
            }
            if (config.queryPan) {
                Infolight.createModalPan(config.queryPan);
            }
            return grid;
        },
        combColumnFomatter: function (value, metaData, record, rowIndex, colIndex, store) {
            var showfield = this.showfield.toUpperCase();
            for (var field in record.data) {
                if (field.toUpperCase() === showfield) {
                    return record.data[field];
                    break;
                }
            }
            return value;
        },
        insertStore: function (store, changedRecord) {
            var Row = Ext.data.Record.create(store.fields);
            var record = new Row(changedRecord);
            if (record) {
                store.add(record);
                record.dirty = true;
                record.modified = changedRecord;
                store.modified.push(record);
            }
        },
        updateStore: function (store, recordKeys, changedRecord) {
            var recordIndex = store.findBy(function (record) {
                var match = true;
                Ext.iterate(recordKeys, function (key, value, obj) {
                    if (value !== record.get(key)) {
                        match = false;
                        return false;
                    }
                });
                return match;
            });
            var record = store.getAt(recordIndex);
            if (record) {
                Ext.iterate(changedRecord, function (key, value) {
                    record.set(key, value);
                });
            }
        }
    };
} ();