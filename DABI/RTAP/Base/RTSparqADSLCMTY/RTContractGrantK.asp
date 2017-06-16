<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<% dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="各類合約管理系統"
  title="補助款資料維護"
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
  formatName="none;none;補助款代號;補助項目;週期;補助金額;補助起日;補助迄日"
  sqlDelete="SELECT HBContractGrant.CTNO,HBContractGrant.grantid,HBContractGrant.PERIOD, RTCode.CODENC, RTCode_1.CODENC AS Expr1, " _
           &"HBContractGrant.EXPENSE, HBContractGrant.GRANTSTRDAT, " _
           &"HBContractGrant.GRANTENDDAT " _
           &"FROM HBContractGrant INNER JOIN " _
           &"RTCode ON HBContractGrant.GRANTID = RTCode.CODE AND " _
           &"RTCode.KIND = 'F8' INNER JOIN " _
           &"RTCode RTCode_1 ON HBContractGrant.PERIOD = RTCode_1.CODE AND " _
           &"RTCode_1.KIND = 'F9'  " _
           &"WHERE HBContractGrant.CTNO=0 " 
  dataTable="HBContractGrant"
  numberOfKey=2
  dataProg="RTContractGrantD.asp"
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
  colSplit=2
  keyListPageSize=80
  searchProg="self"
  searchShow=FrGetContractDesc(aryParmKey(0))
  searchQry="HBContractgrant.CTNO=" &aryParmKey(0) &" "
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  sqlList="SELECT HBContractGrant.CTNO,HBContractGrant.grantid,HBContractGrant.PERIOD, RTCode.CODENC, RTCode_1.CODENC AS Expr1, " _
           &"HBContractGrant.EXPENSE, HBContractGrant.GRANTSTRDAT, " _
           &"HBContractGrant.GRANTENDDAT " _
           &"FROM HBContractGrant INNER JOIN " _
           &"RTCode ON HBContractGrant.GRANTID = RTCode.CODE AND " _
           &"RTCode.KIND = 'F8' INNER JOIN " _
           &"RTCode RTCode_1 ON HBContractGrant.PERIOD = RTCode_1.CODE AND " _
           &"RTCode_1.KIND = 'F9'  " _
           &"WHERE " & searchQry
 'Response.Write "sql=" & sqlLIST & "<br>"
End Sub
%>
<!-- #include file="RTGetContractDesc.inc" -->