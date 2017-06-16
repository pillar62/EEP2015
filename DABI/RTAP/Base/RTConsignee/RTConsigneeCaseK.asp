<!-- #include virtual="/WebUtilityV2/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<% dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="速博(Sparq*)管理系統"
  title="經銷商經銷方案資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  functionOptName=""
  functionOptProgram=""
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;N"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;方案代碼;方案名稱"
  sqlDelete="SELECT RTConsigneeCASE.CUSID, RTConsigneeCASE.CASEID, RTCODE.CODENC " _
           &"FROM RTConsigneeCASE INNER JOIN " _
           &"RTCODE ON RTConsigneeCASE.CASEID = RTCODE.CODE  " _
           &"WHERE RTCODE.KIND = 'L7' AND RTConsigneeCASE.CUSID='*' " 
  dataTable="RTConsigneeCASE"
  numberOfKey=2
  dataProg="RTConsigneeCASED.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  debug36=false
  goodMorningImage="cbbn.jpg"
  colSplit=4
  keyListPageSize=80
  searchProg="self"
  searchShow=FrGetConsigneeDesc(aryParmKey(0))
  searchQry="rtConsigneeCASE.cusid='" &aryParmKey(0) &"'"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  sqlList="SELECT RTConsigneeCASE.CUSID, RTConsigneecase.caseid, RTCODE.CODENC " _
           &"FROM RTConsigneeCASE INNER JOIN " _
           &"RTCODE ON RTConsigneeCASE.CASEID = RTCODE.CODE  " _
           &"WHERE RTCODE.KIND = 'L7' and " & searchQry & " " _
           &"ORDER BY RTConsigneeCASE.CUSID,RTConsigneeCASE.CASEID "
' Response.Write "sql=" & sqlLIST & "<br>"
End Sub
%>
<!-- #include file="RTGetConsigneeDesc.inc" -->