<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="元訊寬頻網路股份有限公司"
  system="HI-Building 管理系統"
  title="社區(客戶)客訴狀況處理"
  buttonName=" 新增 ; 刪除 ; 結束 ;重新整理;頁數;功能選項"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="Y;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="作廢;作廢反轉;處理措施;結案;結案反轉;列印"
  functionOptProgram="RTFaqDropk.asp;RTFaqDropRTNk.asp;RTFaqProcessK.asp;RTFaqFinishUsrK.asp;RTFaqFinishRTNK.asp;RTFaQV.asp"
  functionOptPrompt="Y;Y;N;Y;Y;Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="案件編號;產品;社區;客戶名稱;受理時間;結案時間;作廢日期;處理時間;受理人員;排除人員;排除廠商 " 
  sqlDelete="SELECT RTFAQH.CASENO, RTFAQH.PRODUCT, RTCUSTADSLCmty.COMN, RTFAQH.FAQMAN, " _
         &"RTFAQH.RCVDATE, RTFAQH.FINISHDATE, RTFAQH.DROPDATE, RTObj.CUSNC, RTObj1.CUSNC, " _
         &"RTObj2.SHORTNC " _
         &"FROM RTCUSTADSLCmty RIGHT OUTER JOIN " _
         &"RTFAQH ON RTCUSTADSLCmty.CUTYID = RTFAQH.COMQ1 LEFT OUTER JOIN " _
         &"RTObj RTObj2 ON RTFAQH.FINISHFAC = RTObj2.CUSID LEFT OUTER JOIN " _
         &"RTObj RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
         &"RTFAQH.RCVUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"RTEmployee RTEmployee1 LEFT OUTER JOIN " _
         &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID ON " _
         &"RTFAQH.FINISHUSR = RTEmployee1.EMPLY " _
         &"WHERE RTFAQH.CASENO='*' "
  dataTable="RTFAQH"
  userDefineDelete=""
  numberOfKey=1
  dataProg="RTFAQD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="下列資料將被刪除，請按確認刪除之，或按取消。"
  diaButtonName=" 確認刪除 ; 取消 "
  goodMorning=FALSE
  goodMorningImage="cbbn.JPG"
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  parmkey=""
  searchFirst=False
' When first time enter this keylist default query string to RTcmty.CASENO <> '*'
  searchFirst=False
  If searchQry="" Then
     searchQry=" rtfaqh.cusid='" & aryparmkey(1) & "' and rtfaqh.comq1 = "&aryparmkey(0)
     searchShow=FrGetcustDesc(aryparmkey(0),aryparmkey(1),aryparmkey(2))     
   '  searchShow="全部"
  End If
  sqlList="SELECT RTFAQH.CASENO, RTFAQH.PRODUCT, RTCUSTADSLCmty.COMN, RTFAQH.FAQMAN, " _
         &"RTFAQH.RCVDATE, RTFAQH.FINISHDATE, RTFAQH.DROPDATE,CASE WHEN RTFAQH.RCVDATE IS NOT NULL AND RTFAQH.FINISHDATE IS " _
         &"NOT NULL THEN ltrim(str(Datediff(mi, RTFAQH.RCVDATE, RTFAQH.FINISHDATE)/1440))+'日'+ " _
         &"ltrim(str((Datediff(mi,  RTFAQH.RCVDATE, RTFAQH.FINISHDATE)%1440)/60))+'時'+  " _
         &"ltrim(str(Datediff(mi,  RTFAQH.RCVDATE, RTFAQH.FINISHDATE)%60))+'分' " _
         &"ELSE '' END, RTObj.CUSNC, RTObj1.CUSNC, " _
         &"RTObj2.SHORTNC " _
         &"FROM RTCUSTADSLCmty RIGHT OUTER JOIN " _
         &"RTFAQH ON RTCUSTADSLCmty.CUTYID = RTFAQH.COMQ1 LEFT OUTER JOIN " _
         &"RTObj RTObj2 ON RTFAQH.FINISHFAC = RTObj2.CUSID LEFT OUTER JOIN " _
         &"RTObj RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
         &"RTFAQH.RCVUSR = RTEmployee.EMPLY LEFT OUTER JOIN " _
         &"RTEmployee RTEmployee1 LEFT OUTER JOIN " _
         &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID ON " _
         &"RTFAQH.FINISHUSR = RTEmployee1.EMPLY " _
         &"WHERE RTFAQH.CASENO<>'*' and " &searchQry &" " _
         &"ORDER BY RTFAQH.CASENO desc"
  session("comq1")=aryparmkey(0)
  session("cusid")=aryparmkey(1)
  session("entryno")=aryparmkey(2)  
'Response.Write "SQL=" & SQLlist
End Sub

%>
<!-- #include file="RTGetcustDesc.inc" -->