<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="Sparq* 管理系統"
  title="客戶附加服務資料維護"
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
  formatName="none;none;none;none;附加服務;電話;生效日期;截止日期"
  sqlDelete="SELECT RTSparqADSLcustext.COMQ1, RTSparqADSLcustext.CUSID, " _
           &"RTSparqADSLcustext.ENTRYNO, RTSparqADSLcustext.TELNO, RTCode.CODENC, RTSparqADSLcustext.TELNO, " _
           &"RTSparqADSLcustext.SDATE, RTSparqADSLcustext.DROPDAT " _
           &"FROM RTSparqADSLcustext LEFT OUTER JOIN " _
           &"RTCode ON RTSparqADSLcustext.SRVTYPE = RTCode.CODE AND " _
           &"RTCode.KIND = 'E7'" _
           &"WHERE RTSparqADSLcustext.cusid<>'*' "
  dataTable="RTSparqADSLcustext"
  extTable=""
  numberOfKey=4
  dataProg="RTSparqADSLcustextD.asp"
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
  colSplit=2
  keyListPageSize=40
  searchProg="self"
  searchShow="全部"
  searchQry=" RTSparqADSLcustext.COMQ1=" & aryparmkey(0) & " and RTSparqADSLcustext.cusid='" & aryparmkey(1) & "' " 
  sqlList="SELECT RTSparqADSLcustext.COMQ1, RTSparqADSLcustext.CUSID, " _
         &"RTSparqADSLcustext.ENTRYNO, RTSparqADSLcustext.TELNO, RTCode.CODENC, RTSparqADSLcustext.TELNO, " _
         &"RTSparqADSLcustext.SDATE, RTSparqADSLcustext.DROPDAT " _
         &"FROM RTSparqADSLcustext LEFT OUTER JOIN " _
         &"RTCode ON RTSparqADSLcustext.SRVTYPE = RTCode.CODE AND " _
         &"RTCode.KIND = 'E7'" _
         &"WHERE " &searchQry
'Response.Write "sql=" & SQLLIST         
End Sub
%>
