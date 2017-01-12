
Partial Class Template_VBWebSingle
    Inherits System.Web.UI.Page

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebSingle))
        Me.WMaster = New Srvtools.WebDataSet
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'WMaster
        '
        Me.WMaster.Active = False
        Me.WMaster.AlwaysClose = False
        Me.WMaster.Guid = "74dce241-9b1b-4aea-a66c-035cb8d203c1"
        Me.WMaster.LastKeyValues = Nothing
        Me.WMaster.PacketRecords = 100
        Me.WMaster.Position = -1
        Me.WMaster.RemoteName = Nothing
        Me.WMaster.ServerModify = False
        Me.WMaster.WhereStr = ""
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents WMaster As Srvtools.WebDataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack <> True Then
            InitializeComponent()
            Master.DataSource = WMaster
        End If
    End Sub
End Class
