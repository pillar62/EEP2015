using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Srvtools;
using Microsoft.Win32;
using System.Data;
using FLTools;

/// <summary>
/// Summary description for WFService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class FLService : System.Web.Services.WebService
{
    #region -----------------SQL-----------------

    // 0:user       1:dtstring
    private static string SQL_TODOLIST1 = @"
        SELECT SYS_TODOLIST.LISTID,SYS_TODOLIST.FLOW_ID, SYS_TODOLIST.FLOW_DESC, SYS_TODOLIST.APPLICANT, 
            SYS_TODOLIST.S_USER_ID, SYS_TODOLIST.S_STEP_ID, SYS_TODOLIST.S_STEP_DESC, SYS_TODOLIST.D_STEP_ID, SYS_TODOLIST.D_STEP_DESC, 
            SYS_TODOLIST.EXP_TIME, SYS_TODOLIST.URGENT_TIME, SYS_TODOLIST.TIME_UNIT, SYS_TODOLIST.USERNAME, SYS_TODOLIST.FORM_NAME, 
            SYS_TODOLIST.NAVIGATOR_MODE, SYS_TODOLIST.FLNAVIGATOR_MODE, SYS_TODOLIST.PARAMETERS, SYS_TODOLIST.SENDTO_KIND, SYS_TODOLIST.SENDTO_ID, 
            SYS_TODOLIST.SENDTO_NAME, SYS_TODOLIST.FLOWIMPORTANT, SYS_TODOLIST.FLOWURGENT, SYS_TODOLIST.STATUS, SYS_TODOLIST.FORM_TABLE, 
            SYS_TODOLIST.FORM_KEYS, SYS_TODOLIST.FORM_PRESENTATION, SYS_TODOLIST.FORM_PRESENT_CT, SYS_TODOLIST.REMARK, SYS_TODOLIST.PROVIDER_NAME, 
            SYS_TODOLIST.VERSION, SYS_TODOLIST.EMAIL_ADD, SYS_TODOLIST.EMAIL_STATUS, SYS_TODOLIST.VDSNAME, SYS_TODOLIST.SENDBACKSTEP, 
            SYS_TODOLIST.LEVEL_NO, SYS_TODOLIST.WEBFORM_NAME, CONCAT(SYS_TODOLIST.UPDATE_DATE,' ',SYS_TODOLIST.UPDATE_TIME) AS UPDATE_WHOLE_TIME, 
            SYS_TODOLIST.FLOWPATH, SYS_TODOLIST.PLUSAPPROVE, SYS_TODOLIST.PLUSROLES, SYS_TODOLIST.MULTISTEPRETURN, SYS_TODOLIST.ATTACHMENTS 
        FROM SYS_TODOLIST 
        WHERE (SYS_TODOLIST.SENDTO_KIND = '1'AND SYS_TODOLIST.STATUS<>'F' AND ((SYS_TODOLIST.SENDTO_ID IN (Select GROUPID from GROUPS WHERE 
            GROUPID IN (Select GROUPID from USERGROUPS Where USERID='{0}')  AND ISROLE='Y') OR SYS_TODOLIST.SENDTO_ID IN (Select ROLE_ID AS 
             GROUPID From SYS_ROLES_AGENT where SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND (SYS_ROLES_AGENT.FLOW_DESC='*' OR 
            SYS_ROLES_AGENT.FLOW_DESC IS NULL OR SYS_TODOLIST.FLOW_DESC=SYS_ROLES_AGENT.FLOW_DESC) AND AGENT='{0}' AND START_DATE+START_TIME<=
            '{1}' AND END_DATE+END_TIME>='{1}')) AND (SYS_TODOLIST.S_USER_ID<>'{0}' OR SYS_TODOLIST.STATUS<>'F')) OR (SYS_TODOLIST.SENDTO_KIND = '2' 
            AND SYS_TODOLIST.SENDTO_ID='{0}')) ORDER BY SYS_TODOLIST.UPDATE_DATE,SYS_TODOLIST.UPDATE_TIME";

    // 0:user    1: dtString      2:connectMark
    private static string SQL_TODOLIST2 = @"
        SELECT SYS_TODOLIST.LISTID,SYS_TODOLIST.FLOW_ID, SYS_TODOLIST.FLOW_DESC, SYS_TODOLIST.APPLICANT, SYS_TODOLIST.S_USER_ID, SYS_TODOLIST.S_STEP_ID, 
            SYS_TODOLIST.S_STEP_DESC, SYS_TODOLIST.D_STEP_ID, SYS_TODOLIST.D_STEP_DESC, SYS_TODOLIST.EXP_TIME, SYS_TODOLIST.URGENT_TIME, 
            SYS_TODOLIST.TIME_UNIT, SYS_TODOLIST.USERNAME, SYS_TODOLIST.FORM_NAME, SYS_TODOLIST.NAVIGATOR_MODE, SYS_TODOLIST.FLNAVIGATOR_MODE, 
            SYS_TODOLIST.PARAMETERS, SYS_TODOLIST.SENDTO_KIND, SYS_TODOLIST.SENDTO_ID, SYS_TODOLIST.SENDTO_NAME, SYS_TODOLIST.FLOWIMPORTANT, 
            SYS_TODOLIST.FLOWURGENT, SYS_TODOLIST.STATUS, SYS_TODOLIST.FORM_TABLE, SYS_TODOLIST.FORM_KEYS, SYS_TODOLIST.FORM_PRESENTATION, 
            SYS_TODOLIST.FORM_PRESENT_CT, SYS_TODOLIST.REMARK, SYS_TODOLIST.PROVIDER_NAME, SYS_TODOLIST.VERSION, SYS_TODOLIST.EMAIL_ADD, 
            SYS_TODOLIST.EMAIL_STATUS, SYS_TODOLIST.VDSNAME, SYS_TODOLIST.SENDBACKSTEP, SYS_TODOLIST.LEVEL_NO, SYS_TODOLIST.WEBFORM_NAME, 
            (SYS_TODOLIST.UPDATE_DATE {2} ' ' {2} SYS_TODOLIST.UPDATE_TIME) AS UPDATE_WHOLE_TIME, SYS_TODOLIST.FLOWPATH, SYS_TODOLIST.PLUSAPPROVE, 
            SYS_TODOLIST.PLUSROLES, SYS_TODOLIST.MULTISTEPRETURN, SYS_TODOLIST.ATTACHMENTS 
        FROM SYS_TODOLIST 
        WHERE ((SYS_TODOLIST.SENDTO_KIND = '1' AND SYS_TODOLIST.STATUS<>'F' AND ((SYS_TODOLIST.SENDTO_ID IN ((Select GROUPID from GROUPS WHERE 
            GROUPID IN (Select GROUPID from USERGROUPS Where USERID='{0}')  AND ISROLE='Y')  UNION (Select ROLE_ID AS GROUPID From 
            SYS_ROLES_AGENT where SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC  
            IS NULL OR SYS_TODOLIST.FLOW_DESC=SYS_ROLES_AGENT.FLOW_DESC) AND AGENT='{0}' AND START_DATE+START_TIME<='{1}' AND END_DATE+END_TIME>='{1}')))) 
            OR (SYS_TODOLIST.SENDTO_KIND = '2' AND SYS_TODOLIST.SENDTO_ID='{0}' AND SYS_TODOLIST.STATUS<>'F'))) ORDER BY SYS_TODOLIST.UPDATE_DATE,
            SYS_TODOLIST.UPDATE_TIME";

    // 0:user       1:dtstring
    private static string SQL_TODOLIST3 = @"
        SELECT SYS_TODOLIST.LISTID,SYS_TODOLIST.FLOW_ID, SYS_TODOLIST.FLOW_DESC, SYS_TODOLIST.APPLICANT, 
            SYS_TODOLIST.S_USER_ID, SYS_TODOLIST.S_STEP_ID, SYS_TODOLIST.S_STEP_DESC, SYS_TODOLIST.D_STEP_ID, SYS_TODOLIST.D_STEP_DESC, 
            SYS_TODOLIST.EXP_TIME, SYS_TODOLIST.URGENT_TIME, SYS_TODOLIST.TIME_UNIT, SYS_TODOLIST.USERNAME, SYS_TODOLIST.FORM_NAME, 
            SYS_TODOLIST.NAVIGATOR_MODE, SYS_TODOLIST.FLNAVIGATOR_MODE, SYS_TODOLIST.PARAMETERS, SYS_TODOLIST.SENDTO_KIND, SYS_TODOLIST.SENDTO_ID, 
            SYS_TODOLIST.SENDTO_NAME, SYS_TODOLIST.FLOWIMPORTANT, SYS_TODOLIST.FLOWURGENT, SYS_TODOLIST.STATUS, SYS_TODOLIST.FORM_TABLE, 
            SYS_TODOLIST.FORM_KEYS, SYS_TODOLIST.FORM_PRESENTATION, SYS_TODOLIST.FORM_PRESENT_CT, SYS_TODOLIST.REMARK, SYS_TODOLIST.PROVIDER_NAME, 
            SYS_TODOLIST.VERSION, SYS_TODOLIST.EMAIL_ADD, SYS_TODOLIST.EMAIL_STATUS, SYS_TODOLIST.VDSNAME, SYS_TODOLIST.SENDBACKSTEP, 
            SYS_TODOLIST.LEVEL_NO, SYS_TODOLIST.WEBFORM_NAME, CONCAT(SYS_TODOLIST.UPDATE_DATE,' ',SYS_TODOLIST.UPDATE_TIME) AS UPDATE_WHOLE_TIME, 
            SYS_TODOLIST.FLOWPATH, SYS_TODOLIST.PLUSAPPROVE, SYS_TODOLIST.PLUSROLES, SYS_TODOLIST.MULTISTEPRETURN, SYS_TODOLIST.ATTACHMENTS 
        FROM SYS_TODOLIST 
        WHERE SYS_TODOLIST.LISTID='{2}' AND SYS_TODOLIST.FLOWPATH='{3}' AND (SYS_TODOLIST.SENDTO_KIND = '1'AND SYS_TODOLIST.STATUS<>'F' AND ((SYS_TODOLIST.SENDTO_ID IN (Select GROUPID from GROUPS WHERE 
            GROUPID IN (Select GROUPID from USERGROUPS Where USERID='{0}')  AND ISROLE='Y') OR SYS_TODOLIST.SENDTO_ID IN (Select ROLE_ID AS 
            GROUPID From SYS_ROLES_AGENT where SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND (SYS_ROLES_AGENT.FLOW_DESC='*' OR 
            SYS_ROLES_AGENT.FLOW_DESC IS NULL OR SYS_TODOLIST.FLOW_DESC=SYS_ROLES_AGENT.FLOW_DESC) AND AGENT='{0}' AND START_DATE+START_TIME<=
            '{1}' AND END_DATE+END_TIME>='{1}')) AND (SYS_TODOLIST.S_USER_ID<>'{0}' OR SYS_TODOLIST.STATUS<>'F')) OR (SYS_TODOLIST.SENDTO_KIND = '2' 
            AND SYS_TODOLIST.SENDTO_ID='{0}')) ORDER BY SYS_TODOLIST.UPDATE_DATE,SYS_TODOLIST.UPDATE_TIME";

    // 0:user    1: dtString      2:connectMark
    private static string SQL_TODOLIST4 = @"
        SELECT SYS_TODOLIST.LISTID,SYS_TODOLIST.FLOW_ID, SYS_TODOLIST.FLOW_DESC, SYS_TODOLIST.APPLICANT, SYS_TODOLIST.S_USER_ID, SYS_TODOLIST.S_STEP_ID, 
            SYS_TODOLIST.S_STEP_DESC, SYS_TODOLIST.D_STEP_ID, SYS_TODOLIST.D_STEP_DESC, SYS_TODOLIST.EXP_TIME, SYS_TODOLIST.URGENT_TIME, 
            SYS_TODOLIST.TIME_UNIT, SYS_TODOLIST.USERNAME, SYS_TODOLIST.FORM_NAME, SYS_TODOLIST.NAVIGATOR_MODE, SYS_TODOLIST.FLNAVIGATOR_MODE, 
            SYS_TODOLIST.PARAMETERS, SYS_TODOLIST.SENDTO_KIND, SYS_TODOLIST.SENDTO_ID, SYS_TODOLIST.SENDTO_NAME, SYS_TODOLIST.FLOWIMPORTANT, 
            SYS_TODOLIST.FLOWURGENT, SYS_TODOLIST.STATUS, SYS_TODOLIST.FORM_TABLE, SYS_TODOLIST.FORM_KEYS, SYS_TODOLIST.FORM_PRESENTATION, 
            SYS_TODOLIST.FORM_PRESENT_CT, SYS_TODOLIST.REMARK, SYS_TODOLIST.PROVIDER_NAME, SYS_TODOLIST.VERSION, SYS_TODOLIST.EMAIL_ADD, 
            SYS_TODOLIST.EMAIL_STATUS, SYS_TODOLIST.VDSNAME, SYS_TODOLIST.SENDBACKSTEP, SYS_TODOLIST.LEVEL_NO, SYS_TODOLIST.WEBFORM_NAME, 
            (SYS_TODOLIST.UPDATE_DATE {2} ' ' {2} SYS_TODOLIST.UPDATE_TIME) AS UPDATE_WHOLE_TIME, SYS_TODOLIST.FLOWPATH, SYS_TODOLIST.PLUSAPPROVE, 
            SYS_TODOLIST.PLUSROLES, SYS_TODOLIST.MULTISTEPRETURN, SYS_TODOLIST.ATTACHMENTS 
        FROM SYS_TODOLIST 
        WHERE SYS_TODOLIST.LISTID='{3}' AND SYS_TODOLIST.FLOWPATH='{4}' AND ((SYS_TODOLIST.SENDTO_KIND = '1' AND SYS_TODOLIST.STATUS<>'F' AND ((SYS_TODOLIST.SENDTO_ID IN ((Select GROUPID from GROUPS WHERE 
            GROUPID IN (Select GROUPID from USERGROUPS Where USERID='{0}')  AND ISROLE='Y')  UNION (Select ROLE_ID AS GROUPID From 
            SYS_ROLES_AGENT where SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC 
            IS NULL OR SYS_TODOLIST.FLOW_DESC=SYS_ROLES_AGENT.FLOW_DESC) AND AGENT='{0}' AND START_DATE+START_TIME<='{1}' AND END_DATE+END_TIME>='{1}')))) 
            OR (SYS_TODOLIST.SENDTO_KIND = '2' AND SYS_TODOLIST.SENDTO_ID='{0}' AND SYS_TODOLIST.STATUS<>'F'))) ORDER BY SYS_TODOLIST.UPDATE_DATE,
            SYS_TODOLIST.UPDATE_TIME";

    // 0:user
    private static string SQL_TODOHIS1 = @"
        SELECT SYS_TODOLIST.LISTID, SYS_TODOLIST.FLOW_ID, SYS_TODOLIST.FLOW_DESC, SYS_TODOLIST.APPLICANT, SYS_TODOLIST.S_USER_ID, SYS_TODOLIST.S_STEP_ID, 
            SYS_TODOLIST.S_STEP_DESC, SYS_TODOLIST.D_STEP_ID, SYS_TODOLIST.D_STEP_DESC, SYS_TODOLIST.EXP_TIME, SYS_TODOLIST.URGENT_TIME, 
            SYS_TODOLIST.TIME_UNIT, SYS_TODOLIST.USERNAME, SYS_TODOLIST.FORM_NAME, SYS_TODOLIST.NAVIGATOR_MODE, SYS_TODOLIST.FLNAVIGATOR_MODE, 
            SYS_TODOLIST.PARAMETERS, SYS_TODOLIST.SENDTO_KIND, SYS_TODOLIST.SENDTO_ID, SYS_TODOLIST.SENDTO_NAME, SYS_TODOLIST.FLOWIMPORTANT, 
            SYS_TODOLIST.FLOWURGENT, SYS_TODOLIST.STATUS, SYS_TODOLIST.FORM_TABLE, SYS_TODOLIST.FORM_KEYS, SYS_TODOLIST.FORM_PRESENTATION, 
            SYS_TODOLIST.FORM_PRESENT_CT, SYS_TODOLIST.REMARK, SYS_TODOLIST.PROVIDER_NAME, SYS_TODOLIST.VERSION, SYS_TODOLIST.EMAIL_ADD, 
            SYS_TODOLIST.EMAIL_STATUS, SYS_TODOLIST.VDSNAME, SYS_TODOLIST.SENDBACKSTEP, SYS_TODOLIST.LEVEL_NO, SYS_TODOLIST.WEBFORM_NAME, 
            CONCAT(SYS_TODOLIST.UPDATE_DATE,' ',SYS_TODOLIST.UPDATE_TIME) AS UPDATE_WHOLE_TIME, SYS_TODOLIST.FLOWPATH, SYS_TODOLIST.PLUSAPPROVE, 
            SYS_TODOLIST.PLUSROLES, SYS_TODOLIST.MULTISTEPRETURN FROM SYS_TODOLIST WHERE EXISTS (SELECT SYS_TODOHIS.LISTID 
        FROM SYS_TODOHIS 
        WHERE SYS_TODOHIS.LISTID = SYS_TODOLIST.LISTID AND SYS_TODOHIS.USER_ID ='{0}') AND SYS_TODOLIST.STATUS <> 'F' AND ((SYS_TODOLIST.SENDTO_KIND='1'
            AND SYS_TODOLIST.SENDTO_ID NOT IN (SELECT GROUPID FROM GROUPS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='{0}')  
            AND ISROLE='Y')) OR (SYS_TODOLIST.SENDTO_KIND='2' AND SYS_TODOLIST.SENDTO_ID<>'{0}')) ORDER BY SYS_TODOLIST.UPDATE_DATE,
            SYS_TODOLIST.UPDATE_TIME";

    // 0:user    1:connectMark
    private static string SQL_TODOHIS2 = @"
        SELECT SYS_TODOLIST.LISTID, SYS_TODOLIST.FLOW_ID, SYS_TODOLIST.FLOW_DESC, SYS_TODOLIST.APPLICANT, SYS_TODOLIST.S_USER_ID, SYS_TODOLIST.S_STEP_ID, 
            SYS_TODOLIST.S_STEP_DESC, SYS_TODOLIST.D_STEP_ID, SYS_TODOLIST.D_STEP_DESC, SYS_TODOLIST.EXP_TIME, SYS_TODOLIST.URGENT_TIME, 
            SYS_TODOLIST.TIME_UNIT, SYS_TODOLIST.USERNAME, SYS_TODOLIST.FORM_NAME, SYS_TODOLIST.NAVIGATOR_MODE, SYS_TODOLIST.FLNAVIGATOR_MODE, 
            SYS_TODOLIST.PARAMETERS, SYS_TODOLIST.SENDTO_KIND, SYS_TODOLIST.SENDTO_ID, SYS_TODOLIST.SENDTO_NAME, SYS_TODOLIST.FLOWIMPORTANT, 
            SYS_TODOLIST.FLOWURGENT, SYS_TODOLIST.STATUS, SYS_TODOLIST.FORM_TABLE, SYS_TODOLIST.FORM_KEYS, SYS_TODOLIST.FORM_PRESENTATION, 
            SYS_TODOLIST.FORM_PRESENT_CT, SYS_TODOLIST.REMARK, SYS_TODOLIST.PROVIDER_NAME, SYS_TODOLIST.VERSION, SYS_TODOLIST.EMAIL_ADD, 
            SYS_TODOLIST.EMAIL_STATUS, SYS_TODOLIST.VDSNAME, SYS_TODOLIST.SENDBACKSTEP, SYS_TODOLIST.LEVEL_NO, SYS_TODOLIST.WEBFORM_NAME, 
            (SYS_TODOLIST.UPDATE_DATE {1} ' ' {1} SYS_TODOLIST.UPDATE_TIME) AS UPDATE_WHOLE_TIME, SYS_TODOLIST.FLOWPATH, SYS_TODOLIST.PLUSAPPROVE, 
            SYS_TODOLIST.PLUSROLES, SYS_TODOLIST.MULTISTEPRETURN FROM SYS_TODOLIST WHERE EXISTS (SELECT SYS_TODOHIS.LISTID 
        FROM SYS_TODOHIS 
        WHERE SYS_TODOHIS.LISTID = SYS_TODOLIST.LISTID AND SYS_TODOHIS.USER_ID ='{0}') AND SYS_TODOLIST.STATUS <> 'F' AND ((SYS_TODOLIST.SENDTO_KIND='1' 
            AND SYS_TODOLIST.SENDTO_ID NOT IN (SELECT GROUPID FROM GROUPS WHERE GROUPID IN (SELECT GROUPID FROM USERGROUPS WHERE USERID='{0}')  
            AND ISROLE='Y')) OR (SYS_TODOLIST.SENDTO_KIND='2' AND SYS_TODOLIST.SENDTO_ID<>'{0}')) ORDER BY SYS_TODOLIST.UPDATE_DATE,
            SYS_TODOLIST.UPDATE_TIME";

    //0:user
    //    private static string SQL_RUNOVER1 = @"
    //        SELECT LISTID, FLOW_DESC,D_STEP_ID, FORM_NAME,WEBFORM_NAME,FORM_PRESENTATION,FORM_PRESENT_CT,REMARK, CONCAT(UPDATE_DATE,' ',UPDATE_TIME) AS UPDATE_WHOLE_TIME,
    //            ATTACHMENTS, STATUS, (SELECT CONCAT(USER_ID,' ',USERNAME) FROM SYS_TODOHIS A1 WHERE A1.LISTID = SYS_TODOHIS.LISTID ORDER BY UPDATE_DATE, UPDATE_TIME  LIMIT 0, 1) AS APPLICATE
    //            FROM SYS_TODOHIS WHERE LISTID IN (SELECT DISTINCT LISTID FROM SYS_TODOHIS WHERE USER_ID='{0}') AND (STATUS='Z' OR STATUS='X') ORDER BY UPDATE_WHOLE_TIME DESC";

    //    //0:user 1:connectMark
    //    private static string SQL_RUNOVER2 = @"
    //        SELECT LISTID, FLOW_DESC,D_STEP_ID, FORM_NAME,WEBFORM_NAME,FORM_PRESENTATION,FORM_PRESENT_CT,REMARK, (UPDATE_DATE {1} ' ' {1} UPDATE_TIME) AS UPDATE_WHOLE_TIME,
    //            ATTACHMENTS, STATUS, (SELECT {2} USER_ID {1} '(' {1} USERNAME {1} ')' FROM SYS_TODOHIS A1 WHERE A1.LISTID = SYS_TODOHIS.LISTID {3} ORDER BY UPDATE_DATE, UPDATE_TIME) AS APPLICATE
    //            FROM SYS_TODOHIS WHERE LISTID IN (SELECT DISTINCT LISTID FROM SYS_TODOHIS WHERE USER_ID='{0}') AND (STATUS='Z' OR STATUS='X') ORDER BY UPDATE_WHOLE_TIME DESC";

    private static string SQL_RUNOVER1 = @"
        SELECT LISTID, FLOW_DESC,D_STEP_ID, FORM_NAME,WEBFORM_NAME,FORM_PRESENTATION,FORM_PRESENT_CT,REMARK, CONCAT(UPDATE_DATE,' ',UPDATE_TIME) AS UPDATE_WHOLE_TIME,
            ATTACHMENTS, STATUS FROM SYS_TODOHIS WHERE LISTID IN (SELECT DISTINCT LISTID FROM SYS_TODOHIS WHERE USER_ID='{0}') AND STATUS='Z' ORDER BY UPDATE_WHOLE_TIME DESC";

    //0:user 1:connectMark
    private static string SQL_RUNOVER2 = @"
        SELECT LISTID, FLOW_DESC,D_STEP_ID, FORM_NAME,WEBFORM_NAME,FORM_PRESENTATION,FORM_PRESENT_CT,REMARK, (UPDATE_DATE {1} ' ' {1} UPDATE_TIME) AS UPDATE_WHOLE_TIME,
            ATTACHMENTS, STATUS FROM SYS_TODOHIS WHERE LISTID IN (SELECT DISTINCT LISTID FROM SYS_TODOHIS WHERE USER_ID='{0}') AND STATUS='Z' ORDER BY UPDATE_WHOLE_TIME DESC";

    private static string SQL_APPLICATE = @"
        SELECT USER_ID, USERNAME FROM SYS_TODOHIS WHERE LISTID = '{0}' ORDER BY UPDATE_DATE, UPDATE_TIME";


    // 0:listId
    //    private static string SQL_COMMENT = @"
    //        SELECT SYS_TODOHIS.FLOW_DESC,SYS_TODOHIS.S_ROLE_ID,SYS_TODOHIS.S_STEP_ID,SYS_TODOHIS.USER_ID,SYS_TODOHIS.USERNAME,SYS_TODOHIS.STATUS,
    //            SYS_TODOHIS.UPDATE_DATE,SYS_TODOHIS.UPDATE_TIME,SYS_TODOHIS.REMARK,SYS_TODOHIS.ATTACHMENTS,GROUPS.GROUPNAME FROM SYS_TODOHIS 
    //            LEFT JOIN GROUPS ON SYS_TODOHIS.S_ROLE_ID = GROUPS.GROUPID WHERE (SYS_TODOHIS.LISTID = '{0}') ORDER BY SYS_TODOHIS.UPDATE_DATE,
    //            SYS_TODOHIS.UPDATE_TIME";
    private static string SQL_COMMENT = @"
        SELECT SYS_TODOHIS.FLOW_DESC,SYS_TODOHIS.S_ROLE_ID,SYS_TODOHIS.S_STEP_ID,SYS_TODOHIS.USER_ID,SYS_TODOHIS.USERNAME,SYS_TODOHIS.STATUS,
            SYS_TODOHIS.UPDATE_DATE,SYS_TODOHIS.UPDATE_TIME,SYS_TODOHIS.REMARK,SYS_TODOHIS.ATTACHMENTS,SYS_TODOHIS.D_USERNAME,GROUPS.GROUPNAME FROM SYS_TODOHIS 
            LEFT JOIN GROUPS ON SYS_TODOHIS.S_ROLE_ID = GROUPS.GROUPID WHERE (SYS_TODOHIS.LISTID = '{0}') ORDER BY SYS_TODOHIS.UPDATE_DATE,
            SYS_TODOHIS.UPDATE_TIME";

    // 0:roles
    private static string SQL_DELAY = @"
        select LISTID, FLOW_ID, FLOW_DESC, APPLICANT, S_USER_ID, S_STEP_ID, S_STEP_DESC, D_STEP_ID, D_STEP_DESC, EXP_TIME, URGENT_TIME, TIME_UNIT, 
            USERNAME, FORM_NAME, NAVIGATOR_MODE, FLNAVIGATOR_MODE, PARAMETERS, SENDTO_KIND, SENDTO_ID, FLOWIMPORTANT, FLOWURGENT, STATUS, FORM_TABLE, 
            FORM_KEYS, FORM_PRESENTATION, FORM_PRESENT_CT, REMARK, PROVIDER_NAME, VERSION, EMAIL_ADD, EMAIL_STATUS, VDSNAME, SENDBACKSTEP, 
            LEVEL_NO, WEBFORM_NAME, UPDATE_DATE, UPDATE_TIME, FLOWPATH, PLUSAPPROVE, PLUSROLES
        from SYS_TODOLIST 
        where SENDTO_ID in ('{0}') and SENDTO_KIND='1' ORDER BY UPDATE_DATE";


    // 0:user    1: dtString      2:connectMark
    private static string SQL_NOTIFY1 = @"
        SELECT SYS_TODOLIST.LISTID,SYS_TODOLIST.FLOW_ID, SYS_TODOLIST.FLOW_DESC, SYS_TODOLIST.APPLICANT, SYS_TODOLIST.S_USER_ID, SYS_TODOLIST.S_STEP_ID, 
            SYS_TODOLIST.S_STEP_DESC, SYS_TODOLIST.D_STEP_ID, SYS_TODOLIST.D_STEP_DESC, SYS_TODOLIST.EXP_TIME, SYS_TODOLIST.URGENT_TIME, 
            SYS_TODOLIST.TIME_UNIT, SYS_TODOLIST.USERNAME, SYS_TODOLIST.FORM_NAME, SYS_TODOLIST.NAVIGATOR_MODE, SYS_TODOLIST.FLNAVIGATOR_MODE, 
            SYS_TODOLIST.PARAMETERS, SYS_TODOLIST.SENDTO_KIND, SYS_TODOLIST.SENDTO_ID, SYS_TODOLIST.SENDTO_NAME, SYS_TODOLIST.FLOWIMPORTANT,
            SYS_TODOLIST.FLOWURGENT, SYS_TODOLIST.STATUS, SYS_TODOLIST.FORM_TABLE, SYS_TODOLIST.FORM_KEYS, SYS_TODOLIST.FORM_PRESENTATION, 
            SYS_TODOLIST.FORM_PRESENT_CT, SYS_TODOLIST.REMARK, SYS_TODOLIST.PROVIDER_NAME, SYS_TODOLIST.VERSION, SYS_TODOLIST.EMAIL_ADD, 
            SYS_TODOLIST.EMAIL_STATUS, SYS_TODOLIST.VDSNAME, SYS_TODOLIST.SENDBACKSTEP, SYS_TODOLIST.LEVEL_NO, SYS_TODOLIST.WEBFORM_NAME, 
            (SYS_TODOLIST.UPDATE_DATE {2} ' ' {2} SYS_TODOLIST.UPDATE_TIME) AS UPDATE_WHOLE_TIME, SYS_TODOLIST.FLOWPATH, SYS_TODOLIST.PLUSAPPROVE, 
            SYS_TODOLIST.PLUSROLES, SYS_TODOLIST.MULTISTEPRETURN, SYS_TODOLIST.ATTACHMENTS FROM 
        SYS_TODOLIST 
        WHERE (SYS_TODOLIST.SENDTO_KIND = '1' AND SYS_TODOLIST.STATUS='F' AND ((SYS_TODOLIST.SENDTO_ID IN (Select GROUPID from GROUPS WHERE GROUPID 
            IN (Select GROUPID from USERGROUPS Where USERID='{0}')  AND ISROLE='Y'))  OR (SYS_TODOLIST.SENDTO_ID IN (Select ROLE_ID AS GROUPID 
            From SYS_ROLES_AGENT where SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC 
            IS NULL OR SYS_TODOLIST.FLOW_DESC=SYS_ROLES_AGENT.FLOW_DESC) AND AGENT='{0}' AND START_DATE+START_TIME<='{1}' AND END_DATE+END_TIME>='{1}') 
            AND (SYS_TODOLIST.S_USER_ID<>'{0}'))) OR (SYS_TODOLIST.SENDTO_KIND = '2' AND SYS_TODOLIST.SENDTO_ID='{0}' AND SYS_TODOLIST.STATUS<>'NF' 
            AND SYS_TODOLIST.STATUS<>'NR')) ORDER BY SYS_TODOLIST.UPDATE_DATE,SYS_TODOLIST.UPDATE_TIME";

    // 0:user    1: dtString      2:connectMark
    private static string SQL_NOTIFY2 = @"
        SELECT SYS_TODOLIST.LISTID,SYS_TODOLIST.FLOW_ID, SYS_TODOLIST.FLOW_DESC, SYS_TODOLIST.APPLICANT, SYS_TODOLIST.S_USER_ID, SYS_TODOLIST.S_STEP_ID,
            SYS_TODOLIST.S_STEP_DESC, SYS_TODOLIST.D_STEP_ID, SYS_TODOLIST.D_STEP_DESC, SYS_TODOLIST.EXP_TIME, SYS_TODOLIST.URGENT_TIME, 
            SYS_TODOLIST.TIME_UNIT, SYS_TODOLIST.USERNAME, SYS_TODOLIST.FORM_NAME, SYS_TODOLIST.NAVIGATOR_MODE, SYS_TODOLIST.FLNAVIGATOR_MODE, 
            SYS_TODOLIST.PARAMETERS, SYS_TODOLIST.SENDTO_KIND, SYS_TODOLIST.SENDTO_ID, SYS_TODOLIST.SENDTO_NAME, SYS_TODOLIST.FLOWIMPORTANT, 
            SYS_TODOLIST.FLOWURGENT, SYS_TODOLIST.STATUS, SYS_TODOLIST.FORM_TABLE, SYS_TODOLIST.FORM_KEYS, SYS_TODOLIST.FORM_PRESENTATION, 
            SYS_TODOLIST.FORM_PRESENT_CT, SYS_TODOLIST.REMARK, SYS_TODOLIST.PROVIDER_NAME, SYS_TODOLIST.VERSION, SYS_TODOLIST.EMAIL_ADD, 
            SYS_TODOLIST.EMAIL_STATUS, SYS_TODOLIST.VDSNAME, SYS_TODOLIST.SENDBACKSTEP, SYS_TODOLIST.LEVEL_NO, SYS_TODOLIST.WEBFORM_NAME, 
            (SYS_TODOLIST.UPDATE_DATE {2} ' ' {2} SYS_TODOLIST.UPDATE_TIME) AS UPDATE_WHOLE_TIME, SYS_TODOLIST.FLOWPATH, SYS_TODOLIST.PLUSAPPROVE,
            SYS_TODOLIST.PLUSROLES, SYS_TODOLIST.MULTISTEPRETURN, SYS_TODOLIST.ATTACHMENTS 
        FROM SYS_TODOLIST 
        WHERE (SYS_TODOLIST.SENDTO_KIND = '1' AND SYS_TODOLIST.STATUS='F' AND (SYS_TODOLIST.SENDTO_ID IN ((Select GROUPID from GROUPS WHERE 
            GROUPID IN (Select GROUPID from USERGROUPS Where USERID='{0}')  AND ISROLE='Y')  union (Select ROLE_ID AS GROUPID From SYS_ROLES_AGENT 
            where SYS_TODOLIST.SENDTO_ID=SYS_ROLES_AGENT.ROLE_ID AND (SYS_ROLES_AGENT.FLOW_DESC='*' OR SYS_ROLES_AGENT.FLOW_DESC 
            IS NULL OR SYS_TODOLIST.FLOW_DESC=SYS_ROLES_AGENT.FLOW_DESC) AND AGENT='{0}' AND START_DATE+START_TIME<='{1}' AND END_DATE+END_TIME>='{1}')) 
            AND (SYS_TODOLIST.S_USER_ID<>'{0}')) OR (SYS_TODOLIST.SENDTO_KIND = '2' AND SYS_TODOLIST.SENDTO_ID='{0}' AND SYS_TODOLIST.STATUS<>'NF'
            AND SYS_TODOLIST.STATUS<>'NR')) ORDER BY SYS_TODOLIST.UPDATE_DATE,SYS_TODOLIST.UPDATE_TIME";

    private static string SQL_GETUSERS = @"
        SELECT USERID FROM SYS_TODOLIST, GROUPS, USERGROUPS WHERE GROUPS.GROUPID = SYS_TODOLIST.SENDTO_ID AND USERGROUPS.GROUPID = GROUPS.GROUPID
        AND LISTID = '{0}' AND GROUPS.ISROLE = 'Y' AND SENDTO_KIND = '1'
        UNION
        SELECT SENDTO_ID AS USERID FROM SYS_TODOLIST WHERE LISTID = '{0}' AND SENDTO_KIND = '2'
        UNION
        SELECT USER_ID AS USERID FROM SYS_TODOHIS WHERE LISTID = '{0}' AND USER_ID <> 'SYS'";
    #endregion

    public FLService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 

        string strPath = EEPRegistry.WebClient;

        CliUtils.LoadLoginServiceConfig(strPath + @"\EEPWebClient.exe.config");
        string message = "";
        if (CliUtils.Register(ref message) == false)
        {
            throw new Exception(message);
        }
    }

    //#region --------------------------------DotNet------------------------------------

    //private void InitClientInfo()
    //{
    //    object obj = Session["ClientInfo"];
    //    if (obj != null)
    //    {
    //        object[] clientInfo = (object[])obj;

    //        SYS_LANGUAGE.ENG = (SYS_LANGUAGE)clientInfo[0];
    //        CliUtils.fLoginUser = clientInfo[1].ToString();
    //        CliUtils.fLoginDB = clientInfo[2].ToString();
    //        CliUtils.fCurrentProject = clientInfo[6].ToString();
    //        CliUtils.fClientSystem = "WebService";
    //    }
    //}

    //// fClientLang, fLoginUser, fLoginDB, fSiteCode, fComputerName, fComputerIp, fCurrentProject, fClientSystem, fGroupID, UserPara1, UserPara2
    //[WebMethod(EnableSession = true)]
    //public object[] Login(string user, string password, string loginDB, string projectName, int language)
    //{
    //    SYS_LANGUAGE.ENG = (SYS_LANGUAGE)language;
    //    CliUtils.fLoginUser = user;
    //    CliUtils.fLoginDB = loginDB;
    //    CliUtils.fCurrentProject = projectName;
    //    CliUtils.fClientSystem = "WebService";

    //    string param = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ":0";
    //    object[] ret = CallMethod(clientInfo, "GLModule", "CheckUser", new object[] { (object)param });

    //    if (ret[0].ToString() == "0")
    //    {
    //        Session["IsLogin"] = true;
    //        Session["UserId"] = user;
    //        Session["ClientInfo"] = CliUtils.GetBaseClientInfo();

    //        return ret;
    //    }
    //    else
    //    {
    //        return new object[] { 1, "Login fail." };
    //    }
    //}

    //[WebMethod(EnableSession = true)]
    //public void Logout(string user)
    //{
    //    CallMethod(clientInfo, "GLModule", "LogOut", new object[] { user });
    //}

    //// object[]{instanceId, new object[]{ currentFLActivity, nextFLActivity, 重要,紧急,意见,发送者角色,provider, email_url, orgkid, attachments}}
    //// Notify,PlusApporve的[8]为role，其他的情况都为orgkid
    //[WebMethod(EnableSession = true)]
    //public object[] Submit(string xomlFile, int important, int urgent, string remark, string roleId, string provider, string orgKid,
    //    string keys, string keyValues)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        return new object[] { 1, "You must login first." };
    //    }

    //    object[] objs = new object[]
    //    {
    //        null,
    //        new object[]{
    //            xomlFile,
    //            string.Empty,
    //            important,
    //            urgent,
    //            remark,
    //            roleId,
    //            provider,
    //            0,
    //            orgKid,
    //            string.Empty // attachments
    //        },
    //        new object[]{
    //            keys,
    //            keyValues
    //        }
    //    };

    //    InitClientInfo();
    //    return CallFLMethod(clientInfo, "Submit", objs);
    //}

    //[WebMethod(EnableSession = true)]
    //public object[] Approve(string listId, string flowPath, int important, int urgent, string remark, string roleId,  string orgKid)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = CliUtils.fLoginUser;
    //    DataSet todoListSet = GetCurrentToDoList(dbType, userId, listId, flowPath);
    //    if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
    //    {
    //        return new object[]{1, "ToDoList"};
    //    }
    //    DataTable todoListTable = todoListSet.Tables[0];
    //    string[] ss= flowPath.Split(';');
    //    string currentFLActivity =  ss[0] ;
    //    string nextFLActivity = ss[1];
    //    string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
    //    string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
    //    string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

    //    object[] objs = new object[]
    //    {
    //        new Guid(listId),
    //        new object[]{
    //            currentFLActivity,
    //            nextFLActivity,
    //            important,
    //            urgent,
    //            remark,
    //            roleId,
    //            provider,
    //            0,
    //            orgKid,
    //            string.Empty //attachments
    //        },
    //        new object[]{
    //            keys,
    //            keyValues
    //        }
    //    };

    //    InitClientInfo();
    //    return CallFLMethod(clientInfo, "Approve", objs);
    //}

    //[WebMethod(EnableSession = true)]
    //public object[] Pause(string xomlFile, string provider, string keys, string keyValues)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        return new object[] { 1, "You must login first." };
    //    }

    //    object[] objs = new object[]
    //    {
    //        null,
    //        new object[]{
    //            xomlFile,
    //            string.Empty,
    //            0,
    //            0,
    //            string.Empty,
    //            string.Empty,
    //            provider,
    //            0,
    //            string.Empty,
    //            string.Empty
    //        },
    //        new object[]{
    //            keys,
    //            keyValues
    //        }
    //    };

    //    InitClientInfo();
    //    return CallFLMethod(clientInfo, "Pause", objs);
    //}

    //[WebMethod(EnableSession = true)]
    //public object[] Reject(string listId, string flowPath, int sendNotifyToAllRoles)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = CliUtils.fLoginUser;
    //    DataSet todoListSet = GetCurrentToDoList(dbType, userId, listId, flowPath);
    //    if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
    //    {
    //        return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
    //    }
    //    DataTable todoListTable = todoListSet.Tables[0];
    //    string[] ss = flowPath.Split(';');
    //    string currentFLActivity = ss[0];
    //    string nextFLActivity = ss[1];
    //    string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
    //    string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
    //    string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

    //    object[] objs = new object[]
    //        {
    //            new Guid(listId),
    //            new object[]{currentFLActivity, nextFLActivity, sendNotifyToAllRoles, provider },
    //            new object[]{
    //                keys,
    //                keyValues
    //            }
    //        };

    //    InitClientInfo();
    //    return CallFLMethod(clientInfo, "Reject", objs);
    //}

    //[WebMethod(EnableSession = true)]
    //public object[] Return(string listId, string flowPath, int important, int urgent, string remark, string roleId, string orgKid)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = CliUtils.fLoginUser;
    //    DataSet todoListSet = GetCurrentToDoList(dbType, userId, listId, flowPath);
    //    if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
    //    {
    //        return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
    //    }
    //    DataTable todoListTable = todoListSet.Tables[0];
    //    string[] ss = flowPath.Split(';');
    //    string currentFLActivity = ss[0];
    //    string nextFLActivity = ss[1];
    //    string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
    //    string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
    //    string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

    //    object[] objs = new object[]
    //    {
    //        new Guid(listId),
    //        new object[]{
    //            currentFLActivity,
    //            nextFLActivity,
    //            important,
    //            urgent,
    //            remark,
    //            roleId,
    //            provider,
    //            0,
    //            orgKid,
    //            string.Empty //attachments
    //        },
    //        new object[]{
    //            keys,
    //            keyValues
    //        }
    //    };

    //    InitClientInfo();
    //    return CallFLMethod(clientInfo, "Return", objs);
    //}

    //[WebMethod(EnableSession = true)]
    //public object[] Notify(string listId, string flowPath, string remark, string sendToIds)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = CliUtils.fLoginUser;
    //    DataSet todoListSet = GetCurrentToDoList(dbType, userId, listId, flowPath);
    //    if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
    //    {
    //        return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
    //    }
    //    DataTable todoListTable = todoListSet.Tables[0];
    //    string[] ss = flowPath.Split(';');
    //    string currentFLActivity = ss[0];
    //    string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
    //    string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
    //    string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

    //    object[] objs = new object[]
    //    {
    //        new Guid(listId),
    //        new object[]{
    //            currentFLActivity,
    //            currentFLActivity,
    //            0,
    //            0,
    //            remark,
    //            string.Empty,
    //            provider,
    //            0,
    //            sendToIds,
    //            string.Empty
    //        },
    //        new object[]{
    //            keys,
    //            keyValues
    //        }
    //    };

    //    InitClientInfo();
    //    return CallFLMethod(clientInfo, "Notify", objs);
    //}

    //[WebMethod(EnableSession = true)]
    //public object[] Plus(string listId, string flowPath, string remark, string roleId, string sendToIds)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = CliUtils.fLoginUser;
    //    DataSet todoListSet = GetCurrentToDoList(dbType, userId, listId, flowPath);
    //    if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
    //    {
    //        return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
    //    }
    //    DataTable todoListTable = todoListSet.Tables[0];
    //    string[] ss = flowPath.Split(';');
    //    string nextFLActivity = ss[1];
    //    string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
    //    string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
    //    string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

    //    object[] objs = new object[]
    //    {
    //        new Guid(listId),
    //        new object[]{
    //            nextFLActivity,
    //            nextFLActivity,
    //            0,
    //            0,
    //            remark,
    //            roleId,
    //            provider,
    //            0,
    //            sendToIds,
    //            string.Empty
    //        },
    //        new object[]{
    //            keys,
    //            keyValues
    //        }
    //    };

    //    InitClientInfo();
    //    return CallFLMethod(clientInfo, "PlusApprove", objs);
    //}

    //[WebMethod(EnableSession = true)]
    //public object[] PlusReturn(string listId, string flowPath, string remark, string roleId)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = CliUtils.fLoginUser;
    //    DataSet todoListSet = GetCurrentToDoList(dbType, userId, listId, flowPath);
    //    if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
    //    {
    //        return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
    //    }
    //    DataTable todoListTable = todoListSet.Tables[0];
    //    string[] ss = flowPath.Split(';');
    //    string nextFLActivity = ss[1];
    //    string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
    //    string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
    //    string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

    //    object[] objs = new object[]
    //    {
    //        new Guid(listId),
    //        new object[]{
    //            nextFLActivity,
    //            nextFLActivity,
    //            0,
    //            0,
    //            remark,
    //            roleId,
    //            provider,
    //            0,
    //            string.Empty,
    //            string.Empty
    //        },
    //        new object[]{
    //            keys,
    //            keyValues
    //        }
    //    };

    //    InitClientInfo();
    //    return CallFLMethod(clientInfo, "PlusReturn", objs);
    //}

    //[WebMethod(EnableSession = true)]
    //public DataSet GetToDoList()
    //{
    //    if (Session["IsLogin"] == null || Session["UserId"] == null)
    //    {
    //        // return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = Session["UserId"].ToString();
    //    return GetToDoList2(userId);
    //}

    //[WebMethod(EnableSession = true)]
    //public DataSet GetToDoList2(string userId)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        // return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string connectMark = "+";
    //    switch (dbType)
    //    {
    //        case 1: connectMark = "+"; break;
    //        case 2: connectMark = "+"; break;
    //        case 3: connectMark = "||"; break;
    //        case 4: connectMark = "+"; break;
    //    }

    //    string sql = string.Empty;
    //    if (dbType == 5)
    //    {
    //        sql = string.Format(SQL_TODOLIST1, userId, GetDateTimeString(DateTime.Now));
    //    }
    //    else
    //    {
    //        sql = string.Format(SQL_TODOLIST2, userId, GetDateTimeString(DateTime.Now), connectMark);
    //    }

    //    object[] excuteParam = new object[1] { sql };
    //    InitClientInfo();
    //    object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

    //    if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
    //    {
    //        return objs[1] as DataSet;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //[WebMethod(EnableSession = true)]
    //public DataSet GetToDoHis()
    //{
    //    if (Session["IsLogin"] == null || Session["UserId"] == null)
    //    {
    //        // return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = Session["UserId"].ToString();
    //    return GetToDoHis2(userId);
    //}

    //[WebMethod(EnableSession = true)]
    //public DataSet GetToDoHis2(string userId)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        // return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string connectMark = "+";
    //    switch (dbType)
    //    {
    //        case 1: connectMark = "+"; break;
    //        case 2: connectMark = "+"; break;
    //        case 3: connectMark = "||"; break;
    //        case 4: connectMark = "+"; break;
    //    }

    //    string sql = string.Empty;
    //    if (dbType == 5)
    //    {
    //        sql = string.Format(SQL_TODOHIS1, userId);
    //    }
    //    else
    //    {
    //        sql = string.Format(SQL_TODOHIS2, userId, connectMark);
    //    }

    //    object[] excuteParam = new object[1] { sql };
    //    InitClientInfo();
    //    object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

    //    if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
    //    {
    //        return objs[1] as DataSet;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //[WebMethod(EnableSession = true)]
    //public DataSet GetComment(string listId)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        // return new object[] { 1, "You must login first." };
    //    }

    //    string sql = string.Format(SQL_COMMENT, listId);

    //    object[] excuteParam = new object[1] { sql };
    //    InitClientInfo();
    //    object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

    //    if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
    //    {
    //        return objs[1] as DataSet;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //[WebMethod(EnableSession = true)]
    //public DataSet GetNotify()
    //{
    //    if (Session["IsLogin"] == null || Session["UserId"] == null)
    //    {
    //        // return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string userId = Session["UserId"].ToString();
    //    return GetNotify2(userId);
    //}

    //[WebMethod(EnableSession = true)]
    //public DataSet GetNotify2(string userId)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        // return new object[] { 1, "You must login first." };
    //    }

    //    int dbType = (int)CliUtils.GetDataBaseType(CliUtils.fLoginDB);
    //    string connectMark = "+";
    //    switch (dbType)
    //    {
    //        case 1: connectMark = "+"; break;
    //        case 2: connectMark = "+"; break;
    //        case 3: connectMark = "||"; break;
    //        case 4: connectMark = "+"; break;
    //    }

    //    string sql = string.Empty;
    //    if (dbType == 5)
    //    {
    //        sql = string.Format(SQL_NOTIFY1, userId, GetDateTimeString(DateTime.Now), connectMark);
    //    }
    //    else
    //    {
    //        sql = string.Format(SQL_NOTIFY2, userId, GetDateTimeString(DateTime.Now), connectMark);
    //    }

    //    object[] excuteParam = new object[1] { sql };
    //    InitClientInfo();
    //    object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

    //    if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
    //    {
    //        return objs[1] as DataSet;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //[WebMethod(EnableSession = true)]
    //public DataSet GetDelay(string roleId)
    //{
    //    if (Session["IsLogin"] == null)
    //    {
    //        // return new object[] { 1, "You must login first." };
    //    }

    //    string sql = string.Format(SQL_DELAY, roleId);

    //    object[] excuteParam = new object[1] { sql };
    //    InitClientInfo();
    //    object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

    //    if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
    //    {
    //        return objs[1] as DataSet;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //#endregion

    #region -----------------------------ALL Platform---------------------------------

    //建立基本資訊
    private object[] CreateClientInfo(string user, string password, string loginDB, string projectName, SYS_LANGUAGE language)
    {
        return new object[] { language, user, loginDB
                , "", "", ""
                , projectName, "", ""
                , "", "", ""
                , "", "", "", "0", password };

    }

    //抓資料庫型態
    private ClientType GetDataBaseType(object[] clientInfo, string securityKey)
    {
        string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
        //string[] ss = securityKey.Split("-".ToCharArray());
        string dataBase = ss[1];
        object[] myRet = CallMethod(clientInfo, "GLModule", "GetDataBaseType", new object[] { dataBase });
        if (myRet != null && (int)myRet[0] == 0)
        {
            int intdb = int.Parse(myRet[1].ToString());
            return (ClientType)intdb;
        }
        else
        {
            return ClientType.ctMsSql;
        }
    }

    private object[] CallMethod(object[] clientInfo, string moduleName, string methodName, object[] objParams)
    {
        return CliUtils.RemoteObject.CallMethod(new object[] { clientInfo }, moduleName, methodName, objParams);
    }

    private object[] CallFLMethod(object[] clientInfo, string methodName, object[] objParams)
    {
        return CliUtils.RemoteObject.CallFLMethod(new object[] { clientInfo, -1, -1, string.Empty }, methodName, objParams);
    }

    //組織基本資訊
    private object[] InitClientInfoForAllPlatform(string securityKey)
    {
        string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
        //string[] ss = securityKey.Split("-".ToCharArray());

        string userId = ss[0];
        string dataBase = ss[1];
        string solution = ss[2];
        int language = int.Parse(ss[3]);

        var clientInfo = CreateClientInfo(userId, null, dataBase, solution, (SYS_LANGUAGE)language);
        if (Transactions.ContainsKey(userId))
        {
            clientInfo[10] = Transactions[userId];
        }
        return clientInfo;
    }

    //登錄EEPNetServer
    [WebMethod]
    public string LoginForAllPlatform(string user, string password, string loginDB, string projectName, int language)
    {
        //string param = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ":0";
        //string param = CliUtils.fLoginUser + ':' + CliUtils.fLoginPassword + ':' + CliUtils.fLoginDB + ":0:SharePoint";//matida 2010/1/29 modify

        string param = user + ':' + password + ':' + loginDB + ":0:SharePoint";
        object[] clientInfo = CreateClientInfo(user, password, loginDB, projectName, (SYS_LANGUAGE)language);
        object[] ret = CallMethod(clientInfo, "GLModule", "CheckUser", new object[] { (object)param });

        if (ret[0].ToString() == "0")
        {
            LoginResult result = (LoginResult)ret[1];
            if (result == LoginResult.Success)
            {
                return PublicKey.GetPublicKey2(user, loginDB, projectName, language);
            }
        }
        return string.Empty;
    }

    //登出EEPNetServer
    [WebMethod]
    public void LogoutForAllPlatform(string user)
    {
        object[] clientInfo = CreateClientInfo(user, "", "", "", SYS_LANGUAGE.ENG);
        CallMethod(clientInfo, "GLModule", "LogOut", new object[] { user });
    }

    //呈送
    [WebMethod]
    public object[] SubmitForAllPlatform(string securityKey, string xomlFile, int important, int urgent, string remark, string roleId, string provider, string orgKid, string attachments,
        string keys, string keyValues, string url)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] obj = CallMethod(clientInfo, "GLModule", "GetServerPath", null);
            xomlFile = (string)obj[1] + xomlFile;
            object[] objs = new object[]
            {
                null,
                new object[]{
                    xomlFile,
                    string.Empty,
                    important,
                    urgent,
                    remark,
                    roleId,
                    provider,
                    url,
                    orgKid,
                    attachments // attachments
                },
                new object[]{
                    keys,
                    keyValues
                }
            };

            object[] objParams = CallFLMethod(clientInfo, "Submit", objs);
            if (Convert.ToInt16(objParams[0]) == 0)
            {
                if (objParams[1].ToString() == "60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E")
                {
                    return new object[] { 0, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLClientControls", "FLNavigator", "RunOver", true) };
                }
                else if (objParams[1].ToString() == "512F4277-0D41-441c-BF16-D96B04580C2E")
                {
                    return new object[] { 0, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLClientControls", "FLNavigator", "HasRejected", true) };
                }
            }
            return objParams;
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //審核
    [WebMethod]
    public object[] ApproveForAllPlatform(string securityKey, string listId, string flowPath, int important, int urgent, string remark, string roleId, string orgKid, string attachments
        , string url)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss0 = securityKey.Split('-');
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);
            string userId = ss0[0];
            DataSet todoListSet = GetCurrentDoListForAllAllPlatform(securityKey, dbType, userId, listId, flowPath);
            if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
            {
                return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
            }
            DataTable todoListTable = todoListSet.Tables[0];
            string[] ss = flowPath.Split(';');
            string currentFLActivity = ss[0];
            string nextFLActivity = ss[1];
            string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
            string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
            string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    currentFLActivity,
                    nextFLActivity,
                    important,
                    urgent,
                    remark,
                    roleId,
                    provider,
                    url,
                    orgKid,
                    attachments //attachments
                },
                new object[]{
                    keys,
                    keyValues
                }
            };


            object[] objParams = CallFLMethod(clientInfo, "Approve", objs);
            if (Convert.ToInt16(objParams[0]) == 0)
            {
                if (objParams[1].ToString() == "60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E")
                {
                    return new object[] { 0, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLClientControls", "FLNavigator", "RunOver", true) };
                }
                else if (objParams[1].ToString() == "512F4277-0D41-441c-BF16-D96B04580C2E")
                {
                    return new object[] { 0, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLClientControls", "FLNavigator", "HasRejected", true) };
                }
                else if (objParams[1].ToString() == "")
                {
                    string wait = SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLTools", "GloFix", "WaitMessage", true);

                    string sql = "select SENDTO_KIND, SENDTO_ID from SYS_TODOLIST where LISTID='" + listId + "'";
                    DataTable dtOthers = null;
                    object[] ret1 = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", new object[] { sql });
                    if (ret1 != null && (int)ret1[0] == 0)
                    {
                        dtOthers = ((DataSet)ret1[1]).Tables[0];
                    }
                    string sendToIds = "";
                    foreach (DataRow row in dtOthers.Rows)
                    {
                        string sendtokind = row["SENDTO_KIND"].ToString();
                        string sendtoid = row["SENDTO_ID"].ToString();
                        if (sendtokind == "1")
                        {
                            sendToIds += sendtoid + ";";
                        }
                        else if (sendtokind == "2")
                        {
                            sendToIds += sendtoid + ":UserId;";
                        }
                    }
                    if (sendToIds != "")
                    {
                        return new object[] { 0, GloFix.ShowParallelMessage(sendToIds) };
                    }
                }
            }
            return objParams;
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //暫停
    [WebMethod]
    public object[] PauseForAllPlatform(string securityKey, string xomlFile, string provider, string keys, string keyValues)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] obj = CallMethod(clientInfo, "GLModule", "GetServerPath", null);
            xomlFile = (string)obj[1] + xomlFile;
            object[] objs = new object[]
            {
                null,
                new object[]{
                    xomlFile,
                    string.Empty,
                    0,
                    0,
                    string.Empty,
                    string.Empty,
                    provider,
                    0,
                    string.Empty,
                    string.Empty
                },
                new object[]{
                    keys,
                    keyValues
                }
            };

            return CallFLMethod(clientInfo, "Pause", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //該功能只為EEP的FLDesigner.exe提供使用
    [WebMethod]
    public object[] BackForAllPlatform(string securityKey, string listId, string flowPath)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] objs = new object[]
            {
                new Guid(listId),
                flowPath
            };

            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            return CallFLMethod(clientInfo, "Return3", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //該功能只為EEP的FLDesigner.exe提供使用
    [WebMethod]
    public object[] ForwardForAllPlatform(string securityKey, string listId, string flowPath)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] objs = new object[]
            {
                new Guid(listId),
                flowPath
            };

            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            return CallFLMethod(clientInfo, "Approve3", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //作廢
    [WebMethod]
    public object[] RejectForAllPlatform(string securityKey, string listId, string flowPath, int sendNotifyToAllRoles)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss0 = securityKey.Split('-');
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);
            string userId = ss0[0];
            DataSet todoListSet = GetCurrentDoListForAllAllPlatform(securityKey, dbType, userId, listId, flowPath);
            if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
            {
                return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
            }
            DataTable todoListTable = todoListSet.Tables[0];
            string[] ss = flowPath.Split(';');
            string currentFLActivity = ss[0];
            string nextFLActivity = ss[1];
            string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
            string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
            string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{currentFLActivity, nextFLActivity, sendNotifyToAllRoles, provider },
                new object[]{
                    keys,
                    keyValues
                }
            };


            return CallFLMethod(clientInfo, "Reject", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //退回
    [WebMethod]
    public object[] ReturnForAllPlatform(string securityKey, string listId, string flowPath, int important, int urgent, string remark, string roleId, string orgKid
        , string url)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss0 = securityKey.Split('-');
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);
            string userId = ss0[0];
            DataSet todoListSet = GetCurrentDoListForAllAllPlatform(securityKey, dbType, userId, listId, flowPath);
            if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
            {
                return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
            }
            DataTable todoListTable = todoListSet.Tables[0];
            string[] ss = flowPath.Split(';');
            string currentFLActivity = ss[0];
            string nextFLActivity = ss[1];
            string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
            string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
            string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    currentFLActivity,
                    nextFLActivity,
                    important,
                    urgent,
                    remark,
                    roleId,
                    provider,
                    url,
                    orgKid,
                    string.Empty //attachments
                },
                new object[]{
                    keys,
                    keyValues
                }
            };

            return CallFLMethod(clientInfo, "Return", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //退回特定流程位置
    [WebMethod]
    public object[] ReturnToStepForAllPlatform(string securityKey, string listId, string flowPath, string returnStep, int important, int urgent, string remark, string roleId, string orgKid
        , string url)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss0 = securityKey.Split('-');
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);

            string userId = ss0[0];
            DataSet todoListSet = GetCurrentDoListForAllAllPlatform(securityKey, dbType, userId, listId, flowPath);
            if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
            {
                return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
            }
            DataTable todoListTable = todoListSet.Tables[0];
            string[] ss = flowPath.Split(';');
            string currentFLActivity = returnStep;
            string nextFLActivity = ss[1];
            string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
            string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
            string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    currentFLActivity,
                    nextFLActivity,
                    important,
                    urgent,
                    remark,
                    roleId,
                    provider,
                    url,
                    orgKid,
                    string.Empty //attachments
                },
                new object[]{
                    keys,
                    keyValues
                }
            };

            return CallFLMethod(clientInfo, "Return2", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //取得server所有流程的描述
    [WebMethod]
    public object[] GetFLDescriptionsForAllPlatform(string securityKey)
    {

        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] obj = CallMethod(clientInfo, "GLModule", "GetServerPath", null);
            string path = string.Format("{0}\\WorkFlow\\", obj[1]);
            obj = CallFLMethod(clientInfo, "GetFLDescriptions", new object[] { path });
            if (obj.Length > 1 && obj[1] is string[])
            {
                System.Collections.ArrayList array = new System.Collections.ArrayList((string[])obj[1]);
                obj[1] = array.ToArray();
            }
            return obj;
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //取得流程路徑位置
    [WebMethod]
    public object[] GetFLPathListForAllPlatform(string securityKey, string listId)
    {

        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);

            object[] obj = CallFLMethod(clientInfo, "GetFLPathList", new object[] { new Guid(listId) });
            if (obj.Length > 1 && obj[1] is string[])
            {
                System.Collections.ArrayList array = new System.Collections.ArrayList((string[])obj[1]);
                obj[1] = array.ToArray();
            }
            return obj;
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //取回
    [WebMethod]
    public object[] RetakeForAllPlatform(string securityKey, string listId, string stepId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string sql = "SELECT * FROM SYS_TODOHIS WHERE LISTID='" + listId + "' AND D_STEP_ID='" + stepId + "' ORDER BY UPDATE_TIME DESC";
            DataSet ds = null;
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] ret1 = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", new object[] { sql });
            if (ret1 != null && (int)ret1[0] == 0)
            {
                ds = (DataSet)ret1[1];
            }
            if (ds == null || ds.Tables.Count == 0)
            {
                return new object[] { 1, "ToDoHis" };
            }
            DataRow row = ds.Tables[0].Rows[0];
            string currentFLActivity = row["S_STEP_ID"].ToString();
            string keys = row["FORM_KEYS"].ToString();
            string keyValues = row["FORM_PRESENTATION"].ToString();
            keyValues = keyValues.Replace("'", "''");

            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    currentFLActivity,
                    string.Empty //attachments
                },
                new object[]{
                    keys,
                    keyValues
                }
            };

            return CallFLMethod(clientInfo, "Retake", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //通知
    [WebMethod]
    public object[] NotifyForAllPlatform(string securityKey, string listId, string flowPath, string remark, string sendToIds)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss0 = securityKey.Split('-');
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);
            string userId = ss0[0];
            DataSet todoListSet = GetCurrentDoListForAllAllPlatform(securityKey, dbType, userId, listId, flowPath);
            if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
            {
                return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
            }
            DataTable todoListTable = todoListSet.Tables[0];
            string[] ss = flowPath.Split(';');
            string currentFLActivity = ss[0];
            string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
            string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
            string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    currentFLActivity,
                    currentFLActivity,
                    0,
                    0,
                    remark,
                    string.Empty,
                    provider,
                    0,
                    sendToIds,
                    string.Empty
                },
                new object[]{
                    keys,
                    keyValues
                }
            };

            return CallFLMethod(clientInfo, "Notify", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }


    [WebMethod]
    public object[] ChangeSendToForAllPlatform(string securityKey, string listId, string flowPath, string sendToIds)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss0 = securityKey.Split('-');
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    flowPath,
                    sendToIds
                },
                new object[]{
                 
                }
            };

            return CallFLMethod(clientInfo, "ChangeSendTo", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }
    //刪除通知
    [WebMethod]
    public object[] DeleteNotifyForAllPlatform(string securityKey, string listId, string flowPath)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] objs = new object[]
            {
                listId,
                flowPath
            };
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            return CallFLMethod(clientInfo, "DeleteNotify", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //加簽
    [WebMethod]
    public object[] PlusForAllPlatform(string securityKey, string listId, string flowPath, string remark, string roleId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss0 = securityKey.Split('-');
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);

            string userId = ss0[0];
            DataSet todoListSet = GetCurrentDoListForAllAllPlatform(securityKey, dbType, userId, listId, flowPath);
            if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
            {
                return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
            }
            DataTable todoListTable = todoListSet.Tables[0];
            string[] ss = flowPath.Split(';');
            string nextFLActivity = ss[1];
            string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
            string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
            string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");
            string sendToIds = todoListTable.Rows[0]["SENDTO_ID"].ToString();

            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    nextFLActivity,
                    nextFLActivity,
                    0,
                    0,
                    remark,
                    sendToIds,
                    provider,
                    0,
                    roleId,
                    string.Empty
                },
                new object[]{
                    keys,
                    keyValues
                }
            };

            return CallFLMethod(clientInfo, "PlusApprove", objs);
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //加簽返回
    [WebMethod]
    public object[] PlusReturnForAllPlatform(string securityKey, string listId, string flowPath, string remark, string roleId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss0 = securityKey.Split('-');
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);

            string userId = ss0[0];
            DataSet todoListSet = GetCurrentDoListForAllAllPlatform(securityKey, dbType, userId, listId, flowPath);
            if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
            {
                return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
            }
            DataTable todoListTable = todoListSet.Tables[0];
            string[] ss = flowPath.Split(';');
            string nextFLActivity = ss[1];
            string provider = todoListTable.Rows[0]["PROVIDER_NAME"].ToString();
            string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
            string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");

            object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    nextFLActivity,
                    nextFLActivity,
                    0,
                    0,
                    remark,
                    roleId,
                    provider,
                    0,
                    string.Empty,
                    string.Empty
                },
                new object[]{
                    keys,
                    keyValues
                }
            };

            object[] objParams = CallFLMethod(clientInfo, "PlusReturn", objs);
            if (Convert.ToInt16(objParams[0]) == 0)
            {
                if (objParams[1].ToString() == "60585C77-60E1-4e6f-A2E2-3BBBAD6B4C9E")
                {
                    return new object[] { 0, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLClientControls", "FLNavigator", "RunOver", true) };
                }
                else if (objParams[1].ToString() == "512F4277-0D41-441c-BF16-D96B04580C2E")
                {
                    return new object[] { 0, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLClientControls", "FLNavigator", "HasRejected", true) };
                }
                else if (objParams[1].ToString() == "")
                {
                    string wait = SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLTools", "GloFix", "WaitMessage", true);

                    string sql = "select SENDTO_KIND, SENDTO_ID from SYS_TODOLIST where LISTID='" + listId + "'";
                    DataTable dtOthers = null;

                    CliUtils.fLoginDB = ss0[1];
                    object[] ret1 = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", new object[] { sql });
                    if (ret1 != null && (int)ret1[0] == 0)
                    {
                        dtOthers = ((DataSet)ret1[1]).Tables[0];
                    }
                    string sendToIds = "";
                    foreach (DataRow row in dtOthers.Rows)
                    {
                        string sendtokind = row["SENDTO_KIND"].ToString();
                        string sendtoid = row["SENDTO_ID"].ToString();
                        if (sendtokind == "1")
                        {
                            sendToIds += sendtoid + ";";
                        }
                        else if (sendtokind == "2")
                        {
                            sendToIds += sendtoid + ":UserId;";
                        }
                    }
                    if (sendToIds != "")
                    {
                        return new object[] { 0, GloFix.ShowParallelMessage(sendToIds) };
                    }
                }
            }
            return objParams;
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //取得當前User待辦事項
    [WebMethod]
    public DataSet GetToDoListForAllAllPlatform(string securityKey)
    {
        string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
        //string[] ss = securityKey.Split("-".ToCharArray());
        string userId = ss[0];
        object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
        int dbType = (int)GetDataBaseType(clientInfo, securityKey);

        return GetToDoListForAllAllPlatform2(securityKey, userId);
    }

    //取得某User待辦事項
    [WebMethod]
    public DataSet GetToDoListForAllAllPlatform2(string securityKey, string userId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss = securityKey.Split("-".ToCharArray());
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);

            string connectMark = "+";
            switch (dbType)
            {
                case 1: connectMark = "+"; break;
                case 2: connectMark = "+"; break;
                case 3: connectMark = "||"; break;
                case 4: connectMark = "+"; break;
            }

            string sql = string.Empty;
            if (dbType == 5)
            {
                sql = string.Format(SQL_TODOLIST1, userId, GetDateTimeString(DateTime.Now));
            }
            else
            {
                sql = string.Format(SQL_TODOLIST2, userId, GetDateTimeString(DateTime.Now), connectMark);
            }

            object[] excuteParam = new object[1] { sql };
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

            DataTable table = null;
            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            {
                return (DataSet)objs[1];
            }

            return null;
        }
        else
        {
            return null;
        }
    }

    //取得當前User經辦事項
    [WebMethod]
    public DataSet GetToDoHisForAllAllPlatform(string securityKey)
    {
        string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
        //string[] ss = securityKey.Split("-".ToCharArray());
        string userId = ss[0];
        object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
        int dbType = (int)GetDataBaseType(clientInfo, securityKey);
        return GetToDoHisForAllAllPlatform2(securityKey, userId);
    }

    //取得某User經辦事項
    [WebMethod]
    public DataSet GetToDoHisForAllAllPlatform2(string securityKey, string userId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss = securityKey.Split("-".ToCharArray());
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);

            string connectMark = "+";
            switch (dbType)
            {
                case 1: connectMark = "+"; break;
                case 2: connectMark = "+"; break;
                case 3: connectMark = "||"; break;
                case 4: connectMark = "+"; break;
            }

            string sql = string.Empty;
            if (dbType == 5)
            {
                sql = string.Format(SQL_TODOHIS1, userId);
            }
            else
            {
                sql = string.Format(SQL_TODOHIS2, userId, connectMark);
            }

            object[] excuteParam = new object[1] { sql };
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

            DataTable table = null;
            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            {
                return (DataSet)objs[1];
            }

            return null;
        }
        else
        {
            return null;
        }
    }

    //取得逾時事項
    [WebMethod]
    public DataSet GetRunOverForAllAllPlateform(string securityKey, string userId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss = securityKey.Split("-".ToCharArray());
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);

            string connectMark = "+";
            switch (dbType)
            {
                case 1:
                case 2:
                    connectMark = "+"; break;
                case 3:
                case 4:
                case 5:
                case 6:
                    connectMark = "||"; break;
            }

            string sql = string.Empty;
            if (dbType == 5)
            {
                sql = string.Format(SQL_RUNOVER1, userId);
            }
            else
            {
                sql = string.Format(SQL_RUNOVER2, userId, connectMark);
            }

            object[] excuteParam = new object[1] { sql };
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

            DataTable table = null;
            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            {
                DataSet dataset = (DataSet)objs[1];
                //get applicator
                dataset.Tables[0].Columns.Add(new DataColumn("APPLICATE", typeof(string)));
                for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                {
                    string listID = dataset.Tables[0].Rows[i]["LISTID"].ToString();
                    string applicateSql = string.Format(SQL_APPLICATE, listID);
                    object[] objApplicate = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", new object[] { applicateSql });
                    if (objApplicate[0].ToString() == "0" && objApplicate[1] is DataSet && ((DataSet)objApplicate[1]).Tables.Count != 0)
                    {
                        DataSet applicateDataSet = (DataSet)objApplicate[1];
                        if (applicateDataSet.Tables[0].Rows.Count > 0)
                        {
                            dataset.Tables[0].Rows[i]["APPLICATE"] = string.Format("{0}({1})", applicateDataSet.Tables[0].Rows[0]["USER_ID"], applicateDataSet.Tables[0].Rows[0]["USERNAME"]);
                        }
                    }
                }
                dataset.AcceptChanges();
                return dataset;

            }
            return null;
        }
        else
        {
            return null;
        }
    }

    //取得簽核歷程
    [WebMethod]
    public DataSet GetCommentForAllAllPlatform(string securityKey, string listId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string sql = string.Format(SQL_COMMENT, listId);

            object[] excuteParam = new object[1] { sql };
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

            DataTable table = null;
            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            {
                return (DataSet)objs[1];
            }

            return null;
        }
        else
        {
            return null;
        }
    }

    //取得當前User通知事項
    [WebMethod]
    public DataSet GetNotifyForAllAllPlatform(string securityKey)
    {
        string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
        //string[] ss = securityKey.Split("-".ToCharArray());
        string userId = ss[0];
        object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
        int dbType = (int)GetDataBaseType(clientInfo, securityKey);
        return GetNotifyForAllAllPlatform2(securityKey, userId);
    }

    //取得某User通知事項
    [WebMethod]
    public DataSet GetNotifyForAllAllPlatform2(string securityKey, string userId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string[] ss = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
            //string[] ss = securityKey.Split("-".ToCharArray());
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            int dbType = (int)GetDataBaseType(clientInfo, securityKey);

            string connectMark = "+";
            switch (dbType)
            {
                case 1: connectMark = "+"; break;
                case 2: connectMark = "+"; break;
                case 3: connectMark = "||"; break;
                case 4: connectMark = "+"; break;
            }

            string sql = string.Empty;
            if (dbType == 5)
            {
                sql = string.Format(SQL_NOTIFY1, userId, GetDateTimeString(DateTime.Now), connectMark);
            }
            else
            {
                sql = string.Format(SQL_NOTIFY2, userId, GetDateTimeString(DateTime.Now), connectMark);
            }

            object[] excuteParam = new object[1] { sql };
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

            DataTable table = null;
            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            {
                return (DataSet)objs[1];
            }

            return null;
        }
        else
        {
            return null;
        }
    }

    //取得當前User逾時事項
    [WebMethod]
    public DataSet GetDelayForAllAllPlatform(string securityKey)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            //if (string.IsNullOrEmpty(roleId))
            //{
            //    object[] ret = CallMethod(clientInfo, "GLModule", "GetUserRole", null);
            //    if (ret != null && (int)ret[0] == 0)
            //    {
            //        roleId = ((string)ret[2]).Replace(";", "','");                    
            //    }
            //}

            //string sql = string.Format(SQL_DELAY, roleId);

            //object[] excuteParam = new object[1] { sql };
            //object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);
            object[] objs = CallMethod(clientInfo, "GLModule", "FLOvertimeList", new object[] { clientInfo[0], 2, true, null, true });
            if (objs != null && (int)objs[0] == 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add((DataTable)objs[1]);
                return ds;
            }
            //DataTable table = null;
            //if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            //{
            //    return (DataSet)objs[1];
            //}

            return null;
        }
        else
        {
            return null;
        }
    }

    #endregion

    //取得某User待辦資料，效果同GetCurrentToDoList
    public DataSet GetCurrentDoListForAllAllPlatform(string securityKey, int dbType, string userId, string listId, string flowPath)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string connectMark = "+";
            switch (dbType)
            {
                case 1: connectMark = "+"; break;
                case 2: connectMark = "+"; break;
                case 3: connectMark = "||"; break;
                case 4: connectMark = "+"; break;
            }

            string sql = string.Empty;
            if (dbType == 5)
            {
                sql = string.Format(SQL_TODOLIST3, userId, GetDateTimeString(DateTime.Now), listId, flowPath);
            }
            else
            {
                sql = string.Format(SQL_TODOLIST4, userId, GetDateTimeString(DateTime.Now), connectMark, listId, flowPath);
            }

            object[] excuteParam = new object[1] { sql };
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

            DataTable table = null;
            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            {
                return (DataSet)objs[1];
            }

            return null;
        }
        else
        {
            return null;
        }
    }


    //流程預覽
    [WebMethod]
    public object[] GetPreviewForAllPlatform(string securityKey, string listId, string flowPath, string xomlFile, string roleId, DataSet host, string operate, bool isImage)
    {
        string[] ss0 = System.Text.RegularExpressions.Regex.Split(securityKey, PublicKey.SPLIT_STRING);
        object[] clientInfo = InitClientInfoForAllPlatform(securityKey);

        int dbType = (int)GetDataBaseType(clientInfo, securityKey);
        string userId = ss0[0];

        DataSet todoListSet = GetCurrentDoListForAllAllPlatform(securityKey, dbType, userId, listId, flowPath);
        if (todoListSet == null || todoListSet.Tables[0].Rows.Count == 0)
        {
            return new object[] { 1, SysMsg.GetSystemMessage(SYS_LANGUAGE.ENG, "FLRuntime", "InstanceManager", "RecordNotInToDoList", true) };
        }
        DataTable todoListTable = todoListSet.Tables[0];
        string keys = todoListTable.Rows[0]["FORM_KEYS"].ToString();
        string keyValues = todoListTable.Rows[0]["FORM_PRESENTATION"].ToString().Replace("'", "''");
        string activityname = operate == "Approve" ? flowPath.Split(';')[1] : "";


        object[] obj = CallMethod(clientInfo, "GLModule", "GetServerPath", null);
        xomlFile = (string)obj[1] + xomlFile;

        object[] objs = new object[]
            {
                new Guid(listId),
                new object[]{
                    xomlFile,
                    string.Empty,
                    activityname,
                    host,
                    roleId
                },
                new object[]{
                    keys,
                    keyValues
                },
                isImage
            };

        object[] ret = CallFLMethod(clientInfo, "Preview", objs);

        return ret;

        //if ((int)ret[0] == 0 && ret[1] != null)
        //{
        //    if (ret[1] is byte[])
        //    {

        //    }
        //}
        //return null;
    }

    //public DataSet GetCurrentToDoList(int dbType, string userId, string listId, string flowPath)
    //{
    //    string connectMark = "+";
    //    switch (dbType)
    //    {
    //        case 1: connectMark = "+"; break;
    //        case 2: connectMark = "+"; break;
    //        case 3: connectMark = "||"; break;
    //        case 4: connectMark = "+"; break;
    //    }

    //    string sql = string.Empty;
    //    if (dbType == 5)
    //    {
    //        sql = string.Format(SQL_TODOLIST3, userId, GetDateTimeString(DateTime.Now), listId, flowPath);
    //    }
    //    else
    //    {
    //        sql = string.Format(SQL_TODOLIST4, userId, GetDateTimeString(DateTime.Now), connectMark, listId, flowPath);
    //    }

    //    object[] excuteParam = new object[1] { sql };
    //    InitClientInfo();
    //    object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

    //    DataTable table = null;
    //    if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
    //    {
    //        return (DataSet)objs[1];
    //    }

    //    return null;
    //}

    private string GetDateTimeString(DateTime date)
    {
        string year = date.Year.ToString();
        string month = dformat(date.Month.ToString());
        string day = dformat(date.Day.ToString());
        string hour = dformat(date.Hour.ToString());
        string minute = dformat(date.Minute.ToString());
        string second = dformat(date.Second.ToString());

        return year + month + day + hour + minute + second;
    }

    private string dformat(string datePart)
    {
        return (datePart.Length == 2) ? datePart : "0" + datePart;
    }

    //異動代理人
    [WebMethod]
    public object[] UpdateAgent(string securityKey, string mode, string roleid, string agent, string flowdesc, string startDate, string startTime, string endDate, string endTime, string parAgent, string remark)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            var sql = string.Empty;
            if (mode == "D")
            {
                sql = string.Format("DELETE FROM SYS_ROLES_AGENT WHERE {0}", GetAgentWhere(roleid, agent, flowdesc));
            }
            else if (mode == "U")
            {
                sql = string.Format("UPDATE SYS_ROLES_AGENT SET START_DATE = '{0}', START_TIME = '{1}', END_DATE = '{2}', END_TIME = '{3}', PAR_AGENT = '{4}', REMARK ='{5}' WHERE {6}"
                    , startDate, startTime, endDate, endTime, parAgent, remark, GetAgentWhere(roleid, agent, flowdesc));
            }
            else if (mode == "I")
            {
                sql = string.Format("INSERT INTO SYS_ROLES_AGENT (ROLE_ID, AGENT, FLOW_DESC, START_DATE, START_TIME, END_DATE, END_TIME, PAR_AGENT, REMARK) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')"
                    , roleid, agent, flowdesc, startDate, startTime, endDate, endTime, parAgent, remark);
            }

            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] objs = CallMethod(clientInfo, "GLModule", "UpdateWorkFlow", new object[] { sql });
            return objs;
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    //抓代理人資訊
    [WebMethod]
    public DataSet GetAgent(string securityKey, string roleid, string agent, string flowdesc)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            var sql = string.Format("SELECT * FROM SYS_ROLES_AGENT WHERE {0}", GetAgentWhere(roleid, agent, flowdesc));
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", new object[] { sql });

            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            {
                return (DataSet)objs[1];
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    private string GetAgentWhere(string roleid, string agent, string flowdesc)
    {
        return string.Format("ROLE_ID = '{0}'{1}{2}", roleid
            , string.IsNullOrEmpty(agent) ? string.Empty : string.Format(" AND AGENT = '{0}'", agent)
            , string.IsNullOrEmpty(flowdesc) ? string.Empty : string.Format(" AND FLOW_DESC = '{0}'", flowdesc));
    }

    //抓取流程中所有簽核人員
    [WebMethod]
    public DataSet GetFlowUserForAllPlotForm(string securityKey, string listId)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            string sql = string.Format(SQL_GETUSERS, listId);

            object[] excuteParam = new object[1] { sql };
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);

            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
            {
                return (DataSet)objs[1];
            }

            return null;
        }
        else
        {
            return null;
        }
    }

    #region joy 2010/2/23 add : for MOSS WebPart

    [WebMethod]
    public DataSet EEPExecuteSQL(string securityKey, string strSQL)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] excuteParam = new object[1] { strSQL };
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] objs = CallMethod(clientInfo, "GLModule", "ExcuteWorkFlow", excuteParam);
            if (objs[0].ToString() == "0" && objs[1] is DataSet && ((DataSet)objs[1]).Tables.Count != 0)
                return (DataSet)objs[1];
            else
                return null;
        }
        else
        {
            return null;
        }
    }

    [WebMethod]
    public DataSet EEPGetSolution(string securityKey)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] ret = CallMethod(clientInfo, "GLModule", "GetSolution", null);
            if (ret != null && (int)ret[0] == 0)
                return (DataSet)ret[1];
            else
                return null;
        }
        else
        {
            return null;
        }
    }

    [WebMethod]
    public DataSet EEPFetchMenus(string securityKey, string Param1, string Param2)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            object[] strParam = new object[2];
            strParam[0] = Param1;
            strParam[1] = Param2;
            object[] ret = CallMethod(clientInfo, "GLModule", "FetchMenus", strParam);
            if (ret != null && (int)ret[0] == 0)
                return (DataSet)ret[1];
            else
                return null;
        }
        else
        {
            return null;
        }
    }



    [WebMethod]
    public object[] EEPGetPreview(string securityKey, Guid id, string activityname, string role, string keys, string values, bool isImage)
    {
        if (PublicKey.CheckPublicKey2(securityKey))
        {
            object[] clientInfo = InitClientInfoForAllPlatform(securityKey);
            return CallFLMethod(clientInfo, "Preview", new object[] { id, new object[] { ".xoml", "", activityname, null, role }, 
                                                                       new object[] { keys, values }, isImage});
        }
        else
        {
            return new object[] { 1, "You must login first." };
        }
    }

    private static Dictionary<string, byte[]> Transactions = new Dictionary<string, byte[]>();

    [WebMethod]
    public object[] BeginTransactionScope(string userID, byte[] tx)
    {
        if (Transactions.ContainsKey(userID))
        {
            return new object[] { 0, false };
        }
        else
        {
            Transactions[userID] = tx;
            return new object[] { 0, true };
        }
    }

    [WebMethod]
    public object[] EndTransactionScope(string userID)
    {
        if (Transactions.ContainsKey(userID))
        {
            Transactions.Remove(userID);
        }
        return new object[] { 0 };
    }

    #endregion
}

