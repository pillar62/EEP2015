/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

Infolight.Form = Ext.extend(Ext.form.FormPanel, {
    editMode: 'readonly',
    view: null,
    details: null,
    keyFields: [],
    fields: [],
    valids: [],
    focusEventHandlers: [],
    leaveEventHandlers: [],
    constructor: function(config) {
        this.callParent(arguments);//Calling the parent class constructor
        this.initConfig(config);//Initializing the component
        Ext.apply(this, {
            keyFields: config.keyFields,
            fields: config.fields,
            valids: config.valids,
            focusEventHandlers: config.focusEventHandlers,
            leaveEventHandlers: config.leaveEventHandlers
        });
    },
    load: function(locRecord, params) {
        var loadingCount = 0;
        var baseForm = this.getForm();
        Ext.each(this.fields, function(masterField) {
            var field = baseForm.findField(masterField);
            if (field) {
                if (field.getXType() && field.getXType() == 'infoCombo') {
                    var storeIndex = -1;
                    storeIndex = field.store.findBy(function(rcd, id) {
                        return locRecord[field.name] == rcd.get(field.valueField);
                    });
                    if (storeIndex == -1) {
                        loadingCount++;
                        Ext.Ajax.request({
                            url: '../ExtJs/infolight/ExtGetRecord.ashx',
                            method: 'POST',
                            params: {
                                foreignCacheDataSet: params.cacheDataSet,
                                foreignCommand: params.command,
                                foreignKey: field.name,
                                foreignCommandKeyValues: Ext.encode(locRecord),
                                key: field.valueField,
                                module: field.store.proxy.extraParams.module,
                                command: field.store.proxy.extraParams.command,
                                alias: field.store.proxy.extraParams.alias,
                                sql: field.store.proxy.extraParams.sql,
                                cacheDataSet: field.store.proxy.extraParams.cacheDataSet,
                                fields: field.store.proxy.extraParams.fields
                            },
                            success: function(response, option) {
                                if (response.responseText) {
                                    var result = Ext.decode(response.responseText);
                                    if (result.success === true) {
                                        //var index = field.store.findBy(function(rcd, id) {
                                        //    return result.data[field.valueField] == rcd.get(field.valueField);
                                        //});
                                        //if (index == -1) {
                                        //    var Row = Ext.data.Record.create(field.store.fields);
                                        //    field.store.add(new Row(result.data));
                                        //}
                                    }
                                    else if (result.success === false) {
                                        //Infolight.Exception.throwEx(response.responseText);
                                    }
                                }
                            },
                            callback: function() {
                                if (--loadingCount == 0) {
                                    this.loadData(locRecord, params);
                                }
                            },
                            scope: this
                        });
                    }
                }
            }
        }, this);
        if (loadingCount == 0) {
            this.loadData(locRecord, params);
        }
    },
    loadData: function (locRecord, params) {
        var form = this;
        form.getForm().load({
            url: '../ExtJs/infolight/ExtGetRecord.ashx',
            params: {
                keyvalues: Ext.encode(locRecord),
                fields: this.fields,
                command: params.command,
                cacheDataSet: params.cacheDataSet
            },
            success: function (response, option) {
                form.setNavState('browse');
                if (option.response.responseText) {
                    var result = Ext.decode(option.response.responseText);
                    if (result.success === true) {
                        //response.loadRecord(result.data);
                    }
                    else if (result.success === false) {
                        Infolight.Exception.throwEx(option.response.responseText);
                    }
                }
            },
            failure: function (f, action) {
                form.setNavState('browse');
                Infolight.Exception.throwEx(action.response.responseText);
            }
        });
        //this.fieldsEnable(true);
    },

    initValids: function () {
        Ext.each(this.fields, function(masterField) {
            var baseForm = this.getForm();
            var field = baseForm.findField(masterField);
            if (field) {
                if (this.valids && this.valids.length > 0) {
                    var index;
                    Ext.each(this.valids, function(valid,i) {
                        if (field.name == valid.field && valid.validConfig && valid.validConfig.srvValid) {
                            index = i;
                        }
                    });
                    if (index !== undefined && index >= 0) {
                        field.on('blur', function(field) {
                            Infolight.Validator.validFormViewField(field,
                                                               this.valids[index].validConfig.srvValid,
                                                               this.valids[index].validConfig.msg,
                                                               baseForm.getValues());
                        }, this);
                    }
                }
                field.on('blur', function(field) {
                    //column match
                    if (field.xtype && field.xtype == 'infoCombo' && field.columnMatch && field.columnMatch.length > 0) {
                        var matchValues = {};
                        Ext.each(field.columnMatch, function(match) {
                            var index = field.store.findBy(function(record, id) {
                                return record.get(field.valueField) === field.getValue();
                            });
                            if (index != -1) {
                                var record = field.store.getAt(index);
                                matchValues[match.destField] = record.get(match.srcField);
                            }
                        });
                        baseForm.setValues(matchValues);
                    }
                });
            }
        }, this);
    },
    addHandlers: function() {
        var baseForm = this.getForm();
        if (this.focusEventHandlers) {
            Ext.each(this.focusEventHandlers, function(handler) {
                var field = baseForm.findField(handler.field);
                if (field) {
                    field.on('focus', function(field) {
                        handler.handler(field);
                    });
                }
            });
        }
        if (this.leaveEventHandlers) {
            Ext.each(this.leaveEventHandlers, function(handler) {
                var field = baseForm.findField(handler.field);
                if (field) {
                    field.on('blur', function(field) {
                        handler.handler(field);
                    });
                }
            });
        }
    },
    setNavState: function(state) {
        var setExtToolItemEnable = function (panel, id, enable) {
            var btn = panel.down("#" + id);
            if (btn) {
                if (enable === true) {
                    btn.enable();
                }
                else if (enable === false) {
                    btn.disable();
                }
            }
        };
        if (state) {
            switch (state) {
                case 'browse':
                    setExtToolItemEnable(this, this.id + 'btnAdd', true);
                    setExtToolItemEnable(this, this.id + 'btnEdit', true);
                    setExtToolItemEnable(this, this.id + 'btnDelete', true);
                    setExtToolItemEnable(this, this.id + 'btnOK', false);
                    setExtToolItemEnable(this, this.id + 'btnCancel', false);
                    setExtToolItemEnable(this, this.id + 'btnSave', false);
                    setExtToolItemEnable(this, this.id + 'btnAbort', false);
                    break;
                case 'editing':
                    setExtToolItemEnable(this, this.id + 'btnAdd', false);
                    setExtToolItemEnable(this, this.id + 'btnEdit', false);
                    setExtToolItemEnable(this, this.id + 'btnDelete', false);
                    setExtToolItemEnable(this, this.id + 'btnOK', true);
                    setExtToolItemEnable(this, this.id + 'btnCancel', true);
                    setExtToolItemEnable(this, this.id + 'btnSave', true);
                    setExtToolItemEnable(this, this.id + 'btnAbort', true);
                    break;
                case 'loading':
                    setExtToolItemEnable(this, this.id + 'btnAdd', false);
                    setExtToolItemEnable(this, this.id + 'btnEdit', false);
                    setExtToolItemEnable(this, this.id + 'btnDelete', false);
                    setExtToolItemEnable(this, this.id + 'btnOK', false);
                    setExtToolItemEnable(this, this.id + 'btnCancel', false);
                    setExtToolItemEnable(this, this.id + 'btnSave', false);
                    setExtToolItemEnable(this, this.id + 'btnAbort', false);
                    break;
            }
        }
    },
    addRecord: function(defaultvalues) {
        var view = Ext.getCmp(this.view);
        if (view) {
            view.disable();
            if (view.store.proxy.extraParams.masterKeys) {
                var masterkeys = Ext.decode(view.store.proxy.extraParams.masterKeys);
                if (masterkeys) {
                    Ext.iterate(masterkeys, function (key, value, obj) {
                        defaultvalues[key] = value;
                    });
                }
            }
        }
        if (this.details != null && this.details.length > 0) {
            Ext.each(this.details, function(dtl) {
                var detail = Ext.getCmp(dtl);
                detail.disable();
                if (detail) {
                    detail.store.removeAll();
                }
            });
        }
        this.editMode = 'add';
        this.fieldsEnable(true);
        var baseForm = this.getForm();
        baseForm.reset();
        var srvDefMethods = null;
        for (var def in defaultvalues) {
            var defField = baseForm.findField(def);
            if (defField != undefined && defField != null) {
                if (typeof defaultvalues[def] === 'string') {
                    if (Infolight.Default.isSysDefaultValue(defaultvalues[def])) {
                        if (srvDefMethods === null) {
                            srvDefMethods = new Object();
                        }
                        srvDefMethods[def] = defaultvalues[def];
                    }
                    else if (defaultvalues[def].indexOf('@srvMethod:') === 0) {
                        if (srvDefMethods === null) {
                            srvDefMethods = new Object();
                        }
                        srvDefMethods[def] = defaultvalues[def].split(':')[1];
                    }
                    else if (defField.getXType() == 'datefield') {
                        if (Infolight.convertDate(defaultvalues[def])) {
                            defaultvalues[def] = Infolight.convertDate(defaultvalues[def]);
                            continue;
                        }
                    }
                }
            }
        }
        if (srvDefMethods !== null) {
            Infolight.Default.setDefaultValues(srvDefMethods, function(defValues) {
                Ext.iterate(defValues, function(key, value, obj) {
                    var defField = baseForm.findField(key);
                    if (defField.getXType() == 'datefield') {
                        defaultvalues[key] = Infolight.convertDate(value);
                    }
                    else {
                        defaultvalues[key] = value;
                    }
                });
                this.setDefValues(defaultvalues);
            }, this);
        }
        else {
            this.setDefValues(defaultvalues);
        }
        this.setNavState('editing');
        this.query('textfield')[0].focus(true, true);
    },
    setDefValues: function(defaultvalues) {
        var loadingCount = 0;
        var baseForm = this.getForm();
        Ext.each(baseForm.items.items, function(field) {
            if (field.xtype && field.xtype == 'infoCombo' && Ext.isDefined(defaultvalues[field.name])) {
                var index = field.store.find(field.valueField, field.value);
                if (index == -1) {
                    var keyvalues = new Object();
                    keyvalues[field.valueField] = defaultvalues[field.name];
                    loadingCount++;
                    Ext.Ajax.request({
                        url: '../ExtJs/infolight/ExtGetRecord.ashx',
                        method: 'POST',
                        params: {
                            keyvalues: Ext.encode(keyvalues),
                            module: field.store.baseParams.module,
                            command: field.store.baseParams.command,
                            cacheDataSet: field.store.baseParams.cacheDataSet,
                            fields: field.store.fields.keys.join(',')
                        },
                        callback: function() {
                            if (--loadingCount == 0) {
                                baseForm.setValues(defaultvalues);
                            }
                        },
                        success: function(response, option) {
                            if (response.responseText) {
                                var result = Ext.decode(response.responseText);
                                if (result.success === true) {
                                    var Row = Ext.data.Record.create(field.store.fields);
                                    field.store.add(new Row(result.data));
                                }
                                else if (result.success === false) {
                                    Infolight.Exception.throwEx(response.responseText);
                                }
                            }
                        }
                    });
                }
            }
        });
        if (loadingCount == 0) {
            baseForm.setValues(defaultvalues);
        }
        
    },
    editRecord: function() {
        var view = Ext.getCmp(this.view);
        if (view && view.getSelectionModel().hasSelection()) {
            view.disable();
            this.editMode = 'update';
            var baseForm = this.getForm();
            this.fieldsEnable(true);
            this.setNavState('editing');
        }
    },
    deleteRecord: function(autoApply) {
        var view = Ext.getCmp(this.view);
        if (view) {
            var sm = view.getSelectionModel();
            if (sm.hasSelection()) {
                var gridBaseParams = view.store.proxy.extraParams;
                var params = {
                    keyfields: this.keyFields,
                    command: gridBaseParams.command,
                    cacheDataSet: gridBaseParams.cacheDataSet,
                    oper: 'delete'
                };
                var record = sm.selected;
                if (record) {
                    Ext.each(this.keyFields, function(item) {
                        params[item] = record.items[0].get(item);
                    });
                    var baseForm = this.getForm();
                    Ext.Msg.confirm('', Infolight.GridHelper.msgSureDelete, function(btn) {
                        if (btn == 'yes') {
                            if (autoApply) {
                                params.module = gridBaseParams.module;
                                params.autoApply = autoApply;
                                Ext.Ajax.request({
                                    url: '../ExtJs/infolight/ExtGetRecord.ashx',
                                    method: 'POST',
                                    params: params,
                                    success: function(response, option) {
                                        if (response.responseText) {
                                            var result = Ext.decode(response.responseText);
                                            if (result.success === true) {
                                                view.removeCurrentRow();
                                            }
                                            else if (result.success === false) {
                                                Infolight.Exception.throwEx(response.responseText);
                                            }
                                        }
                                    }
                                });
                            }
                            else {
                                baseForm.submit({
                                    url: '../ExtJs/infolight/ExtGetRecord.ashx',
                                    params: params,
                                    success: function(form, action) {
                                        view.removeCurrentRow();
                                    },
                                    failure: function(form, action) {
                                        Infolight.Exception.throwEx(action.response.responseText);
                                    }
                                });
                            }
                        }
                    });
                }
            }
        }
    },
    submitRecord: function(autoApply) {
        var baseForm = this.getForm();
        if (baseForm.isValid() === false) {
            Ext.Msg.alert('', Infolight.FormHelper.msgValidFail, function() { });
            return;
        }
        Infolight.Validator.validFormViewFields(this.getServerValids(), baseForm, function() {
            if (this.delaySave()) {
                Ext.Msg.alert('', Infolight.FormHelper.msgSaveDetails);
            }
            else {
                var view = Ext.getCmp(this.view);
                if (view) {
                    var gridBaseParams = view.store.proxy.extraParams;
                    var baseForm = this.getForm();
                    baseForm.submit({
                        url: '../ExtJs/infolight/ExtGetRecord.ashx',
                        params: {
                            keyfields: this.keyFields,
                            fields: this.fields,
                            module: gridBaseParams.module,
                            command: gridBaseParams.command,
                            cacheDataSet: gridBaseParams.cacheDataSet,
                            autoApply: autoApply,
                            oper: this.editMode
                        },
                        success: function(form, action) {
                            this.fieldsEnable(false);
                            view.enable();
                            if (this.details != null && this.details.length > 0) {
                                Ext.each(this.details, function (dtl) {
                                    var detail = Ext.getCmp(dtl);
                                    detail.enable();
                                });
                            }
                            this.setNavState('browse');
                            var data = action.result.data;
                            if (data) {
                                if (this.editMode == 'add') {
                                    var keyvalues = {};
                                    Ext.each(view.store.model.getFields(), function(field) {
                                        keyvalues[field.name] = data[field.name];
                                    });
                                    var record = view.store.model.create(keyvalues);
                                    //var Row = Ext.data.Record.create(view.store.model);
                                    //var record = new Row(keyvalues);
                                    if (record) {
                                        var n = view.store.getCount();
                                        view.store.add(record);
                                        view.getView().refresh();
                                        view.getSelectionModel().select(n)
                                    }
                                    else
                                        view.getView().refresh();
                                }
                                else if (this.editMode == 'update') {
                                    var selection = view.getSelectionModel()
                                    if (selection && selection.hasSelection()) {
                                        var record = selection.selected;
                                        if (record) {
                                            Ext.each(view.store.model.getFields(), function (field) {
                                                if (data[field.name] != record.items[0].get(field.name)) {
                                                    record.items[0].set(field.name, data[field.name]);
                                                }
                                            });
                                            record.items[0].commit();
                                            view.getView().refresh();
                                        }
                                    }
                                }
                            }
                            this.editMode = 'readonly';
                        },
                        failure: function(form, action) {
                            Infolight.Exception.throwEx(action.response.responseText);
                        },
                        scope: this
                    });
                }
            }
        }, this);
    },
    getServerValids: function() {
        var srvValids = [];
        Ext.each(this.valids, function(valid) {
            if (valid.validConfig && valid.validConfig.srvValid) {
                srvValids.push(valid);
            }
        });
        return srvValids;
    },
    delaySave: function() {
        var delaySave = false;
        var gridDetails = [];
        if (this.details != null && this.details.length > 0) {
            Ext.each(this.details, function(dtl) {
                gridDetails.push(Ext.getCmp(dtl));
            });
        }
        Ext.each(gridDetails, function(detail) {
            var modRecords = detail.store.getModifiedRecords();
            if (modRecords.length > 0 || (detail.deletedItems && detail.deletedItems.length > 0)) {
                delaySave = true;
            }
        });
        return delaySave;
    },
    cancelRecord: function () {
        if (this.details.length > 0) {
            Ext.each(this.details, function (dtl) {
                var detail = Ext.getCmp(dtl);
                detail.enable();
            });
        }
        var view = Ext.getCmp(this.view);
        if (view) {
            view.enable();
            var sm = view.getSelectionModel();
            var locRecord;
            if (sm.hasSelection()) {
                var record = sm.selected;
                Ext.each(this.keyFields, function(key) {
                    locRecord = locRecord || {};
                    locRecord[key] = record.items[0].get(key)
                });
            }
            if (locRecord) {
                var gridBaseParams = view.store.proxy.extraParams;
                var baseForm = this.getForm();
                baseForm.load({
                    url: '../ExtJs/infolight/ExtGetRecord.ashx',
                    params:
                    {
                        keyvalues: Ext.encode(locRecord),
                        fields: this.fields,
                        command: gridBaseParams.command,
                        cacheDataSet: gridBaseParams.cacheDataSet
                    },
                    failure: function(form, action) {
                        Infolight.Exception.throwEx(action.response.responseText);
                    }
                });
                if (this.details.length > 0) {
                    var gridBaseParams = view.store.proxy.extraParams;
                    Ext.Ajax.request({
                        url: '../ExtJs/infolight/ExtGetRecord.ashx',
                        method: 'POST',
                        params: {
                            command: gridBaseParams.command,
                            cacheDataSet: gridBaseParams.cacheDataSet,
                            oper: 'abort'
                        },
                        success: function(response, option) {
                            Ext.each(this.details, function(dtl) {
                                var detail = Ext.getCmp(dtl);
                                if (detail) {
                                    detail.refresh({ masterKeys: Ext.encode(locRecord) });
                                }
                            });
                        },
                        scope: this
                    });
                }
                this.editMode = 'readonly';
                this.fieldsEnable(false);
                this.setNavState('browse');
            }
        }
    },
    /*master必须是autoapply,所以这里暂时不会用到*/
    saveRecords: function() {
        var view = Ext.getCmp(this.view);
        if (view) {
            var gridBaseParams = view.store.baseParams;
            Ext.Ajax.request({
                url: '../ExtJs/infolight/ExtGetRecord.ashx',
                method: 'POST',
                params: {
                    module: gridBaseParams.module,
                    command: gridBaseParams.command,
                    cacheDataSet: gridBaseParams.cacheDataSet,
                    oper: 'save'
                },
                success: function(response, option) {
                    view.enable();
                    view.store.load();
                    this.setNavState('browse');
                },
                scope: this
            });
        }
    },
    /*master必须是autoapply,所以这里暂时不会用到*/
    abortRecords: function() {
        var view = Ext.getCmp(this.view);
        if (view) {
            var gridBaseParams = view.store.baseParams;
            Ext.Ajax.request({
                url: '../ExtJs/infolight/ExtGetRecord.ashx',
                method: 'POST',
                params: {
                    command: gridBaseParams.command,
                    cacheDataSet: gridBaseParams.cacheDataSet,
                    oper: 'abort'
                },
                success: function(response, option) {
                    view.enable();
                    view.store.load();
                    this.setNavState('browse');
                },
                scope: this
            });
        }
    },
    fieldsEnable: function(enable) {
        var baseForm = this.getForm();
        Ext.each(this.fields, function(fieldName) {
            var field = baseForm.findField(fieldName);
            if (field) {
                if (enable) {
                    field.enable();
                }
                else {
                    field.disable();
                }
            }
        });
    }
});