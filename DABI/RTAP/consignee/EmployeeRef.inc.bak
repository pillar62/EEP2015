<%
Function SrGetEmployeeRef(RTNValue,OPT,Code)
'-----------------------------------------------------------------
' RTNValue=回傳值
'      OPT=執行項目
'     CODE=傳入參數 
'-----------------------------------------------------------------
'      OPT=1:由(logon user)讀取員工相關資料
'      OPT=2:由(工號)讀取相關資料
' array(0)=Emply(工號)
' array(1)=cusnc(對象名稱)
' array(2)=shortnc(對象簡稱)
' array(3)=CUSID(身份證字號)
'-----------------------------------------------------------------
'      OPT=3:由(logon user)讀取判斷員工資料是否存在
'      OPT=4:由(工號)讀取判斷員工資料是否存在
'      OPT=5:由(logon user)讀取員工部門資料
' rtnvalue="Y":表員工資料存在
'-----------------------------------------------------------------
 rtnvalue=""
 SELECT Case OPT
   Case 1
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC , RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID " _
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE netid = '" & code & "'"
        ' response.write "SQL=" & SQL
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      if rs.recordcount > 0 then
         RTNVALUE= rs("EMPLY") & ";" & rs("CUSNC") & ";" & rs("SHORTNC") & ";" & rs("CUSID")
      else
         rtnvalue=";;;"
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing
   Case 2
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC, RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID  " _ 
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE emply = '" & code & "'"
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      If rs.recordcount > 0 then
         RTNVALUE= rs("EMPLY") & ";" & rs("CUSNC") & ";" & rs("SHORTNC") & ";" & rs("CUSID")
      else
         Rtnvalue=";;;"
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing
   Case 3
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC , RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID  " _
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE netid = '" & code & "'"
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      if rs.recordcount > 0 then
         RTNVALUE= "Y"
      else
         rtnvalue=""
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing      
   Case 4
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC, RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID  " _ 
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE emply = '" & code & "'"
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      If rs.recordcount > 0 then
         RTNVALUE= "Y"
      else
         Rtnvalue=""
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing      
   Case 5
      set conn=server.CreateObject("ADODB.Connection")
      set rs=server.CreateObject("ADODB.recordset")
      DSN="DSN=RTlib"
      SQL="SELECT RTObj.CUSNC, RTObj.SHORTNC, RTEmployee.EMPLY, " _
         &"RTEmployee.NETID,RTEmployee.CUSID,rtemployee.dept  " _ 
         &"FROM RTObj INNER JOIN " _
         &"RTEmployee ON RTObj.CUSID = RTEmployee.CUSID " _
         &"WHERE netid = '" & code & "'"
      Conn.Open DSN
      Rs.Open SQL,DSN,1,1
      If rs.recordcount > 0 then
         RTNVALUE= rs("dept")
      else
         Rtnvalue=""
      end if
      Rs.Close
      Conn.Close
      Set Rs=Nothing
      Set Conn=Nothing            
   Case Else
 
 End Select
End Function
%>