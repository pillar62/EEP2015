
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=3
  title="管理類合約產品資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CTNO,PRODNO,ITEMNO,EXPENSE,QTY " _
             &"FROM HBContractmaintain WHERE ctno=0 "
  sqlList="SELECT CTNO,PRODNO,ITEMNO,EXPENSE,QTY " _
             &"FROM HBContractmaintain WHERE  "
  userDefineKey="Yes"
  userDefineData="Yes"
  userdefineactivex="Yes"      
  extDBField=0
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(1))) < 1 Then
       formValid=False
       message="請輸入產品代碼資料"
    ELSEIF len(trim(dspKey(3))) < 1 OR dspKey(3) =0 Then
       formValid=False
       message="請輸入正確產品銷售價格"
    ELSEIF dspKey(4) < 1  Then
       formValid=False
       message="請輸入產品數量"              
    End If              
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub Srcounty2onclick()
       prog="RTGetproddetail.asp"
       prog=prog & "?KEY=" & document.all("KEY1").VALUE
       FUsr=Window.showModalDialog(prog,"d2","dialogWidth:590px;dialogHeight:480px;")  
       if fusr <> "" then
       FUsrID=Split(Fusr,";")   
       if Fusrid(2) ="Y" then
          document.all("key2").value =  trim(Fusrid(0))
       End if       
       end if
   End Sub                
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
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
s=FrGetContractDesc(aryParmKey(0))%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="15%" class=dataListHead>合約歸檔號</td>
       <td width="15%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0"
                 readonly size="10" value="<%=dspKey(0)%>" <%=keyProtect%> maxlength="10" ></td>
           <td width="15%" class=dataListHead>產品編號</td>
           <td width="70%" bgcolor=silver>
<%  Dim conn,rs,sql,s,sx
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT prodno,prodnc FROM rtprodh  ORDER BY prodno "
       If len(trim(dspkey(1))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
       SXX2=" onclick=""Srcounty2onclick()""  "                    
    Else
       sql="SELECT prodno,prodnc FROM rtprodh WHERE prodno='" &dspkey(1) &"' "
       SXX2=""
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("prodno")=dspkey(1) Then sx=" selected "
       s=s &"<option value=""" &rs("proDno") &"""" &sx &">" &rs("prodnc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    conn.close
   set rs=nothing
   set conn=nothing
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
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">價格</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key3" maxlength=10 size=10 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(3)%>" ></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">數量</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key4" maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>">　</td>
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