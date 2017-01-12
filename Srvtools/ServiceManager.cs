using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace Srvtools
{
    ////////////////////////////////////////////////////////////////////////////
    //[Serializable]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(UpdateComponent), "Resources.ServiceManager.ico")]
    public class ServiceManager : InfoBaseComp, IServiceManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public ServiceManager(System.ComponentModel.IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            _serviceCollection = new ServiceCollection(this, typeof(Service));
        }

        public ServiceManager()
        {
            ///
            /// This call is required by the Windows.Forms Designer.
            ///
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            _serviceCollection = new ServiceCollection(this, typeof(Service));
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public String GetMethodName(string serviceName, ref bool bNonLogin)
        {
            //collectionitem-> property -> NonLogin
            //1)if can't find servicename , return "";//null
            //2)if service found, return NonLogin = collectionitem.nonlogin
            //3)return Methodname..
            //return "杨东 will implemented";

            if (serviceName != "")
            {
                Object obj = null;
                foreach (Service s in this._serviceCollection)
                {
                    if (string.Compare(s.ServiceName.Trim(), serviceName.Trim(), true) == 0)//IgnoreCase
                    {
                        obj = s;
                    }
                }

                if (obj != null)
                {
                    Service service = (Service)obj;

                    bNonLogin = service.NonLogin;    // ref 类型的返回值，必须放在返回值的第一位。
                    return service.DelegateName;
                }
                else
                {
                    bNonLogin = false;
                    return "";
                }
            }
            else
            {
                bNonLogin = false;
                return "";
            }
        }

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Data")]
        [Description("Service Collection")]
        public ServiceCollection ServiceCollection
        {
            get
            {
                if (_serviceCollection == null)
                {
                    _serviceCollection = new ServiceCollection(this, typeof(Service));
                }
                return _serviceCollection;
            }
            set
            {
                _serviceCollection = value;
            }
        }

        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        #region Private vars

        private ServiceCollection _serviceCollection;// = new ServiceCollection();

        #endregion
    }
}
