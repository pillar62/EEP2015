<%@ LANGUAGE="VBSCRIPT" %>
 
                                                                     
<HTML>
<HEAD>
    <title>券商專案客戶明細表(三人成行篩選表)</title>
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
  Set ThisParam5 = StoredProcParamCollection.item(5)
  Set ThisParam6 = StoredProcParamCollection.item(6)
  
  
  NewParamValue = v(0)  
  NewParamValue2 = "3"  ' 1: 送件已達三人
                        ' 2: 送件未達三人
                        ' 3: 以上總和
  NewParamValue3 = v(1) '建檔日
  NewParamValue4 = v(2)
  NewParamValue5 = "1990/01/01"   '送件日
  NewParamValue6 = "9999/12/31"   
  
  
  ThisParam.SetCurrentValue cint(NewParamValue)
  ThisParam2.SetCurrentValue cint(NewParamValue2)
  ThisParam3.SetCurrentValue cdate(NewParamValue3)
  ThisParam4.SetCurrentValue cdate(NewParamValue4)
  ThisParam5.SetCurrentValue cdate(NewParamValue5)
  ThisParam6.SetCurrentValue cdate(NewParamValue6)
  
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
	WIDTH=100% HEIGHT=95%
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




