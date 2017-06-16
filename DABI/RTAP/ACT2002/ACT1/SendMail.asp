<%
  Set conn=Server.CreateObject("ADODB.Connection")  
  Set RS=Server.CreateObject("ADODB.recordset")  
  DSN="DSN=RTLib"
  conn.Open DSN  
  sql="select * from HB2002ACT1 where  DROPDAT IS NULL AND SENDMAILDAT IS NULL and email='edson@cbbn.com.tw' "  
  rs.Open sql,conn
  subject="元訊寬頻2002客戶回娘家抽獎活動---抽獎序號.."
  body="<html><body><table border=0 width=""100%""><tr><td colspan=2>" _
        &"<H3>元訊寬頻2002客戶回娘家抽獎活動---抽獎序號</h3></td></tr>" _
        &"<tr><td width=""30%"">&nbsp;</td><td width=""70%"">&nbsp;</td></tr>" _
        &"<tr><td bgcolor=lightblue align=center>抽獎序號</td><td bgcolor=pink align=left>" &rs("serno") &"</td></tr>" _
        &"<tr><td bgcolor=lightblue align=center>姓名</td><td bgcolor=pink align=left>" &rs("name") &"</td></tr>" _
        &"<tr><td bgcolor=lightblue align=center>性別</td><td bgcolor=pink align=left>" &rs("sexc") &"</td></tr>" _
        &"<tr><td bgcolor=lightblue align=center>公司電話</td><td bgcolor=pink align=left>" &rs("telc") & "#" & rs("EXT") &"</td></tr>" _
        &"<tr><td bgcolor=lightblue align=center>住家電話</td><td bgcolor=pink align=left>" &rs("telH") &"</td></tr>" _
        &"<tr><td bgcolor=lightblue align=center>行動電話</td><td bgcolor=pink align=left>" &rs("mobile") & "</td></tr>" _
        &"<tr><td bgcolor=lightblue align=center>E-Mail</td><td bgcolor=pink align=left>" &rs("email") &"</td></tr>" _
        &"<tr><td bgcolor=lightblue align=center>資料填寫日</td><td bgcolor=pink align=left>" &rs("edat") &"</td></tr>" _
        &"<tr><td>&nbsp;</td><td><br><input type=button value="" 結束 "" onclick=""vbscript:window.close""" _
        &" style=""cursor:hand"" id=button1 name=button1></td></tr>" _
        &"</table></body></html>"  
  email="service@cbbn.com.tw"
  Set objMail=CreateObject("CDONTS.Newmail")   
  Do while not rs.EOF
  '   rs("sendmaildat")=now()
  '   rs.update     
 '    objMail.BodyFormat=0
 '    objMail.MailFormat=0
     Response.Write "from=" & email & ";TO=" & rs("email") & ";subject=" & subject & ";body=" & body & "<BR>"
     objMail.Send email,rs("email"),subject,body

  loop
  Set ObjMail=Nothing
%>
