<HTML>
	<HEAD>
		<title></title>
		<%
		if not Session("passed") then
	Response.Redirect "http://www.cbbn.com.tw/Consignee/logon.asp"
end if
%>
		<meta http-equiv="Content-Type" content="text/html; charset=big5">
		<script id="clientEventHandlersJS" language="javascript">
<!--


//-->
		</script>
	</HEAD>
	<body>
<table width="701" border="0" cellspacing="0" cellpadding="0">
  <tr> 
    <td> 
      <div align="center"> 
        <form>
          &nbsp; 
          <p><img src="images/ConsigneeMain.gif" width="513" height="33"><br>
            <br>
          </p>
        </form>
        <table id="Table1" align="center" height="107" border="1" width="80%" cellspacing="1" cellpadding="3" bordercolor="#003366">
          <tr> 
            <td width="10%" bgcolor="#e0f0f8" align="center"><font size="2"><b>類別</b></font></td>
            <td width="70%" bgcolor="#e0f0f8" align="center"><font size="2"><b>程式名稱</b></font></td>
            <td width="20%" bgcolor="#e0f0f8" align="center">&nbsp;</td>
          </tr>
          <tr> 
            <td width="10%" align="center"><font size="2">查詢</font></td>
            <td width="70%" align="center"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://w3c.intra.cbbn.com.tw/webap/rtap/base/consignee/ConsigneeCmtyK.asp" target="_blank"><font size="2">已送件經銷商社區資料</font></a></td>
            <td width="20%">&nbsp;</td>
          </tr>
          <tr> 
            <td width="10%" align="center"><font size="2">查詢</font></td>
            <td width="70%" align="center"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://w3c.intra.cbbn.com.tw/webap/rtap/base/consignee/ConsigneeCustKK.asp" target="_blank"><font size="2">已送件經銷商客戶資料</font></a></td>
            <td width="20%">&nbsp;</td>
          </tr>
          <tr> 
            <td width="10%" align="center"><font size="2">查詢</font></td>
            <td width="70%" align="center"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://w3c.intra.cbbn.com.tw/webap/rtap/base/consignee/ConsigneeCMTYXK.asp" target="_blank"><font size="2">未送件經銷商社區資料</font></a></td>
            <td width="20%">&nbsp;</td>
          </tr>
          <tr> 
            <td width="10%" align="center"><font size="2">查詢</font></td>
            <td width="70%" align="center"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://w3c.intra.cbbn.com.tw/webap/rtap/base/consignee/ConsigneeCustXKK.asp" target="_blank"><font size="2">未送件經銷商客戶資料</font></a></td>
            <td width="20%">&nbsp;</td>
          </tr>
          <tr> 
            <td width="10%" align="center"><font size="2">報表</font></td>
            <td width="70%" align="center"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://w3c.intra.cbbn.com.tw/rptweb/ConsigneeCustP.asp?cnid=<%=session("userid")%>" target="_blank"><font size="2">客戶資料列印</font></a></td>
            <td width="20%">&nbsp;</td>
          </tr>
          <tr> 
            <td width="10%" align="center"><font size="2">報表</font></td>
            <td width="70%" align="center"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://w3c.intra.cbbn.com.tw/rptweb/dialog.asp?cnid=<%=session("userid")%>" target="_blank"><font size="2">經銷商對帳月報表列印</font></a></td>
            <td width="20%">&nbsp;</td>
          </tr>
        </table>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
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
    <td> 
      <div align="center"></div>
    </td>
  </tr>
</table>
</body>
</HTML>
