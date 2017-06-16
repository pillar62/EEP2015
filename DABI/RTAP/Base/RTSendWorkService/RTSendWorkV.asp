<%@ LANGUAGE="VBSCRIPT" %>
 

<% 
  reportname = "RTSendwork.rpt"
   
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

  Set session("oRpt") = session("oApp").OpenReport(path & reportname, 1)

  session("oRpt").MorePrintEngineErrorMessages = False
  session("oRpt").EnableParameterPrompting = False
  
  'userid = ""
  'Password= ""
  
  set crtable = session("oRpt").Database.Tables.Item(1)
  crtable.SetLogonInfo "rtlib", "RTLib", cstr(userid),cstr(Password)

  set StoredProcParamCollection = Session("oRpt").ParameterFields

  Set ThisParam = StoredProcParamCollection.item(1)
  Set ThisParam2 = StoredProcParamCollection.item(2)

  NewParamValue = "T89039"
  NewParamValue2 = "%%"
  
  ThisParam.SetCurrentValue cstr(NewParamValue)
  ThisParam2.SetCurrentValue cstr(NewParamValue2)
  
  On Error Resume Next                                                  
  session("oRpt").ReadRecords                                           
  If Err.Number <> 0 Then                                               
     Response.Write "An Error has occured on the server in attempting to access the data source"
  Else

     If IsObject(session("oPageEngine")) Then                              
        set session("oPageEngine") = nothing
     End If
     set session("oPageEngine") = session("oRpt").PageEngine
  End If                                                                
%>
 
                                                                     
<HTML>
<HEAD>
    <title>派工單/回覆單列印</title>
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

