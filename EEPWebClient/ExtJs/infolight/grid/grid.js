/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

Ext.define("Infolight.Grid", {
    extend: "Ext.grid.Panel",
    //类似Ext.reg的功能  
    alias: "widget.Infolight.Grid",

    constructor: function (config) {
        this.callParent(arguments);//Calling the parent class constructor
        this.initConfig(config);//Initializing the component
        this.on('edit', function (editor,o,eOpts) {
            if (Ext.isArray(this.leaveEventHandlers) && this.leaveEventHandlers.length > 0) {
                Ext.each(this.leaveEventHandlers, function (handler) {
                    if (handler.field === o.field) {
                        handler.handler(o);
                    }
                });
            }
        });
        this.on('keypress', function (e) {
            var key = e.getKey();
            if (key == e.ENTER) {
                var selectionModel = this.getSelectionModel();
                if (selectionModel) {
                    this.startEditing(selectionModel.last, 1);
                }
            }
        });
        var sm = this.getSelectionModel();
        if (sm) {
            var valiParam = { selectIndex: -1 };
            this.getSelectionModel().on('beforerowselect', this.rowSelecting, valiParam);
            this.getSelectionModel().on('rowselect', this.rowSelect, valiParam);
        }
        if (config.bbar) {
            var pageBar = this.dockedItems.get(config.bbar.id);
            if (pageBar) {
                ///这里不知为何在变更页码以及pagebar上的刷新时进行了save动作，暂时先注释掉了
                //pageBar.on('beforechange', this.save, this);
                pageBar.on('change', function (tool, pageData) {
                    if (this.store.proxy.extraParams && this.store.proxy.extraParams.isDetails == 'false' && pageData) {
                        sm.select(0);
                    }
                }, this, { delay: 200 });
            }
        }
        this.setNavState('browse');
    },
    rowSelecting: function (selectionModel, rowIndex, keepExisting) {
        if (this.selectIndex != -1) {
            var grid = selectionModel.grid;
            var store = grid.store;
            if (store) {
                var validRecord = store.getAt(this.selectIndex);
                grid.stopEditing();
                if (validRecord && validRecord.modified && grid.valids) {
                    var expander = grid.colModel.getColumnById('expander');
                    var startColIndex = 0;
                    if (expander) {
                        startColIndex += 1;
                    }
                    for (var i = 0; i < grid.valids.length; i++) {
                        var field = grid.valids[i].field;
                        var value = validRecord.get(field);
                        var validConfig = grid.valids[i].validConfig;
                        if (validConfig.allowBlank == false) {
                            if (value === undefined || value === '') {
                                grid.startEditing(this.selectIndex, store.fields.keys.indexOf(field) + startColIndex);
                                return false;
                            }
                        }
                        if (validConfig.vtype == 'alpha') {
                            if (!Ext.form.VTypes.alpha(value)) {
                                grid.startEditing(this.selectIndex, store.fields.keys.indexOf(field) + startColIndex);
                                return false;
                            }
                        }
                        else if (validConfig.vtype == 'alphanum') {
                            if (!Ext.form.VTypes.alphanum(value)) {
                                grid.startEditing(this.selectIndex, store.fields.keys.indexOf(field) + startColIndex);
                                return false;
                            }
                        }
                        else if (validConfig.vtype == 'email') {
                            if (!Ext.form.VTypes.email(value)) {
                                grid.startEditing(this.selectIndex, store.fields.keys.indexOf(field) + startColIndex);
                                return false;
                            }
                        }
                        else if (validConfig.vtype == 'url') {
                            if (!Ext.form.VTypes.url(value)) {
                                grid.startEditing(this.selectIndex, store.fields.keys.indexOf(field) + startColIndex);
                                return false;
                            }
                        }
                        else if (validConfig.vtype == 'isint') {
                            if (!Ext.form.VTypes.isint(value)) {
                                grid.startEditing(this.selectIndex, store.fields.keys.indexOf(field) + startColIndex);
                                return false;
                            }
                        }
                        else if (validConfig.vtype == 'isfloat') {
                            if (!Ext.form.VTypes.isfloat(value)) {
                                grid.startEditing(this.selectIndex, store.fields.keys.indexOf(field) + startColIndex);
                                return false;
                            }
                        }
                        else if (validConfig.vtype == 'ip') {
                            if (!Ext.form.VTypes.ip(value)) {
                                grid.startEditing(this.selectIndex, store.fields.keys.indexOf(field) + startColIndex);
                                return false;
                            }
                        }
                    }
                    if (grid.autoApply) {
                        Infolight.Validator.validGridViewRecord(grid, this.saveRecordsWithoutValid);
                    }
                    else {
                        Infolight.Validator.validGridViewRecord(grid);
                    }
                }
            }
        }
    },
    rowSelect: function (selectionModel, rowIndex, record) {
        this.selectIndex = rowIndex;
    },
    setNavState: function (state) {    
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
                    setExtToolItemEnable(this, this.id + 'btnQuery', true);
                    setExtToolItemEnable(this, this.id + 'btnRefresh', true);
                    setExtToolItemEnable(this, this.id + 'btnSave', false);
                    setExtToolItemEnable(this, this.id + 'btnAbort', false);
                    break;
                case 'editing':
                    setExtToolItemEnable(this, this.id + 'btnAdd', false);
                    setExtToolItemEnable(this, this.id + 'btnEdit', false);
                    setExtToolItemEnable(this, this.id + 'btnDelete', false);
                    setExtToolItemEnable(this, this.id + 'btnQuery', false);
                    setExtToolItemEnable(this, this.id + 'btnRefresh', false);
                    setExtToolItemEnable(this, this.id + 'btnSave', true);
                    setExtToolItemEnable(this, this.id + 'btnAbort', true);
                    break;
            }
        }
    },
    addRow: function (defaultValues, mColumns, dColumns) {
        // 为Detail新增设置默认key value
        if (this.store.proxy.extraParams.masterKeys) {
            var masterkeys = Ext.decode(this.store.proxy.extraParams.masterKeys);
            if (masterkeys) {
                Ext.iterate(masterkeys, function (key, value, obj) {
                    var index = -1;
                    Ext.each(mColumns,function(v,i,a){
                        if(v == key)
                            index = i;
                    });
                    //var index = mColumns.indexOf(key);
                    if (index != -1) {
                        var s = dColumns[index];
                        defaultValues[s] = value;
                    }
                });
            }
        }
        var srvDefMethods = null;
        Ext.iterate(defaultValues, function (key, value, obj) {
            var model = this.store.proxy.getModel();
            var fields = model.getFields();
            Ext.each(fields, function (field) {
                if (field.name == key) {
                    defField = field;
                }
            });

            if (defField) {
                if (typeof value === 'string') {
                    if (Infolight.Default.isSysDefaultValue(value)) {
                        if (srvDefMethods === null) {
                            srvDefMethods = new Object();
                        }
                        srvDefMethods[key] = value;
                    }
                    else if (value.indexOf('@srvMethod:') === 0) {
                        if (srvDefMethods === null) {
                            srvDefMethods = new Object();
                        }
                        srvDefMethods[key] = value.split(':')[1];
                    }
                    else if (defField.type.type == 'date') {
                        if (Infolight.convertDate(value)) {
                            value = Infolight.convertDate(value);
                        }
                    }
                }
            }
        }, this);
        if (srvDefMethods !== null) {
            var grid = this;
            Infolight.Default.setDefaultValues(srvDefMethods, function (defValues) {
                Ext.iterate(defValues, function (key, value, obj) {
                    var model = this.store.proxy.getModel();
                    var fields = model.getFields();
                    Ext.each(fields, function (field) {
                        if (field.name == key) {
                            defField = field;
                        }
                    });

                    if (defField.type.type == 'int') {
                        defaultValues[key] = parseInt(value, 10);
                    }
                    else if (defField.type.type == 'float') {
                        defaultValues[key] = parseFloat(value);
                    }
                    else if (defField.type.type == 'date') {
                        defaultValues[key] = Infolight.convertDate(value);
                    }
                    else {
                        defaultValues[key] = value;
                    }
                }, grid);
                grid.setDefaultValues(defaultValues);
            });
        }
        else {
            this.setDefaultValues(defaultValues);
        }
        this.setNavState('editing');
    },
    addModal: function (updatePanId) {
        __doPostBack(updatePanId, Ext.encode({ mode: 'insert' }));
    },
    setDefaultValues: function (defaultValues) {
        var record = Ext.create(this.id + "Model", defaultValues);
        if (record) {
            var n = this.store.getCount();
            this.editingPlugin.cancelEdit();
            this.store.add(record);
            record.state = true;
            this.editingPlugin.startEdit(n, 1);
            //var sm = this.getSelectionModel();
            //if (sm.selectionMode == 'SINGLE') {
            //    sm.selectRow(n);
            //}
            var propCount = 0;
            Ext.iterate(defaultValues, function (key, value, obj) {
                propCount++;
            });
            if (propCount > 0) {
                record.dirty = true;
                record.modified = defaultValues;
                //if (this.store.modified.indexOf(record) == -1) {
                //    this.store.modified.push(record);
                //}
            }
        }
    },
    editRow: function () {
        var sm = this.getSelectionModel();
        if (sm.selectionMode=='SINGLE') {
            if (sm.getSelection().length == 1) {
                this.editingPlugin.startEdit(sm.getSelection()[0].index, 1);
            }
            else {
                Ext.MessageBox.alert('', Infolight.GridHelper.msgNonSelToEdit);
            }
        }
    },
    editModal: function (updatePanId) {
        var sm = this.getSelectionModel();
        if (sm.selectionMode == 'SINGLE') {
            if (sm.getSelection().length == 1) {
                var args = {
                    mode: 'edit'
                };
                var record = sm.selected;
                Ext.each(this.keys, function (key) {
                    args[key] = record.items[0].get(key)
                });
                __doPostBack(updatePanId, Ext.encode(args));
            }
            else {
                Ext.MessageBox.alert('', Infolight.GridHelper.msgNonSelToEdit);
            }
        }
    },
    deleteRow: function () {
        var grid = this;
        var sm = this.getSelectionModel();
        if (sm.hasSelection()) {
            Ext.Msg.confirm('', Infolight.GridHelper.msgSureDelete, function (btn) {
                if (btn == 'yes') {
                    grid.deletedItems = grid.deletedItems || [];
                    Ext.each(sm.getSelection(), function (item) {
                        var editType = {
                            editType: 'delete'
                        };
                        Ext.each(this.store.keys, function (key) {
                            editType[key] = item.data[key];
                        });                        
                        grid.deletedItems.push(editType);
                        ///换成each的外层后这个不能用了
                        //this.removeCurrentRow();
                        
                    });
                    this.store.remove(sm.getSelection());
                    if (this.autoApply) {
                        this.saveRecordsWithoutValid(false);
                    }
                    else {
                        this.setNavState('editing');
                    }
                }
            }, this);
        }
        else {
            Ext.MessageBox.alert('', Infolight.GridHelper.msgNonSelToDelete);
        }
    },
    removeCurrentRow: function (item) {
        var sm = this.getSelectionModel();
        if (sm.hasSelection()) {
            var record = sm.selected.items[0];
            //if (sm.hasPrevious()) {
                sm.selectPrevious();
            //}
            this.store.remove(record);
            if (sm.hasSelection() === false) {
                sm.select(0);
            }
        }
    },
    locateLastRecord: function () {
        this.getSelectionModel().selectRow(this.store.getCount() - 1);
    },
    save: function () {
        var sm = this.getSelectionModel();
        var cm = this.headerCt;
        var record = sm.selected;
        if (record.getCount()>0) {
            var colIndex = Ext.each(cm.gridDataColumns, function (col) {
                if (col.dataIndex) {
                    var editor = col.editor;
                    if (editor) {
                        Ext.each(record.items,function(item){
                            var value = item.get(col.dataIndex);
                            if (editor.allowBlank === false) {
                                if (value === '') {
                                    return false;
                                }
                            }
                            if (col.editor.inputType == 'alpha') {
                                if (!Ext.form.VTypes.alpha(value)) {
                                    return false;
                                }
                            }
                            else if (col.editor.inputType == 'alphanum') {
                                if (!Ext.form.VTypes.alphanum(value)) {
                                    return false;
                                }
                            }
                            else if (col.editor.inputType == 'email') {
                                if (!Ext.form.VTypes.email(value)) {
                                    return false;
                                }
                            }
                            else if (col.editor.inputType == 'url') {
                                if (!Ext.form.VTypes.url(value)) {
                                    return false;
                                }
                            }
                            else if (col.editor.inputType == 'isint') {
                                if (!Ext.form.VTypes.isint(value)) {
                                    return false;
                                }
                            }
                            else if (col.editor.inputType == 'isfloat') {
                                if (!Ext.form.VTypes.isfloat(value)) {
                                    return false;
                                }
                            }
                            else if (col.editor.inputType == 'ip') {
                                if (!Ext.form.VTypes.ip(value)) {
                                    return false;
                                }
                            }
                        })
                    }
                }
            });
            //if (colIndex) {
            //    this.startEditing(sm.last, colIndex);
            //    return;
            //}
        }
        Infolight.Validator.validGridViewRecords(this, this.saveRecordsWithoutValid);
    },
    saveRecordsWithoutValid: function (reload) {
        var gridBaseParams = this.store.proxy.extraParams;
        var editTypeArray = [];
        var jsonArray = [];
        Ext.each(this.store.getModifiedRecords(), function (item) {
            if (item.state && item.state==true) {
                editTypeArray.push({
                    editType: 'insert'
                });
            }
            else {
                var editType = {
                    editType: 'edit'
                };
                Ext.each(this.store.keys, function (key) {
                    editType[key] = item.data[key];
                });
                editTypeArray.push(editType);
            }
            jsonArray.push(item.getChanges());
        }, this);
        this.deletedItems = this.deletedItems || [];
        Ext.each(this.deletedItems, function (item) {
            editTypeArray.push(item);
        });
        if (editTypeArray.length > 0) {
            var delaySave = false;
            if (this.masterForm) {
                var formPan = Ext.getCmp(this.masterForm);
                if (formPan.editMode !== 'readonly') {
                    delaySave = true;
                }
            }
            Ext.Ajax.request({
                url: this.store.proxy.url,
                method: "POST",
                params: {
                    oper: 'save',
                    module: gridBaseParams.module,
                    command: gridBaseParams.command,
                    masterCommand: gridBaseParams.masterCommand,
                    sevmod: gridBaseParams.sevmod,
                    editTypes: Ext.encode(editTypeArray),
                    changes: Ext.encode(jsonArray),
                    cacheDataSet: gridBaseParams.cacheDataSet,
                    delaySave: delaySave
                },
                success: function (response, option) {
                    if (response.responseText) {
                        var result = Ext.decode(response.responseText);
                        if (result.success === false) {
                            this.store.rejectChanges();
                            Infolight.Exception.throwEx(response.responseText);
                        }
                    }
                    else {
                        if (reload) {
                            this.store.load();
                        }
                        this.store.commitChanges();
                    }
                },
                callback: function () {
                    this.deletedItems = [];
                },
                scope: this
            });
        }
        this.setNavState('browse');
    },
    abort: function () {
        this.editingPlugin.cancelEdit();
        this.store.rejectChanges();
        //var items = this.store.data.items;
        //for (var i = items.length - 1; i >= 0; i--) {
        //    if (!items[i].json) {
        //        this.store.remove(items[i]);
        //    }
        //}
        this.setNavState('browse');
    },
    // queries(Ext.util.MixedCollection)--->
    // item:{dataField(key):[{condition:'xxx',operator:'xxx',caption:'xxx',editor:'xxx',defVal:xxx,editorConfig(combo):{...}},{...(one field could have several)}](value)}
    // queryConfig:{columnsCount:xxx, panWidth:xxx, panHeight:xxx, uiCaptions:['xxx(title)', 'xxx(submit)', 'xxx(cancel)']}}
    addFormViewModal:function(formViewID,defaultvalues){
        this.genEditFormViewWindow(formViewID);
        var pan = this.EditFormWindow.items.items[0];
        if (pan && pan.xtype == 'form') {
            {
                pan.addRecord(defaultvalues);
                this.EditFormWindow.show();

            }
        }
    },
    editFormViewModal: function (formViewID) {
        this.genEditFormViewWindow(formViewID);
        var pan = this.EditFormWindow.items.items[0];
        if (pan && pan.xtype == 'form') {
            {
                pan.editMode = 'update';
                var baseForm = pan.getForm();
                baseForm.reset();
                pan.fieldsEnable(true);

                var rocord =  this.getSelectionModel().selected.items[0];
                var locRecord;
                Ext.each(this.keys, function (key) {
                    locRecord = locRecord || {};
                    locRecord[key] = rocord.get(key);
                });
                if (locRecord) {
                    pan.load(locRecord, this.store.proxy.extraParams);
                }
                this.EditFormWindow.show();

            }
        }
        
    },
    genEditFormViewWindow: function (formViewID) {
        if (!Ext.isDefined(this.EditFormWindow)) {
            var formPan = new Infolight.Form(this.editFormViewConfig[0]);
            Ext.apply(formPan, {
                view: this.id,
                defaultValues:this.editFormViewDefaultValues,
                keyFields: this.editFormViewKeyFields,
                fields: this.editFormViewFields,
                valids: this.editFormViewValids,
                focusEventHandlers: this.editFormViewFocusEventHandlers,
                leaveEventHandlers: this.editFormViewLeaveEventHandlers
            });
            formPan.initValids();
            formPan.addHandlers();
            var winConfig = {
                closable: true,
                resizable: true,
                modal: true,
                shadow: true,
                width: this.editFormViewWidth,
                height: this.editFormViewHeght,
                //title: this.eidtFormViewID, // title
                layout: 'fit',
                closeAction: 'hide',
                items: [formPan],
                buttons:
                [
                    new Ext.Button({
                        text: 'submit',
                        handler: function () {
                            var pan = this.EditFormWindow.items.items[0];
                            pan.submitRecord(true);
                            this.enable();
                            this.EditFormWindow.hide();
                        },
                        scope: this
                    }),
                    new Ext.Button({
                        text: 'cancel',
                        handler: function () {
                            this.enable();
                            this.EditFormWindow.hide();
                        },
                        scope: this
                    })
                ]
            };
            this.EditFormWindow = new Ext.Window(winConfig);
        }
    },
    gridQuery: function (queries, queryConfig) {
        this.genQueryWindow(queries, queryConfig);
        var pan = this.queryWindow.items.items[0];
        if (pan && pan.getXType() == 'form') {
            var baseForm = pan.getForm();
            baseForm.reset();
            this.queryWindow.show();
            /*var defValues = [
            { id: 'query_ExtGrid_Name1', field:'Name', sysParam: '_USERNAME' },
            { id: 'query_ExtGrid_Age1', field: 'Age', value: '20' },
            { id: 'query_ExtGrid_Age2', field: 'Age', value: '50' },
            { id: 'query_ExtGrid_Birth2', field: 'Birth', value: '1980-1-1' },
            { id: 'query_ExtGrid_Address1', field: 'Address', value: 'xxx' }
            ];*/
            var defValues = [];
            queries.each(function (query, index, length) {
                var field = queries.keys[index];
                var i = 1;
                Ext.each(query, function (q) {
                    var defObj = {
                        id: 'query_' + this.id + '_' + field + i,
                        field: field
                    }
                    if (Infolight.Default.isSysDefaultValue(q.defVal)) {
                        defObj.sysParam = q.defVal;
                    }
                    else {
                        defObj.value = q.defVal;
                    }
                    defValues.push(defObj);
                    i++;
                }, this);
            }, this);
            var setQueryDefValMethod = function () {
                var counter = 0;
                Ext.each(defValues, function (defVal) {
                    var editor = Ext.getCmp(defVal.id);
                    if (editor) {
                        var query = queries.get(defVal.field);
                        var editorType = '';
                        Ext.each(query, function (q) {
                            if (q.editorConfig && q.editorConfig.id === defVal.id) {
                                editorType = q.editor;
                            }
                        });
                        if (editorType === 'ComboBox' && defVal.value) {
                            var keyvalues = {};
                            keyvalues[defVal.field] = defVal.value;
                            counter++;
                            Ext.Ajax.request({
                                url: '../ExtJs/infolight/ExtGetRecord.ashx',
                                method: 'POST',
                                params: {
                                    keyvalues: Ext.encode(keyvalues),
                                    module: editor.store.proxy.extraParams.module,
                                    command: editor.store.proxy.extraParam.command,
                                    alias: editor.store.proxy.extraParam.alias,
                                    sql: editor.store.proxy.extraParam.sql,
                                    cacheDataSet: editor.store.proxy.extraParam.cacheDataSet,
                                    fields: editor.store.fields.keys.join(',')
                                },
                                success: function (response, option) {
                                    var result = Ext.decode(response.responseText);
                                    if (result.success === true) {
                                        var Row = Ext.data.Record.create(editor.store.fields);
                                        editor.store.add(new Row(result.data));
                                    }
                                    else if (result.success === false) {
                                        Infolight.Exception.throwEx(response.responseText);
                                    }
                                },
                                callback: function () {
                                    if (--counter === 0) {
                                        baseForm.setValues(defValues);
                                    }
                                }
                            });
                        }
                        else if (editor.getXType() === 'datefield') {
                            defVal.value = Infolight.convertDate(defVal.value);
                        }
                    }
                });
                if (counter === 0) {
                    baseForm.setValues(defValues);
                }
            };
            var defMethods;
            Ext.each(defValues, function (defVal) {
                if (defVal.sysParam) {
                    if (!defMethods) {
                        defMethods = new Object();
                    }
                    defMethods[defVal.id] = defVal.sysParam;
                }
            });
            if (defMethods) {
                Ext.Ajax.request({
                    url: '../ExtJs/infolight/ExtDefault.ashx',
                    method: 'POST',
                    params: {
                        methods: Ext.encode(defMethods)
                    },
                    success: function (response, option) {
                        if (response.responseText) {
                            var result = Ext.decode(response.responseText);
                            if (result.success === true) {
                                Ext.each(defValues, function (defVal) {
                                    if (result.defObj[defVal.id]) {
                                        defVal.value = result.defObj[defVal.id];
                                    }
                                });
                                setQueryDefValMethod();
                            }
                            else if (result.success === false) {
                                Infolight.Exception.throwEx(response.responseText);
                            }
                        }
                    }
                });
            }
            else {
                setQueryDefValMethod();
            }
        }
    },
    genQueryWindow: function (queries, queryConfig) {
        if (!Ext.isDefined(this.queryWindow)) {
            var formPanConfig = {
                labelAlign: 'left',
                frame: true,
                layout: {
                    type: 'vbox',
                    align: 'stretch',
                    pack: 'start',
                },
                autoScroll: true,
                layoutConfig: {
                    columns: queryConfig.columnsCount
                },
                items: []
            };
            queries.each(function (query, index, length) {
                var field = queries.keys[index];
                var i = 1;
                Ext.each(query, function (q) {
                    var editorPan = {
                        frame: true,
                        xtype: 'panel',
                        layout: {
                            type: 'vbox',
                            align: 'stretch',
                            pack: 'start',
                        },
                        labelWidth: 90,
                        //width: 224,
                        items: []
                    };
                    var fieldId = 'query_' + this.id + '_' + field + i;
                    switch (q.editor) {
                        case 'CheckBox':
                            editorPan.items.push({
                                id: fieldId,
                                xtype: 'combo',
                                name: field,
                                fieldLabel: q.caption,
                                store: ['true', 'false'],
                                typeAhead: false,
                                triggerAction: 'all',
                                width: 127
                            });
                            break;
                        case 'DateTimePicker':
                            editorPan.items.push({
                                id: fieldId,
                                xtype: 'datefield',
                                name: field,
                                format: 'Y/m/d',
                                fieldLabel: q.caption,
                                width: 127
                            });
                            break;
                        case 'TextBox':
                            editorPan.items.push({
                                id: fieldId,
                                xtype: 'textfield',
                                name: field,
                                fieldLabel: q.caption,
                                width: 227
                            });
                            break;
                        case 'ComboBox':
                            Ext.apply(q.editorConfig, {
                                id: fieldId,
                                name: field,
                                fieldLabel: q.caption,
                                width: 127
                            });
                            Ext.apply(q.editor, { embededIn: 'query' });
                            var comb = new Infolight.ComboBox(q.editorConfig);
                            if (comb) {
                                editorPan.items.push(comb);
                            }
                            break;
                    };
                    formPanConfig.items.push(editorPan);
                    i++;
                }, this);
            }, this);
            var formPan = new Ext.form.FormPanel(formPanConfig);
            var winConfig = {
                closable: true,
                resizable: true,
                modal: true,
                shadow: true,
                width: queryConfig.panWidth,
                height: queryConfig.panHeight,
                title: queryConfig.uiCaptions[0], // title
                layout: 'fit',
                closeAction: 'hide',
                items: [formPan],
                buttons:
                [
                    new Ext.Button({
                        text: queryConfig.uiCaptions[1], // submit
                        handler: function () {
                            var queryValues = formPan.getForm().getValues();
                            if (queryValues) {
                                var options = this.store.proxy.extraParams;
                                options.where = null;
                                var filterConditions = new Array();
                                queries.each(function (query, index, length) {
                                    var fieldValues = queryValues[queries.keys[index]];
                                    if (fieldValues instanceof Array) {
                                        var i = 0;
                                        Ext.each(query, function (q) {
                                            if (fieldValues[i] && fieldValues[i] !== 'undefined') {
                                                filterConditions.push({
                                                    condition: q.condition,
                                                    field: queries.keys[index],
                                                    operator: q.operator,
                                                    defVal: fieldValues[i]
                                                });
                                            }
                                            i++;
                                        });
                                    }
                                    else {
                                        if (fieldValues && fieldValues !== 'undefined') {
                                            filterConditions.push({
                                                condition: query[0].condition,
                                                field: queries.keys[index],
                                                operator: query[0].operator,
                                                defVal: fieldValues
                                            });
                                        }
                                    }
                                });
                                if (filterConditions.length > 0) {
                                    options.filterConditions = Ext.encode(filterConditions);
                                }
                                else {
                                    delete options.filterConditions;
                                }
                                options.alwaysClose = false;
                                options.start = 0;
                                this.store.load();
                                this.queryWindow.hide();
                            }
                        },
                        scope: this
                    }),
                    new Ext.Button({
                        text: queryConfig.uiCaptions[2], // cancel
                        handler: function () {
                            this.queryWindow.hide();
                        },
                        scope: this
                    })
                ]
            };
            this.queryWindow = new Ext.Window(winConfig);
        }
    },
    setWhere: function (where) {
        this.refresh({ where: where });
    },
    refresh: function (params) {
        this.store.proxy.extraParams = Ext.apply(this.store.proxy.extraParams, params);
        this.store.load();
    },
    gridSavePersonal: function (userid, remark) {
        var formPathParts = window.location.pathname.split('/');
        if (formPathParts.length >= 2) {
            var formPath = formPathParts[formPathParts.length - 2] + '/' + formPathParts[formPathParts.length - 1];
            var props = [];
            var cm = this.headerCt;
            Ext.each(cm.gridDataColumns, function (col) {
                var hidden = (col.hidden === true);
                props.push({ column: col.id, width: col.width, hidden: hidden });
            });
            Ext.Ajax.request({
                url: "../ExtJs/infolight/ExtPersonalSettings.ashx",
                method: "POST",
                params: {
                    oper: 'save',
                    formName: formPath,
                    compName: this.id,
                    userid: userid,
                    remark: remark,
                    propContent: Ext.encode(props)
                },
                success: function (response, option) {
                    if (response.responseText) {
                        var result = Ext.decode(response.responseText);
                        if (result.success === true) {
                            Ext.Msg.alert('', Infolight.GridHelper.msgPersonalSaved);
                        }
                        else if (result.success === false) {
                            Infolight.Exception.throwEx(response.responseText);
                        }
                    }
                }
            });
        }
    },
    gridLoadPersonal: function (userid) {
        var formPathParts = window.location.pathname.split('/');
        if (formPathParts.length >= 2) {
            var formPath = formPathParts[formPathParts.length - 2] + '/' + formPathParts[formPathParts.length - 1];
            Ext.Ajax.request({
                url: "../ExtJs/infolight/ExtPersonalSettings.ashx",
                method: "POST",
                params: {
                    oper: 'load',
                    formName: formPath,
                    compName: this.id,
                    userid: userid
                },
                success: function (response, option) {
                    if (response.responseText) {
                        var result = Ext.decode(response.responseText);
                        if (result.success === true) {
                            var content = result.content;
                            if (content) {
                                var cm = this.headerCt;
                                Ext.each(content, function (item, index) {
                                    var oldcolumn = cm.items.getByKey(item.column);
                                    //if (oldcolumn.id == item.column) {
                                    var oldIndex = cm.items.indexOf(oldcolumn);
                                        if (oldIndex != -1 && oldIndex != index) {
                                            cm.move(oldIndex, index);
                                        }
                                        oldcolumn.width = item.width;
                                        oldcolumn.hidden = item.hidden;
                                    //}
                                });
                                this.getView().refresh(true);
                                Ext.Msg.alert('', Infolight.GridHelper.msgPersonalLoaded);
                            }
                        }
                        else if (result.success === false) {
                            Infolight.Exception.throwEx(response.responseText);
                        }
                    }
                },
                scope: this
            });
        }
    },
    ExtRefvalChanged:function(newvalue,oldvalue,columnName)
    {
        if (newvalue != oldvalue) {
            var sm = this.getSelectionModel();
            if (sm.hasSelection()) {
                var record = sm.selected.items[0];
                //record.beginEdit();
                //record.data[columnName] = newvalue;
                //record.endEdit();
                record.set(columnName, newvalue);
            }
        }
    }
});