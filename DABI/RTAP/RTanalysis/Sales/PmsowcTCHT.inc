<%
'-------------------------------------------------------------------------
Response.Buffer = True
  Server.ScriptTimeout = 3000
  Dim title,unit,diaProgram,diaWidth,diaHeight
  Dim parmDSN,parmSQL
  Dim defaultRowField,defaultColumnField,defaultFilterField
  Dim defaultChartType,defaultChartLabel
  Dim fieldFormatName,fieldFormat,fieldName
  Dim fieldTotal,fieldTotalBase,fieldTotalFunction,fieldTotalShow
  Dim subRowColorB,subColumnColorB,subRowColorF,subColumnColorF
  Dim sql,chartID
  dim defaultexpandrow,defaultexpandcolumn
  sql=Request("sql")
  chartID="chart"
  defaultChartType=0
  defaultChartLabel=0
  diaProgram=""
  diaWidth=450
  diaHeight=450
  fieldName="" 
  fieldFormatName=""
  fieldFormat=""
  subRowColorB="Wheat"
  subColumnColorB="Wheat"
  subRowColorF="Black"
  subColumnColorF="Black"
  unit=""
  srSpec()
%>
<html>
<head>
<meta http-equiv=Content-Type content="text/html; charset=Big5">
<title><%=title%></title>
</head>
<body style="background-color:silver">
<form name=frmSel action="<%=Request.ServerVariables("PATH_INFO")%>" _
      method=post>
  <input name="sql" style="display:none">
</form>
<span style="position:absoloute;left:90;top:20;cursor:hand;display:none" _
       OnClick="">
  <img src="/Webap/image/btnInquery.gif"></img></span>
<span id="cmdclose" style="position:absolute;left:120;top:20;cursor:hand">
  <img src="/Webap/image/btnEnd.gif"></img></span><br>
<div style="position:absolute;left:300;top:20;display:none">
<select size=1 id="chartType" style="width:200;font-family:細明體" onchange="showChart()">
 <option value="0">群組直條圖</option>
 <option value="1">堆疊直條圖</option>
 <option value="2">100%堆疊直條圖</option>
 <option value="3">群組橫條圖</option>
 <option value="4">堆疊橫條圖</option>
 <option value="5">100%堆疊橫條圖</option>
 <option value="6">折線圖</option>
 <option value="7">折線圖(有標記)</option>
 <option value="8">堆疊折線圖</option>
 <option value="9">堆疊折線圖(有標記)</option>
 <option value="10">堆疊折線圖(百分比顯示)</option>
 <option value="11">堆疊折線圖(有標記百分比顯示)</option>
 <option value="12">平滑曲線圖</option>
 <option value="13">平滑曲線圖(有標記)</option>
 <option value="14">堆疊平滑曲線圖</option>
 <option value="15">堆疊平滑曲線圖(有標記)</option>
 <option value="16">堆疊平滑曲線圖(百分比顯示)</option>
 <option value="17">堆疊平滑曲線圖(有標記百分比顯示)</option>
 <option value="18">圓形圖</option>
 <option value="19">分裂圓形圖</option>
 <option value="20">堆疊圓形圖</option>
</select>
</div>
<div style="position:absolute;left:500;top:20;display:none">
<select size=1 id="chartLabel" style="font-family:細明體" onchange="showChart()">
 <option value="0">不顯示數值標籤</option>
 <option value="1">顯示%數值標籤</option>
 <option value="2">顯示數值標籤</option>
</select>
</div>
<div style="position:absolute;left:650;top:20;display:none">
<input type="button" id="refresh" value="重新顯示圖形" onClick="showChart()" style="font-family:細明體;cursor:hand">
</div>
<script language="VBScript">
 chartType.SelectedIndex=<%=defaultChartType%>
 chartLabel.SelectedIndex=<%=defaultChartLabel%>
Sub cmdQuery_onClick()
    prog="<%=diaProgram%>"
    If prog="" Then
       sql="*none"
    Else
       sql=Window.showModalDialog(prog,"","dialogWidth:<%=diaWidth%>px;dialogHeight:<%=diaHeight%>px;")
    End If
    If sql <> "" And sql <> frmSel.sql.Value Then
      frmSel.sql.Value= sql
      frmSel.submit
    End If
