using EFClientTools.Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace EFClientTools
{
    [Designer(typeof(DataSourceDesigner), typeof(IDesigner))]
    public class WebDataPager : WebControl, IPostBackEventHandler
    {
        private string _DataSourceID;
        [Category("Infolight"),
        Description("The ID of WebDataSource which the control is bound to")]
        [Editor(typeof(DataSourceEditor), typeof(UITypeEditor))]
        public string DataSourceID
        {
            get
            {
                return _DataSourceID;
            }
            set
            {
                _DataSourceID = value;
            }
        }

        private string _BindingObject;
        [Category("Infolight"),
        Description("Specifies the control to bind to")]
        //[Editor(typeof(GridEditor), typeof(UITypeEditor))]
        public string BindingObject
        {
            get
            {
                return _BindingObject;
            }
            set
            {
                _BindingObject = value;
            }
        }

        private WebDataSource _CurrentDataSource;
        public WebDataSource CurrentDataSource
        {
            get
            {
                if (!this.DesignMode && _CurrentDataSource == null)
                {
                    if (this.Page != null && DataSourceID != null)
                    {
                        _CurrentDataSource = this.Page.FindControl(DataSourceID) as WebDataSource;
                    }
                }
                return _CurrentDataSource;
            }
        }

        private object _CurrentBindingObject;
        public object CurrentBindingObject
        {
            get
            {
                if (!this.DesignMode && _CurrentBindingObject == null)
                {
                    if (this.Page != null && BindingObject != null)
                    {
                        _CurrentBindingObject = this.Page.FindControl(BindingObject);
                    }
                }
                return _CurrentBindingObject;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderContents(writer);

            int count = (CurrentDataSource as WebDataSource).View.Table.Rows.Count;
            int allCount = (CurrentDataSource as WebDataSource).GetRecordsCount();
            int pageIndex = Convert.ToInt32(CurrentBindingObject.GetType().GetProperty("PageIndex").GetValue(CurrentBindingObject, null));
            int pageSize = 1;
            if (CurrentBindingObject.GetType().Name == "ASPxGridView")
            {
                var settingsPager = CurrentBindingObject.GetType().GetProperty("SettingsPager").GetValue(CurrentBindingObject, null);
                pageSize = Convert.ToInt16(settingsPager.GetType().GetProperty("PageSize").GetValue(settingsPager, null));
            }
            float maxPageCount = float.Parse(allCount.ToString()) / float.Parse(pageSize.ToString());
            if ((int)(maxPageCount) < maxPageCount)
            {
                maxPageCount = (int)(maxPageCount) + 1;

            }
            float pageCountT = float.Parse(count.ToString()) / float.Parse(pageSize.ToString());
            int pageCount = count / pageSize;
            if (pageCount < pageCountT)
            {
                pageCount += 1;
            }
            int startIndex = pageIndex / 10 * 10;
            ClientScriptManager csm = Page.ClientScript;

            int i = -1;
            if (startIndex > 0)
            {
                String command = String.Format("PAGE{0}", startIndex + i);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + csm.GetPostBackEventReference(this, command));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("...");
                writer.RenderEndTag();
            }

            for (i = 0; i < 10; i++)
            {
                if (startIndex + i + 1 > pageCount)
                    break;
                String text = String.Format("{0}", startIndex + i + 1);
                String command = String.Format("PAGE{0}", startIndex + i);
                if (pageIndex != startIndex + i)
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + csm.GetPostBackEventReference(this, command));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(text);
                writer.RenderEndTag();

                writer.Write("&nbsp;&nbsp;");
            }

            if (startIndex + i < maxPageCount)
            //if (!(CurrentDataSource as WebDataSource).Eof)
            {
                String command = String.Format("NEXT{0}", startIndex + i);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + csm.GetPostBackEventReference(this, command));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("...");
                writer.RenderEndTag();
            }
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            //BeforeCommandArgs bArgs = new BeforeCommandArgs(eventArgument, null);
            ////OnBeforeCommand(bArgs);
            //if (!bArgs.Cancel)
            OnCommand(new CommandEventArgs(eventArgument, null));
        }

        private static readonly object EventCommand = new object();
        protected virtual void OnCommand(CommandEventArgs e)
        {
            LinkButton_Click(e);
            CommandEventHandler clickHandler = (CommandEventHandler)Events[EventCommand];
            if (clickHandler != null)
            {
                clickHandler(this, e);
            }
        }

        void LinkButton_Click(CommandEventArgs e)
        {
            if (e.CommandName.StartsWith("NEXT"))
            {
                //if (!(CurrentDataSource as WebDataSource).Eof)
                {
                    (CurrentDataSource as WebDataSource).GetNextPacket();
                    CurrentBindingObject.GetType().GetMethod("DataBind").Invoke(CurrentBindingObject, null);
                }
                int pageIndex = Convert.ToInt32(e.CommandName.Replace("NEXT", ""));
                CurrentBindingObject.GetType().GetProperty("PageIndex").SetValue(CurrentBindingObject, pageIndex, null);
            }
            else
            {
                int pageIndex = Convert.ToInt32(e.CommandName.Replace("PAGE", ""));
                CurrentBindingObject.GetType().GetProperty("PageIndex").SetValue(CurrentBindingObject, pageIndex, null);
            }

        }
    }
}
