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
  title="COT建置自付額審核撤銷"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="社區明細"
  functionOptPrompt="N"
  functionOptProgram="RTcotpayCANk.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  
  DSN="DSN=RTLIb"
  formatName="付款表批號;列印人員;列印日期;社區筆數;估價總額;標準建置額;自付總額;會計審核日;會計審核員"
  sqlDelete="SELECT RTCmty.PAYPRTSEQ, RTObj1.CUSNC, RTCmty.PAYPRTD,  " _
           &"COUNT(RTCmty.COMQ1), SUM(RTCmty.ASSESS), SUM(RTSysParm.PVALUEN), " _
           &"SUM(RTCmty.ASSESS) - SUM(RTSysParm.PVALUEN), RTCmty.ACCOUNTCFM, " _
           &"RTObj1.CUSNC " _
           &"FROM RTEmployee RTEmployee1 LEFT OUTER JOIN " _
           &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
           &"RTEmployee LEFT OUTER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID RIGHT OUTER JOIN " _
           &"RTCmty ON RTEmployee.EMPLY = RTCmty.ACCOUNTUSR ON  " _
           &"RTEmployee1.EMPLY = RTCmty.PAYPRTUSR, RTSysParm " _
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
     searchQry=" and rtcmty.accountcfm is NOT null "
     searchShow="未審核"
  End If   
  sqlList="SELECT RTCmty.PAYPRTSEQ, RTObj1.CUSNC, RTCmty.PAYPRTD,  " _
           &"COUNT(RTCmty.COMQ1), SUM(RTCmty.ASSESS), SUM(RTSysParm.PVALUEN), " _
           &"SUM(RTCmty.ASSESS) - SUM(RTSysParm.PVALUEN), RTCmty.ACCOUNTCFM, " _
           &"RTObj1.CUSNC " _
           &"FROM RTEmployee RTEmployee1 LEFT OUTER JOIN " _
           &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
           &"RTEmployee LEFT OUTER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID RIGHT OUTER JOIN " _
           &"RTCmty ON RTEmployee.EMPLY = RTCmty.ACCOUNTUSR ON  " _
           &"RTEmployee1.EMPLY = RTCmty.PAYPRTUSR, RTSysParm " _
           &"WHERE (RTCmty.PAYPRTSEQ <> '') AND (RTCmty.ACCOUNTCFM IS NOT NULL) AND " _
           &"RTSYSPARM.PARMID = 'A0' AND RTSYSPARM.EXPDAT IS NULL AND " _
           &"RTCMTY.ASSESS > RTSYSPARM.PVALUEN "  & searchqry _
           &"GROUP BY RTCmty.PAYPRTSEQ, RTObj1.CUSNC, RTCmty.PAYPRTD, " _
           &"RTCmty.ACCOUNTCFM, RTObj1.CUSNC, RTSysParm.PVALUEN " _
           &"ORDER BY RTCmty.PAYPRTSEQ DESC " 
' Response.Write "SQL=" & SQllist
End Sub
%>