 <%
  Dim search1,parm,vk
  parm=request("Key")
  vk=split(parm,";")
  if ubound(vk) > 0 then  searchX=vK(0)
%>
<!-- #include virtual="/WebUtilityv3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="COT建置自付額審核撤銷"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  functionOptName="自付額審核撤銷"
  functionOptProgram="Verify.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="none;社區名稱;估價金額;同意安裝;T1開通日期;列印批號;列印日期;列印人員;審核日期;審核人員"
  sqlDelete="SELECT a.comq1,a.COMN, a.ASSESS, a.AGREE, a.T1APPLY, a.PAYPRTSEQ, a.PAYPRTD, e.CUSNC, a.ACCOUNTCFM,c.CUSNC "_
             & "FROM RTEmployee b INNER JOIN RTObj c ON b.CUSID = c.CUSID " _
             & "RIGHT OUTER JOIN RTObj e INNER JOIN RTEmployee d ON e.CUSID = d.CUSID " _
             & "INNER JOIN RTCmty a ON d.EMPLY = a.PAYPRTUSR ON b.EMPLY = a.ACCOUNTUSR " _
             & "WHERE  a.COMN <>'*' "
  'response.write "sql=" &sqlDelete &"<br>"
  dataTable="rtCMTY"
  numberOfKey=1
  dataProg="/webap/rtap/base/rtcmty/rtcmtyd.asp"
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
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  searchFirst=false
  If searchQry="" Then
     searchQry=""
     searchShow="已審核"
  End If   

   sqlList="SELECT a.comq1,a.COMN, a.ASSESS, a.AGREE, a.T1APPLY, a.PAYPRTSEQ, a.PAYPRTD,  e.CUSNC, a.ACCOUNTCFM, c.CUSNC "_
          & "FROM RTEmployee b INNER JOIN RTObj c ON b.CUSID = c.CUSID " _
          & "RIGHT OUTER JOIN RTObj e INNER JOIN RTEmployee d ON e.CUSID = d.CUSID " _
          & "INNER JOIN RTCmty a ON d.EMPLY = a.PAYPRTUSR ON b.EMPLY = a.ACCOUNTUSR " _
          & "WHERE  a.COMN <>'*' and a.payprtseq='" &aryparmkey(0) & "'"
   session("COTcanprtno")=aryparmkey(0)
  'Response.Write "SQLlist=" & SQllist
End Sub

%>