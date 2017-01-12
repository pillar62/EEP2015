$.extend($.fn.datagrid.methods, {
    endEditing: function (jq) {
        var editIndex = $(jq).data('editIndex');
        if (editIndex == undefined) { return true }
        if ($(jq).datagrid('validateRow', editIndex)) {
            $(jq).datagrid('endEdit', editIndex);
            $(jq).removeData('editIndex');
            return true;
        } else {
            return false;
        }
    },
    acceptAllChanges: function (jq) {
        var key_prefix = 'CopyKeyOfTable_';
        jq.each(function () {
            //同步备份的主键
            var rows = $(this).datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                for (var p in row) {
                    if (row.hasOwnProperty(key_prefix + p)) {
                        row[key_prefix + p] = row[p];
                    }
                }
            }
            $(this).datagrid('acceptChanges');
        })
    }
});