
<%@ Transaction = required %>

<!-- #include virtual="/WebUtilityV3/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=1
  title="員工基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT CUSID,emply,netid,extension,home,mobile,email," _
             &"idat,birthday,posid,titleid,posn,posdn,dept,type,tran1," _
             &"trdat1,tran2,trdat2,eusr,edat,uusr,udat,sex,healthins,laborins " _
             &"FROM RTemployee WHERE cusid='*' "
  sqlList="SELECT CUSID,emply,netid,extension,home,mobile,email," _
             &"idat,birthday,posid,titleid,posn,posdn,dept,type,tran1," _
             &"trdat1,tran2,trdat2,eusr,edat,uusr,udat,sex,healthins,laborins " _
             &"FROM RTemployee WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"  
  extDBField=14
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(0))) < 1 Then
       formValid=False
       message="請輸入員工身份證字號"
    elseif len(trim(extdb(0))) < 1 Then
       formValid=False
       message="請輸入員工姓名"
    elseif len(trim(dspkey(23))) < 1 Then
       formValid=False
       message="請輸入員工性別"
    End If
    if len(trim(dspkey(8))) > 0 and not IsDate(dspkey(8)) then
       formValid=False
       message="出生日期不正確"
    end if
    if len(trim(dspkey(7))) > 0 and not IsDate(dspkey(7)) then
       formValid=False
       message="到職日期不正確"
    end if    
    if not IsNumeric(dspkey(11)) then
       formValid=False
       message="職等不正確"
    end if
    if not IsNumeric(dspkey(12)) then
       formValid=False
       message="職級不正確"
    end if        
    IF LEN(TRIM(DSPKEY(11))) = 0 THEN DSPKEY(11)=0
    IF LEN(TRIM(DSPKEY(12))) = 0 THEN DSPKEY(12)=0
    IF LEN(TRIM(DSPKEY(24))) = 0 THEN DSPKEY(24)=0
    IF LEN(TRIM(DSPKEY(25))) = 0 THEN DSPKEY(25)=0
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="21%" class=dataListHead>員工代碼</td><td width="79%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0" <%=keyprotect%> size="10" 
           value="<%=dspKey(0)%>" maxlength="10" >(身份證字號)</td>
       </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    IF LEN(TRIM(DSPKEY(11))) = 0 THEN DSPKEY(11)=0
    IF LEN(TRIM(DSPKEY(12))) = 0 THEN DSPKEY(12)=0
    Select case dspkey(23)
     case "M"
       status1="Checked"
     case "F"
       status2="Checked"
     case else
       status1=""
       status2=""
    End Select
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(19))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(19)=V(0)
                extdb(10)=v(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(19))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(20)=datevalue(now())
       extdb(11)=datevalue(now())
    else
        if len(trim(dspkey(21))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(21)=V(0)
                extdb(12)=v(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(21))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(19))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(22)=datevalue(now())
        extdb(13)=datevalue(now())
    end if

'-----EXTDB DATA RETRIVE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
SQL="SELECT CUSNC,SHORTNC,CUTID1,TOWNSHIP1,RADDR1,RZONE1,CUTID2," _ 
   &"TOWNSHIP2,RADDR2,RZONE2,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobj where CUSID ='" & dspkey(0) & "'"
conn.Open dsn
rs.Open sql,conn
if not rs.EOF then
   If sw="E" Or (accessMode="A" And sw="") Then 
      if sw="E" then extdb(13)=datevalue(now())
      if sw="" and accessMode="A" then extdb(11)=datevalue(now())
   else
      extdb(0)=rs("cusnc")
      extdb(1)=rs("shortnc")
      extdb(2)=rs("cutid1")
      extdb(3)=rs("township1")
      extdb(4)=rs("raddr1")
      extdb(5)=rs("rzone1")
      extdb(6)=rs("cutid2")
      extdb(7)=rs("township2")
      extdb(8)=rs("raddr2")
      extdb(9)=rs("rzone2")
      extdb(10)=rs("eusr")
      if len(trim(rs("edat"))) > 0 then extdb(11)=datevalue(rs("edat"))
      extdb(12)=rs("uusr")
      if len(trim(rs("udat"))) > 0 then extdb(13)=datevalue(rs("udat")) 
   end if
