<%@ LANGUAGE="VBSCRIPT" %>
 
<%
  reportname = "ADSLdeliver.rpt"
  parm=request("parm")
  v=split(parm,";")  

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

Session("oRpt").ParameterFields.GetItemByName("@num").AddCurrentValue(Cint(v(0)))
Session("oRpt").ParameterFields.GetItemByName("@typ").AddCurrentValue(Cint("4"))
						' 1: 送件已達三人
                        ' 2: 送件未達三人
                        ' 3: 社區有三戶以上申請 
                        ' 4: 社區只二戶申請
                        ' 5: 社區只一戶申請  
Session("oRpt").ParameterFields.GetItemByName("@deliverdats").AddCurrentValue(Cdate(v(1)))
Session("oRpt").ParameterFields.GetItemByName("@deliverdate").AddCurrentValue(Cdate(v(2)))
Session("oRpt").ParameterFields.GetItemByName("@edats").AddCurrentValue(Cdate("1900/01/01"))
Session("oRpt").ParameterFields.GetItemByName("@edate").AddCurrentValue(Cdate("2999/12/31"))

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
%>
 
                                                                     
<HTML>
<HEAD>
    <title>ADSL券商專案客戶明細表(社區申請數二戶)</title>
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

<SCRIPT LANGUAGE="VBScript">
<!--
Sub Window_Onload
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
