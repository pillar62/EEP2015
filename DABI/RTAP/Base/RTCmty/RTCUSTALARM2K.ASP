<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="未作廢未完工用戶查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=" 作  廢 ;作廢返轉"
  functionOptProgram="RTCUSTDROP.ASP;RTCUSTDROPC.ASP"
  functionOptPrompt ="Y;Y"
  functionoptopen   ="1;1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  '====================== history until 90/12/28=================================
  'formatName="none;客戶代號;單次;名稱;開發種類;申請日;聯絡電話;公司電話;發包日;完工日;撤銷日期;安裝員類別"
  ' sqlDelete="SELECT RTCust.COMQ1, RTCust.CUSID, RTCust.ENTRYNO, RTObj.SHORTNC, " _
  '       &"RTCust.CUSTYPE, RTCust.RCVD, RTCust.HOME, " _
  '       &"RTCust.OFFICE + ' ' + RTCust.EXTENSION AS Office,  " _
  '       &"RTCust.REQDAT,rtcust.finishdat,RTCUST.DROPDAT, RTCode.CODENC " _
  '       &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
  '       &"RTCounty ON RTCust.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN RTCode ON RTCust.SETTYPE = RTCode.CODE " _
  '       &"WHERE RTCust.COMQ1=0 AND (RTCode.KIND = 'A7') " _
  '       &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO " 
  '===============================================================================
  formatName="none;none;none;社區序號;社區名稱;用戶名稱;裝機地址;用戶IP;IP申請日;完工日;報竣日;退租/作廢日;經銷商"
  sqlDelete= "SELECT rtcust.comq1,rtcust.cusid,rtcust.entryno,rtcust.comq1,rtcMTY.comn,rtOBJ.cusnc,rtcounty.cutnc+rtcust.township1+rtcust.raddr1,rtcust.ip," _
           &"rtcust.applyipdat,rtcust.finishdat,rtcust.docketdat,rtcust.DROPdat,CASE WHEN RTCMTY.COMTYPE IN ('01','02','03') " _
           &"THEN '元訊' else RTCODE.CODENC end " _
           &"FROM rtcust inner join rtcmty on rtcust.comq1=rtcmty.comq1 left outer join rtobj on rtcust.cusid=rtobj.cusid " _
           &"left outer join rtcode on rtcmty.comtype=rtcode.code and rtcode.kind='B3' " _
           &"left outer join rtcounty on rtcust.cutid1=rtcounty.cutid " _
           &"WHERE (rtcust. DOCKETDAT IS NULL or rtcust. docketdat <finishdat ) AND ip<> '' and rtcmty.rcomdrop is null " _
           &"and dropdat is null and substring(ip,1,1) >='0' and  substring(ip,1,1) <='9' AND RTCUST.COMQ1=0 " _
           &"ORDER BY rtcmty.comn,rtobj.cusnc " 
  dataTable="RTCust"
  userDefineDelete="Yes"
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
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=25
  searchProg="rtcustALARM2S.asp"
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" AND RTcmty.comq1 <>0 "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If  
 ' searchShow=FrGetCmtyDesc(aryParmKey(0))
  sqllist="SELECT  rtcust.comq1,rtcust.cusid,rtcust.entryno,rtcust.comq1,rtcMTY.comn,rtOBJ.cusnc,rtcounty.cutnc+rtcust.township1+rtcust.raddr1,rtcust.ip," _
           &"rtcust.applyipdat,rtcust.finishdat,rtcust.docketdat,rtcust.DROPdat,CASE WHEN RTCMTY.COMTYPE IN ('01','02','03') " _
           &"THEN '元訊' else RTCODE.CODENC end " _
           &"FROM rtcust inner join rtcmty on rtcust.comq1=rtcmty.comq1 left outer join rtobj on rtcust.cusid=rtobj.cusid " _
           &"left outer join rtcode on rtcmty.comtype=rtcode.code and rtcode.kind='B3' " _
           &"left outer join rtcounty on rtcust.cutid1=rtcounty.cutid " _
           &"WHERE rtcust.DROPDAT IS NULL AND RTCUST.FINISHDAT IS NULL AND RTCMTY.RCOMDROP IS NULL AND RTCUST.DOCKETDAT IS NULL AND RTCUST.APPLYIPDAT IS NULL AND RTCMTY.T1APPLY IS NOT NULL  " & SEARCHQRY & " " _
           &"ORDER BY rtcmty.comn,rtobj.cusnc " 
  ' RESPONSE.Write SQLLIST
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->