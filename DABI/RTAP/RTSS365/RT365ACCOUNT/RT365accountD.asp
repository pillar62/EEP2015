
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="先看先贏帳號維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT SS365ACCOUNT, SS365PWD, USEDATE, CUSID, ENTRYNO, APPLYDAT, " _
             &"ACCOUNTLIFE, TYPE, DROPDAT, BATCHNO, DEADLINE, EUSR, EDATE,UUSR, UDATE " _
             &"FROM RT365ACCOUNT "
  sqlList="SELECT SS365ACCOUNT, SS365PWD, USEDATE, CUSID, ENTRYNO, APPLYDAT, " _
             &"ACCOUNTLIFE, TYPE, DROPDAT, BATCHNO, DEADLINE, EUSR, EDATE,UUSR, UDATE " _
             &"FROM RT365ACCOUNT  WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  userdefineactivex="Yes"    
 ' extDBField=1
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(0))) < 1 Then
       formValid=False
       message="請輸入先看先贏帳號"
    elseif len(trim(dspKey(1))) < 1 Then
       formValid=False
       message="請輸入密碼"
    end if        
  '  Response.Write "DSPKEY5=" & dspkey(5) & ";dspkey(7)=" & dspkey(7)
    if len(trim(dspkey(5))) > 0 then
       select case trim(dspkey(7))
              case "399" 
                    dspkey(10)=dateadd("m",3,dspkey(5))
              case "599"
                    dspkey(10)=dateadd("m",15,dspkey(5))
              case "1199"
                    dspkey(10)=dateadd("m",24,dspkey(5))
        end select
    end if
End Sub
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
    <OBJECT classid="CLSID:B8C54992-B7BF-11D3-AACE-0080C8BA466E"     codebase="/webap/activex/EF2KDT.CAB#version=9,0,0,3" 
	        height=60 id=objEF2KDT style="DISPLAY: none; HEIGHT: 0px; LEFT: 0px; TOP: 0px; WIDTH: 0px" 
	        width=60 VIEWASTEXT>
	<PARAM NAME="_ExtentX" VALUE="1270">
	<PARAM NAME="_ExtentY" VALUE="1270">
	</OBJECT>
<%	
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="21%" class=dataListHead>帳號</td><td width="79%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0" <%=keyprotect%> size="10" 
           value="<%=dspKey(0)%>" maxlength="10" ></td>
       </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(11))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(11)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(11))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(12)=datevalue(now())
    else
        if len(trim(dspkey(13))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(13)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(13))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(13))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(14)=datevalue(now())
    end if

'-----EXTDB DATA RETRIVE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
SQL="SELECT SHORTNC FROM RTObj where (CUSID = '" & dspkey(3) & "') "
conn.Open dsn
rs.Open sql,conn
if not rs.EOF then
   cusnc=rs("shortnc")
else
   cusnc=""
end if
rs.close
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">密碼</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key1" <%=dataprotect%>  readonly maxlength=6 size=6 style="TEXT-ALIGN: left" value
            ="<%=dspkey(1)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">使用日期</font></td>
    <td width="25%" bgcolor="#C0C0C0">
   <input readonly class=dataListEntry name="key2" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(2)%>">
   <input type="button" id="B2"  name="B2" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
   <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C2"  name="C2"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="Srclear">
     </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">使用客戶</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input readonly class=dataListEntry name="key3"  maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(3)%>"><%=cusnc%></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">單次</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input readonly class=dataListEntry name="key4"  <%=dataprotect%> maxlength=4 size=4 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">帳號開通日</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input readonly class=dataListEntry name="key5"  <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>">
          <input type="button" id="B5"  name="B5" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
             <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C5"  name="C5"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="Srclear"> </td> 
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">帳號有效日</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input readonly class=dataListEntry name="key10"  <%=dataprotect%> maxlength=10  readonly size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(10)%>">
          <input type="button" id="B10"  name="B10" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
             <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C10"  name="C10"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="Srclear"> </td>
    </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">帳號使用期限</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key6"  <%=dataprotect%> maxlength=2 size=2 style="TEXT-ALIGN: left" value
            ="<%=dspkey(6)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">帳號類型</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key7"  <%=dataprotect%> maxlength=5 size=5 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">帳號註銷日</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input readonly class=dataListEntry name="key8"  <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(8)%>">
    <input type="button" id="B8"  name="B8" height="100%" width="100%" style="Z-INDEX: 1" value="...." onclick="SrBtnOnClick">
       <IMG SRC="/WEBAP/IMAGE/IMGDELETE.GIF" alt="清除" id="C8"  name="C8"   style="Z-INDEX: 1" border=0 onmouseover="ImageIconOver" onmouseout="ImageIconOut"  onclick="Srclear"> </td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">批次代號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key9" <%=dataprotect%>  readonly maxlength=11 size=11 style="TEXT-ALIGN: left" value
            ="<%=dspkey(9)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入人員</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key11" <%=dataprotect%> maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(11)%>" readOnly><%=EusrNc%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入日期</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key12"  maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(12)%>"  readOnly>　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">修改人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry  name="key13" readOnly size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(13)%>"><%=UUsrNC%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">修改日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key14"  maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(14)%>"  readOnly>　</td>    
  </tr>
</table>

<% conn.close
   set rs=nothing
   set conn=nothing
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrSaveExtDB(Smode)
End Sub
' -------------------------------------------------------------------------------------------- 
%>
