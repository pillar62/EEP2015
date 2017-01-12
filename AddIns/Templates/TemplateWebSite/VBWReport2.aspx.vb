Partial Class VBWReport2
    Inherits System.Web.UI.Page

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VBWReport2))
        Me.WData = New Srvtools.WebDataSet
        CType(Me.WData, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'WData
        '
        Me.WData.Active = False
        Me.WData.AlwaysClose = True
        Me.WData.DeleteIncomplete = True
        Me.WData.Guid = Nothing
        Me.WData.LastKeyValues = Nothing
        Me.WData.PacketRecords = -1
        Me.WData.Position = -1
        Me.WData.RemoteName = ""
        Me.WData.ServerModify = False
        CType(Me.WData, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents WData As Srvtools.WebDataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            InitializeComponent()
            WebDataSource1.DataSource = WData
            WebDateTimePicker1.Text = Srvtools.CliUtils.GetValue("_FIRSTDAYTY")(1).ToString()
            WebDateTimePicker2.Text = Srvtools.CliUtils.GetValue("_LASTDAY")(1).ToString()
        Else
            CrystalReportViewer1.DisplayPage = True
        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim swhere As String = "1=0"    ''set condiction here
        WebDataSource1.SetWhere(swhere)
        CrystalReportViewer1.RefreshReport()    '' reload page
    End Sub
End Class
