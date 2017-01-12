using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EFWCFModule;
using System.Data.Objects;
using EFServerTools.Design;
using System.Drawing.Design;
using System.Data.Objects.DataClasses;
using System.Reflection;
using System.Drawing;
using EFServerTools.Common;

namespace EFServerTools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(EFAutoNumber), ICOInfo.EFAutoNumber)]
    public class EFAutoNumber: Component, IAutoNumber
    {
        #region Constructor
        public EFAutoNumber(System.ComponentModel.IContainer container)
            :this()
        {
            container.Add(this);
        }

        public EFAutoNumber()
        { 
            
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
        [Description(EFAutoNumberInfo.UpdateComponent)]
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
        private string _autoNoID;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.AutoNoID)]
        public string AutoNoID
        {
            get { return _autoNoID; }
            set { _autoNoID = value; }
        }

        private string _targetColumn;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.TargetColumn)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string TargetColumn
        {
            get { return _targetColumn; }
            set { _targetColumn = value; }
        }

        private string _autoNumberTableName;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.AutoNumberTableName)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.ENTITYSET)]
        public string AutoNumberTableName
        {
            get { return _autoNumberTableName; }
            set { _autoNumberTableName = value; }
        }

        private string _getFixed;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.GetFixed)]
        public string GetFixed
        {
            get { return _getFixed; }
            set { _getFixed = value; }
        }

        private int _numDig = 3;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.NumDig)]
        public int NumDig
        {
            get { return _numDig; }
            set { _numDig = value; }
        }

        private int _startValue = 1;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.StartValue)]
        public int StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }

        private int _step = 1; 
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.Step)]
        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }

        private bool _overFlow;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.OverFlow)]
        public bool OverFlow
        {
            get { return _overFlow; }
            set { _overFlow = value; }
        }

        private bool _active;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.Active)]
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        private object _number;
        [Browsable(false)]
        public object Number
        {
            set { _number = value; }
            get { return _number; }
        }

        private bool _isNumFill = false;
        [Category(ComponentInfo.COMPANY),
        Description()]
        public bool IsNumFill
        {
            get { return _isNumFill; }
            set { _isNumFill = value; }
        }

        private string _description;
        [Category(ComponentInfo.COMPANY),
        Description(EFAutoNumberInfo.Description)]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        #endregion

        #region Variables
        private Char _marker = '\'';
        private String _fixedString = null;
        #endregion

        #region Methods
        private bool CheckAutoNumber()
        {
            if (string.IsNullOrEmpty(this.AutoNoID))
            {
                throw new ArgumentNullException("AutoNoID");
            }

            if (string.IsNullOrEmpty(this.TargetColumn))
            {
                throw new ArgumentNullException("TargetColumn");
            }

            if (string.IsNullOrEmpty(this.AutoNumberTableName))
            {
                throw new ArgumentNullException("AutoNumberTableName");
            }
            return true;
        }

        internal void GetNumber(EntityObject entityObject, bool isAddStep)
        {
            try
            {
                if (CheckAutoNumber())
                {
                    string fixString = GetFixedString(entityObject);
                    EntityObject currentNumberEntityObject = GetCurrentNumber(fixString);
                    object currentNumberObj = null;

                    if (currentNumberEntityObject != null)
                    {
                        currentNumberObj = currentNumberEntityObject.GetValue(SYSAUTONUMBER.CURRNUM);
                    }

                    if (!isAddStep)
                    {
                        if (currentNumberEntityObject == null)
                        {
                            this.Number = null;
                        }
                        else
                        {
                            this.Number = fixString + FormatNum(Int32.Parse(currentNumberObj.ToString()));
                        }
                    }
                    else
                    {

                        int currnum = 0;

                        if (currentNumberEntityObject == null)
                        {
                            #region Insert into SYSAUTONUM
                            EntityObject sysAutoNumberObj = this.Context.CreateObject(this.AutoNumberTableName);
                            sysAutoNumberObj.SetValue(SYSAUTONUMBER.AUTOID, this.AutoNoID);
                            sysAutoNumberObj.SetValue(SYSAUTONUMBER.FIXED, fixString);
                            sysAutoNumberObj.SetValue(SYSAUTONUMBER.CURRNUM, this.StartValue);
                            sysAutoNumberObj.SetValue(SYSAUTONUMBER.DESCRIPTION, this.Description);
                            this.Context.AddObject(this.AutoNumberTableName, sysAutoNumberObj);
                            #endregion
                            currnum = this.StartValue;
                        }
                        else
                        {
                           
                            int newValue = Convert.ToInt32(currentNumberEntityObject.GetValue(SYSAUTONUMBER.CURRNUM)) + this.Step;
                            currentNumberEntityObject.SetValue(SYSAUTONUMBER.CURRNUM, newValue);
                            this.Context.AttachUpdated(currentNumberEntityObject);

                            currnum = Convert.ToInt32(currentNumberObj) + this.Step;
                        }

                        if (this.IsNumFill)
                        {
                            this.Number = currnum;
                        }
                        else
                        {
                            this.Number = fixString + FormatNum(currnum);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private EntityObject GetCurrentNumber(string fixString)
        {
            EntityObject resValue = null;

            string sql = string.Format("Select value c from {0}.{1} as c", Context.DefaultContainerName, AutoNumberTableName);
            ObjectQuery<EntityObject> query = this.Context.CreateQuery<EntityObject>(sql);
            var parameterPrefix = EntityProvider.GetParameterPrefix(query.Context);
            List<WhereParameter> listWhereParameter = new List<WhereParameter>();
            WhereParameter whereParameter = new WhereParameter();
            whereParameter.Field = SYSAUTONUMBER.AUTOID;
            whereParameter.Value = this.AutoNoID;
            listWhereParameter.Add(whereParameter);

            if (!this.IsNumFill)
            {
                whereParameter = new WhereParameter();
                whereParameter.Field = SYSAUTONUMBER.FIXED;
                whereParameter.And = true;
                whereParameter.Value = fixString;
                listWhereParameter.Add(whereParameter);
            }

            query = EntityProvider.SetWhere(query, listWhereParameter);
            List<EntityObject> objects = query.ToList();
            if (objects.Count != 0)
            {
                resValue = objects[0];
            }
            return resValue;
        }

#warning 此方法有空重新整理
        private String FormatNum(int value)
        {
            if (value >= 10 * (Int32)(Math.Pow(10, (_numDig - 1))) && _overFlow == false)
            {
                //String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "AutoNumber", "msg_AutoNoOverFlow");
                //throw new ArgumentException(String.Format(message, this.Name));
            }
            if (value >= 62 * (Int32)(Math.Pow(10, (_numDig - 1))))
            {
                //String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "AutoNumber", "msg_AutoNoOverFlow");
                //throw new ArgumentException(String.Format(message, this.Name));
            }

            String p1 = ""; String p2 = "";
            Int32 t1 = 0; Int32 t2 = 0; Int32 t3 = 0;

            t1 = value / (Int32)(Math.Pow(10, (_numDig - 1)));
            if (t1 >= 10 && t1 < 36)
            { t3 = t1 + 65 - 10; }

            if (t1 >= 36)
            { t3 = t1 + 97 - 36; }

            if (t1 < 10)
            { t3 = t1 + 48; }

            p1 = Char.ConvertFromUtf32(t3).ToString();


            if (_numDig > 1)
            {
                StringBuilder temp = new StringBuilder();
                for (Int32 i = 0; i < _numDig - 2; i++)
                { temp.Append("0"); }

                String formater = "{0:" + temp.ToString() + "#}";

                t2 = value % (Int32)(Math.Pow(10, (_numDig - 1)));
                //modified by lily 2007/4/2 for 2位編碼時，逢10會將後面的0省掉
                if (t2 == 0 && temp.ToString() == "")
                    p2 = "0";
                else
                    p2 = String.Format(formater, t2);
            }

            return p1 + p2;
        }

#warning 此方法有空重新整理
        private String GetFixedString(EntityObject entityObject)
        {
            if (_fixedString != _getFixed)
            {
                goto GetFixed;
            }
            if (_fixedString != null && _fixedString.Length > 0)
            { return _fixedString; }

            if (_getFixed == null || _getFixed.Length == 0)
            { _fixedString = ""; return _fixedString; }

        GetFixed:
            Char[] cs = _getFixed.ToCharArray();

#warning 加完ServerUtility再回来补
            //object[] myret = SrvUtils.GetValue(_getFixed, this.OwnerComp as DataModule);
            //if (myret != null && (int)myret[0] == 0)
            //{
            //    return (string)myret[1];
            //}

            if (cs[0] != '"' && cs[0] != '\'')
            {
                Char[] sep1 = "()".ToCharArray();
                String[] sps1 = _getFixed.Split(sep1);



                if (sps1.Length == 3) { _fixedString = this.Module.CallMethod(sps1[0], new object[] { entityObject }).ToString(); return _fixedString; }

                if (sps1.Length == 1)
                { _fixedString = sps1[0]; return _fixedString; }

                if (sps1.Length != 1 && sps1.Length != 3)
                {
                    string message = string.Empty;
                    //String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_GetFixedIsBad");
                    throw new ArgumentException(String.Format(message, ("this.Name"), _getFixed));
                }
            }

            Char[] sep2 = null;
            if (cs[0] == '"')
            { sep2 = "\"".ToCharArray(); }
            if (cs[0] == '\'')
            { sep2 = "'".ToCharArray(); }

            String[] sps2 = _getFixed.Split(sep2);
            if (sps2.Length == 3)
            { _fixedString = sps2[1]; return _fixedString; }
            else
            {
                string message = string.Empty;
                //String message = SysMsg.GetSystemMessage(((DataModule)this.OwnerComp).Language, "Srvtools", "UpdateComponent", "msg_GetFixedIsBad");
                throw new ArgumentException(String.Format(message, ("this.Name"), _getFixed));
            }
        }

        private EntityObject CreateEntityObject(ObjectContext context, string entitySetName)
        {
            MetadataProvider metadataProvider = new MetadataProvider(context.MetadataWorkspace);
            Type sysTableType = context.GetType().Assembly.GetTypes().Where(c => c.Name == metadataProvider.GetEntitySetType(context.DefaultContainerName, entitySetName).Name).ElementAt(0);
            EntityObject entityObject = (EntityObject)sysTableType.GetConstructors()[0].Invoke(null);
            return entityObject;
        }
        #endregion

        #region IAutoNumber Members

        public void Execute(List<EntityObject> objects, Dictionary<System.Data.EntityKey, System.Data.EntityState> states)
        {
            if (this.Active)
            {
                foreach (var entityObject in objects)
                {
                    if (entityObject.EntityKey == null)
                    {
                        GetNumber(entityObject, true);
                        entityObject.SetValue(this.TargetColumn, this.Number);
                    }
                }
            }
        }

        #endregion
    }

    internal class SYSAUTONUMBER
    {
        public const string AUTOID = "AUTOID";
        public const string FIXED = "FIXED";
        public const string CURRNUM = "CURRNUM";
        public const string DESCRIPTION = "DESCRIPTION";
    }
}
