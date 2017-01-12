
Partial Class Template_VBWebCMasterDetail_VFG
    Inherits System.Web.UI.Page

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebCMasterDetail_VFG))
        Me.WMaster = New Srvtools.WebDataSet
        Me.WView = New Srvtools.WebDataSet
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WView, System.ComponentModel.ISupportInitialize).BeginInit()
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
        '
        'WView
        '
        Me.WView.Active = False
        Me.WView.AlwaysClose = False
        Me.WView.DeleteIncomplete = True
        Me.WView.Guid = "2c363173-ea0b-47df-8082-340176b68302"
        Me.WView.LastKeyValues = Nothing
        Me.WView.PacketRecords = 100
        Me.WView.Position = -1
        Me.WView.RemoteName = Nothing
        Me.WView.ServerModify = False
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WView, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents WView As Srvtools.WebDataSet
    Private WithEvents WMaster As Srvtools.WebDataSet

    Protected Sub wfvMaster_AfterInsertLocate(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.AfterInsertLocate
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
    End Sub

    Protected Sub wfvMaster_Canceled(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.Canceled
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
    End Sub

    Protected Sub WebNavigator1_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles WebNavigator1.Command
        If e.CommandName = "cmdFirst" Or e.CommandName = "cmdPrevious" Or e.CommandName = "cmdNext" Or e.CommandName = "cmdLast" Then
            Master.ExecuteSync(WgView)
            DataBind()
            Detail.ExecuteSelect(wfvMaster)
            DataBind()
        ElseIf e.CommandName = "cmdAdd" Then
            Detail.ExecuteAdd(wfvMaster)
            DataBind()
        ElseIf (e.CommandName = "cmdDelete" Or e.CommandName = "cmdApply" Or e.CommandName = "cmdOK") And (CType(sender, Srvtools.WebNavigator).State = Srvtools.WebNavigator.NavigatorState.ApplySucess) Then
            View.Reload()
            DataBind()
            If e.CommandName = "cmdDelete" Then
                Master.ExecuteSync(WgView)
                DataBind()
                Detail.ExecuteSelect(wfvMaster)
                DataBind()
            End If
        End If
    End Sub

    Protected Sub WgView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles WgView.SelectedIndexChanged
        Master.ExecuteSync(WgView)
        DataBind()
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack <> True Then
            InitializeComponent()
            Master.DataSource = WMaster
            Detail.DataSource = WMaster
            View.DataSource = WView
        End If
    End Sub
End Class
