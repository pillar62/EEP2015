<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區營業員資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;N"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;營業員姓名;生效日期;截止日期"
  sqlDelete="SELECT RTCmtyBus.COMQ1, RTCmtyBus.CUSID, RTObj.CUSNC, RTCmtyBus.TDAT, " _
           &"RTCmtyBus.EXDAT " _
           &"FROM RTObj INNER JOIN RTCmtyBus ON RTObj.CUSID = RTCmtyBus.CUSID " _
           &"WHERE RTCmtyBus.CUSID='0' "
  dataTable="RTCmtyBus"
  extTable=""
  numberOfKey=2
  dataProg="RTCmtyBusD.asp"
  datawindowFeature=""
  searchWindowFeature=""
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=False
  goodMorningImage=""
  colSplit=1
  keyListPageSize=12
  searchProg="self"
  searchShow=FrGetCmtyDesc(aryParmKey(0))
  searchQry="RTCmtyBus.COMQ1=" &aryParmKey(0) &" "
  sqlList="SELECT RTCmtyBus.COMQ1, RTCmtyBus.CUSID, RTObj.CUSNC, RTCmtyBus.TDAT, " _
         &"RTCmtyBus.EXDAT " _
         &"FROM RTCmtyBus INNER JOIN RTObj ON RTCmtyBus.CUSID = RTObj.CUSID " _
         &"WHERE " &searchQry
End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
