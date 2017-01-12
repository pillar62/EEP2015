
Partial Class Template_VBWebSingle3
    Inherits System.Web.UI.Page


    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebSingle3))
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
        Me.WMaster.Locale = New System.Globalization.CultureInfo("zh-CN")
        Me.WMaster.PacketRecords = 100
        Me.WMaster.Position = -1
        Me.WMaster.RemoteName = Nothing
        Me.WMaster.ServerModify = False
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents WMaster As Srvtools.WebDataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            InitializeComponent()

            Master.DataSource = WMaster
        End If

    End Sub

    Protected Sub ButtonOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        AjaxModalPanel1.Submit()
    End Sub

    Protected Sub buttonClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonClose.Click
        AjaxModalPanel1.Close()
    End Sub
End Class
