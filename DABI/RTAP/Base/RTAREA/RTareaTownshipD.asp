<%
  Dim fieldRole,fieldPa,fieldPb
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=4
  title="業務轄區與鄉鎮市區關係資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT AREAID,GROUPID,CUTID,township,TDAT,EXDAT,eusr,edat,uusr,udat,distancecode " _
             &"FROM RTAreaTOWNSHIP WHERE areaid='*' and cutid='*' ;"
  sqlList="SELECT AREAID,GROUPID,CUTID,township,TDAT,EXDAT,eusr,edat,uusr,udat,distancecode " _
             &"FROM RTAreaTOWNSHIP WHERE " 
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
  userdefineactivex="Yes"  
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(2))) = 0 then
       formValid=False
       message="請輸入(鄉鎮市區)"
    elseIf len(trim(dspkey(3))) = 0  Then
       formValid=False
       message="請輸入生效日期"
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
s=FrGetsalesgroupdesc(aryParmKey(0),aryparmkey(1))
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
       <tr><td width="20%" class=dataListHead>轄區代碼</td>
           <td width="30%" bgcolor=silver>
           <input class=dataListData type="text" name="key0"
                 readonly size="2" value="<%=dspKey(0)%>" maxlength="2" ></td>
           <td width="20%" class=dataListHead>業務組別</td>                 
           <td width="30%" bgcolor=silver>
           <input class=dataListData type="text" name="key1"
                 readonly size="2" value="<%=dspKey(1)%>" maxlength="2" ></td>      
       </tr>
       <tr>           
           <td width="20%" class=dataListHead>縣市代碼</td>
           <td width="30%" colspan="3" bgcolor=silver>
           <%  Call SrGetCountyTownShip(accessMode,sw,Len(Trim(FIELDROLE(1) &dataProtect)),dspkey(2),dspkey(3),s,t)%>
           <select size="1" name="key2"<%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" 
                  onChange="SrRenew()" class="dataListEntry"><%=s%></select>
           <select name="key3" <%=fieldpa%><%=FIELDROLE(1)%><%=dataProtect%> size="1" class=dataListEntry><%=t%></select>  
           </td>
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
  <td width="15%" bgcolor="#008080"><font color="#FFFFFF">區域別</font></td>
  <td width="35%" bgcolor="#C0C0C0" colspan=3>
         <% aryOption=Array("","本縣市","外縣市")
            aryOptionV=Array("","1","2")   
            s=""
            For i = 0 To Ubound(aryOption)
                   If dspKey(10)=aryOptionV(i) Then
                      sx=" selected "
                   Else
                      sx=""
                   End If
                   s=s &"<option value=""" &aryOptionV(i) &"""" &sx &">" &aryOption(i) &"</option>"
            Next
          %>               
         <select size="1" name="key10" <%=fieldpa%><%=dataProtect%> class="dataListEntry" >                                            
           <%=s%>
         </select>
   </td>  
  </tr>  
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
<!-- #include file="RTGetareatownshipdesc.inc" -->
<!-- #include file="RTsalesgroupdesc.inc" -->
<!-- #include virtual="/Webap/rtap/base/rtcmty/RTGetUserRight.inc" -->
<!-- #include file="RTGetCountyTownShip.inc" -->