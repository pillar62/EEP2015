<%
'KEY1=comq1 key2=comtype
'KEY2=1:HB 2:中華399 3:速博399
DIM connXX,rsXX,dsn,sqlXX
SET connXX=server.CreateObject("ADODB.Connection")
set rsXX=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
connXX.Open dsn
KEYARY=SPLIT(REQUEST("KEY"),";")
Select case keyary(1)
   CaSE "1"
      SQLXX="SELECT comq1,comn FROM rtcmty where comq1=" & keyary(0)
   CASE "2"
      sqlxx="SELECT cutyid,comn from rtcustadslcmty where cutyid=" & keyary(0)
   CASE "3"
      sqlxx="SELECT cutyid,comn from rtsparqadslcmty where cutyid=" & keyary(0)
   CaSE "4"
      SQLXX="SELECT comq1,comn FROM rtcmty where comq1=" & keyary(0)
   CaSE "5"
      SQLXX="SELECT comq1,comn FROM rtebtcmtyH where comq1=" & keyary(0)
   CaSE "6"
      SQLXX="SELECT comq1,comn FROM RTSparq499CmtyH where comq1=" & keyary(0)
   case else
END select
RSXX.Open SQLXX,CONNXX
IF RSXX.EOF THEN
   session("comq1xx")=""
   SESSION("comnxx")=""
   SESSION("comtypexx")=""
ELSE
   if keyary(1)="2" OR keyary(1)="3" then
      SESSION("comq1xx")=RSXX("CUTYID")
   else
      SESSION("comq1xx")=RSXX("comq1")
   end if
   SESSION("comnxx")=RSXX("comn")
   SESSION("comtypexx")=keyary(1)
END IF
rsxx.close
connxx.Close
set rsxx=nothing
set connxx=nothing
%>
<HTML>
	<HEAD>
		<meta http-equiv="Content-Type" content="text/html; charset=Big5">
		<TITLE>社區挑選</TITLE>
		<SCRIPT language="VBScript">
Sub window_onload()
   ' Window.Opener.document.all("keyform").Submit
  on error resume next
  Dim winP
  Set winP=window.Opener
  winP.document.all("keyform").Submit
  winP.focus()
  window.close  
End Sub
		</SCRIPT>
		</head>
		</html>
