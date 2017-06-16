<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Hi Building管理系統"
  title="產品主檔資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="產  品"
  functionOptProgram="RTProductDetailK.asp"
  functionOptPrompt="N"    
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="產品編號;產品類別;總帳科目;明細科目;產品名稱;庫存控制"
  sqlDelete="SELECT RTPRODH.PRODNO, RTCode.CODENC , RTPRODH.COUNTINGH, RTPRODH.COUNTINGD, " _
           &"RTPRODH.PRODNC,RTPRODH.STOCKCHECK FROM RTPRODH INNER JOIN " _
           &"RTCode ON RTPRODH.PRODTYP = RTCode.CODE AND RTCode.KIND = 'E2' " _
           &"WHERE  RTPRODH.PRODNO='*' "
  dataTable="RTPRODH"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=1
  dataProg="RTProductD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=true
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="RTPRODUCTS.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=false
  If searchQry="" Then
     searchQry=" RTPRODH.PRODNO <>'*' "
     searchShow="全部"
  End If
  sqlList="SELECT RTPRODH.PRODNO, RTCode.CODENC , RTPRODH.COUNTINGH, RTPRODH.COUNTINGD, " _
         &"RTPRODH.PRODNC,RTPRODH.STOCKCHECK FROM RTPRODH INNER JOIN " _
         &"RTCode ON RTPRODH.PRODTYP = RTCode.CODE AND RTCode.KIND = 'E2' " _
         &"WHERE " &searchQry &" " _
         &"ORDER BY RTPRODH.PRODNO "
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()
End Sub
%>