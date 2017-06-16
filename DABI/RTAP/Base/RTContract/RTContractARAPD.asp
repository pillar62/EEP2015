
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=2
  title="合約應收應付款資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CTNO,ITEMNO,ARORAP,STRBILLINGYM,MONTHLYDAT,EXPENSEITEMNM,AMT,ACTUALDAT,ACTAMT,ENDCODE " _
             &"FROM HBCONTRACTARAP WHERE CTNO=0 "
  sqlList="SELECT CTNO,ITEMNO,ARORAP,STRBILLINGYM,MONTHLYDAT,EXPENSEITEMNM,AMT,ACTUALDAT,ACTAMT,ENDCODE " _
             &"FROM HBCONTRACTARAP WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userdefineactivex="Yes"    
  extDBField=0
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(1))) < 1 Then
       formValid=False
       message="請輸入補助款代號。"
    ELSEIf len(trim(dspKey(2))) < 1 Then
       formValid=False
       message="請輸入計費週期。"
    ELSEIf len(trim(dspKey(3))) < 1 Then
       formValid=False
       message="請輸入補助金額。"
    ELSEIf len(trim(dspKey(4))) < 1 Then
       formValid=False
       message="請輸入補助起日。"
    ELSEIf len(trim(dspKey(5))) < 1 Then
       formValid=False
       message="請輸入補助迄日。"
    ELSEIf dspKey(4) > dspKey(5) Then
       formValid=False
       message="補助起日不可大於迄日。"
    End If                            
    IF LEN(TRIM(DSPKEY(3))) = 0 THEN DSPKEY(3)=0
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
   END SUB
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
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
s=FrGetContractDesc(aryParmKey(0))%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="20%" class=dataListHead>合約歸檔號</td><td width="30%" bgcolor=silver>
           <input class=dataListDATA type="text" name="key0"
                 readonly size="10" value="<%=dspKey(0)%>" maxlength="10" ></td>
           <td width="20%" class=dataListHead>項次</td>
           <td width="30%" bgcolor=silver>
           <input class=dataListDATA type="text" name="key1"
                 readonly size="10" value="<%=dspKey(1)%>" maxlength="10" ID="Text1">
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
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">收付別</font></td>
    <td width="35%" bgcolor="#C0C0C0">
<%  Dim conn,rs,sql,s,sx
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
  '  If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
  '     sql="SELECT * FROM RTcode where Kind='F7' ORDER BY Kind,Code "
  '     If len(trim(dspkey(2))) < 1 Then
  '        sx=" selected " 
  '        s=s & "<option value=""""" & sx & "></option>"  
  '        sx=""
  '     else
  '        s=s & "<option value=""""" & sx & "></option>"  
  '     end if     
  '  Else
       sql="SELECT * FROM RTcode WHERE Kind='F7' and code='" &dspkey(2) &"' order by Kind,CODE"
  '  End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("code")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codeNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    conn.close
   set rs=nothing
   set conn=nothing
    %>           
    <select class=dataListDATA name="key2" <%=dataProtect%> size="1" 
            style="text-align:left;" maxlength="8" READONLY><%=s%></select></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">開始收付年月</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListDATA name="key3" maxlength=10 size=10 style="TEXT-ALIGN: left" READONLY value 
            ="<%=dspkey(3)%>" >　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">開始收付日</font></td>
    <td width="35%" bgcolor="#C0C0C0" colspan=3>
    <input class=dataListDATA  name="key4" size=10 maxlength=10 style="TEXT-ALIGN: left " READONLY
            value="<%=dspkey(4)%>">
    </td>
  </TR>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">應收應付項目說明</font></td>
    <td width="35%" bgcolor="#C0C0C0" colspan=3>
    <input class=dataListDATA name="key5" maxlength=50 size=50  READONLY
            style="TEXT-ALIGN: left" value="<%=dspkey(5)%>" >
    </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">應收付金額</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListDATA  name="key6" size=10 maxlength=10 style="TEXT-ALIGN: left " READONLY
            value="<%=dspkey(6)%>" ID="Text2">
    </td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">實際收付日</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key7" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(7)%>" READONLY>
    <input type="button" id="B7"  name="B7" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
    <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除"  ID="C7" name="C7"   style="Z-INDEX: 1"  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear">               
    </td>
  </tr>  
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">實際收付金額</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry  name="key8" size=10 maxlength=10 style="TEXT-ALIGN: left "
            value="<%=dspkey(8)%>" >
    </td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">結案碼</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListDATA name="key9" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(9)%>" READONLY >
    </td>
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
<!-- #include file="RTGetContractDesc.inc" -->