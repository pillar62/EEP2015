Public Class Form1
    Inherits Srvtools.InfoForm
    Public Sub New()
        InitializeComponent()
    End Sub 'New
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer
        Me.button2 = New System.Windows.Forms.Button
        Me.button1 = New System.Windows.Forms.Button
        Me.clientQuery1 = New Srvtools.ClientQuery(Me.components)
        Me.ibsQuery = New Srvtools.InfoBindingSource(Me.components)
        Me.Query = New Srvtools.InfoDataSet(Me.components)
        Me.InfoStatusStrip1 = New Srvtools.InfoStatusStrip(Me.components)
        Me.infoDataGridView1 = New Srvtools.InfoDataGridView
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        CType(Me.ibsQuery, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Query, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.infoDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'splitContainer1
        '
        Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.splitContainer1.Name = "splitContainer1"
        Me.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.Controls.Add(Me.button2)
        Me.splitContainer1.Panel1.Controls.Add(Me.button1)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.infoDataGridView1)
        Me.splitContainer1.Size = New System.Drawing.Size(634, 376)
        Me.splitContainer1.SplitterDistance = 148
        Me.splitContainer1.TabIndex = 5
        '
        'button2
        '
        Me.button2.Image = CType(resources.GetObject("button2.Image"), System.Drawing.Image)
        Me.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.button2.Location = New System.Drawing.Point(530, 73)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(88, 30)
        Me.button2.TabIndex = 1
        Me.button2.Text = "Clear"
        Me.button2.UseVisualStyleBackColor = True
        '
        'button1
        '
        Me.button1.Image = CType(resources.GetObject("button1.Image"), System.Drawing.Image)
        Me.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.button1.Location = New System.Drawing.Point(530, 35)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(88, 32)
        Me.button1.TabIndex = 0
        Me.button1.Text = "Query"
        Me.button1.UseVisualStyleBackColor = True
        '
        'clientQuery1
        '
        Me.clientQuery1.BindingSource = Me.ibsQuery
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
        'ibsQuery
        '
        Me.ibsQuery.AllowAdd = True
        Me.ibsQuery.AllowDelete = True
        Me.ibsQuery.AllowPrint = True
        Me.ibsQuery.AllowUpdate = True
        Me.ibsQuery.AutoApply = False
        Me.ibsQuery.AutoApplyMaster = False
        Me.ibsQuery.AutoDisibleControl = False
        Me.ibsQuery.AutoRecordLock = False
        Me.ibsQuery.AutoRecordLockMode = Srvtools.InfoBindingSource.LockMode.NoneReload
        Me.ibsQuery.CloseProtect = False
        Me.ibsQuery.DataSource = Me.Query
        Me.ibsQuery.DelayInterval = 300
        Me.ibsQuery.DisableKeyFields = False
        Me.ibsQuery.EnableFlag = False
        Me.ibsQuery.FocusedControl = Nothing
        Me.ibsQuery.OwnerComp = Nothing
        Me.ibsQuery.Position = 0
        Me.ibsQuery.RelationDelay = False
        Me.ibsQuery.text = "ibsQuery"
        '
        'Query
        '
        Me.Query.Active = False
        Me.Query.AlwaysClose = False
        Me.Query.DeleteIncomplete = True
        Me.Query.LastKeyValues = Nothing
        Me.Query.PacketRecords = 100
        Me.Query.Position = -1
        Me.Query.RemoteName = ""
        Me.Query.ServerModify = False
        '
        'InfoStatusStrip1
        '
        Me.InfoStatusStrip1.Location = New System.Drawing.Point(0, 354)
        Me.InfoStatusStrip1.Name = "InfoStatusStrip1"
        Me.InfoStatusStrip1.ShowCompany = False
        Me.InfoStatusStrip1.ShowDate = True
        Me.InfoStatusStrip1.ShowEEPAlias = True
        Me.InfoStatusStrip1.ShowNavigatorStatus = False
        Me.InfoStatusStrip1.ShowSolution = True
        Me.InfoStatusStrip1.ShowUserID = True
        Me.InfoStatusStrip1.ShowUserName = True
        Me.InfoStatusStrip1.Size = New System.Drawing.Size(634, 22)
        Me.InfoStatusStrip1.TabIndex = 6
        Me.InfoStatusStrip1.Text = "InfoStatusStrip1"
        '
        'infoDataGridView1
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.infoDataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle2
        Me.infoDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.infoDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.infoDataGridView1.EnterEnable = True
        Me.infoDataGridView1.EnterRefValControl = False
        Me.infoDataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.infoDataGridView1.Name = "infoDataGridView1"
        Me.infoDataGridView1.RowHeadersWidth = 25
        Me.infoDataGridView1.RowTemplate.Height = 24
        Me.infoDataGridView1.Size = New System.Drawing.Size(634, 224)
        Me.infoDataGridView1.SureDelete = False
        Me.infoDataGridView1.TabIndex = 1
        Me.infoDataGridView1.TotalActive = True
        Me.infoDataGridView1.TotalBackColor = System.Drawing.SystemColors.Info
        Me.infoDataGridView1.TotalCaption = Nothing
        Me.infoDataGridView1.TotalCaptionFont = New System.Drawing.Font("SimSun", 9.0!)
        Me.infoDataGridView1.TotalFont = New System.Drawing.Font("SimSun", 9.0!)
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.ClientSize = New System.Drawing.Size(634, 376)
        Me.Controls.Add(Me.InfoStatusStrip1)
        Me.Controls.Add(Me.splitContainer1)
        Me.Name = "Form1"
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.ResumeLayout(False)
        CType(Me.ibsQuery, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Query, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.infoDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents splitContainer1 As System.Windows.Forms.SplitContainer
    Private WithEvents button2 As System.Windows.Forms.Button
    Private WithEvents button1 As System.Windows.Forms.Button
    Private WithEvents infoDataGridView1 As Srvtools.InfoDataGridView
    Private WithEvents clientQuery1 As Srvtools.ClientQuery
    Private WithEvents ibsQuery As Srvtools.InfoBindingSource
    Private WithEvents Query As Srvtools.InfoDataSet
    Friend WithEvents InfoStatusStrip1 As Srvtools.InfoStatusStrip
    Private components As System.ComponentModel.IContainer

    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
        clientQuery1.Execute(splitContainer1.Panel1)
    End Sub

    Private Sub button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button2.Click
        clientQuery1.Clear(splitContainer1.Panel1)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        clientQuery1.Show(splitContainer1.Panel1)
    End Sub
End Class
