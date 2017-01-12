
Partial Class Template_VBWebSingle2
    Inherits System.Web.UI.Page


    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebSingle2))
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

    Protected Sub WebGridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles WebGridView1.RowCommand
        If e.CommandName = "Select" Then

            WebFormView1.ExecuteSync(e)
        End If

    End Sub

    Protected Sub WebNavigator1_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles WebNavigator1.Command
        If e.CommandName = "cmdAdd" OrElse e.CommandName = "cmdUpdate" Then

            MultiView1.ActiveViewIndex = 1
            WebMultiViewCaptions1.ActiveIndex = 1
        End If

        WebFormView1.ExecuteSync(New GridViewCommandEventArgs(WebGridView1.SelectedRow, New CommandEventArgs("Select", DBNull.Value)))
    End Sub
End Class
