<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="中華399未作廢未完工用戶查詢"
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
  ' sqlDelete="SELECT rtcustadsl.COMQ1, rtcustadsl.CUSID, rtcustadsl.ENTRYNO, RTObj.SHORTNC, " _
  '       &"rtcustadsl.CUSTYPE, rtcustadsl.RCVD, rtcustadsl.HOME, " _
  '       &"rtcustadsl.OFFICE + ' ' + rtcustadsl.EXTENSION AS Office,  " _
  '       &"rtcustadsl.REQDAT,rtcustadsl.finishdat,rtcustadsl.DROPDAT, RTCode.CODENC " _
  '       &"FROM rtcustadsl INNER JOIN RTObj ON rtcustadsl.CUSID = RTObj.CUSID LEFT OUTER JOIN " _
  '       &"RTCounty ON rtcustadsl.CUTID1 = RTCounty.CUTID LEFT OUTER JOIN RTCode ON rtcustadsl.SETTYPE = RTCode.CODE " _
  '       &"WHERE rtcustadsl.COMQ1=0 AND (RTCode.KIND = 'A7') " _
  '       &"ORDER BY rtcustadsl.CUSID, rtcustadsl.ENTRYNO " 
  '===============================================================================
  formatName="none;none;none;社區序號;社區名稱;用戶名稱;裝機地址;完工日;報竣日;退租/作廢日;經銷商"
  sqlDelete= "SELECT rtcustadsl.comq1,rtcustadsl.cusid,rtcustadsl.entryno,rtcustadsl.comq1,rtcustadslcmty.comn,rtOBJ.cusnc,rtcounty.cutnc+rtcustadsl.township2+rtcustadsl.raddr2," _
           &"rtcustadsl.finishdat,rtcustadsl.docketdat,rtcustadsl.DROPdat,CASE WHEN rtcustadslcmty.COMTYPE IN ('01','02','03') " _
           &"THEN '元訊' else RTCODE.CODENC end " _
           &"FROM rtcustadsl inner join rtcustadslcmty on rtcustadsl.comq1=rtcustadslcmty.cutyid left outer join rtobj on rtcustadsl.cusid=rtobj.cusid " _
           &"left outer join rtcode on rtcustadslcmty.comtype=rtcode.code and rtcode.kind='B3' " _
           &"left outer join rtcounty on rtcustadsl.cutid2=rtcounty.cutid " _
           &"WHERE (rtcustadsl. DOCKETDAT IS NULL  )  and rtcustadslcmty.rcomdrop is null " _
           &"and dropdat is null AND rtcustadsl.COMQ1=0 " _
           &"ORDER BY rtcustadslcmty.comn,rtobj.cusnc " 
  dataTable="rtcustadsl"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=3
  dataProg="rtcustD.asp"
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
  searchProg="rtcustALARM3S.asp"
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" AND rtcustadslcmty.cutyid <>0 "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If  
 ' searchShow=FrGetCmtyDesc(aryParmKey(0))
  sqllist="SELECT  rtcustadsl.comq1,rtcustadsl.cusid,rtcustadsl.entryno,rtcustadsl.comq1,rtcustadslcmty.comn,rtOBJ.cusnc,rtcounty.cutnc+rtcustadsl.township2+rtcustadsl.raddr2," _
           &"rtcustadsl.finishdat,rtcustadsl.docketdat,rtcustadsl.DROPdat,CASE WHEN rtcustadslcmty.COMTYPE IN ('01','02','03') " _
           &"THEN '元訊' else RTCODE.CODENC end " _
           &"FROM rtcustadsl inner join rtcustadslcmty on rtcustadsl.comq1=rtcustadslcmty.cutyid left outer join rtobj on rtcustadsl.cusid=rtobj.cusid " _
           &"left outer join rtcode on rtcustadslcmty.comtype=rtcode.code and rtcode.kind='B3' " _
           &"left outer join rtcounty on rtcustadsl.cutid2=rtcounty.cutid " _
           &"WHERE rtcustadsl.DROPDAT IS NULL AND rtcustadsl.FINISHDAT IS NULL AND rtcustadslcmty.RCOMDROP IS NULL AND rtcustadsl.DOCKETDAT IS NULL AND rtcustadslcmty.adslapply IS NOT NULL  " & SEARCHQRY & " " _
           &"ORDER BY rtcustadslcmty.comn,rtobj.cusnc " 
  ' RESPONSE.Write SQLLIST
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->