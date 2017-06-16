<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL客戶基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="發包通知;撤銷通知;客訴處理;剔除客戶;加入客戶;報竣異動;歷史異動;CALL-OUT"
  functionOptProgram="RTSndInfo.asp;RTDropInfo.asp;RTFaqK.ASP;RTDisconnect.asp;RTJOINCUSTk.ASP;RTCUSTADSLCHGOPT.ASP;RTcustadslchgk.asp;/WEBAP/RTAP/BASE/HBCALLOUTPROJECT/RTCUSTOPTK.ASP"
  functionOptPrompt ="Y;Y;N;Y;H;N;N;N"
  functionoptopen   ="1;1;1;1;1;1;1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;客戶名稱;HN號碼;申請;送件;完工;報竣;撤銷日;裝機地址;聯絡電話;券商分行;營業員"
   sqlDelete="SELECT rtcustadsl.COMQ1,RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTObj_1.SHORTNC,rtcustadsl.CUSNO, " _
         &"RTCustADSL.RCVD,RTCUSTADSL.DELIVERDAT, " _
         &"rtcustadsl.finishdat,rtcustadsl.docketdat, " _
         &"rtcustadsl.dropdat, " _
         &"RTCOUNTY.CUTNC + RTCUSTADSL.TOWNSHIP2 + RTCUSTADSL.RADDR2, " _         
         &"RTCustADSL.HOME,RTObj_2.SHORTNC + rtcustadsl.branch, rtobj_3.shortnc  " _ 
         &"FROM RTCustADSL LEFT OUTER JOIN " _
         &"RTObj RTObj_3 ON RTCustADSL.BUSSMAN = RTObj_3.CUSID LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON RTCustADSL.CUSID = RTObj_1.CUSID LEFT OUTER JOIN " _
         &"RTObj RTObj_2 ON RTCustADSL.STOCKID = RTObj_2.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON RTCustADSL.ISP = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode RTCode_1 ON RTCustADSL.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7'" _
         &"WHERE rtcustadsl.cusid='*' " _
         &"ORDER BY RTCOUNTY.CUTNC, RTCUSTADSL.TOWNSHIP2, RTCUSTADSL.RADDR2,rtobj_1.shortnc "
      '   &"case when RTCUSTADSL.WORKINGREPLY IS NOT NULL and DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) < '2001/08/20' then '2001/08/20' " _
      '   &"WHEN RTCUSTADSL.WORKINGREPLY IS NOT NULL THEN DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) " _
      '   &"when RTCUSTADSL.CHTSIGNDAT IS NOT NULL and DateADD(dd, 14, RTCUSTADSL.chtsigndat) < '2001/08/20' then '2001/08/20' " _
      '   &"WHEN RTCUSTADSL.CHTSIGNDAT IS NOT NULL THEN DateADD(dd, 14, RTCUSTADSL.chtsigndat) " _
      '   &"when rtcustADSL.DELIVERDAT IS NOT NULL and DATEADD(mm, 1,rtcustadsl.deliverdat) < '2001/08/20' then '2001/08/20' " _
      '   &"when rtcustADSL.DELIVERDAT IS NOT NULL THEN DATEADD(mm, 1,rtcustadsl.deliverdat)  end ," _         
  dataTable="RTCUSTADSL"
  userDefineDelete=""
  extTable=""
  numberOfKey=3
  dataProg="RTCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage=""
  colSplit=1
  keyListPageSize=20
  searchFirst=false
  searchShow=FrGetCmtyDesc(aryParmKey(0))
  searchQry="RTCUSTADSL.comq1 =" & aryparmkey(0)
  searchProg="self"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'USERLEVEL=2業務人員(只能看到所屬業務組別資料)
 ' IF USERLEVEL = 2 THEN
 ' sqllist="SELECT  rtcustadsl.comq1,RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTObj_1.SHORTNC,rtcustadsl.orderno, " _
 '        &"RTCustADSL.RCVD,RTCUSTADSL.DELIVERDAT, " _
 '        &"case when RTCUSTADSL.WORKINGREPLY IS NOT NULL and DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) < '2001/08/20' then '2001/08/20' " _
 '        &"WHEN RTCUSTADSL.WORKINGREPLY IS NOT NULL THEN DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) " _
 '        &"when RTCUSTADSL.CHTSIGNDAT IS NOT NULL and DateADD(dd, 14, RTCUSTADSL.chtsigndat) < '2001/08/20' then '2001/08/20' " _
 '        &"WHEN RTCUSTADSL.CHTSIGNDAT IS NOT NULL THEN DateADD(dd, 14, RTCUSTADSL.chtsigndat) " _
 '        &"when rtcustADSL.DELIVERDAT IS NOT NULL and DATEADD(mm, 1,rtcustadsl.deliverdat) < '2001/08/20' then '2001/08/20' " _
 '        &"when rtcustADSL.DELIVERDAT IS NOT NULL THEN DATEADD(mm, 1,rtcustadsl.deliverdat)  end ," _
 '        &"rtcustadsl.finishdat," _
 '        &"RTCOUNTY.CUTNC + RTCUSTADSL.TOWNSHIP2 + RTCUSTADSL.RADDR2, " _         
 '        &"RTCustADSL.HOME,RTObj_2.SHORTNC + rtcustadsl.branch, rtobj_3.shortnc  " _ 
 '        &"FROM RTCustADSL LEFT OUTER JOIN " _
 '        &"RTObj RTObj_3 ON RTCustADSL.BUSSMAN = RTObj_3.CUSID LEFT OUTER JOIN " _
 '        &"RTObj RTObj_1 ON RTCustADSL.CUSID = RTObj_1.CUSID LEFT OUTER JOIN " _
 '        &"RTObj RTObj_2 ON RTCustADSL.STOCKID = RTObj_2.CUSID LEFT OUTER JOIN " _
 '        &"RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
 '        &"RTCode RTCode1 ON RTCustADSL.ISP = RTCode1.CODE AND " _
 '        &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
 '        &"RTCode RTCode_1 ON RTCustADSL.SETTYPE = RTCode1.CODE AND " _
 '        &"RTCode1.KIND = 'A7'" _
 '        &"WHERE " & searchqry & " " & " AND " _
 '        &"(RTSalesGroupREF.AREAID + RTSalesGroupREF.GROUPID = " _
 '        &"(SELECT areaid + groupid " _
 '        &"FROM RTSalesGroupREF " _
 '        &"WHERE emply = '" &emply & "')) " _
 '        &"ORDER BY RTCounty.CUTNC, RTCustADSL.TOWNSHIP2, RTCustADSL.RADDR2, " _
 '        &"RTObj_1.SHORTNC "

 ' ELSE
  sqllist="SELECT  rtcustadsl.comq1,RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTObj_1.SHORTNC,rtcustadsl.CUSNO, " _
         &"RTCustADSL.RCVD,RTCUSTADSL.DELIVERDAT, " _
         &"rtcustadsl.finishdat,rtcustadsl.docketdat," _
         &"rtcustadsl.dropdat, " _         
         &"RTCOUNTY.CUTNC + RTCUSTADSL.TOWNSHIP2 + RTCUSTADSL.RADDR2, " _         
         &"RTCustADSL.HOME,RTObj_2.SHORTNC + rtcustadsl.branch, rtobj_3.shortnc  " _ 
         &"FROM RTCustADSL LEFT OUTER JOIN " _
         &"RTObj RTObj_3 ON RTCustADSL.BUSSMAN = RTObj_3.CUSID LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON RTCustADSL.CUSID = RTObj_1.CUSID LEFT OUTER JOIN " _
         &"RTObj RTObj_2 ON RTCustADSL.STOCKID = RTObj_2.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON RTCustADSL.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode RTCode1 ON RTCustADSL.ISP = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'C3' LEFT OUTER JOIN " _
         &"RTCode RTCode_1 ON RTCustADSL.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7'" _
         &"WHERE " & searchqry & " " _
         &"ORDER BY RTCOUNTY.CUTNC, RTCUSTADSL.TOWNSHIP2, RTCUSTADSL.RADDR2,rtobj_1.shortnc "
      '   &"case when RTCUSTADSL.WORKINGREPLY IS NOT NULL and DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) < '2001/08/20' then '2001/08/20' " _
      '   &"WHEN RTCUSTADSL.WORKINGREPLY IS NOT NULL THEN DateADD(dd, 7, RTCUSTADSL.WORKINGREPLY) " _
      '   &"when RTCUSTADSL.CHTSIGNDAT IS NOT NULL and DateADD(dd, 14, RTCUSTADSL.chtsigndat) < '2001/08/20' then '2001/08/20' " _
      '   &"WHEN RTCUSTADSL.CHTSIGNDAT IS NOT NULL THEN DateADD(dd, 14, RTCUSTADSL.chtsigndat) " _
      '   &"when rtcustADSL.DELIVERDAT IS NOT NULL and DATEADD(mm, 1,rtcustadsl.deliverdat) < '2001/08/20' then '2001/08/20' " _
      '   &"when rtcustADSL.DELIVERDAT IS NOT NULL THEN DATEADD(mm, 1,rtcustadsl.deliverdat)  end ," _
         
 '  END IF
 ' Response.Write "sql=" & SQLLIST
  SESSION("COMQ1")=ARYPARMKEY(0)
  SESSION("COMQ1XX")=ARYPARMKEY(0)
  SESSION("COMTYPEXX")="2"
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=RTLIB"
  sqlxx="select COMN from rtcustadslcmty where cutyid=" & session("COMQ1")
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     session("COMN")=rsxx("COMN")
  else
     SESSION("COMN")=""
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing  
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
