
Partial Class Template_VBWebMasterDetailD
    Inherits System.Web.UI.Page


    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Template_VBWebMasterDetailD))
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
        Me.WMaster.RemoteName = ""
        Me.WMaster.ServerModify = False
        CType(Me.WMaster, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private WithEvents WMaster As Srvtools.WebDataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            InitializeComponent()
            Master.DataSource = WMaster
            Detail.DataSource = WMaster
            DetailD.DataSource = WMaster
        End If

    End Sub

    Protected Sub WebNavigator1_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles WebNavigator1.Command
        If e.CommandName = "cmdFirst" OrElse e.CommandName = "cmdPrevious" OrElse e.CommandName = "cmdNext" OrElse e.CommandName = "cmdLast" Then
            Detail.ExecuteSelect(wfvMaster)
            DataBind()
            If Not Detail.IsEmpty AndAlso wgvDetail.SelectedIndex = -1 Then
                wgvDetail.SelectedIndex = 0
                DetailD.ExecuteSelect(wgvDetail)
                DataBind()
            End If
        ElseIf e.CommandName = "cmdAdd" Then
            Detail.ExecuteAdd(wfvMaster)
            DataBind()
        ElseIf e.CommandName = "cmdDelete" Then
            Detail.ExecuteSelect(wfvMaster)
            DataBind()
            If Not Detail.IsEmpty AndAlso wgvDetail.SelectedIndex = -1 Then
                wgvDetail.SelectedIndex = 0
                DetailD.ExecuteSelect(wgvDetail)
                DataBind()
            End If
        ElseIf e.CommandName = "cmdApply" AndAlso wfvMaster.AllValidateSucess Then
            Detail.ExecuteSelect(wfvMaster)
            DataBind()
            If Not Detail.IsEmpty AndAlso wgvDetail.SelectedIndex = -1 Then
                wgvDetail.SelectedIndex = 0
                DetailD.ExecuteSelect(wgvDetail)
                DataBind()
            End If       
        End If

    End Sub

    Protected Sub wfvMaster_AfterInsertLocate(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.AfterInsertLocate
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
        If Not Detail.IsEmpty AndAlso wgvDetail.SelectedIndex = -1 Then
            wgvDetail.SelectedIndex = 0
            DetailD.ExecuteSelect(wgvDetail)
            DataBind()
        End If

    End Sub

    Protected Sub wfvMaster_Canceled(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.Canceled
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
        If Not Detail.IsEmpty AndAlso wgvDetail.SelectedIndex = -1 Then
            wgvDetail.SelectedIndex = 0
            DetailD.ExecuteSelect(wgvDetail)
            DataBind()
        End If
      
    End Sub

    Protected Sub wfvMaster_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wfvMaster.PageIndexChanged
        Detail.ExecuteSelect(wfvMaster)
        DataBind()
        If Not Detail.IsEmpty AndAlso wgvDetail.SelectedIndex = -1 Then
            wgvDetail.SelectedIndex = 0
            DetailD.ExecuteSelect(wgvDetail)
            DataBind()
        End If

    End Sub

    Protected Sub wgvDetail_AfterInsert(ByVal sender As Object, ByVal e As System.EventArgs) Handles wgvDetail.AfterInsert
        If wgvDetail.SelectedIndex = -1 Then
            wgvDetail.SelectedIndex = 0
            DetailD.ExecuteSelect(wgvDetail)
            DataBind()
        End If

    End Sub

    Protected Sub wgvDetail_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wgvDetail.PageIndexChanged
        If Not Detail.IsEmpty AndAlso wgvDetail.SelectedIndex = -1 Then
            wgvDetail.SelectedIndex = 0
            DetailD.ExecuteSelect(wgvDetail)
            DataBind()
        End If
       
    End Sub

    Protected Sub wgvDetail_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wgvDetail.SelectedIndexChanged
        DetailD.ExecuteSelect(wgvDetail)
        DataBind()
    End Sub
End Class
