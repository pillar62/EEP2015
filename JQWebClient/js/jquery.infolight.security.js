$.fn.security = function (methodName, value) {
    if (typeof methodName == "string") {
        var method = $.fn.security.methods[methodName];
        if (method) {
            return method(this, value);
        }
    }
    else if (typeof methodName == "object") {
        this.each(function () {
            if (!$(this).hasClass($.fn.security.foo)) {
                $(this).addClass($.fn.security.foo)
            }
            $(this).security('initialize', methodName);
        });
    }
};

$.fn.security.foo = "info-security";

$.fn.security.methods =
{
    initialize: function (jq, options) {
        jq.each(function () {
            var securityOptions = new Object();
            if (options != undefined) {                                     //load option
                for (var property in options) {
                    securityOptions[property] = options[property];
                }
            }
            $(this).data('options', securityOptions);
            $(this).security('createLayout');
            $(this).security('load');
        });
    },
    options: function (jq) {
        if (jq.length > 0) {
            if ($(jq[0]).data('options') == undefined) {
                return new Object();
            }
            return $(jq[0]).data('options');
        }
        return new Object();
    },
    createLayout: function (jq) {
        jq.each(function () {
            var security = $(this);
            $('<table class="table-grid"/>').height($(this).height()).appendTo(security);

            var editMenu = $("<div/>").appendTo(security).menu({
                hideOnUnhover: false,
                onClick: function () {
                    var detailGrid = $(this).data('datagrid');
                    security.security('editDetail', detailGrid);
                }
            });
            editMenu.menu('appendItem', {
                text: 'Edit',
                iconCls: 'icon-edit'
            });
            security.data('editMenu', editMenu);
        });
    },
    loadData: function (jq, options) {
        jq.each(function () {
            var security = $(this);
            var columns = [];
            columns.push(options.columns);
            var datagrid = security.find('.table-grid');
            var gridOptions = {
                url: '../handler/SystemHandler_Security.ashx?type=Security',
                singleSelect: true,
                onBeforeLoad: function (param) {
                    param.type = security.security('options').type
                },
                onClickRow: function (index) {
                    var editIndex = datagrid.data('editIndex');
                    if (editIndex != undefined && editIndex != index) {
                        if (datagrid.datagrid('endEditing')) {
                            security.security('save');
                        } else {
                            datagrid.datagrid('selectRow', editIndex);
                        }
                    }
                },
                toolbar: [{
                    iconCls: 'icon-add',
                    handler: function () {
                        if (datagrid.datagrid('endEditing')) {
                            var newObj = {};
                            datagrid.datagrid('appendRow', newObj);
                            var editIndex = datagrid.datagrid('getRows').length - 1;
                            datagrid.datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
                            datagrid.data('editIndex', editIndex);
                        }
                    }
                }, {
                    iconCls: 'icon-edit',
                    handler: function () {
                        var selectedRow = datagrid.datagrid('getSelected');
                        if (selectedRow) {
                            var index = datagrid.datagrid('getRowIndex', selectedRow);
                            var editIndex = datagrid.data('editIndex');
                            if (editIndex != index) {
                                datagrid.datagrid('beginEdit', index);
                                datagrid.data('editIndex', index);
                            }
                        }
                    }
                }, {
                    iconCls: 'icon-remove',
                    handler: function () {
                        var selectedRow = datagrid.datagrid('getSelected');
                        if (selectedRow) {
                            var idField = datagrid.datagrid('getColumnFields')[0];
                            var id = selectedRow[idField];
                            $.messager.confirm('Confirm', 'Delete ' + security.security('options').type + ' : ' + id + ' ?', function (r) {
                                if (r) {
                                    var index = datagrid.datagrid('getRowIndex', selectedRow);
                                    datagrid.datagrid('deleteRow', index);
                                    security.security('save');
                                }
                            });
                        }
                    }
                }, '-', {
                    iconCls: 'icon-save',
                    handler: function () {
                        security.security('save');
                    }
                }, {
                    iconCls: 'icon-cancel',
                    handler: function () {
                        datagrid.datagrid('rejectChanges');
                    }
                }, {
                    iconCls: 'icon-search',
                    handler: function () {
                        if ($('#queryDialog').length == 0) {
                            var columns = datagrid.datagrid('getColumnFields');
                            var queryHTML = '<div id="queryDialog" style="padding:5px"><table id="winQuery">';
                            for (var i = 0; i < 2; i++) {
                                queryHTML += '<tr><td>' + columns[i] + ':</td><td><input id="vv' + i + '" name="' + columns[i] + '" class="easyui-validatebox" /></td></tr>';
                            }
                            queryHTML += '<tr><td></td><td><a id="btn" href="#" class="easyui-linkbutton">Query</a></td></tr>';
                            queryHTML += '</table></div>';
                            var queryDialog = $(queryHTML).appendTo(document.body);
                            queryDialog.dialog({
                                title: 'Query',
                                width: 275,
                                height: 130,
                                closed: false,
                                cache: false,
                                modal: true
                            });
                            $('#btn').linkbutton({
                                iconCls: 'icon-search'
                            })
                            .bind('click', function () {
                                var table = $('#winQuery');
                                var queryItems = [];
                                $('input', table).each(function () {
                                    if ($(this).attr('name') != undefined && $(this).val() != "") {
                                        queryItems.push({ field: $(this).attr('name'), condition: 'like', value: $(this).val() + '%' });
                                    }
                                });
                                datagrid.datagrid('options').queryParams.queryItems = JSON.stringify(queryItems);
                                datagrid.datagrid('load');
                                queryDialog.dialog('close');
                            });
                        }
                        $('#queryDialog').dialog('open');
                    }
                }],
                columns: columns,
                pagination: true
            };
            if (security.security('options').type == 'solution') {
                gridOptions.toolbar.push({
                    iconCls: 'icon-save',
                    handler: function () {
                        security.security('save');
                    }
                });
            }
            if (options.detailColumns) {
                gridOptions.view = securityview;
                gridOptions.detailFormatter = function (index, row) {
                    return '<div style="padding:2px"><table class="table-detailGrid" ></table></div>';
                };
                gridOptions.onExpandRow = function (index, row) {
                    var detailColumns = [];
                    detailColumns.push(options.detailColumns);
                    var detailGrid = $(this).datagrid('getRowDetail', index).find('table.table-detailGrid');
                    if (options.detailEditColumns) {
                        detailGrid.data('detailEditColumns', options.detailEditColumns);
                    }
                    var detailGridOptions = {
                        url: '../handler/SystemHandler_Security.ashx?type=Security',
                        fitColumns: true,
                        onBeforeLoad: function (param) {
                            param.type = security.security('options').type + 'detail';
                            param.id = row[options.columns[0].field];
                        },
                        loadMsg: '',
                        height: 'auto',
                        columns: detailColumns,
                        onResize: function () {
                            datagrid.datagrid('fixDetailRowHeight', index);
                        },
                        onLoadSuccess: function () {
                            setTimeout(function () {
                                datagrid.datagrid('fixDetailRowHeight', index);
                            }, 0);
                        },
                        onHeaderContextMenu: function (e, field) {
                            //                            if (security.security('options').type != 'account') {
                            //                                security.data('editMenu').data('datagrid', detailGrid)
                            //                                .menu('show', {
                            //                                    left: e.pageX,
                            //                                    top: e.pageY
                            //                                });
                            //                                e.preventDefault();
                            //                            }
                        },
                        onRowContextMenu: function (e, rowIndex, rowData) {
                            //                            if (security.security('options').type != 'account') {
                            //                                security.data('editMenu').data('datagrid', detailGrid)
                            //                                .menu('show', {
                            //                                    left: e.pageX,
                            //                                    top: e.pageY
                            //                                });
                            //                                e.preventDefault();
                            //                            }
                        }

                    };
                    if (security.security('options').type != 'account') {
                        detailGridOptions.toolbar = [{
                            iconCls: 'icon-edit',
                            handler: function () {
                                security.security('editDetail', detailGrid);
                            }
                        }];
                    }
                    detailGrid.data('detailIndex', index).datagrid(detailGridOptions);
                    datagrid.datagrid('fixDetailRowHeight', index);
                };
            }
            //            if (security.security('options').type == 'menu') {

            //            }
            //            else {
            datagrid.datagrid(gridOptions);
            //            }
        });
    },
    load: function (jq) {
        jq.each(function () {
            var security = $(this);
            $.ajax({
                type: 'get',
                dataType: 'json',
                url: '../js/json/jquery.infolight.security.' + security.security('options').type + '.columns.json',
                async: true,
                cache: false,
                success: function (columns) {
                    for (var i = 0; i < columns.length; i++) {
                        for (var j = 0; j < columns[i].length; j++) {
                            var column = columns[i][j];
                            if (column.editor) {
                                if (column.editor.type == 'text') {
                                    column.editor.type = 'validatebox';
                                }
                                if (column.editor.options) {
                                    if (!column.editor.options.validType) {
                                        column.editor.options.validType = 'crossSiteScript';
                                    }
                                }
                                else {
                                    column.editor.options = { validType: 'crossSiteScript' };
                                }
                            }
                            if (column.editor && column.editor.type == 'datetimebox') {
                                column.formatter = function (value, row, index) {
                                    if (value) {
                                        var date = new Date(value);
                                        return date.toFormatString();
                                    }
                                    else {
                                        return '';
                                    }
                                };
                            }
                            else if (column.editor && column.editor.type == 'password') {
                                column.formatter = function (value, row, index) {
                                    if (value) {
                                        return '(password)';
                                    }
                                    else {
                                        return '';
                                    }
                                };
                            }
                            else if (column.editor && column.editor.type == 'checkbox') {
                                column.formatter = function (value, row, index) {
                                    if (value && value.toString() == 'Y') {
                                        return '<input type="checkbox" checked="true" style="margin:0px" disabled="disabled"/>';
                                    }
                                    else {
                                        return '<input type="checkbox" style="margin:0px" disabled="disabled"/>';
                                    }
                                };
                            }
                            else if (column.editor && column.editor.type == 'combobox') {
                                column.formatter = function (value, row, index) {
                                    var text = value;
                                    if (this.editor.options.data) {
                                        for (var i = 0; i < this.editor.options.data.length; i++) {
                                            if (this.editor.options.data[i].value == value) {
                                                text = this.editor.options.data[i].text;
                                            }
                                        }
                                    }
                                    return text;
                                };
                            }
                        }
                    }
                    var options = new Object();
                    options.columns = columns[0];
                    if (columns.length > 1) {
                        options.detailColumns = columns[1];
                        if (columns.length > 2) {
                            options.detailEditColumns = columns[2];
                        }
                    }
                    security.security('loadData', options);
                }
            });
        });
    },
    save: function (jq) {
        jq.each(function () {
            var security = $(this);
            var options = security.security('options');
            var datagrid = security.find('.table-grid');
            if (datagrid.datagrid('endEditing')) {
                var rows = new Object();
                rows.deleted = datagrid.datagrid('getChanges', 'deleted');
                rows.updated = datagrid.datagrid('getChanges', 'updated');
                rows.inserted = datagrid.datagrid('getChanges', 'inserted');
                $.messager.progress({ msg: 'saving data...' });
                datagrid.datagrid('acceptChanges');

                var htmlEncode = function (key, value) {
                    if (value && typeof value == 'string') {
                        return value.replace(/</g, "&lt;").replace(/>/g, "&gt;");
                    }
                    else {
                        return value;
                    }
                };

                $.ajax({
                    type: 'post',
                    dataType: 'json',
                    url: '../handler/SystemHandler_Security.ashx?type=Security',
                    data: { mode: 'Update', type: options.type, rows: JSON.stringify(rows, htmlEncode) },
                    async: true,
                    cache: false,
                    success: function () {
                        if (options.onSaveSuccess) {
                            options.onSaveSuccess.call(security);
                        }
                        datagrid.datagrid('reload');
                    },
                    error: function () {
                        datagrid.datagrid("rejectChanges");
                    },
                    complete: function () {
                        $.messager.progress('close');
                    }
                });
            }
        });
    },
    editDetail: function (jq, detailGrid) {
        jq.each(function () {
            var security = $(this);
            var type = security.security('options').type;
            var detailEditColumns = [];
            detailEditColumns.push(detailGrid.data('detailEditColumns'));
            var detailEditType = '';
            if (type == 'group') {
                detailEditType = 'user';
            }
            else if (type == 'user') {
                detailEditType = 'group';
            }

            var detailWindow = $('<div/>').appendTo('body')
            .dialog({
                title: 'Select ' + detailEditType,
                width: 280,
                height: 350,
                closed: true,
                modal: true,
                onClose: function () {
                    $(this).remove();
                }
            });

            detailWindow.layout({ fit: true });
            detailWindow.layout('add', {
                region: 'south',
                height: 40,
                split: false,
                collapsible: false
            });
            detailWindow.layout('add', {
                region: 'center'
            });

            var commandbox = detailWindow.layout('panel', 'center');
            $('<table class="table-detailEditGrid"/>').appendTo(commandbox).datagrid({
                url: '../handler/SystemHandler_Security.ashx?type=Security',
                onBeforeLoad: function (param) {
                    param.type = detailEditType
                },
                onLoadSuccess: function () {
                    var detailRows = detailGrid.datagrid('getRows');
                    var idField = detailGrid.datagrid('getColumnFields')[0];
                    for (var i = 0; i < detailRows.length; i++) {
                        var id = detailRows[i][idField];
                        var detailEditRows = $(this).datagrid('getRows');
                        for (var j = 0; j < detailEditRows.length; j++) {
                            if (detailEditRows[j][idField] == id) {
                                $(this).datagrid('checkRow', j);
                                break;
                            }
                        }
                    }
                },
                columns: detailEditColumns
            }).datagrid('enableFilter', []);
            var buttonbox = detailWindow.layout('panel', 'south');
            $('<a style="margin:5px 5px 5px 80px"/>').appendTo(buttonbox).linkbutton({ text: 'OK', iconCls: 'icon-ok' })
            .click(function () {
                var datagrid = security.find('.table-grid');
                var detailIndex = detailGrid.data('detailIndex');
                var idField = datagrid.datagrid('getColumnFields')[0];
                var id = datagrid.datagrid('getRows')[detailIndex][idField];
                var detailEditGrid = detailWindow.find('.table-detailEditGrid');
                var rows = new Object();
                rows.deleted = [];
                rows.updated = []
                rows.inserted = detailEditGrid.datagrid('getChecked');
                $.messager.progress({ msg: 'saving data...' });
                $.ajax({
                    type: 'post',
                    dataType: 'json',
                    url: '../handler/SystemHandler_Security.ashx?type=Security',
                    data: { mode: 'UpdateDetail', type: security.security('options').type + 'detail', id: id, rows: JSON.stringify(rows) },
                    async: true,
                    cache: false,
                    success: function () {
                        detailGrid.datagrid('reload');
                        detailWindow.dialog('close');
                    },
                    complete: function () {
                        detailGrid.datagrid('reload');
                        detailWindow.dialog('close');
                        $.messager.progress('close');
                    }
                });
            });
            $('<a style="margin:5px"/>').appendTo(buttonbox).linkbutton({ text: 'Cancel', iconCls: 'icon-cancel' })
            .click(function () {
                detailWindow.dialog('close');
            });
            detailWindow.dialog('open');
        });
    }
}

