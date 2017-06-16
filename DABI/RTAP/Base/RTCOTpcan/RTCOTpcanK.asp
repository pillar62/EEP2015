<%
  Dim search1,parm,vk,debug36
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
  title="COT建置自付額明細表列印撤銷"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  functionOptName="列印撤銷"
  functionOptProgram="Verify.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLIb"
  formatName="社區名稱;估價金額;同意安裝;T1開通日期;列印批號;審核日期"
  sqlDelete="SELECT COMN, ASSESS, AGREE, T1APPLY, PAYPRTSEQ, ACCOUNTCFM FROM RTCmty WHERE COMN <>'*' "
             
  'response.write "sql=" &sqlDelete &"<br>"
  debug36=true
  dataTable="rtCMTY"
  numberOfKey=1
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature="width=640,height=460,scrollbars=yes"
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=20
  searchProg="RTCOTpcans.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" and payprtseq is null and COMN='*'"& ";"
     searchShow="未列印"
  End If   
  v=split(searchqry,";")
  '---列印批號空白"
  if len(trim(V(1)))=0 then
     V(0)=" and payprtseq is null and COMN='*'"
  end if 
  sqlList="SELECT COMN, ASSESS, AGREE, T1APPLY, PAYPRTSEQ, ACCOUNTCFM FROM RTCmty WHERE COMN <>'*' " &V(0)
  session("COTcanpprtno")=V(1)
'Response.Write "SQLlist=" & SQllist
End Sub
%>