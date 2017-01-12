Public Class Form1
    Inherits Srvtools.InfoForm
    Public Sub New()
        InitializeComponent()
    End Sub 'New
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Master = New Srvtools.InfoDataSet(Me.components)
        Me.ibsMaster = New Srvtools.InfoBindingSource(Me.components)
        Me.clientQuery1 = New Srvtools.ClientQuery(Me.components)
        Me.panel1 = New System.Windows.Forms.Panel
        Me.button1 = New System.Windows.Forms.Button
        Me.button2 = New System.Windows.Forms.Button
        Me.CrystalReportViewer1 = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        CType(Me.Master, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ibsMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Master
        '
        Me.Master.Active = False
        Me.Master.AlwaysClose = False
        Me.Master.DeleteIncomplete = True
        Me.Master.LastKeyValues = Nothing
        Me.Master.PacketRecords = -1
        Me.Master.Position = -1
        Me.Master.RemoteName = ""
        Me.Master.ServerModify = False
        '
        'ibsMaster
        '
        Me.ibsMaster.AllowAdd = True
        Me.ibsMaster.AllowDelete = True
        Me.ibsMaster.AllowPrint = True
        Me.ibsMaster.AllowUpdate = True
        Me.ibsMaster.AutoApply = False
        Me.ibsMaster.AutoApplyMaster = False
        Me.ibsMaster.AutoDisibleControl = False
        Me.ibsMaster.AutoRecordLock = False
        Me.ibsMaster.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload
        Me.ibsMaster.CloseProtect = False
        Me.ibsMaster.DataSource = Me.Master
        Me.ibsMaster.DelayInterval = 300
        Me.ibsMaster.DisableKeyFields = False
        Me.ibsMaster.EnableFlag = False
        Me.ibsMaster.FocusedControl = Nothing
        Me.ibsMaster.OwnerComp = Nothing
        Me.ibsMaster.Position = 0
        Me.ibsMaster.RelationDelay = False
        Me.ibsMaster.text = "ibsMaster"
        '
        'clientQuery1
        '
        Me.clientQuery1.BindingSource = Me.ibsMaster
        Me.clientQuery1.Caption = ""
        Me.clientQuery1.Font = New System.Drawing.Font("SimSun", 9.0!)
        Me.clientQuery1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.clientQuery1.GapHorizontal = 80
        Me.clientQuery1.GapVertical = 20
        Me.clientQuery1.isShow = CType(resources.GetObject("clientQuery1.isShow"), System.Collections.Generic.List(Of String))
        Me.clientQuery1.isShowInsp = False
        Me.clientQuery1.KeepCondition = False
        Me.clientQuery1.Margin = New System.Drawing.Printing.Margins(100, 30, 30, 30)
        Me.clientQuery1.TextColor = System.Drawing.SystemColors.ControlText
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.button1)
        Me.panel1.Controls.Add(Me.button2)
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.panel1.Location = New System.Drawing.Point(0, 0)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(634, 79)
        Me.panel1.TabIndex = 2
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(474, 12)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(75, 23)
        Me.button1.TabIndex = 0
        Me.button1.Text = "Query"
        Me.button1.UseVisualStyleBackColor = True
        '
        'button2
        '
        Me.button2.Location = New System.Drawing.Point(474, 41)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(75, 23)
        Me.button2.TabIndex = 1
        Me.button2.Text = "Clear"
        Me.button2.UseVisualStyleBackColor = True
        '
        'CrystalReportViewer1
        '
        Me.CrystalReportViewer1.ActiveViewIndex = -1
        Me.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CrystalReportViewer1.DisplayGroupTree = False
        Me.CrystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CrystalReportViewer1.Location = New System.Drawing.Point(0, 79)
        Me.CrystalReportViewer1.Name = "CrystalReportViewer1"
        Me.CrystalReportViewer1.SelectionFormula = ""
        Me.CrystalReportViewer1.Size = New System.Drawing.Size(634, 297)
        Me.CrystalReportViewer1.TabIndex = 3
        Me.CrystalReportViewer1.ViewTimeSelectionFormula = ""
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.ClientSize = New System.Drawing.Size(634, 376)
        Me.Controls.Add(Me.CrystalReportViewer1)
        Me.Controls.Add(Me.panel1)
        Me.Name = "Form1"
        CType(Me.Master, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ibsMaster, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents Master As Srvtools.InfoDataSet
    Private WithEvents ibsMaster As Srvtools.InfoBindingSource
    Private WithEvents clientQuery1 As Srvtools.ClientQuery
    Private WithEvents panel1 As System.Windows.Forms.Panel
    Private WithEvents button1 As System.Windows.Forms.Button
    Private WithEvents button2 As System.Windows.Forms.Button
    Private WithEvents CrystalReportViewer1 As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Private components As System.ComponentModel.IContainer


    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
        clientQuery1.Execute(panel1)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        clientQuery1.Show(panel1)
    End Sub

    Private Sub button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button2.Click
        clientQuery1.Clear(panel1)
        CrystalReportViewer1.ReportSource = Nothing
    End Sub
End Class
