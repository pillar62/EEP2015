<!-- #include virtual="/WebUtilityV2/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<% dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Hi Building管理系統"
  title="產品明細檔資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  functionOptName="適用方案;套餐組合"
  functionOptProgram="RTProductsuitcasek.asp;RTProductcombinEk.asp"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;細項編號;單位;品名;規格;定價;售價;安全庫存量;停售日;套餐"
  sqlDelete="SELECT RTPRODD1.PRODNO, RTPRODD1.ITEMNO, RTCode.CODENC, RTPRODD1.itemNC, " _
           &"RTPRODD1.SPEC, RTPRODD1.LISTPRICE, RTPRODD1.SALEPRICE, " _
           &"RTPRODD1.SAFESTOCK, RTPRODD1.BREAKDAT, RTPRODd1.PACKAGE " _
           &"FROM RTPRODD1 INNER JOIN " _
           &"RTCode ON RTPRODD1.UNIT = RTCode.CODE AND RTCode.KIND = 'B5' " _
           &"WHERE RTPRODD1.PRODNO='*' " 
  dataTable="RTProDD1"
  numberOfKey=2
  dataProg="RTPRODUCTDetailD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=60
  searchProg="self"
' searchShow=FrGetConsigneeDesc(aryParmKey(0))
'  searchQry="rtConsigneecty.cusid='" &aryParmKey(0) &"'"
' Open search program when first entry this keylist
  If searchQry="" Then
     searchFirst=True
     searchQry=" RTproDD1.PRODNO='" & aryparmkey(0) & "'"
     searchShow=""
  Else
     searchFirst=False
  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  sqlList="SELECT RTPRODD1.PRODNO, RTPRODD1.ITEMNO, RTCode.CODENC, RTPRODD1.itemNC, " _
           &"RTPRODD1.SPEC, RTPRODD1.LISTPRICE, RTPRODD1.SALEPRICE, " _
           &"RTPRODD1.SAFESTOCK, RTPRODD1.BREAKDAT, RTPRODd1.PACKAGE " _
           &"FROM RTPRODD1 LEFT OUTER JOIN " _
           &"RTCode ON RTPRODD1.UNIT = RTCode.CODE AND RTCode.KIND = 'B5' " _
           &"WHERE RTPRODD1.PRODNO='" & aryparmkey(0) &"' and " & searchQry & " " _
           &"ORDER BY RTPRODD1.PRODNO,rtprodd1.itemno "
End Sub
%>
