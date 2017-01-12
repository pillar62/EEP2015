using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Srvtools;

/// <summary>
/// Summary description for CRM_function
/// </summary>
public class CRM_function
{
    public CRM_function()
    {
        //
        // TODO: Add constructor logic here
        //

    }

    public static string ReCommon(string Nos)
    {
        string Common_name = "";
        DataSet dsComName = CliUtils.ExecuteSql("CRMserver", "crm_cmdtenp", "select COMMON_NAME from CRM_COMMON where COMMON_ID = '" + Nos + "'", true, CliUtils.fCurrentProject);
        if (dsComName.Tables[0].Rows.Count != 0)
        {
            Common_name = dsComName.Tables[0].Rows[0][0].ToString();
        }
        return Common_name;
    }

    public static string ReMode(string ID)
    {
        string Modestr = "";
        DataSet dsComName = CliUtils.ExecuteSql("CRMserver", "crm_cmdtenp", "select GradeContent from CRM_Grade where GradeName = '" + ID + "'", true, CliUtils.fCurrentProject);
        if (dsComName.Tables[0].Rows.Count != 0)
        {
            Modestr = dsComName.Tables[0].Rows[0][0].ToString();
        }
        return Modestr;
    }

    public static string ReCrmSysManage(string Formstr)
    {
        // Get CRM System Manager
        string SysManageID = "";
        DataSet dsetSysManageID = new DataSet();
        object[] objParam = new object[1];
        objParam[0] = Formstr;
        object[] myRet = CliUtils.CallMethod("CRMserver", "GetCRMSysManageID", objParam);
        if (myRet != null && (int)myRet[0] == 0)
        {
            dsetSysManageID = (DataSet)myRet[1];
            int dsLen = dsetSysManageID.Tables[0].Rows.Count;
            if (dsLen > 0)
            {
                SysManageID = dsetSysManageID.Tables[0].Rows[0][0].ToString();
                for (int n = 1; n < dsLen; n++)
                {
                    SysManageID += "," + dsetSysManageID.Tables[0].Rows[n][0].ToString();
                }
            }
        }
        return SysManageID;
    }

}
