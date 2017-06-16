<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="老謝上菜管理系統"
  title="非會員電子報訂閱資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 強 停 ;取消強停"
  functionOptProgram="STNONMEMBERNEWSPAPERSTOP.ASP;STNONMEMBERNEWSPAPERSTOPC.ASP"
  functionOptPrompt="Y;Y"
  functionoptopen="1;1"
  'EMAIL欄位INDEX
  EMAILFIELDNO=5  
  EMAILFIELDFLAG="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
  formatName="none;none;none;名稱;電子報類別;EMAIL;電話;起始日;終止日;強制關閉;傳送次數"
  sqlDelete="SELECT  NonMemberNewspaper.EMAIL, NonMemberNewspaper.NEWSPAPERKIND,NonMemberNewspaper.NEWSPAPERCODE, " _
           &"NonMemberNewspaper.CUSNC, STCode.CODENC, NonMemberNewspaper.EMAIL AS Expr1, NonMemberNewspaper.TEL, " _
           &"NonMemberNewspaper.STRDAT, NonMemberNewspaper.ENDDAT, NonMemberNewspaper.CLOSEFLAG,NonMemberNewspaper.sndcnt " _
           &"FROM NonMemberNewspaper INNER JOIN STCode ON NonMemberNewspaper.NEWSPAPERKIND = STCode.KIND AND " _
           &"NonMemberNewspaper.NEWSPAPERCODE = STCode.CODE "

  dataTable="NONMEMBERNEWSPAPER"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="STNONMemberNEWSPAPERD.asp"
  datawindowFeature=""
  searchWindowFeature="width=400,height=200,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=600,height=350,scrollbars=yes"
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="STNONMemberNEWSPAPERS.asp"
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
     searchQry=" NONMEMBERNEWSPAPER.EMAIL<>'' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqlList="SELECT  NonMemberNewspaper.EMAIL, NonMemberNewspaper.NEWSPAPERKIND,NonMemberNewspaper.NEWSPAPERCODE, " _
           &"NonMemberNewspaper.CUSNC, STCode.CODENC, NonMemberNewspaper.EMAIL AS Expr1, NonMemberNewspaper.TEL, " _
           &"NonMemberNewspaper.STRDAT, NonMemberNewspaper.ENDDAT, NonMemberNewspaper.CLOSEFLAG,NonMemberNewspaper.sndcnt  " _
           &"FROM NonMemberNewspaper INNER JOIN STCode ON NonMemberNewspaper.NEWSPAPERKIND = STCode.KIND AND " _
           &"NonMemberNewspaper.NEWSPAPERCODE = STCode.CODE  WHERE " & SEARCHQRY  & " ORDER BY STRDAT "
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>