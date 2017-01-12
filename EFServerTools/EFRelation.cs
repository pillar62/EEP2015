using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EFWCFModule;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.Metadata.Edm;
using System.Data;
using System.Drawing;
using EFServerTools.Common;

namespace EFServerTools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(EFRelation), ICOInfo.EFRelation)]
    public class EFRelation: Component, IRelation
    {
        #region Constructor

        public EFRelation(System.ComponentModel.IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();


            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void InitializeComponent()
        {
            
        }

        public EFRelation()
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

        #region IRelation Members
        private ICommand _MasterCommand;
        /// <summary>
        /// 
        /// </summary>
        [Category(ComponentInfo.COMPANY)]
        [Description(EFRelationInfo.MasterCommand)]
        public ICommand MasterCommand
        {
            get
            {
                return _MasterCommand;
            }
            set
            {
                _MasterCommand = value;
            }
        }

        private ICommand _DetailCommand;
        /// <summary>
        /// 
        /// </summary>
        [Category(ComponentInfo.COMPANY)]
        [Description(EFRelationInfo.DetailCommand)]
        public ICommand DetailCommand
        {
            get
            {
                return _DetailCommand;
            }
            set
            {
                _DetailCommand = value;
            }
        }

        public void GetDetailObjects(System.Data.Objects.DataClasses.EntityObject masterObject, ClientInfo clientInfo)
        {
            if (masterObject == null)
            {
                throw new ArgumentNullException("masterObject");
            }
            if (clientInfo == null)
            {
                throw new ArgumentNullException("clientInfo");
            }
            if (DetailCommand == null)
            {
                throw new ArgumentNullException("DetailCommand");
            }
            Context.Attach(masterObject);
            //var provider = new MetadataProvider(Context.MetadataWorkspace);
            //var relatedEnd = ((IEntityWithRelationships)masterObject).RelationshipManager.GetAllRelatedEnds()
            //    .FirstOrDefault(c => provider.GetAssociationSetEndEntitySetName(MasterCommand.ContextName, c.RelationshipName, c.TargetRoleName).Equals(DetailCommand.EntitySetName));
            //if (relatedEnd == null)
            //{
            //    throw new ObjectNotFoundException(string.Format("RelatedEnd:{0}->{1} not found.", MasterCommand.EntitySetName, DetailCommand.EntitySetName));
            //}
            var relatedEnd = EntityProvider.GetRelatedEnd(Context, masterObject, DetailCommand.EntitySetName);

            if (relatedEnd.RelationshipSet != null && relatedEnd.RelationshipSet is AssociationSet)
            {
                var constraint = (relatedEnd.RelationshipSet as AssociationSet).ElementType.ReferentialConstraints
                    .FirstOrDefault(c => c.FromRole.Name.Equals(relatedEnd.SourceRoleName) && c.ToRole.Name.Equals(relatedEnd.TargetRoleName));
                if (constraint == null)
                {
                    throw new ObjectNotFoundException(string.Format("ReferentialConstraint:{0}->{1} not found.", relatedEnd.SourceRoleName, relatedEnd.TargetRoleName));
                }
              
                var listWhereParameter = new List<WhereParameter>();
                for (int i = 0; i < constraint.FromProperties.Count; i++)
                {
                    var fromField = constraint.FromProperties[i].Name;
                    var toField = constraint.ToProperties[i].Name;

                    var value = masterObject.GetValue(fromField);
                    listWhereParameter.Add(new WhereParameter() { Field = toField, Value = value });
                }
                DetailCommand.Context = Context;
                var objects = DetailCommand.GetObjects(clientInfo);
                objects = EntityProvider.SetWhere(objects, listWhereParameter);
                objects.ToList();
            }
            else
            {
                throw new EntityException("Can not cast relatedEnd.RelationshipSet to AssociationSet.");
            }
        }

        #endregion

        #region Properties
       
        //private List<RelationColumn> _MasterColumns = new List<RelationColumn>();
        //[Category("Infolight"),
        //Description("Specifies columns in master table associated with coloumns of detail table")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public List<RelationColumn> MasterColumns
        //{
        //    get { return _MasterColumns; }
        //    set { _MasterColumns = value; }
        //}

        //private List<RelationColumn> _DetailColumns = new List<RelationColumn>();
        //[Category("Infolight"),
        //Description("Specifies columns in detail table associated with coloumns of master table")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public List<RelationColumn> DetailColumns
        //{
        //    get
        //    {
        //        return _DetailColumns;
        //    }
        //    set
        //    {
        //        _DetailColumns = value;
        //    }
        //}
        #endregion
    }

    //[Serializable]
    //public class RelationColumn
    //{
    //    private string _ColumnName;
    //    [Category("Infolight"),
    //    Description("Column name of relationship")]
    //    public string ColumnName
    //    {
    //        get { return _ColumnName; }
    //        set { _ColumnName = value; }
    //    }
    //}
}
