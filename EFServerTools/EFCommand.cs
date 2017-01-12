using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using EFWCFModule;
using System.Data;
using System.Data.EntityClient;
using EFServerTools.Design;
using System.Drawing.Design;
using EFBase;
using EFServerTools.Common;
using System.Drawing;
using EFServerTools.Design.EFCommandDesign;
using System.ComponentModel.Design;

namespace EFServerTools
{
    /// <summary>
    /// EFCommand元件
    /// </summary>
    [Designer(typeof(EFCommandDesigner), typeof(IDesigner))]
    [ToolboxBitmap(typeof(EFCommand), ICOInfo.EFCommand)]
    public class EFCommand: Component, ICommand
    {
        #region Constructor
        /// <summary>
        /// 創建一個EFCommand的實例
        /// </summary>
        public EFCommand()
        {
            InitializeComponent();
            _foreignKeyRelations = new EFCollection<IncludeObject>(this);
            _parameters = new EFCollection<EFParameter>(this);
        }

        /// <summary>
        /// 創建一個EFCommand的實例並將EFCommand加到一個特定的容器里
        /// </summary>
        /// <param name="container">包容EFCommand的容器</param>
        public EFCommand(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// 初始化EFCommand - 不要用代碼編輯器修改此方法的內容
        /// </summary>
        private void InitializeComponent() { }

        #endregion

        #region IEFComponent Members

        private CommandType _CommandType = CommandType.Text;
        /// <summary>
        /// 取得或設置EFCommand的CommandType
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.CommandType)]
        public CommandType CommandType
        {
            get
            {
                return _CommandType;
            }
            set
            {
                _CommandType = value;
            }
        }

        private string _CommandText;
        /// <summary>
        /// 取得或設置EFCommand的CommandText
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.CommandText)]
        public string CommandText
        {
            get
            {
                return _CommandText;
            }
            set
            {
                _CommandText = value;
            }
        }

        private ObjectContext _context;
        /// <summary>
        /// 取得或設定此元件的ObjectContext
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
        /// 取得或設定包容此元件的EFModule
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

        private bool _serverModify = true;
        /// <summary>
        /// 取得或設定ServerModify
        /// </summary>
        public bool ServerModify
        {
            get
            {
                return _serverModify;
            }
            set
            {
                _serverModify = value;
            }
        }

        #endregion

        #region ICommand Members
        /// <summary>
        /// 取得一個實體對象的列表
        /// </summary>
        /// <param name="clientInfo">客戶端的資訊</param>
        /// <returns>一個實體對象的列表</returns>
        public ObjectQuery<EntityObject> GetObjects(ClientInfo clientInfo)
        {
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            ObjectQuery<EntityObject> objects = Context.CreateQuery<EntityObject>(CommandText);

            foreach (var item in ForeignKeyRelations)
            {
                objects = objects.Include(item.ObjectName);
            }

            objects = FilterObjects(objects, clientInfo);

            return objects;
        }

        /// <summary>
        /// 取得一個實體對象的列表
        /// </summary>
        /// <returns>一個實體對象的列表</returns>
        public object ExecuteStoredProcedure()
        {
            var storeProcedureName = string.IsNullOrEmpty(CommandText) ? string.Empty : CommandText.Split('.')[1];
            var method = Context.GetType().GetMethods()
                .Where(c=> c.Name.Equals(storeProcedureName) && c.GetParameters().Count().Equals(Parameters.Count)).FirstOrDefault();
            if (method == null)
            { 
                throw new MissingMethodException(string.Format("Store Procudure:{0} not found.", CommandText));
            }

            object[] parameters= new object[Parameters.Count];
            var methodParameters = method.GetParameters().ToList();
            for (int i = 0; i < Parameters.Count; i++)
            {
                var parameterType = methodParameters[i].ParameterType.IsGenericType
                    ? methodParameters[i].ParameterType.GetGenericArguments()[0] : methodParameters[i].ParameterType;

                parameters[i] = Convert.ChangeType(Parameters[i].Value, parameterType);
            }

            var obj = method.Invoke(Context, parameters);

            for (int i = 0; i < Parameters.Count; i++)
            {
                Parameters[i].Value = parameters[i].ToString();
            }
            return obj;
        }

        /// <summary>
        /// 使用鍵來取得EntityObject
        /// </summary>
        /// <param name="keyValues">鍵和值</param>
        /// <returns>實體對象</returns>
        public object GetObjectByKey(Dictionary<string, object> keyValues)
        {
            if (string.IsNullOrEmpty(EntitySetName))
            {
                throw new ArgumentNullException("EntitySetName");
            }
            if (keyValues == null)
            {
                throw new ArgumentNullException("keyValues");
            }
            var entityKey = new EntityKey(string.Format("{0}.{1}", Context.DefaultContainerName, EntitySetName), keyValues);
            object obj = Context.GetObjectByKey(entityKey);

            if (obj != null)
            {
                foreach (var item in ForeignKeyRelations)
                {
                    if(string.IsNullOrEmpty(item.ObjectName))
                    {
                        throw new ArgumentNullException("item.ObjectName");
                    }
                    (obj as EntityObject).LoadReference(item.ObjectName);
                    //var property = obj.GetType().GetProperty(item.ObjectName);
                    //if (property == null)
                    //{
                    //    throw new MissingMemberException(obj.GetType().Name, item.ObjectName);
                    //}
                    //if (!typeof(IRelatedEnd).IsAssignableFrom(property.PropertyType))
                    //{
                    //    property = obj.GetType().GetProperty(string.Format("{0}Reference", item.ObjectName));
                    //}
                    //if (property != null && typeof(IRelatedEnd).IsAssignableFrom(property.PropertyType))
                    //{
                    //    var relatedEnd = (IRelatedEnd)property.GetValue(obj, null);
                    //    if (relatedEnd != null)
                    //    {
                    //        relatedEnd.Load();
                    //    }
                    //}
                }
            }
            return obj;
        }

        private string _database;
        /// <summary>
        /// 取得或設定資料庫別名
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.DataBase)]
        [Editor(typeof(DatabaseEditor), typeof(UITypeEditor))]
        public string DataBase
        {
            get
            {
                return _database;
            }
            set
            {
                _database = value;
            }
        }

        private string _metadataFile;
        /// <summary>
        /// 取得或設置元Metadata文件名
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.MetadataFile)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.METADATA)]
        public string MetadataFile
        {
            get
            {
                return _metadataFile;
            }
            set
            {
                _metadataFile = value;
            }
        }

        /// <summary>
        /// 取得Context名字
        /// </summary>
        [Browsable(false)]
        public string ContextName
        {
            get
            {
                if (this.CommandType == System.Data.CommandType.Text)
                {
                    return EntityProvider.GetContainerName(this.CommandText);
                }
                else if (this.CommandType == System.Data.CommandType.StoredProcedure)
                {
                    return string.IsNullOrEmpty(this.CommandText) ? string.Empty : this.CommandText.Split('.')[0];
                }
                else
                {
                    throw new NotSupportedException(string.Format("CommandType:{0} not supported.", this.CommandType));
                }
            }
        }

        /// <summary>
        /// 取得entity set的名字
        /// </summary>
        [Browsable(false)]
        public string EntitySetName
        {
            get
            {
                if (this.CommandType == System.Data.CommandType.Text)
                {
                    return EntityProvider.GetEntitySetName(this.CommandText);
                }
                else if (this.CommandType == System.Data.CommandType.StoredProcedure)
                {
                    return StoreProcedureEntitySet;
                }
                else
                {
                    throw new NotSupportedException(string.Format("CommandType:{0} not supported.", this.CommandType));
                }
            }
        }

        private string _storeProcedureEntitySet;
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.ENTITYSET)]
        public string StoreProcedureEntitySet
        {
            get 
            {
                return _storeProcedureEntitySet;
            }
            set
            {
                _storeProcedureEntitySet = value;
            }
        }

