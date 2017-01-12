using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Srvtools;
using System.IO;
using System.Text;
using System.Collections.Generic;

public partial class InnerPages_FlowUploadFiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string target = Request.Params["__EVENTTARGET"];
        //string param = Request.Params["__EVENTARGUMENT"];
        //if (!string.IsNullOrEmpty(target) && !string.IsNullOrEmpty(param))
        //{ 
            
        //}
    }

    protected void delContainer_Load(object sender, EventArgs e)
    {
        string attachments = Request.QueryString["ATTACHMENTS"];
        if (!string.IsNullOrEmpty(attachments))
        {
            string[] files = attachments.Split(';');
            foreach (string file in files)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    CheckBox chk = new CheckBox();
                    chk.Text = file;
                    chk.AutoPostBack = true;
                    chk.CheckedChanged += new EventHandler(chk_CheckedChanged);
                    this.delContainer.ContentTemplateContainer.Controls.Add(chk);
                }
            }
        }
        else
        {
            this.btnDelete.Enabled = false;
        }
    }

    void chk_CheckedChanged(object sender, EventArgs e)
    {
        List<string> delFiles = new List<string>();
        List<string> remFiles = new List<string>();
        foreach (Control ctrl in this.delContainer.ContentTemplateContainer.Controls)
        {
            if (ctrl is CheckBox)
            {
                CheckBox chk = ctrl as CheckBox;
                if (chk.Checked)
                {
                    delFiles.Add(chk.Text);
                }
                else
                {
                    remFiles.Add(chk.Text);
                }
            }
        }
        this.ViewState["DeleteFiles"] = delFiles;
        this.ViewState["RemainFiles"] = remFiles;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //if (!isFlowFilesBySolutions())
        //{ 
        //    throw new Exception("FlowFilesBySolutions is false, please contact administrator.");
        //}
        //if(string.IsNullOrEmpty(this.Request["VDSNAME"]))
        //{
        //    throw new Exception("VDSName is empty, please contact administrator.");
        //}


        string spath = Server.MapPath("..") + "\\WorkflowFiles\\" + (isFlowFilesBySolutions() ? this.Request["VDSNAME"] + "\\" : "");
        if (!Directory.Exists(spath))
        {
            Directory.CreateDirectory(spath);
        }
        string file1 = upload(this.FileUpload1, spath);
        string file2 = upload(this.FileUpload2, spath);
        string file3 = upload(this.FileUpload3, spath);
        string file4 = upload(this.FileUpload4, spath);
        string file5 = upload(this.FileUpload5, spath);

        string files = Request.QueryString["ATTACHMENTS"] + (string.IsNullOrEmpty(file1) ? "" : ";" + file1) +
            (string.IsNullOrEmpty(file2) ? "" : ";" + file2) +
            (string.IsNullOrEmpty(file3) ? "" : ";" + file3) +
            (string.IsNullOrEmpty(file4) ? "" : ";" + file4) +
            (string.IsNullOrEmpty(file5) ? "" : ";" + file5);
        if (files.StartsWith(";")) files = files.Substring(1);
        this.ClientScript.RegisterStartupScript(this.GetType(), "upload", string.Format("setfiles('{0}');", HttpUtility.UrlEncode(files)), true);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (this.ViewState["DeleteFiles"] != null && this.ViewState["DeleteFiles"] is List<string>)
        {
            List<string> delFiles = this.ViewState["DeleteFiles"] as List<string>;
            foreach (string file in delFiles)
            {
                string path = Server.MapPath("..") + "\\WorkflowFiles\\" + (isFlowFilesBySolutions() ? this.Request["VDSNAME"] + "\\" : "") + file;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
        if (this.ViewState["RemainFiles"] != null && this.ViewState["RemainFiles"] is List<string>)
        {
            List<string> remFiles = this.ViewState["RemainFiles"] as List<string>;
            string rem = "";
            foreach (string file in remFiles)
            {
                rem += string.Format(";{0}", file);
            }
            this.ClientScript.RegisterStartupScript(this.GetType(), "filterfiles", string.Format("setfiles('{0}');", HttpUtility.UrlEncode(rem)), true);
        }
    }

    private bool isFlowFilesBySolutions()
    {
        string config_FilesBySol = ConfigurationManager.AppSettings["FlowFilesBySolutions"];
        if (!string.IsNullOrEmpty(config_FilesBySol) && string.Compare(config_FilesBySol, "true", true) == 0)
            return true;
        return false;
    }

    private string upload(FileUpload fileUpload, string spath)
    {
        if (fileUpload.HasFile)
        {
            bool uploadSucceed = tryUpload(fileUpload, spath + fileUpload.FileName);

            if (uploadSucceed) return fileUpload.FileName;
            int i = 1;
            while (!uploadSucceed)
            {
                string extendedName = i.ToString();

                if (extendedName.Length == 1) extendedName = "-00" + extendedName;
                else if (extendedName.Length == 2) extendedName = "-0" + extendedName;

                if (fileUpload.FileName.IndexOf('.') != -1)
                {
                    uploadSucceed = tryUpload(fileUpload, spath + fileUpload.FileName.Insert(fileUpload.FileName.LastIndexOf('.'), extendedName));
                    if (uploadSucceed) return fileUpload.FileName.Insert(fileUpload.FileName.LastIndexOf('.'), extendedName);
                }
                else
                {
                    uploadSucceed = tryUpload(fileUpload, spath + extendedName);
                    if (uploadSucceed) return fileUpload.FileName;
                }
                i++;
            }
        }
        return "";
    }

    private bool tryUpload(FileUpload fileUpload, string serverFileName)
    {
        if (File.Exists(serverFileName)) return false;
        fileUpload.SaveAs(serverFileName);
        return true;
    }
}
