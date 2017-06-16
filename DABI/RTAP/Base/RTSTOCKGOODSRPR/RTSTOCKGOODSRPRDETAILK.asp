<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<% dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="送修單明細資料維護"
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
  formatName="送修單號;none;none;產品名稱;產品規格;單位;數量;單價;金額;成本;庫別;備註"
  sqlDelete="SELECT RTSTOCKREPAIRD1.REPAIRNO, RTSTOCKREPAIRD1.PRODNO, " _
           &"RTSTOCKREPAIRD1.ITEMNO, RTPRODH.PRODNC, RTPRODD1.SPEC, " _
           &"RTPRODD1.UNIT, RTSTOCKREPAIRD1.REPAIRQTY, RTSTOCKREPAIRD1.PRICE, " _
           &"RTSTOCKREPAIRD1.AMT, RTSTOCKREPAIRD1.COST, " _
           &"RTSTOCKREPAIRD1.WAREHOUSE, RTSTOCKREPAIRD1.REPAIRDESC " _
           &"FROM RTSTOCKREPAIRD1 LEFT OUTER JOIN " _
           &"RTPRODH ON " _
           &"RTSTOCKREPAIRD1.PRODNO = RTPRODH.PRODNO LEFT OUTER JOIN " _
           &"RTPRODD1 ON RTSTOCKREPAIRD1.PRODNO = RTPRODD1.PRODNO AND " _
           &"RTSTOCKREPAIRD1.ITEMNO = RTPRODD1.ITEMNO " _
           &"WHERE RTSTOCKREPAIRD1.RETURNNO='*' " 
  dataTable="RTSTOCKREPAIRD1"
  numberOfKey=3
  dataProg="RTSTOCKGOODSRPRDETAILD.asp"
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
  keyListPageSize=60
  searchProg="self"
  searchShow="全部"
  searchQry="RTSTOCKREPAIRD1.REPAIRNO='" &aryParmKey(0) &"'"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  sqlList="SELECT RTSTOCKREPAIRD1.REPAIRNO, RTSTOCKREPAIRD1.PRODNO, " _
           &"RTSTOCKREPAIRD1.ITEMNO, RTPRODH.PRODNC, RTPRODD1.SPEC, " _
           &"RTPRODD1.UNIT, RTSTOCKREPAIRD1.REPAIRQTY, RTSTOCKREPAIRD1.PRICE, " _
           &"RTSTOCKREPAIRD1.AMT, RTSTOCKREPAIRD1.COST, " _
           &"RTSTOCKREPAIRD1.WAREHOUSE, RTSTOCKREPAIRD1.REPAIRDESC " _
           &"FROM RTSTOCKREPAIRD1 LEFT OUTER JOIN " _
           &"RTPRODH ON " _
           &"RTSTOCKREPAIRD1.PRODNO = RTPRODH.PRODNO LEFT OUTER JOIN " _
           &"RTPRODD1 ON RTSTOCKREPAIRD1.PRODNO = RTPRODD1.PRODNO AND " _
           &"RTSTOCKREPAIRD1.ITEMNO = RTPRODD1.ITEMNO " _
           &"WHERE " & searchQry & " " _
           &"ORDER BY RTSTOCKREPAIRD1.PRODNO,RTSTOCKREPAIRD1.ITEMNO "
End Sub
%>
