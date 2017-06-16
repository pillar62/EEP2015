
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=3
  title="先看先贏客戶電話訪談記錄維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT  CUSID, ENTRYNO, SEQ1, TELVISITDAT, ACCOUNTRCV, VISITMAN, CONTENTSCORE, DROPDAT " _
             &"FROM RTSS365tel "
  sqlList="SELECT  CUSID, ENTRYNO, SEQ1, TELVISITDAT, ACCOUNTRCV, VISITMAN, CONTENTSCORE, DROPDAT " _
         &"FROM RTSS365tel where "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  userdefineactivex="Yes"    
  extDBField=99
  extdbfield2=99
  extdbfield3=99
  extdbfield4=99
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(trim(dspkey(2))) < 1 then dspkey(2)=0
    If len(trim(dspKey(3))) < 1 Then
       formValid=False
       message="電訪日期不可空白"
    elseif len(trim(dspKey(5))) < 1 Then
       formValid=False
       message="電訪人員不可空白"
    end if        

End Sub
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
	   if isdate(document.all(clickkey).value) then
	      objEF2KDT.varDefaultDateTime=document.all(clickkey).value
       end if
       call objEF2KDT.show(1)
       if objEF2KDT.strDateTime <> "" then
          document.all(clickkey).value = objEF2KDT.strDateTime
       end if
   End Sub 
   Sub SrClear()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="C" & clickid
       clearkey="key" & clickid       
       if len(trim(document.all(clearkey).value)) <> 0 then
          document.all(clearkey).value =  ""
       end if
   End Sub       
   Sub ImageIconOver()
       self.event.srcElement.style.borderBottom = "black 1px solid"
       self.event.srcElement.style.borderLeft="white 1px solid"
       self.event.srcElement.style.borderRight="black 1px solid"
       self.event.srcElement.style.borderTop="white 1px solid"   
   End Sub
   
   Sub ImageIconOut()
       self.event.srcElement.style.borderBottom = ""
       self.event.srcElement.style.borderLeft=""
       self.event.srcElement.style.borderRight=""
       self.event.srcElement.style.borderTop=""
   End Sub        
   </Script>
<%   
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveX() %>
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"     codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
SQL="SELECT SHORTNC FROM RTObj where (CUSID = '" & dspkey(0) & "') "
conn.Open dsn
rs.Open sql,conn
if not rs.EOF then
   cusnc=rs("shortnc")
else
   cusnc=""
end if
rs.close
set rs=nothing
if len(trim(aryparmkey(2))) > 0 then
   dspkey(2)=aryparmkey(2)
end if
%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="16%" class=dataListHead>客戶代號</td><td width="24%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0" <%=keyprotect%> size="10" 
           value="<%=dspKey(0)%>" maxlength="10" ><font size=2><%=cusnc%></font></td>
           <td width="16%" class=dataListHead>單次</td><td width="16%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key1" <%=keyprotect%> size="4" 
           value="<%=dspKey(1)%>" maxlength="4" ></td>
           <td width="16%" class=dataListHead>電訪序號</td><td width="16%" bgcolor=silver>
           <input readonly class=dataListdata type="text" name="key2" <%=keyprotect%> size="4" 
           value="<%=dspKey(2)%>" maxlength="4" ></td>                      
       </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-----EXTDB DATA RETRIVE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
conn.Open dsn
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">電訪日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key3" <%=dataprotect%>  readonly maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(3)%>">
          <input type="button" id="B3"  name="B3" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
             <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C3"  name="C3"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="Srclear">            </td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">帳號收件日</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input readonly class=dataListEntry name="key4"  <%=dataprotect%> maxlength=10  readonly size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>">
          <input type="button" id="B4"  name="B4" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
             <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C4"  name="C4"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="Srclear"> </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">電訪人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
