<%  
  Dim fieldRole,fieldPa,DtlCnt  
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=3
  title="社區重大訊息維護作業"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT comq1,kind,entryno,eventid,occurdat,stopdat,headline,msg " _
             &"FROM RTcmtymsg WHERE comq1=0 "
  sqlList="SELECT comq1,kind,entryno,eventid,occurdat,stopdat,headline,msg " _
             &"FROM RTcmtymsg WHERE  "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userDefineSave="Yes"  
  userdefineactivex="Yes"
 ' if aryparmkey(1)="ADSL" then aryparmkey(1)="2"
 ' if aryparmkey(1)="HB" then aryparmkey(1)="1"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(trim(dspkey(2)))=0 then dspkey(2)=0
    if not Isdate(dspkey(4)) and len(dspkey(4)) > 0 then
       formValid=False
       message="發生日期錯誤"     
    elseif len(trim(dspkey(4))) = 0 then
       formvalid=False
       message="發生日期不得空白"       
    elseif not Isdate(dspkey(5)) and len(dspkey(5)) > 0 then
       formValid=False
       message="截止日期錯誤"           
    elseif len(trim(dspkey(6))) <= 0 then
       formValid=False
       message="事件標題不可空白"           
    end if

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
       call objEF2KDT.show(0)
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"   codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
    Dim conn,rs,s,sx,sql,t

    '讀取社區名稱
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    IF dspkey(1)="2" THEN
       sql="SELECT COMN FROM RTCustAdslCmty WHERE CUTYID=" & dspkey(0)
    ELSE
       sql="SELECT rtcmty.COMN " _
        &"FROM RTCmty INNER JOIN RTCounty ON RTCmty.CUTID = RTCounty.CUTID WHERE Comq1=" & dspkey(0)
    END IF    
  '  Response.Write "SQL=" & SQL
    rs.Open SQL,Conn,1,1,1
    if not rs.eof then
       comn=rs("comn")
    else
       comn=""
    end if
    rs.close
    set rs=nothing
    set conn=nothing
 %>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font color="#FFFFFF">社區編號</font></td>
    <td width="26%" bgcolor="#c0c0c0" height="23">
    <input name="key0" size="10" class="dataListData" value="<%=dspkey(0)%>" maxlength="10"  readonly ><%=COMN%></td>
    <td width="10%" bgcolor="#006666" class="DataListHead" height="23"><font color="#FFFFFF">類別</font></td>
    <td width="10%" bgcolor="#c0c0c0" height="23">
    <%  
      aryOption=Array("HB","ADSL")
      aryOptionV=Array("1","2")   
      s=""
 '     RESPOnse.WRITE "key1=" & DSPKEY(1)
'      Response.END
      For i = 0 To Ubound(aryOptionV)
          If dspKey(1)=aryOptionV(i) Then
             sx=" selected "
             s=s &"<option value=""" &aryOptionV(i) &"""" &sx &">" &aryOption(i) &"</option>"
          Else
             sx=""
          End If
 '         s=s &"<option value=""" &aryOption(i) &"""" &sx &">" &aryOption(i) &"</option>"
      Next      
 '     if aryoptionV(trim(dspkey(1)))="1" then 
 '        J=0
 '     else
 '        J=1
 '     end if
 '     s="<option value=""" &dspKey(1) &""">" &aryOption(j) &"</option>"
     %>                 
   <select size="1" name="key1" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%> class="dataListEntry">                                            
        <%=s%>
   </select>        
    <td width="10%" bgcolor="#006666" class="DataListHead" height="23"><font color="#FFFFFF">序號</font></td>
    <td width="10%" bgcolor="#c0c0c0" height="23">
    <input name="key2" size="10" class="dataListData" value="<%=dspkey(2)%>" maxlength="10"  readonly ></td>
    </td>  
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       

' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t

    '讀取事件項目名稱
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    sql="select code,codenc from RTcode where kind='C9' "   
    rs.Open SQL,Conn,1,1,1
    sx=""    
    Do While Not rs.Eof
       If rs("code")=dspkey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("Codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.close
    set rs=nothing
 %>
<table border="1" width="100%" cellspacing="0" cellpadding="0" >

  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">事件項目</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23">
        <select name="key3" <%=fieldpg%><%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1"  style="text-align:left;" maxlength="8" class="dataListEntry">
          <%=s%>
        </select>
    </td>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="23"><font  color="#FFFFFF">發生日期</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="23"><input name="key4"  <%=dataprotect%> size="10"  class="dataListentry" value="<%=dspkey(4)%>" readonly>
         <input type="button" id="B4"  name="B4"   width="100%" style="Z-INDEX: 1"  value="..." onclick="srbtnonclick"  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C4"  name="C4"   style="Z-INDEX: 1" onclick="srclear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
     </td>
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">截止日期</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23" colspan="3"><input name="key5"  <%=dataprotect%>  size="10" maxlength="10"  <%=fieldpa%>  class="dataListentry" value="<%=dspkey(5)%>" >
             <input type="button" id="B5"  name="B5"   width="100%" style="Z-INDEX: 1"  value="..." onclick="srbtnonclick"  >
          <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1" onclick="srclear"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" >
    </td>
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">事件標題</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23" colspan="3"><input name="key6"  <%=dataprotect%> size="70"  maxlength="100"   <%=fieldpa%>  class="dataListentry" value="<%=dspkey(6)%>" ></td>
  </tr>
   <td width="10%" colspan="4" bgcolor="#a4bcdb" height="11"> 
   <p align="center"><font color="#000000" >事件內容</font></p></td> 
  </tr> 
  <tr> 
    <td width="10%" colspan="4" bgcolor="#c0c0c0" >  <p align="center">
   <TEXTAREA cols="100%" name="key7" rows=10   <%=fieldpa%>  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(7)%>"><%=dspkey(7)%></TEXTAREA></p>
    </td>
  </tr>
</table></center>
<% 
End Sub 
' --------------------------------------------------------------------------------------------  
Sub SrSaveExtDB(Smode)

End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include file="RTGetUserRight.inc" -->
