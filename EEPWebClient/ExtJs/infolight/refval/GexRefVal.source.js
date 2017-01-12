///<reference path="./js/vswd-ext_3.0.2.js" />

//define parameter class
var setRefvalparam = function() {
    this.refvalID = "";
    this.refTitle = "";
    this.refParentID = "";
    this.refDataUrl = "../ExtJs/infolight/refval/Common-SIN1.ashx";
    this.refModuleName = "";
    this.refCommandName = "";
    this.refShowFields = "";
    this.refShowFieldsWidth = "";
    this.refShowFieldsCaption = "";
    this.refWhereHiddenID = "";    //暫時不用，目前還沒有需求，可以使用下面的暫存屬性refFilterStr
    this.refSpecialFields = "";
    this.refHiddenFields = [];
    this.refCheckboxFields = [];
    this.refComboFields = [];
    this.refComboFieldFunc = [];
    this.refSetSort = "";
    this.refBindControlID = "";
    this.refBindValueColumn = "";  //這裡修改屬性，是指綁定的 Value欄位
    this.refBindTextColumn = "";   //這裡增加屬性，是指綁定的Text欄位，這裡一定要後面有*_Name的TextBox，這樣這個Name就不需要設定到後面的ColumnMatch裏面了
    this.refFilterStr = "";        //固定過濾條件
    this.refFilterColumns = [];    //$edit20100814 ,增加過濾聯動機制,注意欄位要帶Table名稱哦
    this.refFilterControls = [];
    this.refFilterColumnsType = []; //如果是數值類型就設int,其他的話不用設

    this.refMatchSrcColumns = [];
    this.refMatchDestcontrolIDs = []; //支持function
    this.refMultiSelect = "N";
    this.refAutoShow = "Y";
    this.refMethorControlID = "";
    this.refCheckValueFlag = "Y";
    //$edit20101130 by navy:增加下面兩個屬性，是為了處理動態SQL語法取數據的情況,Sample見bINVM01.js
    this.refDynamic = "N";
    this.refCmdsql = "";
    //$edit20101130 by navy:增加下面屬性,只爲取匯率時用
    this.refRatebydatecontrolID = "";
    //$edit20101220 by navy For有些欄位是執行function的，不能作為查詢用，所以增加屬性,多個用逗號分隔
    this.refNOQueryColumns = "";

    this.OpenRefHeight = 450;
    this.OpenRefWidth = 510;

}

function checkKey(keyAry,keystr)
{
    var flag = false;
    var len = keyAry.length;
    for(var i=0;i<len;i++)
    {
        if(keyAry[i] == keystr)
        {
            flag = true;
            break;
        }
    }
    return flag;
}

//Grid內欄位做運算式處理的反向操作
 function convertSign(str)
 {
    switch(str)
    {
        case "+":
            return "-";
            break;
        case "-":
            return "+";
            break;
        case "*":
            return "/";
            break;
        case "/":
            return "*"
            break;
        default:
            break;
    }
 }
 
//把數字類似四舍五入到設定的小數位
function Round(a_Num , a_Bit)
{
  return( Math.round(a_Num * Math.pow (10 , a_Bit)) / Math.pow(10 , a_Bit));
} 

//检查是否是数字类型
function checkNum(str)
{
    var flag = false;
    var Numtype = "system.numeric;system.double;system.float;system.decimal;system.int;system.int32;system.int16;system.int64";
    var NumtypeAry = Numtype.split(";");
    for(var i=0;i<NumtypeAry.length;i++)
    {
        if(str.trim().toLowerCase() == NumtypeAry[i].trim().toLowerCase() )
        {
            flag = true;
            break;
        }
    }
    return flag;
}

function checkDecimal(str) {
    var flag = false;
    var Numtype = "system.numeric;system.double;system.float;system.decimal";
    var NumtypeAry = Numtype.split(";");
    for (var i = 0; i < NumtypeAry.length; i++) {
        if (str.trim().toLowerCase() == NumtypeAry[i].trim().toLowerCase()) {
            flag = true;
            break;
        }
    }
    return flag;
}


