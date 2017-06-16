<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building管理系統"
  title="中華ADSL399退租戶拆機派工作業"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="拆機派工"
  functionOptProgram="RTcustADSLdropsndworkk.asp"
  functionOptPrompt ="N"
  functionoptopen   ="1"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;用戶名稱;報竣日;退租日;拆機單號;派拆機日;預定拆機人員;經銷商"
   sqlDelete="SELECT RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTCustAdslCmty.COMN,RTObj.CUSNC, RTCustADSL.DOCKETDAT, " _
         &"RTCustADSL.DROPDAT, RTCUSTADSLDROPSNDWORK.PRTNO, RTCUSTADSLDROPSNDWORK.SENDWORKDAT, " _
         &"CASE WHEN RTOBJ_1.CUSNC <> '' THEN RTOBJ_1.CUSNC ELSE RTOBJ_2.SHORTNC  END, " _
         &"CASE WHEN RTCODE.CODE IN ('01', '02', '03', '04', '14') THEN CASE WHEN RTCUSTADSL.CUTID2 IN " _
         &"('01', '02', '03', '04', '21', '22') AND RTCUSTADSL.township2 NOT IN ('三峽鎮', '鶯歌鎮') " _
         &"THEN '台北' WHEN RTCUSTADSL.CUTID2 IN ('05', '06', '07', '08') OR " _
         &"(RTCUSTADSL.cutid2 = '03' AND RTCUSTADSL.township2 IN ('三峽鎮', '鶯歌鎮')) " _
         &"THEN '桃園' WHEN RTCUSTADSL.CUTID2 IN ('09', '10', '11', '12', '13') " _
         &"THEN '台中' WHEN RTCUSTADSL.CUTID2 IN ('14', '15', '16', '17', '18', '19', '20') " _
         &"THEN '高雄' ELSE '' END ELSE RTCODE.CODENC END " _
         &"FROM RTObj RTObj_2 RIGHT OUTER JOIN RTCUSTADSLDROPSNDWORK ON RTObj_2.CUSID = RTCUSTADSLDROPSNDWORK.ASSIGNCONSIGNEE " _
         &"LEFT OUTER JOIN RTEmployee LEFT OUTER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
         &"RTCUSTADSLDROPSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY RIGHT OUTER JOIN RTCustADSL INNER JOIN RTCustAdslCmty ON " _
         &"RTCustADSL.COMQ1 = RTCustAdslCmty.CUTYID INNER JOIN RTObj ON RTCustADSL.CUSID = RTObj.CUSID ON " _
         &"RTCUSTADSLDROPSNDWORK.CUSID = RTCustADSL.CUSID AND RTCUSTADSLDROPSNDWORK.ENTRYNO = RTCustADSL.ENTRYNO LEFT OUTER JOIN " _
         &"RTCODE ON RTCUSTADSLCMTY.COMTYPE = RTCODE.CODE AND RTCODE.KIND = 'B3' " _
         &"WHERE (RTCustADSL.DROPDAT IS NOT NULL) AND (RTCustADSL.DOCKETDAT IS NOT NULL) AND (RTCustAdslCmty.RCOMDROP IS NULL) " _
         &"AND RTCUSTADSLDROPSNDWORK.CLOSEDAT IS NULL ORDER BY  RTCustAdslCmty.COMN, RTOBJ.CUSNC " 
  dataTable="RTCUSTADSL"
  userDefineDelete=""
  extTable=""
  numberOfKey=2
  dataProg="None"
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
  keyListPageSize=25
  searchFirst=false
 ' response.Write "k0=" & aryparmkey(0) & ";k1=" & aryparmkey(1) & ";k2=" & aryparmkey(2)
  searchShow="全部待拆機派工"
  'searchQry=""
  searchProg="RTCustDropAllS.asp"
  If searchQry="" Then
     searchQry=" AND RTCUSTADSLDROPSNDWORK.closedat is null "
     searchShow="全部(尚未拆機回報)"
  ELSE
     SEARCHFIRST=FALSE
  End If
  
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqllist="SELECT RTCustADSL.CUSID, RTCustADSL.ENTRYNO, RTCustAdslCmty.COMN,RTObj.CUSNC, RTCustADSL.DOCKETDAT, " _
         &"RTCustADSL.DROPDAT, RTCUSTADSLDROPSNDWORK.PRTNO, RTCUSTADSLDROPSNDWORK.SENDWORKDAT, " _
         &"CASE WHEN RTOBJ_1.CUSNC <> '' THEN RTOBJ_1.CUSNC ELSE RTOBJ_2.SHORTNC  END, " _
         &"CASE WHEN RTCODE.CODE IN ('01', '02', '03', '04', '14') THEN CASE WHEN RTCUSTADSL.CUTID2 IN " _
         &"('01', '02', '03', '04', '21', '22') AND RTCUSTADSL.township2 NOT IN ('三峽鎮', '鶯歌鎮') " _
         &"THEN '台北' WHEN RTCUSTADSL.CUTID2 IN ('05', '06', '07', '08') OR " _
         &"(RTCUSTADSL.cutid2 = '03' AND RTCUSTADSL.township2 IN ('三峽鎮', '鶯歌鎮')) " _
         &"THEN '桃園' WHEN RTCUSTADSL.CUTID2 IN ('09', '10', '11', '12', '13') " _
         &"THEN '台中' WHEN RTCUSTADSL.CUTID2 IN ('14', '15', '16', '17', '18', '19', '20') " _
         &"THEN '高雄' ELSE '' END ELSE RTCODE.CODENC END " _
         &"FROM RTObj RTObj_2 RIGHT OUTER JOIN RTCUSTADSLDROPSNDWORK ON RTObj_2.CUSID = RTCUSTADSLDROPSNDWORK.ASSIGNCONSIGNEE " _
         &"LEFT OUTER JOIN RTEmployee LEFT OUTER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
         &"RTCUSTADSLDROPSNDWORK.ASSIGNENGINEER = RTEmployee.EMPLY RIGHT OUTER JOIN RTCustADSL INNER JOIN RTCustAdslCmty ON " _
         &"RTCustADSL.COMQ1 = RTCustAdslCmty.CUTYID INNER JOIN RTObj ON RTCustADSL.CUSID = RTObj.CUSID ON " _
         &"RTCUSTADSLDROPSNDWORK.CUSID = RTCustADSL.CUSID AND RTCUSTADSLDROPSNDWORK.ENTRYNO = RTCustADSL.ENTRYNO LEFT OUTER JOIN " _
         &"RTCODE ON RTCUSTADSLCMTY.COMTYPE = RTCODE.CODE AND RTCODE.KIND = 'B3' " _
         &"WHERE (RTCustADSL.DROPDAT IS NOT NULL) AND (RTCustADSL.DOCKETDAT IS NOT NULL) AND (RTCustAdslCmty.RCOMDROP IS NULL) " & searchqry _
         &" ORDER BY  RTCustAdslCmty.COMN, RTOBJ.CUSNC " 
  ' response.write "SQL=" & sqllist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
