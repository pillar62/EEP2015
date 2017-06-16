<%@ LANGUAGE="VBSCRIPT" %>
<% 
reportname = "rtaccountletter.rpt"

sqlstring2=session("SQLString2")  
otherstring=" and rtcustadsl.cusid='" & session("key1") & "' and rtcustadsl.entryno=" & session("key2")
session("sqlstring1")=session("sqlstring1") & replace(otherstring,"'","""")  
sqlstring1=session("SQLString1")    
                                                                     
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


' 不動 SELECT_String 及 the FROM_String
SELECT_String ="SELECT RTCustADSL.ss365, RTObj.CUSNC, " &_
               "RTCustADSL.HOME " &_
               "RTCustADSL.OFFICE, RTCustADSL.EXTENSION " &_
               "RTCustADSL.MOBILE, " &_ 
               "RTCustADSL.RZONE2, " &_ 
               "RTCounty.CUTNC, RTCustADSL.TOWNSHIP2, RTCustADSL.RADDR2, " &_
               "RTCustADSL.EMAIL, RT365account.ss365ACCOUNT, RT365account.ss365PWD "

FROM_String = "FROM RTCustADSL LEFT OUTER JOIN RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID " &_
              "LEFT OUTER JOIN RT365ACCOUNT ON RTCustADSL.CUSID = RT365ACCOUNT.CUSID AND RTCustADSL.ENTRYNO = RT365ACCOUNT.ENTRYNO " &_
              "LEFT OUTER JOIN RTObj ON RTCustADSL.CUSID = RTObj.CUSID "

' 只修改 WhereSting 
'WHERE_String = "WHERE (RTCustADSL.DELIVERDAT IS NOT NULL) AND '(RTCustADSL.DROPDAT IS NULL) " &_
'               "AND   rtcustadsl.housename <> '' and rtcustadsl.cusid <>'' " '&_
'               "AND   rtcustadsl.socialid <>'*' and rtcustadsl.cusno <>'*' " '&_
'               "AND   rtCUSTadsl.ss365<>'*' "&_
'               "ORDER BY rtobj.shortnc "
where_string=sqlstring1 & sqlstring2
'---------------------------------------------------------------------
' 最後產生完整的 SQL Query
NewSQLQueryString = SELECT_String & CHR(10)& CHR(13)& FROM_String & CHR(10)& CHR(13) & WHERE_String
session("oRpt").SQLQueryString = cstr(NewSQLQueryString)


'Response.Write (NewSQLQueryString)


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
   <TITLE>先看先贏帳號密碼單信封列印</TITLE>
</HEAD>
<BODY BGCOLOR=C6C6C6 LANGUAGE=VBScript ONLOAD="Page_Initialize">

<OBJECT ID="CRViewer"
	CLASSID="CLSID:C4847596-972C-11D0-9567-00A0C9273C2A"
	WIDTH=100% HEIGHT=95%
	 codebase="/viewer/activeXViewer/activexviewer.cab#Version=8,6,1,567">
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



