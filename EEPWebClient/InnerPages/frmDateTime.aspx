<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmDateTime.aspx.cs" Inherits="InnerPages_frmDateTime" Theme="InnerPageSkin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../css/innerpage/datetime.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">        
        function bodyLoad()
        {
            var tradate ="<%=TraDate%>";
            if(tradate == "True")
            {
                var calendar = document.getElementById("Calendar1");
                var sDate = calendar.getElementsByTagName('td')[2].innerHTML;
                var datepart = sDate.split('');
                var syear='',sother='';
                var yearOver = false;
                // 只考虑中文繁体
                for(var i = 0; i < datepart.length; i++)
                {
                    var j = datepart[i];
                    if(!yearOver)
                    {
                        if(parseInt(j) <= 9 && parseInt(j) >= 0)
                        {
                            syear += j;
                        }
                        else
                        {
                            yearOver = true;
                            sother += j;
                        }
                    }
                    else
                    {
                        sother += j;
                    }
                }
                var year = parseInt(syear) - 1911;
                calendar.getElementsByTagName('td')[2].innerHTML = year.toString() + sother;
            }
        }
    </script>

</head>
<body onload="bodyLoad()">
    <form id="form1" runat="server">
    
    <div id="main_container">
    <center>
        <table>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlSelYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSelYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblYear" runat="server" ForeColor="White"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSelMonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSelMonth_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblMonth" runat="server" ForeColor="#FFFFCC"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Calendar ID="Calendar1" runat="server" 
            OnSelectionChanged="Calendar1_SelectionChanged" 
            OnVisibleMonthChanged="Calendar1_VisibleMonthChanged" SkinID="GloCalendar" 
            onprerender="Calendar1_PreRender">
        </asp:Calendar>
         </center>
    </div>

    </form>
</body>
</html>
