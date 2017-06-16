<%
  Dim search1,parm,vk,debug36,search2
  parm=request("Key")
  vk=split(parm,";")
  if ubound(vk) > 0 then  searchX=vK(0)
%>
<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="COT建置自付額審核確認"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="社區明細"
  functionOptPrompt="N"
  functionOptProgram="RTcotpaycfmkeylist.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  
  DSN="DSN=RTLIb"
  formatName="付款表批號;列印人員;列印日期;社區筆數;估價總額;標準建置額;自付總額;會計審核日;會計審核員"
  sqlDelete="SELECT RTCmty.PAYPRTSEQ, RTObj1.CUSNC AS Expr5, RTCmty.PAYPRTD, COUNT(RTCmty.COMQ1) AS Expr1, " _
           &"SUM(RTCmty.ASSESS) AS Expr2, SUM(RTSysParm.PVALUEN) AS Expr3, " _
           &"SUM(RTCmty.ASSESS) - SUM(RTSysParm.PVALUEN) AS Expr4, " _
           &"RTCmty.ACCOUNTCFM, RTObj.CUSNC " _
           &"FROM RTEmployee RTEmployee1 LEFT OUTER JOIN " _
           &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
           &"RTSysParm INNER JOIN " _
           &"RTCmty ON RTSysParm.PVALUEN < RTCmty.ASSESS AND  " _
           &"RTSysParm.PVALUEN < RTCmty.ASSESS ON  " _
           &"RTEmployee1.EMPLY = RTCmty.PAYPRTUSR LEFT OUTER JOIN " _
           &"RTObj RIGHT OUTER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
           &"RTCmty.ACCOUNTUSR = RTEmployee.EMPLY " _
           &"WHERE (RTCmty.PAYPRTSEQ = '*') "
  'response.write "sql=" &sqldelete
  dataTable="b"
  numberOfKey=1
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20  
 
  searchProg="RTCOTpaysearch.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" and rtcmty.accountcfm is null "
     searchShow="未審核"
  End If   
  sqlList="SELECT RTCmty.PAYPRTSEQ, RTObj1.CUSNC AS Expr5, RTCmty.PAYPRTD, COUNT(RTCmty.COMQ1) AS Expr1, " _
           &"SUM(RTCmty.ASSESS) AS Expr2, SUM(RTSysParm.PVALUEN) AS Expr3, " _
           &"SUM(RTCmty.ASSESS) - SUM(RTSysParm.PVALUEN) AS Expr4, " _
           &"RTCmty.ACCOUNTCFM, RTObj.CUSNC " _
           &"FROM RTEmployee RTEmployee1 LEFT OUTER JOIN " _
           &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
           &"RTSysParm INNER JOIN " _
           &"RTCmty ON RTSysParm.PVALUEN < RTCmty.ASSESS AND  " _
           &"RTSysParm.PVALUEN < RTCmty.ASSESS ON  " _
           &"RTEmployee1.EMPLY = RTCmty.PAYPRTUSR LEFT OUTER JOIN " _
           &"RTObj RIGHT OUTER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
           &"RTCmty.ACCOUNTUSR = RTEmployee.EMPLY " _
           &"WHERE (RTCmty.PAYPRTSEQ <> '') AND (RTCmty.ACCOUNTCFM IS NULL) AND " _
           &"(RTSysParm.PARMID = 'A0') AND (RTSysParm.EXPDAT IS NULL) AND " _
           &"(RTCmty.ACCOUNTCFM IS NULL) " & searchqry _
           &"GROUP BY RTCmty.PAYPRTSEQ, RTCmty.PAYPRTD, RTCmty.ACCOUNTCFM, " _
           &"RTSysParm.PVALUEN, RTObj.CUSNC, RTObj1.CUSNC " _
           &"ORDER BY RTCmty.PAYPRTSEQ DESC " 
 'Response.Write "SQL=" & SQllist
End Sub
%>