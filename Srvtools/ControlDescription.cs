using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.ComponentModel.Design;
using System.Collections;
using System.Windows.Forms;
using System.IO;

namespace Srvtools
{
    [Designer(typeof(ControlDescriptionDesigner))]
    public partial class ControlDescription : Component, IGetValues
    {
        public ControlDescription()
        {
            InitializeComponent();
            _Identification = this.GetHashCode();
        }

        public ControlDescription(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            _Identification = this.GetHashCode();
        }

        private string _DataBase;
        [Category("Infolight"),
        Description("Specifies DataBase storing the data of MultiLanguage")]
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DataBase
        {
            get
            {
                return _DataBase;
            }
            set
            {
                _DataBase = value;
            }
        }

        private int _Identification;
        [Browsable(false)]
        public int Identification
        {
            get { return _Identification; }
            set { _Identification = value; }
        }

        internal List<string> GetControlValues()
        {
            List<string> list = new List<string>();
            foreach (IComponent comp in this.Container.Components)
            {
                if (comp is TextBox || comp is DateTimePicker || comp is ComboBox || comp is InfoRefvalBox)
                {
                    list.Add(comp.Site.Name);
                }
                else if (comp is DataGridViewColumn)
                {
                    list.Add((comp as DataGridViewColumn).Name);
                }
            }
            return list;
        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            if (string.Compare(sKind, "DataBase", true) == 0)
            {
                List<string> list = new List<string>();

                XmlDocument xml = new XmlDocument();
                if (File.Exists(SystemFile.DBFile))
                {
                    xml.Load(SystemFile.DBFile);
                    XmlNode aNode = xml.DocumentElement.FirstChild.FirstChild;
                    while (aNode != null)
                    {
                        list.Add(aNode.LocalName);
                        aNode = aNode.NextSibling;
                    }
                }
                return list.ToArray();
            }
            return null;
        }

        #endregion
    }

    public class ControlDescriptionDesigner : ComponentDesigner
    {
        public override void DoDefaultAction()
        {
            if (!string.IsNullOrEmpty((this.Component as ControlDescription).DataBase))
            {
                ControlDescriptionEditorDialog form = new ControlDescriptionEditorDialog(this.Component);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("the property of controldescription hasn't be set correctly");
            }
        }
    }
}
