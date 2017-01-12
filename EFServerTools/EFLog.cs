using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EFWCFModule;
using System.Drawing;
using System.Data.Objects;
using EFServerTools.Design;
using System.Drawing.Design;
using System.Data.Objects.DataClasses;
using EFBase;
using EFServerTools.Common;

namespace EFServerTools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(EFLog), ICOInfo.EFLog)]
    public class EFLog: Component, ILog, IUseEntitySet
    {

        #region Constructor
        public EFLog(System.ComponentModel.IContainer container)
            :this()
        {
            container.Add(this);
        }

        public EFLog()
        {
            _srcFieldNames = new EFCollection<SrcFieldNameColumn>(this);
            _onlyDistinct = true;
            _needLog = true;
            _logIDField = LogField.LogID;
            _markField = LogField.LogState;
            _modifierField = LogField.LogUser;
            _modifyDateField = LogField.LogDateTime;
        }
        #endregion

        #region IEFComponent Members
        private ObjectContext _context;
        /// <summary>
        /// Gets or sets object context of component
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Data.Objects.ObjectContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        private IEFModule _module;
        /// <summary>
        /// Gets or sets container module
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEFModule Module
        {
            get
            {
                return _module;
            }
            set
            {
                _module = value;
            }
        }
        #endregion

        #region IUseUpdateComponent Members
        private IUpdateComponent _updateComponent;
        [Category(ComponentInfo.COMPANY)]
        [Description(EFLogInfo.UpdateComponent)]
        public IUpdateComponent UpdateComponent
        {
            get
            {
                return _updateComponent;
            }
            set
            {
                _updateComponent = value;
            }
        }

        #endregion

        #region Properties
        private string _logTableName;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.LogTableName)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.ENTITYSET)]
        public string LogTableName
        {
            get { return _logTableName; }
            set { _logTableName = value; }
        }

        private bool _onlyDistinct;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.OnlyDistinct)]
        public bool OnlyDistinct
        {
            get { return _onlyDistinct; }
            set { _onlyDistinct = value; }
        }

        private EFCollection<SrcFieldNameColumn> _srcFieldNames;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.SrcFieldNames)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EFCollection<SrcFieldNameColumn> SrcFieldNames
        {
            get { return _srcFieldNames; }
            set { _srcFieldNames = value; }
        }

        private bool _needLog;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.NeedLog)]
        public bool NeedLog
        {
            get { return _needLog; }
            set { _needLog = value; }
        }

        private string _markField;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.MarkField)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        TargetEntitySetItem(), 
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string MarkField
        {
            get { return _markField; }
            set { _markField = value; }
        }

        private string _modifierField;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.ModifierField)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        TargetEntitySetItem(),
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string ModifierField
        {
            get { return _modifierField; }
            set { _modifierField = value; }
        }

        private string _logIDField;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.LogIDField)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        TargetEntitySetItem(),
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string LogIDField
        {
            get { return _logIDField; }
            set { _logIDField = value; }
        }

        private string _modifyDateField;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.ModifyDateField)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        TargetEntitySetItem(),
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string ModifyDateField
        {
            get { return _modifyDateField; }
            set { _modifyDateField = value; }
        }

        private string _logDateFormat;
        [Category(ComponentInfo.COMPANY),
        Description(EFLogInfo.LogDateFormat)]
        public string LogDateFormat
        {
            get { return _logDateFormat; }
            set { _logDateFormat = value; }
        }

        #endregion

        #region ILog Members

        public void Log(List<System.Data.Objects.DataClasses.EntityObject> objects, Dictionary<System.Data.EntityKey, System.Data.EntityState> states)
        {
            if (this.NeedLog && CheckLogField())
            {
                EntityObject logTable = this.Context.CreateObject(this.LogTableName);

                foreach (var obj in objects)
                {
                    foreach (var prop in logTable.GetType().GetProperties())
                    {
                        #region Get Value
                        object value = null;
                        if (prop.Name == this.LogIDField)
                        {
                            value = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        }
                        else if (prop.Name == this.ModifierField)
                        {
                            value = this.Module.ClientInfo.UserID;
                        }
                        else if (prop.Name == this.MarkField)
                        {
                            if (states.ContainsKey(obj.EntityKey))
                            {
                                if (states[obj.EntityKey] == System.Data.EntityState.Modified)
                                {
                                    value = LogFieldMark.M;
                                }
                                else
                                {
                                    value = LogFieldMark.D;
                                }
                            }
                            else
                            {
                                value = LogFieldMark.I;
                            }
                        }
                        else if (prop.Name == this.ModifyDateField)
                        {
                            if (string.IsNullOrEmpty(this.LogDateFormat))
                            {
                                value = DateTime.Now.ToString();
                            }
                            else
                            {
                                value = DateTime.Now.ToString(this.LogDateFormat);
                            }
                        }
                        else
                        {
                            if (this.SrcFieldNames.Where(c => c.FieldName == prop.Name).Count() > 0)
                            {
                                if (states.ContainsKey(obj.EntityKey) && states[obj.EntityKey] == System.Data.EntityState.Modified)
                                {
                                    if (this.OnlyDistinct)
                                    {
                                        EntityObject entityObject = (EntityObject)this.Context.GetObjectByKey(obj.EntityKey);
                                        string originalValue = this.Context.ObjectStateManager.GetObjectStateEntry(entityObject).OriginalValues[prop.Name].ToString();
                                        string currentValue = this.Context.ObjectStateManager.GetObjectStateEntry(entityObject).CurrentValues[prop.Name].ToString();
                                        if (string.Compare(originalValue, currentValue) != 0)
                                        {
                                            value = currentValue;
                                        }
                                    }
                                }
                                else
                                {
                                    value = obj.GetValue(prop.Name);
                                }
                            }
                        }
                        #endregion
                        if (value != null)
                        {
                            logTable.SetValue(prop.Name, value);
                        }
                    }
                }

                this.Context.AddObject(this.LogTableName, logTable);
            }
        }

        #endregion

        #region Methods
        private bool CheckLogField()
        {
            if (string.IsNullOrEmpty(this.LogIDField))
            {
                throw new ArgumentNullException("LogIDField");
            }

            if (string.IsNullOrEmpty(this.LogTableName))
            {
                throw new ArgumentNullException("LogTableName");
            }

            if (string.IsNullOrEmpty(this.MarkField))
            {
                throw new ArgumentNullException("MarkField");
            }

            if (string.IsNullOrEmpty(this.ModifierField))
            {
                throw new ArgumentNullException("ModifierField");
            }

            if (string.IsNullOrEmpty(this.ModifyDateField))
            {
                throw new ArgumentNullException("ModifyDateField");
            }
            return true;
        }

        
        #endregion

        #region IUseEntitySet Members
        [Browsable(false)]
        public string TargetEntitySet
        {
            get { return _logTableName; }
        }

        #endregion
    }

    internal class LogField
    {
        public const string LogID = "Log_ID";
        public const string LogState = "Log_State";
        public const string LogUser = "Log_User";
        public const string LogDateTime = "Log_DateTime";
    }

    internal enum LogFieldMark
    {
        I = 0,

        M = 1,

        D = 2
    }

    public class SrcFieldNameColumn: EFCollectionItem
    {
        #region Properties
        private string _fieldName;
        [Category("Infolight")]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string FieldName
        {
            get 
            { 
                return _fieldName;
            }
            set 
            {
                _fieldName = value;
            }
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return _fieldName;
        }
        #endregion
    }
}
