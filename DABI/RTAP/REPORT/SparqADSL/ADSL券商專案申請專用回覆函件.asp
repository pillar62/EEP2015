<%@ LANGUAGE="VBSCRIPT" %>
 
                                                                     
<HTML>
<HEAD>
    <title>ADSL券商專案申請專用回覆函件</title>
<% 
  reportname = "ADSL券商專案申請專用回覆函件.rpt"
   
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
  Set ThisParam3 = StoredProcParamCollection.item(3)
  Set ThisParam4 = StoredProcParamCollection.item(4)


  NewParamValue = "1990/01/01"
  NewParamValue2 = "2001/08/31"
  NewParamValue3 = "3"   ' 1: 未列印  2: 已列印  3: 全部
  NewParamValue4 = "2"   ' 1: 可建置&未達三人  2: 不可建置  3: 退租戶 
    
  ThisParam.SetCurrentValue cdate(NewParamValue)
  ThisParam2.SetCurrentValue cdate(NewParamValue2)
  ThisParam3.SetCurrentValue cstr(NewParamValue3)
  ThisParam4.SetCurrentValue cstr(NewParamValue4)
    
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
 
                                                                     
<TITLE>Seagate ActiveX Viewer</TITLE>
</HEAD>
<BODY BGCOLOR=C6C6C6 LANGUAGE=VBScript ONLOAD="Page_Initialize">

<OBJECT ID="CRViewer"
	CLASSID="CLSID:C4847596-972C-11D0-9567-00A0C9273C2A"
	WIDTH=100% HEIGHT=100%
	CODEBASE="/viewer/activeXViewer/activexviewer.cab#Version=8,6,1,567">
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
Sub Page_Initialize
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

</BODY>
</HTML>





