<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="ET-City�޲z�t��"
  title="ET-City�Τ�]�ƪ��~����沧�ʸ�Ƭd��"
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
  formatName="none;����;���ʤ�;�������O;���ʤH��;�������O;����ӽФ�;����ӽФH;��ڲ���H;���൲�פ�;���פH��;�@�o��;�@�o�H��;���u�渹;�ಾ���H��;����ƶq"
  sqlDelete="SELECT  RTLessorCustRTNHardwareLOG.RCVPRTNO AS RCVPRTNO, " _
                   &"        RTLessorCustRTNHardwareLOG.entryno, " _
                      &"     RTLessorCustRTNHardwareLOG.chgdat, RTCODE.CODENC, RTOBJ_10.CUSNC, " _
                         &"  CASE WHEN RTLessorCustRTNHardwareLOG.DATASRC = '01' THEN '�˾����u���' " _
                   &"         WHEN RTLessorCustRTNHardwareLOG.DATASRC = '02' THEN '���ڬ��u���' ELSE " _
                  &"          '' END, RTLessorCustRTNHardwareLOG.APPLYDAT AS APPLYDAT,  " _
                  &"         CASE WHEN RTObj_7.CUSNC = '' OR " _
                  &"         RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                  &"          CASE WHEN RTObj_1.CUSNC = '' OR " _
                  &"         RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                  &"          RTLessorCustRTNHardwareLOG.CLOSEDAT AS CLOSEDAT,  " _
                 &"          RTObj_4.CUSNC AS CUSNC5,  " _
                  &"         RTLessorCustRTNHardwareLOG.CANCELDAT AS CANCELDAT,  " _
                  &"         RTObj_5.CUSNC AS CUSNC6,  " _
                  &"         RTLessorCustRTNHardwareLOG.PRTNO AS PRTNO,  " _
                 &"          RTObj_6.CUSNC AS cusnc7, SUM(RTLessorCustRTNHardwareDTL.QTY) " _
