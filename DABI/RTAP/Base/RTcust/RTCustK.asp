<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="客戶資料查詢"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;" & V(3)
  'buttonEnable="Y;Y;Y;Y;Y;N"
  functionOptName="發包記錄;客訴處理"
  functionOptProgram="/webap/rtap/base/rtsendwork/RTSendWorkHisK.ASP;/webap/rtap/base/rtCMTY/RTFAQK.ASP"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;營運點;社區名稱;客戶名稱;單次;申請日;完工日;報竣日;欠退日;欠拆;聯絡電話;公司電話;安裝員類別;同意書編號"
 ' sqlDelete="SELECT RTCust.COMQ1,RTCust.CUSID, RTCust.ENTRYNO, RTObj.shortnc, RTCust.CUSTYPE, " _
 '          &"RTCust.LINETYPE, RTCust.RCVD, RTCust.HOME," _
 '          &"RTCust.OFFICE + ' ' + RTCust.EXTENSION  AS Office,RTCust.SNDINFODAT ,rtcust.reqdat " _
 '          &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " _
 '          &"WHERE RTCust.COMQ1=0 " _
 '          &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO "
   sqlDelete="SELECT RTCust.COMQ1, RTCust.CUSID, RTCust.ENTRYNO,rtcmty.comn, RTObj.cusNC, " _
         &"RTCust.ENTRYNO, RTCust.RCVD,rtcust.finishdat,rtcust.docketdat,rtcust.dropdat, RTCust.OVERDUE, RTCust.HOME, " _
         &"RTCust.OFFICE + ' ' + RTCust.EXTENSION AS Office, RTCode.CODENC,rtcust.consentno " _
         &"FROM RTCounty RIGHT OUTER JOIN RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID ON " _
         &"RTCounty.CUTID = RTCust.CUTID2 LEFT OUTER JOIN " _
         &"RTAreaCty LEFT OUTER JOIN RTArea ON RTAreaCty.AREAID = RTArea.AREAID RIGHT OUTER JOIN " _
         &"RTCmty ON RTAreaCty.CUTID = RTCmty.CUTID ON RTCust.COMQ1 = RTCmty.COMQ1 LEFT OUTER JOIN " _
         &"RTCode ON RTCust.SETTYPE = RTCode.CODE " _
         &"WHERE RTCust.COMQ1=0 AND (RTCode.KIND = 'A7') " _
         &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO " 
  dataTable="RTCust"
  userDefineDelete="Yes"
  extTable=""
  numberOfKey=3
  dataProg="/webap/rtap/base/rtcmty/RTCustD.asp"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=True
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="rtcusts.asp"
  searchFirst=TRUE
  If searchQry="" Then
     searchQry=" RTCust.CUSID='*' "
     searchShow="全部"
  ELSE
     searchFirst=False
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'userlevel=2:為業務工程師==>只能看所屬社區資料
  'DOMAIN:'T','C','K'北中南轄區人員(客服,技術)只能看所屬轄區資料
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="='A1'"
         case "P"
            DAreaID="='A1'"            
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="='*'"
  end select
  '高階主管可讀取全部資料
  if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
     Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T92134" then
     DAreaID="<>'*'"
  end if
  '資訊部管理員可讀取全部資料
  if userlevel=31 then DAreaID="<>'*'"  
  'sqlList="SELECT RTCust.COMQ1,RTCust.CUSID, RTCust.ENTRYNO, RTObj.shortnc, RTCust.CUSTYPE, " _
  '         &"RTCust.LINETYPE, RTCust.RCVD, RTCust.HOME, " _
  '         &"RTCust.OFFICE+' '+ RTCust.EXTENSION  AS Office,RTCust.SNDINFODAT,rtcust.reqdat " _
  '         &"FROM RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID " _
  '         &"WHERE " &searchQry &" " _
  '         &"ORDER BY RTCust.CUSID, RTCust.ENTRYNO "
  sqllist="SELECT RTCust.COMQ1, RTCust.CUSID, RTCust.ENTRYNO," _
         &"CASE WHEN RTCODE_A.PARM1 = 'AA' THEN CASE WHEN RTCTYTOWN.OPERATIONNAME <> '' THEN " _
         &"RTCTYTOWN.OPERATIONNAME ELSE '無法歸屬' END ELSE CASE WHEN RTCODE_A.CODENC = '二煒' THEN " _
         &"'第十營運點' ELSE RTCODE_A.CODENC END END," _ 
         &"rtcmty.comn, RTObj.cusNC, " _
         &"RTCust.ENTRYNO, RTCust.RCVD,rtcust.finishdat,rtcust.docketdat,rtcust.dropdat, RTCust.OVERDUE,RTCust.HOME, " _
         &"RTCust.OFFICE + ' ' + RTCust.EXTENSION AS Office,RTCode.CODENC,rtcust.consentno " _
         &"FROM RTCounty RIGHT OUTER JOIN RTCust INNER JOIN RTObj ON RTCust.CUSID = RTObj.CUSID ON " _
         &"RTCounty.CUTID = RTCust.CUTID2 LEFT OUTER JOIN " _
         &"RTAreaCty LEFT OUTER JOIN RTArea ON RTAreaCty.AREAID = RTArea.AREAID RIGHT OUTER JOIN " _
         &"RTCmty ON RTAreaCty.CUTID = RTCmty.CUTID ON RTCust.COMQ1 = RTCmty.COMQ1 LEFT OUTER JOIN " _
         &"RTCode ON RTCust.SETTYPE = RTCode.CODE AND RTCode.KIND = 'A7' " _
         &"LEFT OUTER JOIN RTCODE RTCODE_A ON RTCMTY.COMTYPE=RTCODE_A.CODE AND RTCODE_A.KIND='B3' " _
         &"LEFT OUTER JOIN RTCtyTown ON RTCust.CUTID1 = RTCtyTown.CUTID AND " _
         &"RTCust.TOWNSHIP1 = RTCtyTown.TOWNSHIP " _
         &"WHERE " & searchqry & " " _
         &"ORDER BY rtobj.cusnc,rtcmty.comn, RTCust.ENTRYNO "
 ' Response.Write "sql=" & SQLLIST
End Sub
%>
