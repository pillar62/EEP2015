
<%@ Transaction = required %>

<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="倉庫基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT warehouse,warename,cutid,township,address,maintainusr,rzone " _
             &"FROM hbwarehouse WHERE warehouse='*' "
  sqlList="SELECT  warehouse,warename,cutid,township,address,maintainusr,rzone  " _
             &"FROM hbwarehouse WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  extDBField=0
  userdefineactivex="Yes" 
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(0))) < 1 Then
       formValid=False
       message="請輸入倉庫代碼"
    elseif len(trim(dspKey(1))) < 1  Then
       formValid=False
       message="請輸入倉庫名稱"
    elseif len(trim(dspKey(2))) < 1 or len(trim(dspKey(3))) < 1 or len(trim(dspKey(4))) < 1  Then
       formValid=False
       message="請輸入倉庫地址"
    elseif len(trim(dspKey(5))) < 1  Then
       formValid=False
       message="請輸入倉庫維護人員"
    End If        
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">

   Sub Srcounty2onclick()
       prog="RTGetcountyD.asp"
       prog=prog & "?KEY=" & document.all("KEY2").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(3) ="Y" then
          document.all("key3").value =  trim(Fusrid(0))
          document.all("key6").value =  trim(Fusrid(1))
       End if       
       end if
   End Sub   
   Sub Srcounty5onclick()
       prog="RTGetEMPLOYEED.asp"
       prog=prog & "?KEY=" & ""
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key5").value =  trim(Fusrid(0))
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
Sub SrGetUserDefineKey()%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="21%" class=dataListHead>倉庫代碼</td><td width="79%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0" <%=keyprotect%> size="10" 
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
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">倉庫名稱</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key1" <%=dataprotect%> maxlength=50 size=40 style="TEXT-ALIGN: left" value
            ="<%=dspkey(1)%>"></td>
    <td width="10%" bgcolor="#008080"><font color="#FFFFFF">維護人員</font></td>
    <td width="30%" bgcolor="#C0C0C0">
<%    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)Then 
         SXX5=" onclick=""Srcounty5onclick()""  "    
      else
         sxx5=""
      end if
%>    
     <input class=dataListEntry name="key5" <%=dataprotect%> readonly maxlength=6 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>">
    <input type="button" id="B5"  name="B5"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX5%>  >   
        </td>
  </tr>
  <tr>
      <tr>                                 
        <td width="15%" class="dataListHead" height="25">倉庫地址</td>                                 
        <td width="60%"  height="25" bgcolor="silver">
  <%s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) Then 
       sql="SELECT Cutid,Cutnc FROM RTCounty " 
       If len(trim(dspkey(2))) < 1 Then
          sx=" selected " 
       else
          sx=""
       end if     
       s=s &"<option value=""" &"""" &sx &">(縣市別)</option>"       
       SXX2=" onclick=""Srcounty2onclick()"" "
       fieldpe=" onclick=""Srclear()"" "
    Else
       sql="SELECT Cutid,Cutnc FROM RTCounty where cutid='" & dspkey(2) & "' " 
       sXX2=""
       fieldpe=""
    End If
    sx=""    
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("cutid")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("Cutid") &"""" &sx &">" &rs("Cutnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
   %>        
        <select name="key2" <%=dataProtect%> size="1"  style="text-align:left;" maxlength="8" class="dataListEntry">
        <%=s%></select>
        <input type="text" name="key3" size="8" value="<%=dspkey(3)%>" maxlength="10" readonly <%=dataProtect%> class="dataListEntry"><font size=2>(鄉鎮市區)                 
         <input type="button" id="B3"  name="B3"   width="100%" style="Z-INDEX: 1"  value="..." <%=SXX2%>  >        
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C3"  name="C3"   style="Z-INDEX: 1" <%=fieldpe%>  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >    
        <input type="text" name="key4" size="30" value="<%=dspkey(4)%>" maxlength="60" <%=dataProtect%> class="dataListEntry"></td>                                
        <td width="10%" class="dataListHead" height="25">郵遞區號</td>                                 
        <td width="16%" height="25" bgcolor="silver"><input type="text" name="key6" size="10" value="<%=dspkey(6)%>" maxlength="5"<%=dataProtect%> class="dataListdata" readonly></td>                                 
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
