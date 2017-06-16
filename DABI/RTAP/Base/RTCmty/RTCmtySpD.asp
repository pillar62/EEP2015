<!-- #include virtual="/WebUtilityV3/DBAUDI/zzDataList.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLib"
  numberOfKey=2
  title="社區管委會資料維護"
  formatName=";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;"
  sqlFormatDB="SELECT * FROM RTCmtySp WHERE Comq1=0 "
  sqlList="SELECT * FROM RTCmtySp WHERE "
  userDefineKey="Yes"
  userDefineData="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
  ' (自動編號) 存檔時預設值給-1
    If accessMode="A" And sw="S" Then dspKey(1)=-1
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineKey()
    Dim i
    s=FrGetCmtyDesc(aryParmKey(0))%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>	
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<input type="text" style="display:none" name=key0 value="<%=dspKey(0)%>">
<input type="text" style="display:none" name=key1 value="<%=dspKey(1)%>">
<%
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrGetUserDefineData()
'-------UserInformation----------------------       
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
        Call SrGetEmployeeRef(rtnvalue,2,dspkey(11))
             V=split(rtnvalue,";")      
             EUsrNc=V(1)
        dspkey(14)=datevalue(now())
    end if      
    
    Dim conn,rs,i,sql,s,t
    Set conn=Server.CreateObject("ADODB.Connection")
    Set rs=Server.CreateObject("ADODB.Recordset")
    conn.open DSN%>
<table width="100%" border=1 cellPadding=0 cellSpacing=0 height="173">
<tr><td width="32%" class=dataListHead height="23">職務名稱</td>
    <td width="49%" height="23" bgcolor="silver">
<%  If sw="E" Or (accessMode="A" And sw="") Then
        sql="SELECT RTCode.CODE, RTCode.CODENC FROM RTCode WHERE RTCode.KIND='A1' "
    Else
        sql="SELECT RTCode.CODE, RTCode.CODENC FROM RTCode WHERE RTCode.KIND='A1' " _
           &"AND RTCode.CODE='" &dspKey(3) &"' "
    End If
    rs.Open sql,conn
    s=""
    sx=" selected "
    Do While Not rs.Eof
       If rs("Code")=dspKey(3) Then sx=" selected "
       s=s &"<option value=""" &rs("Code") &"""" &sx &">" &rs("CodeNC") &"</option>"
       rs.MoveNext
       sx=""
    Loop
    rs.Close%>
        <select class=dataListEntry name="key3" <%=dataProtect%> size="1"
               style="text-align:left;" maxlength="8"><%=s%></select></td>
    <td width="16%" class=dataListHead height="23">姓名</td>
    <td width="41%" height="23" bgcolor="silver">
        <!--webbot bot="Validation" B-Value-Required="TRUE"
        I-Maximum-Length="10" -->
        <input class=dataListEntry type="text" name="key2" <%=dataProtect%>
               style="text-align:left;" maxlength="10"
               value="<%=dspKey(2)%>" size="16"></td></tr>
<tr><td width="32%" class=dataListHead height="23">聯絡電話</td>
    <td width="49%" height="23" bgcolor="silver">
        <!--webbot bot="Validation" B-Value-Required="TRUE"
        I-Maximum-Length="15" -->
        <input class=dataListEntry type="text" name="key4" <%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(4)%>" size="22"></td>
    <td width="16%" class=dataListHead height="23">行動電話</td>
    <td width="41%" height="23" bgcolor="silver">
        <!--webbot bot="Validation" B-Value-Required="TRUE"
        I-Maximum-Length="15" -->
        <input class=dataListEntry type="text" name="key5" <%=dataProtect%>
               style="text-align:left;" maxlength="15"
               value="<%=dspKey(5)%>" size="16"></td></tr>
<tr><td width="32%" class=dataListHead height="23">喜好</td>
    <td width="106%" colspan="3" height="23" bgcolor="silver">
        <input class=dataListEntry type="text" name="key6" <%=dataProtect%>
               style="text-align:left;" maxlength="50"
               value="<%=dspKey(6)%>" size="57"></td>
</tr>
<tr><td width="32%" class=dataListHead height="23">聯絡地址</td>
    <td width="106%" colspan="3" height="23" bgcolor="silver">
<%  Call SrGetCountyTownShip(accessMode,sw,Len(Trim(dataProtect)),dspKey(7),dspKey(8),s,t)%>
         <select size="1" name="key7" class="dataListEntry" <%=dataProtect%> size="1" 
                onChange="SrRenew()"><%=s%></select>
         <select class=dataListEntry name="key8" <%=dataProtect%> size="1"><%=t%></select>    
 <!--webbot bot="Validation" B-Value-Required="TRUE" I-Maximum-Length="60" --><input type="text" name="key9" size="60" value="<%=dspKey(9)%>" class="dataListEntry" maxlength="60" <%=dataProtect%>></td>   
</tr>
<tr>
  <td width="32%" class=dataListHead height="21">背景說明</td>
    <td width="106%" height="21" colspan="3" bgcolor="silver">
        <textarea rows="2" name="key10" cols="54" class="dataListEntry"><%=dspKey(10)%></textarea>

        　</td>
</tr>
<tr>
  <td width="32%" class=dataListHead height="23">輸入人員</td>
    <td width="49%" height="23" bgcolor="silver">
        <input type="text" name="key11" readonly
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(11)%>" size="22" class=dataListData><%=EusrNc%></td>
    <td width="16%" class=dataListHead height="23">輸入日期</td>
    <td width="41%" height="23" bgcolor="silver">
        <input type="text" name="key12" readonly
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(12)%>" size="16" class=dataListData></td>
</tr>
<tr>
  <td width="32%" class=dataListHead height="23">異動人員</td>
    <td width="49%" height="23" bgcolor="silver">
        <input type="text" name="key13" readonly
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(13)%>" size="22" class=dataListData><%=UUsrNc%></td>
    <td width="16%" class=dataListHead height="23">異動日期</td>
    <td width="41%" height="23" bgcolor="silver">
        <input type="text" name="key14" readonly
               style="text-align:left;" maxlength="8"
               value="<%=dspKey(14)%>" size="16" class=dataListData></td>
</tr>
</table>
<%
End Sub
' -------------------------------------------------------------------------------------------- 
%>
<!-- #include file="RTGetCmtyDesc.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<!-- #include file="RTGetCountyTownShip.inc" -->