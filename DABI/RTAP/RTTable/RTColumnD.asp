<!-- #include virtual="/WebUtilityV4/dBAUDI/zzDataList.inc" -->
<!-- #include virtual="/Webap/include/employeeref.inc" -->
<%
' -------------------------------------------------------------------------------------------- 
Sub SrEnvironment()
  DSN="DSN=RTLIB"
  numberOfKey=2
  title="Table欄位資料維護檔"
  formatName="Table名稱(英);欄位名稱(英);欄位名稱(中);資料類別;主鍵;Null;預設值;備註"  
  sqlFormatDB="SELECT tbName, colName, colNameC, colDataType, " &_
			"       colIsKey, colIsNull, colDefault, colDesc " &_
            "FROM   AColumnList WHERE tbName ='*' " 
  sqlList="SELECT tbName, colName, colNameC, colDataType, " &_
	      "       colIsKey, colIsNull, colDefault, colDesc " &_
          "FROM   AColumnList WHERE  " 
  userDefineData="Yes"
  userDefineKey="Yes"
End Sub
' -------------------------------------------------------------------------------------------- 
Sub SrCheckData(message,formValid)
'RECORD CHECK
'    set conn=server.CreateObject("ADODB.connection")
'    set rs=server.CreateObject("ADODB.recordset")
'    DSN="DSN=Rtlib"
'    sql="select * from rtctytown where cutid='" & dspkey(0) & "'" & " AND Township='" & dspkey(1) & "'"
'    conn.Open dsn
'    rs.Open sql,dsn,1,1
'    if not rs.EOF and dspmode = "新增" then
'       formvalid=False
'       message="資料重覆"
'    end if
'    rs.Close
'    conn.Close
'    set rs=nothing
'    set conn=nothing    
'DATA CHECK    
    'If dspKey(0)="" Then
    '   formValid=False
    '   message="請輸入轄區代碼"
    'End If
'    If dspKey(0)="" Then
'       formValid=False
'       message="請輸入縣市代碼"
'    End If
'    If dspKey(1)="" Then
'       formValid=False
'       message="請輸入鄉鎮區名稱"
'    End If    
    'If dspKey(2)="" Then
    '   formValid=False
    '   message="請輸入轄區類別"
    'End If
End Sub

Sub SrGetUserDefineData()
'Get Employee Name and EmployID   
'-----------------------------------------------------------------
' OPT=1:由(logon user)讀取員工相關資料
' OPT=2:由(工號)讀取相關資料
'-----------------------------------------------------------------
' V(0)=Emply(工號)
' V(1)=cusnc(對象名稱)
' V(2)=shortnc(對象簡稱)
'-----------------------------------------------------------------
'    logonid=session("userid")
'        if len(trim(dspkey(2))) < 1 then
'           Call SrGetEmployeeRef(Rtnvalue,1,logonid)
'                V=split(rtnvalue,";")  
'                EUsrNc=V(1) 
'                dspkey(2)=V(0)
'        else
'           Call SrGetEmployeeRef(rtnvalue,2,dspkey(2))
'                V=split(rtnvalue,";")      
'                EUsrNc=V(1)
'        End if  
'       dspkey(3)=datevalue(now())
%>   
     <table border=1 cellPadding=0 cellSpacing=0 width="100%" align=center>
     <tr>
     <td width="30%" class=dataListHead><%=aryKeyName(2)%></td>
     <td width="70%" bgcolor=silver>
     <input class=dataListData name="key2" maxlength=30 size =35 style="TEXT-ALIGN: left"
            value="<%=dspkey(2)%>" readOnly></td></tr>    
     <tr>
     <td width="30%" class=dataListHead><%=aryKeyName(3)%></td>
     <td width="70%" bgcolor=silver>
     <input class=dataListData name="key3" maxlength=20 size=25 style="TEXT-ALIGN: left" 
            value="<%=dspkey(3)%>" readOnly></td></tr>
     <tr>
     <td width="30%" class=dataListHead><%=aryKeyName(4)%></td>
     <td width="70%" bgcolor=silver>
     <input class=dataListData name="key4" maxlength=1 size=5 style="TEXT-ALIGN: left" 
            value="<%=dspkey(4)%>" readOnly></td></tr>
     <tr>
     <td width="30%" class=dataListHead><%=aryKeyName(5)%></td>
     <td width="70%" bgcolor=silver>
     <input class=dataListData name="key5" maxlength=1 size=5 style="TEXT-ALIGN: left" 
            value="<%=dspkey(5)%>" readOnly></td></tr>
     <tr>
     <td width="30%" class=dataListHead><%=aryKeyName(6)%></td>
     <td width="70%" bgcolor=silver>
     <input class=dataListData name="key6" maxlength=20 size=25 style="TEXT-ALIGN: left" 
            value="<%=dspkey(6)%>" readOnly></td></tr>
     <tr>
     <td width="30%" class=dataListHead><%=aryKeyName(7)%></td>
     <td width="70%" bgcolor=silver>
     <input class=dataListEntry name="key7" maxlength=100 size=70 style="TEXT-ALIGN: left" 
            value="<%=dspkey(7)%>"></td></tr>
            
     </table></TABLE>

<%End Sub%>

<%Sub SrGetUserDefineKey() %>
<table width="100%" border=1 cellPadding=0 cellSpacing=0>
<tr><td width="20%" class=dataListSearch>資料範圍</td>
    <td width="80%" class=dataListSearch2><%=s%></td></tr>
</table>
<p>
      <table width="100%" border=1 cellPadding=0 cellSpacing=0>
       <tr><td width="30%" class=dataListHead><%=aryKeyName(0)%></td>
           <td width="70%" bgcolor=silver><input class=dataListData type="text" name="key0"
                 <%=keyProtect%> size="30" value="<%=dspKey(0)%>" maxlength="30"></td></tr>
                 
       <tr><td width="30%" class=dataListHead><%=aryKeyName(1)%></td>
           <td width="70%" bgcolor=silver><input class=dataListData type="text" name="key1"
                 <%=keyProtect%> size="25" value="<%=dspKey(1)%>" maxlength="20"></td></tr>    
                 
      </table>
<%End Sub%>