$.extend($.fn.propertygrid.defaults.editors, {
    password: {
        init: function (container, options) {
            var input = $('<input type="password" class="datagrid-editable-input"/>').appendTo(container);
            return input;
        },
        getValue: function (target) {
            return $(target).val();
        },
        setValue: function (target, value) {
            $(target).val(value);
            //if (value) {
            //    if ($.browser.msie) {

            //    }
            //    else {
            //        target[0].setSelectionRange(0, value.length);
            //    }
            //}
        },
        resize: function (target, width) {
            $(target).outerWidth(width);
        }
    }
});

var securityview = $.extend({}, $.fn.datagrid.defaults.view, {
    addExpandColumn: function (target, index) {
        var opts = $.data(target, 'datagrid').options;
        if (index >= 0) {
            _add(index);
        } else {
            var length = $(target).datagrid('getRows').length;
            for (var i = 0; i < length; i++) {
                _add(i);
            }

            var commandButtons = getInfolightOption($(target)).commandButtons;
            var commandCount = 0;
            if (commandButtons == undefined || commandButtons.indexOf('u') != -1) {
                commandCount++;
            }
            if (commandButtons == undefined || commandButtons.indexOf('d') != -1) {
                commandCount++;
            }

            opts.finder.getTr(target, 0, 'allfooter', 1).each(function () {
                var totalCaption = getInfolightOption($(target)).totalCaption;;
                var s = '<td><div style="width:' + commandCount * 20 + 'px;text-align:right;">' + totalCaption + '</div></td>';
                var tr = $(this);
                if (tr.is(':empty')) {
                    tr.html(s);
                } else if (tr.children('td.datagrid-td-rownumber').length) {
                    $(s).insertAfter(tr.children('td.datagrid-td-rownumber'));
                } else {
                    $(s).insertBefore(tr.children('td:first'));
                }
            });
        }

        function _add(rowIndex) {
            var tr = opts.finder.getTr(target, rowIndex, 'body', 1);
            if (tr.find('span.datagrid-row-expander').length) { return; } // the expander is already exists

            var commandButtons = getInfolightOption($(target)).commandButtons;
            var updateCommandVisible = false;
            var deleteCommandVisible = false;
            var commandCount = 0;
            if (commandButtons == undefined || commandButtons.indexOf('u') != -1) {
                updateCommandVisible = true;
                commandCount++;
            }
            if (commandButtons == undefined || commandButtons.indexOf('d') != -1) {
                deleteCommandVisible = true;
                commandCount++;
            }

            var cc = [];
            cc.push('<td>');
            cc.push('<div style="text-align:center;width:' + commandButtons * 20 + 'px;height:16px;">');
            if (updateCommandVisible) {
                cc.push('<span class="datagrid-row-expander datagrid-row-expand" style="display:inline-block;width:16px;height:16px;cursor:pointer;" />');
            }
            if (deleteCommandVisible) {
                cc.push('<span class="icon-remove" style="display:inline-block;width:16px;height:16px;cursor:pointer;padding:2px;" />');
            }
            cc.push('</div>');
            cc.push('</td>');
            if (tr.is(':empty')) {
                tr.html(cc.join(''));
            } else if (tr.children('td.datagrid-td-rownumber').length) {
                $(cc.join('')).insertAfter(tr.children('td.datagrid-td-rownumber'));
            } else {
                $(cc.join('')).insertBefore(tr.children('td:first'));
            }
            $(target).datagrid('fixRowHeight', rowIndex);
            tr.find('span.datagrid-row-expander').unbind('.datagrid').bind('click.datagrid', function (e) {
                var rowIndex = $(this).closest('tr').attr('datagrid-row-index');
                if ($(this).hasClass('datagrid-row-expand')) {
                    $(target).datagrid('expandRow', rowIndex);
                } else {
                    $(target).datagrid('collapseRow', rowIndex);
                }
                $(target).datagrid('fixRowHeight');
                return false;
            });

            tr.find('span.icon-remove').unbind('.datagrid').bind('click.datagrid', function (e) {
                var rowIndex = parseInt($(this).closest('tr').attr('datagrid-row-index'));
                if (confirm(deleteMessage)) {
                    $(target).datagrid('deleteRow', rowIndex);
                    $("." + $.fn.security.foo).security('save');
                }
                return false;
            });
        }
    },

    render: function (target, container, frozen) {
        var state = $.data(target, 'datagrid');
        var opts = state.options;
        var rows = state.data.rows;
        var fields = $(target).datagrid('getColumnFields', frozen);
        var table = [];
        for (var i = 0; i < rows.length; i++) {
            table.push('<table class="datagrid-btable" cellspacing="0" cellpadding="0" border="0"><tbody>');

            // get the class and style attributes for this row
            var cls = (i % 2 && opts.striped) ? 'class="datagrid-row datagrid-row-alt"' : 'class="datagrid-row"';
            var styleValue = opts.rowStyler ? opts.rowStyler.call(target, i, rows[i]) : '';
            var style = styleValue ? 'style="' + styleValue + '"' : '';
            var rowId = state.rowIdPrefix + '-' + (frozen ? 1 : 2) + '-' + i;
            table.push('<tr id="' + rowId + '" datagrid-row-index="' + i + '" ' + cls + ' ' + style + '>');
            table.push(this.renderRow.call(this, target, fields, frozen, i, rows[i]));
            table.push('</tr>');

            table.push('<tr style="display:none;">');
            if (frozen) {
                table.push('<td colspan=' + (fields.length + 2) + ' style="border-right:0">');
            } else {
                table.push('<td colspan=' + (fields.length) + '>');
            }
            table.push('<div class="datagrid-row-detail">');
            if (frozen) {
                table.push('&nbsp;');
            } else {
                table.push(opts.detailFormatter.call(target, i, rows[i]));
            }
            table.push('</div>');
            table.push('</td>');
            table.push('</tr>');

            table.push('</tbody></table>');
        }

        $(container).html(table.join(''));
    },

    insertRow: function (target, index, row) {
        var opts = $.data(target, 'datagrid').options;
        var dc = $.data(target, 'datagrid').dc;
        var panel = $(target).datagrid('getPanel');
        var view1 = dc.view1;
        var view2 = dc.view2;

        var isAppend = false;
        var rowLength = $(target).datagrid('getRows').length;
        if (rowLength == 0) {
            $(target).datagrid('loadData', { total: 1, rows: [row] });
            return;
        }

        if (index == undefined || index == null || index >= rowLength) {
            index = rowLength;
            isAppend = true;
            this.canUpdateDetail = false;
        }

        $.fn.datagrid.defaults.view.insertRow.call(this, target, index, row);

        _insert(true);
        _insert(false);

        this.addExpandColumn(target, index);
        this.canUpdateDetail = true;

        function _insert(frozen) {
            var v = frozen ? view1 : view2;
            var tr = v.find('tr[datagrid-row-index=' + index + ']');
            var table = tr.parents('table:first');

            var newTable = $('<table cellspacing="0" cellpadding="0" border="0"><tbody></tbody></table>');
            if (isAppend) {
                newTable.insertAfter(table);
                var newDetail = tr.next().clone();
            } else {
                newTable.insertBefore(table);
                var newDetail = tr.next().next().clone();
            }
            tr.appendTo(newTable.children('tbody'));
            newDetail.insertAfter(tr);
            newDetail.hide();
            if (!frozen) {
                newDetail.find('div.datagrid-row-detail').html(opts.detailFormatter.call(target, index, row));
            }
        }
    },

    deleteRow: function (target, index) {
        var opts = $.data(target, 'datagrid').options;
        var dc = $.data(target, 'datagrid').dc;
        var tr = opts.finder.getTr(target, index);
        tr.parent().parent().remove();
        $.fn.datagrid.defaults.view.deleteRow.call(this, target, index);
        dc.body2.triggerHandler('scroll');
    },

    updateRow: function (target, rowIndex, row) {
        var dc = $.data(target, 'datagrid').dc;
        var opts = $.data(target, 'datagrid').options;
        var cls = $(target).datagrid('getExpander', rowIndex).attr('class');
        $.fn.datagrid.defaults.view.updateRow.call(this, target, rowIndex, row);
        this.addExpandColumn.call(this, target, rowIndex);
        $(target).datagrid('getExpander', rowIndex).attr('class', cls);

        // update the detail content
        if (this.canUpdateDetail) {
            var row = $(target).datagrid('getRows')[rowIndex];
            var detail = $(target).datagrid('getRowDetail', rowIndex);
            detail.html(opts.detailFormatter.call(target, rowIndex, row));
        }
    },

    onBeforeRender: function (target) {
        var opts = $.data(target, 'datagrid').options;
        var dc = $.data(target, 'datagrid').dc;
        var panel = $(target).datagrid('getPanel');

        var t = dc.view1.children('div.datagrid-header').find('table');
        if (t.find('div.datagrid-header-expander').length) {
            return;
        }

        var commandButtons = getInfolightOption($(target)).commandButtons;
        var commandCount = 0;
        if (commandButtons == undefined || commandButtons.indexOf('u') != -1) {
            commandCount++;
        }
        if (commandButtons == undefined || commandButtons.indexOf('d') != -1) {
            commandCount++;
        }

        var td = $('<td rowspan="' + opts.frozenColumns.length + '"><div class="datagrid-header-expander" style="width:' + commandCount * 20 + 'px;"></div></td>');
        if ($('tr', t).length == 0) {
            td.wrap('<tr></tr>').parent().appendTo($('tbody', t));
        } else if (opts.rownumbers) {
            td.insertAfter(t.find('td:has(div.datagrid-header-rownumber)'));
        } else {
            td.prependTo(t.find('tr:first'));
        }
    },

    onAfterRender: function (target) {
        var state = $.data(target, 'datagrid');
        var dc = state.dc;
        var opts = state.options;
        var panel = $(target).datagrid('getPanel');

        $.fn.datagrid.defaults.view.onAfterRender.call(this, target);

        if (!state.onResizeColumn) {
            state.onResizeColumn = opts.onResizeColumn;
        }
        if (!state.onResize) {
            state.onResize = opts.onResize;
        }
        function setBodyTableWidth() {
            var columnWidths = dc.view2.children('div.datagrid-header').find('table').width();
            dc.body2.children('table').width(columnWidths);
        }

        opts.onResizeColumn = function (field, width) {
            setBodyTableWidth();
            var rowCount = $(target).datagrid('getRows').length;
            for (var i = 0; i < rowCount; i++) {
                $(target).datagrid('fixDetailRowHeight', i);
            }

            // call the old event code
            state.onResizeColumn.call(target, field, width);
        };
        opts.onResize = function (width, height) {
            setBodyTableWidth();
            state.onResize.call(panel, width, height);
        };

        this.addExpandColumn(target);
        this.canUpdateDetail = true; // define if to update the detail content when 'updateRow' method is called;

        dc.footer1.find('span.datagrid-row-expander').css('visibility', 'hidden');
        $(target).datagrid('resize');

        renderQueryAutoColumn(target);
    }
});