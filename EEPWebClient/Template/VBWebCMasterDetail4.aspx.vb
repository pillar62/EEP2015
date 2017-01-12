
Partial Class Template_VBWebCMasterDetail4
    Inherits System.Web.UI.Page

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebCMasterDetail4))
        Me.WMaster = New Srvtools.WebDataSet
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'WMaster
        '
        Me.WMaster.Active = False
        Me.WMaster.AlwaysClose = False
        Me.WMaster.DeleteIncomplete = True
        Me.WMaster.Guid = Nothing
        Me.WMaster.LastKeyValues = Nothing
        Me.WMaster.Locale = New System.Globalization.CultureInfo("zh-CN")
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
            Detail.DataSource = WMaster
        End If
    End Sub

    Protected Sub WebNavigator1_BeforeCommand(ByVal sender As Object, ByVal e As Srvtools.BeforeCommandArgs) Handles WebNavigator1.BeforeCommand
        If e.CommandName = "cmdApply" OrElse e.CommandName = "cmdOK" Then
            SaveDetail(True)
        End If
    End Sub

    Public Sub SaveDetail(ByVal ifSave As Boolean)
        If Not ifSave Then
            wfvDetail.ChangeMode(FormViewMode.[ReadOnly])
            If Master.InnerDataSet.GetChanges() Is Nothing Then
                WebNavigator1.SetNavState("Browsed")
            End If
        Else
            If wfvDetail.CurrentMode = FormViewMode.Edit Then
                wfvDetail.UpdateItem(False)
                '請將您Detail的FormView的放置的DropDownList的名字放入到wdls列表中，EditTemplate內的就可以了 
                Dim wdls As String() = New String() {"DropDownList1", "DropDownList2"}
                For Each id As String In wdls
                    Dim list As DropDownList = TryCast(wfvDetail.FindControl(id), DropDownList)
                    If list IsNot Nothing Then
                        list.EnableViewState = False
                    End If
                Next
            ElseIf wfvDetail.CurrentMode = FormViewMode.Insert Then
                wfvDetail.InsertItem(True)
            End If
        End If
        WebMultiViewCaptions1.ActiveIndex = 0
    End Sub

    Protected Sub WebNavigator1_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles WebNavigator1.Command
        If e.CommandName = "cmdFirst" OrElse e.CommandName = "cmdPrevious" OrElse e.CommandName = "cmdNext" OrElse e.CommandName = "cmdLast" Then
            Detail.ExecuteSelect(wfvMaster)
            DataBind()
        ElseIf e.CommandName = "cmdAdd" Then
            Detail.ExecuteAdd(wfvMaster)
            DataBind()
        ElseIf e.CommandName = "cmdDelete" Then
            Detail.ExecuteSelect(wfvMaster)
            DataBind()
        ElseIf e.CommandName = "cmdApply" AndAlso wfvMaster.AllValidateSucess Then
            Detail.ExecuteSelect(wfvMaster)
            DataBind()
        End If
    End Sub

    Protected Sub wfvMaster_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.PageIndexChanged
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
    End Sub

    Protected Sub wfvMaster_AfterInsertLocate(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.AfterInsertLocate
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
    End Sub

    Protected Sub wfvMaster_Canceled(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.Canceled
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
    End Sub

    Protected Sub wgvDetail_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles wgvDetail.RowEditing
        wfvDetail.ExecuteSync(New GridViewCommandEventArgs(wgvDetail.Rows(e.NewEditIndex), New CommandEventArgs("Edit", Nothing)))
        WebMultiViewCaptions1.ActiveIndex = 1
        wfvDetail.ChangeMode(FormViewMode.Edit)
        e.Cancel = True
    End Sub

    Protected Sub ImageButton10_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton10.Click
        SaveDetail(True)
        wgvDetail.DataBind()
    End Sub

    Protected Sub ImageButton11_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton11.Click
        SaveDetail(False)
    End Sub

    Protected Sub ImageButton1_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim stat As String = WebNavigator1.CurrentNavState
        If stat <> "Editing" AndAlso stat <> "Inserting" Then
            WebNavigator1.SetState(Srvtools.WebNavigator.NavigatorState.Editing)
            WebNavigator1.SetNavState("Editing")
        End If
        wfvDetail.ChangeMode(FormViewMode.Insert)
        WebMultiViewCaptions1.ActiveIndex = 1
    End Sub
End Class
