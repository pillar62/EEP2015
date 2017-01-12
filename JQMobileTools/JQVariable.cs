using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JQMobileTools
{
    public class JQProperty
    {
        public const string DataOptions = "infolight-options";
        public const string SourceDataOptions = "data-options";
        
        public const string DataRole = "data-role";
        public const string DataMode = "data-mode";
        public const string DataType = "data-type";
        public const string DataTheme = "data-theme";
        public const string DataDividerTheme = "data-divider-theme";
        public const string DataOverlayTheme = "data-overlay-theme";

        public const string DataMini = "data-mini";
        public const string DataInline = "data-inline";
        public const string DataInSet = "data-inset";
        public const string DataDismissible = "data-dismissible";
       
        public const string DataIcon = "data-icon";
        public const string DataIconPos = "data-iconpos";


        public const string DataRel = "data-rel";
        public const string DataTransition = "data-transition";

        public const string DataFilter = "data-filter";
        public const string DataNativeMenu = "data-native-menu";
        public const string For = "for";
    }

    public class JQDataRole
    {
        public const string Page = "page";
        public const string Header = "header";
        public const string Footer = "footer";
        public const string Content = "content";
        public const string Tabs = "tabs";
        public const string ControlGroup = "controlgroup";
        public const string FieldContain = "fieldcontain";
        public const string NavBar = "navbar";
        public const string ListView = "listview";
        public const string ListDivider = "list-divider";
        public const string Button = "button";
        public const string Collapsible = "collapsible";
        public const string Table = "table";
        public const string Slider = "slider";
        public const string Popup = "popup";
        public const string None = "none";
        public const string Datebox = "datebox";
    }

    public class JQDataTheme
    {
        public const string A = "a";
        public const string B = "b";
        public const string C = "c";
        public const string D = "d";
        public const string E = "e";
    }

    public class JQDataMode
    { 
        public const string Reflow = "reflow";
    }

    public class JQDataIcon
    {
        public const string ArrowL = "arrow-l";
        public const string ArrowR = "arrow-r";
        public const string ArrowU = "arrow-u";
        public const string ArrowD = "arrow-d";
        public const string Delete = "delete";
        public const string Plus = "plus";
        public const string Minus = "minus";
        public const string Check = "check";
        public const string Gear = "gear";
        public const string Refresh = "refresh";
        public const string Forward = "forward";
        public const string Back = "back";
        public const string Grid = "grid";
        public const string Star = "star";
        public const string Alert = "alert";
        public const string Info = "info";
        public const string Home = "home";
        public const string Search = "search";
        public const string CaratL = "carat-l";
    }

    public class JQDataTransition
    {
        public const string Slide = "slide";
        public const string SlideUp = "slideup";
        public const string SlideDown = "slideDown";
        public const string Pop = "pop";
        public const string Fade = "fade";
        public const string Flip = "flip";
    }

    public class JQDataIconPos
    {
        public const string Left = "left";
        public const string Right = "right";
        public const string Top = "top";
        public const string Buttom = "bottom";
        public const string NoText = "notext";
    }

    public class JQClass
    {
        public const string DataGrid = "info-datagrid";
        public const string DataList = "info-datalist";
        public const string Form = "info-form";
        public const string Query = "info-query";
        public const string Selects = "info-selects";
        public const string RadioButtons = "info-radiobuttons";
        public const string Checkboxes = "info-checkboxes";
        public const string Refval = "info-refval";
        public const string FlipSwitch = "info-flipswitch";
        public const string DateBox = "info-datebox";
        public const string TimeSpinner = "info-time";
        public const string File = "info-file";
        public const string Geolocation = "info-geolocation";
        public const string Map = "info-map";
        public const string Rotator = "info-rotator";
        public const string Scan = "info-scan";
        public const string Contacts = "info-contacts";
        public const string MsgPush = "info-msgpush";
        public const string MsgRecv = "info-msgrecv";
        public const string Place = "info-place";
        public const string Call = "info-call";
        public const string Signature = "info-signature";
    }

    public class JQEditorControl
    {
        public const string Text = "text";
        public const string Selects = "selects";
        public const string Date = "date";
        public const string YearMonth = "yearmonth";
        public const string TimeSpinner = "time";
        public const string FlipSwitch = "flipswitch";
        public const string TextArea = "textarea";
        public const string Password = "password";
        public const string RadioButtons = "radiobuttons";
        public const string CheckBoxes = "checkboxes";
        public const string File = "file";
        public const string Sliders = "sliders";
        public const string Refval = "refval";
        public const string Geolocation = "geolocation";
        public const string Map = "map";
        public const string Qrcode = "qrcode";
        public const string Scan = "scan";
        public const string Capture = "capture";
        public const string Contacts = "contacts";
        public const string Call = "call";
        public const string Place = "place";
        public const string Signature = "signature";
    }

    public class JQDisplayControl
    {
        public const string Relation = "relation";
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

    public enum JQMapType
    { 
        None,
        Google,
        Baidu
    }
}
