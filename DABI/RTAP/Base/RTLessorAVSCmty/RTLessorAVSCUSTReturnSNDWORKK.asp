<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="AVS-City�޲z�t��"
  title="AVS-City�Τ�_�����ڬ��u���ƺ��@"
  buttonName=" �s�W ; �R�� ; ���� ;���s��z;����;�\��ﶵ"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="���~��γ�; �C �L ;���u����;�����u����;���ת���; �@ �o ;�@�o����;�]�Ʃ���;���v����"
  functionOptProgram="RTLessorAVSCustReturnHardwareRCVK.ASP;RTLessorAVSCustReturnSNDWORKPV.asp;RTLessorAVSCustReturnsndworkF.asp;RTLessorAVSCustReturnsndworkUF.asp;RTLessorAVSCustReturnsndworkFR.asp;RTLessorAVSCustReturnSNDWORKdrop.asp;RTLessorAVSCustReturnSNDWORKdropc.asp;RTLessorAVSCustReturnhardwareK.asp;RTLessorAVSCustReturnSNDWORKLOGK.asp"
  functionOptPrompt="N;N;Y;Y;Y;Y;Y;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;�D�u;���u�渹;���u���;�C�L�H��;�w�w�I�u��;��ڬI�u��;���פ�;�����u���פ�;none;none;�����b�ڽs��;none;none;�@�o��;�]�Ƽƶq;���γ�ƶq;�w��ƶq;�ݻ�ƶq"
  sqlDelete="SELECT RTLessorAVSCustReturnSNDWORK.CUSID, RTLessorAVSCustReturnSNDWORK.ENTRYNO, RTLessorAVSCustReturnSNDWORK.PRTNO,rtrim(convert(char(6),RTLessorAVScust.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVScust.lineQ1))  as comqline, RTLessorAVSCustReturnSNDWORK.PRTNO, RTLessorAVSCustReturnSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, " _
           &"RTLessorAVSCustReturnSNDWORK.closedat,RTLessorAVSCustReturnSNDWORK.unclosedat,RTLessorAVSCustReturnSNDWORK.BONUSCLOSEYM, RTLessorAVSCustReturnSNDWORK.BONUSFINCHK,RTLessorAVSCustReturnSNDWORK.batchno, RTLessorAVSCustReturnSNDWORK.STOCKCLOSEYM, RTLessorAVSCustReturnSNDWORK.STOCKFINCHK, " _
           &"RTLessorAVSCustReturnSNDWORK.DROPDAT FROM RTLessorAVSCustReturnSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTLessorAVSCustReturnSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON RTLessorAVSCustReturnSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTLessorAVSCustReturnSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorAVSCustReturnSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTLessorAVSCustReturnSNDWORK.PRTUSR = RTEmployee.EMPLY left outer join RTLessorAVScmtyline on " _
           &"RTLessorAVSCustReturnSNDWORK.cusid=RTLessorAVScust.cusid where RTLessorAVSCustReturnSNDWORK.cusid=''" 
  dataTable="RTLessorAVSCustReturnSNDWORK"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg="RTLessorAVSCustReturnSNDWORKD.asp"
  datawindowFeature=""
  searchWindowFeature="width=350,height=160,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="�U�C��ƱN�Q�R���A�Ы��T�{�R�����A�Ϋ������C"
  diaButtonName=" �T�{�R�� ; ���� "
  goodMorning=false
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTLessorAVSCustReturnSNDWORKs.asp"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  set connXX=server.CreateObject("ADODB.connection")
  set rsXX=server.CreateObject("ADODB.recordset")
  dsnxx="DSN=XXLIB"
  sqlxx="select * from usergroup where userid='" & Request.ServerVariables("LOGON_USER") & "'"
  connxx.Open dsnxx
  rsxx.Open sqlxx,connxx
  if not rsxx.EOF then
     usergroup=rsxx("group")
  else
     usergroup=""
  end if
  rsxx.Close
  connxx.Close
  set rsxx=nothing
  set connxx=nothing
  '----
   set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyH ON " _
       &"RTCounty.CUTID = RTLessorAVSCmtyH.CUTID RIGHT OUTER JOIN RTLessorAVSCust ON RTLessorAVSCmtyH.COMQ1 = RTLessorAVSCust.COMQ1 " _
       &"where RTLessorAVSCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTLessorAVSCmtyLine ON  " _
       &"RTCounty.CUTID = RTLessorAVSCmtyLine.CUTID RIGHT OUTER JOIN " _
       &"RTLessorAVSCust ON RTLessorAVSCmtyLine.COMQ1 = RTLessorAVSCust.COMQ1 AND " _
       &"RTLessorAVSCmtyLine.LINEQ1 = RTLessorAVSCust.LINEQ1 " _
       &"where RTLessorAVSCust.cusid='" & ARYPARMKEY(0) & "'"
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township")
     if rsyy("village") <> "" then
         COMaddr= COMaddr & rsyy("village") & rsyy("cod1")
     end if
     if rsyy("NEIGHBOR") <> "" then
         COMaddr= COMaddr & rsyy("NEIGHBOR") & rsyy("cod2")
     end if
     if rsyy("STREET") <> "" then
         COMaddr= COMaddr & rsyy("STREET") & rsyy("cod3")
     end if
     if rsyy("SEC") <> "" then
         COMaddr= COMaddr & rsyy("SEC") & rsyy("cod4")
     end if
     if rsyy("LANE") <> "" then
         COMaddr= COMaddr & rsyy("LANE") & rsyy("cod5")
     end if
     if rsyy("ALLEYWAY") <> "" then
         COMaddr= COMaddr & rsyy("ALLEYWAY") & rsyy("cod7")
     end if
     if rsyy("NUM") <> "" then
         COMaddr= COMaddr & rsyy("NUM") & rsyy("cod8")
     end if
     if rsyy("FLOOR") <> "" then
         COMaddr= COMaddr & rsyy("FLOOR") & rsyy("cod9")
     end if
     if rsyy("ROOM") <> "" then
         COMaddr= COMaddr & rsyy("ROOM") & rsyy("cod10")
     end if
  else
     COMaddr=""
  end if
  RSYY.Close
  sqlYY="select * from RTLessorAVSCUST  where CUSID='" & ARYPARMKEY(0) & "' "
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     CUSNC=rsYY("CUSNC")
     comq1xx=rsyy("comq1")
     lineq1xx=rsyy("lineq1")
  else
     CUSNC=""
     comq1xx=""
     lineq1xx=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorAVSCustReturnSNDWORK.cusid='" & aryparmkey(0) & "' and RTLessorAVSCustReturnSNDWORK.entryno=" & aryparmkey(1) & " and RTLessorAVSCustReturnSNDWORK.dropdat is null and RTLessorAVSCustReturnSNDWORK.unclosedat is null "
     searchShow="�D�u�Ǹ��J"& comq1xx &"-" & lineq1xx & ",���ϦW�١J" & COMN & ",�Τ�W�١J" & cusnc & ",�D�u��}�J" & COMADDR
  ELSE
     SEARCHFIRST=FALSE
  End If
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  'Ū���n�J�b�����s�ո��
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:���~�Ȥu�{�v==>�u��ݩ��ݪ��ϸ��
  'DOMAIN:'T','C','K'�_���n�ҰϤH��(�ȪA,�޳N)�u��ݩ����Ұϸ��
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  Domain=Mid(Emply,1,1)
  select case Domain
         case "T"
            DAreaID="<>'*'"
         case "P"
            DAreaID="='A1'"                        
         case "C"
            DAreaID="='A2'"         
         case "K"
            DAreaID="='A3'"         
         case else
            DareaID="=''"
  end select
  '�����D�ޥiŪ���������
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T90076" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '��T���޲z���iŪ���������
  'if userlevel=31 then DAreaID="<>'*'"
  
  '�ѩ�����q�h�a�|���ӽШ�u���A�G�ȪA���}��Ҧ��ϰ��v���A�@�����x�_�ȪA�B�z
  if userlevel=31 then DAreaID="<>'*'"
 
         sqlList="SELECT RTLessorAVSCustReturnSNDWORK.CUSID, RTLessorAVSCustReturnSNDWORK.ENTRYNO, RTLessorAVSCustReturnSNDWORK.PRTNO,rtrim(convert(char(6),RTLessorAVScust.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorAVScust.lineQ1))  as comqline, RTLessorAVSCustReturnSNDWORK.PRTNO, RTLessorAVSCustReturnSNDWORK.SENDWORKDAT, " _
           &"RTOBJ.CUSNC,CASE WHEN RTOBJ_2.SHORTNC <>'' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END,CASE WHEN RTOBJ_4.SHORTNC <>'' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, " _
           &"RTLessorAVSCustReturnSNDWORK.closedat,RTLessorAVSCustReturnSNDWORK.unclosedat,RTLessorAVSCustReturnSNDWORK.BONUSCLOSEYM, RTLessorAVSCustReturnSNDWORK.BONUSFINCHK,RTLessorAVSCustReturnSNDWORK.batchno, RTLessorAVSCustReturnSNDWORK.STOCKCLOSEYM, RTLessorAVSCustReturnSNDWORK.STOCKFINCHK, " _
           &"RTLessorAVSCustReturnSNDWORK.DROPDAT ,SUM(CASE WHEN RTLessorAVSCustReturnHARDWARE.dropdat IS NULL AND RTLessorAVSCustReturnHARDWARE.QTY > 0 " _
           &"THEN RTLessorAVSCustReturnHARDWARE.QTY ELSE 0 END), SUM(CASE WHEN RTLessorAVSCustReturnHARDWARE.dropdat IS NULL AND " _
           &"RCVPRTNO <> '' THEN RTLessorAVSCustReturnHARDWARE.QTY ELSE 0 END), SUM(CASE WHEN RTLessorAVSCustReturnHARDWARE.dropdat IS NULL " _
           &"AND RCVPRTNO <> '' AND RTLessorAVSCustReturnHARDWARE.rcvfinishdat IS NOT NULL THEN RTLessorAVSCustReturnHARDWARE.QTY ELSE 0 END), " _
           &"SUM(CASE WHEN RTLessorAVSCustReturnHARDWARE.dropdat IS NULL AND RTLessorAVSCustReturnHARDWARE.QTY > 0 THEN RTLessorAVSCustReturnHARDWARE.QTY ELSE 0 END) - " _
           &"SUM(CASE WHEN RTLessorAVSCustReturnHARDWARE.dropdat IS NULL AND RCVPRTNO <> '' AND RTLessorAVSCustReturnHARDWARE.rcvfinishdat IS NOT NULL THEN RTLessorAVSCustReturnHARDWARE.QTY ELSE 0 END) " _
           &"FROM RTLessorAVSCustReturnSNDWORK LEFT OUTER JOIN RTObj RTObj_4 ON RTLessorAVSCustReturnSNDWORK.REALCONSIGNEE = RTObj_4.CUSID LEFT OUTER JOIN " _
           &"RTEmployee RTEmployee_2 INNER JOIN RTObj RTObj_3 ON RTEmployee_2.CUSID = RTObj_3.CUSID ON RTLessorAVSCustReturnSNDWORK.REALENGINEER = RTEmployee_2.EMPLY LEFT OUTER JOIN " _
           &"RTObj RTObj_2 ON RTLessorAVSCustReturnSNDWORK.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTEmployee RTEmployee_1 INNER JOIN " _
           &"RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON RTLessorAVSCustReturnSNDWORK.ASSIGNENGINEER = RTEmployee_1.EMPLY LEFT OUTER JOIN " _
           &"RTObj INNER JOIN RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON RTLessorAVSCustReturnSNDWORK.PRTUSR = RTEmployee.EMPLY left outer join RTLessorAVScust on " _
           &"RTLessorAVSCustReturnSNDWORK.cusid=RTLessorAVScust.cusid LEFT OUTER JOIN " _
           &"RTLessorAVSCustReturnHARDWARE ON RTLessorAVSCustReturnSNDWORK.cusid = RTLessorAVSCustReturnHARDWARE.CUSID AND " _
           &"RTLessorAVSCustReturnSNDWORK.PRTNO = RTLessorAVSCustReturnHARDWARE.PRTNO " _
            &"where  RTLessorAVSCustReturnSNDWORK.cusid='" & aryparmkey(0) & "' and RTLessorAVSCustReturnSNDWORK.entryno=" & aryparmkey(1) & " and " & searchqry & " " _
           &"GROUP BY  RTLessorAVSCustReturnSNDWORK.CUSID, RTLessorAVSCustReturnSNDWORK.ENTRYNO, RTLessorAVSCustReturnSNDWORK.PRTNO, " _
           &"rtrim(CONVERT(char(6), RTLessorAVScust.COMQ1)) + '-' + rtrim(CONVERT(char(6), RTLessorAVScust.lineQ1)), " _
           &"RTLessorAVSCustReturnSNDWORK.PRTNO, RTLessorAVSCustReturnSNDWORK.SENDWORKDAT, RTOBJ.CUSNC, " _
           &"CASE WHEN RTOBJ_2.SHORTNC <> '' THEN RTOBJ_2.SHORTNC ELSE RTOBJ_1.CUSNC END, " _
           &"CASE WHEN RTOBJ_4.SHORTNC <> '' THEN RTOBJ_4.SHORTNC ELSE RTOBJ_3.CUSNC END, RTLessorAVSCustReturnSNDWORK.closedat, " _
           &"RTLessorAVSCustReturnSNDWORK.unclosedat, RTLessorAVSCustReturnSNDWORK.BONUSCLOSEYM, RTLessorAVSCustReturnSNDWORK.BONUSFINCHK, " _
           &"RTLessorAVSCustReturnSNDWORK.batchno, RTLessorAVSCustReturnSNDWORK.STOCKCLOSEYM, RTLessorAVSCustReturnSNDWORK.STOCKFINCHK, " _
           &"RTLessorAVSCustReturnSNDWORK.DROPDAT "
 
  'end if
'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>