<%
    'If (sw="E" Or (accessMode="A" And sw=""))  Then 
       sql="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID WHERE (RTEmployee.DEPT = 'C300') "
    'Else
    '   SQL="SELECT RTEmployee.EMPLY, RTObj.CUSNC FROM RTEmployee INNER JOIN " _
    '      &"RTObj ON RTEmployee.CUSID = RTObj.CUSID WHERE (RTEmployee.DEPT = 'C300') and rtemployee.emply='" & dspkey(5) & "'"
    'End If
    rs.Open sql,conn
    s=""
    If rs.Eof Then 
       s="<option value="""" selected>&nbsp;</option>"
    else
       sx=""
       s="<option value="""">&nbsp;</option>" & vbcrlf      
       Do While Not rs.Eof
          If rs("emply")=dspKey(5) Then sx=" selected "
          s=s &"<option value=""" &rs("emply") &"""" &sx &">" &rs("cusnc") &"</option>" & vbcrlf
          rs.MoveNext
          sx=""
       Loop
    end if
    rs.Close
%>
        <select name="key5" <%=dataProtect%> size="1"    
               style="text-align:left;" maxlength="8" class="dataListEntry"><%=s%></select>    
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">產品滿意度</font></td>
    <td width="35%" bgcolor="#C0C0C0">
         <% aryOption=Array("","1","2","3","4","5")
            s=""
            For i = 0 To Ubound(aryOption)
                   If dspKey(6)=aryOption(i) Then
                      sx=" selected "
                   Else
                      sx=""
                   End If
                   select case aryoption(i)
                        case "1"
                             aryY="非常滿意"
                        case "2"
                             aryY="滿意"
                        case "3"
                             aryY="可接受"
                        case "4"
                             aryY="不滿意"
                        case "5"
                             aryY="非常不滿意"
                   end select   
                   s=s &"<option value=""" &aryOption(i) &"""" &sx &">" & aryY &"</option>"
            Next
       '     Else
       '            select case aryoption(i)
       '                 case "1"
       '                      aryY="非常滿意"
       '                 case "2"
       '                      aryY="滿意"
       '                 case "3"
       '                      aryY="可接受"
       '                 case "4"
       '                      aryY="不滿意"
       '                 case "5"
       '                      aryY="非常不滿意"
       '            end select   
       '            s="<option value=""" &dspKey(6) &""">" &aryY &"</option>"
         %>               
         <select size="1" name="key6"<%=dataProtect%> class="dataListEntry">                                            
           <%=s%>
         </select>    </td> 
         </tr>
    <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">作廢日期</font></td>
    <td width="35%" colspan="3" bgcolor="#C0C0C0">
     <input readonly class=dataListEntry name="key7"  <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>">
          <input type="button" id="B7"  name="B7" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
             <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C7"  name="C7"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="Srclear">            </td>
  </tr>
  </table>
  <table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr><td width="15%" colspan="3" bgcolor="#a4bcdb" align="CENTER"><font color="#000000">連絡紀錄</font></td></tr>
  <tr><td width="33%" bgCOLOR="DARKSEAGREEN" align="center">電訪紀錄</td><td width="34%"  bgCOLOR="DARKSEAGREEN"  align="center">客戶反應</td><td width="33%"  bgCOLOR="DARKSEAGREEN" align="center">紀錄</td></tr>
  <tr>
  <td BGCOLOR="LIGHTyellow">
 <%
    IF LEN(TRIM(DSPKEY(2))) = 0 THEN DSPKEY(2)=0
    sqlteld1="SELECT RTCode.KIND, RTCode.CODE, RTCode.CODENC, RTSS365D1.CUSID, RTSS365D1.ENTRYNO, RTSS365D1.SEQ1 " _
            &"FROM RTCode LEFT OUTER JOIN RTSS365D1 ON RTCode.KIND = RTSS365D1.KIND AND RTCode.CODE = RTSS365D1.CODE " _
            &"and rtss365d1.cusid='" & dspkey(0) & "' and " _
            &"rtss365d1.entryno=" & dspkey(1) & " and rtss365d1.seq1=" & DSPKEY(2) _
            &"WHERE (RTCode.KIND = 'C5') "
  ' Response.Write "sqlteld1=" & sqlteld1
    rs.open sqlteld1,conn,1,1
    Dtlcnt=0
    Do until rs.eof
       IF not IsNull(RS("cusid")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="ext<%=Dtlcnt%>"  <%=fieldpc%> value="<%=rs("code")%>"><%=rs("codenc")%></p>  
  <%
    dtlcnt=Dtlcnt+1
    rs.MoveNext
    loop
    rs.close
  %>                          
  </td>
  <td  BGCOLOR="LIGHTyellow">
   <%
       IF LEN(TRIM(DSPKEY(2))) = 0 THEN DSPKEY(2)=0
    sqlteld1="SELECT RTCode.KIND, RTCode.CODE, RTCode.CODENC, RTSS365D1.CUSID, RTSS365D1.ENTRYNO, RTSS365D1.SEQ1 " _
            &"FROM RTCode LEFT OUTER JOIN RTSS365D1 ON RTCode.KIND = RTSS365D1.KIND AND RTCode.CODE = RTSS365D1.CODE " _
            &"and rtss365d1.cusid='" & dspkey(0) & "' and " _
            &"rtss365d1.entryno=" & dspkey(1) & " and rtss365d1.seq1=" & DSPKEY(2) _
            &"WHERE (RTCode.KIND = 'C6') "
    rs.open sqlteld1,conn,1,1
    Dtlcnt=0
    Do until rs.eof
       IF not IsNull(RS("cusid")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="exta<%=Dtlcnt%>"  <%=fieldpc%> value="<%=rs("code")%>"><%=rs("codenc")%></p>  
  <%
    dtlcnt=Dtlcnt+1
    rs.MoveNext
    loop
    rs.close
  %>
  </td>
  <td  BGCOLOR="LIGHTyellow">
   <%
       IF LEN(TRIM(DSPKEY(2))) = 0 THEN DSPKEY(2)=0
    sqlteld1="SELECT RTCode.KIND, RTCode.CODE, RTCode.CODENC, RTSS365D1.CUSID, RTSS365D1.ENTRYNO, RTSS365D1.SEQ1 " _
            &"FROM RTCode LEFT OUTER JOIN RTSS365D1 ON RTCode.KIND = RTSS365D1.KIND AND RTCode.CODE = RTSS365D1.CODE " _
            &"and rtss365d1.cusid='" & dspkey(0) & "' and " _
            &"rtss365d1.entryno=" & dspkey(1) & " and rtss365d1.seq1=" & DSPKEY(2) _
            &"WHERE (RTCode.KIND = 'C7') "
    rs.open sqlteld1,conn,1,1
    Dtlcnt=0
    Do until rs.eof
       IF not IsNull(RS("cusid")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="extb<%=Dtlcnt%>"  <%=fieldpc%> value="<%=rs("code")%>"><%=rs("codenc")%></p>  
  <%
    dtlcnt=Dtlcnt+1
    rs.MoveNext
    loop
    rs.close
  %>                      
  </td></tr>
  <TR><TD COLSPAN="3" BGCOLOR="DARKSEAGREEN" align="center">其它原因說明</TD></TR>
  <TR>
  <TD  ALIGN="CENTER" bgcolor="silver">
   <%
    IF LEN(TRIM(DSPKEY(2))) = 0 THEN DSPKEY(2)=0
    sqlteld2="SELECT * FROM RTSS365d2 " _
            &"WHERE RTSS365d2.CUSID = '" & DSPKEY(0) & "' AND RTSS365D2.ENTRYNO=" & DSPKEY(1) _
            &" AND RTSS365D2.SEQ1=" & dspkey(2) & " AND RTSS365D2.KIND='C5'"
    rs.open sqlteld2,conn,1,1
    IF RS.EOF THEN
       s1="" 
    ELSE
       s1=RS("OTHERDESC")
    END IF
    rs.close
  %>                        
 <font ><TEXTAREA  COLS="33%"  bgcolor="lightyellow" name="extC0" rows=10  value="<%=s1%>"><%=s1%></TEXTAREA></font>
  </TD>
  <TD  ALIGN="CENTER" bgcolor="silver" >
   <%
    IF LEN(TRIM(DSPKEY(2))) = 0 THEN DSPKEY(2)=0
    sqlteld2="SELECT * FROM RTSS365d2 " _
            &"WHERE RTSS365d2.CUSID = '" & DSPKEY(0) & "' AND RTSS365D2.ENTRYNO=" & DSPKEY(1) _
            &" AND RTSS365D2.SEQ1=" & dspkey(2) & " AND RTSS365D2.KIND='C6'"
    rs.open sqlteld2,conn,1,1
    IF RS.EOF THEN
      s2="" 
    ELSE
       s2=RS("OTHERDESC")
    END IF
    rs.close
  %>                
  <font ><TEXTAREA COLS="34%"  bgcolor="lightyellow"   name="extC1" rows=10  value="<%=s2%>"><%=s2%></TEXTAREA></font>
  </TD>
  <TD  ALIGN="CENTER"  bgcolor="silver">
   <%
    IF LEN(TRIM(DSPKEY(2))) = 0 THEN DSPKEY(2)=0
    sqlteld2="SELECT * FROM RTSS365d2 " _
            &"WHERE RTSS365d2.CUSID = '" & DSPKEY(0) & "' AND RTSS365D2.ENTRYNO=" & DSPKEY(1) _
            &" AND RTSS365D2.SEQ1=" & dspkey(2) & " AND RTSS365D2.KIND='C7'"
    rs.open sqlteld2,conn,1,1
    IF RS.EOF THEN
       s3="" 
    ELSE
       s3=RS("OTHERDESC")
    END IF
    rs.close
  %>                
  <font ><TEXTAREA COLS="33%" bgcolor="lightyellow"    name="extC2" rows=10  value="<%=s3%>"><%=s3%></TEXTAREA></font>
  </TD>
  </TR>
</table>

<% conn.close
   set rs=nothing
   set conn=nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set comm=Server.CreateObject("ADODB.Command")
    rs.open "select max(seq1) AS SEQ1 from rtSS365TEL",conn
    '新增時,由於seq1為identify欄位故須當master file寫檔時才會產生,產生後由主檔挑選最大值即為該筆資料之seq1
    '若為修改時,則由master file的seq1值為主
    if smode="A" then
       if not rs.eof then
          dspkey(2)=rs("SEQ1")
       end if
    else
       dspkey(2)=aryparmkey(2)
    end if
    rs.close           
'---BEGIN TRANsaction 
    conn.begintrans
  '  on error resume next    
'--------------------------------------------------------
    DELSS365D1="delete from rtSS365D1 where CUSID='" & dspkey(0) & "' AND ENTRYNO=" & DSPKEY(1) & " and SEQ1=" & DSPKEY(2)
    conn.Execute delSS365d1 
    For i=0 to 99
        if len(trim(extdb(i))) > 0  then
           rs.Open "SELECT * FROM RTSS365D1 WHERE cusid='" &dspKey(0) &"' and entryno=" & dspkey(1) & " and seq1=" _
                   & dspkey(2) & " and kind='C5' and code='" & extdb(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("cusid")=dspKey(0)
              rs("entryno")=dspkey(1)
              rs("seq1")=dspkey(2)
              rs("kind")="C5"
              rs("code")=extdb(i)          
           End If
           rs.Update
           rs.Close
        end if
    Next
    For i=0 to 99
        if len(trim(extdb2(i))) > 0  then
           rs.Open "SELECT * FROM RTSS365D1 WHERE cusid='" &dspKey(0) &"' and entryno=" & dspkey(1) & " and seq1=" _
                   & dspkey(2) & " and kind='C6' and code='" & extdb2(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("cusid")=dspKey(0)
              rs("entryno")=dspkey(1)
              rs("seq1")=dspkey(2)
              rs("kind")="C6"
              rs("code")=extdb2(i)  
           End If
           rs.Update
           rs.Close              
        end if
    Next    
    For i=0 to 99
        if len(trim(extdb3(i))) > 0  then
           rs.Open "SELECT * FROM RTSS365D1 WHERE cusid='" &dspKey(0) &"' and entryno=" & dspkey(1) & " and seq1=" _
                   & dspkey(2) & " and kind='C7' and code='" & extdb2(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("cusid")=dspKey(0)
              rs("entryno")=dspkey(1)
              rs("seq1")=dspkey(2)
              rs("kind")="C7"
              rs("code")=extdb3(i)  
           End If
           rs.Update
           rs.Close              
        end if
    Next    
'--------------------------------------------------------
    DELSS365D2="delete from rtSS365D2 where CUSID='" & dspkey(0) & "' AND ENTRYNO=" & DSPKEY(1) & " and SEQ1=" & DSPKEY(2)
    conn.Execute delSS365d2
    rs.Open "SELECT * FROM RTSS365D2 WHERE cusid='" &dspKey(0) &"' and entryno=" & dspkey(1) & " and seq1=" _
            & dspkey(2) & " and kind='C5'" ,conn,3,3    
    if len(trim(extdb4(0))) > 0  then
           rs.AddNew
           rs("cusid")=dspKey(0)
           rs("entryno")=dspkey(1)
           rs("seq1")=dspkey(2)
           rs("kind")="C5"
           rs("otherdesc")=extdb4(0)  
           rs.Update
    end if     
    rs.close
    rs.Open "SELECT * FROM RTSS365D2 WHERE cusid='" &dspKey(0) &"' and entryno=" & dspkey(1) & " and seq1=" _
            & dspkey(2) & " and kind='C6'" ,conn,3,3    
    if len(trim(extdb4(1))) > 0  then
           rs.AddNew
           rs("cusid")=dspKey(0)
           rs("entryno")=dspkey(1)
           rs("seq1")=dspkey(2)
           rs("kind")="C6"
           rs("otherdesc")=extdb4(1)
           rs.Update
    end if         
    rs.close
    rs.Open "SELECT * FROM RTSS365D2 WHERE cusid='" &dspKey(0) &"' and entryno=" & dspkey(1) & " and seq1=" _
            & dspkey(2) & " and kind='C7'" ,conn,3,3    
    if len(trim(extdb4(2))) > 0  then
           rs.AddNew
           rs("cusid")=dspKey(0)
           rs("entryno")=dspkey(1)
           rs("seq1")=dspkey(2)
           rs("kind")="C7"
           rs("otherdesc")=extdb4(2)
           rs.Update
    end if             
    rs.close
    if err.number <> 0 then
       conn.rollbacktrans
    else
       conn.commitTrans
    end if
    conn.close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
%>
