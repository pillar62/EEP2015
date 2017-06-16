<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<% dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="各類合約管理系統"
  title="業務類合約產品檔資料維護"
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
  formatName="none;產品代號;規格代號;產品名稱;規格名稱;進貨價;經銷出貨價;銷售價;利潤;保固起日;保固迄日"
  sqlDelete="SELECT HBContractSalesD.CTNO, HBContractSalesD.PRODNO, " _
           &"HBContractSalesD.ITEMNO, RTPRODH.PRODNC, RTPRODD1.SPEC, " _
           &"HBContractSalesD.IPRICE, " _
           &"HBContractSalesD.OPRICE, HBContractSalesD.SPRICE, HBContractSalesD.PROFIT, " _
           &"HBContractSalesD.SGUARANTEE, HBContractSalesD.EGUARANTEE " _
           &"FROM HBContractSalesD INNER JOIN " _
           &"RTPRODH ON HBContractSalesD.PRODNO = RTPRODH.PRODNO INNER JOIN " _
           &"RTPRODD1 ON HBContractSalesD.PRODNO = RTPRODD1.PRODNO AND " _
           &"HBContractSalesD.ITEMNO = RTPRODD1.ITEMNO  " _
           &"WHERE HBContractSalesD.CTNO=0 " 
  dataTable="HBContractSalesD"
  numberOfKey=3
  dataProg="RTContractkind1D.asp"
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
  keyListPageSize=20
  searchProg="self"
  searchShow=FrGetContractDesc(aryParmKey(0))
  searchQry="HBContractSalesD.CTNO=" &aryParmKey(0) &" "
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  sqlList="SELECT HBContractSalesD.CTNO, HBContractSalesD.PRODNO, " _
           &"HBContractSalesD.ITEMNO, RTPRODH.PRODNC, RTPRODD1.SPEC, " _
           &"HBContractSalesD.IPRICE,  " _
           &"HBContractSalesD.OPRICE, HBContractSalesD.SPRICE, HBContractSalesD.PROFIT, " _
           &"HBContractSalesD.SGUARANTEE, HBContractSalesD.EGUARANTEE " _
           &"FROM HBContractSalesD INNER JOIN " _
           &"RTPRODH ON HBContractSalesD.PRODNO = RTPRODH.PRODNO INNER JOIN " _
           &"RTPRODD1 ON HBContractSalesD.PRODNO = RTPRODD1.PRODNO AND " _
           &"HBContractSalesD.ITEMNO = RTPRODD1.ITEMNO  " _
           &"WHERE " & searchQry
 'Response.Write "sql=" & sqlLIST & "<br>"
End Sub
%>
<!-- #include file="RTGetContractDesc.inc" -->