#warning 备用
        private bool _useSystemDB;
        /// <summary>
        /// 取得或設置是否使用系統資料庫
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.UseSystemDB)]
        public bool UseSystemDB
        {
            get
            {
                return _useSystemDB;
            }
            set
            {
                _useSystemDB = value;
            }
        }
        #endregion

        #region Properties

       

        private int _commandTimeout = 30;
        /// <summary>
        /// 取得或設置執行Command的超時時間，預設值為30
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.CommandTimeout)]
        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

#warning 暂时不用
        private bool _multiSetWhere;
        /// <summary>
        /// Gets or sets a value indicating style of apply where.
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.MultiSetWhere)]
        public bool MultiSetWhere
        {
            get 
            { 
                return _multiSetWhere;
            }
            set 
            { 
                _multiSetWhere = value; 
            }
        }

        private SecurityStyle _secStyle;
        /// <summary>
        /// 取得或設置EFCommand的安全類型
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.SecStyle)]
        public SecurityStyle SecStyle
        {
            get 
            { 
                return _secStyle;
            }
            set 
            {
                _secStyle = value;
            }
        }

        private string _secFieldName;
        /// <summary>
        /// 取得或設置EFCommand的安全欄位
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.SecFieldName)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string SecFieldName
        {
            get 
            { 
                return _secFieldName;
            }
            set 
            { 
                _secFieldName = value;
            }
        }

        private string _secExcept;
        /// <summary>
        /// 取得或設置EFCommand的安全例外
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.SecExcept)]
        public string SecExcept
        {
            get 
            { 
                return _secExcept;
            }
            set 
            { 
                _secExcept = value;
            }
        }

