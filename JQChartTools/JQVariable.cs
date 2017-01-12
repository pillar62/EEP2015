using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace JQChartTools
{
    public class JQControl
    {
        public const string Pie = "info-plotpiechart";
        public const string Bar = "info-plotbarchart";
        public const string Line = "info-plotlinechart";
        public const string DashBoard = "info-plotDashBoard";
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
}
