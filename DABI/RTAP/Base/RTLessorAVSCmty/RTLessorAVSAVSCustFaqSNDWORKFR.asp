<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM CONN
   Set conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")  
   DSN="DSN=RtLib"
   conn.Open DSN
   'conn.BeginTrans(���STORE PROCEDURE������TRANSACTION�BCOMMIT�BROLLBACK)
   '�]����STORE PROCEDURE��OPEN�ӦhTABLE�ɡAASP�L�k����CURSOR�ӷ|�o�Ϳ��~(�����A�i�H�NBEGIN�BCOMMIT�BROLLBACK��MARK�����ð����Y��)
   '�ˬd�Ȥ���
   sqlxx="select * FROM RTLessorAVSCUST WHERE CUSID='" & KEY(0) & "' "
   sqlyy="select * FROM RTLessorAVSCustFaqH WHERE CUSID='" & KEY(0) & "' and Faqno='" & key(1) & "' "
   rsyy.open sqlyy,conn
   rsxx.open sqlxx,conn
   if rsxx.eof then
      '�䤣��Ȥ����
      endpgm="7"
   elseif rsyy.eof then
      '�䤣��Ȥ�ȪA����
      endpgm="13"
   elseif LEN(TRIM(RSXX("cancelDAT"))) > 0 then
      '�Ȥ�w�@�o,���i���ת���
      endpgm="8"
   elseif LEN(TRIM(RSyy("callbackdat"))) > 0 then
      '�ȪA��w��^�Ф�A���i����
      endpgm="9"      
   elseif LEN(TRIM(RSyy("finishdat"))) > 0 then
      '�ȪA��w���סA���i����
      endpgm="10"          
   elseif LEN(TRIM(RSyy("cancelDAT"))) > 0 then
      '�Ȥ�ȪA���Ƥw�@�o,���i���ת���
      endpgm="14"      
   '�Ȧs�ȪA���ɤ������u��B���u�渹�B���u�H��
   ELSE
      XXSNDWORK=RSYY("SNDWORK")
      XXSNDUSR=RSYY("SNDUSR")
      XXSNDPRTNO=RSYY("SNDPRTNO")
   END IF
   rsxx.close
   rsyy.close

'�W�z���T��
if endpgm="" then
   endpgm="1"
  
   sqlxx="select * FROM RTLessorAVSCustfaqsndwork WHERE CUSID='" & KEY(0) & "' and faqno='" & key(1) & "' and prtno='" & key(2) & "' "
   RSXX.OPEN SQLXX,CONN
   BATCHNOXX=RSXX("BATCHNO")
   '�ˬd�����b���ɬO�_�w�R�b
   sqlyy="select * FROM RTLessorAVSCUSTAR WHERE CUSID='" & KEY(0) & "' AND BATCHNO='" & BATCHNOXx & "'"
   rsyy.open sqlyy,conn
   if rsyy.eof THEN
   ELSE
      if len(trim(rsyy("mdat")))>0 OR RSYY("REALAMT") > 0 then
      '�����b�ڤw�R�b���i���ת���
         endpgm="4"
      end if
   end if
   RSyy.CLOSE     
   if endpgm="1" then    
   '�����u��|�����׮ɡA���i����
   IF isnull(RSXX("CLOSEDAT")) and isnull(RSXX("unCLOSEDAT"))  THEN
      ENDPGM="3"
   elseif len(trim(rsxx("dropdat"))) > 0 then
      ENDPGM="5"
   '�p�G�ȪA��D�ɤw���䥦���u�渹�ɡA�h�����\���楼���u���ת���
   '(���u���ת��ण�b�����A��]�O���u���ת���u���M�����u�浲�פ�A�ӥ����u���ת���h�|�s�P���u�渹�B���u��@�_��s)
   ELSEIF (LEN(TRIM(XXSNDPRTNO)) > 0 OR LEN(TRIM(XXSNDWORK)) > 0 ) AND LEN(TRIM(RSXX("unCLOSEDAT")))>0 THEN
      ENDPGM="6"
   ELSE
      '�I�sstore procedure��s�����ɮ�
      strSP="usp_RTLessorAVSCustFaqSndworkFR " & "'" & key(0) & "','" & key(1) & "','" & key(2) & "','" & V(0) & "','" & batchnoxx & "'" 
      Set ObjRS = conn.Execute(strSP)
      If Err.number = 0 then
         ENDPGM="1"
         ERRMSG=""
         'conn.CommitTrans
      else
         ENDPGM="2"
         errmsg=cstr(Err.number) & "=" & Err.description
         'conn.rollbackTrans
      end if         
   END IF
   end if
   RSXX.CLOSE
end if
conn.Close
SET RSXX=NOTHING
SET RSYY=NOTHING
set conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "AVS-City�Τ���׬��u�浲�ת��ন�\",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "�����u��|�����סA���i���浲�ת���@�~" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="4" then
       msgbox "�����b�ڤw�R�b�A���i���浲�ת���@�~(�лP��T���sô)" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="5" then
       msgbox "�����׬��u��w�@�o�A���i����C" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close              
    elseIF frm1.htmlfld.value="6" then
       msgbox "�����׬��u����ݫȪA��w���ͨ䥦���u��A�]��������榹���u�����@�~�C" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                     
   elseIF frm1.htmlfld.value="7" then
       msgbox "�䤣��Ȥ���ɡA�L�k����C" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="8" then
       msgbox "�Ȥ��Ƥw�@�o�A�L�k����C" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
   elseIF frm1.htmlfld.value="9" then
       msgbox "�ȪA��w��^�Ф�A���i���欣�u�浲�ת���C" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
   elseIF frm1.htmlfld.value="10" then
       msgbox "�ȪA��w���סA���i���欣�u�浲�ת���C" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                          
    elseIF frm1.htmlfld.value="13" then
       msgbox "�䤣��Ȥ�ȪA�����ɡA�L�k����C" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close            
    elseIF frm1.htmlfld.value="14" then
       msgbox "�Ȥ�ȪA���Ƥw�@�o�A���i����C" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                   
    else
       msgbox "�L�k������׬��u�槹�u���ק@�~,���~�T��" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorAVSCustFAQsndworkf.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>