<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="老謝上菜管理系統"
  title="網站會員電子報訂閱資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  functionoptopen=""
  'EMAIL欄位INDEX
  EMAILFIELDNO=10
  EMAILFIELDFLAG="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
  formatName="none;none;none;會員編號;名稱;會員類別;電子報種類;起始訂閱日;終止訂閱日;強停日期;EMAIL;電話;行動;傳真;傳送次數"
  sqlDelete="SELECT  STMemberNewsPaper.MEMBERID, STMemberNewsPaper.NEWSPAPERKIND, STMemberNewsPaper.NEWSPAPERCODE,STMemberNewsPaper.MEMBERID, " _
           &"STMember.CUSNC, STCode_1.CODENC AS Expr1, STCode_2.CODENC, STMemberNewsPaper.STRDAT, STMemberNewsPaper.ENDDAT, " _
           &"STMemberNewsPaper.CLOSEFLAG, STMember.EMAIL, STMember.TEL, STMember.MOBILE, STMember.FAX,STMemberNewsPaper.sndcnt " _
           &"FROM STCode STCode_1 RIGHT OUTER JOIN STRegMember ON STCode_1.CODE = STRegMember.YorMorT AND " _
           &"STCode_1.KIND = 'A4' RIGHT OUTER JOIN STMemberNewsPaper ON STRegMember.MEMBERID = STMemberNewsPaper.MEMBERID AND " _
           &"STRegMember.PRODID = '10558' LEFT OUTER JOIN STMember ON STMemberNewsPaper.MEMBERID = STMember.MEMBERID LEFT OUTER JOIN " _
           &"STCode STCode_2 ON STMemberNewsPaper.NEWSPAPERKIND = STCode_2.KIND AND " _
           &"STMemberNewsPaper.NEWSPAPERCODE = STCode_2.CODE "

  dataTable="STMEMBERNEWSPAPER"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=500,height=300,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="STMembernewspaperS2.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=XXLIB"
  sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     usergroup=rsxx("group")
  else
     usergroup=""
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '----
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" STMEMBERnewspaper.MEMBERID<>'' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqlList="SELECT  STMemberNewsPaper.MEMBERID, STMemberNewsPaper.NEWSPAPERKIND, STMemberNewsPaper.NEWSPAPERCODE,STMemberNewsPaper.MEMBERID, " _
           &"STMember.CUSNC, STCode_1.CODENC AS Expr1, STCode_2.CODENC, STMemberNewsPaper.STRDAT, STMemberNewsPaper.ENDDAT, " _
           &"STMemberNewsPaper.CLOSEFLAG, STMember.EMAIL, STMember.TEL, STMember.MOBILE, STMember.FAX,STMemberNewsPaper.sndcnt " _
           &"FROM STCode STCode_1 RIGHT OUTER JOIN STRegMember ON STCode_1.CODE = STRegMember.YorMorT AND " _
           &"STCode_1.KIND = 'A4' RIGHT OUTER JOIN STMemberNewsPaper ON STRegMember.MEMBERID = STMemberNewsPaper.MEMBERID AND " _
           &"STRegMember.PRODID = '10558' LEFT OUTER JOIN STMember ON STMemberNewsPaper.MEMBERID = STMember.MEMBERID LEFT OUTER JOIN " _
           &"STCode STCode_2 ON STMemberNewsPaper.NEWSPAPERKIND = STCode_2.KIND AND " _
           &"STMemberNewsPaper.NEWSPAPERCODE = STCode_2.CODE  where " & SEARCHQRY  
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>