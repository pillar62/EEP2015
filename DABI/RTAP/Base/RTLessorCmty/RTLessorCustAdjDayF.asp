<%@ Language=VBScript %>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<% KEY=SPLIT(REQUEST("KEY"),";")
   logonid=session("userid")
   Call SrGetEmployeeRef(Rtnvalue,1,logonid)
         V=split(rtnvalue,";")  
   DIM Conn
   Set Conn=Server.CreateObject("ADODB.Connection")  
   SET RSXX=Server.CreateObject("ADODB.RECORDSET")  
   SET RSyy=Server.CreateObject("ADODB.RECORDSET")
   DSN="DSN=RtLib"
   Conn.Open DSN
 '  On Error Resume Next
   sqlxx="select * FROM RTLessorCustADJDAY WHERE CUSID='" & KEY(0) & "' AND ENTRYNO=" & KEY(1)
   sqlYY="select * FROM RTLessorCust WHERE CUSID='" & KEY(0) & "' "
   'RESPONSE.Write SQLXX
 '  RESPONSE.END
   RSXX.OPEN SQLXX,Conn
   RSYY.OPEN SQLYY,Conn
   endpgm="1"
   '���Τ�w���סA���i���Ƶ���
   IF LEN(TRIM(RSXX("ADJCLOSEDAT"))) <> 0  THEN
      ENDPGM="3"
   '���Τ�w�@�o�ɡA���i����
   elseif LEN(TRIM(RSXX("CANCELdat"))) <> 0 then
      endpgm="4"
   '���Τ�D�ɸ�Ƭ��w�@�o�Τw�h���ɡA���i����
   elseif LEN(TRIM(RSYY("CANCELdat"))) <> 0 OR LEN(TRIM(RSYY("DROPDAT"))) <> 0 then
      endpgm="5"      
   '���Τ�D�ɤ����Ƹ�ƻP�վ��Ƥ����Ƭۥ[��G�p��s�̡A�h�����\����
   elseif RSYY("PERIOD") + RSXX("ADJPERIOD") < 0 then
      endpgm="6"           
   ELSE
     '�ˬd�����վ��O�_�p��}�l�p�O�������}�l��
     XXDUEDAT=RSYY("DUEDAT")
     XXSTRBILLINGDAT=RSYY("STRBILLINGDAT")
     XXNEWBILLINGDAT=RSYY("NEWBILLINGDAT")
     XXADJPERIOD=RSXX("ADJPERIOD")
     XXADJDAY=RSXX("ADJDAY")
     XX2DUEDAT=DATEADD("m",XXADJPERIOD,XXDUEDAT)
     XX2DUEDAT=DATEADD("d",XXADJdat,XXDUEDAT)
     if len(trim(xxnewbillingdat)) > 0 and XX2DUEDAT < xxnewbillingdat then
        endpgm="7"   
     elseif len(trim(XXSTRBILLINGDAT)) > 0 and XX2DUEDAT < xxstrbillingdat then
        endpgm="7"   
     else
       '�I�sstore procedure��s�����ɮ�
        strSP="usp_RTLessorCustAdjDayF " & "'" & key(0) & "'" & "," & key(1) & ",'" & V(0) & "'" 
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
     end if
   END IF
   RSXX.CLOSE
   RSYY.CLOSE
   Conn.Close
   SET RSXX=NOTHING
   SET RSYY=NOTHING
   set Conn=nothing
   
%> 
<HTML>
<Head>
<script language=vbscript>
 sub window_onload()
    if frm1.htmlfld.value="1" then
       msgbox "ET-City�Τ�վ�����Ƹ�Ƶ��צ��\",0
       Set winP=window.Opener
       Set docP=winP.document       
       docP.all("keyform").Submit
       winP.focus()              
       window.CLOSE
    elseIF frm1.htmlfld.value="3" then
       msgbox "���Τ�w���סA���i���Ƶ���" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close       
    elseIF frm1.htmlfld.value="4" then
       msgbox "���Τ�w�@�o�ɡA���i���סC" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close             
    elseIF frm1.htmlfld.value="5" then
       msgbox "���Τ�D�ɸ�Ƭ��w�@�o�Τw�h���ɡA���i���סC" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    elseIF frm1.htmlfld.value="6" then
       msgbox "���Τ�D�ɤ����Ƹ�ƻP�վ��Ƥ����Ƭۥ[��G�p��s�̡A�h�����\���סC" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close          
    elseIF frm1.htmlfld.value="7" then
       msgbox "���Τ�D�ɤ������g���վ��ƽվ��䵲�G�|�p��}�l�p�O��(������}�l��)�A<br>�]�������\���סC" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close                
    else
       msgbox "�L�k����Τ�վ�����Ƹ�Ƶ��ק@�~,���~�T��" & "  " & frm1.htmlfld1.value
       Set winP=window.Opener
       winP.focus()
       window.close
    end if
   ' window.close    
 end sub
</script> 
</head>  
<form name=frm1 method=post action=RTLessorCustADJDAYF.asp ID="Form1">
<input type="text" name=HTMLfld style=display:none value="<%=endpgm%>" ID="Text1">
<input type="text" name=HTMLfld1 style=display:none value="<%=errmsg%>" ID="Text2">
</form>
</html>