<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<% dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="各類合約管理系統"
  title="應收應付帳號資料維護"
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
  formatName="none;項次;收付別;應收付月份;應收付日;費用說明;應收/付金額;實收付日期;實收/付金額;結案碼"
  sqlDelete="SELECT HBContractARAP.CTNO,HBContractARAP.ITEMNO, RTCode.CODENC, " _
           &"HBContractARAP.STRBILLINGYM, HBContractARAP.MONTHLYDAT, " _
           &"HBContractARAP.EXPENSEITEMNM, HBContractARAP.AMT, " _
           &"HBContractARAP.ACTUALDAT, HBContractARAP.ACTAMT, " _
           &"HBContractARAP.ENDCODE " _
           &"FROM HBContractARAP INNER JOIN " _
           &"RTCode ON HBContractARAP.ARORAP = RTCode.CODE AND RTCode.KIND = 'F7' " _
           &"WHERE HBContractARAP.CTNO=0 " 
  dataTable="HBContractARAP"
  numberOfKey=2
  dataProg="RTContractARAPd.asp"
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
  colSplit=1
  keyListPageSize=80
  searchProg="self"
  searchShow=FrGetContractDesc(aryParmKey(0))
  searchQry="HBContractarap.CTNO=" &aryParmKey(0) &" "
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  sqlList="SELECT HBContractARAP.CTNO, HBContractARAP.ITEMNO, RTCode.CODENC, " _
           &"HBContractARAP.STRBILLINGYM, HBContractARAP.MONTHLYDAT, " _
           &"HBContractARAP.EXPENSEITEMNM, HBContractARAP.AMT, " _
           &"HBContractARAP.ACTUALDAT, HBContractARAP.ACTAMT, " _
           &"HBContractARAP.ENDCODE " _
           &"FROM HBContractARAP INNER JOIN " _
           &"RTCode ON HBContractARAP.ARORAP = RTCode.CODE AND RTCode.KIND = 'F7' " _
           &"WHERE " & searchQry
 'Response.Write "sql=" & sqlLIST & "<br>"
End Sub
%>
<!-- #include file="RTGetContractDesc.inc" -->