<%@ Transaction = required %>

<!-- #include virtual="/WebUtilityV3/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->

<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=3
  title="證券公司營業員基本資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT stockid,branch,cusid,sex,birthday,contact," _
             &"mobil,email,taxid,eusr,edat,uusr,udat " _
             &"FROM RTBussMan WHERE cusid='*' "
  sqlList="SELECT stockid,branch,cusid,sex,birthday,contact," _
         &"mobil,email,taxid,eusr,edat,uusr,udat " _
         &"FROM RTBussMan WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  userDefineSave="Yes"
  extDBField=13
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(2))) < 1 Then
       formValid=False
       message="請輸入營業員代碼"
    elseIf len(trim(extdb(0))) < 1 Then
       formValid=False
       message="請輸入營業員姓名"
    elseif len(trim(dspKey(3))) < 1 Then
       formValid=False
       message="請輸入營業員性別"
    elseif not IsDate(dspkey(4)) and len(trim(dspkey(4))) > 0 then
       formValid=False
       message="出生日期不正確"    
    End If              
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
s=FrGetStockBussDesc(aryParmKey(0),aryParmKey(1))%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="30%" class=dataListHead>證券公司代碼</td><td width="30%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0"
                 readonly size="10" value="<%=dspKey(0)%>" maxlength="10" ></td>
           <td width="30%" class=dataListHead>分行名稱</td><td width="30%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key1" readonly size="12"
                 value="<%=dspKey(1)%>" maxlength="12" ></td></tr>
       <tr><td width="30%" class=dataListHead>營業員代碼</td><td width="30%" bgcolor=silver colspan="3">
           <input class=dataListEntry type="password" name="key2" <%=keyprotect%> size="10"
                 value="<%=dspKey(2)%>" maxlength="10" >(身份證字號)</td>                
       </tr>
      </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    Select case dspkey(3)
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
        if len(trim(dspkey(9))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(9)=V(0)
                extdb(10)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(9))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(10)=datevalue(now())
       extdb(11)=datevalue(now())
    else
        if len(trim(dspkey(11))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(11)=V(0)
                extdb(12)=V(0)                
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(11))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(9))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(12)=datevalue(now())    
       extdb(13)=datevalue(now())
    end if
'-----EXTDB DATA RETRIVE
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
SQL="SELECT CUSNC,SHORTNC,CUTID1,TOWNSHIP1,RADDR1,RZONE1,CUTID2," _ 
   &"TOWNSHIP2,RADDR2,RZONE2,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobj where CUSID ='" & dspkey(2) & "'"
conn.Open dsn
rs.Open sql,conn
if not rs.EOF then
   If sw="E" Or (accessMode="A" And sw="") Or (accessMode="A" And sw="S")Then 
      if sw="E" then
         extdb(12)=v(0)
         extdb(13)=datevalue(now())
      elseif sw="" and accessMode="A" then
         extdb(10)=V(0)
         extdb(11)=datevalue(now()) 
      end if
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
            ="<%=extdb(0)%>"></td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">性別</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <INPUT id=radio1 <%=status1%> name=key3 readonly type=radio value="M">男
    <INPUT id=radio1 <%=status2%> name=key3 readonly  type=radio value="F">女
    　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">出生日期</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key4" <%=dataprotect%> maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">聯絡電話</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key5" <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(5)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">行動電話</font></td>
    <td width="45%" bgcolor="#C0C0C0">
     <input class=dataListEntry name="key6" <%=dataprotect%> maxlength=15 size=15 style="TEXT-ALIGN: left" value
            ="<%=dspkey(6)%>">　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">電子郵件</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key7" <%=dataprotect%> maxlength=30 size=20 style="TEXT-ALIGN: left" value
            ="<%=dspkey(7)%>">　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">證號別</font></td>
    <td width="45%" bgcolor="#C0C0C0"  colspan="3">
 <% s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTTaxType ORDER BY TaxID "
  '     If len(trim(dspkey(8))) < 1 Then
  '        sx=" selected " 
  '        s=s & "<option value=""""" & sx & "></option>" & vbcrlf  
  '        sx=""
  '     else
   '       s=s & "<option value=""""" & sx & "></option>" & vbcrlf  
   '    end if     
    Else
       sql="SELECT * FROM RTTaxType WHERE TaxID='" &dspkey(8) &"' " & vbcrlf 
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("TaxID")=dspkey(8) Then sx=" selected "
       s=s &"<option value=""" &rs("TaxID") &"""" &sx &">" &rs("TaxNC") &"</option>" & vbcrlf 
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
    <select class=dataListEntry name="key8" <%=dataProtect%> size="1" 
       style="text-align:left;" maxlength="8"><%=s%></select> 
    　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">戶籍地址</font></td>
    <td width="45%" bgcolor="#C0C0C0">
 <% s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTCounty ORDER BY CutID "
       If len(trim(extDB(2))) < 1 Then
          sx="" 
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
       sx=""
       If len(trim(extDB(3))) < 1 Then sx=" selected "
       s=s & "<option value=""""" & sx & "></option>"
       sx=""
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
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTCounty ORDER BY CutID "
       If len(trim(extDB(6))) < 1 Then
          sx="" 
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
       sx=""
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
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入人員</font></td>
    <td width="45%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key9" maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(9)%>" readOnly><%=EusrNc%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入日期</font></td>
    <td width="25%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key10" maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(10)%>"  readOnly>　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry  name="key11" readOnly size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(11)%>"><%=UUsrNC%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key12" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(12)%>" readOnly>　</td>
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
DIM conn,rs,dsn,sql
SET conn=server.CreateObject("ADODB.Connection")
set rs=server.CreateObject("ADODB.recordset")
DSN="DSN=RTLIB"
SQL="SELECT cusid,CUSNC,SHORTNC,CUTID1,TOWNSHIP1,RADDR1,RZONE1,CUTID2," _ 
   &"TOWNSHIP2,RADDR2,RZONE2,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobj where CUSID ='" & dspkey(2) & "'"
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
   rs("eusr")     =dspkey(9)
  ' if len(trim(extdb(11))) < 1 then extdb(11)=Null
   rs("edat")     =dspkey(10)
   rs("uusr")     =dspkey(11)
  ' if len(trim(extdb(13))) < 1 then extdb(13)=Null
   rs("udat")     =dspkey(12)
   rs.update
else
   rs.addnew
   rs("cusid")    =dspkey(2)
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
   rs("eusr")     =dspkey(9)
  if len(trim(extdb(11))) < 1 then extdb(11)=Null
   rs("edat")     =dspkey(10)
   rs("uusr")     =dspkey(11)
  if len(trim(extdb(13))) < 1 then extdb(13)=Null
   rs("udat")     =dspkey(12)
   RESPONSE.WRITE "KEY=" & rs("cusid")
   rs.update
end if
rs.close
'-----save RTOBJLINK
SQL="SELECT cusid,custyid,EUSR,EDAT,UUSR,UDAT " _
   &"FROM RTobjLink where CUSID ='" & dspkey(2) & "' and custyid='09' " 
rs.Open sql,conn,3,3
if not rs.EOF then
   rs("uusr")     =dspkey(11)
   rs("udat")     =dspkey(12)   
   rs.update
else
   rs.addnew
   rs("cusid")    =dspkey(2)
   rs("custyid")  ="09"  
   rs("eusr")     =dspkey(9)
   rs("edat")     =dspkey(10)
   rs("uusr")     =dspkey(11)
   rs("udat")     =dspkey(12)
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
<!-- #include file="RTGetStockBussDesc.inc" -->