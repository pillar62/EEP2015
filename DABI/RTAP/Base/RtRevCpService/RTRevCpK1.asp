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
  title="RT收款表列印撤銷(客服部)"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="客戶明細"
  functionOptPrompt="N"
  functionOptProgram="RTRevCpK.asp"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  
  DSN="DSN=RTLIb"
  formatName="收款表批號;列印日期;列印人員;客戶筆數;收款總額;財務審核日期;財務審核人員"
  sqlDelete="SELECT RTCust.RCVDTLNO, RTCust.RCVDTLDAT, RTObj.CUSNC, COUNT(RTCust.CUSID), " _
           &"SUM(RTCust.ACTRCVAMT), RTCust.FINRDFMDAT, RTObj1.CUSNC " _
           &"FROM RTEmployee RTEmployee1 LEFT OUTER JOIN " _
           &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
           &"RTEmployee LEFT OUTER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID RIGHT OUTER JOIN " _
           &"RTCust ON RTEmployee.EMPLY = RTCust.RCVDTLPRT ON  " _
           &"RTEmployee1.EMPLY = RTCust.FINCFMUSR " _
           &"WHERE rcvdtlno = '*' " 
 ' response.write "sql=" &sqldelete
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
 
  searchProg="RTRevCpS.asp"
  searchFirst=false
  If searchQry="" Then
     searchQry=" and RcvDtlNo <>'' "
     searchShow="已列印"
  End If   
  sqlList="SELECT RTCust.RCVDTLNO, RTCust.RCVDTLDAT, RTObj.CUSNC, COUNT(RTCust.CUSID), " _
         &"SUM(RTCust.ACTRCVAMT), RTCust.FINRDFMDAT, RTObj1.CUSNC " _
         &"FROM RTEmployee RTEmployee1 LEFT OUTER JOIN " _
         &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID RIGHT OUTER JOIN " _
         &"RTEmployee LEFT OUTER JOIN " _
         &"RTObj ON RTEmployee.CUSID = RTObj.CUSID RIGHT OUTER JOIN " _
         &"RTCust ON RTEmployee.EMPLY = RTCust.RCVDTLPRT ON  " _
         &"RTEmployee1.EMPLY = RTCust.FINCFMUSR " _
         &"WHERE rcvdtlno <> '' " & searchqry _
         &"group by RTCust.RCVDTLNO, RTCust.RCVDTLPRT, RTCust.RCVDTLDAT, " _
         &"RTCust.FINRDFMDAT, RTCust.FINCFMUSR, RTObj1.CUSNC, RTObj.CUSNC order by rcvdtlno desc "
' Response.Write "SQL=" & SQllist
End Sub
%>