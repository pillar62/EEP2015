Public Class Form1
    Inherits Srvtools.InfoForm
    Public Sub New()
        InitializeComponent()
    End Sub 'New
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Master = New Srvtools.InfoDataSet(Me.components)
        Me.crystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.button1 = New System.Windows.Forms.Button
        CType(Me.Master, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Master
        '
        Me.Master.Active = False
        Me.Master.AlwaysClose = False
        Me.Master.DeleteIncomplete = True
        Me.Master.LastKeyValues = Nothing
        Me.Master.PacketRecords = 100
        Me.Master.Position = -1
        Me.Master.RemoteName = Nothing
        Me.Master.ServerModify = False
        '
        'crystalReportViewer1
        '
        Me.crystalReportViewer1.ActiveViewIndex = -1
        Me.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crystalReportViewer1.DisplayGroupTree = False
        Me.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.crystalReportViewer1.Location = New System.Drawing.Point(0, 77)
        Me.crystalReportViewer1.Name = "crystalReportViewer1"
        Me.crystalReportViewer1.SelectionFormula = ""
        Me.crystalReportViewer1.Size = New System.Drawing.Size(634, 299)
        Me.crystalReportViewer1.TabIndex = 2
        Me.crystalReportViewer1.ViewTimeSelectionFormula = ""
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(501, 27)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(83, 23)
        Me.button1.TabIndex = 3
        Me.button1.Text = "Print"
        Me.button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.ClientSize = New System.Drawing.Size(634, 376)
        Me.Controls.Add(Me.button1)
        Me.Controls.Add(Me.crystalReportViewer1)
        Me.Name = "Form1"
        CType(Me.Master, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private components As System.ComponentModel.IContainer
    Private WithEvents crystalReportViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Private WithEvents button1 As System.Windows.Forms.Button
    Friend WithEvents Master As Srvtools.InfoDataSet

    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
        Dim Wstr As String = "1=0"
        Master.SetWhere(Wstr)
    End Sub
End Class
