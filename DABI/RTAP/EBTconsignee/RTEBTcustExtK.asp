<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS用戶附加服務資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  ButtonEnable="N;N;Y;Y;Y;N"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  accessMode="I"
  DSN="DSN=RTLib"
  formatName="none;none;none;項次;none;附加服務;電話;資費方案;申請日期;截止日期;轉檔審核日;電子轉檔日"
  sqlDelete="SELECT RTEBTCUSTEXT.COMQ1, RTEBTCUSTEXT.LINEQ1, RTEBTCUSTEXT.CUSID, RTEBTCUSTEXT.ENTRYNO,RTEBTCUSTEXT.TELNO, RTCode_2.CODENC AS Expr1, " _
         &"RTEBTCUSTEXT.TELNO, RTCode_1.CODENC, RTEBTCUSTEXT.SDATE, " _
         &"RTEBTCUSTEXT.DROPDAT,RTEBTCUSTEXT.chkdat, RTEBTCUSTEXT.transdat FROM RTEBTCUSTEXT INNER JOIN RTCode RTCode_1 ON RTEBTCUSTEXT.DIALERPAYTYPE = RTCode_1.CODE AND " _
         &"RTCode_1.KIND = 'G7' LEFT OUTER JOIN RTCode RTCode_2 ON RTEBTCUSTEXT.SRVTYPE = RTCode_2.CODE AND RTCode_2.KIND = 'E7' " _
           &"WHERE RTEBTcustext.COMQ1=0 "
  dataTable="RTEBTcustext"
  extTable=""
  numberOfKey=5
  dataProg="RTEBTcustextD.asp"
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
  keyListPageSize=40
  searchProg="self"
  searchShow="全部"
  searchQry=" RTEBTcustext.COMQ1=" & aryparmkey(0) & " and RTEBTcustext.LINEQ1=" & aryparmkey(1) & " and RTEBTcustext.CUSID='" &  aryparmkey(2) & "' " 
  sqlList="SELECT RTEBTCUSTEXT.COMQ1, RTEBTCUSTEXT.LINEQ1, RTEBTCUSTEXT.CUSID, RTEBTCUSTEXT.ENTRYNO,RTEBTCUSTEXT.TELNO, RTCode_2.CODENC AS Expr1, " _
         &"RTEBTCUSTEXT.TELNO, RTCode_1.CODENC, RTEBTCUSTEXT.SDATE, " _
         &"RTEBTCUSTEXT.DROPDAT,RTEBTCUSTEXT.chkdat, RTEBTCUSTEXT.transdat FROM RTEBTCUSTEXT INNER JOIN RTCode RTCode_1 ON RTEBTCUSTEXT.DIALERPAYTYPE = RTCode_1.CODE AND " _
         &"RTCode_1.KIND = 'G7' LEFT OUTER JOIN RTCode RTCode_2 ON RTEBTCUSTEXT.SRVTYPE = RTCode_2.CODE AND RTCode_2.KIND = 'E7' " _
         &"WHERE " &searchQry
'Response.Write "sql=" & SQLLIST         
End Sub
%>
