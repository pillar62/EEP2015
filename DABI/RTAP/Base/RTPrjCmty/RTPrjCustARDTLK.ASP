<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="�M�ץΤ�޲z�t��"
  title="�M�ץΤ��������I�b�ک��Ӭd��"
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
  sqlDelete="SELECT     RTPrjCustARDTL.CUSID, RTPrjCustARDTL.BATCHNO, " _
                        &"  RTPrjCustARDTL.SEQ, " _
                        &"  RTPrjCustARDTL.L14 + '-' + RTPrjCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC, RTPrjCustARDTL.ITEMNC, " _
                        &"  RTPrjCustARDTL.PORM, RTPrjCustARDTL.AMT, " _
                        &"  RTPrjCustARDTL.REALAMT, " _
                        &"  RTPrjCustARDTL.AMT - RTPrjCustARDTL.REALAMT AS Expr1, " _
                        &"  RTPrjCustARDTL.CDAT, RTPrjCustARDTL.MDAT, " _
                        &"  RTPrjCustARDTL.CANCELDAT, RTPrjCustARDTL.CANCELMEMO " _
           &"FROM           RTPrjCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTPrjCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTPrjCustARDTL.L23 = RTAccountNo.L23 " _
           &"where RTPrjCustARDTL.cusid='' "
  dataTable="RTPrjCustARDTL"
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
  sqlYY="select * from RTCounty RIGHT OUTER JOIN RTPrjCmtyH ON " _
       &"RTCounty.CUTID = RTPrjCmtyH.CUTID RIGHT OUTER JOIN RTPrjCust ON RTPrjCmtyH.COMQ1 = RTPrjCust.COMQ1 " _
       &"where RTPrjCust.cusid='" & ARYPARMKEY(0) & "'"
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
  else
     COMN=""
  end if
  rsYY.Close
  sqlYY="select RTCounty.CUTNC+RTPrjCmtyLine.township+RTPrjCmtyLine.raddr as comaddr from RTCounty RIGHT OUTER JOIN RTPrjCmtyLine ON  " _
       &"RTCounty.CUTID = RTPrjCmtyLine.CUTID RIGHT OUTER JOIN " _
       &"RTPrjCust ON RTPrjCmtyLine.COMQ1 = RTPrjCust.COMQ1 AND " _
       &"RTPrjCmtyLine.LINEQ1 = RTPrjCust.LINEQ1 " _
       &"where RTPrjCust.cusid='" & ARYPARMKEY(0) & "'"
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMaddr=rsyy("comaddr")
  else
     COMaddr=""
  end if
  RSYY.Close
  sqlYY="select * from RTPrjCUST  where CUSID='" & ARYPARMKEY(0) & "' "
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
     searchQry=" RTPrjCustARDTL.CUSID='" & ARYPARMKEY(0) & "' AND RTPrjCustARDTL.BATCHNO='" & ARYPARMKEY(1) & "' "
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
  
    sqlList="SELECT          RTPrjCustARDTL.CUSID, RTPrjCustARDTL.BATCHNO, " _
                        &"  RTPrjCustARDTL.SEQ, " _
                        &"  RTPrjCustARDTL.L14 + '-' + RTPrjCustARDTL.L23 AS Expr2, " _
                        &"  RTAccountNo.ACNAMEC,convert(varchar(4),RTPrjCustARDTL.syy)+'/'+convert(varchar(2),RTPrjCustARDTL.smm),convert(varchar(4),RTPrjCustARDTL.tyy)+'/'+convert(varchar(2),RTPrjCustARDTL.tmm), RTPrjCustARDTL.ITEMNC, " _
                        &"  RTPrjCustARDTL.PORM, RTPrjCustARDTL.AMT, " _
                        &"  RTPrjCustARDTL.REALAMT, " _
                        &"  RTPrjCustARDTL.AMT - RTPrjCustARDTL.REALAMT AS Expr1, " _
                        &"  RTPrjCustARDTL.CDAT, RTPrjCustARDTL.MDAT, " _
                        &"  RTPrjCustARDTL.CANCELDAT, RTPrjCustARDTL.CANCELMEMO " _
           &"FROM           RTPrjCustARDTL LEFT OUTER JOIN " _
                        &"  RTAccountNo ON RTPrjCustARDTL.L14 = RTAccountNo.L14 AND " _
                        &"  RTPrjCustARDTL.L23 = RTAccountNo.L23 " _
           &"where " & searchqry & " ORDER BY RTPrjCustARDTL.SEQ "


  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>