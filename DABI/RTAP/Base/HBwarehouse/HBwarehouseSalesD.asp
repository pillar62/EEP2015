
<%@ Transaction = required %>

<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="倉庫與員工關係資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT WAREHOUSE,DEPT,emply,onoff FROM HBWarehouseSales WHERE warehouse='*' "
  sqlList="SELECT  WAREHOUSE,DEPT,emply,onoff fROM hbwarehousesales WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  extDBField=0
  userdefineactivex="Yes" 
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if dspkey(3)="" then dspkey(3)="Y"
    If len(trim(dspKey(1))) < 1 Then
       formValid=False
       message="請輸入部門"
    ELSEIF len(trim(dspKey(2))) < 1 Then
       formValid=False
       message="請輸入業務員"       
    End If        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srcounty2onclick()
       prog="RTGetEMPLOYEED.asp"
       prog=prog & "?KEY=" & document.all("KEY1").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key2").value =  trim(Fusrid(0))
       End if       
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
Sub SrGetUserDefineKey()
DSPKEY(0)=ARYPARMKEY(0) %>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="21%" class=dataListHead>倉庫代碼</td><td width="79%" bgcolor=silver>
           <input class=dataListDATA type="text" name="key0" <%=keyprotect%> size="10" 
           value="<%=dspKey(0)%>" maxlength="2" ></td>
       </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    Set conn=Server.CreateObject("ADODB.Connection")  
    Set rs=Server.CreateObject("ADODB.Recordset")
    DSN="DSN=RTLIB"
    conn.Open DSN
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
      <td width="15%" class="dataListHead" height="25">可領用員工</td>                                 
        <td width="35%"  height="25" bgcolor="silver">
  <%s=""
    sx=" selected " 
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)Then 
       sql="SELECT * FROM RTdept where tdat <= GETDATE() AND ((exdat IS NULL) OR " _
          &"exdat >= GETDATE()) ORDER BY dept "
       If len(trim(dspkey(1))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  & vbcrlf
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>" & vbcrlf 
       end if            
       SXX2=" onclick=""Srcounty2onclick()""  "       
    Else
       sql="SELECT * FROM RTdept WHERE dept='" &dspkey(1) &"' " & vbcrlf
       SXX2=""
    End if 
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("dept")=dspkey(1) Then sx=" selected "
          s=s &"<option value=""" &rs("dept") &"""" &sx &">" &trim(rs("deptn3")) & trim(rs("deptn4")) & trim(rs("deptn5"))  &"</option>" & vbcrlf
          rs.MoveNext
          sx=""
    Loop
    rs.Close%>      
    <select class=dataListEntry name="key1" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8" ID="Select1"><%=s%></select>         
   <input class=dataListDATA name="key2"  maxlength=6 size=6 style="TEXT-ALIGN: left" value
            ="<%=dspkey(2)%>"  readOnly ID="Text8">
    <input type="button" id="B2"  name="B2"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX2%>  >              
    </td>         
        <td width="15%" class="dataListHead" height="25">可否領用</td>                                 
        <td width="20%"  height="25" bgcolor="silver">
      <%  dim rdo1, rdo2
             If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)Then 
                 rdo1=""
                 rdo2=""
              Else
                 rdo1=" disabled "
                 rdo2=" disabled "
              End If
             ' If Trim(dspKey(84))="" Then dspKey()="Y"
              If trim(dspKey(3))="Y" Then 
                 rdo1=" checked "    
              elseIf trim(dspKey(3))="N" Then 
                 rdo2=" checked " 
              elseif trim(dspkey(3))="" then
                 dspkey(3)=""                 
              end if
             %>
        <input type="radio" value="Y" <%=rdo1%> name="key3" <%=dataProtect%> ID="Radio1"><font size=2>可領用
        <input type="radio" value="N" <%=rdo2%>  name="key3" <%=dataProtect%> ID="Radio2"><font size=2>不可領用</TD>      
        </td>
      </tr>
 </table>
<% conn.close
   set rs=nothing
   set conn=nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
end sub
%>