#warning 此属性待定
        private int _selectTop;
        /// <summary>
        /// 取得或設定獲取資料的筆數
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.SelectTop)]
        public int SelectTop
        {
            get 
            { 
                return _selectTop;
            }
            set 
            { 
                _selectTop = value;
            }
        }

        private EFCollection<IncludeObject> _foreignKeyRelations;
        /// <summary>
        /// 取得或設定外鍵關係
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.ForeignKeyRelations)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EFCollection<IncludeObject> ForeignKeyRelations
        {
            get 
            { 
                return _foreignKeyRelations;
            }
            set 
            { 
                _foreignKeyRelations = value;
            }
        }


        private EFCollection<EFParameter> _parameters;
        /// <summary>
        /// 取得或設定存诸过程的参数
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.Parameters)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EFCollection<EFParameter> Parameters
        {
            get
            {
                return _parameters;
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// 根據安全類型過濾資料
        /// </summary>
        /// <param name="query">對象集的ObjectQuery</param>
        /// <param name="clientInfo">客戶端資訊</param>
        /// <returns>過濾完的資料</returns>
        private ObjectQuery<EntityObject> FilterObjects(ObjectQuery<EntityObject> query, ClientInfo clientInfo)
        {
            if (SecStyle == SecurityStyle.None)
            {
                return query;
            }
            else
            {
                var listWhereParameter = new List<WhereParameter>();
                if (string.IsNullOrEmpty(SecFieldName))
                {
                    throw new ArgumentNullException("SecFieldName");
                }

                List<string> secExceptValues = new List<string>();
                if (!string.IsNullOrEmpty(SecExcept))
                {
                    secExceptValues.AddRange(SecExcept.Split(';'));
                }

                if (SecStyle == SecurityStyle.ByUser)
                {
                    var userID = clientInfo.UserID;
                    if (!string.IsNullOrEmpty(userID) && !secExceptValues.Contains(userID))
                    {
                        listWhereParameter.Add(new WhereParameter() { Field = SecFieldName, Value = userID });
                    }
                }
                else if (SecStyle == SecurityStyle.BySite)
                {
                    var site = clientInfo.Site;
                    if (!string.IsNullOrEmpty(site))
                    {
                        listWhereParameter.Add(new WhereParameter() { Field = SecFieldName, Value = site });
                    }
                }
                else
                {
                    var listValues = new List<string>();

                    if (SecStyle == SecurityStyle.ByGroup)
                    {
                        listValues.AddRange(clientInfo.Groups
                            .Where(c => c.Type == GroupType.Normal).Select(c => c.ID));
                    }
                    else if (SecStyle == SecurityStyle.ByRole)
                    {
                        listValues.AddRange(clientInfo.Groups
                            .Where(c => c.Type == GroupType.Role).Select(c => c.ID));
                    }
                    else if (SecStyle == SecurityStyle.ByOrg)
                    {
                        listValues.AddRange(clientInfo.Groups
                            .Where(c => c.Type == GroupType.Role || c.Type == GroupType.Org).Select(c => c.ID));
                    }
                    else if (SecStyle == SecurityStyle.ByOrgShare)
                    {
                        listValues.AddRange(clientInfo.Groups
                            .Where(c => c.Type == GroupType.Role || c.Type == GroupType.Org || c.Type == GroupType.OrgShare).Select(c => c.ID));
                    }

                    bool isExcept = false;
                    foreach (string value in listValues)
                    {
                        if (secExceptValues.Contains(value))
                        {
                            isExcept = true;
                        }
                    }

                    if (!isExcept)
                    {
                        if (listValues.Count > 0)
                        {
                            listWhereParameter.Add(new WhereParameter() { Field = SecFieldName, Condition = WhereCondition.In, Value = listValues });
                        }
                    }
                }
                return EntityProvider.SetWhere(query, listWhereParameter);
            }
        }
        #endregion
    }

    /// <summary>
    /// 外鍵對象
    /// </summary>
    public class IncludeObject: EFCollectionItem
    {

        #region Properties
        private string _objectName;
        /// <summary>
        /// 取得或設置外鍵對象名
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFCommandInfo.ObjectName)]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.NAVIGATION_PROPERTY)]
        public string ObjectName
        {
            get 
            {
                return _objectName;
            }
            set 
            { 
                _objectName = value;
            }
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return _objectName;
        }
        #endregion
    }

    public class EFParameter : EFCollectionItem
    {
        private object _value;
       
        [Category("Infolight")]
        [Description("aaa")]
        [Editor(typeof(Design.EFCommandDesign.EFParameterEditor), typeof(UITypeEditor))]
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }


    /// <summary>
    /// 安全的類型
    /// </summary>
    public enum SecurityStyle
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// By user
        /// </summary>
        ByUser,
        /// <summary>
        /// By group
        /// </summary>
        ByGroup,
        /// <summary>
        /// By role
        /// </summary>
        ByRole,
        /// <summary>
        /// By site
        /// </summary>
        BySite,
        /// <summary>
        /// By org
        /// </summary>
        ByOrg,
        /// <summary>
        /// By org share
        /// </summary>
        ByOrgShare
    };
}
