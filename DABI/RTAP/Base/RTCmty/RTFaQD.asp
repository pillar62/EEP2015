<%  
  Dim fieldRole,fieldPa,DtlCnt  
  fieldRole=Split(FrGetUserRight("RTCustD",Request.ServerVariables("LOGON_USER")),";")
%>
<!-- #include virtual="/WebUtilityV4/DBAUDI/zzDataList.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="社區(客戶)客訴資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CASENO,PRODUCT,FAQMAN,CUSID,TEL1,TEL2,MOBILE,RCVUSR,COMQ1,RCVDATE,BREAKDATE,SendDate," _
             &"dropdate,dropusr,finishdate,finishusr,finishfac,monthclose,memo,entryno, servicetype,comtype,DEVICETYPE " _
             &"FROM RTfaqh WHERE CASENO=0 "
  sqlList="SELECT CASENO,PRODUCT,FAQMAN,CUSID,TEL1,TEL2,MOBILE,RCVUSR,COMQ1,RCVDATE,BREAKDATE,SendDate," _
         &"dropdate,dropusr,finishdate,finishusr,finishfac,monthclose,memo,entryno, servicetype,comtype,DEVICETYPE FROM RTfaqh WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=99
  userDefineSave="Yes"  
  userdefineactivex="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    IF LEN(TRIM(DSPKEY(21)))= 0 THEN DSPKEY(21)="1"
    if not Isdate(dspkey(9)) and len(dspkey(9)) > 0 then
       formValid=False
       message="受理日期錯誤"     
    elseif len(dspkey(9)) = 0 then
       formvalid=False
       message="受理日期不得空白"       
    elseif not Isdate(dspkey(10)) and len(dspkey(10)) > 0 then
       formValid=False
       message="無法上網日期錯誤"           
    elseif len(dspkey(20)) = 0 then
       formvalid=False
       message="請選擇服務類別"               
    elseif len(dspkey(22)) = 0 then
       formvalid=False
       message="請選擇用戶設備種類"                        
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
 %>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font color="#FFFFFF">案件編號</font></td>
    <td width="26%" bgcolor="#c0c0c0" height="23">
    <input name="key0" size="10" class="dataListData" value="<%=dspkey(0)%>" maxlength="10"  readonly ></td>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="23"><font color="#FFFFFF">產品</font></td>
    <td width="25%" bgcolor="#c0c0c0" height="23">
    <select name="key1" size="1"  <%=dataprotect%>  class="dataListentry">
<% if len(dspkey(1)) = 0 then %> 
      <option value='RT'>RT設備</option>      
      <option value='COT'>COT</option>
      <option value='T1專線'>T1專線</option>      
      <option value='PL1600'>PL1600</option>
      <option value='先看先贏'>先看先贏</option>            
      <option value='個人電腦'>個人電腦</option>                  
<% else %>
      <option value='<%=dspkey(1)%>'  class="datalistdata"><%=dspkey(1)%> </option>
<% End if %>
    </select>
    </td>
  </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
    logonid=session("userid")
       if len(trim(dspkey(7))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(7)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(7))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
' -------------------------------------------------------------------------------------------- 
    Dim conn,rs,s,sx,sql,t
    If IsDate(dspKey(14)) or dspkey(17)="Y" or UCASE(trim(dataprotect))="READONLY" Then
       fieldPa=" class=""dataListData"" readonly "
       fieldpb=""
    Else
       fieldPa=""
       fieldpb=" onclick=""SrBtnOnClick"" "
    End If
    if UCASE(trim(dataprotect))="READONLY" then
       fieldpd=" disabled "
       fieldpb=""
    end if
    '讀取社區名稱, 固定制及計量制社區ip
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    
    Dim SQLCMTY,SQLCUST, SqlCotIP, SqlRouterIP
    IF len(trim(dspkey(8))) = 0 then
       sqlCMTY="select comn from RTCMTY where RTCMTY.COMQ1=" & session("comq1")
       SqlCotIP = "select SNMPIP from RTSNMP where COMKIND ='1' and COMQ1=" & session("comq1")
       SqlRouterIP = "select SNMPIP from RTSNMP where COMKIND ='3' and COMQ1=" & session("comq1")
       dspkey(8)=session("COMQ1")       
    else
       sqlCMTY="select comn from RTCMTY where RTCMTY.COMQ1=" & dspkey(8)   
       SqlCotIP = "select SNMPIP from RTSNMP where COMKIND ='1' and COMQ1=" & dspkey(8)
       SqlRouterIP = "select SNMPIP from RTSNMP where COMKIND ='3' and COMQ1=" & dspkey(8)  
    end if
