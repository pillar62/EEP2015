using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Srvtools
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:FLWebSigntureDataSource runat=server></{0}:FLWebSigntureDataSource>")]
    public class FLWebSigntureDataSource : WebDataSource
    {
        public new String SelectAlias
        {
            get
            {
                return base.SelectAlias;
            }
            set
            {
                base.SelectAlias = value;
                if (!String.IsNullOrEmpty(value) && this.DesignMode)
                {
                    this.SelectCommand = "SELECT UPDATE_DATE,UPDATE_TIME,USER_ID,USERNAME,REMARK,STATUS,S_STEP_ID FROM SYS_TODOHIS";
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
            if (this.Page != null)
            {
                //if (!this.Page.IsPostBack)
                //{
                    this.ViewState["ListId"] = this.Page.Request.QueryString["LISTID"];
                    this.ViewState["ATTACHMENTS"] = this.Page.Request.QueryString["ATTACHMENTS"];
                    this.ViewState["VDSNAME"] = this.Page.Request.QueryString["VDSNAME"];
                    GenSource();
                //}
            }
        }

        private void GenSource()
        {
            string sql = "";
            if (this.ViewState["ListId"] != null && this.ViewState["ListId"].ToString() != "")
            {
                sql = "SELECT SYS_TODOHIS.FLOW_DESC,SYS_TODOHIS.S_ROLE_ID,SYS_TODOHIS.S_STEP_ID,SYS_TODOHIS.USER_ID,SYS_TODOHIS.USERNAME,SYS_TODOHIS.STATUS,SYS_TODOHIS.UPDATE_DATE,SYS_TODOHIS.UPDATE_TIME,SYS_TODOHIS.REMARK,SYS_TODOHIS.ATTACHMENTS,SYS_TODOHIS.FORM_PRESENT_CT,GROUPS.GROUPNAME FROM SYS_TODOHIS LEFT JOIN GROUPS ON SYS_TODOHIS.S_ROLE_ID = GROUPS.GROUPID WHERE (SYS_TODOHIS.LISTID = '" + this.ViewState["ListId"].ToString() + "') ORDER BY SYS_TODOHIS.UPDATE_DATE,SYS_TODOHIS.UPDATE_TIME";
                object[] ret1 = CliUtils.CallMethod("GLModule", "ExcuteWorkFlow", new object[] { sql });
                if (ret1 != null && (int)ret1[0] == 0)
                {
                    DataTable dt = ((DataSet)ret1[1]).Tables[0].DefaultView.Table;
                    String[] lstStatus = SysMsg.GetSystemMessage(CliUtils.fClientLang, "FLDesigner", "FLDesigner", "Item3", true).Split(',');
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        String formatValue = String.Empty;
                        switch (dt.Rows[i]["STATUS"].ToString())
                        {
                            case "Z":
                                formatValue = lstStatus[0];
                                break;
                            case "N":
                                formatValue = lstStatus[1];
                                break;
                            case "NR":
                                formatValue = lstStatus[2];
                                break;
                            case "NF":
                                formatValue = lstStatus[3];
                                break;
                            case "X":
                                formatValue = lstStatus[4];
                                break;
                            case "A":
                                formatValue = lstStatus[5];
                                break;
                            case "V":
                                formatValue = lstStatus[6];
                                break;
                        }
                        dt.Rows[i]["STATUS"] = formatValue;
                    }

                    this.DataSource = dt.DefaultView;
                    this.InnerDataSet = (DataSet)ret1[1];
                    this.DataMember = dt.TableName;

                    this.DataBind();
                }
            }
            else
            {
                this.SelectCommand = "SELECT UPDATE_DATE,UPDATE_TIME,USER_ID,USERNAME,REMARK,STATUS,S_STEP_ID FROM SYS_TODOHIS WHERE 1=0";
            }
        }
    }
}
