<%
  Dim search1,parm,vk,debug36,search2
  parm=request("Key")
  vk=split(parm,";")
  if ubound(vk) > 0 then  searchX=vK(0)
%>
<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->

<%
Sub SrEnvironment()
  company=application("company")
  system="HI-Building管理系統"
  title="RT派工單列印撤銷(客服部)"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="客戶明細"
  functionOptPrompt="N"
  functionOptProgram="RTworkCpK.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  
  DSN="DSN=RTLIb"
  formatName="派工單號;列印人員;列印日期;客戶筆數;完工日期"
  sqlDelete="SELECT RTCust.insprtno, RTObj.CUSNC, RTCust.insprtdat, COUNT(RTCust.CUSID), " _
           &"RTCust.FINISHDAT " _
           &"FROM RTEmployee LEFT OUTER JOIN RTObj " _
           &"ON RTEmployee.CUSID = RTObj.CUSID RIGHT OUTER JOIN " _
           &"RTCust ON RTEmployee.EMPLY = RTCust.insprtusr " _
           &"WHERE RTCust.insprtno = '*' " 
  'response.write "sql=" &sqldelete
  dataTable="b"
  numberOfKey=1
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=TRUE
  goodMorningImage="cbbn.jpg"
  colSplit=2
  keyListPageSize=40
 
  searchProg="rtwcansearch.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" and RTCust.insprtno <>'' "
     searchShow="已列印"
  End If   
  sqlList="SELECT RTCust.insprtno, RTObj.CUSNC, RTCust.insprtdat, COUNT(RTCust.CUSID) " _
         &"FROM RTEmployee LEFT OUTER JOIN RTObj " _
         &"ON RTEmployee.CUSID = RTObj.CUSID RIGHT OUTER JOIN " _
         &"RTCust ON RTEmployee.EMPLY = RTCust.insprtusr " _
         &"WHERE RTCust.insprtno <> '' " & searchqry _
         &"group by RTCust.insprtno, RTObj.CUSNC, RTCust.insprtdat " _
         &" order by RTCust.insprtno desc "
 'Response.Write "SQL=" & SQllist
End Sub
%>