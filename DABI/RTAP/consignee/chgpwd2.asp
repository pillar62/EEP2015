<%  
  uid=request("uid")
  if len(trim(uid))=0 then
     Response.Redirect "http://www.cbbn.com.tw/consignee/logon.asp"
  end if
  
  pwd2=request("pwd2")  
  pwd3=request("pwd3")  
  
  if pwd2 =""or pwd3= "" then
     msg="密碼不能是空白"
  elseif pwd2 <> pwd3 then
     msg="密碼未正確重覆輸入！"
  else   
     nsg=""
  
     Set conn=Server.CreateObject("ADODB.Connection")
     conn.open "DSN=RTLib"
     Set rs=Server.CreateObject("ADODB.Recordset")
     sql="select CUSID, CUSNO, PASSWORD from RTCONSIGNEE where CUSNO='" & uid & "'"
     rs.open sql,conn,3,3
     rs("password")=pwd2
     rs.update
     rs.Close
     set rs=nothing
     conn.Close
     set conn=nothing
     
     Response.Redirect "http://www.cbbn.com.tw/consignee/logon.asp"
  end if     
%>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=big5">
		<title>使用者密碼變更作業</title>
	</head>
	<body>
<table width="700" border="0" cellspacing="0" cellpadding="0" height="287">
  <tr> 
    <td> 
      <div align="center"><br>
        <br>
        <img src="images/chpw.gif" width="513" height="32"><br>
        <br>
        <br>
        <br>
        <form name="frm1" method="post">
          <table align="center" width="77%" border="1" cellspacing="1" cellpadding="3" bordercolor="#003366">
            <tr> 
              <td width="20%" height="16" bgcolor="#e0f0f8" align="center"><font size="2">輸 
                入 新 密 碼</font></td>
              <td width="80%" height="16" bgcolor="#e0f0f8"> 　
<input type="password" name="pwd2" size=15 value="<%=pwd2%>">
              </td>
            </tr>
            <tr> 
              <td width="20%" height="16" bgcolor="#e0f0f8" align="center"><font size="2">新 
                密 碼 確 認</font></td>
              <td width="80%" height="16" bgcolor="#e0f0f8"> 　
<input type="password" name="pwd3" size=15 value="<%=pwd3%>">
              </td>
            </tr>
            <tr> 
              <td width="20%" height="20" bgcolor="#e0f0f8" align="center"><font size="2">訊 
                息 列</font></td>
              <td width="80%" height="20" bgcolor="e0f0f8" bordercolor="#003063"><font size="2"><%=msg%>&nbsp;</font></td>
            </tr>
            <tr bgcolor="#F0F8FF"> 
              <td colspan="2" height="20" align="center"> 
                <input type="submit" name="cmdok" value="　變　更　">
                　
<input type="reset" name="cmdcancel" value="　重　設　">
              </td>
            </tr>
          </table>
          <br>
          <center>
            <div></div>
          </center>
        </form>
      </div>
    </td>
  </tr>
  <tr> 
    <td> 
      <p>&nbsp;</p>
      <p align="center">&nbsp;</p>
      <p>&nbsp;</p>
    </td>
  </tr>
  <tr> 
    <td> 
      <div align="right"> </div>
      <hr size="1" color=666666  noshade width="600">
    </td>
  </tr>
  <tr> 
    <td height="2"> 
      <div align="center"> <font color="#333333" size="2" face="Verdana, Arial, Helvetica, sans-serif">Copyright 
        2001 COSMACTIVE CO.,Ltd </font> </div>
    </td>
  </tr>
  <tr> 
    <td> 
      <div align="center"></div>
    </td>
  </tr>
</table>
</body>
</html>
