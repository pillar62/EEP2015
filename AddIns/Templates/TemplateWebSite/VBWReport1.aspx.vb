
Partial Class Template_VBWReport1
    Inherits System.Web.UI.Page

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWReport1))
        Me.WData = New Srvtools.WebDataSet
        CType(Me.WData, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'WData
        '
        Me.WData.Active = False
        Me.WData.AlwaysClose = False
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
        End If
        CrystalReportViewer1.DisplayPage = True
    End Sub
End Class
