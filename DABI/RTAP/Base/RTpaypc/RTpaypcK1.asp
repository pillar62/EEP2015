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
  title="RT施工費用明細表列印撤銷(技術部)"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="客戶明細"
  functionOptPrompt="N"
  functionOptProgram="RTpaypcK.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  
  DSN="DSN=RTLIb"
  formatName="付款表批號;列印員;列印日;客戶筆數;施工費總額;補助費總額;合計;會計審核日;會計審核員"
  sqlDelete="SELECT RTCust.PAYDTLPRTNO, RTCust.paydtldat, RTObj.CUSNC, COUNT(RTCust.CUSID), " _
           &"SUM(RTCust.SETFEE), sum(rtcust.setfeediff),SUM(RTCust.SETFEE)+sum(rtcust.setfeediff),RTCust.acccfmDAT, RTObj1.CUSNC " _
           &"FROM RTEmployee RTEmployee1 LEFT OUTER JOIN " _
           &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
           &"RTEmployee LEFT OUTER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID RIGHT OUTER JOIN " _
           &"RTCust ON RTEmployee.EMPLY = RTCust.paydtlusr ON  " _
           &"RTEmployee1.EMPLY = RTCust.acccfmusr " _
           &"WHERE RTCust.PAYDTLPRTNO = '*' " 
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
 
  searchProg="RTPayPcS.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" and rtcust.PAYDTLPRTNO <>'' "
     searchShow="已列印"
  End If   
  sqlList="SELECT RTCust.PAYDTLPRTNO, RTCust.paydtldat, RTObj.CUSNC, COUNT(RTCust.CUSID), " _
           &"SUM(RTCust.SETFEE), sum(rtcust.setfeediff),SUM(RTCust.SETFEE)+sum(rtcust.setfeediff),RTCust.acccfmDAT, RTObj1.CUSNC " _
           &"FROM RTEmployee RTEmployee1 LEFT OUTER JOIN " _
           &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
           &"RTEmployee LEFT OUTER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID RIGHT OUTER JOIN " _
           &"RTCust ON RTEmployee.EMPLY = RTCust.paydtlusr ON  " _
           &"RTEmployee1.EMPLY = RTCust.acccfmusr " _
           &"WHERE RTCust.PAYDTLPRTNO <> '' " & searchqry _
           &"group by RTCust.PAYDTLPRTNO, RTCust.paydtldat,  RTObj.CUSNC,RTCust.acccfmDAT, RTObj1.CUSNC " _
           &"order by RTCust.PAYDTLPRTNO desc "
          'Response.Write "SQL=" & SQllist
End Sub
%>