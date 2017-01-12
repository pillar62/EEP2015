namespace AjaxTools
{
    public enum AnimationAction { Open = 0, Close = 1 }

    public enum AjaxTreeViewColumnType { TextBoxColumn = 0, ComboBoxColumn = 1, RefValColumn = 2, CalendarColumn = 3, RadioButtonColumn = 4 }

    public enum AjaxColorStyle { Blue, Green, White, Black }

    public enum UpdateProgressContentType { Image, Text }

    public enum ResourceFileType { Css, Javascript }

    //public enum ExtGridType { PageDataSource, Complex }

    //public enum ExtGridSelectionMode { None, CellSelectionModel, RowSelectionModel, CheckboxSelectionModel }

    public enum ExtGridSystemHandler { Abort, Add, Cancel, CustomDefine, Delete, Edit, LoadPersonal, OK, Query, Refresh, Save, SavePersonal }

    public enum ExtGridToolItemType { Label, Button, Separation, Fill }

    public enum ExtGridEditor { CheckBox, ComboBox, DateTimePicker, RefVal, RefButton, TextBox, TextArea, NumberField };

    public enum ExtModalPanelMode { None, OkCancel };

    public enum ValidateType { None, Method, Alpha, AlphaNumber, Email, Url, Int, Float, IPAddress }

    public enum ExtTheme { Access, Default, Gray, Slate }

    public enum ExtModalPanelType { InsertUpdate, Query, RefButton };

    //public enum AjaxScheduleButtonPosition { Left, Center, Right };

    public enum AjaxScheduleButtonType { PreviousYear, Previous, NextYear, Next, Today, Month, Week, Day };

    public enum AjaxScheduleWeekMode { Fixed, Liquid, Variable };

    public enum AjaxScheduleDefaultView { Month, Week, Day };

    public enum AjaxTheme { Beige, Black, Blue, Green, Purple, Sunny, White }
}