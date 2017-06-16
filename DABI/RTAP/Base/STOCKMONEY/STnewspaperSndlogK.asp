<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="老謝上菜管理系統"
  title="電子報發送記錄查詢"
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
 ' EMAILFIELDNO=3
 ' EMAILFIELDFLAG="Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=STOCK;uid=sa;pwd=alittlecat@cbn"
  formatName="none;none;電子報;年度;月份;01;02;03;04;05;06;07;08;09;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31"
  sqlDelete="SELECT  NEWSPAPERKIND, NEWSPAPERCODE, YY, MM, CASE WHEN SUBSTRING(DD,1, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 2, 1) ='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 3, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 4, 1) ='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 5, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 6, 1) ='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 7, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 8, 1) ='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 9, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 10, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 11, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 12, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 13, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 14, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 15, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 16, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 17, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 18, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 19, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 20, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 21, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 22, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 23, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 24, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 25, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 26, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 27, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 28, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 29, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 30, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 31, 1)='1' THEN 'Y' ELSE '' END " _
           &"FROM STNEWSPAPERSNDLOG "

  dataTable="STNEWSPAPERSNDLOG"
  userDefineDelete="Yes"
  numberOfKey=1
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=300,height=175,scrollbars=yes"
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
  searchProg="STnewspapersndlogs.asp"
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
     searchQry=" STNEWSPAPERSNDLOG.NEWSPAPERKIND<>'' "
     searchShow="全部"
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  sqlList="SELECT  NEWSPAPERKIND, NEWSPAPERCODE,codenc, YY, MM, CASE WHEN SUBSTRING(DD,1, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 2, 1) ='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 3, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 4, 1) ='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 5, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 6, 1) ='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 7, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 8, 1) ='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 9, 1) = '1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 10, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 11, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 12, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 13, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 14, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 15, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 16, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 17, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 18, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 19, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 20, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 21, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 22, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 23, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 24, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 25, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 26, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 27, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 28, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 29, 1)='1' THEN 'Y' ELSE '' END," _
           &"CASE WHEN SUBSTRING(DD, 30, 1)='1' THEN 'Y' ELSE '' END, CASE WHEN SUBSTRING(DD, 31, 1)='1' THEN 'Y' ELSE '' END " _
           &"FROM STNEWSPAPERSNDLOG left outer join stcode on STNEWSPAPERSNDLOG.NEWSPAPERKIND=stcode.kind and STNEWSPAPERSNDLOG.NEWSPAPERCODE=stcode.code where " & searchqry
  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>