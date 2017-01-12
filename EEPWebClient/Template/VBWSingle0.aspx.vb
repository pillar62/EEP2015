
Partial Class Template_VBWSingle0
    Inherits System.Web.UI.Page

    Dim WMaster As Srvtools.WebDataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not IsPostBack Then
            InitializeComponent()
            Master.DataSource = WMaster
        End If
    End Sub

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWSingle0))
        Me.WMaster = New Srvtools.WebDataSet()
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'WMaster
        '
        Me.WMaster.Active = False
        Me.WMaster.AlwaysClose = False
        Me.WMaster.DataCompressed = False
        Me.WMaster.DeleteIncomplete = True
        Me.WMaster.Guid = Nothing
        Me.WMaster.LastKeyValues = Nothing
        Me.WMaster.Locale = New System.Globalization.CultureInfo("zh-TW")
        Me.WMaster.PacketRecords = 100
        Me.WMaster.Position = -1
        Me.WMaster.RemoteName = Nothing
        Me.WMaster.ServerModify = False
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

End Class