End Sub
Sub cmdClose_onClick()
    Dim winP,docP
    Set winP=window.Opener
    Set docP=winP.document
    winP.focus()
    window.close
End Sub
<%If sql="" Or Len(sql)=0 Then%>
  cmdQuery_onClick
</script>
<%Else%>
</script>
<%SrDisplay
  End If%>
</body>
</html>
<%
'-------------------------------------------------------------------------
Sub SrDisplay
%>
<h5 align="center"><%=title%></h5>
<h5 id="heading" align="left" style="color:red">資料下載中請稍等片刻....，
您所查詢的結果將以圖表方式展現。
您可由欄位清單將資料移到表格之[分頁]、[列]、[欄]或[資料]欄內，即可呈現您所
需要的查詢結果。</h5>
<h6 align="left" style="font-family:細明體;display:none"><%=unit%></h6>
<center >
<object id="chart" 
        codebase="http://w3c.intra.cbbn.com.tw/webap/activex/OLAP.cab#version=1,0,0,0"
        classid="CLSID:0002E500-0000-0000-C000-000000000046" width="100%"
                 height="50%" style="display:none">
</object>
<object id="ptable"
        codebase="http://w3c.intra.cbbn.com.tw/webap/activex/OLAP.cab#version=1,0,0,0"
        classid="CLSID:0002E520-0000-0000-C000-000000000046" width="90%" height="70%">
</object>
<p align=left><font size=2>總送線數︰主線已向CHT申請之數量(不含已撤線)<br>
總申請戶︰已輸入正式申請日之戶數總計(不含已退租或撤銷)<br>
總開通數︰主線已測通之數量，可執行用戶裝機作業(不含已撤線)<br>
總完工戶︰用戶已裝機完成之戶數(不含已退租或撤銷)<br>
總報竣戶︰用戶已完工且已報竣至CHT之戶數(不含已退租或撤銷)</FONT></p>
<script language="VBScript">
    ptable.ConnectionString = "provider=mspersist"
    ptable.CommandText="/Webap/include/CreateXML.asp?DSN=<%=parmDSN%>&sql=<%=parmSQL%>&field=<%=fieldName%>"
    ptable.BackColor=Document.Body.Style.BackgroundColor
    ptable.DisplayToolbar=True
    set pview = ptable.ActiveView
    pview.FieldLabelBackColor=Document.Body.Style.BackgroundColor
    pview.Label.BackColor=Document.Body.Style.BackgroundColor
    pview.AutoLayout()
    rePivot "<%=defaultRowField%>","<%=defaultColumnField%>","<%=defaultFilterField%>","<%=defaultExpandrow%>","<%=defaultExpandcolumn%>"
    set c=ptable.Constants
    aryField=Split("<%=fieldFormatName%>",";")
    aryFormat=Split("<%=fieldFormat%>",";")
    For i = 0 To Ubound(aryField)
        pview.FieldSets(aryField(i)).Fields(0).NumberFormat=aryFormat(i)
    Next
    aryTotal=Split("<%=fieldTotal%>",";")
    aryTotalFunction=Split("<%=fieldTotalFunction%>",";")
    aryTotalShow=Split("<%=fieldTotalShow%>",";")
    aryTotalBase=Split("<%=fieldTotalBase%>",";")
    For i = 0 To Ubound(aryTotal)
        pview.AddTotal aryTotal(i),pview.FieldSets(aryTotalBase(i)).Fields(0),aryTotalFunction(i)
        If aryTotalShow(i) Then
           pview.DataAxis.InsertTotal pview.totals(aryTotal(i))
        End If
    Next
    For Each pfs In pview.RowAxis.FieldSets
        pfs.Fields(0).SubtotalBackColor="<%=subRowColorB%>"
        pfs.Fields(0).SubtotalForeColor="<%=subRowColorF%>"
        pfs.Fields(0).SubtotalFont.Bold=True
    Next
    For Each pfs In pview.ColumnAxis.FieldSets
        pfs.Fields(0).SubtotalBackColor="<%=subColumnColorB%>"
        pfs.Fields(0).SubtotalForeColor="<%=subColumnColorF%>"
        pfs.Fields(0).SubtotalFont.Bold=True
    Next
    ptable_QueryComplete
