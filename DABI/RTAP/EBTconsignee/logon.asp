<%  
  usr = request("usr")
  pwd = request("pwd")
  typ = request("typ")
  Msg="請輸入您的使用者帳號及密碼"    
  session("passed") = False
  
  if typ = "login" or typ="" then
     typ1 = "checked" 
     typ2 = "" 
  elseif typ = "chgpw" then
     typ1 = "" 
     typ2 = "checked" 
  end if
  
  If Not (IsEmpty(Request("Usr")) or Request("Usr") = "" or Request("PWD") = "" or IsEmpty(Request("PWD"))) Then
     Set conn=Server.CreateObject("ADODB.Connection")
     conn.open "DSN=RTLib"
     Set rs=Server.CreateObject("ADODB.Recordset")
    '必須於經銷商與isp關係檔中存在經銷商與東森isp的關係資料者,才可進入
     sql = "SELECT rtconsignee.CUSID, rtconsignee.CUSNO, rtconsignee.PASSWORD,rtconsignee.COMTYPE FROM RTConsignee inner join rtconsigneeisp on rtconsignee.cusid=rtconsigneeisp.cusid and rtconsigneeisp.isp='04' WHERE CUSNO='" & USR & "'"     
     
     rs.open sql,conn
     If Not rs.EOF Then
        if pwd <> trim(rs("PASSWORD")) then
           msg ="密碼錯誤！"
        else
           msg ="密碼正確！"
           Session("UserID")=rs("CUSID")
           Session("COMTYPE")=rs("COMTYPE")
           'Session("UserPW")=rs("CUSID")
           Session("passed") = True
           if typ = "login" then '重導至[經銷商]的主畫面
              '東森
              if TRIM(rs("cusid"))="70771579" then
                 Response.Redirect "http://www.avsl.com.tw/avsconsignee/rtebtcmtyinq.asp"
              else
                 Response.Redirect "http://www.avsl.com.tw/avsconsignee/rtebtcmtyinq.asp"
              end if
           elseif typ = "chgpw" then '重導至[修改密碼]的頁面
              Response.Redirect "http://www.avsl.com.tw/avsconsignee/chgpwd2.asp?uid="&usr
           end if   
        end if
     else
        Msg="帳號不存在!"
     End If
     rs.Close
     conn.close
  End If
  
%>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=big5">
		<title>元訊經銷商客戶查詢系統</title>
	</head>
	<body bgcolor="" text="#000000">
<table width="700" border="0" cellspacing="0" cellpadding="0" height="385">
  <tr> 
    <td> 
      <div align="center"> 
        <form action="logon.asp" name="frm1" method="post">
          <p>&nbsp;</p>
          <p align="center"><b><font color="#000000" size="3"><img src="images/consignee.gif" width="511" height="95"></font> 
            <br>
            </b> 
          <p> 
          <table align="center" width="521" border="1" cellspacing="1" cellpadding="3" bordercolor="#003366" height="51">
            <tr> 
              <td width="76" height="16" bgcolor="#E0F0F8" align="center"><font color="#000000" size="2">帳 
                號</font></td>
              <td width="191" height="16" bgcolor="#E0F0F8"> 
                <input type="TEXT" id="usr" name="usr" size=15 value="<%=USr%>">
              </td>
              <td width="98" height="16" bgcolor="#E0F0F8"> 
                <p align="center"><font color="#000000" size="2">密 碼</font> 
              </td>
              <td width="167" height="16" bgcolor="#E0F0F8"> 
                <input type="password" id="pwd" name="PWD" size=15 value="<%=PWD%>">
              </td>
            </tr>
            <tr> 
              <td width="98" height="16" bgcolor="#E0F0F8"> 
                <p align="center"><font color="#000000" size="2">執行動作</font> 
              </td>
              <td width="532" height="16" bgcolor="#E0F0F8" colspan="4"> 
                <p> 
                  <input type="radio" name=typ value=login <%=typ1%>>
                  <font size="2">登入</font>&nbsp;&nbsp;&nbsp; 
                  <input type="radio" name=typ value=chgpw <%=typ2%>>
                  <font size="2">修改密碼</font></p>
              </td>
            </tr>
            <tr> 
              <td width="532" height="16" bgcolor="" colspan="4"> 
                <p align="center"> 
                  <input type="submit" name="b1" value="執　行">
                </p>
              </td>
            </tr>
            <tr> 
              <td width="76" height="16" bgcolor="#E0F0F8" align="center"><font size="2">訊息列</font></td>
              <td width="456" height="16" colspan="3" bgcolor="#E0F0F8"><font size="2">&nbsp;<%=msg%></font> 
              </td>
            </tr>
          </table>
          <p><br>
            <br>
            <br>
            <br>
            <br>
            <br>
          </p>
        </form>
      </div>
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
    <td>&nbsp;</td>
  </tr>
</table>
</body>
</html>
