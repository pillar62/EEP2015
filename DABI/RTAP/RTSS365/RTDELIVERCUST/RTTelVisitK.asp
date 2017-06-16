<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="先看先贏客戶電話訪談記錄"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  functionOptOpen="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;客戶名稱;單次;電訪日;帳號收件;電訪人員;滿意度;作廢日期"
sqlDelete="SELECT RTSS365tel.CUSID, RTSS365tel.ENTRYNO, RTSS365tel.SEQ1, RTObj.SHORTNC, RTSS365tel.ENTRYNO, " _
         &"RTSS365tel.TELVISITDAT,RTSS365tel.ACCOUNTRCV , rtobj_1.cusnc, " _
         &"case when RTSS365tel.CONTENTSCORE='1' then '非常滿意' when RTSS365tel.CONTENTSCORE='2' then '滿意' " _
         &" when RTSS365tel.CONTENTSCORE='3' then '可接受' when RTSS365tel.CONTENTSCORE='4' then '不滿意' " _
         &" when RTSS365tel.CONTENTSCORE='5' then '非常不滿意' end, " _
         &"RTSS365tel.DROPDAT " _
         &"FROM RTObj RTObj_1 RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj_1.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
         &"RTSS365tel ON RTEmployee.EMPLY = RTSS365tel.VISITMAN LEFT OUTER JOIN " _
         &"RTObj ON RTSS365tel.CUSID = RTObj.CUSID " _
         &"where rtss365tel.cusid='*'"
  dataTable="RTss365Tel"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=3
  dataProg="RTTelVisitD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=480,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  searchFirst=False
  If searchQry="" Then
     searchQry=" RTSS365Tel.CUSID='" & aryparmkey(0) & "' and rtss365tel.entryno=" & aryparmkey(1) 
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  sqllist="SELECT RTSS365tel.CUSID, RTSS365tel.ENTRYNO, RTSS365tel.SEQ1, RTObj.SHORTNC, RTSS365tel.ENTRYNO, " _
         &"RTSS365tel.TELVISITDAT,RTSS365tel.ACCOUNTRCV , rtobj_1.cusnc, " _
         &"case when RTSS365tel.CONTENTSCORE='1' then '非常滿意' when RTSS365tel.CONTENTSCORE='2' then '滿意' " _
         &" when RTSS365tel.CONTENTSCORE='3' then '可接受' when RTSS365tel.CONTENTSCORE='4' then '不滿意' " _
         &" when RTSS365tel.CONTENTSCORE='5' then '非常不滿意' end, " _
         &"RTSS365tel.DROPDAT " _
         &"FROM RTObj RTObj_1 RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj_1.CUSID = RTEmployee.CUSID RIGHT OUTER JOIN " _
         &"RTSS365tel ON RTEmployee.EMPLY = RTSS365tel.VISITMAN LEFT OUTER JOIN " _
         &"RTObj ON RTSS365tel.CUSID = RTObj.CUSID " _
         &"where " & searchQry 
 'Response.Write "sql=" & SQLLIST
End Sub
%>
