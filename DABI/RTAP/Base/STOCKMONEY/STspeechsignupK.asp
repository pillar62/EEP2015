<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="老謝上菜管理系統"
  title="演講活動報名管理"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=" 審核 ;取消審核; 作廢 ;作廢返轉;異動查詢"
  functionOptProgram="STSPEECHSIGNUPCFM.ASP;STSPEECHSIGNUPCFMC.ASP;STSPEECHSIGNUPDROP.ASP;STSPEECHSIGNUPDROPC.ASP;STSPEECHSIGNUPLOGK.ASP"
  functionOptPrompt="Y;Y;Y;Y;N"
  functionoptopen="1;1;1;1;1"
    'EMAIL欄位INDEX
  EMAILFIELDNO=7
  EMAILFIELDFLAG="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
  formatName="none;none;演講日期;姓名;報名日期;報名種類;電話;EMAIL;審核日期;審核人員;作廢日期;作廢人員"
  sqlDelete="SELECT  YYMMDD,EMAIL, YYMMDD, NAME, APPLYDAT,CODENC, TEL, EMAIL, CONFIRMDAT, CONFIRMUSR, CANCELDAT, CANCELUSR " _
           &"FROM  SpeechSignUP LEFT OUTER JOIN STCODE ON LIVEORNET = STCODE.CODE AND STCODE.KIND='A5' "

  dataTable="STSPEECHSIGNUP"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth="600"
  diaHeight="400"
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="STSPEECHSIGNUPS.asp"
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
     searchQry=" SPEECHSIGNUP.YYMMDD<>'' and  SPEECHSIGNUP.canceldat is null "
     searchShow="全部(不含已作廢)"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqlList="SELECT YYMMDD,EMAIL, YYMMDD, NAME, APPLYDAT,CODENC, TEL, EMAIL, CONFIRMDAT, CONFIRMUSR, CANCELDAT, CANCELUSR " _
         &"FROM  SpeechSignUP LEFT OUTER JOIN STCODE ON LIVEORNET = STCODE.CODE AND STCODE.KIND='A5' WHERE " & SEARCHQRY  
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>