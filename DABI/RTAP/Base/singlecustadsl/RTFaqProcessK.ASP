<!-- #include virtual="/WebUtilityV3/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="HI-Building �޲z�t��"
  title="����(�Ȥ�)�ȶD�B�z���I����"
  buttonName=" �s�W ; �R�� ; ���� ;���s��z;����;�\��ﶵ"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="�@�o;�@�o����"
  functionOptProgram="RTFaqProcessDropk.asp;RTFaqProcessDropRTNk.asp"
  functionOptPrompt="N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="�ץ�s��;����;�B�z���;�B�z���I;�B�z���u;�B�z�t��;�@�o���;�@�o�H��" 
  sqlDelete="SELECT RTFAQD1.CASENO AS Expr1, RTFAQD1.ENTRYNO AS Expr2, " _
           &"RTFAQD1.LOGDATE AS Expr3, left(RTFAQD1.LOGDESC,20), RTObj.CUSNC AS Expr4, RTObj1.SHORTNC AS Expr5, " _
           &"RTFAQD1.LOGDROPDATE AS Expr6, RTObj2.CUSNC AS Expr7 " _
           &"FROM RTFAQD1 INNER JOIN " _
           &"RTEmployee ON RTFAQD1.LOGUSR = RTEmployee.EMPLY INNER JOIN " _
           &"RTObj ON RTEmployee.CUSID = RTObj.CUSID INNER JOIN " _
           &"RTObj RTObj1 ON RTFAQD1.LOGFAC = RTObj1.CUSID INNER JOIN " _
           &"RTEmployee RTEmployee1 ON " _
           &"RTFAQD1.LOGDROPUSR = RTEmployee1.EMPLY INNER JOIN " _
           &"RTObj RTObj2 ON RTEmployee1.CUSID = RTObj2.CUSID " _
           &"WHERE RTFAQD1.CASENO='*' "
  dataTable="RTFAQD1"
  userDefineDelete=""
  numberOfKey=2
  dataProg="RTFAQProcessD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="�U�C��ƱN�Q�R���A�Ы��T�{�R�����A�Ϋ������C"
  diaButtonName=" �T�{�R�� ; ���� "
  goodMorning=FALSE
  goodMorningImage="cbbn.GIF"
  colSplit=1
  keyListPageSize=20
  searchProg="self"
  First=False
' When first time enter this keylist default query string to RTcmty.CASENO <> '*'
  searchFirst=False
  If searchQry="" Then
     searchQry=" RTFAQD1.CASENO='" & aryparmkey(0) & "'"
     searchShow="�ȶD�渹�G" & aryParmKey(0)     
   '  searchShow="����"
  End If
  sqlList="SELECT RTFAQD1.CASENO AS Expr1, RTFAQD1.ENTRYNO AS Expr2, " _
         &"RTFAQD1.LOGDATE AS Expr3, left(RTFAQD1.LOGDESC,20), RTObj.CUSNC AS Expr4, RTObj1.SHORTNC AS Expr5, " _
         &"RTFAQD1.LOGDROPDATE AS Expr6, RTObj2.CUSNC AS Expr7 " _
         &"FROM RTObj RTObj2 INNER JOIN " _
         &"RTEmployee RTEmployee1 ON  " _
         &"RTObj2.CUSID = RTEmployee1.CUSID RIGHT OUTER JOIN " _
         &"RTObj RTObj1 RIGHT OUTER JOIN " _
         &"RTFAQD1 ON RTObj1.CUSID = RTFAQD1.LOGFAC ON  " _
         &"RTEmployee1.EMPLY = RTFAQD1.LOGDROPUSR LEFT OUTER JOIN " _
         &"RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
         &"RTFAQD1.LOGUSR = RTEmployee.EMPLY " _
         &"WHERE RTFAQD1.CASENO<>'*' AND "&searchQry &" " _
         &"ORDER BY RTFAQD1.CASENO,RTFAQD1.ENTRYNO"
' Response.Write "SQL=" & SQLlist
End Sub

%>