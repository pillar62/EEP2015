
Partial Class Template_VBWebCMasterDetail5
    Inherits System.Web.UI.Page

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebCMasterDetail5))
        Me.WMaster = New Srvtools.WebDataSet
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'WMaster
        '
        Me.WMaster.Active = False
        Me.WMaster.AlwaysClose = False
        Me.WMaster.DeleteIncomplete = True
        Me.WMaster.Guid = "3cfb9c47-87cb-4493-ae85-f4b6932e3f30"
        Me.WMaster.LastKeyValues = Nothing
        Me.WMaster.Locale = New System.Globalization.CultureInfo("zh-TW")
        Me.WMaster.PacketRecords = 100
        Me.WMaster.Position = -1
        Me.WMaster.RefCommandText = Nothing
        Me.WMaster.RefDBAlias = Nothing
        Me.WMaster.RemoteName = Nothing
        Me.WMaster.ServerModify = False
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents WMaster As Srvtools.WebDataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitializeComponent()
            Master.DataSource = WMaster
            Detail1.DataSource = WMaster
            Detail2.DataSource = WMaster
        End If
    End Sub

    Protected Sub WebNavigator1_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles WebNavigator1.Command
        If e.CommandName = "cmdFirst" OrElse e.CommandName = "cmdPrevious" OrElse e.CommandName = "cmdNext" OrElse e.CommandName = "cmdLast" Then
            Detail1.ExecuteSelect(wfvMaster)
            Detail2.ExecuteSelect(wfvMaster)
            DataBind()
        ElseIf e.CommandName = "cmdAdd" Then
            Detail1.ExecuteAdd(wfvMaster)
            Detail2.ExecuteAdd(wfvMaster)
            DataBind()
        ElseIf (e.CommandName = "cmdDelete" OrElse e.CommandName = "cmdApply" OrElse e.CommandName = "cmdOK") AndAlso TryCast(sender, Srvtools.WebNavigator).State = Srvtools.WebNavigator.NavigatorState.ApplySucess Then
            Detail1.ExecuteSelect(wfvMaster)
            Detail2.ExecuteSelect(wfvMaster)
            DataBind()
        End If
    End Sub

    Protected Sub wfvMaster_AfterInsertLocate(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.AfterInsertLocate
        Detail1.ExecuteSelect(wfvMaster)
        Detail2.ExecuteSelect(wfvMaster)
        DataBind()
    End Sub

    Protected Sub wfvMaster_Canceled(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.Canceled
        Detail1.ExecuteSelect(wfvMaster)
        Detail2.ExecuteSelect(wfvMaster)
        DataBind()
    End Sub
End Class
