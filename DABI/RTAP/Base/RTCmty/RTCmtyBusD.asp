<!-- #include virtual="/WebUtilityV3/DBAUDI/zzDataList.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=2
  title="社區營業員資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT * FROM RTCmtyBus WHERE Comq1=0 "
  sqlList="SELECT * FROM RTCmtyBus WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
    If Not IsDate(dspKey(2)) Then
       message="請輸入生效日期"
       formValid=False
    'ElseIf Not IsDate(dspKey(3)) Then
    '   message="請輸入截止日期"
    '   formValid=False
    'ElseIf DateDiff("d",DateValue(dspKey(2)),DateValue(dspKey(3)))<0 Then
    '   message="截止日期不得大於生效日期"
    '   formValid=False
    elseif IsDate(dspkey(3)) then
        if DateDiff("d",DateValue(dspKey(2)),DateValue(dspKey(3)))<0 Then
           message="截止日期不得大於生效日期"
           formValid=False
        end if
    end if

End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
    s=FrGetCmtyDesc(aryParmKey(0))%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
</table>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListHead>社區序號</td>
    <td width="30%" bgcolor="silver">
        <input class=dataListEntry type="text" name="key0" readonly
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(0)%>"></td>
    <td width="20%" class=dataListHead>營業員姓名</td>
    <td width="30%" bgcolor="silver">
<%  Dim conn,rs,s,sx,sql
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN
    If accessMode="A" and sw="" Then
       sql="SELECT [RTObj].[CUSID] AS CusID, [RTObj].[CUSNC] AS CusNC " _
          &"FROM RTObj INNER JOIN RTObjLink ON [RTObj].[CUSID]=[RTObjLink].[CUSID] " _
          &"WHERE ((([RTObjLink].[CUSTYID])='09')) " _
          &"ORDER BY [RTObj].[CUSNC] "
    Else
       sql="SELECT RTObj.CUSID AS CusID, RTObj.CUSNC AS CusNC " _
          &"FROM RTObj " _
          &"WHERE RTObj.CUSID='" &dspKey(1) &"' "
    End If
    rs.Open sql,conn
    s=""
    sx=" selected "
    Do While Not rs.Eof
       If rs("CusID")=dspKey(1) Then sx=" selected "
       s=s &"<option value=""" &rs("CusID") &"""" &sx &">" &rs("CusNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close
    rs.Open "SELECT RTObj.SHORTNC AS BusCo, RTBussMan.BRANCH AS Branch " _
           &"FROM (RTBussMan INNER JOIN RTBranch ON RTBussMan.BRANCH = RTBranch.BRANCH) " _
           &"INNER JOIN RTObj ON RTBranch.CUSID = RTObj.CUSID " _
           &"WHERE (((RTBussMan.CUSID)='" &dspKey(1) &"')) ",conn
    If Not rs.Eof Then
       busCo=rs("BusCo")
       branch=rs("Branch")
    End If 
    rs.Close
    conn.Close
    Set rs=Nothing
    Set conn=Nothing%>
        <select class=dataListEntry name="key1" <%=keyProtect%> size="1"
               style="text-align:left;" maxlength="8"><%=s%></select></td></tr>
<tr><td width="20%" class=dataListHead>證券公司</td>
    <td width="30%" bgcolor="silver">
        <input class=dataListdata type="text" readonly
               style="text-align:left;" maxlength="8"
               value="<%=busCo%>"></td>
    <td width="20%" class=dataListHead>分行名稱</td>
    <td width="30%" bgcolor="silver">  
        <input class=dataListdata type="text" readonly
               style="text-align:left;" maxlength="8"
               value="<%=branch%>"></td>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
' -------------------------------------------------------------------------------------------- 
    logonid=session("userid")
    if dspmode="新增" then
        if len(trim(dspkey(4))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                EUsrNc=V(1) 
                dspkey(4)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(4))
                V=split(rtnvalue,";")      
                EUsrNc=V(1)
        End if  
       dspkey(5)=datevalue(now())
    else
        if len(trim(dspkey(6))) < 1 then
           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
                V=split(rtnvalue,";")  
                UUsrNc=V(1)
                DSpkey(6)=V(0)
        else
           Call SrGetEmployeeRef(rtnvalue,2,dspkey(6))
                V=split(rtnvalue,";")      
                UUsrNc=V(1)
        End if         
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(4))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(7)=datevalue(now())
    end if      %>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListHead>生效日期</td>
    <td width="30%" bgcolor="silver">
        <input class=dataListEntry type="text" name="key2" <%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(2)%>"></td>
    <td width="20%" class=dataListHead>截止日期</td>
    <td width="30%" bgcolor="silver">
        <input class=dataListEntry type="text" name="key3" <%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(3)%>"></td></tr>
<tr><td width="20%" class=dataListHead>輸入人員</td>
    <td width="30%" bgcolor="silver">
        <input class=dataListData type="text" name="key4" readonly
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(4)%>"><%=EusrNc%></td>
    <td width="20%" class=dataListHead>輸入日期</td>
    <td width="30%" bgcolor="silver">
        <input class=dataListData type="text" name="key5" readonly
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(5)%>"></td></tr>
<tr><td width="20%" class=dataListHead>異動人員</td>
    <td width="30%" bgcolor="silver">
        <input class=dataListData type="text" name="key6" readonly
               style="text-align:left;" maxlength="6"
               value="<%=dspKey(6)%>"><%=UUsrNc%></td>
    <td width="20%" class=dataListHead>異動日期</td>
    <td width="30%" bgcolor="silver">
        <input class=dataListData type="text" name="key7" readonly
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(7)%>"></td></tr>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->