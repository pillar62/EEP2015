<%
  Dim fieldRole,fieldPa,fieldPb,fieldpc,fieldpd,fieldpe
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
  userlevel=FrGetUserlevel(Request.ServerVariables("LOGON_USER"))
%>
<!-- #include virtual="/WebUtilityV4/DBaudi/zzDataList.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=5
  title="速博499已報竣客戶資料異動維護作業"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT COMQ1, LINEQ1, CUSID, MODIFYCODE, MODIFYDAT, MODIFYUSR, MODIFYDESC, DOCKETDAT, TRANSDAT, DROPDAT, DROPUSR "_
             &"FROM   RTSparq499CustChg where cusid='*' "

  sqllist    ="SELECT COMQ1, LINEQ1, CUSID, MODIFYCODE, MODIFYDAT, MODIFYUSR, MODIFYDESC, DOCKETDAT, TRANSDAT, DROPDAT, DROPUSR "_
             &"FROM   RTSparq499CustChg where "
 ' Response.write "SQL=" & SQLlist
 ' Response.end            
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userDefineRead="Yes"
  userDefineSave="Yes"
  userdefineactivex="Yes"  
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
'    Set connxx=Server.CreateObject("ADODB.Connection")  
'    Set rsxx=Server.CreateObject("ADODB.Recordset")
'    DSNXX="DSN=RTLIB"
'    connxx.Open DSNxx
'    sqlXX="SELECT COMN FROM RTCust499CmtyH where comq1=" & dspkey(0)
'    rsxx.Open sqlxx,connxx
'    s=""
'    If rsxx.Eof Then
'       message="社區代號:" &dspkey(0) &"在社區基本資料內找不到"
'       formvalud=false
'    Else 
'       dspkey(27)=rsxx("ComN") 
'    End If
'    rsxx.Close
'    Set rsxx=Nothing
'    connxx.Close
'    Set connxx=Nothing    
End Sub
' -------------------------------------------------------------------------------------------- 
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"    codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
's=FrGetCmtyDesc(SESSION("comq1"))
Set conn=Server.CreateObject("ADODB.Connection")
Set rs=Server.CreateObject("ADODB.Recordset")
conn.open DSN%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
</table>

<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="10%" class=dataListHead>社區序號</td>
    <td width="10%" bgcolor="silver">
		<input type="text" name="key0" <%=fieldRole(1)%> <%=keyProtect%> style="text-align:left;" maxlength="10" size="6"
               value="<%=dspKey(0)%>" readonly class=dataListdata></td>
    <td width="10%" class=dataListHead>主線序號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key1" <%=fieldRole(1)%><%=keyProtect%> style="text-align:left;" maxlength="10" size="10"
               value="<%=dspKey(1)%>" class=dataListdata></td>
    <td width="10%" class=dataListHead>客戶代號</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key2" readonly style="text-align:left;" maxlength="15" size="15"
			   value="<%=dspKey(2)%>" class=dataListdata></td>
</tr>

