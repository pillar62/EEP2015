<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONNXX
   Set connXX=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   SET RSZZ=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTcmtysndwork WHERE comq1=" & KEY(0) & " and prtno='" & key(1) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   sndtype=""
   endpgm="1"
   SQLYY="SELECT comtype,CASE WHEN RTCMTY.CUTID IN ('01', '02', '03', '04', '21', '22') AND township NOT IN ('�T�l��', '�a�q��') " _
     &"THEN 'A1' WHEN rtcmty.cutid IN ('05', '06','07', '08') OR (rtcmty.cutid = '03' AND rtcmty.township IN ('�T�l��', '�a�q��')) " _
     &"THEN 'A1' WHEN rtcmty.cutid IN ('09', '10', '11', '12', '13') THEN 'A2' WHEN rtcmty.cutid IN ('14', '15', '16', '17', '18', '19', '20') " _
     &"THEN 'A3' ELSE '' END as AREAID,CASE WHEN RTCMTY.CUTID IN ('01', '02', '03', '04', '21', '22') AND township NOT IN ('�T�l��', '�a�q��') " _
     &"THEN 'G1' WHEN rtcmty.cutid IN ('05', '06','07', '08') OR (rtcmty.cutid = '03' AND rtcmty.township IN ('�T�l��', '�a�q��')) " _
     &"THEN '01' WHEN rtcmty.cutid IN ('09', '10', '11', '12', '13') THEN 'C1' WHEN rtcmty.cutid IN ('14', '15', '16', '17', '18', '19', '20') " _
     &"THEN '1' ELSE '' END as GROUPID FROM RTcmty WHERE comq1=" & KEY(0) 
   RSYY.Open SQLYY,CONNXX
   IF ( RSYY("comtype") >= "01" and RSYY("comtype") <= "04" ) or RSYY("comtype") = "07" or RSYY("comtype") <= "14" THEN
      AREAID=RSYY("AREAID")
      GROUPID=RSYY("GROUPID") 
      CONSIGNEE="N" '���P ==> MAIL ���P���f�ΦU�շ~�U
   ELSE
      CONSIGNEE="Y" '�g�P ==> MAIL���g�P���fTINA 
   END IF   
   RSYY.CLOSE 
   '���w�@�o�ɡA���i���浲�ת���
   IF LEN(TRIM(RSXX("DROPDAT"))) <> 0 THEN
      ENDPGM="3"
   '�����u���שΥ����u���פ�ҪťծɡA���i���浲�ת���
   elseif isnull(RSXX("FINISHDAT")) AND isnull(RSXX("UNCLOSEDAT")) then
      endpgm="4"      
   ELSE
      sndtype=rsxx("sndtype")
      finishdat=rsxx("finishdat")
      sqlyy="select max(entryno) as entryno FROM RTcmtysndworklog WHERE COMQ1=" & KEY(0) & " and prtno='" & key(1) & "' "
      rsZZ.Open sqlyy,connxx
      
      if len(trim(rsZZ("entryno"))) > 0 then
         entryno=rsZZ("entryno") + 1
      else
         entryno=1
      end if
      rsZZ.close
      set rsZZ=nothing
      '���u���ת���
      IF NOT ISNULL(RSXX("FINISHDAT")) THEN
         sqlyy="insert into RTcmtysndworklog " _ 
           &"SELECT   COMQ1, PRTNO, " & ENTRYNO & ", GETDATE() ,'FR','" & V(0) & "',SENDWORKDAT, PRTUSR, ASSIGNENGINEER1, ASSIGNENGINEER2, " _
           &"ASSIGNENGINEER3, ASSIGNENGINEER4, ASSIGNENGINEER5, ASSIGNCONSIGNEE, REALENGINEER1, REALENGINEER2, REALENGINEER3, " _
           &"REALENGINEER4, REALENGINEER5, REALCONSIGNEE, DROPDAT,DROPDESC, CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT,STOCKCLOSEUSR, STOCKFINCHK, SNDTYPE, HOSTCABLENO, MEMO, " _
           &"PRTDAT, EUSR, EDAT, UUSR, UDAT, CLOSEUSR, DROPUSR, UNCLOSEDAT,finishdat " _
           &"FROM RTcmtysndwork where COMQ1=" & key(0) & " and prtno='" & key(1) & "' "
         STS="1"
      '�����u���ת���
      ELSE
         sqlyy="insert into RTcmtysndworklog " _ 
           &"SELECT   COMQ1, PRTNO, " & ENTRYNO & ", GETDATE() ,'UR','" & V(0) & "',SENDWORKDAT, PRTUSR, ASSIGNENGINEER1, ASSIGNENGINEER2, " _
           &"ASSIGNENGINEER3, ASSIGNENGINEER4, ASSIGNENGINEER5, ASSIGNCONSIGNEE, REALENGINEER1, REALENGINEER2, REALENGINEER3, " _
           &"REALENGINEER4, REALENGINEER5, REALCONSIGNEE, DROPDAT,DROPDESC, CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT,STOCKCLOSEUSR, STOCKFINCHK, SNDTYPE, HOSTCABLENO, MEMO, " _
           &"PRTDAT, EUSR, EDAT, UUSR, UDAT, CLOSEUSR, DROPUSR, UNCLOSEDAT,finishdat " _
           &"FROM RTcmtysndwork where COMQ1=" & key(0) & " and prtno='" & key(1) & "' "
         STS="2"
      END IF
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTcmtysndwork set FINISHdat=NULL,UNCLOSEDAT=NULL,CLOSEUSR='',UUSR='" & V(0) & "',UDAT=GETDATE()  where COMQ1=" & KEY(0)  & " and prtno='" & key(1) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '�o�Ϳ��~�ɡA�R�������ɩҷs�W�����ʸ��
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTcmtysndworklog WHERE COMQ1=" & key(0) & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            '���u���ת���A�Y���зǤu�{�B�s���l�u�{�B���q�B���q�Υ[�˳]�ƥB�D�u�|�����q�̡A����s�D�u���q��adslapplydat���ť�(����O�ӱi���u��O�w���u���ת�,finishdat is not null)
            '�Y�������u���ת���ɡA�����ʥD�ɤ��D�u���q��
            IF (sndtype ="OC" OR  sndtype ="OP" OR  sndtype ="ST" OR  sndtype ="CB") and len(trim(finishdat)) > 0 THEN
               SQLXX=" update RTcmty set T1apply=null,UUSR='" & V(0) & "',UDAT=GETDATE()  where COMQ1=" & KEY(0) & " AND RTcmty.T1APPLY IS not NULL "
               STS="3"
               connxx.Execute SQLXX
               If Err.number > 0 then
                  endpgm="2"
               '�o�Ϳ��~�ɡA�R�������ɩҷs�W�����ʸ��
                  errmsg=cstr(Err.number) & "=" & Err.description
                  sqlyy="delete * FROM RTcmtysndworklog WHERE COMQ1=" & key(0) & "' and prtno='" & key(1) & "' AND ENTRYNO=" & ENTRYNO
                  CONNXX.Execute sqlyy 
               ELSE
                  endpgm="1"
                  errmsg=""
               END IF
           END IF
         end if      
      end if
   END IF
 '  IF ENDPGM="1" THEN
 '     TOemail="edson@cbbn.com.tw"
 '     FROMEMAIL="MIS@CBBN.COM.TW"
 '     Set objMail=CreateObject("CDONTS.Newmail")   
 '     Subject = "�F��AVS�D�u���u���ת���q��--����"
 '     body="<html><body><table border=0 width=""100%""><tr><td colspan=2>" _
 '     &"<H3>�F��AVS�D�u���u���ת���q��</h3></td></tr>" _
 '     &"<tr><td width=""30%"">&nbsp;</td><td width=""70%"">&nbsp;</td></tr>" _
 '     &"<tr><td bgcolor=lightblue align=center>���u�渹</td><td bgcolor=pink align=left>" &KEY(2) &"</td></tr>" _
 '     &"<tr><td bgcolor=lightblue align=center>���ϥD�u�Ǹ�</td><td bgcolor=pink align=left>" &KEY(0) & "-" & KEY(1) &"</td></tr>" _
 '     &"</table></body></html>"  
 '     objMail.Send FROMemail,TOEMAIL,subject,body         
 '  END IF
 '  rsyy.close
   SQLYY="SELECT * FROM RTcmty WHERE COMQ1=" & KEY(0)
   RSYY.Open SQLYY,CONNXX
   COMN=RSYY("COMN")
   RSYY.CLOSE
   IF ENDPGM="1" AND STS="3" THEN '���ݧ�s�D�u�����u�骺���u����(STS=3)����~���n�q�������H��
      FROMEMAIL="MIS@CBBN.COM.TW"
      Set jmail = Server.CreateObject("Jmail.Message")
      jmail.charset="BIG5"
      jmail.from = "MIS@cbbn.com.tw"
      Jmail.fromname="HI-Building�޲z�t�Ψt�γq��"
      IF STS="1" THEN
         jmail.Subject = "���ϡJ" & COMN & "�A���u��J" & KEY(1) & "�A���u���ת���q��"
      ELSE
         jmail.Subject = "���ϡJ" & COMN & "�A���u��J" & KEY(1) & "�A�����u���ת���q��"
      END IF
      
      jmail.priority = 1  
      body="<html><body><table border=1 width=""60%""><tr><td colspan=2>" _
      &"<H3>HI-Building�D�u���u���׳q��</h3></td></tr>" _
      &"<tr><td width=""30%"">&nbsp;</td><td width=""70%"">&nbsp;</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>���ϥD�u�Ǹ�</td><td bgcolor=pink align=left>" &KEY(0) &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>���ϦW��</td><td bgcolor=pink align=left>" &comn &"</td></tr>" _
      &"<tr><td bgcolor=lightblue align=center>���u�渹</td><td bgcolor=pink align=left>" &KEY(1) &"</td></tr>" _
      &"</table>" _
      &"<P><U>���D�u���u�O���w�Q����A�|���i����Τ�˾����u�@�~</U></body></html>"  
      jmail.HTMLBody = BODY
      JMail.AddRecipient "TINAH@cbbn.com.tw","HI-Building�`���f"
      JMail.AddRecipient "mis@cbbn.com.tw","��T��"
      JMail.AddRecipient "brian@cbbn.com.tw","�u�ȵ��f"
      IF CONSIGNEE="Y" THEN
       '  JMail.AddRecipient "tiffany01@cbbn.com.tw","�g�P�~�U���f"
         JMail.AddRecipient "EDSON@cbbn.com.tw","�Y�B"
      ELSE
         IF AREAID="A2" THEN
            JMail.AddRecipient "sally@cbbn.com.tw","�x���~�U"
            JMail.AddRecipient "elaine@cbbn.com.tw","�x���~�U"
         ELSEIF AREAID="A3" THEN
            JMail.AddRecipient "feng@cbbn.com.tw","�����~�U"
         ELSEIF AREAID="A1" AND (GROUPID="01" OR GROUPID="07") THEN '���
            JMail.AddRecipient "lilu@cbbn.com.tw","���~�U"
         ELSEIF AREAID="A1" AND (GROUPID="G1" OR GROUPID="G2" OR GROUPID="G3"  OR GROUPID="G4"  OR GROUPID="02" OR GROUPID="03" OR GROUPID="04" OR GROUPID="05" OR GROUPID="06") THEN '�x�_
            JMail.AddRecipient "tiffany01@cbbn.com.tw","�x�_�~�U"            
         END IF
      END IF
      jmail.Send ( "219.87.146.239" )      
   END IF   
   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "HI-Building�D�u���u�浲�ת��ন�\",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "���w�@�o�ɡA���i���浲�ת���" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "�����u���שΥ����u���פ�ҪťծɡA���i���浲�ת���" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close           
    elseIF frm1.htmlfld.value="5" then
       msgbox "���D�u���u�w�뵲�A���i����" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close      
    elseIF frm1.htmlfld.value="6" then
       msgbox "���D�u���u�浲�׮ɡA��������J��ڸ˾��H���ι�ڸ˾��g�P��" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                  
    elseIF frm1.htmlfld.value="7" then
       msgbox "���D�u�w���q�^��EBT�A���i���浲�ת���@�~" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                      
    else
       msgbox "�L�k����D�u���u�槹�u���ק@�~,���~�T��" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=rtebtcmtylinesndworkdrop.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>