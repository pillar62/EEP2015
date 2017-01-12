using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Xml;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using System.IO;
using System.Web.UI;

namespace Srvtools
{
    [Designer(typeof(WebControlDescriptionEditor), typeof(IDesigner))]
    public class WebControlDescription : WebControl, IGetValues
    {
        private string _DataBase;
        [Category("Infolight"),
        Description("Specifies DataBase storing the data of WebControlDescription")]
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
            return GetControlValues(this.Page.Controls);
        }

        private List<string> GetControlValues(ControlCollection collection)
        {
            List<string> list = new List<string>();
            foreach (Control ctrl in collection)
            {
                if (ctrl is GridView)
                {
                    GridView view = ctrl as GridView;
                    for (int i = 0; i < view.Columns.Count; i++)
                    {
                        if (view.Columns[i] is BoundField)
                        {
                            list.Add(string.Format("{0}.{1}", ctrl.ID, ((BoundField)view.Columns[i]).DataField));
                        }
                    }
                }
                else if (ctrl is DetailsView)
                {
                    DetailsView view = ctrl as DetailsView;
                    for (int i = 0; i < view.Fields.Count; i++)
                    {
                        if (view.Fields[i] is BoundField)
                        {
                            list.Add(string.Format("{0}.{1}", ctrl.ID, ((BoundField)view.Fields[i]).DataField));
                        }
                    }
                }
                else if (ctrl is FormView)
                {
                    if (ctrl is WebFormView)
                    {
                        WebFormView view = ctrl as WebFormView;
                        for (int i = 0; i < view.Fields.Count; i++)
                        {
                            list.Add(string.Format("{0}.{1}", ctrl.ID, (view.Fields[i] as FormViewField).ControlID));
                        }
                    }
                }
                else if (ctrl is TextBox || ctrl is DropDownList || ctrl is WebDateTimePicker || ctrl is WebRefVal)
                {
                    list.Add(ctrl.ID);
                }
                else
                {
                    list.AddRange(GetControlValues(ctrl.Controls));
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

    public class WebControlDescriptionEditor : DataSourceDesigner
    { 
        DesignerActionListCollection _actionLists;

        public WebControlDescriptionEditor()
        {
            DesignerVerb editVerb = new DesignerVerb("Edit", new EventHandler(OnEdit));
            this.Verbs.Add(editVerb);
        }

        public void OnEdit(object sender, EventArgs e)
        {
            if ((this.Component as WebControlDescription).Identification == 0)
            {
                int hashcode = this.Component.GetHashCode();
                (this.Component as WebControlDescription).Identification = hashcode;
                TypeDescriptor.GetProperties(typeof(WebControlDescription))["Identification"].SetValue(this.Component, hashcode);
            }
            if (!string.IsNullOrEmpty((this.Component as WebControlDescription).DataBase))
            {
                ControlDescriptionEditorDialog form = new ControlDescriptionEditorDialog(this.Component);
                form.ShowDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("the property of webcontroldescription hasn't be set correctly");
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                _actionLists = base.ActionLists;

                if (_actionLists != null)
                    _actionLists.Add(new WebControlDescriptionActionList(this.Component));

                return _actionLists;
            }
        }
    }

    public class WebControlDescriptionActionList : DesignerActionList
    {
        private WebControlDescription comp;

        public WebControlDescriptionActionList(IComponent component)
            : base(component)
        {
            comp = component as WebControlDescription;
        }

        public void OnEdit()
        {
            if (comp.Identification == 0)
            {
                int hashcode = comp.GetHashCode();
                comp.Identification = hashcode;
                TypeDescriptor.GetProperties(typeof(WebControlDescription))["Identification"].SetValue(comp, hashcode);
            }
            if (!string.IsNullOrEmpty(comp.DataBase))
            {
                ControlDescriptionEditorDialog form = new ControlDescriptionEditorDialog(comp);
                form.ShowDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("the property of webcontroldescription hasn't be set correctly");
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            items.Add(new DesignerActionMethodItem(this, "OnEdit", "Edit", "UserEdit", true));
            return items;
        }
    }
}
