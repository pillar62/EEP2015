<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="���Ǥj�e�W���ϫ��޲z�t��"
  title="���Ǥj�e�W���ϫ��Τ��������I�b�ک��Ӭd��"
  buttonName=" �s�W ; �R�� ; ���� ;���s��z;����;�\��ﶵ"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable="N;N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName=""
  functionOptProgram=""
  functionOptPrompt=""
  functionoptopen=""
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;����;�|�p���;��ئW��;�߱b�~��;�{�C�~��;���ئW��;��/�t;����(�I)���B;�w�R�P���B;���R�b���B;���ͤ�;�R�b��;�@�o��;�@�o��]"
  sqlDelete="SELECT     RTfareastCustARDTL.CUSID, RTfareastCustARDTL.BATCHNO, " _
                        &"  RTfareastCustARDTL.SEQ, " _
                        &"  RTfareastCustARDTL.L14 + '-' + RTfareastCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC, RTfareastCustARDTL.ITEMNC, " _
                        &"  RTfareastCustARDTL.PORM, RTfareastCustARDTL.AMT, " _
                        &"  RTfareastCustARDTL.REALAMT, " _
                        &"  RTfareastCustARDTL.AMT - RTfareastCustARDTL.REALAMT AS Expr1, " _
                        &"  RTfareastCustARDTL.CDAT, RTfareastCustARDTL.MDAT, " _
                        &"  RTfareastCustARDTL.CANCELDAT, RTfareastCustARDTL.CANCELMEMO " _
           &"FROM           RTfareastCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTfareastCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTfareastCustARDTL.L23 = RTAccountNo.L23 " _
           &"where RTfareastCustARDTL.cusid='' "
  dataTable="RTfareastCustARDTL"
  userDefineDelete="Yes"
  numberOfKey=3
  dataProg=""
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
  set connYY=server.CreateObject("ADODB.connection")
  set rsYY=server.CreateObject("ADODB.recordset")
  dsnYY="DSN=RTLIB"
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTfareastCmtyH ON " _
       &"RTCounty.CUTID = RTfareastCmtyH.CUTID RIGHT OUTER JOIN RTfareastCust ON RTfareastCmtyH.COMQ1 = RTfareastCust.COMQ1 " _
       &"where RTfareastCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTfareastCmtyLine ON  " _
       &"RTCounty.CUTID = RTfareastCmtyLine.CUTID RIGHT OUTER JOIN " _
       &"RTfareastCust ON RTfareastCmtyLine.COMQ1 = RTfareastCust.COMQ1 AND " _
       &"RTfareastCmtyLine.LINEQ1 = RTfareastCust.LINEQ1 " _
       &"where RTfareastCust.cusid='" & ARYPARMKEY(0) & "'"
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     comaddr=""
     COMaddr=rsYY("cutnc") & rsyy("township") & rsyy("raddr")
    ' if rsyy("village") <> "" then
    '     COMaddr= COMaddr & rsyy("village") & rsyy("cod1")
    ' end if
    ' if rsyy("NEIGHBOR") <> "" then
    '     COMaddr= COMaddr & rsyy("NEIGHBOR") & rsyy("cod2")
    ' end if
    ' if rsyy("STREET") <> "" then
    '     COMaddr= COMaddr & rsyy("STREET") & rsyy("cod3")
    ' end if
    ' if rsyy("SEC") <> "" then
    '     COMaddr= COMaddr & rsyy("SEC") & rsyy("cod4")
    ' end if
    ' if rsyy("LANE") <> "" then
    '     COMaddr= COMaddr & rsyy("LANE") & rsyy("cod5")
    ' end if
    ' if rsyy("ALLEYWAY") <> "" then
    '     COMaddr= COMaddr & rsyy("ALLEYWAY") & rsyy("cod7")
    ' end if
    ' if rsyy("NUM") <> "" then
    '     COMaddr= COMaddr & rsyy("NUM") & rsyy("cod8")
    ' end if
    ' if rsyy("FLOOR") <> "" then
    '     COMaddr= COMaddr & rsyy("FLOOR") & rsyy("cod9")
    ' end if
    ' if rsyy("ROOM") <> "" then
    '     COMaddr= COMaddr & rsyy("ROOM") & rsyy("cod10")
    ' end if
  else
     COMaddr=""
  end if
  RSYY.Close
  sqlYY="select * from RTfareastCUST  where CUSID='" & ARYPARMKEY(0) & "' "
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
     searchQry=" RTfareastCustARDTL.CUSID='" & ARYPARMKEY(0) & "' AND RTfareastCustARDTL.BATCHNO='" & ARYPARMKEY(1) & "' "
     searchShow="�D�u�J"& comq1xx & "-" & lineq1xx & ",���ϡJ" & COMN & ",�D�u��}�J" & COMADDR & ",�Τ�Ǹ��J" & aryparmkey(0) & ",�Τ�W�١J" & CUSNC & ",�����b�ڽs���J" & ARYPARMKEY(1)
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
  
    sqlList="SELECT          RTfareastCustARDTL.CUSID, RTfareastCustARDTL.BATCHNO, " _
                        &"  RTfareastCustARDTL.SEQ, " _
                        &"  RTfareastCustARDTL.L14 + '-' + RTfareastCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC,convert(varchar(4),RTfareastCustARDTL.syy)+'/'+convert(varchar(2),RTfareastCustARDTL.smm),convert(varchar(4),RTfareastCustARDTL.tyy)+'/'+convert(varchar(2),RTfareastCustARDTL.tmm), RTfareastCustARDTL.ITEMNC, " _
                        &"  RTfareastCustARDTL.PORM, RTfareastCustARDTL.AMT, " _
                        &"  RTfareastCustARDTL.REALAMT, " _
                        &"  RTfareastCustARDTL.AMT - RTfareastCustARDTL.REALAMT AS Expr1, " _
                        &"  RTfareastCustARDTL.CDAT, RTfareastCustARDTL.MDAT, " _
                        &"  RTfareastCustARDTL.CANCELDAT, RTfareastCustARDTL.CANCELMEMO " _
           &"FROM           RTfareastCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTfareastCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTfareastCustARDTL.L23 = RTAccountNo.L23 " _
           &"where " & searchqry & " ORDER BY RTfareastCustARDTL.SEQ "


  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>