/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>
Infolight.Layout = function() {
    return {
        _createMasterPanConfig: function(master) {
            return {
                layout: master.layout,
                defaults:{
                    bodyStyle: 'padding:5px 5px 0px 0px',
                    border: false
                },
                title: master.title,
                items: master.items
            };
        },
        _createDetails: function(gridDetails) {
            var panDetailsConfig = {
                region: 'south',
                height: gridDetails[0].height,
                split: true,
                items: gridDetails
            };
            if (gridDetails.length === 1) {
                Ext.apply(panDetailsConfig, {
                    layout: 'fit',
                    title: ''
                });
                return new Ext.Panel(panDetailsConfig);
            }
            else if (gridDetails.length > 1) {
                Ext.apply(panDetailsConfig, {
                    activeTab: 0,
                    defaults: { autoScroll: true },
                    deferredRender: false
                });
                return new Ext.TabPanel(panDetailsConfig);
            }
            return null;
        },
        initLayout: function(config) {
            var items = [];
            var layoutId = config.layout.id;
            Ext.apply(config.view, { isView: true });
            var gridView = Infolight.GridHelper.createGrid(config.view);
            var panView = new Ext.Panel({
                id: layoutId + 'ViewPan',
                title: config.viewTitle,
                region: 'west',
                layout: 'fit',
                split: true,
                width: gridView.width,
                collapsible: true,
                collapsed: config.layout.panelcollapsed,
                items: [gridView]
            });
            items.push(panView);
            var panData, gridDetails = new Array();
            if (config.details) {
                Ext.each(config.details, function(detail) {
                    var detailGrid = Infolight.GridHelper.createGrid(detail.grid);
                    detailGrid.masterForm = layoutId;
                    if (detail.title) {
                        detailGrid.title = detail.title;
                    }
                    gridDetails.push(detailGrid);
                });
            }
            if (config.masters) {
                var panDataConfig = {
                    id: layoutId + 'DataPan',
                    region: 'center',
                    tbar: config.masterTools,
                    frame: true,
                    items: []
                };
                if (config.masters.length == 1) {
                    var master = config.masters[0];
                    var panMasterConfig = Infolight.Layout._createMasterPanConfig(master);
                    Ext.apply(panMasterConfig, { autoScroll: true });
                    if (config.details) {
                        Ext.apply(panMasterConfig, { region: 'center' });
                        var panMaster = new Ext.Panel(panMasterConfig);
                        panDataConfig.layout = 'border';
                        panDataConfig.items.push(panMaster, Infolight.Layout._createDetails(gridDetails));
                    }
                    else {
                        // 10
                        panDataConfig.layout = 'table';
                        Ext.apply(panDataConfig, panMasterConfig);
                    }
                }
                else if (config.masters.length > 1) {
                    var masterTabConfig = {
                        activeTab: 0,
                        defaults: { autoScroll: true },
                        deferredRender: false,
                        items: []
                    };
                    Ext.each(config.masters, function(master) {
                        var panMasterConfig = Infolight.Layout._createMasterPanConfig(master);
                        Ext.apply(panMasterConfig, { frame: true });
                        masterTabConfig.items.push(panMasterConfig);
                    });
                    if (config.details) {
                        masterTabConfig.region = 'center';
                        var masterTab = new Ext.TabPanel(masterTabConfig);
                        panDataConfig.layout = 'border';
                        panDataConfig.items.push(masterTab, Infolight.Layout._createDetails(gridDetails));
                    }
                    else {
                        // N0
                        var masterTab = new Ext.TabPanel(masterTabConfig);
                        Ext.apply(panDataConfig, {
                            layout: 'fit',
                            autoScroll: true,
                            items: masterTab
                        });
                    }
                }
                panData = new Ext.Panel(panDataConfig);
                items.push(panData);
            }
            config.layout.items = items;
            Ext.apply(config.layout, {
                view: gridView.id,
                details: Ext.pluck(gridDetails, 'id'),
                keyFields: config.masterKeyFields,
                fields: config.masterFields,
                valids: config.masterValids,
                focusEventHandlers: config.masterFocusEventHandlers,
                leaveEventHandlers: config.masterLeaveEventHandlers
            });
            var formPanel = new Infolight.Form(config.layout);
            formPanel.initValids();
            formPanel.addHandlers();
            if (panData) {
                formPanel.setNavState('browse');
            }
            var masterFields = config.masterFields;
            var masterKeyFields = config.masterKeyFields;
            if (gridView) {
                var sm = gridView.getSelectionModel();
                sm.on('select', function(selectionModel,record, rowIndex,obj ) {
                    var locRecord;
                    Ext.each(masterKeyFields, function(key) {
                        locRecord = locRecord || {};
                        locRecord[key] = record.get(key);
                    });
                    if (locRecord) {
                        formPanel.setNavState('loading');
                        formPanel.load(locRecord, gridView.store.proxy.extraParams);
                        if (gridDetails.length > 0) {
                            Ext.each(gridDetails, function (gridDetail) {
                                if (gridDetail.store.proxy.extraParams.masterCommand == gridView.store.proxy.extraParams.command) {
                                    //只有要傳鍵連key的Grid的主表就是目前這個grid才要往下執行
                                    gridDetail.refresh({ masterKeys: Ext.encode(locRecord),where:''  });
                                    //傳過去鍵連key並更新主表就是目前這個grid的其它grid的資料
                                }
                                else {
                                    gridDetail.store.removeAll();
                                }
                            });
                        }
                    }
                });
                sm.on('selectionchange', function() {
                    if (this.selected.length == 0 && gridView.store.totalCount == 0) {
                        formPanel.getForm().reset();
                        Ext.each(gridDetails, function(gridDetail) {
                            gridDetail.store.removeAll();
                        });
                    }
                });
                if (gridDetails.length > 0) {
                    Ext.each(gridDetails, function(gridDetail) {
                        gridDetail.setNavState('browse');                        
                    });
                }
            }
        }
    };
} ();