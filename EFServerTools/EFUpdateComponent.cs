using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EFWCFModule;
using System.Drawing;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Collections;
using System.Reflection;
using EFServerTools.Design;
using System.Drawing.Design;
using EFBase;
using EFServerTools.Common;

namespace EFServerTools
{
#warning detail 会随Master一起新增和修改，事件会有问题
    /// <summary>
    /// EFUpdateComponent
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(EFUpdateComponent), ICOInfo.EFUpdateComponent)]
    public class EFUpdateComponent : Component, IUpdateComponent
    {

        #region Constructor
        /// <summary>
        /// Creates a new instance of EFUpdateComponent
        /// </summary>
        public EFUpdateComponent()
        {
            InitializeComponent();
            _fields = new EFCollection<FieldItem>(this);
        }

        /// <summary>
        /// Creates a new instance of EFUpdateComponent and adds the EFCommand to the specified container
        /// </summary>
        /// <param name="container">IContainer to add the current EFUpdateComponent to</param>
        public EFUpdateComponent(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() { }

        #endregion

        #region IUseCommand Members
        private ICommand _command;
        /// <summary>
        /// Gets or sets command of component
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description("The EFCommand which the control is bound to")]
        public ICommand Command
        {
            get
            {
                return _command;
            }
            set
            {
                _command = value;
            }
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

        #region IUpdateComponent Members

        public int Update(List<EntityObject> objects, Dictionary<EntityKey, EntityState> states, string masterEntitySetName)
        {
            if (objects == null)
            {
                throw new ArgumentNullException("objects");
            }
            int resCount = 0;

            List<EntityObject> addedDetails = new List<EntityObject>();
            EntityObject masterObject = null;
            if (!string.IsNullOrEmpty(masterEntitySetName))
            {
                foreach (var obj in objects)
                {
                    if (obj.EntityKey == null)
                    {
                        addedDetails.Add(obj);
                    }
                }
                foreach (var obj in addedDetails)
                {
                    if (masterObject == null)
                    {
                        var masterRelatedEnd = EntityProvider.GetRelatedEnd(Context, obj, masterEntitySetName);
                        if (masterRelatedEnd is EntityReference)
                        {
                            masterObject = (EntityObject)(masterRelatedEnd as EntityReference).GetType().GetProperty("Value").GetValue(masterRelatedEnd, null);
                        }
                    }
                    if (masterObject != null)
                    {
                        DoEntityCollectionFunction(masterObject, obj, EntityCollectionFunction.Remove);
                    }
                }
                if (masterObject != null)
                {
                    //if (masterObject.EntityState == EntityState.Detached)
                    //{
                    //    this.Context.Attach(masterObject);
                    //}
                    masterObject = (EntityObject)this.Context.GetObjectByKey(masterObject.EntityKey);
                }
            }

            foreach (var obj in objects)
            {
                if (obj.EntityKey != null)
                {
                    if (obj.EntityState == EntityState.Added)//detail 会随Master一起新增和修改
                    {
                        OnBeforeInsert(new EFUpdateComponentUpdateEventArgs(obj));
                        ApplyFieldAttribute(obj, obj.EntityState);
                        this.Context.AttachUpdated(obj);
                        OnAfterInsert(new EFUpdateComponentUpdateEventArgs(obj));
                    }
                    else if (obj.EntityState == EntityState.Modified)
                    {
                        OnBeforeModify(new EFUpdateComponentUpdateEventArgs(obj));
                        this.Context.AttachUpdated(obj);
                        OnAfterModify(new EFUpdateComponentUpdateEventArgs(obj));
                    }
                    else
                    {
                        if (states.ContainsKey(obj.EntityKey))
                        {
                            if (states[obj.EntityKey] == EntityState.Modified)
                            {
                                OnBeforeModify(new EFUpdateComponentUpdateEventArgs(obj));
                            }
                            else if (states[obj.EntityKey] == EntityState.Deleted)
                            {
                                OnBeforeDelete(new EFUpdateComponentUpdateEventArgs(obj));
                            }

                            Update(obj, states[obj.EntityKey], masterEntitySetName);

                            if (states[obj.EntityKey] == EntityState.Modified)
                            {
                                OnAfterModify(new EFUpdateComponentUpdateEventArgs(obj));
                            }
                            else if (states[obj.EntityKey] == EntityState.Deleted)
                            {
                                OnAfterDelete(new EFUpdateComponentUpdateEventArgs(obj));
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(masterEntitySetName))
                    {
                        OnBeforeInsert(new EFUpdateComponentUpdateEventArgs(obj));
                        Update(obj, EntityState.Added, masterEntitySetName);
                        OnAfterInsert(new EFUpdateComponentUpdateEventArgs(obj));
                    }
                }
            }
            if (!string.IsNullOrEmpty(masterEntitySetName))
            {
                
                foreach (var obj in addedDetails)
                {
                    if (masterObject != null)
                    {
                        OnBeforeInsert(new EFUpdateComponentUpdateEventArgs(obj));
                        DoEntityCollectionFunction(masterObject, obj, EntityCollectionFunction.Add);
                        ProcessForeignKey(obj, EntityState.Added);
                        OnAfterInsert(new EFUpdateComponentUpdateEventArgs(obj));
                    }
                }
            }
            return resCount;
        }

        #endregion

        #region Properties
        private EFCollection<FieldItem> _fields;
        /// <summary>
        /// Gets or sets fields
        /// </summary>
        [Category(ComponentInfo.COMPANY),
        Description(EFUpdateComponentInfo.Fields)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EFCollection<FieldItem> Fields
        {
            get
            {
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }

        //private Boolean _serverModify;
        ///// <summary>
        ///// Gets or sets whether server transfers the lastest date to client automatically after the data in database changes
        ///// </summary>
        //[Category("Infolight"),
        //Description("Indicates whether server transfers the lastest date to client automatically after the data in database changes")]
        //public Boolean ServerModify
        //{
        //    get { return _serverModify; }
        //    set { _serverModify = value; }
        //}

        //private bool _serverModifyGetMax;
        ///// <summary>
        ///// 
        ///// </summary>
        //[Category("Infolight"),
        //Description("Indicates whether server get the the data having the max value")]
        //public bool ServerModifyGetMax
        //{
        //    get { return _serverModifyGetMax; }
        //    set { _serverModifyGetMax = value; }
        //}

        //private IsolationLevel _transIsolationLevel = IsolationLevel.Unspecified;
        //[Category("Infolight"),
        //Description("Specifies isolation level")]
        //public IsolationLevel TransIsolationLevel
        //{
        //    get { return _transIsolationLevel; }
        //    set { _transIsolationLevel = value; }
        //}
        #endregion

        #region Methods
        /// <summary>
        /// Updates entity object to database
        /// </summary>
        /// <param name="entityObject">Entity object</param>
        /// <param name="state">State of entity objects</param>
        /// <param name="masterEntitySetName">Name of master entity set</param>
        private void Update(EntityObject entityObject, EntityState state, string masterEntitySetName)
        {
            if (entityObject == null)
            {
                throw new ArgumentNullException("entityObject");
            }
            switch (state)
            {
                case EntityState.Added:
                    {
                        ApplyFieldAttribute(entityObject, state);
                        DoAction(entityObject, EntityState.Added, masterEntitySetName);
                        break;
                    }
                case EntityState.Modified:
                    {
                        ApplyFieldAttribute(entityObject, state);
                        DoAction(entityObject, EntityState.Modified, masterEntitySetName);
                        break;
                    }
                case EntityState.Deleted:
                    {
                        DoAction(entityObject, EntityState.Deleted, masterEntitySetName);
                        break;
                    }
                default: break;
            }
        }

        /// <summary>
        /// Applies field attribute to object
        /// </summary>
        /// <param name="entityObject">Entity object</param>
        /// <param name="state">State of entity objects</param>
        private void ApplyFieldAttribute(EntityObject entityObject, EntityState state)
        {
            if (entityObject == null)
            {
                throw new ArgumentNullException("entityObject");
            }
            if (state != EntityState.Added && state != EntityState.Modified)
            {
                throw new ArgumentOutOfRangeException("state", string.Format("State:{0} can not apply field attribute.", state));
            }
            foreach (var field in Fields)
            {
                if (string.IsNullOrEmpty(field.FieldName))
                {
                    throw new ArgumentNullException("field.FieldName");
                }

                if (state == EntityState.Added)//check null, default value
                {
                    if (!string.IsNullOrEmpty(field.DefaultValue))
                    {
                        var value = field.DefaultValue;
                        entityObject.SetValue(field.FieldName, value);
                    }
                    if (field.CheckNull)
                    {
                        var value = entityObject.GetValue(field.FieldName);
                        if (value == null)
                        {
                            throw new NoNullAllowedException(string.Format("Value of field:{0} can not be null.", field.FieldName));
                        }
                    }
                }
                if (!field.UpdateEnable)// update enable
                {
                    if (state == EntityState.Added)
                    {
                        entityObject.SetValue(field.FieldName, null);
                    }
                    else if (state == EntityState.Modified)
                    {
                        var entityKey = entityObject.EntityKey;
                        if (entityKey == null)
                        {
                            throw new EntityException("Entity key is null.");
                        }
                        object objectInDatabase = null;
                        if (Context.TryGetObjectByKey(entityKey, out objectInDatabase))
                        {
                            entityObject.SetValue(field.FieldName, ((EntityObject)objectInDatabase).GetValue(field.FieldName));
                        }
                        else
                        {
                            throw new ObjectNotFoundException(string.Format("Object:{0} not found.", entityKey));
                        }
                    }
                }

                if (field.TrimLength > 0) //trim length
                {
                    var value = entityObject.GetValue(field.FieldName);
                    if (value is string)
                    {
                        var strValue = (string)value;
                        if (!string.IsNullOrEmpty(strValue) && strValue.Length > field.TrimLength)
                        {
                            value = strValue.Substring(0, field.TrimLength);
                            entityObject.SetValue(field.FieldName, strValue);
                        }
                    }
                }
            }
        }

        private EntityObject DoAction(EntityObject entityObject, EntityState state, string masterEntitySetName)
        {
            switch (state)
            {
                case EntityState.Added:
                    if (!string.IsNullOrEmpty(masterEntitySetName))
                    {
                        //var masterRelatedEnd = EntityProvider.GetRelatedEnd(Context, entityObject, masterEntitySetName);
                        //if (masterRelatedEnd is EntityReference)
                        //{
                        //    var masterObject = (EntityObject)(masterRelatedEnd as EntityReference).GetType().GetProperty("Value").GetValue(masterRelatedEnd, null);

                        //    //try
                        //    //{
                        //    //    masterObject = (EntityObject)this.Context.GetObjectByKey(masterObject.EntityKey);
                        //    //    this.Context.Detach(masterObject);
                        //    //}
                        //    //catch (ObjectNotFoundException)
                        //    //{

                        //    //}

                        //    DoEntityCollectionFunction(masterObject, entityObject, EntityCollectionFunction.Remove);
                        //    this.Context.Attach(masterObject);
                        //    DoEntityCollectionFunction(masterObject, entityObject, EntityCollectionFunction.Add);

                        //    ProcessForeignKey(entityObject, state);

                        //}
                    }
                    else // 第一阶 Master
                    {
                        this.Context.AddObject(this.Command.EntitySetName, entityObject);
                    }
                    break;
                case EntityState.Deleted:
#warning M_D不能刪除
#warning 删除2笔有问题,第一笔删掉,第二笔删不掉
                    //try
                    //{
                    //    entityObject = (EntityObject)this.Context.GetObjectByKey(entityObject.EntityKey);
                    //}
                    //catch (ObjectNotFoundException)
                    //{

                    //}
                    if (entityObject.EntityState == EntityState.Detached)
                    {
                        if (!string.IsNullOrEmpty(masterEntitySetName))
                        {
                            entityObject = (EntityObject)this.Context.GetObjectByKey(entityObject.EntityKey);
                        }
                        else
                        {




                            ProcessForeignKey(entityObject, EntityState.Deleted);

                            this.Context.Attach(entityObject);
                        }
                    }
                    this.Context.DeleteObject(entityObject);
                    break;
                case EntityState.Detached:
                    break;
                case EntityState.Modified:
                    ProcessForeignKey(entityObject, state);
                    this.Context.AttachUpdated(entityObject);
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    break;
            }

            return entityObject;
        }

        private void ProcessForeignKey(EntityObject entityObject, EntityState state)
        {
            #region 解决外键的问题
            foreach (var relatedEnd in ((IEntityWithRelationships)entityObject).RelationshipManager.GetAllRelatedEnds())
            {
                if (relatedEnd is EntityReference)
                {
                    var reference = relatedEnd as EntityReference;

                    EntityObject refObject = null;

                    if (reference.GetType().GetProperty("Value").GetValue(reference, null) == null)
                    {
                        if (reference.EntityKey != null)
                        {
                            try
                            {
                                this.Context.GetObjectByKey(reference.EntityKey);
                            }
                            catch (ObjectNotFoundException)
                            {
                                refObject = (EntityObject)this.Context.GetType().Assembly.GetTypes().FirstOrDefault(c => c.Name == reference.TargetRoleName).GetConstructors()[0].Invoke(null);
                                refObject.EntityKey = this.Context.CreateEntityKey(reference.TargetRoleName, refObject);
                                reference.GetType().GetProperty("Value").SetValue(reference, refObject, null);
                            }

                            if (state == EntityState.Modified)
                            {
                                reference.EntityKey = null;
                            }
                        }
                    }
                    else
                    {
                        if (state == EntityState.Modified)
                        {
                            reference.EntityKey = null;
                        }
                        else if (state == EntityState.Deleted)
                        {
                            //清除外键，不然删除时会报错
                            reference.GetType().GetProperty("Value").SetValue(reference, null, null);
                        }
                    }

                }
            }
            #endregion
        }

        private void DoEntityCollectionFunction(EntityObject masterObject, EntityObject detailObject, EntityCollectionFunction functionName)
        {
            PropertyInfo detailObjectPropertyInfo = masterObject.GetType().GetProperty(this.Command.EntitySetName);
            detailObjectPropertyInfo.PropertyType.GetMethod(functionName.ToString()).Invoke(detailObjectPropertyInfo.GetValue(masterObject, null), new object[] { detailObject });
        }

        //public void UpdateOptimisticConcurrency(EntityObject detachedObject, EntityObject originalObject)
        //{
        //    try
        //    {
        //        //(CDLTLL) Original entity must first be reunited with a Context 
        //        this.Context.Attach(originalObject);

        //        //(CDLTLL)Apply property changes to all referenced entities in context                
        //        this.Context.ApplyReferencePropertyChanges(detachedObject, originalObject);

        //        // Apply entity properties changes to the context 
        //        this.Context.ApplyPropertyChanges(detachedObject.EntityKey.EntitySetName,  
        //                                                           detachedObject);
        //        //(CDLTLL) EF SaveChanges() persists all updates to the store and 
        //        // resets change tracking in the object context.                
        //        this.Context.SaveChanges();
        //    }
        //    catch (OptimisticConcurrencyException e)
        //    {
        //        //(CDLTLL) Concurrency Exception Management 
        //        //context.Refresh(RefreshMode.ClientWins, newCustomer); // Last in wins 
        //        //context.SaveChanges(); 
        //        //Manage or throw the exception

        //        throw (e);

        //    }
        //}

        #endregion

        #region EFUpdateComponent's events

        protected void OnBeforeApply(EFUpdateComponentUpdateEventArgs value)
        {
            EFUpdateComponentUpdateEventHandler handler = (EFUpdateComponentUpdateEventHandler)Events[EventBeforeApply];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EFUpdateComponentUpdateEventHandler BeforeApply
        {
            add { Events.AddHandler(EventBeforeApply, value); }
            remove { Events.RemoveHandler(EventBeforeApply, value); }
        }

        protected void OnAfterApply(EFUpdateComponentUpdateEventArgs value)
        {
            EFUpdateComponentUpdateEventHandler handler = (EFUpdateComponentUpdateEventHandler)Events[EventAfterApply];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EFUpdateComponentUpdateEventHandler AfterApply
        {
            add { Events.AddHandler(EventAfterApply, value); }
            remove { Events.RemoveHandler(EventAfterApply, value); }
        }

        protected void OnBeforeInsert(EFUpdateComponentUpdateEventArgs value)
        {
            EFUpdateComponentUpdateEventHandler handler = (EFUpdateComponentUpdateEventHandler)Events[EventBeforeInsert];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EFUpdateComponentUpdateEventHandler BeforeInsert
        {
            add { Events.AddHandler(EventBeforeInsert, value); }
            remove { Events.RemoveHandler(EventBeforeInsert, value); }
        }

        protected void OnAfterInsert(EFUpdateComponentUpdateEventArgs value)
        {
            EFUpdateComponentUpdateEventHandler handler = (EFUpdateComponentUpdateEventHandler)Events[EventAfterInsert];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EFUpdateComponentUpdateEventHandler AfterInsert
        {
            add { Events.AddHandler(EventAfterInsert, value); }
            remove { Events.RemoveHandler(EventAfterInsert, value); }
        }


        protected void OnBeforeDelete(EFUpdateComponentUpdateEventArgs value)
        {
            EFUpdateComponentUpdateEventHandler handler = (EFUpdateComponentUpdateEventHandler)Events[EventBeforeDelete];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EFUpdateComponentUpdateEventHandler BeforeDelete
        {
            add { Events.AddHandler(EventBeforeDelete, value); }
            remove { Events.RemoveHandler(EventBeforeDelete, value); }
        }

        protected void OnAfterDelete(EFUpdateComponentUpdateEventArgs value)
        {
            EFUpdateComponentUpdateEventHandler handler = (EFUpdateComponentUpdateEventHandler)Events[EventAfterDelete];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EFUpdateComponentUpdateEventHandler AfterDelete
        {
            add { Events.AddHandler(EventAfterDelete, value); }
            remove { Events.RemoveHandler(EventAfterDelete, value); }
        }


        protected void OnBeforeModify(EFUpdateComponentUpdateEventArgs value)
        {
            EFUpdateComponentUpdateEventHandler handler = (EFUpdateComponentUpdateEventHandler)Events[EventBeforeModify];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EFUpdateComponentUpdateEventHandler BeforeModify
        {
            add { Events.AddHandler(EventBeforeModify, value); }
            remove { Events.RemoveHandler(EventBeforeModify, value); }
        }

        protected void OnAfterModify(EFUpdateComponentUpdateEventArgs value)
        {
            EFUpdateComponentUpdateEventHandler handler = (EFUpdateComponentUpdateEventHandler)Events[EventAfterModify];
            if (handler != null)
            {
                handler(this, value);
            }
        }

        public event EFUpdateComponentUpdateEventHandler AfterModify
        {
            add { Events.AddHandler(EventAfterModify, value); }
            remove { Events.RemoveHandler(EventAfterModify, value); }
        }

        #endregion

        #region Variable
        internal static readonly object EventBeforeApply = new object();
        internal static readonly object EventAfterApply = new object();
        internal static readonly object EventBeforeInsert = new object();
        internal static readonly object EventAfterInsert = new object();
        internal static readonly object EventBeforeDelete = new object();
        internal static readonly object EventAfterDelete = new object();
        internal static readonly object EventBeforeModify = new object();
        internal static readonly object EventAfterModify = new object();
        #endregion
    }

    public enum EntityCollectionFunction
    {
        Add,
        Remove
    }

    public class FieldItem : EFCollectionItem
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

        private string _defaultValue;
        [Category("Infolight")]
        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
            }
        }

#warning 备用
        private bool _updateEnable = true;
        [Category("Infolight")]
        public bool UpdateEnable
        {
            get
            {
                return _updateEnable;
            }
            set
            {
                _updateEnable = value;
            }
        }

        private bool _checkNull;
        [Category("Infolight")]
        public bool CheckNull
        {
            get
            {
                return _checkNull;
            }
            set
            {
                _checkNull = value;
            }
        }

        private int _trimLength;
        [Category("Infolight")]
        public int TrimLength
        {
            get
            {
                return _trimLength;
            }
            set
            {
                _trimLength = value;
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

    #region EFUpdateComponent's Events

    public delegate void EFUpdateComponentUpdateEventHandler(object sender, EFUpdateComponentUpdateEventArgs e);

    public sealed class EFUpdateComponentUpdateEventArgs : EventArgs
    {
        public EFUpdateComponentUpdateEventArgs(EntityObject obj)
        {
            _object = obj;
        }

        private EntityObject _object;
        public EntityObject Object 
        {
            get
            {
                return _object;
            }
            set
            {
                _object = value;
            }
        }
    }

    #endregion
}
