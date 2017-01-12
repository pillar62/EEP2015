Public Class TAG_FORMNAME
    Inherits Srvtools.InfoForm
    Private Sub TAG_FORMNAME_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        clientQuery1.Show(Me.panel1)
        AddHandler Me.reportViewer1.LocalReport.SubreportProcessing, AddressOf LocalReport_SubreportProcessing
        Me.reportViewer1.LocalReport.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("NewDataSet_", Me.DataSet.RealDataSet.Tables(0)))
    End Sub

    Private Sub LocalReport_SubreportProcessing(ByVal sender As Object, ByVal e As Microsoft.Reporting.WinForms.SubreportProcessingEventArgs)
        e.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource("NewDataSet_", Detail))
    End Sub

    Private Sub btClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btClear.Click
        clientQuery1.Clear(Me.panel1)
    End Sub

    Private Sub btQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btQuery.Click
        clientQuery1.Execute(Me.panel1)
        Me.reportViewer1.RefreshReport()
    End Sub
End Class
