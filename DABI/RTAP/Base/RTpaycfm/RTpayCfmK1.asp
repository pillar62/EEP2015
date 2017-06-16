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
  title="RT施工費用付款審核確認"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="客戶明細"
  functionOptPrompt="N"
  functionOptProgram="RTpaycfmkeylist.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  
  DSN="DSN=RTLIb"
  formatName="付款表批號;列印人員;列印日期;客戶筆數;標準施工費總額;施工補助費總額;會計審核日期;會計審核人員"
  sqlDelete="SELECT  RTCust.PAYDTLPRTNO, RTObj1.CUSNC, RTCust.PAYDTLDAT, COUNT(RTCust.CUSID), " _
         &"SUM(RTCust.SETFEE), SUM(RTCust.SETFEEDIFF), RTCust.ACCCFMDAT, RTObj.CUSNC " _
         &"FROM  RTEmployee RTEmployee1 LEFT OUTER JOIN " _
         &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
         &"RTCust ON RTEmployee1.EMPLY = RTCust.PAYDTLUSR LEFT OUTER JOIN " _
         &"RTObj RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
         &"RTCust.ACCCFMUSR = RTEmployee.EMPLY " _
         &"WHERE rtcust.PAYDTLPRTNO = '*' "
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
  colSplit=1
  keyListPageSize=20  
 
  searchProg="rtPAYsearch.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" and rtcust.acccfmdat is null "
     searchShow="未審核"
  End If   
  sqlList="SELECT  RTCust.PAYDTLPRTNO, RTObj1.CUSNC, RTCust.PAYDTLDAT, COUNT(RTCust.CUSID), " _
         &"SUM(RTCust.SETFEE), SUM(RTCust.SETFEEDIFF), RTCust.ACCCFMDAT, RTObj.CUSNC " _
         &"FROM  RTEmployee RTEmployee1 LEFT OUTER JOIN " _
         &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
         &"RTCust ON RTEmployee1.EMPLY = RTCust.PAYDTLUSR LEFT OUTER JOIN " _
         &"RTObj RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
         &"RTCust.ACCCFMUSR = RTEmployee.EMPLY " _
         &"WHERE rtcust.PAYDTLPRTNO <> '' " & searchqry _
         &" GROUP BY RTCust.PAYDTLPRTNO, RTOBJ1.CUSNC, RTCust.paydtldat, RTCust.acccfmdat,rtobj.cusnc order by RTCust.PAYDTLPRTNO desc "
 'Response.Write "SQL=" & SQllist
End Sub
%>