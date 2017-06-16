<%@ Transaction = required %>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=3
  title="月結控制檔資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT cyy,cmm,areaid,closing,lastrundat,lastrunusr,lastrstdat,lastrstusr,runcnt,runlock " _
             &"FROM RTClosingCtl WHERE cyy=0 "
  sqlList="SELECT cyy,cmm,areaid,closing,lastrundat,lastrunusr,lastrstdat,lastrstusr,runcnt,runlock " _
             &"FROM RTClosingCtl WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if Not IsNumeric(dspkey(0)) then
       formValid=False
       message="年度資料不正確"
    elseif not IsNumeric(dspkey(1)) Then
       formValid=False
       message="月份資料不正確"
    elseif not IsNumeric(dspkey(8)) Then
       formValid=False
       message="執行總次數資料不正確"       
    End If    
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
    If dspkey(3)="Y" or dspkey(9)="Y" or UCASE(TRIM(dataprotect))="READONLY" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=""
       fieldpc=""
       fieldpd=""
    Else
       fieldPa=""
       fieldpb=" onclick=""SrBtnOnClick"" "
       fieldpc=" onclick=""SrSelOnClick"" "       
       fieldpD=" onclick=""SrClear"" "              
    End If%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="15%" class=dataListHead>年度</td><td width="15%" bgcolor=silver>
           <input <%=fieldpa%>  class=dataListEntry type="text" name="key0" <%=keyprotect%> size="4" 
           value="<%=dspKey(0)%>" maxlength="4" ></td>
           <td width="15%" class=dataListHead>月份</td><td width="15%" bgcolor=silver>
           <input <%=fieldpa%>  class=dataListEntry type="text" name="key1" <%=keyprotect%> size="2" 
           value="<%=dspKey(1)%>" maxlength="2" ></td>  
           <td width="15%" class=dataListHead>轄區</td><td width="15%" bgcolor=silver>
  <%
    DIM conn,rs,dsn,sql
    SET conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    DSN="DSN=RTLIB"  
    s=""
    sx=" selected " 
   ' Response.Write "SW=" & SW &";" & "MODE=" & accessmode
    If  (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM rtarea where areatype='1' "
       If len(trim(dspkey(2))) =0 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  & vbcrlf
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>" & vbcrlf 
       end if            
    Else
       sql="SELECT * FROM rtarea where areatype='1' and areaid='" &dspkey(2) &"' " & vbcrlf
    End if 
    conn.Open dsn        
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("areaid")=dspkey(2) Then sx=" selected "
          s=s &"<option value=""" &rs("areaid") &"""" &sx &">" &trim(rs("areanc")) &"</option>" & vbcrlf
          rs.MoveNext
          sx=""
    Loop
    rs.Close
    conn.close
    set rs=nothing
    set conn=nothing
%>
    <select <%=fieldpa%>   class=dataListEntry name="key2" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8"><%=s%></select>                    
   </td>                     
       </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
'-----EXTDB DATA RETRIVE
    if len(trim(dspkey(8)))=0 then dspkey(8)=0
    If dspkey(3)="Y" or dspkey(9)="Y" or UCASE(TRIM(dataprotect))="READONLY" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=""
       fieldpc=""
       fieldpd=""
    Else
       fieldPa=""
       fieldpb=" onclick=""SrBtnOnClick"" "
       fieldpc=" onclick=""SrSelOnClick"" "       
       fieldpD=" onclick=""SrClear"" "              
    End If
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">月結</font></td>
    <td width="35%" bgcolor="#C0C0C0" colspan="3">
     <input <%=fieldpa%> class=dataListEntry name="key3" <%=dataprotect%> maxlength=1 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(3)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">最後月結日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input <%=fieldpa%>  class=dataListEntry name="key4"  <%=dataprotect%> maxlength=10 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">最後月結人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input <%=fieldpa%>  class=dataListEntry name="key5" <%=dataprotect%> maxlength=6 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">最後回復日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
     <input <%=fieldpa%>  class=dataListentry name="key6" <%=dataprotect%> maxlength=10 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(6)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">最後回復人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input <%=fieldpa%>  class=dataListentry name="key7" <%=dataprotect%> maxlength=6 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">執行總次數</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input <%=fieldpa%>  class=dataListEntry name="key8" <%=dataprotect%> maxlength=5 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(8)%>">
    　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">執行鎖定</font>　</td>
    <td width="35%" bgcolor="#C0C0C0">
     <input <%=fieldpa%>  class=dataListEntry name="key9" <%=dataprotect%> maxlength=1 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(9)%>">　</td>
  </tr>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'--------------SAVE RTOBJ FILE

End Sub
' -------------------------------------------------------------------------------------------- 
%>
