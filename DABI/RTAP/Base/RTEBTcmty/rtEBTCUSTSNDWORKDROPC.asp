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
   DSN="DSN=RtLib"
   connXX.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTEBTCUSTsndwork WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and prtno='" & key(3) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,CONNxx
   endpgm="1"
   '�������p��~��w�s�b��Ʈɪ��ܸӵ���Ƨ��u������뤧�����w����,���i�A����
   IF LEN(TRIM(RSXX("bonuscloseym"))) <> 0 THEN
      ENDPGM="3"
   elseif LEN(TRIM(RSXX("dropdat"))) = 0 then
      endpgm="4"
   ELSE
      sqlyy="select max(entryno) as entryno FROM RTEBTCUSTsndworklog WHERE comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and prtno='" & key(3) & "' "
      rsyy.Open sqlyy,connxx
      
      if len(trim(rsyy("entryno"))) > 0 then
         entryno=rsyy("entryno") + 1
      else
         entryno=1
      end if
      rsyy.close
      set rsyy=nothing
      sqlyy="insert into RTEBTCUSTsndworklog " _
           &"SELECT   COMQ1, LINEQ1, CUSID,PRTNO, " & ENTRYNO & ", getdate(), 'R','" &  v(0) & "', " _
           &"SENDWORKDAT, PRTUSR, ASSIGNENGINEER,ASSIGNCONSIGNEE, REALENGINEER, REALCONSIGNEE, DROPDAT,'AVS�Τᵤ�u�T�{��@�o����', " _
           &"CLOSEDAT, BONUSCLOSEYM, BONUSCLOSEDAT, " _
           &"BONUSCLOSEUSR, BONUSFINCHK, STOCKCLOSEYM, STOCKCLOSEDAT, " _
           &"STOCKCLOSEUSR, STOCKFINCHK, MDF1,MDF2, HOSTNO,HOSTPORT, MEMO, PRTDAT,UNCLOSEDAT,DROPUSR,CLOSEUSR  " _
           &"FROM RTEBTCUSTsndwork where comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and prtno='" & key(3) & "' "
     ' Response.Write sqlyy
      CONNXX.Execute sqlyy     
      If Err.number > 0 then
         endpgm="2"
         errmsg=cstr(Err.number) & "=" & Err.description
      else
         SQLXX=" update RTEBTCUSTsndwork set dropdat=null,dropdesc='�w�@�o����' where comq1=" & KEY(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and prtno='" & key(3) & "' "
         connxx.Execute SQLXX
         If Err.number > 0 then
            endpgm="2"
            '�o�Ϳ��~�ɡA�R�������ɩҷs�W�����ʸ��
            errmsg=cstr(Err.number) & "=" & Err.description
            sqlyy="delete * FROM RTEBTCUSTsndworklog WHERE comq1=" & key(0) & " and lineq1=" & key(1) & " AND CUSID='" & KEY(2) & "' and prtno='" & key(3) & "' AND ENTRYNO=" & ENTRYNO
            CONNXX.Execute sqlyy 
         else
            endpgm="1"
            errmsg=""
         end if      
      end if
   END IF
   RSXX.CLOSE
   connXX.Close
   SET RSXX=NOTHING
   set connXX=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "�Τᵤ�u��@�o���ন�\",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "�������p��~��w�s�b��Ʈɪ��ܸӵ���Ƨ��u������뤧�����w����,���i�A�@�o����" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "���Τᵤ�u��|���@�o�A���i����@�o����@�~�G" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    else
       msgbox "�L�k����Τᵤ�u��@�o����@�~,���~�T���G" & "  " & frm1.htmlfld1.value
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