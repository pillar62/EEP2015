
Partial Class Template_VBWebQuery
    Inherits System.Web.UI.Page

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebQuery))
        Me.WMaster = New Srvtools.WebDataSet
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'WMaster
        '
        Me.WMaster.Active = False
        Me.WMaster.AlwaysClose = True
        Me.WMaster.DeleteIncomplete = True
        Me.WMaster.Guid = Nothing
        Me.WMaster.LastKeyValues = Nothing
        Me.WMaster.PacketRecords = 100
        Me.WMaster.Position = -1
        Me.WMaster.RemoteName = Nothing
        Me.WMaster.ServerModify = False
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents WMaster As Srvtools.WebDataSet

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        WebClientQuery1.Execute(Panel1)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        WebClientQuery1.Clear(Panel1)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            InitializeComponent()
            Master.DataSource = WMaster
        End If
        WebClientQuery1.Show(Panel1)
    End Sub
End Class
