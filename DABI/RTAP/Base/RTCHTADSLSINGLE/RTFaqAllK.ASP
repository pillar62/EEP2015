<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="HI-Building �޲z�t��"
  title="�ȶD�ץ�l�ܧ@�~"
  buttonName=" �s�W ; �R�� ; ���� ;���s��z;����;�\��ﶵ"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="�@�o;�@�o����;�B�z���I;����;���פ���;�C�L"
  functionOptProgram="RTFaqDropk.asp;RTFaqDropRTNk.asp;RTFaqProcessK.asp;RTFaqFinishUsrK.asp;RTFaqFinishRTNK.asp;RTFaQV.asp"
  functionOptPrompt="Y;Y;N;Y;Y;Y"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="�ץ�s��;���~;����;�Ȥ�W��;RT���u��;���z�ɶ�;�t�Z�Ѽ�;���׮ɶ�;�@�o���;�B�z�ɶ�;���z�H��;�ư��H��;�ư��t�� " 
  sqlDelete="SELECT RTFAQH.CASENO,RTFAQH.PRODUCT, RTCMTY.COMN, RTFAQH.faqman,RTCUST.Finishdat, RTFAQH.RCVDATE, " _
         &"CASE WHEN RTCUST.FINISHDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL THEN ltrim(str(Datediff(mi, RTCUST.FINISHDAT, RTFAQH.RCVDATE)/1440))+'��'+ " _
         &"ltrim(str((Datediff(mi, RTCUST.FINISHDAT,  RTFAQH.RCVDATE)%1440)/60))+'��'+  " _
         &"ltrim(str(Datediff(mi, RTCUST.FINISHDAT,  RTFAQH.RCVDATE)%60))+'��' " _
         &"ELSE '' END, " _ 
         &"RTFAQH.FINISHDATE,RTFAQH.DROPDATE,CASE WHEN RTFAQH.RCVDATE IS NOT NULL AND RTFAQH.FINISHDATE IS " _
         &"NOT NULL THEN ltrim(str(Datediff(mi, RTFAQH.RCVDATE, RTFAQH.FINISHDATE)/1440))+'��'+ " _
         &"ltrim(str((Datediff(mi,  RTFAQH.RCVDATE, RTFAQH.FINISHDATE)%1440)/60))+'��'+  " _
         &"ltrim(str(Datediff(mi,  RTFAQH.RCVDATE, RTFAQH.FINISHDATE)%60))+'��' " _
         &"ELSE '' END, " _
         &"RTOBJ.CUSNC, RTOBJ1.CUSNC,RTOBJ2.SHORTNC " _
         &"FROM RTCmty RIGHT OUTER JOIN " _
         &"RTFAQH ON RTCmty.COMQ1 = RTFAQH.COMQ1 LEFT OUTER JOIN " _
         &"RTCust ON RTFAQH.CUSID = RTCust.CUSID AND " _
         &"RTFAQH.ENTRYNO = RTCust.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj RTObj2 ON RTFAQH.FINISHFAC = RTObj2.CUSID LEFT OUTER JOIN " _
         &"RTEmployee RTEmployee1 LEFT OUTER JOIN " _
         &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID ON " _
         &"RTFAQH.FINISHUSR = RTEmployee1.EMPLY LEFT OUTER JOIN " _
         &"RTObj RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTFAQH.RCVUSR = RTEmployee.EMPLY " _
         &"WHERE RTFAQH.CASENO='*' "
  dataTable="RTFAQH"
  userDefineDelete=""
  numberOfKey=1
  dataProg="RTFAQD.asp"
  datawindowFeature=""
  searchWindowFeature="width=400,height=300,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="�U�C��ƱN�Q�R���A�Ы��T�{�R�����A�Ϋ������C"
  diaButtonName=" �T�{�R�� ; ���� "
  goodMorning=true
  goodMorningImage="cbbn.JPG"
  colSplit=1
  keyListPageSize=20
  searchProg="RTFaQAllS.asp"
  parmkey=""
First=False
' When first time enter this keylist default query string to RTcmty.CASENO <> '*'
  searchFirst=False
  If searchQry="" Then
     searchQry=" RTFAQH.caseno<>'*' and RTFAQH.Finishdate is null and RTFAQH.DropDate Is Null " 
     searchShow="�����G�����סA���@�o���"
  End If
  sqlList="SELECT RTFAQH.CASENO,RTFAQH.PRODUCT, RTCMTY.COMN, RTFAQH.faqman,RTCUST.Finishdat, RTFAQH.RCVDATE, " _
         &"CASE WHEN RTCUST.FINISHDAT IS NOT NULL AND RTFAQH.RCVDATE IS NOT NULL THEN ltrim(str(Datediff(mi, RTCUST.FINISHDAT, RTFAQH.RCVDATE)/1440))+'��'+ " _
         &"ltrim(str((Datediff(mi, RTCUST.FINISHDAT,  RTFAQH.RCVDATE)%1440)/60))+'��'+  " _
         &"ltrim(str(Datediff(mi, RTCUST.FINISHDAT,  RTFAQH.RCVDATE)%60))+'��' " _
         &"ELSE '' END, " _ 
         &"RTFAQH.FINISHDATE,RTFAQH.DROPDATE,CASE WHEN RTFAQH.RCVDATE IS NOT NULL AND RTFAQH.FINISHDATE IS " _
         &"NOT NULL THEN ltrim(str(Datediff(mi, RTFAQH.RCVDATE, RTFAQH.FINISHDATE)/1440))+'��'+ " _
         &"ltrim(str((Datediff(mi,  RTFAQH.RCVDATE, RTFAQH.FINISHDATE)%1440)/60))+'��'+  " _
         &"ltrim(str(Datediff(mi,  RTFAQH.RCVDATE, RTFAQH.FINISHDATE)%60))+'��' " _
         &"ELSE '' END, " _
         &"RTOBJ.CUSNC, RTOBJ1.CUSNC,RTOBJ2.SHORTNC " _
         &"FROM RTCmty RIGHT OUTER JOIN " _
         &"RTFAQH ON RTCmty.COMQ1 = RTFAQH.COMQ1 LEFT OUTER JOIN " _
         &"RTCust ON RTFAQH.CUSID = RTCust.CUSID AND " _
         &"RTFAQH.ENTRYNO = RTCust.ENTRYNO LEFT OUTER JOIN " _
         &"RTObj RTObj2 ON RTFAQH.FINISHFAC = RTObj2.CUSID LEFT OUTER JOIN " _
         &"RTEmployee RTEmployee1 LEFT OUTER JOIN " _
         &"RTObj RTObj1 ON RTEmployee1.CUSID = RTObj1.CUSID ON " _
         &"RTFAQH.FINISHUSR = RTEmployee1.EMPLY LEFT OUTER JOIN " _
         &"RTObj RIGHT OUTER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
         &"RTFAQH.RCVUSR = RTEmployee.EMPLY " _
         &"WHERE RTFAQH.CASENO<>'*' and " &searchQry &" " _
         &"ORDER BY RTFAQH.CASENO desc"
' Response.Write "SQL=" & SQLlist
End Sub

%>