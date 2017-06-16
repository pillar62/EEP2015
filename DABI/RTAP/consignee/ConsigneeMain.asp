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
	
<body leftmargin="1" topmargin="1" bgcolor="#FFFFFF">
<table width="637" border="0" cellspacing="0" cellpadding="0">
  <tr> 
    <td> 
      <div align="center"> 
        <form>
          &nbsp; 
          <p><img src="images/ConsigneeMain.gif" width="513" height="33"><br>
          </p>
        </form>
        <table width="636" bgcolor="#FFFFFF" align="left">
          <tr>
            <td align="left"> 
              <table id="Table1" height="107" border="1" width="627" cellspacing="1" cellpadding="1" bordercolor="#003366">
                <tr> 
                  <td width="6%" bgcolor="#e0f0f8" align="center"><font size="2"><b>類別</b></font></td>
                  <td width="6%" bgcolor="#e0f0f8" align="center"><font size="2"><b>分類</b></font></td>
                  <td width="40%" bgcolor="#e0f0f8" align="center"><font size="2"><b>作業名稱</b></font></td>
                  <td width="7%" bgcolor="#e0f0f8" align="center"><font size="2"><b>分類</b></font></td>
                  <td width="41%" bgcolor="#e0f0f8" align="center"><font size="2"><b>作業名稱</b></font></td>
                </tr>
                <tr bgcolor="#EAEAF4"> 
                  <td width="6%" align="center"><font size="2">查詢</font></td>
                  <td rowspan="5" align="left"> 
                    <div align="center"><font size="2">速博</font></div>
                  </td>
                  <td width="40%" align="left"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeCmtyK.asp" target="_blank"><font size="2" >速博399社區資料</font></a></td>
                  <td rowspan="5" align="left"> 
                    <div align="center"><font size="2">中華</font></div>
                  </td>
                  <td width="41%" align="left"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeCHT399CmtyK.asp" target="_blank"><font size="2" >中華399社區資料</font></a></td>
                </tr>
                <tr bgcolor="#FEFEFE"> 
                  <td width="6%" align="center"><font size="2">查詢</font></td>
                  <td width="40%" align="left" bgcolor="#FEFEFE"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeCustKK.asp" target="_blank"><font size="2">速博399客戶資料</font></a></td>
                  <td width="41%" align="left" bgcolor="#FEFEFE"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeHBCmtyK.asp" target="_blank"><font size="2">中華Hi-Building社區資料</font></a></td>
                </tr>
                <tr bgcolor="#EAEAF4"> 
                  <td width="6%" align="center"><font size="2">查詢</font></td>
                  <td width="40%" align="left"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeallcmtyK.asp" target="_blank"><font size="2">速博399全部社區資料查詢(全部經銷商)</font></a></td>
                  <td width="41%" align="left"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeCustKK3.asp" target="_blank"><font size="2">中華399客戶資料</font></a></td>
                </tr>
                <tr bgcolor="#FEFEFE"> 
                  <td width="6%" align="center"><font size="2">查詢</font></td>
                  <td width="40%" align="left" bgcolor="#FEFEFE"><font size="2"><font color="#6894E0">■<a href="http://www.cbbn.com.tw/Consignee/ConsigneeSparq499CustK2.asp" target="_blank"><font size="2"> 
                    速博499客戶資料</font></a></font></font></td>
                  <td width="41%" align="left" bgcolor="#FEFEFE"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeHBCustKK.asp" target="_blank"><font size="2">中華Hi-Building客戶資料</font></a></td>
                </tr>
                <tr bgcolor="#FEFEFE"> 
                  <td width="6%" align="center" bgcolor="#EAEAF4"><font size="2">查詢</font></td>
                  <td width="40%" align="left" bgcolor="#EAEAF4"><font size="2"><font color="#6894E0">■<a href="http://www.cbbn.com.tw/Consignee/ConsigneeSparqVoIPCustK.asp" target="_blank"><font size="2"> 
                    速博VOIP客戶資料</font></a></font></font></td>
                  <td width="41%" align="left" bgcolor="#EAEAF4"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeallHBcmtyK.asp" target="_blank"><font size="2">Hi-Building全部社區資料查詢(全部經銷商)</font></a></td>
                </tr>
                <tr bgcolor="#EAEAF4"> 
                  <td width="6%" align="center"><font size="2">查詢</font></td>
                  <td width="6%" align="left"><font size="2">東森</font></td>
                  <td width="40%" align="left"><font size="2"><font color="#6894E0">■&nbsp;</font></font><a href="http://www.cbbn.com.tw/Consignee/ConsigneeEBTcmtyLINEK2.asp" target="_blank"><font size="2">東森AVS主線資料查詢</font></a></td>
                  <td width="7%" align="left">&nbsp;</td>
                  <td width="41%" align="left">&nbsp;</td>
                </tr>
              </table>
            </td>
          </tr></table>
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