else
end if
rs.close
%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">姓名</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="ext0" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(0)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">性別</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <INPUT id=radio1 <%=status1%> name=key23 readonly type=radio value="M">男
    <INPUT id=radio1 <%=status2%> name=key23 readonly  type=radio value="F">女
    </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">出生日期</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key8"  <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(8)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">到職日期</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key7"  <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">部門名稱</font></td>
    <td width="45%" bgcolor="#C0C0C0">
  <%s=""
    sx=" selected " 
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)Then 
       sql="SELECT * FROM RTdept where tdat <= GETDATE() AND ((exdat IS NULL) OR " _
          &"exdat >= GETDATE()) ORDER BY dept "
       If len(trim(dspkey(13))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  & vbcrlf
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>" & vbcrlf 
       end if            
    Else
       sql="SELECT * FROM RTdept WHERE dept='" &dspkey(13) &"' " & vbcrlf
    End if 
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("dept")=dspkey(13) Then sx=" selected "
          s=s &"<option value=""" &rs("dept") &"""" &sx &">" &trim(rs("deptn3")) & trim(rs("deptn4")) & trim(rs("deptn5"))  &"</option>" & vbcrlf
          rs.MoveNext
          sx=""
    Loop
    rs.Close%>   
    <select class=dataListEntry name="key13" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8"><%=s%></select>     
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">員工工號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key1" <%=dataprotect%> maxlength=6 size=6 style="TEXT-ALIGN: left" value
            ="<%=dspkey(1)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">職位</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key9"  <%=dataprotect%> maxlength=2 size=2 style="TEXT-ALIGN: left" value
            ="<%=dspkey(9)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">職稱</font></td>
    <td width="25%" bgcolor="#C0C0C0">
  <%s=""
    sx=" selected " 
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTcode where Kind='A3' ORDER BY Kind,Code "
       If len(trim(dspkey(10))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
    sql="SELECT * FROM RTcode WHERE Kind='A3' and code='" &dspkey(10) &"' order by Kind,CODE"
    end if
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("code")=dspkey(10) Then sx=" selected "
          s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codeNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>      
    <select class=dataListEntry name="key10" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8"><%=s%></select> 
    </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">職等</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key11"  <%=dataprotect%> maxlength=2 size=2 style="TEXT-ALIGN: left" value
            ="<%=dspkey(11)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">職級</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key12"  <%=dataprotect%> maxlength=2 size=2 style="TEXT-ALIGN: left" value
            ="<%=dspkey(12)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">聯絡電話</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key4"  <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">行動電話</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key5" <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">工作類別</font></td>
    <td width="45%" bgcolor="#C0C0C0">
  <%s=""
    sx=" selected " 
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTcode where Kind='A4' ORDER BY Kind,Code "
       If len(trim(dspkey(14))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT * FROM RTcode WHERE Kind='A4' and code='" &dspkey(14) &"' order by Kind,CODE"
    end if
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("code")=dspkey(14) Then sx=" selected "
       s=s &"<option value=""" &rs("code") &"""" &sx &">" &rs("codeNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>  
    <select class=dataListEntry name="key14" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8"><%=s%></select>      
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">網路帳號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key2"  <%=dataprotect%> maxlength=20 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(2)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">分機號碼</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key3" <%=dataprotect%> maxlength=5 size=5 style="TEXT-ALIGN: left" value
            ="<%=dspkey(3)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">電子郵件</font></td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key6" <%=dataprotect%> maxlength=30 size=25 style="TEXT-ALIGN: left" value
            ="<%=dspkey(6)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">戶籍地址</font></td>
    <td width="45%" bgcolor="#C0C0C0">
 <% s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTCounty ORDER BY CutID "
       If len(trim(extDB(2))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT * FROM RTCounty WHERE CutID='" &extdb(2) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CutID")=extDB(2) Then sx=" selected "
       s=s &"<option value=""" &rs("CutID") &"""" &sx &">" &rs("CutNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="ext2" <%=dataProtect%> size="1" 
                onChange="SrRenew()"
               style="text-align:left;" maxlength="8"><%=s%></select>    
<%  If sw="E" Then
       rs.Open "SELECT * FROM RTCtyTown WHERE CutID='" &extDB(2) &"' ORDER BY TownShip ",conn
       s=""
       sx=" selected "
       If len(trim(extDB(3))) < 1 Then sx=" selected "
       s=s & "<option value=""""" & sx & "></option>"
       Do While Not rs.Eof
          If rs("TownShip")=extDB(3) Then sx=" selected "
          s=s &"<option value=""" &rs("TownShip") &"""" &sx &">" &rs("TownShip") &"</option>"
          rs.MoveNext
          sx=""
       Loop
       rs.Close
    Else
       s="<option value=""" &extDB(3) &""" selected>" &extDB(3) &"</option>"
    End If %>
    <select class=dataListEntry name="ext3" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8"><%=s%></select>   
    <input class=dataListEntry name="ext4" <%=dataprotect%> maxlength=40 size=25 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(4)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">郵遞區號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="ext5" <%=dataprotect%> maxlength=5 size=5 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(5)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">通訊地址</font></td>
    <td width="45%" bgcolor="#C0C0C0">
 <% s=""
    sx=" selected " 
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false)Then 
       sql="SELECT * FROM RTCounty ORDER BY CutID "
       If len(trim(extDB(6))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if            
    Else
       sql="SELECT * FROM RTCounty WHERE CutID='" &extdb(6) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CutID")=extDB(6) Then sx=" selected "
       s=s &"<option value=""" &rs("CutID") &"""" &sx &">" &rs("CutNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="ext6" <%=dataProtect%> size="1" 
                onChange="SrRenew()"
               style="text-align:left;" maxlength="8"><%=s%></select>
<%  If sw="E" Then
       rs.Open "SELECT * FROM RTCtyTown WHERE CutID='" &extDB(6) &"' ORDER BY TownShip ",conn
       s=""
       sx=" selected "
       If len(trim(extDB(7))) < 1 Then sx=" selected "
       s=s & "<option value=""""" & sx & "></option>"       
       Do While Not rs.Eof
          If rs("TownShip")=extDB(7) Then sx=" selected "
          s=s &"<option value=""" &rs("TownShip") &"""" &sx &">" &rs("TownShip") &"</option>"
          rs.MoveNext
          sx=""
       Loop
       rs.Close
    Else
       s="<option value=""" &extDB(7) &""" selected>" &extDB(7) &"</option>"
    End If %>
    <select class=dataListEntry name="ext7" <%=dataProtect%> size="1" 
               style="text-align:left;" maxlength="8"><%=s%></select>                   
     <input class=dataListEntry name="ext8" <%=dataprotect%> maxlength=40 size=25 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(8)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">郵遞區號</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="ext9" <%=dataprotect%> maxlength=5 size=5 style="TEXT-ALIGN: left" value
            ="<%=EXTDB(9)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">健保費</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key24" maxlength=6 size=6 value="<%=dspkey(24)%>" ID="Text1"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">勞保費</font></td>
    <td width="25%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key25" maxlength=6 size=6 value="<%=dspkey(25)%>" ID="Text2"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動代碼1</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key15" readOnly maxlength=2 size=2 style="TEXT-ALIGN: left" value
            ="<%=dspkey(15)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動生效日</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key16" readOnly maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(16)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動代碼2</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key17" maxlength=2 size=2 style="TEXT-ALIGN: left" value
            ="<%=dspkey(17)%>">&nbsp;離職：10</b></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動生效日</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key18" maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(18)%>"></td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入人員</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key19" <%=dataprotect%> maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(19)%>" readOnly><%=EusrNc%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入日期</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key20"  maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(20)%>"  readOnly>　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">修改人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry  name="key21" readOnly size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(21)%>"><%=UUsrNC%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">修改日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key22" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(22)%>" readOnly>　</td>
    <input class=dataListEntry name="ext10" maxlength=6 size=6    
            style="TEXT-ALIGN: left" value="<%=extdb(10)%>" style="display:none">
    <input class=dataListEntry name="ext11" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=extdb(11)%>" style="display:none">  
    <input class=dataListEntry name="ext12" maxlength=6 size=6    
            style="TEXT-ALIGN: left" value="<%=extdb(12)%>" style="display:none">  
    <input class=dataListEntry name="ext13" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=extdb(13)%>" style="display:none">  
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
'--------------SAVE RTOBJ FILE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
SQL="SELECT cusid,CUSNC,SHORTNC,CUTID1,TOWNSHIP1,RADDR1,RZONE1,CUTID2," _ 
   &"TOWNSHIP2,RADDR2,RZONE2,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobj where CUSID ='" & dspkey(0) & "'"
conn.Open dsn
rs.Open sql,conn,3,3
if not rs.EOF then  
   '--由於對象基本資料檔係共用資料,為避免資料因不得使用者輸入導致資料lose
   '--現象;故判斷當使用者有輸入資料時再取代原本資料
   '===========
   '--?????是否會造成欲將資料清空,卻又無法取代的現象發生??????
   '+++再考慮
   rs("cusnc")    =extdb(0)
   rs("shortnc")  =extdb(0)
   rs("cutid1")   =extdb(2)
   rs("township1")=extdb(3)
   rs("raddr1")   =extdb(4)
   rs("rzone1")   =extdb(5)
   rs("cutid2")   =extdb(6)
   rs("township2")=extdb(7)
   rs("raddr2")   =extdb(8)
   rs("rzone2")   =extdb(9)
   rs("eusr")     =dspkey(19)
   rs("edat")     =dspkey(20)
   rs("uusr")     =dspkey(21)
   rs("udat")     =dspkey(22)
   rs.update
else
   rs.addnew
   rs("cusid")    =dspkey(0)
   rs("cusnc")    =extdb(0)
   rs("shortnc")  =extdb(0)
   rs("cutid1")   =extdb(2)
   rs("township1")=extdb(3)
   rs("raddr1")   =extdb(4)
   rs("rzone1")   =extdb(5)
   rs("cutid2")   =extdb(6)
   rs("township2")=extdb(7)
   rs("raddr2")   =extdb(8)
   rs("rzone2")   =extdb(9)
   rs("eusr")     =dspkey(19)
   rs("edat")     =dspkey(20)
   rs("uusr")     =dspkey(21)
   rs("udat")     =dspkey(22)
   rs.update
end if
rs.close
'-----save RTOBJLINK
SQL="SELECT cusid,custyid,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobjLink where CUSID ='" & dspkey(0) & "' and custyid='08' " 
rs.Open sql,conn,3,3
if not rs.EOF then
   rs("uusr")     =dspkey(21)
   rs("udat")    =dspkey(22)        
   rs.update
else
   rs.addnew
   rs("cusid")    =dspkey(0)
   rs("custyid")  ="08"  
   rs("eusr")     =dspkey(19)
   rs("edat")     =dspkey(20)
   rs("uusr")     =dspkey(21)
   rs("udat")     =dspkey(22)
   rs.update
end if     
rs.close
conn.close
set rs=nothing
set conn=nothing
objectcontext.setcomplete
End Sub
' -------------------------------------------------------------------------------------------- 
%>