'response.Write SqlCotIP & "<br>" & SqlRouterIP
    rs.Open SQLCMTY,Conn,1,1,1
    if not rs.EOF  then
       comn=rs("COMN")
    end if
    rs.close
    rs.Open SqlCotIP,Conn,1,1,1
    if not rs.EOF  then
       cotip=rs("SNMPIP")
    end if
    rs.close
    rs.Open SqlRouterIP,Conn,1,1,1
    if not rs.EOF  then
       routerip=rs("SNMPIP")
    end if
    rs.close
    '讀取客戶名稱,電話,地址
    if len(trim(dspkey(3))) = 0 then dspkey(3)=session("cusid")
    if len(trim(dspkey(19))) = 0 then dspkey(19)=session("entryno")
    SQLCUST="select a.cusid, isnull(rtcounty.cutnc, '')+ a.township1+ a.raddr1 as addr, rtobj.cusnc, rtobj.shortnc, "_
		   &"a.office + case when a.office<>'' and a.extension<>'' then '#' else '' end +a.extension as COMTEL,a.home,a.mobile "_
		   &"from RTCust a inner join  rtobj on a.cusid=rtobj.cusid "_
		   &"left outer join rtcounty on rtcounty.cutid = a.cutid1 "_
		   &"where a.cusid='" & dspkey(3) & "' and a.entryno=" & dspkey(19)
    rs.open SQLCUST,CONN,1,1,1
    If not rs.Eof Then 
       if len(trim(dspkey(2))) = 0 then
          dspkey(2)=rs("cusnc")
       end if       
       if len(trim(dspkey(4))) = 0 then
          dspkey(4)=rs("COMTEL")
       end if
       if len(trim(dspkey(5))) = 0 then
          dspkey(5)=rs("home")
       end if
       if len(trim(dspkey(6))) = 0 then
          dspkey(6)=rs("mobile")
       end if       
       ADDR=rs("addr")
    end if
    rs.close
    
    set rs=nothing
 %>
