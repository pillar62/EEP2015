
Partial Class Template_VBWebMasterDetailReport
    Inherits System.Web.UI.Page
   

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebMasterDetailReport))
        Me.WMaster = New Srvtools.WebDataSet()
        Me.WDetail = New Srvtools.WebDataSet()
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'WMaster
        '
        Me.WMaster.Active = False
        Me.WMaster.AlwaysClose = True
        Me.WMaster.DataCompressed = False
        Me.WMaster.DeleteIncomplete = True
        Me.WMaster.Guid = "522577e4-34b7-402c-801d-0c4256675bfc"
        Me.WMaster.LastKeyValues = Nothing
        Me.WMaster.Locale = New System.Globalization.CultureInfo("zh-CN")
        Me.WMaster.PacketRecords = 100
        Me.WMaster.Position = -1
        Me.WMaster.RemoteName = Nothing
        Me.WMaster.ServerModify = False
        '
        'WDetail
        '
        Me.WDetail.Active = False
        Me.WDetail.AlwaysClose = False
        Me.WDetail.DataCompressed = False
        Me.WDetail.DeleteIncomplete = True
        Me.WDetail.Guid = "77df1b03-e43c-45f8-a8ec-80060a1a60e2"
        Me.WDetail.LastKeyValues = Nothing
        Me.WDetail.Locale = New System.Globalization.CultureInfo("zh-CN")
        Me.WDetail.PacketRecords = 100
        Me.WDetail.Position = -1
        Me.WDetail.RemoteName = Nothing
        Me.WDetail.ServerModify = False
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WDetail, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents WDetail As Srvtools.WebDataSet
    Private WithEvents WMaster As Srvtools.WebDataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitializeComponent()
            Master.DataSource = WMaster
        End If


        WebClientQuery1.Show(Panel1)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        WebClientQuery1.Execute(Panel1)
        DataBind()
        ReportViewer1.LocalReport.Refresh()
    End Sub
     protected Sub SubreportProcessing(ByVal sender As Object,  ByVal e As Microsoft.Reporting.WebForms.SubreportProcessingEventArgs ) 
        e.DataSources.Add(New Microsoft.Reporting.WebForms.ReportDataSource("NewDataSet_", Master.InnerDataSet.Tables.IndexOf(1)))
    End Sub


    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        WebClientQuery1.Clear(Panel1)
    End Sub

End Class
