using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using EFClientTools.Editor;
using System.ServiceModel.Channels;
using System.ServiceModel;
using EFClientTools.EFServerReference;
using System.Reflection;
using EFClientTools.Common;

namespace EFClientTools.Web
{
    //只贴一个吗？？
    public class EFDataSource: DataSourceControl, IDataSource, IEFDataSource
    {
        protected override DataSourceView GetView(string viewName)
        {
            viewName = DataMember;
            EFDataSourceView view = new EFDataSourceView(this, viewName);
            return view;
        }

        protected override ICollection GetViewNames()
        {
            return new string[] { "Customers" };//use name not entity set name
        }

        string _dataMember = "";
        string _remoteName = "";
        string _masterDataSourceID = "";
        bool _active = false;
        bool _autoApply = false;
        int _packageRecords = 10;

        [DefaultValue("")]
        [Editor(typeof(EntityClassEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get { return _dataMember; }
            set { _dataMember = value; }
        }

        [DefaultValue("")]
        [Editor(typeof(RemoteNameEditor), typeof(UITypeEditor))]
        public string RemoteName
        {
            get { return _remoteName; }
            set { _remoteName = value; }
        }

        [DefaultValue("")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string MasterDataSourceID
        {
            get { return _masterDataSourceID; }
            set { _masterDataSourceID = value; }
        }

        [DefaultValue(false)]
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        [DefaultValue(false)]
        public bool AutoApply
        {
            get { return _autoApply; }
            set { _autoApply = value; }
        }

        [DefaultValue(10)]
        public int PackageRecords
        {
            get { return _packageRecords; }
            set { _packageRecords = value; }
        }

        public List<string> GetEntityClasses()
        {
            List<string> entityClasses = new List<string>();
            if (string.IsNullOrWhiteSpace(this.MasterDataSourceID))
            {
                string masterClassName = GetMasterEntityClassName(this.RemoteName);
                if (!string.IsNullOrEmpty(masterClassName))
                {
                    entityClasses.Add(masterClassName);
                }
            }
            else
            {
                Control control = this.Page.FindControl(this.MasterDataSourceID);
                if (control != null)
                {
                    IEFDataSource dataSource = (IEFDataSource)control;

                    if (!string.IsNullOrWhiteSpace(this.RemoteName) && this.RemoteName.IndexOf('.') != -1)
                    {
                        string masterClassName = GetMasterEntityClassName(dataSource.RemoteName);
                        if (!string.IsNullOrEmpty(masterClassName))
                        {
                            entityClasses.AddRange(EntityProvider.GetDetailEntityClassNames(masterClassName));
                        }
                    }
                    else
                    {
                        entityClasses.AddRange(EntityProvider.GetDetailEntityClassNames(dataSource.DataMember));
                    }
                }
            }
            return entityClasses;
        }

        private string GetMasterEntityClassName(string remoteName)
        {
            string masterClassName = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.RemoteName) && this.RemoteName.IndexOf('.') != -1)
            {
                string[] remotes = remoteName.Split('.');
                masterClassName = DesignClientUtility.Client.GetObjectClassName(DesignClientUtility.ClientInfo, remotes[0], remotes[1], string.Empty);
                
            }
            return masterClassName;
        }

        public Type GetEntityType()
        {
            return EntityProvider.GetEntityType(this.DataMember);
        }
    }

    public class EFDataSourceView : DataSourceView
    {
        private EFDataSource _owner;

        public EFDataSource Owner
        {
            get { return _owner; }
        }

        private string _viewName;
        public string ViewName
        {
            get { return _viewName; }
        }

        public EFDataSourceView(EFDataSource owner, string viewName):base(owner, viewName)
        {
            _owner = owner;
            _viewName = viewName;
        }

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            if (string.IsNullOrEmpty(ViewName))
            {
                throw new ArgumentNullException("ViewName");
            }
            var objects = ClientUtility.GetObjects(this.Owner);
            return objects;
        }

        protected override int ExecuteInsert(IDictionary values)
        {
            return base.ExecuteInsert(values);
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            return base.ExecuteUpdate(keys, values, oldValues);
        }

        protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
        {
            return base.ExecuteDelete(keys, oldValues);
        }
    }
}
