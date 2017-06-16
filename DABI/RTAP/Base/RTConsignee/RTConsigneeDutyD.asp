
<!-- #include virtual="/WebUtilityV2/DBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include virtual="/webap/include/lockright.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=2
  title="經銷商責任區分資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT cusid,cutid,dutytype,eusr,edat,uusr,udat " _
             &"FROM RTConsigneeCty WHERE cusid='*' "
  sqlList="SELECT cusid,cutid,dutytype,eusr,edat,uusr,udat " _
             &"FROM RTConsigneeCty WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
  extDBField=0
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If len(trim(dspKey(1))) < 1 Then
       formValid=False
       message="請輸入縣市資料"
    elseIf len(trim(dspkey(2))) < 1 Then
       formValid=False
       message="請輸入責任區分資料"
    End If              
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
s=FrGetConsigneeDesc(aryParmKey(0))%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="20%" class=dataListHead>廠商代碼</td><td width="30%" bgcolor=silver>
           <input class=dataListEntry type="text" name="key0"
                 readonly size="10" value="<%=dspKey(0)%>" maxlength="10" ></td>
           <td width="20%" class=dataListHead>縣市代碼</td>
           <td width="30%" bgcolor=silver>
<%  Dim conn,rs,sql,s,sx
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTCounty ORDER BY CutID "
       If len(trim(dspkey(1))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT * FROM RTCounty WHERE CutID='" &dspkey(1) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("CutID")=dspkey(1) Then sx=" selected "
       s=s &"<option value=""" &rs("CutID") &"""" &sx &">" &rs("CutNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>           
    <select class=dataListEntry name="key1" <%=dataProtect%> size="1" 
            style="text-align:left;" maxlength="8"><%=s%></select>
    </td></tr>
    </table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(3))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(3)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(3))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(4)=datevalue(now())
    else
        if len(trim(dspkey(5))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(5)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(5))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(3))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(6)=datevalue(now())    
    end if

%>
<table border="1" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">責任區分</font></td>
    <td width="35%" bgcolor="#C0C0C0" colspan="3">
<%  Dim conn,rs,sql,s,sx
    set conn=server.CreateObject("ADODB.Connection")
    set rs=server.CreateObject("ADODB.recordset")
    conn.Open dsn
    s=""
    sx=" selected "
    If sw="E" Or (accessMode="A" And sw="") or (sw="S" and formvalid=false) Then 
       sql="SELECT * FROM RTCODE where kind='A2' ORDER BY kind,code "
       If len(trim(dspkey(2))) < 1 Then
          sx=" selected " 
          s=s & "<option value=""""" & sx & "></option>"  
          sx=""
       else
          s=s & "<option value=""""" & sx & "></option>"  
       end if     
    Else
       sql="SELECT * FROM RTCODE WHERE kind='A2' and CODE='" &dspkey(2) &"' "
    End If
    rs.Open sql,conn
    Do While Not rs.Eof
       If rs("code")=dspkey(2) Then sx=" selected "
       s=s &"<option value=""" &rs("CODE") &"""" &sx &">" &rs("codenc") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>   
    <select class=dataListEntry name="key2" <%=dataProtect%> size="1" 
            style="text-align:left;" maxlength="8"><%=s%></select>               
  </td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key3" maxlength=6 size=6 style="TEXT-ALIGN:  left" 
     value="<%=dspkey(3)%>" readOnly><%=EusrNc%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">輸入日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key4" maxlength=10 size=10 style="TEXT-ALIGN: left" value
            ="<%=dspkey(4)%>"  readOnly>　</td>
  </tr>
  <tr>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動人員</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry  name="key5" readOnly size=6 maxlength=6 style="TEXT-ALIGN: left "
            value="<%=dspkey(5)%>"><%=UUsrNC%>　</td>
    <td width="15%" bgcolor="#008080"><font color="#FFFFFF">異動日期</font></td>
    <td width="35%" bgcolor="#C0C0C0">
    <input class=dataListEntry name="key6" maxlength=10 size=10    
            style="TEXT-ALIGN: left" value="<%=dspkey(6)%>" readOnly>　</td>
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
<!-- #include file="RTGetConsigneeDesc.inc" -->