Ext.Loader.setConfig({
    enabled: true
});
Ext.Loader.setPath('Ext.ux', '../ux');
Ext.require([
    'Ext.form.*',
    'Ext.grid.*',
    'Ext.data.*',
    'Ext.util.*',
    'Ext.state.*',
    'Ext.toolbar.Paging',
    'Ext.ux.RowExpander',
    'Ext.selection.CheckboxModel',
    'Ext.ux.PreviewPlugin',
    'Ext.ModelManager',
    'Ext.tip.QuickTipManager'

]);

// Define Company entity
// Null out built in convert functions for performance *because the raw data is known to be valid*
// Specifying defaultValue as undefined will also save code. *As long as there will always be values in the data, or the app tolerates undefined field values*
Ext.define('Customers', {
    extend: 'Ext.data.Model',
    fields: [
                      { name: "CustomerID" },
                      { name: 'CompanyName' },
                      { name: 'ContactName' },
                      { name: 'ContactTitle'},
                      { name: 'Address' },
                      { name: 'City' },
                      { name: 'Region'},
                      { name: 'PostalCode' },
                      { name: 'Country'},
                      { name: 'Phone' },
                      { name: 'Fax' },
       ],
    idProperty: 'Customers'
});

Ext.onReady(function () {
    Ext.tip.QuickTipManager.init();
    Ext.QuickTips.init();
    
    // setup the state provider, all state information will be saved to a cookie
    Ext.state.Manager.setProvider(Ext.create('Ext.state.CookieProvider'));

/**
     * Custom function used for column renderer
     * @param {Object} val
     */
    function change(val) {
        if (val > 0) {
            return '<span style="color:green;">' + val + '</span>';
        } else if (val < 0) {
            return '<span style="color:red;">' + val + '</span>';
        }
        return val;
    }

    /**
     * Custom function used for column renderer
     * @param {Object} val
     */
    function pctChange(val) {
        if (val > 0) {
            return '<span style="color:green;">' + val + '%</span>';
        } else if (val < 0) {
            return '<span style="color:red;">' + val + '%</span>';
        }
        return val;
    }

    var rowEditing = Ext.create('Ext.grid.plugin.RowEditing', {
        clicksToMoveEditor: 1,
        autoCancel: true
    });

    var store = Ext.create('Ext.data.Store', {
        // destroy the store if the grid is destroyed
        //autoDestroy: true,
        pageSize: 100,
        //autoLoad: true,
        remoteSort: true,
        model: 'Customers',
        proxy: {
            type: 'ajax',
            url: '../../../ExtJs/infolight/ExtGetData2.ashx',
            reader: {
                totalProperty: 'total',
                type: 'json',
                root: 'data'
            },
            extraParams: {
                start: 0,
                limit: 100,
                oper: 'select',
                tableName: 'Customers',
                order: "CustomerID",
                oper: 'select',
                fields: 'CustomerID,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax',
                module: 'S001',
                command: 'Customers',
                sevmod: false,
                alwaysClose: false,
            }
        },
        sorters: [{
            property: 'CustomerID',
            direction: 'asc'
        }]
        
    });
    store = Ext.apply(store, {
        keys: ['CustomerID'],
    });
    // create the Grid
    var grid = Ext.create('Ext.grid.Panel', {
        store: store,
        //store: new Ext.data.Store({ proxy: new Ext.data.HttpProxy({ url: 'T1.ashx' }), reader: new Ext.data.JsonReader({ totalProperty: 'total', root: 'data', fields: [{ name: 'CustomerID' }, { name: 'CompanyName' }, { name: 'ContactName' }, { name: 'ContactTitle' }, { name: 'Address' }, { name: 'City' }, { name: 'Region' }, { name: 'PostalCode' }, { name: 'Country' }, { name: 'Phone' }, { name: 'Fax' }] }) }),
        stateful: true,
        collapsible: true,
        multiSelect: true,
        stateId: 'stateGrid',
        columns:
                  [
                      Ext.create('Ext.grid.RowNumberer'),
                      { id: 'colCustomerID', header: 'CustomerID', dataIndex: 'CustomerID', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colCompanyName', header: 'CompanyName', dataIndex: 'CompanyName', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colContactName', header: 'ContactName', dataIndex: 'ContactName', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colContactTitle', header: 'ContactTitle', dataIndex: 'ContactTitle', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colAddress', header: 'Address', dataIndex: 'Address', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colCity', header: 'City', dataIndex: 'City', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colRegion', header: 'Region', dataIndex: 'Region', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colPostalCode', header: 'PostalCode', dataIndex: 'PostalCode', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colCountry', header: 'Country', dataIndex: 'Country', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colPhone', header: 'Phone', dataIndex: 'Phone', editor: new Ext.form.TextField({}), width: 75 },
                      { id: 'colFax', header: 'Fax', dataIndex: 'Fax', editor: new Ext.form.TextField({}), width: 75 }
                  ],
        bbar: Ext.create('Ext.PagingToolbar', {
            store: store,
            displayInfo: true,
            pageSize: 100
        }),
        plugins: [{
            ptype: 'rowexpander',
            rowBodyTpl: [
                '<p><b>CustomerID:</b> {CustomerID}</p>',
                '<p><b>CompanyName:</b> {CompanyName}</p>',
                '<p><b>ContactName:</b> {ContactName}</p>',
                '<p><b>Address:</b> {Address}</p>'
            ]
            
        },
        rowEditing
        ],
        listeners: {
            'selectionchange': function (view, records) {
                grid.down('#remove').setDisabled(!records.length);
            }},
        loadMask: true,
        columnLines: true,
        height: 350,
        width: 600,
        title: 'Array Grid',
        renderTo: 'grid-example',
        viewConfig: {
            stripeRows: true,
            enableTextSelection: true
        },
        tbar: [{
            text: 'Add',
            iconCls: 'add',
            handler: function () {
                rowEditing.cancelEdit();

                // Create a model instance
                var r = Ext.create('Customers', {
                    CustomerID: '',
                });

                store.insert(0, r);
                rowEditing.startEdit(0, 0);
            }
        }, {
            itemId: 'remove',
            text: 'Remove',
            iconCls: 'remove',
            handler: function () {
                var sm = grid.getSelectionModel();
                rowEditing.cancelEdit();
                store.deletedItems = store.deletedItems || [];
                Ext.each(sm.getSelection(),function(item){
                    var editType = {
                        editType: 'delete'
                    };
                    Ext.each(store.keys, function (key) {
                        editType[key] = item.data[key];
                    });
                    store.deletedItems.push(editType);
                });
                store.remove(sm.getSelection());
                if (store.getCount() > 0) {
                    sm.select(0);
                }
            },
            disabled: true
        }, {
            itemId: 'save',
            text: 'Save',
            iconCls: 'save',
            handler: function () {
                var extraParams = store.proxy.extraParams;
                alert(extraParams);
                var editTypeArray = [];
                var jsonArray = [];
                Ext.each(store.getModifiedRecords(), function (item) {
                    if (item.data) {
                        var editType = {
                            editType: 'edit'
                        };
                        Ext.each(store.keys, function (key) {
                            editType[key] = item.data[key];
                        });
                        editTypeArray.push(editType);
                    }
                    else {
                        editTypeArray.push({
                            editType: 'insert'
                        });
                    }
                    jsonArray.push(item.getChanges());
                }, this);
                store.deletedItems = store.deletedItems || [];
                Ext.each(store.deletedItems, function (item) {
                    editTypeArray.push(item);
                });
                alert("a" + editTypeArray.length);
                if (editTypeArray.length > 0) {
                    Ext.Ajax.request({
                        url: store.proxy.url,
                        method: "POST",
                        params: {
                            oper: 'save',
                            editTypes: Ext.encode(editTypeArray),
                            changes: Ext.encode(jsonArray),
                            tablename: "Customers",
                            keys:store.keys
                        },
                        success: function (response, option) {
                            if (response.responseText) {
                                var result = Ext.decode(response.responseText);
                                if (result.success === false) {
                                    store.rejectChanges();
                                    alter(response.responseText);
                                }
                            }
                            else {
                                if (reload) {
                                    store.load();
                                }
                                store.commitChanges();
                            }
                        },
                        callback: function () {
                            store.deletedItems = [];
                            alert("1");
                        },
                        scope: this
                    });
                }
            }
        }]
    });
    //store.load();
    store.loadPage(1);
});
