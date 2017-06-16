<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="東森AVS管理系統"
  title="AVS用戶M2欠費拆機設備資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""  
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;<center>拆機工單號</center>;<center>項次</center>;<center>設備名稱/規格</center>;<center>數量</center>;<center>金額</center>;<center>出庫別</center>;<center>作廢日期</center>;<center>作廢原因</center>;<center>作廢人員</center>"
  sqlDelete="SELECT RTEBTcustM2M3SNDWORKHARDWARE.AVSNO, RTEBTcustM2M3SNDWORKHARDWARE.M2M3, RTEBTcustM2M3SNDWORKHARDWARE.seqx, " _
         &"RTEBTcustM2M3SNDWORKHARDWARE.PRTNO, RTEBTcustM2M3SNDWORKHARDWARE.SEQ, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTEBTcustM2M3SNDWORKHARDWARE.QTY, RTEBTcustM2M3SNDWORKHARDWARE.amt, " _
         &"HBwarehouse.WARENAME, RTEBTcustM2M3SNDWORKHARDWARE.DROPDAT, RTEBTcustM2M3SNDWORKHARDWARE.DROPREASON, RTObj.CUSNC " _
         &"FROM RTProdH RIGHT OUTER JOIN RTEBTcustM2M3SNDWORKHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTEBTcustM2M3SNDWORKHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTEBTcustM2M3SNDWORKHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTEBTcustM2M3SNDWORKHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTEBTcustM2M3SNDWORKHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTEBTcustM2M3SNDWORKHARDWARE.PRODNO " _
         &"WHERE RTEBTcustM2M3SNDWORKHARDWARE.AVSNO=0 "
  dataTable="RTEBTcustM2M3SNDWORKHARDWARE"
  extTable=""
  numberOfKey=4
  dataProg="RTEBTcustM2M3SNDWORKHARDWARED.asp"
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
  keyListPageSize=25
  '----
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTcustM2M3SNDWORKHARDWARE.AVSNO='" & aryparmkey(0) & "' and RTEBTcustM2M3SNDWORKHARDWARE.M2M3='" & aryparmkey(1) & "'  and RTEBTcustM2M3SNDWORKHARDWARE.seqx=" & aryparmkey(2) & " and RTEBTcustM2M3SNDWORKHARDWARE.prtno='" & aryparmkey(3) & "' "
     searchShow="合約編號︰"& aryparmkey(0) & ",拆機單號︰" &  aryparmkey(2)  
  ELSE
     SEARCHFIRST=FALSE
  End If  
 sqlList="SELECT RTEBTcustM2M3SNDWORKHARDWARE.AVSNO, RTEBTcustM2M3SNDWORKHARDWARE.M2M3,RTEBTcustM2M3SNDWORKHARDWARE.seqx, " _
         &"RTEBTcustM2M3SNDWORKHARDWARE.PRTNO, RTEBTcustM2M3SNDWORKHARDWARE.SEQ, " _
         &"RTProdH.PRODNC + '--' + RTProdD1.SPEC, RTEBTcustM2M3SNDWORKHARDWARE.QTY, RTEBTcustM2M3SNDWORKHARDWARE.amt, " _
         &"HBwarehouse.WARENAME, RTEBTcustM2M3SNDWORKHARDWARE.DROPDAT, RTEBTcustM2M3SNDWORKHARDWARE.DROPREASON, RTObj.CUSNC " _
         &"FROM RTProdH RIGHT OUTER JOIN RTEBTcustM2M3SNDWORKHARDWARE LEFT OUTER JOIN " _
         &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTEBTcustM2M3SNDWORKHARDWARE.DROPUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"HBwarehouse ON RTEBTcustM2M3SNDWORKHARDWARE.WAREHOUSE = HBwarehouse.WAREHOUSE LEFT OUTER " _
         &"JOIN RTProdD1 ON RTEBTcustM2M3SNDWORKHARDWARE.PRODNO = RTProdD1.PRODNO AND " _
         &"RTEBTcustM2M3SNDWORKHARDWARE.ITEMNO = RTProdD1.ITEMNO ON RTProdH.PRODNO = RTEBTcustM2M3SNDWORKHARDWARE.PRODNO " _
                  &"WHERE " &searchQry & ""
'Response.Write "sql=" & SQLLIST         
End Sub
%>
