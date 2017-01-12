<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TAG_FORMNAME
    Inherits Srvtools.InfoForm

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer
        Me.panel1 = New System.Windows.Forms.Panel
        Me.btClear = New System.Windows.Forms.Button
        Me.btQuery = New System.Windows.Forms.Button
        Me.reportViewer1 = New Microsoft.Reporting.WinForms.ReportViewer
        Me.infoStatusStrip1 = New Srvtools.InfoStatusStrip(Me.components)
        Me.clientQuery1 = New Srvtools.ClientQuery(Me.components)
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.panel1.SuspendLayout()
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
        Me.splitContainer1.Panel1.Controls.Add(Me.panel1)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.reportViewer1)
        Me.splitContainer1.Size = New System.Drawing.Size(593, 380)
        Me.splitContainer1.SplitterDistance = 129
        Me.splitContainer1.TabIndex = 5
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.btClear)
        Me.panel1.Controls.Add(Me.btQuery)
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panel1.Location = New System.Drawing.Point(0, 0)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(593, 129)
        Me.panel1.TabIndex = 0
        '
        'btClear
        '
        Me.btClear.Location = New System.Drawing.Point(479, 64)
        Me.btClear.Name = "btClear"
        Me.btClear.Size = New System.Drawing.Size(75, 21)
        Me.btClear.TabIndex = 1
        Me.btClear.Text = "Clear"
        Me.btClear.UseVisualStyleBackColor = True
        '
        'btQuery
        '
        Me.btQuery.Location = New System.Drawing.Point(479, 23)
        Me.btQuery.Name = "btQuery"
        Me.btQuery.Size = New System.Drawing.Size(75, 21)
        Me.btQuery.TabIndex = 0
        Me.btQuery.Text = "Query"
        Me.btQuery.UseVisualStyleBackColor = True
        '
        'reportViewer1
        '
        Me.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.reportViewer1.Location = New System.Drawing.Point(0, 0)
        Me.reportViewer1.Name = "reportViewer1"
        Me.reportViewer1.Size = New System.Drawing.Size(593, 247)
        Me.reportViewer1.TabIndex = 0
        '
        'infoStatusStrip1
        '
        Me.infoStatusStrip1.Location = New System.Drawing.Point(0, 380)
        Me.infoStatusStrip1.Name = "infoStatusStrip1"
        Me.infoStatusStrip1.ShowCompany = False
        Me.infoStatusStrip1.ShowDate = True
        Me.infoStatusStrip1.ShowEEPAlias = True
        Me.infoStatusStrip1.ShowNavigatorStatus = True
        Me.infoStatusStrip1.ShowProgress = False
        Me.infoStatusStrip1.ShowSolution = True
        Me.infoStatusStrip1.ShowUserID = True
        Me.infoStatusStrip1.ShowUserName = True
        Me.infoStatusStrip1.Size = New System.Drawing.Size(593, 26)
        Me.infoStatusStrip1.TabIndex = 4
        Me.infoStatusStrip1.Text = "infoStatusStrip1"
        '
        'clientQuery1
        '
        Me.clientQuery1.BindingSource = Nothing
        Me.clientQuery1.Caption = ""
        Me.clientQuery1.Font = New System.Drawing.Font("宋体", 9.0!)
        Me.clientQuery1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.clientQuery1.GapHorizontal = 80
        Me.clientQuery1.GapVertical = 20
        Me.clientQuery1.isShow = Nothing
        Me.clientQuery1.isShowInsp = False
        Me.clientQuery1.KeepCondition = False
        Me.clientQuery1.Margin = New System.Drawing.Printing.Margins(100, 30, 30, 30)
        Me.clientQuery1.TextColor = System.Drawing.SystemColors.ControlText
        '
        'TAG_FORMNAME
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(593, 406)
        Me.Controls.Add(Me.splitContainer1)
        Me.Controls.Add(Me.infoStatusStrip1)
        Me.Name = "TAG_FORMNAME"
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.ResumeLayout(False)
        Me.panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents splitContainer1 As System.Windows.Forms.SplitContainer
    Private WithEvents panel1 As System.Windows.Forms.Panel
    Private WithEvents btClear As System.Windows.Forms.Button
    Private WithEvents btQuery As System.Windows.Forms.Button
    Private WithEvents reportViewer1 As Microsoft.Reporting.WinForms.ReportViewer
    Private WithEvents infoStatusStrip1 As Srvtools.InfoStatusStrip
    Private WithEvents clientQuery1 As Srvtools.ClientQuery

End Class