&" FROM             RTEmployee RTEmployee_4 INNER JOIN " _
                 &"          RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID RIGHT OUTER JOIN " _
                &"           RTLessorCustRTNHardwareLOG ON  " _
                 &"          RTEmployee_4.EMPLY = RTLessorCustRTNHardwareLOG.RCVUSR LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTEmployee RTEmployee_3 INNER JOIN " _
                 &"          RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID ON  " _
                &"           RTLessorCustRTNHardwareLOG.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER " _
                 &"           JOIN " _
                  &"         RTEmployee RTEmployee_2 INNER JOIN " _
                  &"         RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON  " _
                 &"          RTLessorCustRTNHardwareLOG.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTObj RTObj_3 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTObj RTObj_2 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER " _
                 &"           JOIN " _
                &"           RTEmployee RTEmployee_1 INNER JOIN " _
               &"           RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON  " _
                &"           RTLessorCustRTNHardwareLOG.REALENGINEER = RTEmployee_1.EMPLY LEFT " _
                 &"           OUTER JOIN " _
                  &"         RTObj RTObj_7 INNER JOIN " _
                 &"          RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON  " _
                 &"          RTLessorCustRTNHardwareLOG.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT " _
                 &"           OUTER JOIN " _
                 &"          RTLessorCustRTNHardwareDTL ON  " _
                 &"          RTLessorCustRTNHardwareLOG.RCVPRTNO = RTLessorCustRTNHardwareDTL.RCVPRTNO " _
                &"            LEFT OUTER JOIN " _
                &"           rtcode ON RTLessorCustRTNHardwareLOG.chgcodE = rtcode.code AND  " _
                &"           rtcode.kind = 'G2' LEFT OUTER JOIN " _
                &"           RTEMPLOYEE RTEMPLOYEE_10 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.CHGUSR = RTEMPLOYEE_10.EMPLY INNER JOIN " _
                 &"          RTOBJ RTOBJ_10 ON RTEMPLOYEE_10.CUSID = RTOBJ_10.CUSID " _
                       &" where RTLessorCustRTNHardwareLOG.RCVPRTNO='' " _
                       &" GROUP BY  RTLessorCustRTNHardwareLOG.RCVPRTNO,  " _
                &"           RTLessorCustRTNHardwareLOG.entryno,  " _
               &"            RTLessorCustRTNHardwareLOG.chgdat, RTCODE.CODENC, RTOBJ_10.CUSNC,  " _
               &"            CASE WHEN RTLessorCustRTNHardwareLOG.DATASRC = '01' THEN '�˾����u���' " _
               &"             WHEN RTLessorCustRTNHardwareLOG.DATASRC = '02' THEN '���ڬ��u���' ELSE " _
                &"            '' END, RTLessorCustRTNHardwareLOG.APPLYDAT,  " _
                &"           CASE WHEN RTObj_7.CUSNC = '' OR " _
                &"           RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                &"            CASE WHEN RTObj_1.CUSNC = '' OR " _
                 &"          RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                &"            RTLessorCustRTNHardwareLOG.CLOSEDAT, RTObj_4.CUSNC,  " _
                &"           RTObj_5.CUSNC, RTLessorCustRTNHardwareLOG.PRTNO,  " _
                &"           RTLessorCustRTNHardwareLOG.CANCELDAT, RTObj_6.CUSNC  "
  dataTable="RTlessorcustHARDWARELOG"
  userDefineDelete="Yes"
  numberOfKey=4
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

  searchFirst=FALSE
  If searchQry="" Then
     searchQry=" RTLessorCustRTNHardwareLOG .RCVprtno='" & aryparmkey(0) & "' "
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
  'if UCASE(emply)="T89001" or Ucase(emply)="T89002" or  Ucase(emply)="T89020" or Ucase(emply)="T89018" or Ucase(emply)="T93168" OR _
  '   Ucase(emply)="T89003" or Ucase(emply)="T89005" or Ucase(emply)="T89025" or Ucase(emply)="T89076"then
  '   DAreaID="<>'*'"
  'end if
  '��T���޲z���iŪ���������
  'if userlevel=31 then DAreaID="<>'*'"
  
  '�ѩ�����q�h�a�|���ӽШ�u���A�G�ȪA���}��Ҧ��ϰ��v���A�@�����x�_�ȪA�B�z
  if userlevel=31 or userlevel =1  or userlevel =5 then DAreaID="<>'*'"
  
         sqlList="SELECT  RTLessorCustRTNHardwareLOG.RCVPRTNO AS RCVPRTNO, " _
                   &"        RTLessorCustRTNHardwareLOG.entryno, " _
                      &"     RTLessorCustRTNHardwareLOG.chgdat, RTCODE.CODENC, RTOBJ_10.CUSNC, " _
                         &"  RTCODE_2.CODENC, RTLessorCustRTNHardwareLOG.APPLYDAT AS APPLYDAT,  " _
                  &"         CASE WHEN RTObj_7.CUSNC = '' OR " _
                  &"         RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                  &"          CASE WHEN RTObj_1.CUSNC = '' OR " _
                  &"         RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                  &"          RTLessorCustRTNHardwareLOG.CLOSEDAT AS CLOSEDAT,  " _
                 &"          RTObj_4.CUSNC AS CUSNC5,  " _
                  &"         RTLessorCustRTNHardwareLOG.CANCELDAT AS CANCELDAT,  " _
                  &"         RTObj_5.CUSNC AS CUSNC6,  " _
                  &"         RTLessorCustRTNHardwareLOG.PRTNO AS PRTNO,  " _
                 &"          RTObj_6.CUSNC AS cusnc7, SUM(RTLessorCustRTNHardwareDTL.QTY) " _
