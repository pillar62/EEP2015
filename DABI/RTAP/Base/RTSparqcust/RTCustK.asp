<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="速博ADSL客戶基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N" & ";N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="發包通知;撤銷通知;客訴處理;繳款記錄;維修收款;設備保管收據列印"
  functionOptProgram="RTSndInfo.asp;RTDropInfo.asp;RTFaqK.ASP;RTSPARQCUSTPAYK.ASP;/webap/rtap/base/rtsparqadslcmty/RTSparqAdslCustRepairK.asp;/RTAP/REPORT/Common/RTStorageReceiptSparq399.asp"
  functionOptPrompt ="Y;Y;N;N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;業務轄區;客戶名稱;none;對帳序號;社區名稱;申請日;送件日;完工日;報竣日;欠退日;欠拆;裝機地址;聯絡電話 "  
   sqlDelete="SELECT rtsparqadslcust.COMQ1, rtsparqadslcust.CUSID, rtsparqadslcust.ENTRYNO, RTObj.SHORTNC,rtsparqadslcust.orderno,rtsparqadslcust.exttel+'-'+rtsparqadslcust.sphnno,rtsparqadslcust.HOUSENAME, " _
         &"rtsparqadslcust.reqdat,rtsparqadslcust.DELIVERDAT, " _
         &"rtsparqadslcust.FINISHDAT, rtsparqadslcust.DOCKETDAT, rtsparqadslcust.DROPDAT, rtsparqadslcust.overdue," _
         &"RTCOUNTY.CUTNC + rtsparqadslcust.TOWNSHIP2 + rtsparqadslcust.RADDR2, " _         
         &"rtsparqadslcust.HOME " _
         &"FROM rtsparqadslcust INNER JOIN " _
         &"RTObj ON rtsparqadslcust.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON rtsparqadslcust.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON rtsparqadslcust.ISP = RTCode1.CODE AND  " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode ON rtsparqadslcust.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7' " _
         &"WHERE rtsparqadslcust.cusid='*' " _
         &"ORDER BY RTCOUNTY.CUTNC, rtsparqadslcust.TOWNSHIP2, rtsparqadslcust.RADDR2,rtobj.shortnc "
  dataTable="rtsparqadslcust"
  userDefineDelete=""
  extTable=""
  numberOfKey=3
  dataProg="..\RTSparqAdslCmty\RTCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=400,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage=""
  colSplit=1
  keyListPageSize=25
  searchProg="RTCustS.asp"
  searchFirst=true
  If searchQry="" Then
     searchShow="全部"
     searchQry="rtsparqadslcust.CUSID ='*' "
  ELSE
     SEARCHFIRST=FALSE
  End If

  'userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select * from RTAreaSales where cusid='" & Emply & "' and areaid ='D0' "
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     limitemply	=" and RTSparqAdslCmty.bussid ='" & Emply & "' "
  else
     limitemply =" " 
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '-------------------------------------------------------------------------------------------
  
  sqllist="SELECT rtsparqadslcust.COMQ1, rtsparqadslcust.CUSID, rtsparqadslcust.ENTRYNO, " _
         &"isnull(RTObj_a.shortnc, isnull(rtobj_b.cusnc,'')), " _
         &"RTObj.SHORTNC,rtsparqadslcust.orderno,rtsparqadslcust.exttel+'-'+rtsparqadslcust.sphnno,RTSparqADSLcmty.comn, " _
         &"rtsparqadslcust.reqdat,rtsparqadslcust.DELIVERDAT, " _
         &"rtsparqadslcust.FINISHDAT, rtsparqadslcust.DOCKETDAT, rtsparqadslcust.DROPDAT, rtsparqadslcust.overdue," _
         &"RTCOUNTY.CUTNC + rtsparqadslcust.TOWNSHIP2 + rtsparqadslcust.RADDR2, " _         
         &"rtsparqadslcust.HOME " _
         &"FROM RTSparqADSLcust INNER JOIN " _
         &"RTObj ON RTSparqADSLcust.CUSID = RTObj.CUSID inner JOIN " _
         &"RTSparqADSLcmty ON " _
         &"RTSparqADSLcust.COMQ1 = RTSparqADSLcmty.CUTYID LEFT OUTER JOIN " _
         &"RTCounty ON RTSparqADSLcust.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON RTSparqADSLcust.ISP = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode RTCode_1 ON RTSparqADSLcust.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7' " _ 
         &"left outer join rtctytown on RTSparqAdslCust.cutid2=rtctytown.cutid and RTSparqAdslCust.township2=rtctytown.township " _
         &"LEFT OUTER JOIN RTObj rtobj_a ON RTSparqAdslCmty.CONSIGNEE = RTObj_a.CUSID " _
		 &"LEFT OUTER JOIN RTEmployee inner join RTObj rtobj_b ON rtobj_b.cusid=RTEmployee.cusid on RTEmployee.emply = RTSparqAdslCmty.bussid " _
         &"where " & searchqry & " " & limitemply _         
         &"ORDER BY RTCOUNTY.CUTNC, rtsparqadslcust.TOWNSHIP2, rtsparqadslcust.RADDR2,rtobj.shortnc "

  'Response.Write "sql=" & SQLLIST
End Sub

Sub SrRunUserDefineDelete()

End Sub
%>
