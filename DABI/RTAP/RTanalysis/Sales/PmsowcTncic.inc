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
<select size=1 id="chartType" style="width:200;font-family:�ө���" onchange="showChart()">
 <option value="0">�s�ժ�����</option>
 <option value="1">���|������</option>
 <option value="2">100%���|������</option>
 <option value="3">�s�վ����</option>
 <option value="4">���|�����</option>
 <option value="5">100%���|�����</option>
 <option value="6">��u��</option>
 <option value="7">��u��(���аO)</option>
 <option value="8">���|��u��</option>
 <option value="9">���|��u��(���аO)</option>
 <option value="10">���|��u��(�ʤ������)</option>
 <option value="11">���|��u��(���аO�ʤ������)</option>
 <option value="12">���Ʀ��u��</option>
 <option value="13">���Ʀ��u��(���аO)</option>
 <option value="14">���|���Ʀ��u��</option>
 <option value="15">���|���Ʀ��u��(���аO)</option>
 <option value="16">���|���Ʀ��u��(�ʤ������)</option>
 <option value="17">���|���Ʀ��u��(���аO�ʤ������)</option>
 <option value="18">��ι�</option>
 <option value="19">������ι�</option>
 <option value="20">���|��ι�</option>
</select>
</div>
<div style="position:absolute;left:500;top:20;display:none">
<select size=1 id="chartLabel" style="font-family:�ө���" onchange="showChart()">
 <option value="0">����ܼƭȼ���</option>
 <option value="1">���%�ƭȼ���</option>
 <option value="2">��ܼƭȼ���</option>
</select>
</div>
<div style="position:absolute;left:650;top:20;display:none">
<input type="button" id="refresh" value="���s��ܹϧ�" onClick="showChart()" style="font-family:�ө���;cursor:hand">
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
<h5 id="heading" align="left" style="color:red">��ƤU�����еy������....�A
�z�Ҭd�ߪ����G�N�H�Ϫ��覡�i�{�C
�z�i�����M��N��Ʋ�����椧[����]�B[�C]�B[��]��[���]�椺�A�Y�i�e�{�z��
�ݭn���d�ߵ��G�C</h5>
<h6 align="left" style="font-family:�ө���;display:none"><%=unit%></h6>
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

<p align=left><font size=2>�`�e�u�ơJ�D�u�w�Vsparq�ӽФ��ƶq(���t�w�M�u)<br>
�`�ӽФ�J�w�Vsparq�ӽХD�u�����Ϩ����`�p(���t�w�h���κM�P)<br>
�`�}�q�ơJ�D�u�w���q���ƶq�A�i����Τ�˾��@�~(���t�w�M�u)<br>
�`���u��J�Τ�w�˾����������(���t�w�h���κM�P)<br>
�`������J�Τ�w���u�B�w������sparq�����(���t�w�h���κM�P)</font></p>
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
    'str=str & "�ϯä��R�Ϫ�"
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