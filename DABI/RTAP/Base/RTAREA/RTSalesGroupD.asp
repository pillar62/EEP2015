
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=2
  title="業務組別資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT areaid,groupid,custyid,groupnc,leader,sdate,edate,eusr,edat,uusr,udat " _
             &"FROM RTSalesGroup WHERE areaid='*' and groupid='*' "
  sqlList="SELECT areaid,groupid,custyid,groupnc,leader,sdate,edate,eusr,edat,uusr,udat " _
             &"FROM RTSalesGroup WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"  
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(1))) = 0 then
       formValid=False
       message="請輸入組別代碼"
    elseIf len(trim(dspkey(3))) = 0 Then
       formValid=False
       message="請輸入組別名稱"
    elseIf len(trim(dspkey(5))) = 0  Then
       formValid=False
       message="請輸入組別生效日期"
    End If                  
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
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srbtnonclick()
       Dim ClickID
       ClickID=mid(window.event.srcElement.id,2,len(window.event.srcElement.id)-1)
       clickkey="KEY" & clickid
       clickTD="TD" & clickid       
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
       document.all(clearkey).value =  ""
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
Sub SrGetUserDefineKey()
s=FrGetAreaDesc(aryParmKey(0))
'---<<field i/o control>>----------------
    If Ucase(trim(dataprotect))="READONLY" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=""
       fieldpc=""
    Else
       fieldPa=""
       fieldpb=" onclick=""SrBtnOnClick"" "
       fieldpc=" onclick=""SrClear"" "    
    End If
%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="20%" class=dataListHead>轄區代碼</td><td width="30%" bgcolor=silver>
           <input class=dataListData type="text" name="key0"
                 readonly size="2" value="<%=dspKey(0)%>" maxlength="2" ></td>
           <td width="20%" class=dataListHead>組別代碼</td>
           <td width="30%" bgcolor=silver>
           <input type="text" name="key1"
                  size="2" value="<%=dspKey(1)%>" <%=fieldpa%>  <%=keyprotect%> class=dataListEntry  maxlength="2" >
    </td></tr>
    </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(7))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(7)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(7))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(8)=datevalue(now())
    else
        if len(trim(dspkey(9))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(9)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(9))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(7))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(10)=datevalue(now())    
    end if
'---<<field i/o control>>----------------
    If Ucase(trim(dataprotect))="READONLY" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=""
       fieldpc=""
    Else
       fieldPa=""
       fieldpb=" onclick=""SrBtnOnClick"" "
       fieldpc=" onclick=""SrClear"" "    
    End If
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">組別類型</font></td>
    <td width="35%" bgcolor="#C0C0C0">
<%  Dim conn,rs,sql,s,sx
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTCODE where kind='B2' ORDER BY kind,code "
       If len(trim(dspkey(2))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT * FROM RTCODE WHERE kind='B2' and CODE='" &dspkey(2) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("code")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>   
    <select name="key2" <%=fieldpa%>  <%=dataProtect%>  class=dataListEntry size="1" 
            style="text-align:left;" maxlength="8"><%=s%></select>               
  </td>
  <td width="15%" bgcolor="#008080"><font color="#FFFFFF">組別名稱</font></td>
  <td width="35%" bgcolor="#C0C0C0">
    <input name="key3"  <%=fieldpa%>  <%=dataprotect%>  class=dataListEntry  maxlength=10 size=20 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(3)%>"></td>  
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">組長</font></td>
    <td width="35%" bgcolor="#C0C0C0" colspan="3">
<%
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT RTAreaSales.CUSID, RTObj.CUSNC, RTEmployee.EMPLY FROM RTAreaSales INNER JOIN " _
          &"RTEmployee ON RTAreaSales.CUSID = RTEmployee.EMPLY INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE rtareasales.areaid = '" & dspkey(0) & "' and rtemployee.tran2 <> '10' "
        '  Response.Write "SQL=" & SQL
       If len(trim(dspkey(4))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT RTAreaSales.CUSID, RTObj.CUSNC, RTEmployee.EMPLY FROM RTAreaSales INNER JOIN " _
          &"RTEmployee ON RTAreaSales.CUSID = RTEmployee.EMPLY INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE rtareasales.areaid = '" & dspkey(0) & "' and RTAreaSales.CusID='" & dspkey(4) & "'"
       '   Response.Write "SQL=" & SQL
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(4) Then sx=" selected "
       s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>   
    <select name="key4" <%=fieldpa%> <%=dataProtect%>  class=dataListEntry size="1" 
            style="text-align:left;" maxlength="8"><%=s%></select>               
  </td></TR>
  <tr>
  <td width="15%" bgcolor="#008080"><font color="#FFFFFF">生效日期</font></td>
  <td width="35%" bgcolor="#C0C0C0">
    <input name="key5"  <%=fieldpa%>  <%=dataprotect%>   class=dataListEntry maxlength=10 size=20 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(5)%>" readOnly>
   <input type="button" id="B5"  name="B5" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpb%>>
   <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1"  <%=fieldpc%> border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
   </td>  
  <td width="15%" bgcolor="#008080"><font color="#FFFFFF">終止日期</font></td>
  <td width="35%" bgcolor="#C0C0C0">
    <input name="key6"  <%=fieldpa%>  <%=dataprotect%>  class=dataListEntry  maxlength=10 size=20 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(6)%>" readOnly>
        <input type="button" id="B6"  name="B6" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpb%>>
   <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C6"  name="C6"   style="Z-INDEX: 1"  <%=fieldpc%> border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >        
        </td>       
  </tr>  
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListdata name="key7" maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(7)%>" readOnly><%=EusrNc%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListdata name="key8" maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(8)%>"  readOnly>　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListdata  name="key9" readOnly size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(9)%>"><%=UUsrNC%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListdata name="key10" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(10)%>" readOnly>　</td>
  </tr>
</table>
<% conn.close
   set rs=nothing
   set conn=nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include file="RTGetareaDesc.inc" -->