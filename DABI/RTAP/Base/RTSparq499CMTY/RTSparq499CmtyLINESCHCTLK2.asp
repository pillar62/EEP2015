<!-- #include virtual="/WebUtilityV4EBT/DBAUDI/zzKeyList.inc" -->
<!-- #include virtual="/webap/include/accesspermit.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserEmply.inc" -->
<%
Sub SrEnvironment()
  company="���T�e�W�����ѥ��������q"
  system="�t��499�޲z�t��"
  title="�t��499�D�u���u�@�~"
  buttonName=" �s�W ; �R�� ; ���� ;���s��z;����;�\��ﶵ"
  V=split(SrAccessPermit,";")
  AllowA=V(0):AllowU=V(1):AllowD=V(2):AllowP=V(3)
  ButtonEnable=V(0) & ";N;Y;Y;Y;Y"  
  'buttonEnable="Y;Y;Y;Y;Y;Y"
  functionOptName="�]�Ƭd��;�Τ�d��;�إ߬��u��"
  functionOptProgram="RTSparq499CmtyHARDWAREK2.asp;RTSparq499CustK.asp;RTSparq499CmtylineSNDWORKk.asp"
  functionOptPrompt="N;N;N"
  If V(1)="Y" then
     accessMode="U"
  Else
     accessMode="I"
  End IF
  DSN="DSN=RTLib"
  formatName="none;none;���ϦW��;�D�u;�u��IP;�q�H�Ǧ�m;���g�P;�����q��;�p�渹�X;�i�ظm;�ӽФ�;���u�渹;���q��"
  sqlDelete="select a.COMQ1, a.LINEQ1, b.COMN, "_
		   &"rtrim(convert(varchar(10),a.COMQ1)) +'-'+ rtrim(convert(varchar(10),a.LINEQ1)), "_
		   &"convert(varchar(3),a.LINEIPSTR1)+'.'+convert(varchar(3),a.LINEIPSTR2)+'.'+convert(varchar(3),a.LINEIPSTR3)+'.'+convert(varchar(3),a.LINEIPSTR4)+' - '+convert(varchar(3),a.LINEIPEND), "_
		   &"isnull(d.cutnc, '')+a.TOWNSHIP+a.RADDR+a.EQUIPPOSITION, "_
		   &"case a.CONSIGNEE when '' then g.AREANC else e.sHORTNC end, "_ 
		   &"a.LINETEL, a.CHTWORKINGNO, a.AGREE, a.ADSLAPPLYDAT, c.PRTNO, a.ADSLOPENDAT "_
		   &"from	RTSparq499CmtyLine a "_
		   &"inner join RTSparq499CmtyH b on a.comq1 = b.comq1 "_
		   &"left outer join RTSparq499CmtyLINESNDWORK c on c.COMQ1 = a.COMQ1 and c.LINEQ1 = a.LINEQ1 "_
		   &"left outer join RTCounty d on d.CUTID = a.CUTID "_
		   &"left outer join RTObj e on e.CUSID = a.CONSIGNEE "_
		   &"left outer join RTCtyTown f inner join RTArea g on g.AREAID = f.AREAID and g.AREATYPE ='3 'on f.CUTID = a.CUTID and f.TOWNSHIP = a.TOWNSHIP "_
		   &"where	a.COMQ1= 0 "

  dataTable="RTSparq499Cmtyline"
  userDefineDelete="Yes"
  numberOfKey=2
  dataProg="RTSparq499CmtylineD.asp"
  datawindowFeature=""
  searchWindowFeature="width=640,height=460,scrollbars=yes"
  optionWindowFeature=""
  detailWindowFeature=""
  diaWidth=""
  diaHeight=""
  diaTitle="�U�C��ƱN�Q�R���A�Ы��T�{�R�����A�Ϋ������C"
  diaButtonName=" �T�{�R�� ; ���� "
  goodMorning=FALSE
  goodMorningImage="cbbn.jpg"
  colSplit=1
  keyListPageSize=25
  searchProg="RTSparq499CmtyLINESCHCTLS.ASP"
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
     searchQry=" a.LINEIPSTR1 <> '' and a.adslopendat is null "
     searchShow="�w���X�u���ӽ�(��IP)�A�B�u���|���}�q���D�u�M�� "
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
         case "C"
            DAreaID="='A2'"         
         case "P"
            DAreaID="='A1'"                        
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
'  if userlevel=31  then DAreaID="<>'*'"
'    If searchShow="����" Then
         sqlList="select a.COMQ1, a.LINEQ1, b.COMN, "_
				&"rtrim(convert(varchar(10),a.COMQ1)) +'-'+ rtrim(convert(varchar(10),a.LINEQ1)), "_
				&"convert(varchar(3),a.LINEIPSTR1)+'.'+convert(varchar(3),a.LINEIPSTR2)+'.'+convert(varchar(3),a.LINEIPSTR3)+'.'+convert(varchar(3),a.LINEIPSTR4)+' - '+convert(varchar(3),a.LINEIPEND), "_
				&"isnull(d.cutnc, '')+a.TOWNSHIP+a.RADDR+a.EQUIPPOSITION, "_
				&"case a.CONSIGNEE when '' then g.AREANC else e.sHORTNC end, "_ 
				&"a.LINETEL, a.CHTWORKINGNO, a.AGREE, a.ADSLAPPLYDAT, c.PRTNO, a.ADSLOPENDAT "_
				&"from	RTSparq499CmtyLine a "_
				&"inner join RTSparq499CmtyH b on a.comq1 = b.comq1 "_
				&"left outer join RTSparq499CmtyLINESNDWORK c on c.COMQ1 = a.COMQ1 and c.LINEQ1 = a.LINEQ1 "_
				&"left outer join RTCounty d on d.CUTID = a.CUTID "_
				&"left outer join RTObj e on e.CUSID = a.CONSIGNEE "_
				&"left outer join RTCtyTown f inner join RTArea g on g.AREAID = f.AREAID and g.AREATYPE ='3 'on f.CUTID = a.CUTID and f.TOWNSHIP = a.TOWNSHIP "_
				&"where	a.dropdat is null and a.canceldat is null AND " & SEARCHQRY _
				&" ORDER BY 7, 1"
