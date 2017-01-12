using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EEPSDModule2015
{
    public class JQDataGridPrint
    {
        private String _ID;
        private String _dataOptions;
        private String _dataMember;
        private String _remoteName;
        private String _title;
        private String _autoApply;
        private String _alwaysClose;
        private String _pagination;
        private String _pageSize;
        private String _queryAutoColumn;
        private String _duplicateCheck;

        public String ID
        {
            set { _ID = value; }
            get { return _ID; }
        }

        public String DataOptions
        {
            set { _dataOptions = value; }
            get { return _dataOptions; }
        }

        public String DataMember
        {
            set { _dataMember = value; }
            get { return _dataMember; }
        }
        public String RemoteName
        {
            set { _remoteName = value; }
            get { return _remoteName; }
        }
        public String Title
        {
            set { _title = value; }
            get { return _title; }
        }
        public String AutoApply
        {
            set { _autoApply = value; }
            get { return _autoApply; }
        }
        public String AlwaysClose
        {
            set { _alwaysClose = value; }
            get { return _alwaysClose; }
        }
        public String Pagination
        {
            set { _pagination = value; }
            get { return _pagination; }
        }
        public String PageSize
        {
            set { _pageSize = value; }
            get { return _pageSize; }
        }
        public String QueryAutoColumn
        {
            set { _queryAutoColumn = value; }
            get { return _queryAutoColumn; }
        }
        public String DuplicateCheck
        {
            set { _duplicateCheck = value; }
            get { return _duplicateCheck; }
        }

        public List<JQDataGridQueryColumnsPrint> JQDataGridQueryColumnsPrintList { get; set; }

        public List<JQDataGridToolItemsPrint> JQDataGridToolItemsPrintList { get; set; }

        public List<JQDataGridColumnsPrint> JQDataGridColumnsPrintList { get; set; }
    }
    public class JQDataGridColumnsPrint
    {
        public string DataField { get; set; }
        public string HeaderText { get; set; }
        public string Editor { get; set; }
        public string EditorOption { get; set; }
        public string Format { get; set; }
        public string Sortable { get; set; }
        public string Frozen { get; set; }
        public string Total { get; set; }
    }
    public class JQDataGridToolItemsPrint
    {
        public string ID { get; set; }
        public string Icon { get; set; }
        public string ItemType { get; set; }
        public string Text { get; set; }
        public string OnClick { get; set; }
    }
    public class JQDataGridQueryColumnsPrint
    {
        public string DataField { get; set; }
        public string HeaderText { get; set; }
        public string Editor { get; set; }
        public string Condition { get; set; }
        public string DefaultValue { get; set; }
        public string AndOr { get; set; }
        public string NewLine { get; set; }
    }

    public class JQDataFormPrint
    {
        public string ID { get; set; }
        public string DataOptions { get; set; }
        public string RemoteName { get; set; }
        public string DataMember { get; set; }
        public string IsShowFlowIcon { get; set; }
        public string ValidateStyle { get; set; }
        public string ContinueAdd { get; set; }
        public string DuplicateCheck { get; set; }
        public List<JQDataFormColumnsPrint> JQDataFormColumnsPrintList { get; set; }
    }
    public class JQDataFormColumnsPrint
    {
        public string DataField { get; set; }
        public string HeaderText { get; set; }
        public string Editor { get; set; }
        public string EditorOption { get; set; }
        public string Format { get; set; }
    }
    public class JQValidatePrint
    {
        public string ID { get; set; }
        public string BindingObjectID { get; set; }
        public List<JQValidateItemPrint> JQValidateItemPrintList { get; set; }
    }
    public class JQValidateItemPrint
    {
        public string FieldName { get; set; }
        public string CheckNull { get; set; }
        public string ValidateType { get; set; }
        public string CheckRangeFrom { get; set; }
        public string CheckRangeTo { get; set; }
        public string CheckMethod { get; set; }
        public string Message { get; set; }
    }
    public class JQDefaultPrint
    {
        public string ID { get; set; }
        public string BindingObjectID { get; set; }
        public List<JQDefaultItemPrint> JQDefaultItemPrintList { get; set; }
    }
    public class JQDefaultItemPrint
    {
        public string FieldName { get; set; }
        public string DefaultValue { get; set; }
        public string CarryOn { get; set; }
    }
}
