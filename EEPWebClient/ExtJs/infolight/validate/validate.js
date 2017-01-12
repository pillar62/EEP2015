/// <reference path="../../adapter/ext/ext-base-debug.js"/>
/// <reference path="../../ext-all-debug.js"/>

(function() {
    var alpha = /^[a-zA-Z_]+$/;
    var alphanum = /^[a-zA-Z0-9_]+$/;
    var email = /^([\w]+)(\.[\w]+)*@([\w\-]+\.){1,5}([A-Za-z]){2,4}$/;
    var url = /(((https?)|(ftp)):\/\/([\-\w]+\.)+\w{2,3}(\/[%\-\w]+(\.\w{2,})?)*(([\w\-\.\?\\\/+@&#;`~=%!]*)(\.\w{2,})?)*\/?)/i;

    var positiveint = /^[0-9]\d*$/;
    var positivefloat = /^[+|-]?\d*\.?\d*$/;
    var ipaddress = /\d+\.\d+\.\d+\.\d+/;

    Ext.apply(Ext.form.VTypes, {
        isint: function(value) {
            return positiveint.test(value);
        },

        isfloat: function(value) {
            return positivefloat.test(value);
        },

        ip: function(value) {
            return ipaddress.test(value);
        }

    });
})();

Infolight.Validator = function() {
    return {
        validFormViewField: function(field, method, msg, record) {
            Ext.Ajax.request({
                url: '../ExtJs/infolight/ExtValid.ashx',
                method: 'POST',
                params: {
                    method: method,
                    value: record[field.name],
                    record: Ext.encode(record),
                    oper: 'cell'
                },
                success: function(response, option) {
                    if (response.responseText) {
                        var result = Ext.decode(response.responseText);
                        if (result.success === true) {
                            if (result.validSuccess !== true) {
                                Infolight.Validator.markInvalid(null, field, -1, msg);
                            }
                        }
                        else if (result.success === false) {
                            Infolight.Exception.throwEx(response.responseText);
                        }
                    }
                }
            });
        },
        validFormViewFields: function(srvValids, baseForm, validSuccessFunc, scope) {
            var sp = scope || this;
            if (srvValids.length > 0) {
                var record = baseForm.getValues();
                var validMethods = new Object();
                Ext.each(srvValids, function(valid) {
                    validMethods[valid.field] = valid.validConfig.srvValid;
                });
                Ext.Ajax.request({
                    url: '../ExtJs/infolight/ExtValid.ashx',
                    method: 'POST',
                    params: {
                        validMethods: Ext.encode(validMethods),
                        validRecord: Ext.encode(record),
                        oper: 'record'
                    },
                    success: function(response, option) {
                        if (response.responseText === '') {
                            if (validSuccessFunc) {
                                validSuccessFunc.apply(this);
                            }
                        }
                        else {
                            var result = Ext.decode(response.responseText);
                            if (result) {
                                if (result.success === true) {
                                    if (result.validSuccess !== true) {
                                        var index = Ext.each(srvValids, function(valid) {
                                            return valid.field !== result.field;
                                        });
                                        if (index !== -1) {
                                            var field = baseForm.findField(srvValids[index].field);
                                            Infolight.Validator.markInvalid(null, field, -1, srvValids[index].validConfig.msg);
                                        }
                                    }
                                }
                                else if (result.success === false) {
                                    Infolight.Exception.throwEx(response.responseText);
                                }
                            }
                        }
                    },
                    scope: sp
                });
            }
            else {
                validSuccessFunc.apply(sp);
            }
        },
        validGridViewRecord: function(grid, validSuccessFunc) {
            var srvValidColumns = Infolight.Validator.getSrvValidGridColumns(grid);
            if (srvValidColumns && srvValidColumns.length > 0) {
                var rowIndex = grid.getSelectionModel().last;
                var record = grid.store.getAt(rowIndex).data;
                Ext.each(srvValidColumns, function(validColumn) {
                    if (record[validColumn.dataIndex] === undefined) {
                        record[validColumn.dataIndex] = null;
                    }
                });
                var validMethods = Infolight.Validator.getSrvValidMethods(grid);
                Ext.Ajax.request({
                    url: '../ExtJs/infolight/ExtValid.ashx',
                    method: 'POST',
                    params: {
                        validMethods: Ext.encode(validMethods),
                        validRecord: Ext.encode(record),
                        oper: 'record'
                    },
                    success: function(response, option) {
                        if (response.responseText === '') {
                            if (validSuccessFunc) {
                                validSuccessFunc.apply(grid, [false]);
                            }
                        }
                        else {
                            var result = Ext.decode(response.responseText);
                            if (result) {
                                if (result.success === true) {
                                    if (result.validSuccess !== true) {
                                        Ext.each(srvValidColumns, function(col) {
                                            if (col.dataIndex === result.field) {
                                                var validMsg = Infolight.Validator.getSrvValidMsg(grid);
                                                Infolight.Validator.markInvalid(grid, col, rowIndex, validMsg[result.field]);
                                            }
                                        });
                                    }
                                }
                                else if (result.success === false) {
                                    Infolight.Exception.throwEx(response.responseText);
                                }
                            }
                        }
                    }
                });
            }
        },
        validGridViewRecords: function(grid, validSuccessFunc) {
            var modiRecords = grid.store.getModifiedRecords();
            if (modiRecords) {
                var srvValidColumns = Infolight.Validator.getSrvValidGridColumns(grid);
                if (srvValidColumns && srvValidColumns.length > 0) {
                    var validMethods = Infolight.Validator.getSrvValidMethods(grid);
                    var records = new Array();
                    Ext.each(modiRecords, function(record) {
                        Ext.each(srvValidColumns, function(validColumn) {
                            if (record.get(validColumn.dataIndex) === undefined) {
                                record.set(validColumn.dataIndex, null);
                            }
                        });
                        records.push(record.data);
                    });
                    Ext.Ajax.request({
                        url: '../ExtJs/infolight/ExtValid.ashx',
                        method: 'POST',
                        params: {
                            validMethods: Ext.encode(validMethods),
                            modiRecords: Ext.encode(records),
                            oper: 'all'
                        },
                        success: function(response, option) {
                            if (response.responseText === '') {
                                validSuccessFunc.apply(grid, [true]);
                            }
                            else {
                                var result = Ext.decode(response.responseText);
                                if (result) {
                                    if (result.success == true) {
                                        if (result.validSuccess !== true) {
                                            Ext.each(srvValidColumns, function(col) {
                                                if (col.dataIndex === result.field) {
                                                    var validMsg = Infolight.Validator.getSrvValidMsg(grid);
                                                    var rowIndex = grid.store.findBy(function(record) {
                                                        var isInvalidRecord = true;
                                                        for (var i = 0; i < grid.keys.length; i++) {
                                                            if (result.record[grid.keys[i]] !== record.get(grid.keys[i])) {
                                                                isInvalidRecord = false;
                                                            }
                                                        }
                                                        return isInvalidRecord;
                                                    });
                                                    Infolight.Validator.markInvalid(grid, col, rowIndex, validMsg[result.field]);
                                                }
                                            });
                                        }
                                    }
                                    else if (result.success == false) {
                                        Infolight.Exception.throwEx(response.responseText);
                                    }
                                }
                            }
                        }
                    });
                }
                else {
                    validSuccessFunc.apply(grid, [true]);
                }
            }
        },
        //private
        markInvalid: function(grid, field, validGridRowIndex, msg) {
            if (grid) {
                Ext.Msg.alert('', msg, function() {
                    if (grid.getSelectionModel().last !== validGridRowIndex) {
                        grid.getSelectionModel().selectRow(validGridRowIndex);
                    }
                    grid.startEditing(validGridRowIndex, grid.colModel.findColumnIndex(field.dataIndex));
                });
            }
            else {
                Ext.Msg.alert('', msg, function() {
                    if (field) {
                        field.focus(true);
                    }
                });
            }
        },
        getSrvValidGridColumns: function(validGrid) {
            return validGrid.headerCt.getGridColumns(function(col) {
                if (validGrid.valids) {
                    for (i = 0; i < validGrid.valids.length; i++) {
                        if (validGrid.valids[i].validConfig.srvValid && validGrid.valids[i].field == col.dataIndex) {
                            return true;
                        }
                    }
                }
                return false;
            });
        },
        getSrvValidMethods: function(validGrid) {
            var validMethods = new Object();
            Ext.each(validGrid.valids, function(valid) {
                if (valid.validConfig.srvValid) {
                    validMethods[valid.field] = valid.validConfig.srvValid;
                }
            });
            return validMethods;
        },
        getSrvValidMsg: function(validGrid) {
            var validMsgs = new Object();
            Ext.each(validGrid.valids, function(valid) {
                if (valid.validConfig.srvValid) {
                    validMsgs[valid.field] = valid.validConfig.msg;
                }
            });
            return validMsgs;
        }
    };
} ();