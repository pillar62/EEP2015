using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace JQClientTools
{
    public class JQControl
    {
        public const string DataGrid = "info-datagrid";
        public const string LinkButton = "easyui-linkbutton";
        public const string Window = "easyui-window";
        public const string Panel = "easyui-panel";
        public const string Dialog = "easyui-dialog";
        public const string Tab = "easyui-tabs";
        public const string ValidateBox = "easyui-validatebox";
        public const string ComboBox = "info-combobox";
        public const string ComboGrid = "info-combogrid";
        public const string DateBox = "easyui-datebox";
        public const string DateTimeBox = "easyui-datetimebox";
        public const string TimeSpinner = "easyui-timespinner";
        public const string NumberBox = "easyui-numberbox";
        public const string RefValBox = "info-refval";
        public const string ToolBar = "info-toolbar";
        public const string FileUpload = "info-fileUpload";
        public const string TreeView = "info-treeview";
        public const string AutoComplete = "info-autocomplete";
        public const string Options = "info-options";
        public const string FLComment = "info-flcomment";
        public const string Qrcode = "info-qrcode";
        public const string Schedule = "info-schedule";
        public const string BatchMove = "info-batchMove";
        public const string Mail = "info-mail";
        public const string YearMonth = "info-yearmonth";
        public const string Rotator = "info-rotator";
        public const string PivotTable = "info-pivottable";
    }

    public class JQEditorControl
    {
        public const string TextBox = "text";
        public const string FixTextBox = "fixtext";
        public const string TextArea = "textarea";
        public const string CheckBox = "checkbox";
        public const string NumberBox = "numberbox";
        public const string ValidateBox = "validatebox";
        public const string DateBox = "datebox";
        public const string TimeSpinner = "timespinner";
        public const string ComboBox = "infocombobox";
        public const string ComboGrid = "infocombogrid";
        //public const string ComboTree = "combotree";
        public const string RefValBox = "inforefval";
        public const string Password = "password";
        public const string FileUpload = "infofileupload";
        public const string AutoComplete = "infoautocomplete";
        public const string Options = "infooptions";
        public const string Qrcode = "qrcode";
        public const string YearMonth = "yearMonth";
    }

    public class JQProperty
    {
        public const string DataOptions = "data-options";
        public const string InfolightOptions = "infolight-options";
    }

    public class JQIcon
    {
        public const string Add = "icon-add";
        public const string Back = "icon-back";
        public const string Blank = "icon-blank";
        public const string Cancel = "icon-cancel";
        public const string Cut = "icon-cut";
        public const string Edit = "icon-edit";
        public const string Help = "icon-help";
        public const string MiniAdd = "icon-mini-add";
        public const string MiniEdit = "icon-mini-edit";
        public const string MiniRefresh = "icon-mini-refresh";
        public const string No = "icon-no";
        public const string OK = "icon-ok";
        public const string Print = "icon-print";
        public const string Redo = "icon-redo";
        public const string Reload = "icon-reload";
        public const string Remove = "icon-remove";
        public const string Save = "icon-save";
        public const string Search = "icon-search";
        public const string Sum = "icon-sum";
        public const string Tip = "icon-tip";
        public const string Undo = "icon-undo";
        public const string View = "icon-view";
        public const string Excel = "icon-excel";
        public const string Previous = "icon-previous";
        public const string Next = "icon-next";
    }

    public class JQCondtion
    {
        public const string Equal = "=";
        public const string NotEqual = "!=";
        public const string GreaterThan = ">";
        public const string EqualOrGreaterThan = ">=";
        public const string LessThan = "<";
        public const string EqualOrLessThan = "<=";
        public const string BeginWith = "%";
        public const string Contain = "%%";
        public const string In = "in";
    }

    public class JQAndOr
    {
        public const string And = "and";
        public const string Or = "or";
    }

    public class JQDataType
    {
        public const string Number = "number";
        public const string String = "string";
        public const string DateTime = "datetime";
        public const string Guid = "guid";
        //public const string OracleDateTime = "oracledatetime";
    }

    public class JQTotalType
    {
        public const string Sum = "sum";
        public const string Max = "max";
        public const string Min = "min";
        public const string Average = "avg";
        public const string Count = "count";
    }

    public class JQLocale
    {
        public const string Afrikaans = "af";
        public const string Arabic = "ar";
        public const string Bulgarian = "bg";
        public const string Catalan = "ca";
        public const string Czech = "cs";
        //public const string Czech = "cz";
        public const string Danish = "da";
        public const string German = "de";
        public const string Greek = "el";
        public const string English = "en";
        public const string Spanish = "es";
        public const string French = "fr";
        public const string Italian = "it";
        public const string Japanese = "jp";
        public const string Dutch = "nl";
        public const string Portuguese = "pt_BR";
        public const string Russian = "ru";
        public const string Turkish = "tr";
        public const string Chinese_China = "zh_CN";
        public const string Chinese_Taiwan = "zh_TW";
    }

    public class JQAlignment
    {
        public const string Left = "left";
        public const string Center = "center";
        public const string Right = "right";
    }

    public class JQReportAlignment
    {
        public const string Left = "Left";
        public const string Center = "Center";
        public const string Right = "Right";
        //public const string Stretch = "Stretch";
    }

    public class JQReportTotal
    {
        public const string Avg = "Avg";
        public const string Count = "Count";
        public const string Max = "Max";
        public const string Min = "Min";
        public const string Sum = "Sum";
    }
}
