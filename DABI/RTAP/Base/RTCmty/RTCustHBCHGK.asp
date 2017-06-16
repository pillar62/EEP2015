<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="Hi-Building已報竣客戶歷史異動查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="異動作廢"
  functionOptProgram="RTcustchGdrop.asp"
  functionOptPrompt ="Y"
  functionoptopen   ="1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;none;客戶名稱;單次;異動代碼;異動名稱;異動日期;異動人員;作廢日期;報竣日期;轉檔日期;裝機地址;聯絡電話"
   sqlDelete="SELECT RTCUSThbCHG.COMQ1,rtcusthbchg.cusid,rtcusthbchg.entryno,rtcusthbchg.modifycode,rtcusthbchg.modifydat, RTObj_1.SHORTNC, rtcusthbCHG.ENTRYNO, " _
            &"rtcusthbCHG.MODIFYCODE, RTCode.CODENC, " _
            &"rtcusthbCHG.MODIFYDAT, RTObj_2.CUSNC, rtcusthbCHG.DROPDAT, " _
            &"rtcusthbCHG.DOCKETDAT, rtcusthbCHG.TRANSDAT " _
            &"FROM  RTObj RTObj_1 RIGHT OUTER JOIN " _
            &"rtcusthbCHG ON " _
            &"RTObj_1.CUSID = rtcusthbCHG.CUSID LEFT OUTER JOIN " _
            &"RTObj RTObj_2 RIGHT OUTER JOIN " _
            &"RTEmployee ON RTObj_2.CUSID = RTEmployee.CUSID ON " _
            &"rtcusthbCHG.MODIFYUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
            &"RTCode ON rtcusthbCHG.MODIFYCODE = RTCode.CODE AND " _
            &"RTCode.KIND = 'C8' " _
            &"where rtcusthbchg,cusid='*' " _
            &"ORDER BY  rtcusthbCHG.MODIFYDAT " 
  dataTable="rtcusthbCHG"
  userDefineDelete=""
  extTable=""
  numberOfKey=5
  dataProg="rtcusthbCHGD.asp"
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
  searchQry="rtcusthb.comq1 =" & aryparmkey(0)
  searchProg="self"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqllist="SELECT rtcusthbCHG.COMQ1,rtcusthbchg.cusid,rtcusthbchg.entryno,rtcusthbchg.modifycode,rtcusthbchg.modifydat, RTObj_1.SHORTNC, rtcusthbCHG.ENTRYNO, " _
            &"rtcusthbCHG.MODIFYCODE, RTCode.CODENC, " _
            &"rtcusthbCHG.MODIFYDAT, RTObj_2.CUSNC, rtcusthbCHG.DROPDAT, " _
            &"rtcusthbCHG.DOCKETDAT, rtcusthbCHG.TRANSDAT " _
            &"FROM  RTObj RTObj_1 RIGHT OUTER JOIN " _
            &"rtcusthbCHG ON " _
            &"RTObj_1.CUSID = rtcusthbCHG.CUSID LEFT OUTER JOIN " _
            &"RTObj RTObj_2 RIGHT OUTER JOIN " _
            &"RTEmployee ON RTObj_2.CUSID = RTEmployee.CUSID ON " _
            &"rtcusthbCHG.MODIFYUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
            &"RTCode ON rtcusthbCHG.MODIFYCODE = RTCode.CODE AND " _
            &"RTCode.KIND = 'C8' " _
            &"where rtcusthbchg.comq1=" & aryparmkey(0) & "and  rtcusthbchg.cusid='" & aryparmkey(1) & "' and rtcusthbchg.entryno=" & aryparmkey(2) & " " _
            &"ORDER BY  rtcusthbCHG.MODIFYDAT " 
   'response.write "SQL=" & sqllist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