&" FROM             RTEmployee RTEmployee_4 INNER JOIN " _
                 &"          RTObj RTObj_6 ON RTEmployee_4.CUSID = RTObj_6.CUSID RIGHT OUTER JOIN " _
                &"           RTLessorCustRTNHardwareLOG ON  " _
                 &"          RTEmployee_4.EMPLY = RTLessorCustRTNHardwareLOG.RCVUSR LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTEmployee RTEmployee_3 INNER JOIN " _
                 &"          RTObj RTObj_5 ON RTEmployee_3.CUSID = RTObj_5.CUSID ON  " _
                &"           RTLessorCustRTNHardwareLOG.CANCELUSR = RTEmployee_3.EMPLY LEFT OUTER " _
                 &"           JOIN " _
                  &"         RTEmployee RTEmployee_2 INNER JOIN " _
                  &"         RTObj RTObj_4 ON RTEmployee_2.CUSID = RTObj_4.CUSID ON  " _
                 &"          RTLessorCustRTNHardwareLOG.CLOSEUSR = RTEmployee_2.EMPLY LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTObj RTObj_3 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.REALCONSIGNEE = RTObj_3.CUSID LEFT OUTER " _
                 &"           JOIN " _
                 &"          RTObj RTObj_2 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.ASSIGNCONSIGNEE = RTObj_2.CUSID LEFT OUTER " _
                 &"           JOIN " _
                &"           RTEmployee RTEmployee_1 INNER JOIN " _
               &"           RTObj RTObj_1 ON RTEmployee_1.CUSID = RTObj_1.CUSID ON  " _
                &"           RTLessorCustRTNHardwareLOG.REALENGINEER = RTEmployee_1.EMPLY LEFT " _
                 &"           OUTER JOIN " _
                  &"         RTObj RTObj_7 INNER JOIN " _
                 &"          RTEmployee RTEmployee_5 ON RTObj_7.CUSID = RTEmployee_5.CUSID ON  " _
                 &"          RTLessorCustRTNHardwareLOG.ASSIGNENGINEER = RTEmployee_5.EMPLY LEFT " _
                 &"           OUTER JOIN " _
                 &"          RTLessorCustRTNHardwareDTL ON  " _
                 &"          RTLessorCustRTNHardwareLOG.RCVPRTNO = RTLessorCustRTNHardwareDTL.RCVPRTNO " _
                &"            LEFT OUTER JOIN " _
                &"           rtcode ON RTLessorCustRTNHardwareLOG.chgcodE = rtcode.code AND  " _
                &"           rtcode.kind = 'G2' LEFT OUTER JOIN " _
                &"           RTEMPLOYEE RTEMPLOYEE_10 ON  " _
                 &"          RTLessorCustRTNHardwareLOG.CHGUSR = RTEMPLOYEE_10.EMPLY INNER JOIN " _
                 &"          RTOBJ RTOBJ_10 ON RTEMPLOYEE_10.CUSID = RTOBJ_10.CUSID LEFT OUTER JOIN " _
                 &"          RTCODE RTCODE_2 ON RTLessorCustRTNHardwareLOG.DATASRC=RTCODE_2.CODE AND RTCODE_2.KIND='O1' " _
                       &" where " & searchqry & " " _
                       &" GROUP BY  RTLessorCustRTNHardwareLOG.RCVPRTNO,  " _
                &"           RTLessorCustRTNHardwareLOG.entryno,  " _
               &"            RTLessorCustRTNHardwareLOG.chgdat, RTCODE.CODENC, RTOBJ_10.CUSNC,  " _
               &"            RTCODE_2.CODENC, RTLessorCustRTNHardwareLOG.APPLYDAT,  " _
                &"           CASE WHEN RTObj_7.CUSNC = '' OR " _
                &"           RTObj_7.CUSNC IS NULL THEN RTObj_2.SHORTNC ELSE RTObj_7.CUSNC END, " _
                &"            CASE WHEN RTObj_1.CUSNC = '' OR " _
                 &"          RTObj_1.CUSNC IS NULL THEN RTObj_3.SHORTNC ELSE RTObj_1.CUSNC END, " _
                &"            RTLessorCustRTNHardwareLOG.CLOSEDAT, RTObj_4.CUSNC,  " _
                &"           RTObj_5.CUSNC, RTLessorCustRTNHardwareLOG.PRTNO,  " _
                &"           RTLessorCustRTNHardwareLOG.CANCELDAT, RTObj_6.CUSNC  "

  'end if
 ' Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>