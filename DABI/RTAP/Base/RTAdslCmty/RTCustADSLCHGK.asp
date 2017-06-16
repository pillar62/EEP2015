<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="ADSL已報竣客戶歷史異動查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="異動作廢"
  functionOptProgram="RTcustadslchGdrop.asp"
  functionOptPrompt ="Y"
  functionoptopen   ="1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;none;客戶名稱;單次;異動代碼;異動名稱;異動日期;異動人員;作廢日期;報竣日期;轉檔日期;裝機地址;聯絡電話"
   sqlDelete="SELECT RTCUSTADSLCHG.COMQ1,rtcustadslchg.cusid,rtcustadslchg.entryno,rtcustadslchg.modifycode,rtcustadslchg.modifydat, RTObj_1.SHORTNC, RTCUSTADSLCHG.ENTRYNO, " _
            &"RTCUSTADSLCHG.MODIFYCODE, RTCode.CODENC, " _
            &"RTCUSTADSLCHG.MODIFYDAT, RTObj_2.CUSNC, RTCUSTADSLCHG.DROPDAT, " _
            &"RTCUSTADSLCHG.DOCKETDAT, RTCUSTADSLCHG.TRANSDAT " _
            &"FROM  RTObj RTObj_1 RIGHT OUTER JOIN " _
            &"RTCUSTADSLCHG ON " _
            &"RTObj_1.CUSID = RTCUSTADSLCHG.CUSID LEFT OUTER JOIN " _
            &"RTObj RTObj_2 RIGHT OUTER JOIN " _
            &"RTEmployee ON RTObj_2.CUSID = RTEmployee.CUSID ON " _
            &"RTCUSTADSLCHG.MODIFYUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
            &"RTCode ON RTCUSTADSLCHG.MODIFYCODE = RTCode.CODE AND " _
            &"RTCode.KIND = 'C8' " _
            &"where rtcustadslchg,cusid='*' " _
            &"ORDER BY  RTCUSTADSLCHG.MODIFYDAT " 
  dataTable="RTCUSTADSLCHG"
  userDefineDelete=""
  extTable=""
  numberOfKey=5
  dataProg="RTCustADSLCHGD.asp"
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
  sqllist="SELECT RTCUSTADSLCHG.COMQ1,rtcustadslchg.cusid,rtcustadslchg.entryno,rtcustadslchg.modifycode,rtcustadslchg.modifydat, RTObj_1.SHORTNC, RTCUSTADSLCHG.ENTRYNO, " _
            &"RTCUSTADSLCHG.MODIFYCODE, RTCode.CODENC, " _
            &"RTCUSTADSLCHG.MODIFYDAT, RTObj_2.CUSNC, RTCUSTADSLCHG.DROPDAT, " _
            &"RTCUSTADSLCHG.DOCKETDAT, RTCUSTADSLCHG.TRANSDAT " _
            &"FROM  RTObj RTObj_1 RIGHT OUTER JOIN " _
            &"RTCUSTADSLCHG ON " _
            &"RTObj_1.CUSID = RTCUSTADSLCHG.CUSID LEFT OUTER JOIN " _
            &"RTObj RTObj_2 RIGHT OUTER JOIN " _
            &"RTEmployee ON RTObj_2.CUSID = RTEmployee.CUSID ON " _
            &"RTCUSTADSLCHG.MODIFYUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
            &"RTCode ON RTCUSTADSLCHG.MODIFYCODE = RTCode.CODE AND " _
            &"RTCode.KIND = 'C8' " _
            &"where rtcustadslchg.comq1=" & aryparmkey(0) & "and  rtcustadslchg.cusid='" & aryparmkey(1) & "' and rtcustadslchg.entryno=" & aryparmkey(2) & " " _
            &"ORDER BY  RTCUSTADSLCHG.MODIFYDAT " 
   'response.write "SQL=" & sqllist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
