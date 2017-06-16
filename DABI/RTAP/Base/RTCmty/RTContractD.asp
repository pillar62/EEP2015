<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="社區合約資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CTNO, CTOBJECT, CTOBJNAME, CTSTRDAT, CTENDDAT, CTCONTACT, CTTELNO, "_
			 &"MEMO, DROPDAT, DROPUSR, EUSR, EDAT FROM HBCONTRACTH WHERE CTNO=0 "
      sqlList="SELECT CTNO, CTOBJECT, CTOBJNAME, CTSTRDAT, CTENDDAT, CTCONTACT, CTTELNO, "_
			 &"MEMO, DROPDAT, DROPUSR, EUSR, EDAT FROM HBCONTRACTH WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userdefineactivex="Yes"    
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  ' (自動編號) 存檔時預設值給-1
    If accessMode="A" And sw="S" Then dspKey(0)=-1
End Sub

Sub SrActiveXScript()%>
   <SCRIPT Language="VBScript">
   Sub KEY1_OnChange()
       <%=XXS%>
       document.all("KEY2D").innerHTML=aryproperty(document.all("KEY1").selectedIndex)
   End Sub   
  ' Sub KEY2_OnChange()
  '     <%=YYS%>
  '     document.all("KEY3D").innerHTML=arypropertyY(document.all("KEY1").selectedIndex,document.all("KEY2").selectedIndex)
  '     msgbox document.all("KEY3D").innerHTML
  ' End Sub   
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E" height=60 width=60 id=objEF2KDT
			codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub

' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
   %>

<input type="text" style="display:none" name=key0 value="<%=dspKey(0)%>">
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(10))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(10)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(10))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(11)=datevalue(now())
'    else
'        if len(trim(dspkey(13))) < 1 then
'           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'                V=split(rtnvalue,";")  
'                UUsrNc=V(1)
'                DSpkey(13)=V(0)
'        else
'           Call SrGetEmployeeRef(rtnvalue,2,dspkey(13))
'                V=split(rtnvalue,";")      
'                UUsrNc=V(1)
'        End if         
'        Call SrGetEmployeeRef(rtnvalue,2,dspkey(11))
'             V=split(rtnvalue,";")      
'             EUsrNc=V(1)
'        dspkey(14)=datevalue(now())
    end if      
    
    Dim conn,rs,i,sql,s,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0 height="173">
<tr><td width="15%" class=dataListHead height="23">社區流水號</td>
    <td width="35%" height="23" bgcolor="silver">
        <input class=dataListdata type="text" name="key1" readonly
         style="text-align:left;" value="<%=dspKey(1)%>" size="16"></td>
    <td width="15%" class=dataListHead height="23">合約對象名稱</td>
    <td width="35%" height="23" bgcolor="silver">
        <input class=dataListdata type="text" name="key2" readonly
         style="text-align:left;" value="<%=dspKey(2)%>" size="16"></td></tr>

<tr><td width="15%" class=dataListHead height="23">合約起始日</td>
    <td width="35%" bgcolor="#C0C0C0">
		<input class=dataListEntry name="key3" <%=dataprotect%> READONLY maxlength=10 size=10 
		 style="TEXT-ALIGN: left" value="<%=dspkey(3)%>">
        <input type="button" id="B3" name="B3" height="100%" width="100%" style="Z-INDEX: 1" 
         value="...." onclick="SrBtnOnClick">
        <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" ID="C3" name="C3" style="Z-INDEX: 1"
         border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td>
     <td width="15%" class=dataListHead height="23">合約終止日</td>
     <td width="35%" bgcolor="#C0C0C0" colspan="3">
		 <input class=dataListEntry name="key4" <%=dataprotect%> READONLY maxlength=10 size=10 
		  style="TEXT-ALIGN: left" value="<%=dspkey(4)%>">
         <input type="button" id="B4" name="B4" height="100%" width="100%" style="Z-INDEX: 1"
          value="...." onclick="SrBtnOnClick">
		 <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" ID="C4" name="C4" style="Z-INDEX: 1"
		  border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut" onclick="SrClear"></td></tr>          
               
<tr><td width="15%" class=dataListHead height="23">聯絡人</td>
    <td width="35%" height="23" bgcolor="silver">
        <input class=dataListEntry type="text" name="key5" <%=dataProtect%>
         style="text-align:left;" maxlength="10" value="<%=dspKey(5)%>" size="16"></td>
    <td width="15%" class=dataListHead height="23">聯絡電話</td>
    <td width="35%" height="23" bgcolor="silver">
        <input class=dataListEntry type="text" name="key6" <%=dataProtect%>
         style="text-align:left;" maxlength="10" value="<%=dspKey(6)%>" size="16"></td></tr>

<tr><td width="15%" class=dataListHead height="21">備註</td>
    <td width="85%" height="21" colspan="3" bgcolor="silver">
        <textarea rows="8" name="key7" cols="90" class="dataListEntry"><%=dspKey(7)%></textarea></td></tr>

<tr>
  <td width="15%" class=dataListHead height="23">輸入人員</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key8" readonly style="text-align:left;" maxlength="8"
               value="<%=dspKey(8)%>" size="22" class=dataListData><%=EusrNc%></td>
    <td width="15%" class=dataListHead height="23">輸入日期</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key9" readonly style="text-align:left;" maxlength="8"
               value="<%=dspKey(9)%>" size="16" class=dataListData></td></tr>

<tr>
  <td width="15%" class=dataListHead height="23">作廢人員</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key10" readonly style="text-align:left;" maxlength="8"
               value="<%=dspKey(10)%>" size="22" class=dataListData><%=EUsrNc%></td>
    <td width="15%" class=dataListHead height="23">作廢日期</td>
    <td width="35%" height="23" bgcolor="silver">
        <input type="text" name="key11" readonly style="text-align:left;" maxlength="8"
               value="<%=dspKey(11)%>" size="16" class=dataListData></td></tr>

</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
