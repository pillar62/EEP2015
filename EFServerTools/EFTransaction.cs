using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EFWCFModule;
using EFServerTools.Design;
using System.Drawing.Design;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data;
using EFBase;
using System.ComponentModel.Design;
using EFServerTools.Design.EFTransactionDesign;
using EFServerTools.Common;
using System.Reflection;
using System.Drawing;

namespace EFServerTools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(EFTransaction), ICOInfo.EFTransaction)]
    [Designer(typeof(EFTransactionEditor), typeof(IDesigner))]
    public class EFTransaction : Component, ITransaction
    {
        const string MinusSign = "-";
        const string PlusSign = "+";

        #region Constructor
        public EFTransaction(System.ComponentModel.IContainer container)
            : this()
        {
            container.Add(this);
        }

        public EFTransaction()
        {
            _transactions = new EFCollection<Transaction>(this);
        }
        #endregion

        #region IEFComponent Members

        private ObjectContext _context;
        /// <summary>
        /// Gets or sets object context of component
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObjectContext Context
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
        [Description(EFTransactionInfo.UpdateComponent)]
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
        private EFCollection<Transaction> _transactions;
        [Category(ComponentInfo.COMPANY),
        Description(EFTransactionInfo.Transactions)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EFCollection<Transaction> Transactions
        {
            set { _transactions = value; }
            get { return _transactions; }
        }
        #endregion

        #region Events

        protected void OnBeforeTrans(EFTransactionBeforeTransEventArgs value)
        {
            EFTransactionBeforeTransEventHandler handler = (EFTransactionBeforeTransEventHandler)Events[EventBeforeTrans];
            if ((handler != null) && (value is EFTransactionBeforeTransEventArgs))
            {
                handler(this, (EFTransactionBeforeTransEventArgs)value);
            }
        }

        public event EFTransactionBeforeTransEventHandler BeforeTrans
        {
            add { Events.AddHandler(EventBeforeTrans, value); }
            remove { Events.RemoveHandler(EventBeforeTrans, value); }
        }

        protected void OnAfterTrans(EFTransactionAfterTransEventArgs value)
        {
            EFTransactionAfterTransEventHandler handler = (EFTransactionAfterTransEventHandler)Events[EventAfterTrans];
            if ((handler != null) && (value is EFTransactionAfterTransEventArgs))
            {
                handler(this, (EFTransactionAfterTransEventArgs)value);
            }
        }

        public event EFTransactionAfterTransEventHandler AfterTrans
        {
            add { Events.AddHandler(EventAfterTrans, value); }
            remove { Events.RemoveHandler(EventAfterTrans, value); }
        }

        #endregion

        #region Variables
        internal static readonly object EventBeforeTrans = new object();
        internal static readonly object EventAfterTrans = new object();
        #endregion

        #region ITransaction Members

        public void Execute(List<System.Data.Objects.DataClasses.EntityObject> objects, Dictionary<System.Data.EntityKey, System.Data.EntityState> states)
        {
            foreach (EntityObject entityObject in objects)
            {
                foreach (Transaction transaction in _transactions)
                {
                    EntityState currentState = EntityState.Detached;

                    #region Get Current Object State
                    if (states.ContainsKey(entityObject.EntityKey))
                    {
                        if (states[entityObject.EntityKey] == System.Data.EntityState.Modified)
                        {
                            if (transaction.WhenUpdate)
                            {
                                currentState = EntityState.Modified;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (transaction.WhenDelete)
                            {
                                currentState = EntityState.Deleted;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
#warning detail的新增状态判断
                        if ((entityObject.EntityKey == null || (entityObject.EntityKey != null && entityObject.EntityState == EntityState.Added)) && transaction.WhenInsert)
                        {
                            currentState = EntityState.Added;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    #endregion

                    transaction.CurrentState = currentState;

                    transaction.CurrentObject = entityObject;

                    #region Before Transaction Event
                    EFTransactionBeforeTransEventArgs beforeArgs = new EFTransactionBeforeTransEventArgs(transaction);
                    OnBeforeTrans(beforeArgs);

                    if (beforeArgs.Cancel)
                    {
                        continue;
                    }
                    #endregion

                    if (CheckFieldsValidate(transaction))
                    {
                        #region Logic of transaction
                        List<TransKeyField> transKeyFields = this.GetTransKeyFieldsList(transaction, entityObject, currentState);
                        List<TransField> transFields = this.GetTransFieldList(transaction, entityObject, currentState);
                        bool hasRow = false;

                        switch (currentState)
                        {
                            case EntityState.Added:
                            case EntityState.Deleted:
                            case EntityState.Modified:
                                switch (transaction.TransMode)
                                {
                                    case TransMode.AlwaysAppend:
                                        #region AlwaysAppend
                                        if (currentState == EntityState.Modified)
                                        {
                                            this.AddObject(transaction, entityObject, currentState, transKeyFields, transFields, true, true, true);
                                        }
                                        this.AddObject(transaction, entityObject, currentState, transKeyFields, transFields, false, false, false);
                                        #endregion
                                        break;
                                    case TransMode.Exception:
                                    case TransMode.AutoAppend:
                                    case TransMode.Ignore:
                                        #region AutoAppend && Exception
                                        bool keyChanged = false;
                                        // 只有在Modified的情况下才有可能TransKeyFields被改变。
                                        if (currentState == EntityState.Modified)
                                        {
                                            keyChanged = this.CheckTransKeyFieldsIsOrNotChanged(transKeyFields);
                                        }

                                        if (keyChanged)
                                        {
                                            // Modified：TransKeyFields有被改变，两笔记录。
                                            // 1、
                                            EntityObject oldDestinationObject = this.GetDestinationObject(transaction, transKeyFields, currentState, true);
                                            this.UpdateObject(oldDestinationObject, transFields, currentState, true, false);
                                        }

                                        // 2、
                                        EntityObject destinationObject = this.GetDestinationObject(transaction, transKeyFields, currentState, false);
                                        if (destinationObject != null)
                                        {
                                            hasRow = true;
                                        }

                                        if (hasRow)
                                        {
                                            if (keyChanged)
                                            {
                                                UpdateObject(destinationObject, transFields, currentState, true, true);
                                            }
                                            else
                                            {
                                                UpdateObject(destinationObject, transFields, currentState, false, false);
                                            }

                                            #region Write back
                                            bool writeBack = this.CheckWriteBack(transaction.TransFields);
                                            if (writeBack)
                                            {
                                                if (currentState != EntityState.Deleted)
                                                {
                                                    this.WriteBack(transaction, entityObject, destinationObject, transaction.TransFields);
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            switch (transaction.TransMode)
                                            {
                                                case TransMode.AutoAppend:
                                                    if (currentState != EntityState.Deleted)
                                                    {
                                                        this.AddObject(transaction, entityObject, currentState, transKeyFields, transFields, false, false, false);
                                                    }
                                                    else
                                                    {
                                                        this.AddObject(transaction, entityObject, currentState, transKeyFields, transFields, true, true, true);
                                                    }
                                                    break;
                                                case TransMode.Exception:
                                                    throw new ArgumentException(MessageHelper.EFTransactionMessage.NotExistRowInTable);
                                                case TransMode.Ignore:
                                                    break;
                                            }
                                        }
                                        #endregion
                                        break;
                                }


                                break;
                        }
                        #endregion
                    }
                }

                #region After Transaction Event
                OnAfterTrans(new EFTransactionAfterTransEventArgs());
                #endregion
            }
        }

        public object GetDestinationColumnValue(Transaction transaction, string columnName)
        {
            EntityObject entityObject = GetDestinationEntityObject(transaction);

            return entityObject.GetValue(columnName);
        }

        private EntityObject GetDestinationEntityObject(Transaction transaction)
        {
            List<TransKeyField> transKeyFields = this.GetTransKeyFieldsList(transaction, transaction.CurrentObject, transaction.CurrentState);
            return this.GetDestinationObject(transaction, transKeyFields, transaction.CurrentState, false);
        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private bool CheckFieldsValidate(Transaction transaction)
        {
            bool result = true;
            foreach (var keyField in transaction.TransKeyFields)
            {
                if (!CheckFieldValidate(transaction, keyField, false))
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                foreach (var field in transaction.TransFields)
                {
                    if (!CheckFieldValidate(transaction, field, field.UpdateMode == UpdateMode.WriteBack))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private bool CheckFieldValidate(Transaction transaction, TransFieldBase field, bool writeBack)
        {
            if (writeBack)
            {
                if (string.IsNullOrWhiteSpace(field.SrcField))
                {
                    throw new ArgumentNullException("SrcField");
                }

                if (string.IsNullOrWhiteSpace(field.DesField) && string.IsNullOrWhiteSpace(field.SrcGetValue))
                {
                    throw new ArgumentNullException("DesField or SrcGetValue");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(field.DesField))
                {
                    throw new ArgumentNullException("DesField");
                }

                if (string.IsNullOrWhiteSpace(field.SrcField) && string.IsNullOrWhiteSpace(field.SrcGetValue))
                {
                    throw new ArgumentNullException("SrcField or SrcGetValue");
                }
            }

            if (string.IsNullOrWhiteSpace(field.SrcField)) 
            {
                PropertyInfo sourceColumn = transaction.CurrentObject.GetType().GetProperty(field.SrcField);
                if (sourceColumn == null)
                {
                    throw new ArgumentException(string.Format(MessageHelper.EFTransactionMessage.ColumnNotInTable, field.SrcField, transaction.CurrentObject.EntityKey.EntitySetName));
                }
            }

            PropertyInfo destinationColumn = this.Context.CreateObject(transaction.TransTableName).GetType().GetProperty(field.DesField);
            if (destinationColumn == null)
            {
                throw new ArgumentException(string.Format(MessageHelper.EFTransactionMessage.ColumnNotInTable, field.DesField, transaction.TransTableName));
            }

            return true;
        }

        private bool CheckWriteBack(List<TransField> transFields)
        {
            bool result = false;
            foreach (var field in transFields)
            {
                if (field.UpdateMode == UpdateMode.WriteBack)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool CheckTransKeyFieldsIsOrNotChanged(List<TransKeyField> transKeyFields)
        {
            bool result = false;
            List<TransKeyField> changedKeyFields = new List<TransKeyField>();

            foreach (var keyField in transKeyFields)
            {
                string desField = keyField.DesField;
                string srcField = string.Empty;

                if (string.IsNullOrEmpty(keyField.SrcGetValue))
                {
                    srcField = keyField.SrcField;
                }

                if (keyField.OldValue == null && keyField.NewValue == null)
                {
                    changedKeyFields.Add(keyField);
                }
                else if (keyField.OldValue != null && keyField.NewValue != null
                    && !object.Equals(keyField.OldValue, keyField.NewValue))
                {
                    changedKeyFields.Add(keyField);
                }
            }

            if (changedKeyFields.Count != 0)
            {
                result = true;
            }

            return result;
        }

        private void WriteBack(Transaction transaction, EntityObject entityObject, EntityObject destinationEntitiyObject, List<TransField> transFields)
        {
            foreach (var field in transFields)
            {
                object desFieldValue = null;
                if (field.UpdateMode == UpdateMode.WriteBack)
                {
                    if (transaction.AutoNumber != null && string.Compare(field.DesField, transaction.AutoNumber.TargetColumn, true) == 0)
                    {
                        desFieldValue = transaction.AutoNumber.Number;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(field.DesField))
                        {
                            desFieldValue = this.GetFieldDefaultValue(field, entityObject);
                        }
                        else
                        {
                            desFieldValue = destinationEntitiyObject.GetValue(field.DesField);
                        }
                    }

                    entityObject.SetValue(field.SrcField, desFieldValue);
                }

            }
            this.Context.AttachUpdated(entityObject);
        }

        private string Operate(object oldValue, object currentValue, UpdateMode operation)
        {
            string resValue = string.Empty;
            decimal result = decimal.Zero;
            decimal oldValueDecimal = decimal.Zero;
            decimal currentValueDecimal = decimal.Zero;

            oldValueDecimal = oldValue == null ? decimal.Zero : decimal.Parse(oldValue.ToString());
            currentValueDecimal = currentValue == null ? decimal.Zero : decimal.Parse(currentValue.ToString());

            if (operation == UpdateMode.Increase)
            {
                result = oldValueDecimal + currentValueDecimal;
            }
            else
            {
                result = oldValueDecimal - currentValueDecimal;
            }

            resValue = result.ToString();

            return resValue;
        }

        private EntityObject GetDestinationObject(Transaction transaction, List<TransKeyField> transKeyFields, EntityState state, bool isGetOldKeyObject)
        {
            EntityObject resValue = null;

            string sql = string.Format("Select value c from {0}.{1} as c", Context.DefaultContainerName, transaction.TransTableName);
            ObjectQuery<EntityObject> query = this.Context.CreateQuery<EntityObject>(sql);
            var parameterPrefix = EntityProvider.GetParameterPrefix(query.Context);
            List<WhereParameter> listWhereParameter = new List<WhereParameter>();

            foreach (var keyField in transKeyFields)
            {
                if (keyField.WhereMode != WhereMode.InsertOnly)
                {
                    WhereParameter whereParameter = new WhereParameter();
                    whereParameter.And = true;
                    whereParameter.Field = keyField.DesField;
                    switch (state)
                    {
                        case EntityState.Added:
                            whereParameter.Value = keyField.NewValue;
                            break;
                        case EntityState.Deleted:
                            whereParameter.Value = keyField.OldValue;
                            break;
                        case EntityState.Modified:
                            whereParameter.Value = isGetOldKeyObject == true ? keyField.OldValue : keyField.NewValue;
                            break;
                    }
                    listWhereParameter.Add(whereParameter);
                }
            }

            query = EntityProvider.SetWhere(query, listWhereParameter);
            List<EntityObject> objects = query.ToList();
            if (objects.Count != 0)
            {
                resValue = objects[0];
            }
            return resValue;
        }

        private bool CheckValue(TransField field, object fieldValue)
        {
            bool result = false;

            if (field.DeleteForDecrease && decimal.Parse(fieldValue.ToString()) == decimal.Zero)
            {
                result = true;
            }

            if (field.ExceptionForMinus && decimal.Parse(fieldValue.ToString()) < decimal.Zero)
            {
                throw new ArgumentException(MessageHelper.EFTransactionMessage.ValueIsNegativeNumber);
            }

            return result;
        }

        private void UpdateObject(EntityObject entityObject, List<TransField> updateColumnsList, EntityState state, bool keyFieldsChanged, bool secondOperation)
        {
            var sqlBuidler = new StringBuilder();
            foreach (var field in updateColumnsList)
            {
                if (field.ReadOnly)
                {
                    throw new ArgumentException(MessageHelper.EFTransactionMessage.TransDesFieldIsReadOnly);
                }
                if (sqlBuidler.Length > 0)
                {
                    sqlBuidler.Append(",");
                }

                if (keyFieldsChanged)
                {
                    if (secondOperation)
                    {
                        field.OldValue = 0;
                    }
                    else
                    {
                        field.NewValue = 0;
                    }
                }
                string columnSql = string.Empty;
                switch (field.UpdateMode)
                {
                    case UpdateMode.Increase:
                        columnSql = string.Format("{0} = {0} + {1} - {2}", field.DesField, field.NewValue ?? 0, field.OldValue ?? 0);
                        break;
                    case UpdateMode.Decrease:
                        columnSql = string.Format("{0} = {0} - {1} + {2}", field.DesField, field.NewValue ?? 0, field.OldValue ?? 0);
                        break;
                    case UpdateMode.Replace:
                        if (field.NewValue == null || field.NewValue == DBNull.Value)
                        {
                            columnSql = string.Format("{0} = null", field.DesField);
                        }
                        else if (field.NewValue is string || field.NewValue is Guid)
                        {
                            columnSql = string.Format("{0} = '{1}'", field.DesField, field.NewValue);
                        }
                        else
                        {
                            columnSql = string.Format("{0} = {1}", field.DesField, field.NewValue);
                        }
                        break;
                }
                sqlBuidler.Append(columnSql);
            }
            var whereBuilder = new StringBuilder();
            foreach (var keyValues in entityObject.EntityKey.EntityKeyValues)
            {
                if (whereBuilder.Length > 0)
                {
                    whereBuilder.Append(" and ");
                }
                var key = keyValues.Key;
                var value = keyValues.Value;
                if (value is string || value is Guid)
                {
                    whereBuilder.Append(string.Format("{0} = '{1}'", key, value));
                }
                else
                {
                    whereBuilder.Append(string.Format("{0} = {1}", key, value));
                }
            }

            string updateSql = string.Format("Update {0} set {1} where {2}", entityObject.GetType().Name, sqlBuidler, whereBuilder);
            this.Context.ExecuteNonQuery(updateSql.ToString());
        }

        //private void UpdateObject(EntityObject entityObject, List<TransField> updateColumnsList, EntityState state, bool keyFieldsChanged, bool secondOperation)
        //{
        //    bool isDeleteObject = false;
        //    foreach (var field in updateColumnsList)
        //    {
        //        if (field.ReadOnly)
        //        {
        //            throw new ArgumentException(MessageHelper.EFTransactionMessage.TransDesFieldIsReadOnly);
        //        }

        //        #region Get Field Value
        //        object fieldValue = null;

        //        if (keyFieldsChanged)
        //        {
        //            if (!secondOperation)
        //            {
        //                switch (field.UpdateMode)
        //                {
        //                    case UpdateMode.Increase:
        //                        fieldValue = Operate(entityObject.GetValue(field.DesField), field.OldValue, UpdateMode.Decrease);
        //                        isDeleteObject = CheckValue(field, fieldValue);
        //                        break;
        //                    case UpdateMode.Decrease:
        //                        fieldValue = Operate(entityObject.GetValue(field.DesField), field.OldValue, UpdateMode.Increase);
        //                        break;
        //                    default:
        //                        continue;
        //                }
        //            }
        //            else
        //            {
        //                switch (field.UpdateMode)
        //                {
        //                    case UpdateMode.Increase:
        //                        fieldValue = Operate(entityObject.GetValue(field.DesField), field.NewValue, UpdateMode.Increase);
        //                        break;
        //                    case UpdateMode.Decrease:
        //                        fieldValue = Operate(entityObject.GetValue(field.DesField), field.NewValue, UpdateMode.Decrease);
        //                        isDeleteObject = CheckValue(field, fieldValue);
        //                        break;
        //                    case UpdateMode.Replace:
        //                        fieldValue = field.NewValue;
        //                        break;
        //                    default:
        //                        continue;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            switch (field.UpdateMode)
        //            {
        //                case UpdateMode.Increase:
        //                    fieldValue = Operate(entityObject.GetValue(field.DesField), field.NewValue, UpdateMode.Increase);
        //                    fieldValue = Operate(fieldValue, field.OldValue, UpdateMode.Decrease);
        //                    isDeleteObject = CheckValue(field, fieldValue);
        //                    break;
        //                case UpdateMode.Decrease:
        //                    fieldValue = Operate(entityObject.GetValue(field.DesField), field.NewValue, UpdateMode.Decrease);
        //                    fieldValue = Operate(fieldValue, field.OldValue, UpdateMode.Increase);
        //                    isDeleteObject = CheckValue(field, fieldValue);
        //                    break;
        //                case UpdateMode.Replace:
        //                    if (state == EntityState.Deleted)
        //                    {
        //                        fieldValue = field.OldValue;
        //                    }
        //                    else
        //                    {
        //                        fieldValue = field.NewValue;
        //                    }
        //                    break;
        //                default:
        //                    continue;
        //            }
        //        }
        //        #endregion

        //        entityObject.SetValue(field.DesField, fieldValue);
        //    }

        //    if (isDeleteObject)
        //    {
        //        this.Context.DeleteObject(entityObject);
        //    }
        //    else
        //    {

        //        this.Context.AttachUpdated(entityObject);
        //    }
        //}

        private void AddObject(Transaction transaction, EntityObject entityObject, EntityState state, List<TransKeyField> transKeyFields, List<TransField> transFields, bool isKeyChanged, bool isGetOldValue, bool isAlawysAppend)
        {
            EntityObject transactionTable = this.Context.CreateObject(transaction.TransTableName);

            EFAutoNumber autoNumber = transaction.AutoNumber;
            if (autoNumber != null)
            {
                autoNumber.Context = this.Context;
                autoNumber.GetNumber(entityObject, true);
                transactionTable.SetValue(autoNumber.TargetColumn, autoNumber.Number);
            }

            foreach (var keyField in transKeyFields)
            {
                object fieldValue = null;
                if (!keyField.ReadOnly)
                {
                    if (autoNumber != null)
                    {
                        if (keyField.DesField != autoNumber.TargetColumn)
                        {
                            fieldValue = keyField.OldValue;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (isKeyChanged || isGetOldValue)
                    {
                        fieldValue = keyField.OldValue;
                    }
                    else
                    {
                        fieldValue = keyField.NewValue;
                    }

                    transactionTable.SetValue(keyField.DesField, fieldValue);
                }
            }



            foreach (var field in transFields)
            {
                object fieldValue = null;

                if (!field.ReadOnly)
                {
                    if (autoNumber != null)
                    {
                        if (field.DesField != autoNumber.TargetColumn)
                        {
                            fieldValue = field.OldValue;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (isKeyChanged && !isAlawysAppend)
                    {
                        if (field.UpdateMode != UpdateMode.WriteBack)
                        {
                            fieldValue = field.OldValue;
                        }
                    }
                    else
                    {
                        if (field.UpdateMode != UpdateMode.WriteBack)
                        {
                            fieldValue = this.GetFieldValue(field, isGetOldValue, isAlawysAppend);
                        }
                    }

                    if (fieldValue != null)
                    {
                        transactionTable.SetValue(field.DesField, fieldValue);
                    }
                }
            }

            this.Context.AddObject(transaction.TransTableName, transactionTable);
        }

        private object GetFieldValue(TransField field, bool isGetOldValue, bool isAlwaysAppend)
        {
            object fieldValue = null;

            switch (field.UpdateMode)
            {
                case UpdateMode.Increase:
                case UpdateMode.Decrease:
                    if (isGetOldValue)
                    {
                        if (field.OldValue != null)
                        {
                            fieldValue = field.OldValue;
                        }
                    }
                    else
                    {
                        if (field.NewValue != null)
                        {
                            fieldValue = field.NewValue;
                        }
                    }

                    if (fieldValue != null)
                    {
                        if (isAlwaysAppend)
                        {
                            fieldValue = (field.UpdateMode == UpdateMode.Increase ? MinusSign : string.Empty) + fieldValue;
                        }
                        else
                        {
                            fieldValue = (field.UpdateMode == UpdateMode.Increase ? string.Empty : MinusSign) + fieldValue;
                        }
                    }
                    break;
                case UpdateMode.Replace:
                    fieldValue = isGetOldValue == true ? field.OldValue : field.NewValue;
                    break;
            }

            return fieldValue;
        }

        private List<TransKeyField> GetTransKeyFieldsList(Transaction transaction,
            EntityObject entityObject, EntityState state)
        {
            List<TransKeyField> transKeyFieldsList = new List<TransKeyField>();

            foreach (var keyField in transaction.TransKeyFields)
            {
                if (string.IsNullOrWhiteSpace(keyField.SrcField))
                {
                    if (!string.IsNullOrWhiteSpace(keyField.SrcGetValue))
                    {
                        if (state == EntityState.Added)
                        {
                            keyField.OldValue = null;
                        }
                        else
                        {
                            if (keyField.OldValue == null)
                            {
                                keyField.OldValue = GetFieldDefaultValue(keyField, entityObject);
                            }
                        }

                        if (state == EntityState.Deleted)
                        {
                            keyField.NewValue = null;
                        }
                        else
                        {
                            if (keyField.NewValue == null)
                            {
                                keyField.NewValue = GetFieldDefaultValue(keyField, entityObject);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (keyField.WhereMode == WhereMode.InsertOnly)
                    {
                        keyField.ReadOnly = true;
                    }
                    else
                    {
                        keyField.ReadOnly = false;
                    }

                    if (state == EntityState.Added)
                    {
                        keyField.OldValue = null;
                    }
                    else
                    {
                        keyField.OldValue = this.Context.GetObjectOriginalValue(entityObject, keyField.SrcField);
                    }

                    if (state == EntityState.Deleted)
                    {
                        keyField.NewValue = null;
                    }
                    else
                    {
                        keyField.NewValue = this.Context.GetObjectCurrentValue(entityObject, keyField.SrcField);
                    }
                }
                transKeyFieldsList.Add(keyField);
            }

            return transKeyFieldsList;
        }

        private List<TransField> GetTransFieldList(Transaction transaction,
            EntityObject entityObject, EntityState state)
        {
            List<TransField> updateColumnsList = new List<TransField>();

            foreach (var field in transaction.TransFields)
            {
                if (field.UpdateMode == UpdateMode.Disable)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(field.SrcField))
                {
                    if (!string.IsNullOrWhiteSpace(field.SrcGetValue))
                    {
                        if (state == EntityState.Added)
                        {
                            field.OldValue = null;
                        }
                        else
                        {
                            if (field.OldValue == null)
                            {
                                field.OldValue = GetFieldDefaultValue(field, entityObject);
                            }
                        }

                        if (state == EntityState.Deleted)
                        {
                            field.NewValue = null;
                        }
                        else
                        {
                            if (field.NewValue == null)
                            {
                                field.NewValue = GetFieldDefaultValue(field, entityObject);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (field.UpdateMode == UpdateMode.WriteBack)
                    {
                        continue;
                    }

                    if (state == EntityState.Added)
                    {
                        field.OldValue = null;
                    }
                    else
                    {
                        field.OldValue = this.Context.GetObjectOriginalValue(entityObject, field.SrcField);
                    }

                    if (state == EntityState.Deleted)
                    {
                        if (field.UpdateMode == UpdateMode.Replace)
                        {
                            field.NewValue = this.Context.GetObjectOriginalValue(entityObject, field.SrcField);
                        }
                        else
                        {
                            field.NewValue = null;
                        }
                    }
                    else
                    {
                        field.NewValue = this.Context.GetObjectCurrentValue(entityObject, field.SrcField);
                    }
                }

                updateColumnsList.Add(field);
            }

            return updateColumnsList;
        }

#warning 此方法有空重新整理
        private string GetFieldDefaultValue(TransFieldBase field, EntityObject entityObject)
        {
            string defaultValue = field.SrcGetValue;
            char[] cs = defaultValue.ToCharArray();


#warning 加完ServerUtility再回来补
            //object[] myret = SrvUtils.GetValue(defaultValue, (DataModule)((InfoTransaction)_transaction.Owner).OwnerComp);
            //if (myret != null && (int)myret[0] == 0)
            //{
            //    return (string)myret[1];
            //}
            if (cs[0] != '"' && cs[0] != '\'')
            {
                char[] sep1 = "()".ToCharArray();
                string[] sps1 = defaultValue.Split(sep1);

                if (sps1.Length == 3)
                {
                    return this.Module.CallMethod(sps1[0], new object[] { entityObject }).ToString();
                }

                if (sps1.Length == 1)
                {
                    return sps1[0];
                }

                if (sps1.Length != 1 && sps1.Length == 3)
                {
                    //String message = SysMsg.GetSystemMessage(((DataModule)((InfoTransaction)_transaction.Owner).OwnerComp).Language, "Srvtools", "InfoTranscation", "msg_TransFieldBaseDefaultValueIsBad");
                    //throw new ArgumentException(String.Format(message, ((InfoTransaction)(_transaction.Owner)).Name, field.DesField));
                }
            }

            char[] sep2 = null;
            if (cs[0] == '"')
            {
                sep2 = "\"".ToCharArray();
            }
            if (cs[0] == '\'')
            {
                sep2 = "'".ToCharArray();
            }

            string[] sps2 = defaultValue.Split(sep2);
            if (sps2.Length == 3)
            {
                return sps2[1];
            }
            else
            {
                //String message = SysMsg.GetSystemMessage(((DataModule)((InfoTransaction)_transaction.Owner).OwnerComp).Language, "Srvtools", "InfoTranscation", "msg_TransFieldBaseDefaultValueIsBad");
                //throw new ArgumentException(String.Format(message, ((InfoTransaction)(_transaction.Owner)).Name, field.DesField));
            }

            return defaultValue;
        }

        // Order transaction by transstep.
        private Comparison<Transaction> GetTransStepComparsion()
        {
            Comparison<Transaction> comparison = new Comparison<Transaction>(GetTransStepRule);

            return comparison;
        }

        // Get transstep order rule.
        private int GetTransStepRule(Transaction transaction1, Transaction transaction2)
        {
            if (transaction1.TransStep >= transaction2.TransStep)
            {
                return transaction1.TransStep;
            }
            else
            {
                return transaction2.TransStep;
            }
        }

        public EFTransaction Copy()
        {
            EFTransaction transPrivateCopy = new EFTransaction();

            foreach (var transaction in this.Transactions)
            {
                transPrivateCopy.Transactions.Add(transaction.Copy());
            }

            transPrivateCopy.UpdateComponent = this.UpdateComponent;
            transPrivateCopy.Context = this.Context;

            return transPrivateCopy;
        }
        #endregion
    }

    #region Event Definition
    public delegate void EFTransactionBeforeTransEventHandler(object sender, EFTransactionBeforeTransEventArgs e);

    public delegate void EFTransactionAfterTransEventHandler(object sender, EFTransactionAfterTransEventArgs e);


    public sealed class EFTransactionBeforeTransEventArgs : CancelEventArgs
    {
        public EFTransactionBeforeTransEventArgs(Transaction trans)
            : base()
        {
            _transaction = trans;
        }

        private Transaction _transaction;
        public Transaction Transaction
        {
            get { return _transaction; }
        }
    }

    public sealed class EFTransactionAfterTransEventArgs : EventArgs
    {
        public EFTransactionAfterTransEventArgs()
            : base()
        {
        }
    }
    #endregion

    #region Transaction
    public class Transaction : EFCollectionItem, IUseEntitySet
    {
        #region Constructor

        public Transaction()
            : this("")
        {
        }

        public Transaction(String name)
        {
            _transKeyFields = new EFCollection<TransKeyField>(this);
            _transFields = new EFCollection<TransField>(this);
            _transMode = TransMode.AutoAppend;
            _whenDelete = true;
            _whenInsert = true;
            _whenUpdate = true;
        }

        #endregion

        #region Properties
        private int _transStep;
        [Category("Data")]
        public int TransStep
        {
            set { _transStep = value; }
            get { return _transStep; }
        }

        private string _transTableName;
        [Category("Data")]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.ENTITYSET)]
        public string TransTableName
        {
            set { _transTableName = value; }
            get { return _transTableName; }
        }

        private TransMode _transMode;
        [Category("Design")]
        public TransMode TransMode
        {
            set { _transMode = value; }
            get { return _transMode; }
        }

        private bool _whenInsert;
        [Category("Design")]
        public bool WhenInsert
        {
            set { _whenInsert = value; }
            get { return _whenInsert; }
        }

        private bool _whenUpdate;
        [Category("Design")]
        public bool WhenUpdate
        {
            set { _whenUpdate = value; }
            get { return _whenUpdate; }
        }

        private bool _whenDelete;
        [Category("Design")]
        public bool WhenDelete
        {
            set { _whenDelete = value; }
            get { return _whenDelete; }
        }

        private EFAutoNumber _autoNumber;
        [Category("Data")]
        public EFAutoNumber AutoNumber
        {
            set { _autoNumber = value; }
            get { return _autoNumber; }
        }

        private EFCollection<TransKeyField> _transKeyFields;
        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EFCollection<TransKeyField> TransKeyFields
        {
            set { _transKeyFields = value; }
            get { return _transKeyFields; }
        }

        private EFCollection<TransField> _transFields;
        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EFCollection<TransField> TransFields
        {
            set { _transFields = value; }
            get { return _transFields; }
        }

        private EntityState _currentState;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EntityState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        private EntityObject _currentObject;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EntityObject CurrentObject
        {
            get { return _currentObject; }
            set { _currentObject = value; }
        }
        #endregion

        #region IUseEntitySet Members
        [Browsable(false)]
        public string TargetEntitySet
        {
            get { return _transTableName; }
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return _transTableName;
        }

        public Transaction Copy()
        {
            Transaction transPrivateCopy = new Transaction();

            transPrivateCopy.AutoNumber = this.AutoNumber;

            foreach (TransField field in this.TransFields)
            {
                TransField tf = new TransField();
                tf.DesField = field.DesField;
                tf.OldValue = field.OldValue;
                tf.SrcField = field.SrcField;
                tf.SrcGetValue = field.SrcGetValue;
                tf.NewValue = field.NewValue;
                tf.UpdateMode = field.UpdateMode;

                transPrivateCopy.TransFields.Add(tf);
            }

            foreach (TransKeyField keyField in this.TransKeyFields)
            {
                TransKeyField tkf = new TransKeyField();
                tkf.DesField = keyField.DesField;
                tkf.SrcField = keyField.SrcField;
                tkf.SrcGetValue = keyField.SrcGetValue;
                tkf.NewValue = keyField.NewValue;
                tkf.WhereMode = keyField.WhereMode;

                transPrivateCopy.TransKeyFields.Add(tkf);
            }
            transPrivateCopy.TransMode = this.TransMode;
            transPrivateCopy.TransStep = this.TransStep;
            transPrivateCopy.TransTableName = this.TransTableName;
            transPrivateCopy.WhenInsert = this.WhenInsert;
            transPrivateCopy.WhenUpdate = this.WhenUpdate;
            transPrivateCopy.WhenDelete = this.WhenDelete;

            return transPrivateCopy;
        }
        #endregion
    }

    public enum TransMode
    {
        /// <summary>
        /// If not exist the transtable then auto append the transtable.
        /// </summary>
        AutoAppend = 0,

        /// <summary>
        /// If not exist the transtable then throw exception and rollback.
        /// </summary>
        Exception = 1,

        /// <summary>
        /// If not exist the transtable then contiute the next transstep.
        /// </summary>
        Ignore = 2,

        /// <summary>
        /// 
        /// </summary>
        AlwaysAppend = 3
    }
    #endregion

    #region TransField
    public class TransField : TransFieldBase
    {
        #region Constructor

        public TransField()
            : base("")
        {

        }

        public TransField(string desField)
            : base(desField)
        {
            _updateMode = UpdateMode.Increase;
        }

        #endregion

        #region Propeties
        private UpdateMode _updateMode;
        [Category("Design")]
        [Description("Update mode")]
        public UpdateMode UpdateMode
        {
            set { _updateMode = value; }
            get { return _updateMode; }
        }

        private bool deleteForDecrease;
        [Category("Design")]
        public bool DeleteForDecrease
        {
            get { return deleteForDecrease; }
            set { deleteForDecrease = value; }
        }

        private bool exceptionForMinus;
        [Category("Design")]
        public bool ExceptionForMinus
        {
            get { return exceptionForMinus; }
            set { exceptionForMinus = value; }
        }
        #endregion
    }

    public enum UpdateMode
    {
        Increase = 0,

        Decrease = 1,

        Replace = 2,

        WriteBack = 3,

        Disable = 4
    }
    #endregion

    #region TransKeyField
    public class TransKeyField : TransFieldBase
    {
        #region Constructor

        public TransKeyField()
            : base("")
        {
            _wherMode = WhereMode.Both;
        }

        public TransKeyField(string desField)
            : base(desField)
        {
            _wherMode = WhereMode.Both;
        }

        #endregion

        #region Propeties
        private WhereMode _wherMode;
        [Category("Design")]
        public WhereMode WhereMode
        {
            set { _wherMode = value; }
            get { return _wherMode; }
        }

        #endregion
    }

    public enum WhereMode
    {
        WhereOnly = 0,

        InsertOnly = 1,

        Both = 2
    }
    #endregion

    public class TransFieldBase : EFCollectionItem
    {
        #region Constructor

        public TransFieldBase()
            : this("")
        {

        }

        public TransFieldBase(string desField)
        {
            _desField = desField;
        }

        #endregion

        #region Propeties
        private string _desField;
        [Category("Data")]
        [Description("Destination column")]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        TargetEntitySetItem(),
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string DesField
        {
            set { _desField = value; }
            get { return _desField; }
        }

        private string _srcField;
        [Category("Data")]
        [Description("Source column")]
        [Editor(typeof(EdmPropertyDropDownEditor), typeof(UITypeEditor)),
        EdmItem(EdmItemAttribute.SCALE_PROPERTY)]
        public string SrcField
        {
            set { _srcField = value; }
            get { return _srcField; }
        }

        private string _srcGetValue;
        [Category("Data")]
        [Description("Source GetValue")]
        public string SrcGetValue
        {
            set { _srcGetValue = value; }
            get { return _srcGetValue; }
        }

        private bool _readOnly;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        private object _srcValue;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object NewValue
        {
            set { _srcValue = value; }
            get { return _srcValue; }
        }

        private object _desValue;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object OldValue
        {
            set { _desValue = value; }
            get { return _desValue; }
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return _desField;
        }
        #endregion
    }
}
