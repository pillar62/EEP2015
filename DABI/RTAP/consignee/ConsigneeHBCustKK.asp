<!-- #include virtual="/WebUtilityV4/DBAUDI/zzKeyList.inc" -->
<%
if not Session("passed") then
   Response.Redirect "http://www.cbbn.com.tw/Consignee/logon.asp"
end if

Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="HI-Building客戶基本資料維護"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  'V=split(SrAccessPermit,";")
  'AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  'ButtonEnable=V(0) & ";" & V(2) & ";Y;Y;Y;" & V(3)
  buttonEnable="N;N;Y;Y;Y;N"
  'functionOptName="通知技術;撤銷通知;客訴處理"
  'functionOptProgram="RTSndInfo.asp;RTDropInfo.asp;RTFaqK.ASP"
  'If V(1)="Y" then
  '   accessMode="U"
  'Else
  '   accessMode="I"
  'End IF
  DSN="DSN=RTLib"
  formatName="none;none;社區名稱;客戶;HN號碼;完工日;報竣日;退租日;裝機地址;聯絡電話"
   sqlDelete="SELECT rtcust.CUSID, rtcust.ENTRYNO,  RTCmty.comn, "_
		 &"RTObj_1.SHORTNC, rtcust.cusno, " _
         &"rtcust.finishdat,rtcust.Docketdat, " _
         &"rtcust.dropdat, " _         
         &"RTCOUNTY.CUTNC + rtcust.TOWNSHIP2 + rtcust.RADDR2, " _         
         &"rtcust.HOME " _ 
         &"FROM rtcust LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON rtcust.CUSID = RTObj_1.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON rtcust.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode_1 ON rtcust.SETTYPE = RTCode1.CODE AND " _
         &"RTCode1.KIND = 'A7' LEFT OUTER JOIN " _
         &"RTCmty ON rtcust.COMQ1 = RTCmty.comq1 " _
         &" WHERE  rtcust.CUSID='*' "
  
  dataTable="RTcust"
  userDefineDelete=""
  extTable=""
  numberOfKey=2
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=700,height=460,scrollbars=yes"
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
  searchProg="ConsigneeHBCustSS.asp"
  searchFirst=false
  If searchQry="" Then
     searchShow="全部"
     searchQry=" and rtcust.CUSID<>'*' and RTcmty.CoMTYPE ='" &Session("COMTYPE")& "' "
  ELSE
     SEARCHFIRST=FALSE
  End If  
  sqllist="SELECT rtcust.CUSID, rtcust.ENTRYNO,  RTCmty.comn, "_
		 &"RTObj_1.SHORTNC, rtcust.cusno, " _
         &"rtcust.finishdat,rtcust.Docketdat, " _
         &"rtcust.dropdat, " _         
         &"RTCOUNTY.CUTNC + rtcust.TOWNSHIP2 + rtcust.RADDR2, " _         
         &"rtcust.HOME " _ 
         &"FROM rtcust LEFT OUTER JOIN " _
         &"RTObj RTObj_1 ON rtcust.CUSID = RTObj_1.CUSID LEFT OUTER JOIN " _
         &"RTCounty ON rtcust.CUTID2 = RTCounty.CUTID LEFT OUTER JOIN " _
         &"RTCode ON rtcust.SETTYPE = RTCode.CODE AND " _
         &"RTCode.KIND = 'A7' LEFT OUTER JOIN " _
         &"RTCmty ON rtcust.COMQ1 = RTCmty.comq1 " _
         &" WHERE  rtcust.CUSID<>'*' and RTcmty.CoMTYPE ='" &Session("COMTYPE") & "' " & searchqry & " " _
         &"ORDER BY rtcounty.CUTNC, rtcust.TOWNSHIP2, rtcust.RADDR2,rtobj_1.shortnc "
   'END IF
 'Response.Write "sql=" & SQLLIST
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>
