<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="�F��AVS�޲z�t��"
  title="AVS�Τ�_���u�沧�ʸ�Ƭd��"
  buttonName=" �s�W ; �R�� ; ���� ;���s��z;����;�\��ﶵ"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;none;none;none;�D�u;�_���u��;����;���ʤ��;�������O;���ʤH��;���u��;�@�o��;���ʭ�];���u����;�����u����;�����p��~��"
  sqlDelete="SELECT  RTEBTCUSTm2RTNSNDWORKLOG.avsno, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.m2m3,RTEBTCUSTm2RTNSNDWORKLOG.seq,RTEBTCUSTm2RTNSNDWORKLOG.RTNNO, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.ENTRYNO, rtrim(convert(char(6),RTEBTCUST.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUST.lineQ1))  as comqline," _
           &"RTEBTCUSTm2RTNSNDWORKLOG.RTNNO,RTEBTCUSTm2RTNSNDWORKLOG.ENTRYNO,RTEBTCUSTm2RTNSNDWORKLOG.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.SENDWORKDAT, RTEBTCUSTm2RTNSNDWORKLOG.DROPDAT, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.DROPDESC, RTEBTCUSTm2RTNSNDWORKLOG.CLOSEDAT, RTEBTCUSTm2RTNSNDWORKLOG.unCLOSEDAT, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.BONUSCLOSEYM FROM RTCode RIGHT OUTER JOIN " _
           &"RTEBTCUSTm2RTNSNDWORKLOG ON RTCode.CODE = RTEBTCUSTm2RTNSNDWORKLOG.CHGCODE " _
           &"AND RTCode.KIND = 'G2' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.CHGUSR = RTEmployee.EMPLY " _
           &"LEFT OUTER  JOIN RTEBTCUST ON RTEBTCUSTm2RTNSNDWORKLOG.AVSNO = RTEBTCUST.AVSNO " _
           &"LEFT OUTER JOIN  RTEBTCMTYH ON RTEBTCUST.COMQ1 = RTEBTCMTYH.COMQ1 " _
           &"where RTEBTCUSTm2RTNSNDWORKLOG.COMQ1=0"
  dataTable="RTEBTCUSTm2RTNSNDWORKLOG"
  userDefineDelete="Yes"
  numberOfKey=5
  dataProg="None"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
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
  searchProg="self"
' Open search program when first entry this keylist
'  If searchQry="" Then
'     searchFirst=True
'     searchQry=" RTCmty.ComQ1=0 "
'     searchShow=""
'  Else
'     searchFirst=False
'  End If
' When first time enter this keylist default query string to RTcmty.ComQ1 <> 0
  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTEBTCUSTm2RTNSNDWORKLOG.avsno='" & aryparmkey(0) & "' and RTEBTCUSTm2RTNSNDWORKLOG.m2m3='" & aryparmkey(1) & "' and RTEBTCUSTm2RTNSNDWORKLOG.seq=" & aryparmkey(2) & " and RTEBTCUSTm2RTNSNDWORKLOG.RTNNO='" & aryparmkey(3) & "' "
     searchShow="����"
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
  if userlevel=31  then DAreaID="<>'*'"
  
         sqlList="SELECT  RTEBTCUSTm2RTNSNDWORKLOG.avsno, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.m2m3,RTEBTCUSTm2RTNSNDWORKLOG.seq,RTEBTCUSTm2RTNSNDWORKLOG.RTNNO, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.ENTRYNO, rtrim(convert(char(6),RTEBTCUST.COMQ1)) +'-'+ rtrim(convert(char(6),RTEBTCUST.lineQ1))  as comqline, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.RTNNO,RTEBTCUSTm2RTNSNDWORKLOG.ENTRYNO,RTEBTCUSTm2RTNSNDWORKLOG.CHGDAT, RTCode.CODENC, RTObj.CUSNC, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.SENDWORKDAT, RTEBTCUSTm2RTNSNDWORKLOG.DROPDAT, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.DROPDESC, RTEBTCUSTm2RTNSNDWORKLOG.CLOSEDAT, RTEBTCUSTm2RTNSNDWORKLOG.unCLOSEDAT, " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.BONUSCLOSEYM FROM RTCode RIGHT OUTER JOIN " _
           &"RTEBTCUSTm2RTNSNDWORKLOG ON RTCode.CODE = RTEBTCUSTm2RTNSNDWORKLOG.CHGCODE " _
           &"AND RTCode.KIND = 'G2' LEFT OUTER JOIN RTObj INNER JOIN " _
           &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON " _
           &"RTEBTCUSTm2RTNSNDWORKLOG.CHGUSR = RTEmployee.EMPLY " _
           &"LEFT OUTER  JOIN RTEBTCUST ON RTEBTCUSTm2RTNSNDWORKLOG.AVSNO = RTEBTCUST.AVSNO " _
           &"LEFT OUTER JOIN  RTEBTCMTYH ON RTEBTCUST.COMQ1 = RTEBTCMTYH.COMQ1 " _
           &"where " & searchqry

  'end if
  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>