<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="ET-City�޲z�t��"
  title="ET-City�D�u��ƺ��@"
  buttonName=" �s�W ; �R�� ; ���� ;���s��z;����;�\��ﶵ"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
  Emply=FrGetUserEmply(Request.ServerVariables("LOGON_USER"))  
  functionOptName="�D�u���u;�]�Ƭd��;�Τ���@;�ȪA�ץ�;������;�M�u�@�~;�@�@�@�o;�@�o����;���v����"
  functionOptProgram="RTLessorCmtyLineSNDWORKK.asp;RTLessorCmtylineHardwareK2.asp;RTLessorCustK.asp;RTLessorCmtyLineFAQK.asp;RTLessorCmtyLineContK.asp;RTLessorCmtyLineDROPK.asp;RTLessorCmtyLineCANCEL.asp;RTLessorCmtyLineCANCELRTN.asp;RTLessorCmtyLineLOGK.asp"
  functionOptPrompt="N;N;N;N;N;N;Y;Y;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;���ϦW��;�D�u;����-�m��;�s��;�D�uIP;none;none;none;�D�u����;�u��ISP;IP����;�D�u�t�v;IP��;�ӽФ�;����;���q��;�M�u��;�@�o��;�Τ�;����;�h��;�p�O;�t��"
  sqlDelete="SELECT RTLessorCmtyLine.COMQ1, RTLessorCmtyLine.LINEQ1,RTLESSORCMTYH.COMN,rtrim(convert(char(6),RTLessorCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLine.lineQ1)) , RTCOUNTY.CUTNC+'-'+ " _
                &"RTLESSORCMTYLINE.TOWNSHIP,RTLessorCmtyLine.LINEGROUP,RTLessorCmtyLine.LINEIP,RTLessorCmtyLine.GATEWAY, " _
                &"RTLessorCmtyLine.PPPOEACCOUNT, RTLessorCmtyLine.PPPOEPASSWORD, RTLessorCmtyLine.LINETEL, " _
                &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorCmtyLine.IPCNT, " _
                &"RTLessorCmtyLine.APPLYDAT, RTLessorCmtyLine.HARDWAREDAT, RTLessorCmtyLine.ADSLAPPLYDAT, " _
                &"RTLessorCmtyLine.DROPDAT, RTLessorCmtyLine.CANCELDAT, " _
                &"SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND RTLessorCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END), " _
                &"SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND RTLessorCust.FINISHDAT IS NOT NULL AND " _
                &"RTLessorCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
                &"(RTLessorCust.STRBILLINGDAT IS NOT NULL OR RTLessorCust.NEWBILLINGDAT IS NOT NULL) AND " _
                &"RTLessorCust.DROPDAT IS NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
                &"RTLessorCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
                &"RTLessorCust.FINISHDAT IS NOT NULL AND RTLessorCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
                &"- SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND (RTLessorCust.STRBILLINGDAT IS NOT NULL OR " _
                &"RTLessorCust.NEWBILLINGDAT IS NOT NULL) AND RTLessorCust.DROPDAT IS NULL THEN 1 ELSE 0 END) " _
                &"FROM    RTLessorCmtyLine LEFT OUTER JOIN RTLessorCust ON RTLessorCmtyLine.COMQ1 = RTLessorCust.COMQ1 AND " _
                &"RTLessorCmtyLine.LINEQ1 = RTLessorCust.LINEQ1 LEFT OUTER JOIN RTCode RTCode_3 ON " _
                &"RTLessorCmtyLine.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
                &"RTCode RTCode_1 ON RTLessorCmtyLine.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
                &"RTCode RTCode_2 ON RTLessorCmtyLine.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' LEFT OUTER JOIN " _
                &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
                &"RTLessorCmtyLine.SALESID = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_2 ON " _
                &"RTLessorCmtyLine.CONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTLESSORCMTYH ON " _
                &"RTLessorCmtyLine.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON " _
                &"RTLESSORCMTYLINE.CUTID = RTCOUNTY.CUTID " _
                &"WHERE RTLessorCmtyLine.COMQ1=0 " _
                &"GROUP BY  RTLessorCmtyLine.COMQ1, RTLessorCmtyLine.LINEQ1, RTLESSORCMTYH.COMN,rtrim(convert(char(6),RTLessorCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLine.lineQ1)) , RTCOUNTY.CUTNC+'-'+ " _
                &"RTLESSORCMTYLINE.TOWNSHIP, RTLessorCmtyLine.LINEGROUP, RTLessorCmtyLine.LINEIP, " _
                &"RTLessorCmtyLine.GATEWAY, RTLessorCmtyLine.PPPOEACCOUNT, RTLessorCmtyLine.PPPOEPASSWORD, " _
                &"RTLessorCmtyLine.LINETEL, RTCode_1.CODENC, RTCode_3.CODENC, RTCode_2.CODENC, RTLessorCmtyLine.IPCNT, " _
                &"RTLessorCmtyLine.APPLYDAT, RTLessorCmtyLine.HARDWAREDAT, RTLessorCmtyLine.ADSLAPPLYDAT, " _
                &"RTLessorCmtyLine.DROPDAT, RTLessorCmtyLine.CANCELDAT " _
                &"ORDER BY  RTLessorCmtyLine.COMQ1, RTLessorCmtyLine.LINEQ1 "

  dataTable="RTLessorCmtyLine"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTLessorCmtyLineD.asp"
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
  sqlYY="select * from RTLessorCmtyH LEFT OUTER JOIN RTCOUNTY ON RTLessorCmtyH.CUTID=RTCOUNTY.CUTID where COMQ1=" & ARYPARMKEY(0)
  connYY.Open dsnYY
  rsYY.Open sqlYY,connYY
  if not rsYY.EOF then
     COMN=rsYY("COMN")
     COMADDR=RSYY("CUTNC") & RSYY("TOWNSHIP") & RSYY("RADDR")
  else
     COMN=""
     COMADDR=""
  end if
  rsYY.Close
  connYY.Close
  set rsYY=nothing
  set connYY=nothing
  searchFirst=FALSE
  If searchQry="" Then
	'searchQry=" RTLessorCmtyLine.ComQ1=" & aryparmkey(0)
	'searchShow="���ϧǸ��J"& aryparmkey(0) & ",���ϦW�١J" & COMN & ",���Ϧa�}�J" & COMADDR
	'�ק�A
    if ARYPARMKEY(1) ="" then 
		searchQry=" RTLessorCmtyLine.ComQ1=" & aryparmkey(0)
		searchShow="���ϧǸ��J"& aryparmkey(0) & ",���ϦW�١J" & COMN & ",���Ϧa�}�J" & COMADDR
	else
		searchQry=" RTLessorCmtyLine.ComQ1=" & aryparmkey(0) & " and RTLessorCmtyLine.Lineq1 =" &aryparmkey(1)
		searchShow="�D�u�Ǹ��J"& aryparmkey(0)&"-"&aryparmkey(1) & ",���ϦW�١J" & COMN & ",���Ϧa�}�J" & COMADDR
	end if		

  ELSE
     SEARCHFIRST=FALSE
  End If
  'Response.Write "user=" & Request.ServerVariables("LOGON_USER")
  'Ū���n�J�b�����s�ո��
  'Response.Write "GP=" & usergroup
  '-------------------------------------------------------------------------------------------
  'userlevel=2:���~�Ȥu�{�v==>�u��ݩ��ݪ��ϸ��
  'DOMAIN:'T','C','K'�_���n�ҰϤH��(�ȪA,�޳N)�u��ݩ����Ұϸ��
 ' Response.Write "DOMAIN=" & domain & "<BR>"
  'Domain=Mid(Emply,1,1)
    sqlList="SELECT RTLessorCmtyLine.COMQ1, RTLessorCmtyLine.LINEQ1,RTLESSORCMTYH.COMN,rtrim(convert(char(6),RTLessorCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLine.lineQ1)) , RTCOUNTY.CUTNC+'-'+ " _
        &"RTLESSORCMTYLINE.TOWNSHIP,RTLessorCmtyLine.LINEGROUP,RTLessorCmtyLine.LINEIP,RTLessorCmtyLine.GATEWAY, " _
        &"RTLessorCmtyLine.PPPOEACCOUNT, RTLessorCmtyLine.PPPOEPASSWORD, RTLessorCmtyLine.LINETEL, " _
        &"RTCode_1.CODENC, RTCode_3.CODENC AS Expr1, RTCode_2.CODENC AS Expr2, RTLessorCmtyLine.IPCNT, " _
        &"RTLessorCmtyLine.APPLYDAT, RTLessorCmtyLine.HARDWAREDAT, RTLessorCmtyLine.ADSLAPPLYDAT, " _
        &"RTLessorCmtyLine.DROPDAT, RTLessorCmtyLine.CANCELDAT, " _
        &"SUM(CASE WHEN RTLessorCust.cusid is not null and RTLessorCust.CANCELDAT IS NULL THEN 1 ELSE 0 END), " _
        &"SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND RTLessorCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END), " _
        &"SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND RTLessorCust.FINISHDAT IS NOT NULL AND " _
        &"RTLessorCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
        &"(RTLessorCust.STRBILLINGDAT IS NOT NULL OR RTLessorCust.NEWBILLINGDAT IS NOT NULL) AND " _
        &"RTLessorCust.DROPDAT IS NULL THEN 1 ELSE 0 END), SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
        &"RTLessorCust.FINISHDAT IS NOT NULL THEN 1 ELSE 0 END) - SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND " _
        &"RTLessorCust.FINISHDAT IS NOT NULL AND RTLessorCust.DROPDAT IS NOT NULL THEN 1 ELSE 0 END) " _
        &"- SUM(CASE WHEN RTLessorCust.CANCELDAT IS NULL AND (RTLessorCust.STRBILLINGDAT IS NOT NULL OR " _
        &"RTLessorCust.NEWBILLINGDAT IS NOT NULL) AND RTLessorCust.DROPDAT IS NULL THEN 1 ELSE 0 END) " _
        &"FROM    RTLessorCmtyLine LEFT OUTER JOIN RTLessorCust ON RTLessorCmtyLine.COMQ1 = RTLessorCust.COMQ1 AND " _
        &"RTLessorCmtyLine.LINEQ1 = RTLessorCust.LINEQ1 LEFT OUTER JOIN RTCode RTCode_3 ON " _
        &"RTLessorCmtyLine.LINEIPTYPE = RTCode_3.CODE AND RTCode_3.KIND = 'M5' LEFT OUTER JOIN " _
        &"RTCode RTCode_1 ON RTLessorCmtyLine.LINEISP = RTCode_1.CODE AND RTCode_1.KIND = 'C3' LEFT OUTER JOIN " _
        &"RTCode RTCode_2 ON RTLessorCmtyLine.LINERATE = RTCode_2.CODE AND RTCode_2.KIND = 'D3' LEFT OUTER JOIN " _
        &"RTEmployee INNER JOIN RTObj RTObj_1 ON RTEmployee.CUSID = RTObj_1.CUSID ON " _
        &"RTLessorCmtyLine.SALESID = RTEmployee.EMPLY LEFT OUTER JOIN RTObj RTObj_2 ON " _
        &"RTLessorCmtyLine.CONSIGNEE = RTObj_2.CUSID LEFT OUTER JOIN RTLESSORCMTYH ON " _
        &"RTLessorCmtyLine.COMQ1 = RTLESSORCMTYH.COMQ1 LEFT OUTER JOIN RTCOUNTY ON " _
        &"RTLESSORCMTYLINE.CUTID = RTCOUNTY.CUTID " _
        &"WHERE " & SEARCHQRY & " " _
        &"GROUP BY  RTLessorCmtyLine.COMQ1, RTLessorCmtyLine.LINEQ1, RTLESSORCMTYH.COMN,rtrim(convert(char(6),RTLessorCmtyLine.COMQ1)) +'-'+ rtrim(convert(char(6),RTLessorCmtyLine.lineQ1)) , RTCOUNTY.CUTNC+'-'+ " _
        &"RTLESSORCMTYLINE.TOWNSHIP, RTLessorCmtyLine.LINEGROUP, RTLessorCmtyLine.LINEIP, " _
        &"RTLessorCmtyLine.GATEWAY, RTLessorCmtyLine.PPPOEACCOUNT, RTLessorCmtyLine.PPPOEPASSWORD, " _
        &"RTLessorCmtyLine.LINETEL, RTCode_1.CODENC, RTCode_3.CODENC, RTCode_2.CODENC, RTLessorCmtyLine.IPCNT, " _
        &"RTLessorCmtyLine.APPLYDAT, RTLessorCmtyLine.HARDWAREDAT, RTLessorCmtyLine.ADSLAPPLYDAT, " _
        &"RTLessorCmtyLine.DROPDAT, RTLessorCmtyLine.CANCELDAT " _
        &"ORDER BY  RTLessorCmtyLine.COMQ1, RTLessorCmtyLine.LINEQ1 "

  'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>