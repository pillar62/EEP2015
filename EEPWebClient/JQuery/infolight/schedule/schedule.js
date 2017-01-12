function renderSchedule(){
    if (this.container) {
        var container = $('#' + this.container);
        var formPanConfig = {
            labelAlign: 'left',
            frame: true,
            layout: 'table',
            width: 500,
            height: 300,
            autoScroll: true,
            layoutConfig: {
                columns: 2
            },
            items: []
        };
        
        formPanConfig.items.push({
            xtype: 'panel',
            layout: 'form',
            labelWidth: 70,
            colspan: 2,
            items: [{
                xtype: 'textfield',
                fieldLabel: this.titleCaption,
                name: this.titleField,
                allowBlank: false,
                width: 400
            }]
        });
        
        if (this.descriptionField) {
            formPanConfig.items.push({
                xtype: 'panel',
                layout: 'form',
                labelWidth: 70,
                colspan: 2,
                items: [{
                    xtype: 'textarea',
                    fieldLabel: this.descriptionCaption,
                    name: this.descriptionField,
                    width: 400
                }]
            });
        }
        
        if (this.allDayField) {
            formPanConfig.items.push({
                xtype: 'panel',
                layout: 'form',
                labelWidth: 70,
                colspan: 2,
                items: [{
                    xtype: 'checkbox',
                    fieldLabel: this.allDayCaption,
                    name: this.allDayField,
                    width: 160,
                    listeners: {
                        check: function(chk, checked){
                            var stafield = formPan.findById('dateStartField');
                            var endfield = formPan.findById('dateEndField');
                            var staDate = Date.parseDate(stafield.value, stafield.format);
                            var endDate = Date.parseDate(endfield.value, endfield.format);
                            if (staDate && endDate) {
                                if (checked) {
                                    stafield.altFormats = 'Y-m-d';
                                    stafield.format = 'Y-m-d';
                                    stafield.setValue(staDate.clearTime());
                                    
                                    endfield.altFormats = 'Y-m-d';
                                    endfield.format = 'Y-m-d';
                                    endfield.setValue(endDate.clearTime());
                                }
                                else {
                                    stafield.altFormats = 'Y-m-d  H:i';
                                    stafield.format = 'Y-m-d H:i';
                                    stafield.setValue(staDate);
                                    
                                    endfield.altFormats = 'Y-m-d H:i';
                                    endfield.format = 'Y-m-d H:i';
                                    endfield.setValue(endDate);
                                }
                            }
                        }
                    }
                }]
            });
        }
        
        formPanConfig.items.push({
            xtype: 'panel',
            layout: 'form',
            labelWidth: 70,
            items: [{
                id: 'dateStartField',
                xtype: 'datefield',
                fieldLabel: this.startCaption,
                name: this.startField,
                allowBlank: false,
                altFormats: 'Y-m-d H:i',
                format: 'Y-m-d H:i',
                width: 160
            }]
        });
        
        formPanConfig.items.push({
            xtype: 'panel',
            layout: 'form',
            labelWidth: 70,
            items: [{
                id: 'dateEndField',
                xtype: 'datefield',
                fieldLabel: this.endCaption,
                name: this.endField,
                allowBlank: false,
                altFormats: 'Y-m-d H:i',
                format: 'Y-m-d H:i',
                width: 160
            }]
        });
        
        var deleteButton = new Ext.Button({
            text: this.deleteCaption,
            scope: this,
            handler: function(){
                Ext.Msg.confirm('', 'are you sure to delete this event?', function(btn, text){
                    if (btn == 'yes') {
                        var recordValues = new Object();
                        recordValues[this.idField] = formPan.originValues.id;
                        Ext.Ajax.request({
                            url: '../JQuery/infolight/AjaxSchedule.ashx',
                            method: 'POST',
                            params: {
                                module: this.module,
                                cacheDataSet: this.cacheDataSet,
                                command: this.command,
                                idField: this.idField,
                                recordValues: Ext.encode(recordValues),
                                oper: 'delete'
                            },
                            success: function(response, option){
                                container.fullCalendar('removeEvents', formPan.originValues.id);
                                win.hide();
                            }
                        });
                    }
                }, this);
            }
        });
        
        var submitButton = new Ext.Button({
            text: this.submitCaption,
            scope: this,
            handler: function(){
                var recordValues = formPan.getForm().getValues();
                if (recordValues) {
                    if (this.allDayField) {
                        if (recordValues[this.allDayField] && recordValues[this.allDayField] == 'on') {
                            recordValues[this.allDayField] = 1;
                        }
                        else {
                            recordValues[this.allDayField] = 0;
                        }
                    }
                    if (!recordValues[this.titleField]) {
                        Ext.Msg.show({
                            title: 'warning',
                            msg: 'title is required.'
                        });
                        return;
                    }
                    if (recordValues[this.startField] && recordValues[this.endField]) {
                        var sd = Date.parseDate(recordValues[this.startField], formPan.findById('dateStartField').format);
                        var ed = Date.parseDate(recordValues[this.endField], formPan.findById('dateEndField').format);
                        if (sd > ed) {
                            Ext.Msg.show({
                                title: 'warning',
                                msg: 'start date must be earlier than end date.'
                            });
                            return;
                        }
                    }
                    else {
                        Ext.Msg.show({
                            title: 'warning',
                            msg: 'start date and end date is required.'
                        });
                        return;
                    }
                    if (oper == 'update') {
                        recordValues[this.idField] = formPan.originValues.id;
                    }
                    Ext.Ajax.request({
                        url: '../JQuery/infolight/AjaxSchedule.ashx',
                        method: 'POST',
                        params: {
                            module: this.module,
                            cacheDataSet: this.cacheDataSet,
                            command: this.command,
                            idField: this.idField,
                            titleField: this.titleField,
                            descriptionField: this.descriptionField,
                            startField: this.startField,
                            endField: this.endField,
                            allDayField: this.allDayField,
                            recordValues: Ext.encode(recordValues),
                            oper: oper
                        },
                        success: function(response, option){
                            if (response.responseText) {
                                var record = Ext.decode(response.responseText);
                                if (record) {
                                    if (option.params.oper == 'insert') {
                                        container.fullCalendar('renderEvent', record);
                                    }
                                    else 
                                        if (option.params.oper == 'update') {
                                            var event = formPan.originValues;
                                            $.extend(event, record);
                                            container.fullCalendar('updateEvent', event);
                                        }
                                }
                            }
                            win.hide();
                        }
                    });
                }
            }
        });
        
        var cancelButton = new Ext.Button({
            text: this.cancelCaption,
            handler: function(){
                win.hide();
            }
        });
        
        var formPan = new Ext.FormPanel(formPanConfig);
        var oper = 'insert';
        var winConfig = {
            closable: false,
            resizable: false,
            modal: true,
            shadow: true,
            width: 500,
            shadowOffset: 8,
            title: this.winTitle,
            layout: 'fit',
            closeAction: 'hide',
            items: [formPan],
            buttons: [deleteButton, submitButton, cancelButton]
        };
        winConfig.height = 140;
        if (this.allDayField) {
            winConfig.height += 30;
        }
        if (this.descriptionField) {
            winConfig.height += 60;
        }
        var win = new Ext.Window(winConfig);

        this.scheduleConfig.disableResizing = true;
        this.scheduleConfig.theme = true;
        var getJsonParams = {
            cacheDataSet: this.cacheDataSet,
            command: this.command,
            idField: this.idField,
            titleField: this.titleField,
            descriptionField: this.descriptionField,
            startField: this.startField,
            endField: this.endField,
            allDayField: this.allDayField
        };
        this.scheduleConfig.events = function(start, end, callback){
            getJsonParams.start = start.format('Y-m-d');
            getJsonParams.end = end.format('Y-m-d');
            Ext.Ajax.request({
                url: '../JQuery/infolight/AjaxSchedule.ashx',
                method: 'POST',
                params: getJsonParams,
                success: function(response, option){
                    if (response.responseText) {
                        var records = Ext.decode(response.responseText);
                        callback(records);
                    }
                }
            });
        };
        
        this.scheduleConfig.dayDoubleClick = function(dayDate, allDay, jsEvent, view){
            win.show(jsEvent.target);
            oper = 'insert';
            deleteButton.disable();
            formPan.getForm().reset();
            var initValues = new Object();
            initValues[getJsonParams.startField] = dayDate;
            initValues[getJsonParams.endField] = dayDate;
            formPan.getForm().setValues(initValues);
        };
        this.scheduleConfig.eventDoubleClick = function(calEvent, jsEvent, view){
            win.show(jsEvent.target);
            oper = 'update';
            deleteButton.enable();
            formPan.getForm().reset();
            var initValues = new Object();
            initValues[getJsonParams.idField] = calEvent.id;
            initValues[getJsonParams.titleField] = calEvent.title;
            if (getJsonParams.descriptionField) {
                initValues[getJsonParams.descriptionField] = calEvent.description;
            }
            if (getJsonParams.allDayField) {
                initValues[getJsonParams.allDayField] = calEvent.allDay;
            }
            initValues[getJsonParams.startField] = calEvent.start;
            if (calEvent.end) {
                initValues[getJsonParams.endField] = calEvent.end;
            }
            else {
                initValues[getJsonParams.endField] = calEvent.start;
            }
            formPan.getForm().setValues(initValues);
            formPan.originValues = calEvent;
        };
        
        var scheduleParams = this;
        var isAllDay = false;
        this.scheduleConfig.eventDragStart = function(calEvent, jsEvent, ui, view){
            isAllDay = calEvent.allDay;
        }
        this.scheduleConfig.eventDrop = function(calEvent, dayDelta, minuteDelta, allDay, revertFunc, jsEvent, ui, view){
            if (isAllDay === !allDay) {
                revertFunc();
                var events = ['all day event', 'normal event'];
                if (!isAllDay) {
                    events = events.reverse();
                }
                var msg = String.format('can not drag from {0} to {1}.', events[0], events[1]);
                Ext.Msg.show({
                    title: 'warning',
                    msg: msg
                });
                return;
            }
            var recordValues = new Object();
            recordValues[scheduleParams.idField] = calEvent.id;
            recordValues[scheduleParams.startField] = calEvent.start;
            recordValues[scheduleParams.endField] = calEvent.end;
            Ext.Ajax.request({
                url: '../JQuery/infolight/AjaxSchedule.ashx',
                method: 'POST',
                params: {
                    module: scheduleParams.module,
                    cacheDataSet: scheduleParams.cacheDataSet,
                    command: scheduleParams.command,
                    idField: scheduleParams.idField,
                    startField: scheduleParams.startField,
                    endField: scheduleParams.endField,
                    recordValues: Ext.encode(recordValues),
                    oper: 'drag'
                }
            });
        };
        
        var view = container.fullCalendar(this.scheduleConfig);
    }
}
