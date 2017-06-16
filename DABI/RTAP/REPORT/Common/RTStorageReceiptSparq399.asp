<%@ LANGUAGE="VBSCRIPT" %>
 
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%

	logonid=Request.ServerVariables("LOGON_USER")
	Call SrGetEmployeeRef(Rtnvalue,1,logonid)
	logonid=split(rtnvalue,";")  

	parm=request("key")
	v=split(parm,";")  

	DIM CONNXX
	Set connXX=Server.CreateObject("ADODB.Connection")  
	SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
	DSN="DSN=RtLib"
	connXX.Open DSN
	'  On Error Resume Next
	sqlxx="select GTEQUIP, GTPRTDAT FROM RTSparqAdslCust WHERE comq1=" &V(0)& " and entryno=" &V(2)& " AND CUSID='" &V(1)& "' "
	RSXX.OPEN SQLXX,CONNxx
	endpgm=""
	IF len(trim(RSXX("GTPRTDAT"))) > 0 THEN
		if datevalue(RSXX("GTPRTDAT")) < datevalue(now()) then    
			'ENDPGM="1"
		END IF
	ELSEIF len(trim(RSXX("GTEQUIP"))) = 0 THEN
			ENDPGM="2"
	END IF
	RSXX.CLOSE
	connXX.Close
	SET RSXX=NOTHING
	set connXX=nothing

	IF ENDPGM ="" THEN
		reportname = "RTStorageReceipt.rpt"

		If Not IsObject (session("oApp")) Then                              
			Set session("oApp") = Server.CreateObject("CrystalRuntime.Application")
		End If                                                               

		Path = Request.ServerVariables("PATH_TRANSLATED")                     
		While (Right(Path, 1) <> "\" And Len(Path) <> 0)                      
		iLen = Len(Path) - 1                                                  
		Path = Left(Path, iLen)                                               
		Wend                                                                  
		                                                                      
		If IsObject(session("oRpt")) then
			Set session("oRpt") = nothing
		End if

		On error resume next

		Set session("oRpt") = session("oApp").OpenReport(path & reportname, 1)

		If Err.Number <> 0 Then
		Response.Write "Error Occurred creating Report Object: " & Err.Description
		Set Session("oRpt") = nothing
		Set Session("oApp") = nothing
		Session.Abandon
		Response.End
		End If

		session("oRpt").MorePrintEngineErrorMessages = False
		session("oRpt").EnableParameterPrompting = False
		session("oRpt").DiscardSavedData

		'UserId = "sa"
		'Password = ""

		Set oMainReportTable = Session("oRpt").Database.Tables.Item(1)

		oMainReportTable.SetLogonInfo "rtlib", "RTLib", CStr(Userid), CStr(Password)

		Session("oRpt").ParameterFields.GetItemByName("@casetype").AddCurrentValue(CStr("速博399"))
		Session("oRpt").ParameterFields.GetItemByName("@comq1").AddCurrentValue(CInt(V(0)))
		Session("oRpt").ParameterFields.GetItemByName("@lineq1").AddCurrentValue(CInt(V(2)))
		Session("oRpt").ParameterFields.GetItemByName("@cusid").AddCurrentValue(CStr(V(1)))
		Session("oRpt").ParameterFields.GetItemByName("@gtprtusr").AddCurrentValue(CStr(logonid(0)))

		On Error Resume Next

		session("oRpt").ReadRecords

		If Err.Number <> 0 Then                                               
		Response.Write "Error Occurred Reading Records: " & Err.Description
		Set Session("oRpt") = nothing
		Set Session("oApp") = nothing
		Session.Abandon
		Response.End
		Else
		If IsObject(session("oPageEngine")) Then                              
  			set session("oPageEngine") = nothing
		End If
		set session("oPageEngine") = session("oRpt").PageEngine
		End If
	END IF
%>
 
                                                                     
<HTML>
<HEAD>
<title>速博399設備保管收據列印</title>
</HEAD>
<BODY BGCOLOR=C6C6C6 ONUNLOAD="CallDestroy();" topmargin=0 leftmargin=0>

<OBJECT ID="CRViewer"
	CLASSID="CLSID:C4847596-972C-11D0-9567-00A0C9273C2A"
	WIDTH=100% HEIGHT=99%
	 codebase="/viewer/activeXViewer/activexviewer.cab#Version=8,5,0,217">
<PARAM NAME="EnableRefreshButton" VALUE=1>
<PARAM NAME="EnableGroupTree" VALUE=1>
<PARAM NAME="DisplayGroupTree" VALUE=1>
<PARAM NAME="EnablePrintButton" VALUE=1>
<PARAM NAME="EnableExportButton" VALUE=1>
<PARAM NAME="EnableDrillDown" VALUE=1>
<PARAM NAME="EnableSearchControl" VALUE=1>
<PARAM NAME="EnableAnimationControl" VALUE=1>
<PARAM NAME="EnableZoomControl" VALUE=1>
</OBJECT>

<form name=frm1 method=post action=RTStorageReceiptSparq399.asp ID="Form1">
	<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
	<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>

<SCRIPT LANGUAGE="VBScript">
<!--
Sub Window_Onload
    IF frm1.htmlfld.value="1" THEN
		msgbox "保證金收據隔天不可列印" & "  " & frm1.htmlfld1.value
		Set winP=window.Opener
		winP.focus()
		window.close
    ELSEIF frm1.htmlfld.value="2" THEN
		msgbox "用戶保管設備空白, 無法列印" & "  " & frm1.htmlfld1.value
		Set winP=window.Opener
		winP.focus()
		window.close
    ELSE  
		On Error Resume Next
		Dim webBroker
		Set webBroker = CreateObject("WebReportBroker.WebReportBroker")
		if ScriptEngineMajorVersion < 2 then
			window.alert "IE 3.02 users on NT4 need to get the latest version of VBScript or install IE 4.01 SP1. IE 3.02 users on Win95 need DCOM95 and latest version of VBScript, or install IE 4.01 SP1. These files are available at Microsoft's web site."
			CRViewer.ReportName = "rptserver.asp"
		else
			Dim webSource
			Set webSource = CreateObject("WebReportSource.WebReportSource")
			webSource.ReportSource = webBroker
			webSource.URL = "rptserver.asp"
			webSource.PromptOnRefresh = True
			CRViewer.ReportSource = webSource
		end if
		CRViewer.ViewReport
	END IF
End Sub
-->
</SCRIPT>

<script language="javascript">
function CallDestroy()
{
	window.open("Cleanup.asp","Cleanup","status=no,toolbar=no,location=no,menu=no,scrollbars=no,width=1,height=1");
        self.focus()
}
</script>

</BODY>
</HTML>
