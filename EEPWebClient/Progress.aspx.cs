using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebProgressBar
{
	/// <summary>
	/// Summary description for Progress.
	/// </summary>
	public partial class Progress : System.Web.UI.Page
	{
	
		private int state = 0;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
            if (Session["State"] != null)
			{
				state = Convert.ToInt32(Session["State"].ToString());
			}
			else
			{
				Session["State"]=0;
			}
            if (state > 0 && state <= 10)
            {
                this.lblMessages.Text = "Task undertaking!";
                this.panelProgress.Width = state * 30;
                this.lblPercent.Text = state * 10 + "%";
                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>window.setTimeout('window.Form1.submit()',100);</script>");
            }
            if (state == 100)
            {
                this.panelProgress.Visible = false;
                this.panelBarSide.Visible = false;
                this.lblMessages.Text = "Task Completed!";
                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>window.close();</script>");
            }
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