function createGexRef(getRefvalparam) {
    var refvalid = getRefvalparam.refvalID;
    var reftitle = getRefvalparam.refTitle;
    var refparent = getRefvalparam.refParentID;
    var refdataurl = getRefvalparam.refDataUrl;
    var refmodulename = getRefvalparam.refModuleName;
    var refcommandname = getRefvalparam.refCommandName;
    var refshowfields = getRefvalparam.refShowFields;
    var refshowfieldswidth = getRefvalparam.refShowFieldsWidth;
    var refshowfieldscaption = getRefvalparam.refShowFieldsCaption;
    var refspecialfields = getRefvalparam.refSpecialFields;
    var refhiddendields = getRefvalparam.refHiddenFields;
    var refwherehiddenid = getRefvalparam.refWhereHiddenID;
    var refcheckboxfields = getRefvalparam.refCheckboxFields;
    var refcombofields = getRefvalparam.refComboFields;
    var refcombofieldfunc = getRefvalparam.refComboFieldFunc;
    var refsetsort = getRefvalparam.refSetSort;
    var refbindcontrolid = getRefvalparam.refBindControlID;
    var refbindvaluecolumn = getRefvalparam.refBindValueColumn;
    var refbindtextcolumn = getRefvalparam.refBindTextColumn;
    //var reffilterstr = getRefvalparam.refFilterStr;
    var reffiltercolumns = getRefvalparam.refFilterColumns;
    var reffiltercontrols = getRefvalparam.refFilterControls;
    var reffiltercolumnstype = getRefvalparam.refFilterColumnsType;
    var refcheckvalueflag = getRefvalparam.refCheckValueFlag;
    var refdynamic = getRefvalparam.refDynamic;
    var refcmdsql = getRefvalparam.refCmdsql;
    var refratebycontrolid = getRefvalparam.refRatebydatecontrolID;
    var refnoquerycolumns = getRefvalparam.refNOQueryColumns;
    var OpenRefHeight = getRefvalparam.OpenRefHeight;
    var OpenRefWidth = getRefvalparam.OpenRefWidth;


    //$edit20101130 by navy:增加下面屬性,只爲取匯率時用,因為取匯率的SQL Func（GEXFUNC_GETCURRRATE）中需要單據日期，所以取出單據日期替換 SQL中日期
    if (document.getElementById(refratebycontrolid) != null && document.getElementById(refratebycontrolid).value != "") {
        var ratebydate = document.getElementById(refratebycontrolid).value;
        refcmdsql = refcmdsql.replace("getdate()", "'" + ratebydate + "'");
    }
    
    var refsrccolumns = getRefvalparam.refMatchSrcColumns;
    var refdestcontrolids = getRefvalparam.refMatchDestcontrolIDs;
    var refmultiselect = getRefvalparam.refMultiSelect;
    var refautoshow = getRefvalparam.refAutoShow;
    var refmethorcontrolid = getRefvalparam.refMethorControlID;

    var valueColumn = refbindvaluecolumn.indexOf('.') > -1 ? refbindvaluecolumn.substring(refbindvaluecolumn.indexOf('.') + 1) : refbindvaluecolumn;
    var textColumn = refbindtextcolumn.indexOf('.') > -1 ? refbindtextcolumn.substring(refbindtextcolumn.indexOf('.') + 1) : refbindtextcolumn;
    var motherTag = refmethorcontrolid != "" ? refmethorcontrolid + "_" : "";
    var refBindValueControlID = motherTag + refbindcontrolid + "_txtValue";
    var refBindSelectControlID = motherTag + refbindcontrolid + "_ddlSelect";
    var refBindTextControlID = motherTag + refbindcontrolid + "_txtDisplay";
    var refBindShowControlID = motherTag + refbindcontrolid + "_txtShow";

    var objRef = new Object;
    Ext.state.Manager.setProvider(new Ext.state.CookieProvider());

    //===============內部參數定義 Start=========================================
    objRef.multiJson = "";
    objRef.ref_Data = null;
    objRef.ref_Store = null;
    objRef.ref_Fields = null;
    objRef.ref_Plant = null;
    objRef.ref_Global_rowindex = 0;
    objRef.ref_wherestr = getRefvalparam.refFilterStr;
    objRef.ref_gridid = refvalid + "grid";
    objRef.ref_gridPanel = null;
    objRef.ref_Panel = null;
    objRef.ref_Window = null;
    //===============內部參數定義 End===========================================

    //$edit20100814 by navy For 增加過濾聯動機制，設定聯動的欄位和元件
    if (reffiltercolumns.length > 0) {
        var getFilterStr = "";
        for (var f = 0; f < reffiltercolumns.length; f++) {
            //因為元件是放在WebFormView裏面，所以ID會加前綴
            var filtercontrolsID = motherTag + reffiltercontrols[f];
            if (reffiltercontrols.length > f && document.getElementById(filtercontrolsID) != null && document.getElementById(filtercontrolsID).value != "") {
                if (reffiltercolumnstype.length > f && reffiltercolumnstype[f].toLowerCase() == "int")
                    getFilterStr += " AND " + reffiltercolumns[f] + "=" + document.getElementById(filtercontrolsID).value;
                else
                    getFilterStr += " AND " + reffiltercolumns[f] + "='" + document.getElementById(filtercontrolsID).value + "'";
            }
        }
        if (getFilterStr.length > 4)
            getFilterStr = getFilterStr.substr(4);

        if (objRef.ref_wherestr == "")
            objRef.ref_wherestr = getFilterStr;
        else
            objRef.ref_wherestr += " AND " + getFilterStr;
    }

    //===============Create grid Start======================================
    //debugger;
    if (typeof (multiJson_wgvMaster) != 'undefined' && multiJson_wgvMaster != undefined && multiJson_wgvMaster != null && multiJson_wgvMaster != "")
        objRef.multiJson = multiJson_wgvMaster;
    else if (typeof (multiJson_wgvDetail) != 'undefined' && multiJson_wgvDetail != undefined && multiJson_wgvDetail != null && multiJson_wgvDetail != "")
        objRef.multiJson = multiJson_wgvDetail;
//    else
//        objRef.multiJson = genieReadXml();

    objRef.getColumnWidth = function(columnname) {
        var wid = 100;
        var columnAry = refshowfields.split(',');
        var widthAry = refshowfieldswidth.split(',');
        for (var s = 0; s < columnAry.length; s++) {
            if (columnAry[s] == columnname) {
                if (widthAry.length > s && widthAry[s] != null && widthAry[s] != "")
                    wid = widthAry[s];
                break;
            }
        }
        return wid;
    }

    objRef.getColumnCaption = function(columnname) {
        //debugger;
        //var ssTag = "objRef.multiJson." + refvalid + "_" + columnname;
        //var colCap = eval(ssTag);
        //if (colCap == "" || colCap == undefined) {
        //    colCap = columnname;
        //}
        //return colCap;

        var cap = "";
        var columnAry = refshowfields.split(',');
        var captionAry = refshowfieldscaption.split(',');
        for (var s = 0; s < columnAry.length; s++) {
            if (columnAry[s] == columnname) {
                if (captionAry.length > s && captionAry[s] != null && captionAry[s] != "")
                    cap = captionAry[s];
                break;
            }
        }
        return cap;
    }
       
    var CheckColumn = new Array();
    var addColumn = function() {
        this.fields = '';
        this.columns = '';
        this.initvalue = '';

        this.addColumns = function(name, caption)  /* Create Column Class, and Dynamic add row*/
        {
            if (this.fields.length > 0) {
                this.fields += ',';
            }
            if (this.initvalue.length > 0) {
                this.initvalue += ',';
            }

            var renderfunc = "";
            var typefunc = "";
            var xtypefunc = "";
            var intval = "";

            /*
            Special Operation 
            这里是处理specialfields的设定，specialfields多栏位是以逗号分隔，目前主要是DB栏位是varchar要存贮Datatime类型的资料时,
            */
            var spfileds = refspecialfields.split(',');
            if (checkKey(spfileds, name)) {
                renderfunc = ",renderer:Ext.util.Format.dateRenderer('Y/m/d'),editor: new Ext.form.DateField({format: 'Y/m/d'}) ";
                typefunc = ",type:'date'";
            }
            else if (checkKey(refcombofields, name)) {
                for (var c = 0; c < refcombofields.length; c++) {
                    if (refcombofields[c] == name)
                        renderfunc = ",renderer:MyDefineComboFormat,editor: " + refcombofieldfunc[c];
                }
            }
            else if (checkKey(refcheckboxfields, name) || caption.toLowerCase() == "system.boolean") {
                renderfunc = "";
                typefunc = ",type:'bool'";
            }
            else {
                if (caption.toLowerCase() == "system.datetime") {
                    renderfunc = ",renderer:Ext.util.Format.dateRenderer('Y/m/d'),editor: new Ext.form.DateField({format: 'Y/m/d'}) ";
                    typefunc = ",type:'date'";
                }
                else if (caption.toLowerCase() == "system.string") {
                    renderfunc = ",editor: new Ext.form.TextField({ allowBlank: true })";
                }
                else if (checkNum(caption)) {
                    renderfunc = ",editor: new Ext.form.NumberField({ allowBlank: true })"; // ,renderer:MyDefineNumFormat
                    intval = "0";
                }
                else {
                    renderfunc = ",editor: new Ext.form.TextField({ allowBlank: true })";
                }
            }

            this.fields += '{name:"' + name + '" ' + typefunc + '}';
            this.initvalue += name + ':"' + intval + '"';


            //$增加屬性more_syssetfields,設定不容許操作欄位，原hidefields拿出作為預設隱藏欄位設定，可以讓User操作的
            if (typeof (more_syssetfields) == 'undefined' || !checkKey(more_syssetfields, name)) {
                if (this.columns.length > 0) {
                    this.columns += ',';
                }

                var hiddenFlag = checkKey(refhiddendields, name);

                if (checkKey(refcheckboxfields, name) || caption.toLowerCase() == "system.boolean") {
                    CheckColumn.push(new Ext.grid.CheckColumn({ "id": name + "id", "header": name, "dataIndex": name, hidden: hiddenFlag, "align": "left", "width": 100, "sortable": true }));
                    this.columns += 'CheckColumn[' + (CheckColumn.length - 1).toString() + ']';
                }
                else {
                    var alignStr = checkNum(caption) ? 'right' : 'left';
                    this.columns += '{id:"' + name + 'id",header:"' + objRef.getColumnCaption(name) + '",dataIndex:"' + name + '",hidden:' + hiddenFlag + ',align:"' + alignStr
                    + '",width:' + objRef.getColumnWidth(name) + ',sortable:' + true + renderfunc + '}';
                }
            }
        };
    };

    objRef.makeGridColumn = function () {
        var columnURL = refdataurl + "?doType=getRefvalColumns&refdynamic=" + refdynamic + "&refcmdsql=" + refcmdsql + "&module=" + refmodulename + "&command=" + refcommandname + "&fields=" + refshowfields;
        Ext.Ajax.request
         ({
             url: columnURL,
             success: function (response, option) {

                 if (response.responseText == "") {
                     return;
                 }
                 objRef.ref_Data = new addColumn();

                 var res = Ext.decode(response.responseText);
                 for (var i = 0; i < res.length; i++) {
                     for (var p in res[i]) {
                         objRef.ref_Data.addColumns(p, eval("res[i]." + p));
                     }
                 }

                 /*New blank Field, for difference Insert / Edit*/
                 //objRef.ref_Data.fields += ',{name:"genieEditType",type:"string"}';
                 //objRef.ref_Data.initvalue += ',genieEditType:"add" ';

                 objRef.makeGrid();
             },
             failure: function (response, option) {
                 alert('222');
                 //alert(geniesysMessage.getdata.getfailure);
             }
         });
    }

    objRef.makeGrid = function() {
    
        //var sm = new Ext.grid.CheckboxSelectionModel();
        //var cm = new Ext.grid.ColumnModel(eval('([new Ext.grid.RowNumberer(),' + objRef.ref_Data.columns + '])'));

        objRef.ref_Fields = eval('([' + objRef.ref_Data.fields + '])');
        //debugger;
        //if (document.getElementById(refwherehiddenid) != null)
        //    objRef.ref_wherestr = document.getElementById(refwherehiddenid).value;
        //else
        //    objRef.ref_wherestr = "";

        var checkfields = "";
        if (refcheckboxfields.length > 0) {
            for (var s = 0; s < refcheckboxfields.length; s++) {
                if (checkfields != "")
                    checkfields += ",";
                checkfields += refcheckboxfields[s];
            }
        }

        var dataURL = refdataurl + "?doType=getRefvalData&refdynamic=" + refdynamic + "&refcmdsql=" + refcmdsql + "&module=" + refmodulename + "&command=" + refcommandname + "&fields=" + refshowfields
         + "&specialfields=" + refspecialfields + "&checkfields=" + checkfields + "&setsort=" + refsetsort + "&where=" + encodeURIComponent(objRef.ref_wherestr) + "&refnoquerycolumns=" + refnoquerycolumns;
        //var dataURL = refdataurl;

        Ext.define('GexRefValGridViewModel', {
            extend: 'Ext.data.Model',
            fields: objRef.ref_Fields,
            state: []
        });

        objRef.ref_Store = Ext.create('Ext.data.JsonStore', {
            model: 'GexRefValGridViewModel',
            proxy: {
                type: 'ajax',
                url: dataURL,
                reader: {
                    totalProperty: 'total',
                    type: 'json',
                    root: 'root'
                },
            },
            extraParams: {
                doType: 'getRefvalData',
                refdynamic: refdynamic,
                refcmdsql: refcmdsql,
                module: refmodulename,
                command: refcommandname,
                fields: refshowfields,
                specialfields: refspecialfields,
                checkfields: checkfields,
                setsort: refsetsort,
                where: encodeURIComponent(objRef.ref_wherestr),
                refnoquerycolumns: refnoquerycolumns
            }
        });

       
        objRef.ref_Plant = Ext.create("GexRefValGridViewModel");

        var gridConfig = {
            //title: '',
            selType: 'rowmodel',
            multiSelect: true,
            columns: eval('([new Ext.grid.RowNumberer(),' + objRef.ref_Data.columns + '])'),
            id: objRef.ref_gridid,
            //renderTo: more_GridDivID,
            store: objRef.ref_Store,
            frame: true,
            border: true,
            layout: "fit",
            bodyStyle: 'width:100%',
            //height: 290,
            /*autoHeight :true,*/
            //tbar: itemlist,
            //bbar: more_pagingBar,
            //clicksToEdit: 1,
            border: true,
            autoScroll: true,
            //plugins: CheckColumn,
            //view: view,
            listeners: { 'rowclick': function (Grid, rowIdx, e) { objRef.ref_Global_rowindex = rowIdx; } },  //Add listeners, Save the index, For the function of Combo Refval
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
                                        delete options.filterConditions;
                                        delete options.where;
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
                                        options.alwaysClose = false;
                                        options.start = 0;
                                        options.filterConditions = Ext.encode(filterConditions);
 //                                       var dataURL = refdataurl + "?doType=getRefvalData&refdynamic=" + refdynamic + "&refcmdsql=" + refcmdsql + "&module=" + refmodulename + "&command=" + refcommandname + "&fields=" + refshowfields
 //+ "&specialfields=" + refspecialfields + "&checkfields=" + checkfields + "&setsort=" + refsetsort + "&refnoquerycolumns=" + refnoquerycolumns;
                                        this.store.proxy.url = dataURL;
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
            }
        };
        //debugger;
        objRef.ref_gridPanel = new Ext.grid.Panel(gridConfig);

        /*$edit090925,增加跳转回原单的功能*/
        //if (more_gotokeyName != undefined && more_gotoUrl != undefined && more_gotokeyName != '' && more_gotoUrl != '')
        //    gridPanel.on("rowdblclick", rowDBClick, gridPanel);

        /*gridPanel.on("afterrender",afterrendgrid,gridPanel);Sample見bPMSM13A.js*/
        //if (more_rowclick != undefined && more_rowclick == "Y")
        //    gridPanel.on("rowclick", personClick);

        //For Combo的功能增強
        /*
        var comboLen = this.ref_gridPanel.getColumnModel().config.length;
        for (var ci = 0; ci < comboLen; ci++) {
        if (this.ref_gridPanel.getColumnModel().config[ci].editor && this.ref_gridPanel.getColumnModel().config[ci].editor.field) {
        var field = this.ref_gridPanel.getColumnModel().config[ci].editor.field;
        var ctype = this.ref_gridPanel.getColumnModel().config[ci].editor.getXType();
        if (ctype == "combo") {
        var destRecord = null;
        field.on('focus', function(field) {
        //debugger;
        destRecord = this.ref_gridPanel.getSelectionModel().getSelections()[0];
        });
        field.on('change', function(field, newvalue, oldvalue) {
        //debugger;
        var columnMatch = field.initialConfig.columnMatch;
        var srcRecord = field.store.getAt(field.selectedIndex);
        if (destRecord && srcRecord) {
        Ext.each(columnMatch, function(match) {
        destRecord.set(match.destField, srcRecord.data[match.srcField]);
        });
        }
        });
        }
        }
        }
        };
        */

        objRef.createWindow();
    }
    
    //===============Create grid End========================================

    objRef.createWindow = function() {
        objRef.ref_Panel = new Ext.Panel({
            //applyTo: 'search-panel',
            //title: 'Forum Search',
            //height: 320,
            autoScroll: true,
            items: objRef.ref_gridPanel,

            tbar:[{
                id: refvalid + 'RefValQueryButton',
                text: '查询',
                icon: '../Image/Ext/query.gif',
                cls: 'x-btn-text-icon details',
                handler: function (sender, args) {
                    var queries = new Ext.util.MixedCollection();
                    for (var k = 0; k < objRef.ref_Fields.length; k++) {
                        var columnName = objRef.ref_Fields[k];
                        if (queries.containsKey(columnName.name)) {
                            queries.get(columnName.name).push({ condition: 'And', operator: '%', caption: objRef.getColumnCaption(columnName.name), editor: 'TextBox' });
                        }
                        else { queries.add(columnName.name, [{ condition: 'And', operator: '%', caption: objRef.getColumnCaption(columnName.name), editor: 'TextBox' }]); }
                    }
                    if (queries.getCount() > 0) {
                        objRef.ref_gridPanel.gridQuery(queries,
                            {
                                columnsCount: 1,
                        panWidth: 265,
                        panHeight: 400,
                        uiCaptions: ['', '提交', '取消']
                    });
                    }
                }
            }],
            bbar: new Ext.PagingToolbar({
                store: objRef.ref_Store,
                displayInfo: true,
                pageSize:25,
                displayMsg: 'Topics {0} - {1} of {2}',
                emptyMsg: "No topics to display"
                /*items: [
                '-', {
                pressed: true,
                enableToggle: true,
                text: 'Show Preview',
                cls: 'x-btn-text-icon details',
                toggleHandler: function(btn, pressed) {
                var view = grid.getView();
                view.showPreview = pressed;
                view.refresh();
                }
                }]*/
            })
        });

        // trigger the data store load
        objRef.ref_Store.load();
        //=========================================================================

        objRef.ref_Window = new Ext.Window({
            title: reftitle,
            width: OpenRefWidth,
            height: OpenRefHeight,
            //minWidth: 700,
            //minHeight: 300,
            layout: 'fit',
            plain: true, //将标签页头的背景设置为透明
            hidden: true,
            closable: true,
            modal: true,
            bodyStyle: 'padding:5px;',
            buttonAlign: 'center',
            items: objRef.ref_Panel,
            buttons: [{
                text: '確 定',
                cls: 'x-btn-text-icon details',
                icon: '../genieExtSources/images/ok.png',
                handler: function(btn, pressed) {
                    //debugger;
                    var rows = objRef.ref_gridPanel.getSelectionModel().getSelection();
                    if (rows.length == 0)
                        alert("請先選中資料!");
                    else {
                        if (refmultiselect == "N") {
                            var selectValue = "";
                            var selectText = "";
                            for (var k = 0; k < rows.length; k++) {
                                selectValue += eval('rows[0].data.' + valueColumn);
                                selectText += eval('rows[0].data.' + textColumn);
                                if (k != rows.length - 1) {
                                    selectValue += ",";
                                    selectText += ",";
                                }
                            }
                            if (document.getElementById(refbindcontrolid) != null) {
                                document.getElementById(refbindcontrolid).value = selectValue;
                                //document.getElementById(refbindcontrolid).onchange();
                            }
                            if (document.getElementById(refbindcontrolid + "-inputEl") != null) {
                                document.getElementById(refbindcontrolid + "-inputEl").value = selectValue;
                                //document.getElementById(refbindcontrolid).onchange();
                            }
                            if (document.getElementById(refbindcontrolid + "_Name-inputEl") != null)
                                document.getElementById(refbindcontrolid + "_Name-inputEl").value = selectText;
                            objRef.doneColumnMatch(rows, "N", "N");
                        }
                        else {
                            if (rows.length > 1)
                                alert("選中的筆數大於1，請選中一筆!");
                            else {
                                //debugger;
                                //objRef.doneColumnMatch(rows, "N", "N");
                            }
                        }
                        if (objRef.ref_Window) {
                            objRef.ref_Window.close();
                        }
                    }
                }
            }, {
                text: '放 棄',
                cls: 'x-btn-text-icon details',
                icon: '../genieExtSources/images/delete.png',
                handler: function(btn, pressed) {
                    if (objRef.ref_Window) {
                        objRef.ref_Window.close();
                    }
                }
}]
            });
            if (refautoshow != "N")
                objRef.ref_Window.show();
        }

        //根據參數，開始創建Grid
        if (refautoshow != "N")
            objRef.makeGridColumn();
        
        //===============================================================Add method=============================================================================
        objRef.showwindow = function() {
            objRef.ref_Window.show();
        };

        objRef.doneColumnMatch = function(rows, mtflag,sf) {
            //debugger;
            var rowSourcestr = 'rows[0].data.';
            if (mtflag != undefined && mtflag == "Y")
                rowSourcestr = 'rows[0].';

            if (refsrccolumns.length > 0) {
                //debugger;
                for (var p = 0; p < refsrccolumns.length; p++) {
                    var ssa = refparent + refdestcontrolids[p] + "-inputEl";
                    //$Edit 20101019 by alex：檔多個源欄位對應一個目標欄位時，用“ | ”分割，issue860
                    var column = refsrccolumns[p].split('|');
                    for (var c = 0; c < column.length; c++) {
                        if (document.getElementById(ssa) != null && column != null) {
                            if (eval(rowSourcestr + column[c]) == "")
                                continue;
                            else {
                                document.getElementById(ssa).value = eval(rowSourcestr + column[c]);
                                if (document.getElementById(ssa).onchange != null)
                                    document.getElementById(ssa).onchange();
                                break;
                            }
                        }
                    }
                }
            }
        };

        //手動輸入觸發
        objRef.getRefName = function(refvalue) {

        var getNameURL = refdataurl + "?doType=getRefvalRow&refdynamic=" + refdynamic + "&refcmdsql=" + refcmdsql + "&module=" + refmodulename + "&command=" + refcommandname + "&fields=" + refshowfields + "&refvalue=" + encodeURIComponent(refvalue) +
            "&refvaluecolumn=" + refbindvaluecolumn + "&reftextcolumn=" + refbindtextcolumn;
            Ext.Ajax.request
             ({
                 url: getNameURL,
                 success: function(response, option) {
                     //debugger;
                     var refdata = eval("(" + response.responseText + ")");
                     if (refdata.flag == "Y") {
                         if (refdata.count > 0) {
                             var rowSourcestr = 'refdata.rowdata[0].';

                             if (document.getElementById(refbindcontrolid + "_Name-inputEl") != null)
                                 document.getElementById(refbindcontrolid + "_Name-inputEl").value = eval(rowSourcestr + textColumn);

                             //objRef.doneColumnMatch(refdata.rowdata, "Y","N");
                         }
                         else {
                             if (refcheckvalueflag == "Y") {
                                 if (document.getElementById(refbindcontrolid + "_Name-inputEl") != null)
                                     document.getElementById(refbindcontrolid + "_Name-inputEl").value = "";

                             }
                             else {
                                 if (document.getElementById(refbindcontrolid + "_Name-inputEl") != null)
                                     document.getElementById(refbindcontrolid + "_Name-inputEl").value = "";

                             }
                         }
                     }
                     else {
                         if (document.getElementById(refbindcontrolid + "_Name-inputEl") != null)
                             document.getElementById(refbindcontrolid + "_Name-inputEl").value = "";

                         alert(refdata.rowdata);
                     }
                 },
                 failure: function(response) { alert(response.responseText); }
             });
        }

    //离开时触发ColumnMatch
        objRef.onblurColumnMatch = function (refvalue) {
            var getNameURL = refdataurl + "?doType=getRefvalRow&refdynamic=" + refdynamic + "&refcmdsql=" + refcmdsql + "&module=" + refmodulename + "&command=" + refcommandname + "&fields=" + refshowfields + "&refvalue=" + encodeURIComponent(refvalue) +
                "&refvaluecolumn=" + refbindvaluecolumn + "&reftextcolumn=" + refbindtextcolumn;
            Ext.Ajax.request
             ({
                 url: getNameURL,
                 success: function (response, option) {
                     //debugger;
                     var refdata = eval("(" + response.responseText + ")");
                     if (refdata.flag == "Y") {
                         if (refdata.count > 0) {
                             var rowSourcestr = 'refdata.rowdata[0].';

                             objRef.doneColumnMatch(refdata.rowdata, "Y", "N");
                         }
                         else {
                         }
                     }
                 },
                 failure: function (response) { alert(response.responseText); }
             });
        }
        return objRef;
    }
