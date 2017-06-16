<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Dim debug36
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="倉庫與業務員關係資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  debug36=false
  formatName="庫別;倉庫名稱;倉管員;業務員;可否領用"
  sqlDelete="SELECT HBWarehouseSales.WAREHOUSE, HBwarEhouse.WARENAME, RTObj.CUSNC, " _
           &"RTObj_1.CUSNC AS Expr1, HBWarehouseSales.ONOFF " _
           &"FROM HBWarehouseSales INNER JOIN " _
           &"HBwarEhouse ON HBWarehouseSales.WAREHOUSE = HBwarEhouse.WAREHOUSE INNER JOIN " _
           &"RTEmployee ON HBwarEhouse.MAINTAINUSR = RTEmployee.EMPLY INNER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
           &"RTEmployee RTEmployee_1 ON " _
           &"HBWarehouseSales.EMPLY = RTEmployee_1.EMPLY INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID " _
           &"where hbwarehouseSALES.warehouse='*' order by hbwarehouseSALES.warehouse "

  dataTable="hbwarehousesales"
  userDefineDelete="Yes"  
  extTable=""
  numberOfKey=1
  dataProg="HBwarehouseSALESD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=2
  keyListPageSize=40
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
  searchFirst=false
  If searchQry="" Then
     searchQry=" HBWAREHOUSEsales.WAREHOUSE <>'*' "
     searchShow="全部"
  End If
  sqlList="SELECT HBWarehouseSales.WAREHOUSE, HBwarEhouse.WARENAME, RTObj.CUSNC, " _
           &"RTObj_1.CUSNC AS Expr1, HBWarehouseSales.ONOFF " _
           &"FROM HBWarehouseSales INNER JOIN " _
           &"HBwarEhouse ON HBWarehouseSales.WAREHOUSE = HBwarEhouse.WAREHOUSE INNER JOIN " _
           &"RTEmployee ON HBwarEhouse.MAINTAINUSR = RTEmployee.EMPLY INNER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
           &"RTEmployee RTEmployee_1 ON " _
           &"HBWarehouseSales.EMPLY = RTEmployee_1.EMPLY INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID " _
           &"where hbwarehouseSALES.warehouse<>'*'  and " & searchqry & " order by hbwarehouseSALES.warehouse "
'Response.Write "SQL=" &sqllist           
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>