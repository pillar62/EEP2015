Public Class Component
    Inherits Srvtools.DataModule

    Friend WithEvents ucMaster As Srvtools.UpdateComponent
    Friend WithEvents ServiceManager As Srvtools.ServiceManager
    Private components As System.ComponentModel.IContainer
    Friend WithEvents Master As Srvtools.InfoCommand
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        container.Add(Me)
        InitializeComponent()
    End Sub 'New
    Public Sub New()
        InitializeComponent()
    End Sub 'New
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.InfoConnection = New Srvtools.InfoConnection
        Me.Master = New Srvtools.InfoCommand(Me.components)
        Me.ServiceManager = New Srvtools.ServiceManager(Me.components)
        Me.ucMaster = New Srvtools.UpdateComponent(Me.components)
        CType(Me.InfoConnection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Master, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'InfoConnection
        '
        Me.InfoConnection.ConnectionString = Nothing
        Me.InfoConnection.ConnectionType = Srvtools.ConnectionType.SqlClient
        '
        'Master
        '
        Me.Master.CommandText = ""
        Me.Master.CommandTimeout = 30
        Me.Master.CommandType = System.Data.CommandType.Text
        Me.Master.Connection = Nothing
        Me.Master.EEPAlias = Nothing
        Me.Master.InfoConnection = Me.InfoConnection
        Me.Master.Name = "Master"
        Me.Master.NotificationAutoEnlist = False
        Me.Master.SecExcept = Nothing
        Me.Master.SecFieldName = Nothing
        Me.Master.SecStyle = Srvtools.SecurityStyle.ssByNone
        Me.Master.SelectTop = 0
        Me.Master.SiteControl = False
        Me.Master.SiteFieldName = Nothing
        Me.Master.Transaction = Nothing
        Me.Master.UpdatedRowSource = System.Data.UpdateRowSource.Both
        '
        'ucMaster
        '
        Me.ucMaster.AutoTrans = True
        Me.ucMaster.ExceptJoin = False
        Me.ucMaster.LogInfo = Nothing
        Me.ucMaster.Name = Nothing
        Me.ucMaster.SelectCmd = Me.Master
        Me.ucMaster.ServerModify = True
        Me.ucMaster.ServerModifyGetMax = False
        Me.ucMaster.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted
        Me.ucMaster.WhereMode = Srvtools.WhereModeType.Keyfields
        CType(Me.InfoConnection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Master, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents InfoConnection As Srvtools.InfoConnection
End Class
