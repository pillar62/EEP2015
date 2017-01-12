using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using AjaxControlToolkit;
using System.ComponentModel.Design;
using System.Drawing.Design;
using Srvtools;
using System.Web.UI.HtmlControls;

namespace AjaxTools
{
    public class AjaxCalendar : TextBox, INamingContainer
    {
        public AjaxCalendar()
        {
            if (string.IsNullOrEmpty(this.CalenderCssClass))
                this.CalenderCssClass = "AjaxCalendar";
        }

        private CalendarExtender _calendarExtender = new CalendarExtender();
        private SYS_LANGUAGE language;

        [Category("Infolight")]
        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        [Category("Infolight")]
        public bool Animated
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.Animated;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.Animated = value;
            }
        }

        [Category("Infolight"), DefaultValue(true)]
        public bool Display
        {
            get
            {
                object obj = this.ViewState["Display"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["Display"] = value;
            }
        }

        [Category("Infolight")]
        public string BehaviorID
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.BehaviorID;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.BehaviorID = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue("AjaxCalendar")]
        public string CalenderCssClass
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.CssClass;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.CssClass = value;
            }
        }

        [Category("Infolight")]
        public bool EnabledOnClient
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.EnabledOnClient;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.EnabledOnClient = value;
            }
        }

        [Category("Infolight")]
        public FirstDayOfWeek FirstDayOfWeek
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.FirstDayOfWeek;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.FirstDayOfWeek = value;
            }
        }

        [Category("Infolight")]
        public string Format
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.Format;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.Format = value;
            }
        }

        [Category("Infolight")]
        public string OnClientDateSelectionChanged
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.OnClientDateSelectionChanged;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.OnClientDateSelectionChanged = value;
            }
        }

        [Category("Infolight")]
        public string OnClientHidden
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.OnClientHidden;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.OnClientHidden = value;
            }
        }

        [Category("Infolight")]
        public string OnClientHiding
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.OnClientHiding;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.OnClientHiding = value;
            }
        }

        [Category("Infolight")]
        public string OnClientShowing
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.OnClientShowing;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.OnClientShowing = value;
            }
        }

        [Category("Infolight")]
        public string OnClientShown
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.OnClientShown;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.OnClientShown = value;
            }
        }

        [Category("Infolight")]
        public string PopupButtonID
        {
            get
            {
                EnsureChildControls();
                return this._calendarExtender.PopupButtonID;
            }
            set
            {
                EnsureChildControls();
                this._calendarExtender.PopupButtonID = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool CheckDate
        {
            get
            {
                object obj = this.ViewState["CheckDate"];
                if (obj != null)
                {
                    return (bool)obj;
                }
                return true;
            }
            set
            {
                this.ViewState["CheckDate"] = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                EnsureChildControls();
                this._calendarExtender.ID = "_calendarExtender";
                this._calendarExtender.TargetControlID = this.ClientID;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (!this.DesignMode)
            {
                _calendarExtender = new CalendarExtender();
                _calendarExtender.ID = "_calendarExtender";
                //_calendarExtender.OnClientDateSelectionChanged = this.ClientID + "SelDate";
                this.Controls.Add(_calendarExtender);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.Display)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            if (this.CheckDate)
            {
                string blurScript = "if(!checkdate(" + this.UniqueID + ".value)) {" + this.ClientID + ".focus();" + this.ClientID + ".value='';}";
                writer.AddAttribute("onblur", blurScript);
            }
            //this.RenderBeginTag(writer);
            //base.RenderContents(writer);
            //this.RenderEndTag(writer);
            base.Render(writer);
            if (!this.DesignMode)
            {
                _calendarExtender.OnClientDateSelectionChanged = this.ClientID + "SelDate";
                _calendarExtender.RenderControl(writer);
            }
            ClientScriptManager csm = Page.ClientScript;
            if (!csm.IsClientScriptBlockRegistered("clientScript"))
            {
                //language = CliSysMegLag.GetClientLanguage();
                language = CliUtils.fClientLang;
                string message = SysMsg.GetSystemMessage(language, "Srvtools", "WebDateTimePicker", "DateTimeValidate", true);
                
                writer.WriteLine("<script language=\"JavaScript\">");

                writer.WriteLine("function checkdate(inpar)");
                writer.WriteLine("{");
                //writer.WriteLine("var flag=true;");
                writer.WriteLine("var flagM = false;");
                writer.WriteLine("var flagY = false;");
                writer.WriteLine("var flagD = false;");
                writer.WriteLine("var getdate = inpar;");
                writer.WriteLine("if(getdate != \"\")");
                writer.WriteLine("{");
                writer.WriteLine("var datepart = getdate.split('');");
                writer.WriteLine("var syear='', smonth='', sday='';");
                writer.WriteLine("var f = 0;");
                writer.WriteLine("for(var i = 0; i < datepart.length; i++)");
                writer.WriteLine("{");
                writer.WriteLine("var j = datepart[i];");
                writer.WriteLine("if(parseInt(j) <= 9 && parseInt(j) >= 0)");
                writer.WriteLine("{");

                writer.WriteLine("if(f == 0)");
                writer.WriteLine("{");
                switch (this.Format)
                {
                    case "d":
                    case "MM/dd/yyyy":
                    case "MM-dd-yyyy":
                        writer.WriteLine("smonth += j;");
                        break;
                    case "yyyy/MM/dd":
                    case "yyyy-MM-dd":
                        writer.WriteLine("syear += j;");
                        break;
                }
                writer.WriteLine("}");
                writer.WriteLine("else if(f == 1)");
                writer.WriteLine("{");
                switch (this.Format)
                {
                    case "d":
                    case "MM/dd/yyyy":
                    case "MM-dd-yyyy":
                        writer.WriteLine("sday += j;");
                        break;
                    case "yyyy/MM/dd":
                    case "yyyy-MM-dd":
                        writer.WriteLine("smonth += j;");
                        break;
                }
                writer.WriteLine("}");
                writer.WriteLine("else if(f == 2)");
                writer.WriteLine("{");
                switch (this.Format)
                {
                    case "d":
                    case "MM/dd/yyyy":
                    case "MM-dd-yyyy":
                        writer.WriteLine("syear += j;");
                        break;
                    case "yyyy/MM/dd":
                    case "yyyy-MM-dd":
                        writer.WriteLine("sday += j;");
                        break;
                }
                writer.WriteLine("}");

                writer.WriteLine("}");
                writer.WriteLine("else");
                writer.WriteLine("{");
                writer.WriteLine("f++;");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("var year = parseInt(syear);");
                //if (TraDate)
                //{
                //    writer.WriteLine("year = year + 1911;");
                //}
                writer.WriteLine("if (smonth.indexOf('0') == 0)");
                writer.WriteLine("{");
                writer.WriteLine("smonth = smonth.substring(1);");
                writer.WriteLine("}");
                writer.WriteLine("if (sday.indexOf('0') == 0)");
                writer.WriteLine("{");
                writer.WriteLine("sday = sday.substring(1);");
                writer.WriteLine("}");
                writer.WriteLine("var month = parseInt(smonth);");
                writer.WriteLine("var day = parseInt(sday);");
                //判断年份是否格式正确
                writer.WriteLine("if(year <= 9999 && year >= 1) { flagY=true; }");
                // 判断月份是否格式正确
                writer.WriteLine("if(month <= 12 && month >= 1) { flagM=true; }");
                // 判断4,6,9,11月份
                writer.WriteLine("if (month==4 || month==6 || month==9 || month==11)");
                writer.WriteLine("{");
                writer.WriteLine("if(day<=30 && day>=1)");
                writer.WriteLine("{");
                writer.WriteLine("flagD=true;");
                writer.WriteLine("}");
                writer.WriteLine("}");
                // 判断2月份
                writer.WriteLine("else if (month==2)");
                writer.WriteLine("{");
                writer.WriteLine("if (LeapYear(year))");
                writer.WriteLine("{");
                writer.WriteLine("if (day<=29 && day>=1){ flagD=true; }");
                writer.WriteLine("}");
                writer.WriteLine("else");
                writer.WriteLine("{");
                writer.WriteLine("if (day<=28 && day>=1){flagD=true; } ");
                writer.WriteLine("}");
                writer.WriteLine("}");
                // 判断1,3,5,7,8,10,12月份
                writer.WriteLine("else");
                writer.WriteLine("{");
                writer.WriteLine("if(day<=31 && day >= 1)");
                writer.WriteLine("{");
                writer.WriteLine("flagD=true;");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("if (flagY==false || flagM==false || flagD==false)");
                writer.WriteLine("{");
                writer.WriteLine("alert('" + message + "');");
                writer.WriteLine("}");
                writer.WriteLine("return (flagY && flagM && flagD);");
                writer.WriteLine("}");
                writer.WriteLine("else {return true;}");
                writer.WriteLine("}");


                // 判断当前年是否为闰年
                writer.WriteLine("function LeapYear(intYear)");
                writer.WriteLine("{");
                writer.WriteLine("if (intYear % 100 == 0)");
                writer.WriteLine("{");
                writer.WriteLine("if (intYear % 400 == 0) { return true; }");
                writer.WriteLine("}");
                writer.WriteLine("else");
                writer.WriteLine("{");
                writer.WriteLine("if ((intYear % 4) == 0) { return true; }");
                writer.WriteLine("}");
                writer.WriteLine("return false;");
                writer.WriteLine("}");

                writer.WriteLine("function " + this.OnClientDateSelectionChanged + "()");
                writer.WriteLine("{");
                writer.WriteLine("$find('" + _calendarExtender.BehaviorID + "').hide();");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
                csm.RegisterClientScriptBlock(this.GetType(), "clientScript", "");
            }
        }
    }
}