'..........................................................
Sub showChart()
    On Error Resume Next
    Set c=chart.Constants
    chart.Clear
    Set chart.DataSource=ptable
    Set cht=chart.Charts.Add()
    cht.HasLegend=True
    cht.Interior.Color=Document.Body.Style.BackgroundColor
    cht.Legend.Interior.Color=Document.Body.Style.BackgroundColor
    cht.PlotArea.Interior.Color=Document.Body.Style.BackgroundColor
    cht.Type=chartType.Value
    cht.SetData c.chDimSeriesNames,0,c.chPivotRowAggregates
    cht.SetData c.chDimCategories,0,c.chPivotColAggregates
    cht.SetData c.chDimValues,0,0
    Set ax=cht.Axes(c.chAxisPositionLeft)
    ax.NumberFormat=pview.DataAxis.Totals(0).NumberFormat
    Set ax=cht.Axes(c.chAxisPositionBottom)
    ax.NumberFormat=pview.DataAxis.Totals(0).NumberFormat
    For Each ser In chart.Charts(0).SeriesCollection
        On Error Resume Next
        Set lbs=ser.DataLabelsCollection.Add()
        If chartLabel.Value="0" Then
           lbs.HasPercentage=False
           lbs.HasValue=False
        ElseIf chartLabel.Value="1" Then
           lbs.HasPercentage=True
           lbs.HasValue=False
        Else
           lbs.HasPercentage=False
           lbs.HasValue=True
        End If
    Next
End Sub
Sub ptable_QueryComplete()
    str=""
    For Each fs In pview.FilterAxis.FieldSets
        For Each fd In fs.Fields
            str=str & "[" & fd.Caption & "]"
        Next
    Next
    For Each fs In pview.RowAxis.FieldSets
        For Each fd In fs.Fields
            str=str & "[" & fd.Caption & "]"
        Next
    Next
    For Each fs In pview.ColumnAxis.FieldSets
        For Each fd In fs.Fields
            str=str & "[" & fd.Caption & "]"
        Next
    Next
    'str=str & "樞紐分析圖表"
    str=""
    heading.InnerText=str
    heading.Align="center"
    heading.Style.Color="black"
    showChart
End Sub
Sub rePivot(row,column,filter,expandrow,expandcol)
    set pview = ptable.ActiveView
    For Each fs In pview.RowAxis.FieldSets
        pview.RowAxis.RemoveFieldSet fs
    Next
    For Each fs In pview.ColumnAxis.FieldSets
        pview.ColumnAxis.RemoveFieldSet fs
    Next
    For Each fs In pview.FilterAxis.FieldSets
        pview.FilterAxis.RemoveFieldSet fs
    Next
    aryRow=Split(row,";")
    aryColumn=Split(column,";")
    aryFilter=Split(filter,";")
    aryExpandRow=Split(ExpandRow,";")
    aryExpandCol=Split(ExpandCol,";")
    On Error Resume Next
    For Each fs In aryRow
        pview.RowAxis.InsertFieldSet pview.FieldSets(fs)
        pview.FieldSets(fs).Fields(0).Expanded=False
        for i=0 to Ubound(aryExpandRow)
            If fs = aryExpandRow(i) then
               pview.FieldSets(fs).Fields(0).Expanded=True
            End if
        next
    Next
    For Each fs In aryColumn
        pview.ColumnAxis.InsertFieldSet pview.FieldSets(fs)
        pview.FieldSets(fs).Fields(0).Expanded=False
        for i=0 to Ubound(aryExpandCol)
            If fs = aryExpandCol(i) then
               pview.FieldSets(fs).Fields(0).Expanded=True
            End if
        next
    Next
    For Each fs In aryFilter
        pview.FilterAxis.InsertFieldSet pview.FieldSets(fs)
    Next
End Sub
</script>

<%
End Sub
%>