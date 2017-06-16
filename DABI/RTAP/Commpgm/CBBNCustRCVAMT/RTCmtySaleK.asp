<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區業務員資料維護"
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
  formatName="none;none;業務姓名;生效日期;截止日期"
  sqlDelete="SELECT RTCmtySale.COMQ1, RTCmtySale.CUSID, RTObj.CUSNC, RTCmtySale.TDAT, " _
           &"RTCmtySale.EXDAT " _
           &"FROM RTObj INNER JOIN RTCmtySale ON RTObj.CUSID = RTCmtySale.CUSID " _
           &"WHERE RTCmtySale.CUSID='0' "
  dataTable="RTCmtySale"
  extTable=""
  numberOfKey=2
  dataProg="RTCmtySaleD.asp"
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
  keyListPageSize=20
  searchProg="self"
  searchShow=FrGetCmtyDesc(aryParmKey(0))
  searchQry="RTCmtySale.COMQ1=" &aryParmKey(0) &" "
  sqlList="SELECT RTCmtySale.COMQ1, RTCmtySale.CUSID, RTObj.CUSNC, RTCmtySale.TDAT, " _
         &"RTCmtySale.EXDAT " _
         &"FROM RTCmtySale INNER JOIN RTObj ON RTCmtySale.CUSID = RTObj.CUSID " _
         &"WHERE " &searchQry
End Sub
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
