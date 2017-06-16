
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=3
  title="送修單明細資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT REPAIRno,prodno,itemno,REPAIRqty,price,amt,cost,CAMT,warehouse,REPAIRdesc " _
             &"FROM RTStockREPAIRD1 WHERE REPAIRno='*' "
  sqlList="SELECT REPAIRno,prodno,itemno,REPAIRqty,price,amt,cost,CAMT,warehouse,REPAIRdesc " _
             &"FROM RTStockREPAIRD1 WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userdefineactivex="Yes"    
  extDBField=0
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    if len(trim(dspkey(3)))=0 then dspkey(3)=0
    if len(trim(dspkey(4)))=0 then dspkey(4)=0
    if len(trim(dspkey(5)))=0 then dspkey(5)=0
    if len(trim(dspkey(6)))=0 then dspkey(6)=0      
    if len(trim(dspkey(7)))=0 then dspkey(7)=0      
    If len(trim(dspKey(1))) < 1 Then
       formValid=False
       message="請輸入產品代碼資料"
    elseIf len(trim(dspkey(3))) < 1 Then
       formValid=False
       message="送修數量不可為0"
    elseIf len(trim(dspkey(8))) < 1 Then
       formValid=False
       message="請輸入產品所屬庫別"
    elseIf len(trim(dspkey(9))) < 1 Then
       formValid=False
       message="請輸入送修原因"              
    End If              
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">

   Sub Srk3change()
       if len(trim(document.all("key3").value))=0 then document.all("key3").value = 0
       if len(trim(document.all("key6").value))=0 then document.all("key6").value = 0
       document.all("key5").value=document.all("key3").value * document.all("key4").value
       document.all("key7").value=document.all("key3").value * document.all("key6").value
   End Sub          
   Sub Srk6change()
       if len(trim(document.all("key3").value))=0 then document.all("key3").value = 0
       if len(trim(document.all("key6").value))=0 then document.all("key6").value = 0
       document.all("key7").value=document.all("key3").value * document.all("key6").value
   End Sub             
   Sub Srcounty2onclick()
       prog="RTGetproddetail.asp"
       prog=prog & "?KEY=" & document.all("KEY1").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:700px;dialogHeight:480px;")  
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
          '當處理人員及處理廠商皆為空白時，才可清除此欄位資料
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
Sub SrGetUserDefineKey()
%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="10%" class=dataListHead>送修單號</td><td width="10%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0"
                 readonly size="10" value="<%=dspKey(0)%>" maxlength="10" ></td>
           <td width="10%" class=dataListHead>產品</td>
           <td width="60%" bgcolor=silver>
<%  set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
    If (sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)) AND TRIM(KEYPROTECT)<>"readonly" Then 
       sql="SELECT * FROM RTprodh ORDER BY prodno "
       If len(trim(dspkey(1))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       SXX2=" onclick=""Srcounty2onclick()""  "             
    Else
       sql="SELECT * FROM RTprodh WHERE prodno='" &dspkey(1) &"' order by prodno"
       SXX2=""
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("prodno")=dspkey(1) Then sx=" selected "
       s=s &"<option value=""" &rs("prodno") &"""" &sx &">" &rs("prodNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    
    sql="SELECT itemno,spec FROM RTprodd1 where prodno='" & dspkey(1) & "' and itemno='" & dspkey(2) & "' ORDER BY prodno "
    rs.Open sql,conn
    if rs.EOF then 
       objname = ""
    else
       objname = rs("spec")
    end if
    rs.close
    %>           
    <select class=dataListEntry name="key1" <%=keyProtect%> size="1" 
            style="text-align:left;" maxlength="8" ID="Select1"><%=s%></select>

    <input class=dataListEntry type="text" name="key2"
                 readonly size="5" value="<%=dspKey(2)%>" <%=keyProtect%> maxlength="5" ID="Text1">          
    <input type="button" id="B2"  name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...." <%=SXX2%> >
    <%=objname%>                  
    </td></tr>
    </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">送修數量</font></td>
    <td width="35%" bgcolor="#C0C0C0" colspan="3">
    <input class=dataListEntry name="key3" maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(3)%>" ONCHANGE="Srk3change"> </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">單價</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key4" maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(4)%>"  ONCHANGE="Srk3change"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">金額</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListDATA name="key5" maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>"  readOnly>　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">單位成本</font></td>
    <td width="35%" bgcolor="#C0C0C0" >
    <input class=dataListEntry  name="key6"  size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(6)%>" ONCHANGE="Srk6change"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">總成本</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListDATA name="key7" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(7)%>" readOnly>　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">庫別</font></td>
    <td width="35%" COLSPAN=3 bgcolor="#C0C0C0">
  <%  If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM HBWAREHOUSE ORDER BY WAREHOUSE "
       If len(trim(dspkey(8))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT * FROM HBWAREHOUSE WHERE WAREHOUSE='" &dspkey(8) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("WAREHOUSE")=dspkey(8) Then sx=" selected "
       s=s &"<option value=""" &rs("WAREHOUSE") &"""" &sx &">" &rs("WARENAME") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>           
    <select class=dataListEntry name="key8" <%=dataProtect%> size="1" 
            style="text-align:left;" maxlength="8" ><%=s%></select>
    
</td>
  </TR>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">送修原因</font></td>
    <td width="35%" COLSPAN=3 bgcolor="#C0C0C0">
    <input class=dataListEntry name="key9" maxlength=70 size=70    
            style="TEXT-ALIGN: left" value="<%=dspkey(9)%>"  ID="Text2">　</td>
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