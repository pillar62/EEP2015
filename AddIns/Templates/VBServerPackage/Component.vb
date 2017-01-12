Public Class Component
    Inherits Srvtools.DataModule
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        container.Add(Me)
        InitializeComponent()
    End Sub 'New
    Public Sub New()
        InitializeComponent()
    End Sub 'New
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ServiceManager = New Srvtools.ServiceManager(Me.components)

    End Sub
    Private components As System.ComponentModel.IContainer
    Friend WithEvents ServiceManager As Srvtools.ServiceManager
End Class