</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
        if len(trim(dspkey(5))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                dspkey(5)=V(0)
        End if  
       dspkey(4)=datevalue(now())
    
' -------------------------------------------------------------------------------------------- 
    '(1)己轉檔至中華電信(2)已作廢之異動 ==> 不可修改報竣日期且不可作廢
    If len(trim(dspKey(8))) > 0 or len(trim(dspkey(9))) > 0 Then
       fieldPa=" class=""dataListData"" readonly "
    Else
       fieldPa=""
    End If    
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
    <table border="1" width="100%" cellpadding="0" cellspacing="0">  

<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND ='K3' " 
       If len(trim(dspkey(3))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
    Else
       sql="SELECT Code,CodeNC FROM RTCode WHERE KIND='K3' AND CODE='" & dspkey(3) &"' " 
    End If
    
    rs.Open sql,conn
    s=""
    sx=""
    Do While Not rs.Eof
       If rs("CODE")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
%>        
<tr><td width="10%" class=dataListHead >異動項目</td>
    <td width="10%" bgcolor="silver">
		<select size="1" name="key3" <%=FIELDROLE(1)%><%=dataProtect%>  class="dataListEntry"><%=s%></select></td>

	<td width="10%" class=dataListHead>異動日期</td>
    <td width="10%" bgcolor="silver">
        <input type="text" name="key4" <%=fieldRole(1)%> <%=keyProtect%> 
               value="<%=dspKey(4)%>" maxlength="10" size="10" readonly  class=dataListData></td>
    </tr>

<%  
	name="" 
	if dspkey(5) <> "" then
		sql="select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
           &"where rtemployee.emply='" & dspkey(5) & "' "
		rs.Open sql,conn
	    if rs.eof then
			name=""
        else
			name=rs("cusnc")
        end if
		rs.close
    end if
%>    
    <tr><td width="25%" class="dataListHead" height="25">異動人員</td>
        <td width="25%" height="25" bgcolor="silver"> 
        <input type="text" name="key5" size="10" maxlength="10" value="<%=dspkey(5)%>" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata"><font size=2><%=name%></font></td>

		<td width="25%" class="dataListHead" height="25">異動說明</td>
        <td width="25%" height="25" bgcolor="silver"> 
        <input type="text" name="key6" size="50" maxlength="100" value="<%=dspkey(6)%>" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListEntry"></td> 
    </tr>        
    <tr><td width="25%" class="dataListHead" height="25">報竣日期</td>
        <td width="25%" height="25" bgcolor="silver"> 
        <input type="text" name="key7" size="10" maxlength="10" value="<%=dspkey(7)%>" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> class="dataListentry">
         <input type="button" id="B7"  name="B7"   width="100%" style="Z-INDEX: 1"  value="...." readonly onclick="Srbtnonclick()"  >  
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C7"  name="C7"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="SrClear">
        <td width="25%" class="dataListHead" height="25">轉檔日期</td>       
        <td width="25%" height="25" bgcolor="silver"> 
        <input type="text" name="key8" size="10" maxlength="10" value="<%=dspkey(8)%>" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata"></td> 
    </tr>
    <tr><td width="25%" class="dataListHead" height="25">作廢日期</td>
        <td width="25%" height="25" bgcolor="silver">
        <input type="text" name="key9" size="10" maxlength="10" value="<%=dspkey(9)%>" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata"></td>

<%  
	name="" 
	if dspkey(10) <> "" then
		sql="select cusnc from rtemployee inner join rtobj on rtemployee.cusid=rtobj.cusid " _
           &"where rtemployee.emply='" & dspkey(10) & "' "
		rs.Open sql,conn
	    if rs.eof then
			name=""
        else
			name=rs("cusnc")
        end if
		rs.close
    end if
%>    
	    <td width="25%" class="dataListHead" height="25">作廢人員</td>
        <td width="25%" height="25" bgcolor="silver">
        <input type="text" name="key10" size="10" maxlength="6" value="<%=dspkey(10)%>" <%=FIELDROLE(1)%><%=dataProtect%> class="dataListdata"><font size=2><%=name%></font></td>
    </tr>        
</table>                            

<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrReadExtDB()

End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase

'    Dim conn,rs
'    Set conn=Server.CreateObject("ADODB.Connection")
'    conn.open DSN
'    Set rs=Server.CreateObject("ADODB.Recordset")
'    rs.Open "SELECT COMQ1, LINEQ1, CUSID, DROPDAT, OVERDUE FROM RTSparq499Cust "_
'		   &"WHERE COMQ1=" &dspKey(0) &" AND LINEQ1=" & DSPKEY(1) &" AND CUSID ='" & DSPKEY(2) &"' ",conn,3,3
'    If rs.Eof Or rs.Bof Then
'    ELSE
'       rs("dropdat")=dspkey(7)
'       rs("overdue")=""
'       rs.Update
'       rs.Close
'    end if
'    conn.Close
'    Set rs=Nothing
'    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include virtual="/Webap/include/RTGetUserLevel.inc" -->
<!-- #include file="RTGetCmtyDesc.inc" -->