<table border="1" width="100%" cellspacing="0" cellpadding="0" height="717">

  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">客戶名稱</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23"><input name="key2"  <%=dataprotect%>  size="20" maxlength="12" <%=fieldpa%> class="dataListentry" value="<%=dspkey(2)%>" ></td>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="23"><font  color="#FFFFFF">身份證字號</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="23"><input name="key3"  <%=dataprotect%> size="20"  class="dataListData" value="<%=dspkey(3)%>" readonly></td>
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">客戶電話(1)</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23"><input name="key4"  <%=dataprotect%>  size="30" maxlength="30"  <%=fieldpa%>  class="dataListentry" value="<%=dspkey(4)%>" ></td>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="23"><font  color="#FFFFFF">客戶電話(2)</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="23"><input name="key5"  <%=dataprotect%> size="30"  maxlength="30"   <%=fieldpa%>  class="dataListentry" value="<%=dspkey(5)%>" ></td>
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">客戶行動電話</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23"><input name="key6"  <%=dataprotect%> size="30"  maxlength="30"   <%=fieldpa%>  class="dataListentry" value="<%=dspkey(6)%>" ></td>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="23"><font  color="#FFFFFF">受理人員</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="23"><input name="key7"  <%=dataprotect%> size="20" class="dataListData" value="<%=dspkey(7)%>" readonly ></td>
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">社區名稱</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23"><input name="key8" size="20" class="dataListData" value="<%=dspkey(8)%>" style="display:none"><font ><%=comn%></font></td>
    <td width="15%" bgcolor="#006666" class="DataListHead" height="23"><font  color="#FFFFFF">地址</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="23"><font><%=addr%></font></td>
  </tr>
    <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">固定制社區IP</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23"><input name="key23" size="20" class="dataListData" value="<%=dspkey(8)%>" style="display:none"><font ><%=CotIP%></font></td>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="23"><font  color="#FFFFFF">計量制社區IP</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="23"><input name="key24" size="20" class="dataListData" value="<%=dspkey(8)%>" style="display:none"><font ><%=RouterIP%></font></td>
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="25"><font  color="#FFFFFF">受理時間</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="25"><input name="key9"  <%=dataprotect%> size="25"  <%=fieldpa%>   class="dataListentry"  value="<%=dspkey(9)%>">
    <input type="button" id="B9"  name="B9" height="100%" width="100%" style="Z-INDEX: 1"  value="...."   <%=fieldpb%>></td> 
    <td width="15%" bgcolor="#006666" class="DataListHead" height="25"><font  color="#FFFFFF">發文時間</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="25"><input name="key11"  <%=dataprotect%> size="25"  <%=fieldpa%>  class="dataListentry"  value="<%=dspkey(11)%>">
    <input type="button" id="B11"  name="B11" height="100%" width="100%" style="Z-INDEX: 1"  value="...."  <%=fieldpb%>></td> 
  </tr> 
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="25"><font  color="#FFFFFF">作廢日期</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="25"><input name="key12"  <%=dataprotect%> size="20"  <%=fieldpa%>   readonly  class="dataListdata"  value="<%=dspkey(12)%>">
    </td> 
    <td width="15%" bgcolor="#006666" class="DataListHead" height="25"><font  color="#FFFFFF">作廢人員</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="25"><input name="key13" size="20"  class="dataListdata"  readonly  value="<%=dspkey(13)%>"></td> 
  </tr>   
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="25"><font  color="#FFFFFF">結案日期</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="25"><input name="key14" size="25"   class="dataListdata"  readonly  value="<%=dspkey(14)%>"></td> 
    <td width="15%" bgcolor="#006666" class="DataListHead" height="25"><font  color="#FFFFFF">排除人員</font></td>
    <td width="35%" bgcolor="#c0c0c0"  height="25"><input name="key15" size="20"   class="dataListdata"  readonly  value="<%=dspkey(15)%>"></td> 
  </tr>
  <tr>
    <td width="14%" bgcolor="#006666" class="datalisthead" height="25"><font  color="#FFFFFF">排除廠商</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="25"><input name="key16" size="20"  class="dataListdata"  readonly  value="<%=dspkey(16)%>"></td> 
    <td width="15%" bgcolor="#006666" class="DataListHead" height="25"><font  color="#FFFFFF">月結碼</font></td>
    <td width="35%" bgcolor="#c0c0c0" height="25"><input name="key17" size="20"   class="dataListdata"  readonly  value="<%=dspkey(17)%>"></td> 
  </tr>         
  
  <tr>
  <%
    s=""
    sx=" selected "
    
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='E8' " 
       If len(trim(dspkey(20))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(20) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>                  
    <td width="14%" bgcolor="#006666" class="datalisthead" height="25"><font  color="#FFFFFF">服務類別</font></td>
    <td width="36%" bgcolor="#c0c0c0"  height="25" ><select name="key20" <%=dataprotect%> <%=fieldpa%> class="dataListEntry" size="1" 
            style="text-align:left;" maxlength="2"><%=s%></select> </td>
		<td width="15%" bgcolor="#c0c0c0" class="datalisthead"><font color="#FFFFFF">用戶設備種類</td>
		<%
    s=""
    sx=" selected "
    
       sql="SELECT CODE,CODENC FROM RTCODE WHERE KIND='J1' " 
       If len(trim(dspkey(22))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CODE")=dspkey(22) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("CODENC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>		
		<td width="35%" bgcolor="#c0c0c0" height="25"><select name="key22" <%=dataprotect%> <%=fieldpa%> class="dataListEntry" size="1" 
            style="text-align:left;" maxlength="2" ID="Select2"><%=s%></select></td>
  </tr>
	<tr style="display:none">
		<td width="14%" bgcolor="#006666" style="display:none" class="datalisthead" height="25"><font style="display:none" color="#FFFFFF">方案</font></td>
		<td width="36%" bgcolor="#c0c0c0" height="25" style="display:none"><input name="key21"  <%=dataprotect%> size="20"  <%=fieldpa%>   class="dataListentry" value="<%=dspkey(21)%>" ID="Text1"></td>
	</tr>
  
  <tr> 
    <td width="100%" colspan="4" bgcolor="#a4bcdb" height="11"> 
   <p align="center"><font color="#000000" >客戶問題描述</font></p></td> 
  </tr> 
  <tr> 
    <td width="100%" colspan="4" bgcolor="#c0c0c0" height="311"> 
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
      <font color="#ff0000" >無法上網時間為&nbsp;&nbsp;&nbsp; <input name="key10"  <%=dataprotect%> size="25" <%=fieldpa%>   class="dataListentry" value="<%=dspkey(10)%>">
     <input type="button" id="B10"  name="B10" height="100%" width="100%" style="Z-INDEX: 1" value="...."  <%=fieldpb%>></font>   
     <font >                            
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
      問題：</p>
 <%
    Set rs=Server.CreateObject("ADODB.Recordset")
    sqlfaqD2="SELECT * FROM RTFAQD2 RIGHT OUTER JOIN " _
            &"RTCode ON RTFAQD2.CODEID = RTCode.CODE AND CASENO ='" & DSPKEY(0) & "'" _
            &"WHERE RTCODE.KIND = 'A9' "
    rs.open sqlfaqd2,conn,1,1
    Dtlcnt=0
    Do until rs.eof
       IF not IsNull(RS("caseno")) then
          fieldpc=" checked "
       else
          fieldpc=""
       end if
    '-----
  %>
        <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 5px; MARGIN-TOP: 5px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="ext<%=Dtlcnt%>"  <%=fieldpc%> <%=fieldpd%> value="<%=rs("code")%>"><%=rs("codenc")%></p>  
  <%
    dtlcnt=Dtlcnt+1
    rs.MoveNext
    loop
    rs.close
    set rs=nothing
  %>                               
      <p style="LINE-HEIGHT: 100%; MARGIN-BOTTOM: 3px; MARGIN-TOP: 3px" align="center">
      </font>
      <TEXTAREA cols="80%" name="key18" rows=10   <%=fieldpa%>  class="dataListentry"  <%=dataprotect%>  value="<%=dspkey(18)%>"><%=dspkey(18)%></TEXTAREA></p>
    </td>
  </tr>
  <input name="key19" size="10"  <%=fieldpa%>  class="dataListentry" value="<%=dspkey(19)%>" style="display:none">
  <tr>
    <td width="100%" colspan="4" bgcolor="#a4bcdb" height="11">
      <p align="center"><font color="#000000" >客服部處理措施</font></p></td>
  </tr>
  <%
    Set rs=Server.CreateObject("ADODB.Recordset")
    s1=""
    sqlfaqd1="SELECT RTFAQD1.CASENO AS Expr1, RTFAQD1.ENTRYNO AS Expr2, " _
            &"RTFAQD1.LOGDATE AS Expr3, RTFAQD1.LOGDESC, RTObj.CUSNC AS Expr4, RTObj1.SHORTNC AS Expr5, " _
            &"RTFAQD1.LOGDROPDATE AS Expr6, RTObj2.CUSNC AS Expr7,rtfaqd1.logusrrole " _
            &"FROM RTObj RTObj2 INNER JOIN " _
            &"RTEmployee RTEmployee1 ON  " _
            &"RTObj2.CUSID = RTEmployee1.CUSID RIGHT OUTER JOIN " _
            &"RTObj RTObj1 RIGHT OUTER JOIN " _
            &"RTFAQD1 ON RTObj1.CUSID = RTFAQD1.LOGFAC ON  " _
            &"RTEmployee1.EMPLY = RTFAQD1.LOGDROPUSR LEFT OUTER JOIN " _
            &"RTObj INNER JOIN " _
            &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
            &"RTFAQD1.LOGUSR = RTEmployee.EMPLY " _
            &"WHERE RTFAQD1.CASENO='" & dspkey(0) & "' and logusrrole='4' and logdropdate is null order by logdate desc"
    rs.open sqlfaqd1,conn,1,1
    Do until rs.eof
       s1=s1 & "處理日期：" & rs("expr3") & "　" & "處理人員：" & rs("expr4") & rs("expr5") & chr(13)&chr(10) & "處理措施：" & chr(13)&chr(10)  & rs("logdesc") & chr(13)&chr(10) & chr(13)&chr(10)
    rs.MoveNext
    loop
    rs.close
    set rs=nothing
  %>
  <tr>
    <td width="100%" colspan="4" bgcolor="#c0c0c0"  height="117">
      <p align="center"><font ><TEXTAREA cols="80%" name=S1 rows=10 class="dataListdata" readonly ><%=S1%></TEXTAREA></font></p>
    </td>
  </tr>
  <tr>
    <td width="100%" colspan="4" bgcolor="#a4bcdb" height="11">
      <p align="center"><font color="#000000" >其它部門處理措施及說明</font></p>
    </td>
  </tr>
  <%
    Set rs=Server.CreateObject("ADODB.Recordset")
    S2=""
    sqlfaqd1="SELECT RTFAQD1.CASENO AS Expr1, RTFAQD1.ENTRYNO AS Expr2, " _
            &"RTFAQD1.LOGDATE AS Expr3, RTFAQD1.LOGDESC, RTObj.CUSNC AS Expr4, RTObj1.SHORTNC AS Expr5, " _
            &"RTFAQD1.LOGDROPDATE AS Expr6, RTObj2.CUSNC AS Expr7,rtfaqd1.logusrrole " _
            &"FROM RTObj RTObj2 INNER JOIN " _
            &"RTEmployee RTEmployee1 ON  " _
            &"RTObj2.CUSID = RTEmployee1.CUSID RIGHT OUTER JOIN " _
            &"RTObj RTObj1 RIGHT OUTER JOIN " _
            &"RTFAQD1 ON RTObj1.CUSID = RTFAQD1.LOGFAC ON  " _
            &"RTEmployee1.EMPLY = RTFAQD1.LOGDROPUSR LEFT OUTER JOIN " _
            &"RTObj INNER JOIN " _
            &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID ON  " _
            &"RTFAQD1.LOGUSR = RTEmployee.EMPLY " _
            &"WHERE RTFAQD1.CASENO='" & dspkey(0) & "' and logusrrole<>'4' and logdropdate is null order by logdate desc" 
            
    rs.open sqlfaqd1,conn,1,1
    Do until rs.eof
       s2=s2 & "處理日期：" & rs("expr3") & "　" & "處理人員：" & rs("expr4") & rs("expr5") & chr(13)&chr(10) & "處理措施："  & chr(13)&chr(10)  & rs("logdesc") & chr(13)&chr(10) & chr(13)&chr(10)
    rs.MoveNext
    loop
    rs.close
    set rs=nothing
  %>  
  <tr>
    <td width="100%" colspan="4" bgcolor="#c0c0c0"  height="117">
      <p align="center"><font ><TEXTAREA cols="80%" name=S2 rows=10 class="dataListdata" readonly ><%=S2%></TEXTAREA></font></p><P>
    </td>
  </tr>
</table></center>
<% 
End Sub 
' --------------------------------------------------------------------------------------------  
Sub SrSaveExtDB(Smode)
    Dim conn,rs
' Smode A:add U:update
' extDBField = n
' use extDB(i) for Screen ,and map it to DataBase
'
    Set conn=Server.CreateObject("ADODB.Connection")
    conn.open DSN
    Set rs=Server.CreateObject("ADODB.Recordset")
    Set comm=Server.CreateObject("ADODB.Command")
    
'------ RTObj ---------------------------------------------------
    DELFAQD2="delete from rtfaqd2 where caseno='" & dspkey(0) & "'"
    conn.Execute delFAQD2  
    For i=0 to 99
        if len(trim(extdb(i))) > 0  then
           rs.Open "SELECT * FROM RTFAQD2 WHERE caseno='" &dspKey(0) &"' and codeid='" & extDB(i) & "'" ,conn,3,3
           If rs.Eof Or rs.Bof Then
              rs.AddNew
              rs("caseno")=dspKey(0)
              rs("codeid")=extDB(i)          
           End If
        rs("Eusr")=dspkey(7)
        rs("Edat")=now()
       ' rs("Uusr")=dspKey(47)
       ' rs("Udat")=dspKey(48)
           rs.Update
           rs.Close
        end if
    Next
    conn.Close
    Set rs=Nothing
    Set conn=Nothing
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include file="RTGetUserRight.inc" -->
<!-- #include file="RTGetCountyTownShip.inc" -->