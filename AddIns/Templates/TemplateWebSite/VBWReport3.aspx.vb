
Partial Class Template_VBWReport3
    Inherits System.Web.UI.Page

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWReport3))
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
            CrystalReportViewer1.DisplayPage = False
        Else
            CrystalReportViewer1.DisplayPage = True
        End If
        WebClientQuery1.Show(Panel1)
    End Sub

    Protected Sub Button1_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        WebClientQuery1.Execute(Panel1, True)
        CrystalReportViewer1.RefreshReport()
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        WebClientQuery1.Clear(Panel1)
        CrystalReportViewer1.DisplayPage = False
    End Sub
End Class
