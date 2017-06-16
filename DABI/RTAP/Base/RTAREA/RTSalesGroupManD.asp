
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=4
  title="業務組別與業務員關係資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT areaid,groupid,emply,version,sdate,edate,eusr,edat,uusr,udat " _
             &"FROM RTSalesGroupRef WHERE areaid='*' "
  sqlList="SELECT areaid,groupid,emply,version,sdate,edate,eusr,edat,uusr,udat " _
             &"FROM RTSalesGroupRef WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"  
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(trim(dspkey(3)))=0 then dspkey(3)=0
    If len(trim(dspKey(2))) = 0 then
       formValid=False
       message="請選擇業務員"
    elseIf len(trim(dspkey(4))) = 0 Then
       formValid=False
       message="請輸入業務員生效日期"
    End If                  
    '---檢查版次(新增時有效)
    if accessmode="A" then
    Dim conn,rs,sql,s,sx
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    SQL="select max(version) as version from rtsalesgroupref where areaid='" & dspkey(0) & "' and groupid='" & dspkey(1) & "' " _
       &"and emply='" & dspkey(2) & "'" 
    rs.open sql,conn
    if len(trim(rs("version"))) > 0 or rs("version")=0 then
       dspkey(3)=rs("version") + 1
       PreV=rs("version")
    else
       dspkey(3)=1
    end if
    rs.close
    '---作廢前版資料(更新截止日期為本版次生效日的前一天)
    if len(trim(PreV)) > 0 then
       SQL="select * from rtsalesgroupref where areaid='" & dspkey(0) & "' and groupid='" & dspkey(1) & "' " _
          &"and emply='" & dspkey(2) & "' and version=" & PreV 
       rs.open sql,conn,3,3
       if not rs.eof then
          rs("Edate")=dateAdd("d",-1,dspkey(4)) 
          rs.update
       end if
       rs.close
     end if
     conn.close
     set rs=nothing
     set conn=nothing
     end if
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
s1="": s2=""
s1=FrGetAreaDesc(aryParmKey(0))
s2=FrGetSalesGroupDesc(aryParmKey(0),aryparmkey(1))   
S=S1& " " & s2  
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
                  size="2" value="<%=dspKey(1)%>" readonly class=dataListdata  maxlength="2" ></td>
       </tr>
       <tr><td width="20%" class=dataListHead>業務姓名</td><td width="30%" bgcolor=silver>
<%  Dim conn,rs,sql,s,sx
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT RTAreaSales.CUSID, RTObj.CUSNC, RTEmployee.EMPLY FROM RTAreaSales INNER JOIN " _
          &"RTEmployee ON RTAreaSales.CUSID = RTEmployee.EMPLY INNER JOIN " _
          &"RTObj ON RTEmployee.CUSID = RTObj.CUSID " _
          &"WHERE rtareasales.areaid = '" & dspkey(0) & "' and rtemployee.tran2 <> '10' "
        '  Response.Write "SQL=" & SQL
       If len(trim(dspkey(2))) < 1 Then
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
          &"WHERE rtareasales.areaid = '" & dspkey(0) & "' and RTAreaSales.CusID='" & dspkey(2) & "'"
       '   Response.Write "SQL=" & SQL
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CUSID")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("EMPLY") &"""" &sx &">" &rs("CUSNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    conn.close
    set rs=nothing
    set conn=nothing
   %>   
    <select name="key2" <%=fieldpa%> <%=dataProtect%>  class=dataListEntry size="1" 
            style="text-align:left;" maxlength="8"><%=s%></select>              </td>
    <td width="20%" class=dataListHead>版次</td>
    <td width="30%" bgcolor=silver>
    <input type="text" name="key3"
                  size="2" value="<%=dspKey(3)%>" readonly class=dataListdata  maxlength="2" ></td>
       </tr>       
    </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(6))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(6)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(6))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(7)=datevalue(now())
    else
        if len(trim(dspkey(8))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(8)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(8))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(6))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(9)=datevalue(now())    
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
  <td width="15%" bgcolor="#008080"><font color="#FFFFFF">生效日期</font></td>
  <td width="35%" bgcolor="#C0C0C0">
    <input name="key4"  <%=fieldpa%>  <%=dataprotect%>   class=dataListEntry maxlength=10 size=20 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(4)%>" readOnly>
   <input type="button" id="B4"  name="B4" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpb%>>
   <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C4"  name="C4"   style="Z-INDEX: 1"  <%=fieldpc%> border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
   </td>  
  <td width="15%" bgcolor="#008080"><font color="#FFFFFF">終止日期</font></td>
  <td width="35%" bgcolor="#C0C0C0">
    <input name="key5"  <%=fieldpa%>  <%=dataprotect%>  class=dataListEntry  maxlength=10 size=20 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(5)%>" readOnly>
        <input type="button" id="B5"  name="B5" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=fieldpb%>>
   <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1"  <%=fieldpc%> border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >        
        </td>       
  </tr>  
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListdata name="key6" maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(6)%>" readOnly><%=EusrNc%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListdata name="key7" maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>"  readOnly>　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListdata  name="key8" readOnly size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(8)%>"><%=UUsrNC%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListdata name="key9" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(9)%>" readOnly>　</td>
  </tr>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include file="RTGetareaDesc.inc" -->
<!-- #include file="RTSalesGroupDesc.inc" -->