'    Else
'         sqlList="select a.COMQ1, a.LINEQ1, b.COMN, "_
'				&"rtrim(convert(varchar(10),a.COMQ1)) +'-'+ rtrim(convert(varchar(10),a.LINEQ1)), "_
'				&"convert(varchar(3),a.LINEIPSTR1)+'.'+convert(varchar(3),a.LINEIPSTR2)+'.'+convert(varchar(3),a.LINEIPSTR3)+'.'+convert(varchar(3),a.LINEIPSTR4)+' - '+convert(varchar(3),a.LINEIPEND), "_
'				&"isnull(d.cutnc, '')+a.TOWNSHIP+a.RADDR+a.EQUIPPOSITION, "_
'				&"case a.CONSIGNEE when '' then g.AREANC else e.sHORTNC end, "_ 
'				&"a.LINETEL, a.CHTWORKINGNO, a.AGREE, a.ADSLAPPLYDAT, c.PRTNO, a.ADSLOPENDAT "_
'				&"from	RTSparq499CmtyLine a "_
'				&"inner join RTSparq499CmtyH b on a.comq1 = b.comq1 "_
'				&"left outer join RTSparq499CmtyLINESNDWORK c on c.COMQ1 = a.COMQ1 and c.LINEQ1 = a.LINEQ1 "_
'				&"left outer join RTCounty d on d.CUTID = a.CUTID "_
'				&"left outer join RTObj e on e.CUSID = a.CONSIGNEE "_
'				&"left outer join RTCtyTown f inner join RTArea g on g.AREAID = f.AREAID and g.AREATYPE ='3 'on f.CUTID = a.CUTID and f.TOWNSHIP = a.TOWNSHIP "_
'				&"where	a.COMQ1<> 0 AND " & SEARCHQRY _
'				&" AND (a.MOVETOCOMQ1 IS NULL OR a.MOVETOCOMQ1=0) AND (a.MOVEFROMCOMQ1 IS NULL OR a.MOVEFROMCOMQ1=0) "_
'				&" ORDER BY 7, 1"
'    End If  
'Response.Write "SQL=" & SQLlist
End Sub
Sub SrRunUserDefineDelete()

End Sub
%>