<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="老謝上菜管理系統"
  title="會員購買內容查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  functionoptopen=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
  formatName="none;none;none;產品方案;申請日;包月註冊碼;用戶HinetHN;年繳付款日;年繳付款額;年繳確認日;年繳認證帳號;有效日;作廢日"
  sqlDelete="SELECT STRegMember.MEMBERID, STRegMember.PRODID, STRegMember.YorMorT, STCode.CODENC, STRegMember.APPLYDAT, " _
           &"STRegMember.REGISTERNO, STRegMember.HNNO, STRegMember.PAYDAT, STRegMember.PAYAMT, STRegMember.CONFIRMDAT, " _
           &"STRegMember.YEARACCOUNT, STRegMember.VALIDDAT, STRegMember.DROPDAT " _
           &"FROM STRegMember LEFT OUTER JOIN STCode ON STRegMember.YorMorT = STCode.CODE AND STCode.KIND = 'A4' "

  dataTable="STREGMEMBER"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="STREGMemberD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="self"
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
     searchQry=" STREGMEMBER.MEMBERID='" & aryparmkey(0) & "' AND PRODID='10558'"
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqlList="SELECT STRegMember.MEMBERID, STRegMember.PRODID, STRegMember.YorMorT, STCode.CODENC, STRegMember.APPLYDAT, " _
           &"STRegMember.REGISTERNO, STRegMember.HNNO, STRegMember.PAYDAT, STRegMember.PAYAMT, STRegMember.CONFIRMDAT, " _
           &"STRegMember.YEARACCOUNT, STRegMember.VALIDDAT, STRegMember.DROPDAT " _
           &"FROM STRegMember LEFT OUTER JOIN STCode ON STRegMember.YorMorT = STCode.CODE AND STCode.KIND = 'A4' " _
           &"WHERE " & SEARCHQRY
  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>