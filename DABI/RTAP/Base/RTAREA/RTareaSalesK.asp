<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="業務轄區與業務員關係資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;N"
 ' buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;業務員姓名"
  sqlDelete="SELECT RtareaSales.AREAID,rtareaSales.cusid, RTObj.CUSNC " _
           &"FROM RTAreaSales INNER JOIN " _
           &"RTEmployee ON RTAreaSales.CUSID = RTEmployee.EMPLY INNER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID and (RTareaSales.areaid='*') "
  dataTable="RTareaSales"
  userDefineDelete=""  
  extTable=""
  numberOfKey=2
  dataProg="RTareaSalesD.asp"
  datawindowFeature=""
  searchWindowFeature=""
  optionWindowFeature=""
  detailWindowFeature="width=640,height=460,scrollbars=yes"
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage="cbbn.jpg"
  colSplit=5
  keyListPageSize=100
  searchProg="self"
  searchFirst=false
  searchShow=FrGetAreaDesc(aryParmKey(0))  
  searchQry=" RTareaSales.areaid='" & aryparmkey(0) & "'"  
  sqlList="SELECT RtareaSales.AREAID,rtareaSales.cusid, RTObj.CUSNC " _
         &"FROM RTAreaSales INNER JOIN " _
         &"RTEmployee ON RTAreaSales.CUSID = RTEmployee.EMPLY INNER JOIN " _
         &"RTObj ON RTEmployee.CUSID = RTObj.CUSID  AND " &searchQry 
'Response.Write "SQL=" &sqllist           
End Sub
%>
<!-- #include file="RTGetAreaDesc.inc" -->