using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Srvtools
{
    public enum WebMultiViewCaptionStyle { Style1, Style2, Style3, Style4, Style5 }
    public class WebMultiViewCaptions : WebInfoBaseControl, IPostBackEventHandler
    {
        public WebMultiViewCaptions()
        {
            _Captions = new WebMultiViewCaptionCollection(this, typeof(WebMultiViewCaption));
        }

        #region Properties
        [Category("Infolight")]
        [Editor(typeof(MultiViewEditor), typeof(UITypeEditor))]
        public string MultiViewID
        {
            get
            {
                object obj = this.ViewState["MultiViewID"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set
            {
                this.ViewState["MultiViewID"] = value;
            }
        }

        private WebMultiViewCaptionCollection _Captions;
        [Category("Infolight"),
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        TypeConverter(typeof(CollectionConverter)),
        NotifyParentProperty(true)]
        public WebMultiViewCaptionCollection Captions
        {
            get { return _Captions; }
        }

        [Category("Infolight"),
        DefaultValue(WebMultiViewCaptionStyle.Style1)]
        public WebMultiViewCaptionStyle TableStyle
        {
            get
            {
                object obj = this.ViewState["TableStyle"];
                if (obj != null)
                {
                    return (WebMultiViewCaptionStyle)obj;
                }
                return WebMultiViewCaptionStyle.Style1;
            }
            set
            {
                this.ViewState["TableStyle"] = value;
            }
        }

        [Category("Infolight")]
        public int ActiveIndex
        {
            get
            {
                object obj = this.ViewState["ActiveIndex"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 0;
            }
            set
            {
                this.ViewState["ActiveIndex"] = value;
                if (!this.DesignMode && this.MultiViewID != null && this.MultiViewID != "")
                {
                    object obj = this.GetObjByID(this.MultiViewID);
                    if (obj != null && obj is MultiView)
                    {
                        MultiView multiView = (MultiView)obj;
                        multiView.ActiveViewIndex = value;
                    }
                }
            }
        }

        [Category("Infolight")]
        [DefaultValue(false)]
        public bool IsInRootPath
        {
            get
            {
                object obj = this.ViewState["IsInRootPath"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return false;
            }
            set
            {
                this.ViewState["IsInRootPath"] = value;
            }
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            object obj = this.GetObjByID(this.MultiViewID);
            if (obj != null && obj is MultiView)
            {
                MultiView multiView = (MultiView)obj;
                if (!Page.IsPostBack)
                {
                    if (multiView.ActiveViewIndex == -1)
                        multiView.ActiveViewIndex = 0;
                }
                multiView.ActiveViewChanged += new EventHandler(multiView_ActiveViewChanged);
            }
        }

        private void multiView_ActiveViewChanged(object sender, EventArgs e)
        {
            object obj = this.GetObjByID(this.MultiViewID);
            if (obj != null && obj is MultiView)
            {
                MultiView multiView = (MultiView)obj;
                OnTabChanged(new TabChangedEventArgs(multiView.ActiveViewIndex));
            }
        }

        public event TabChangedEventHandler TabChanged
        {
            add { Events.AddHandler(EventTabChanged, value); }
            remove { Events.RemoveHandler(EventTabChanged, value); }
        }

        private static readonly object EventTabChanged = new object();

        protected virtual void OnTabChanged(TabChangedEventArgs e)
        {
            TabChangedEventHandler TabChangedHandler = (TabChangedEventHandler)Events[EventTabChanged];
            if (TabChangedHandler != null)
            {
                TabChangedHandler(this, e);
            }
        }

        public event TabChangingEventHandler TabChanging
        {
            add { Events.AddHandler(EventTabChanging, value); }
            remove { Events.RemoveHandler(EventTabChanging, value); }
        }

        private static readonly object EventTabChanging = new object();

        protected virtual void OnTabChanging(TabChangingEventArgs e)
        {
            TabChangingEventHandler TabChangingHandler = (TabChangingEventHandler)Events[EventTabChanging];
            if (TabChangingHandler != null)
            {
                TabChangingHandler(this, e);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);
            int i = this.Captions.Count;
            if (i == 0)
            {
                writer.Write(this.ID);
            }
            else
            {
                writer.AddStyleAttribute("float", "left");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                writer.AddStyleAttribute("line-height", "normal");
                writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "70%");
                writer.AddStyleAttribute("border-bottom", "1px solid #666666");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
                writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0");
                writer.AddStyleAttribute("list-style", "none");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul); // <ul>
                for (int j = 0; j < i; j++)
                {
                    if (this.Captions[j].Visible)
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline");
                    else
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li); // <li>
                    this.createTabCaptions(writer, this.Captions[j].Caption, j);
                    writer.RenderEndTag(); // </li>
                }
                writer.RenderEndTag(); // </ul>
                writer.RenderEndTag(); // </div>
                //解决firefox中float靠右显示不换行的问题
                writer.AddStyleAttribute("clear", "both");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();
            }
        }

        protected virtual void createTabCaptions(HtmlTextWriter writer, string caption, int index)
        {
            ClientScriptManager csm = Page.ClientScript;
            writer.AddStyleAttribute("float", "left");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0 0 0 4px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.TextDecoration, "none");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "pointer");
            switch (this.TableStyle)
            {
                case WebMultiViewCaptionStyle.Style1:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tableft1.gif') no-repeat left top" : "url('../Image/MultiViewStyles/tableft1.gif') no-repeat left top");
                    break;
                case WebMultiViewCaptionStyle.Style2:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tableft2.gif') no-repeat left top" : "url('../Image/MultiViewStyles/tableft2.gif') no-repeat left top");
                    break;
                case WebMultiViewCaptionStyle.Style3:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tableft3.gif') no-repeat left top" : "url('../Image/MultiViewStyles/tableft3.gif') no-repeat left top");
                    break;
                case WebMultiViewCaptionStyle.Style4:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tableft4.gif') no-repeat left top" : "url('../Image/MultiViewStyles/tableft4.gif') no-repeat left top");
                    break;
                case WebMultiViewCaptionStyle.Style5:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tableft5.gif') no-repeat left top" : "url('../Image/MultiViewStyles/tableft5.gif') no-repeat left top");
                    break;
            }
            //if (this.ViewState["ActiveIndex"] != null)
            //{
            //    if (index == (int)this.ViewState["ActiveIndex"])
            //        writer.AddStyleAttribute("background-position", "0% -42px");
            //}
            //else
            //{
            //    if (index == 0)
            //        writer.AddStyleAttribute("background-position", "0% -42px");
            //}
            if (index == (int)this.ActiveIndex)
                writer.AddStyleAttribute("background-position", "0% -42px");
            writer.RenderBeginTag(HtmlTextWriterTag.Span); // <span container>
            switch (this.TableStyle)
            {
                case WebMultiViewCaptionStyle.Style1:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tabright1.gif') no-repeat right top" : "url('../Image/MultiViewStyles/tabright1.gif') no-repeat right top");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "#666666");
                    break;
                case WebMultiViewCaptionStyle.Style2:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tabright2.gif') no-repeat right top" : "url('../Image/MultiViewStyles/tabright2.gif') no-repeat right top");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "#FFFFFF");
                    break;
                case WebMultiViewCaptionStyle.Style3:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tabright3.gif') no-repeat right top" : "url('../Image/MultiViewStyles/tabright3.gif') no-repeat right top");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "#FFFFFF");
                    break;
                case WebMultiViewCaptionStyle.Style4:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tabright4.gif') no-repeat right top" : "url('../Image/MultiViewStyles/tabright4.gif') no-repeat right top");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "#FFFFFF");
                    break;
                case WebMultiViewCaptionStyle.Style5:
                    writer.AddStyleAttribute("background", this.IsInRootPath ? "url('Image/MultiViewStyles/tabright5.gif') no-repeat right top" : "url('../Image/MultiViewStyles/tabright5.gif') no-repeat right top");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Color, "#FFFFFF");
                    break;
            }
            writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0");
            writer.AddStyleAttribute("float", "left");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "block");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "5px 15px 4px 6px");
            //if (this.ViewState["ActiveIndex"] != null)
            //{
            //    if (index == (int)this.ViewState["ActiveIndex"])
            //        writer.AddStyleAttribute("background-position", "100% -42px");
            //}
            //else
            //{
            //    if (index == 0)
            //        writer.AddStyleAttribute("background-position", "100% -42px");
            //}
            if (index == (int)this.ActiveIndex)
                writer.AddStyleAttribute("background-position", "100% -42px");
            else
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, csm.GetPostBackEventReference(this, index.ToString()));
            writer.RenderBeginTag(HtmlTextWriterTag.Span); // <span innerHtml>
            writer.Write(caption);
            writer.RenderEndTag(); // </span innerHtml>
            writer.RenderEndTag(); // </span container>
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            object obj = this.GetObjByID(this.MultiViewID);
            if (obj != null && obj is MultiView)
            {
                MultiView multiView = (MultiView)obj;
                int index;
                if (Int32.TryParse(eventArgument, out index))
                {
                    TabChangingEventArgs args = new TabChangingEventArgs(index);
                    OnTabChanging(args);
                    if (args.Cancel)
                        return;
                    multiView.ActiveViewIndex = index;
                    this.ActiveIndex = index;
                }
            }
        }
    }

    public class WebMultiViewCaptionCollection : InfoOwnerCollection
    {
        public WebMultiViewCaptionCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(WebMultiViewCaption))
        {
        }

        public new WebMultiViewCaption this[int index]
        {
            get
            {
                return (WebMultiViewCaption)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is WebMultiViewCaption)
                    {
                        //原来的Collection设置为0
                        ((WebMultiViewCaption)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((WebMultiViewCaption)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class WebMultiViewCaption : InfoOwnerCollectionItem
    {
        public WebMultiViewCaption()
        {
        }

        [NotifyParentProperty(true)]
        public override string Name
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
            }
        }

        private string _Caption;
        [NotifyParentProperty(true)]
        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
            }
        }

        private bool _Visible = true;
        [DefaultValue(true)]
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        public override string ToString()
        {
            return _Caption;
        }
    }

    public class MultiViewEditor : UITypeEditor
    {
        public MultiViewEditor()
            : base()
        {
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand)]
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            List<string> objName = new List<string>();
            if (context.Instance != null)
            {
                ControlCollection ctrlList = ((Control)context.Instance).Page.Controls;
                foreach (Control ctrl in ctrlList)
                {
                    if (ctrl is MultiView)
                    {
                        objName.Add(ctrl.ID);
                    }
                }
            }

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (service != null)
            {
                StringListSelector selector = new StringListSelector(service, objName.ToArray());
                string strValue = (string)value;
                if (selector.Execute(ref strValue)) value = strValue;
            }
            return value;
        }
    }

    public delegate void TabChangedEventHandler(object sender, TabChangedEventArgs e);

    public sealed class TabChangedEventArgs : EventArgs
    {
        public TabChangedEventArgs(int activeIndex)
        {
            _ActiveIndex = activeIndex;
        }

        private int _ActiveIndex;
        public int ActiveIndex
        {
            get
            {
                return _ActiveIndex;
            }
        }
    }

    public delegate void TabChangingEventHandler(object sender, TabChangingEventArgs e);

    public sealed class TabChangingEventArgs : EventArgs
    {
        public TabChangingEventArgs(int newActiveIndex)
        {
            _NewActiveIndex = newActiveIndex;
            //_Cancel = canel;
        }

        private int _NewActiveIndex;
        public int NewActiveIndex
        {
            get
            {
                return _NewActiveIndex;
            }
        }

        private bool _Cancel;
        public bool Cancel
        {
            get
            {
                return _Cancel;
            }
            set
            {
                _Cancel = value;
            }
        }
    }
}
