Public Class Form1
    Inherits Srvtools.InfoForm
    Public Sub New()
        InitializeComponent()
    End Sub 'New
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim StateItem9 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem10 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem11 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem12 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem13 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem14 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem15 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem16 As Srvtools.StateItem = New Srvtools.StateItem
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.InfoNavigator1 = New Srvtools.InfoNavigator
        Me.bindingNavigatorAbortItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorAddNewItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorApplyItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorCancelItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorEditItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.bindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.bindingNavigatorOKItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.bindingNavigatorRefreshItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorQueryItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorPrintItem = New System.Windows.Forms.ToolStripButton
        Me.bindingNavigatorSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.bindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
        Me.bindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
        Me.InfoStatusStrip1 = New Srvtools.InfoStatusStrip(Me.components)
        Me.scMaster = New System.Windows.Forms.SplitContainer
        CType(Me.InfoNavigator1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InfoNavigator1.SuspendLayout()
        Me.scMaster.SuspendLayout()
        Me.SuspendLayout()
        '
        'InfoNavigator1
        '
        Me.InfoNavigator1.AbortItem = Me.bindingNavigatorAbortItem
        Me.InfoNavigator1.AddNewItem = Me.bindingNavigatorAddNewItem
        Me.InfoNavigator1.ApplyItem = Me.bindingNavigatorApplyItem
        Me.InfoNavigator1.CancelItem = Me.bindingNavigatorCancelItem
        Me.InfoNavigator1.CountItem = Nothing
        Me.InfoNavigator1.DeleteItem = Me.bindingNavigatorDeleteItem
        Me.InfoNavigator1.DescriptionItem = Nothing
        Me.InfoNavigator1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
        Me.InfoNavigator1.EditItem = Me.bindingNavigatorEditItem
        Me.InfoNavigator1.ExportItem = Nothing
        Me.InfoNavigator1.ForeColors = System.Drawing.Color.Empty
        Me.InfoNavigator1.GetServerText = False
        Me.InfoNavigator1.InternalQuery = True
        Me.InfoNavigator1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.bindingNavigatorMoveFirstItem, Me.bindingNavigatorMovePreviousItem, Me.bindingNavigatorMoveNextItem, Me.bindingNavigatorMoveLastItem, Me.bindingNavigatorSeparator1, Me.bindingNavigatorAddNewItem, Me.bindingNavigatorDeleteItem, Me.bindingNavigatorEditItem, Me.bindingNavigatorSeparator2, Me.bindingNavigatorOKItem, Me.bindingNavigatorCancelItem, Me.bindingNavigatorApplyItem, Me.bindingNavigatorAbortItem, Me.bindingNavigatorSeparator3, Me.bindingNavigatorRefreshItem, Me.bindingNavigatorQueryItem, Me.bindingNavigatorPrintItem, Me.bindingNavigatorSeparator4, Me.bindingNavigatorPositionItem, Me.bindingNavigatorCountItem})
        Me.InfoNavigator1.Location = New System.Drawing.Point(0, 0)
        Me.InfoNavigator1.MoveFirstItem = Nothing
        Me.InfoNavigator1.MoveLastItem = Nothing
        Me.InfoNavigator1.MoveNextItem = Nothing
        Me.InfoNavigator1.MovePreviousItem = Nothing
        Me.InfoNavigator1.Name = "InfoNavigator1"
        Me.InfoNavigator1.OKItem = Me.bindingNavigatorOKItem
        Me.InfoNavigator1.PositionItem = Nothing
        Me.InfoNavigator1.PreQueryCondition = CType(resources.GetObject("InfoNavigator1.PreQueryCondition"), System.Collections.Generic.List(Of String))
        Me.InfoNavigator1.PreQueryField = CType(resources.GetObject("InfoNavigator1.PreQueryField"), System.Collections.Generic.List(Of String))
        Me.InfoNavigator1.PreQueryValue = CType(resources.GetObject("InfoNavigator1.PreQueryValue"), System.Collections.Generic.List(Of String))
        Me.InfoNavigator1.PrintItem = Me.bindingNavigatorPrintItem
        Me.InfoNavigator1.QueryKeepCondition = False
        Me.InfoNavigator1.QuerySQLSend = True
        Me.InfoNavigator1.Size = New System.Drawing.Size(634, 36)
        StateItem9.Description = Nothing
        StateItem9.EnabledControls = CType(resources.GetObject("StateItem9.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem9.EnabledControlsEdited = False
        StateItem9.StateText = "Initial"
        StateItem10.Description = Nothing
        StateItem10.EnabledControls = CType(resources.GetObject("StateItem10.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem10.EnabledControlsEdited = False
        StateItem10.StateText = "Browsed"
        StateItem11.Description = Nothing
        StateItem11.EnabledControls = CType(resources.GetObject("StateItem11.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem11.EnabledControlsEdited = False
        StateItem11.StateText = "Inserting"
        StateItem12.Description = Nothing
        StateItem12.EnabledControls = CType(resources.GetObject("StateItem12.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem12.EnabledControlsEdited = False
        StateItem12.StateText = "Editing"
        StateItem13.Description = Nothing
        StateItem13.EnabledControls = CType(resources.GetObject("StateItem13.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem13.EnabledControlsEdited = False
        StateItem13.StateText = "Applying"
        StateItem14.Description = Nothing
        StateItem14.EnabledControls = CType(resources.GetObject("StateItem14.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem14.EnabledControlsEdited = False
        StateItem14.StateText = "Changing"
        StateItem15.Description = Nothing
        StateItem15.EnabledControls = CType(resources.GetObject("StateItem15.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem15.EnabledControlsEdited = False
        StateItem15.StateText = "Querying"
        StateItem16.Description = Nothing
        StateItem16.EnabledControls = CType(resources.GetObject("StateItem16.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem16.EnabledControlsEdited = False
        StateItem16.StateText = "Printing"
        Me.InfoNavigator1.States.Add(StateItem9)
        Me.InfoNavigator1.States.Add(StateItem10)
        Me.InfoNavigator1.States.Add(StateItem11)
        Me.InfoNavigator1.States.Add(StateItem12)
        Me.InfoNavigator1.States.Add(StateItem13)
        Me.InfoNavigator1.States.Add(StateItem14)
        Me.InfoNavigator1.States.Add(StateItem15)
        Me.InfoNavigator1.States.Add(StateItem16)
        Me.InfoNavigator1.StatusStrip = Me.InfoStatusStrip1
        Me.InfoNavigator1.SureDelete = True
        Me.InfoNavigator1.SureDeleteText = Nothing
        Me.InfoNavigator1.SureInsert = False
        Me.InfoNavigator1.SureInsertText = Nothing
        Me.InfoNavigator1.TabIndex = 0
        Me.InfoNavigator1.Text = "InfoNavigator1"
        Me.InfoNavigator1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.InfoNavigator1.ViewBindingSource = Nothing
        Me.InfoNavigator1.ViewCountItem = Me.bindingNavigatorCountItem
        Me.InfoNavigator1.ViewCountItemFormat = "of {0}"
        Me.InfoNavigator1.ViewMoveFirstItem = Me.bindingNavigatorMoveFirstItem
        Me.InfoNavigator1.ViewMoveLastItem = Me.bindingNavigatorMoveLastItem
        Me.InfoNavigator1.ViewMoveNextItem = Me.bindingNavigatorMoveNextItem
        Me.InfoNavigator1.ViewMovePreviousItem = Me.bindingNavigatorMovePreviousItem
        Me.InfoNavigator1.ViewPositionItem = Me.bindingNavigatorPositionItem
        Me.InfoNavigator1.ViewQueryItem = Me.bindingNavigatorQueryItem
        Me.InfoNavigator1.ViewRefreshItem = Me.bindingNavigatorRefreshItem
        '
        'bindingNavigatorAbortItem
        '
        Me.bindingNavigatorAbortItem.Image = CType(resources.GetObject("bindingNavigatorAbortItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorAbortItem.Name = "bindingNavigatorAbortItem"
        Me.bindingNavigatorAbortItem.Size = New System.Drawing.Size(37, 33)
        Me.bindingNavigatorAbortItem.Text = "abort"
        Me.bindingNavigatorAbortItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorAddNewItem
        '
        Me.bindingNavigatorAddNewItem.Image = CType(resources.GetObject("bindingNavigatorAddNewItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem"
        Me.bindingNavigatorAddNewItem.Size = New System.Drawing.Size(29, 33)
        Me.bindingNavigatorAddNewItem.Text = "add"
        Me.bindingNavigatorAddNewItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorApplyItem
        '
        Me.bindingNavigatorApplyItem.Image = CType(resources.GetObject("bindingNavigatorApplyItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorApplyItem.Name = "bindingNavigatorApplyItem"
        Me.bindingNavigatorApplyItem.Size = New System.Drawing.Size(37, 33)
        Me.bindingNavigatorApplyItem.Text = "apply"
        Me.bindingNavigatorApplyItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorCancelItem
        '
        Me.bindingNavigatorCancelItem.Image = CType(resources.GetObject("bindingNavigatorCancelItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorCancelItem.Name = "bindingNavigatorCancelItem"
        Me.bindingNavigatorCancelItem.Size = New System.Drawing.Size(41, 33)
        Me.bindingNavigatorCancelItem.Text = "cancel"
        Me.bindingNavigatorCancelItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorDeleteItem
        '
        Me.bindingNavigatorDeleteItem.Image = CType(resources.GetObject("bindingNavigatorDeleteItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem"
        Me.bindingNavigatorDeleteItem.Size = New System.Drawing.Size(41, 33)
        Me.bindingNavigatorDeleteItem.Text = "delete"
        Me.bindingNavigatorDeleteItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorEditItem
        '
        Me.bindingNavigatorEditItem.Image = CType(resources.GetObject("bindingNavigatorEditItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorEditItem.Name = "bindingNavigatorEditItem"
        Me.bindingNavigatorEditItem.Size = New System.Drawing.Size(29, 33)
        Me.bindingNavigatorEditItem.Text = "edit"
        Me.bindingNavigatorEditItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorMoveFirstItem
        '
        Me.bindingNavigatorMoveFirstItem.Enabled = False
        Me.bindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("bindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem"
        Me.bindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(30, 33)
        Me.bindingNavigatorMoveFirstItem.Text = "first"
        Me.bindingNavigatorMoveFirstItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorMovePreviousItem
        '
        Me.bindingNavigatorMovePreviousItem.Enabled = False
        Me.bindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("bindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem"
        Me.bindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(52, 33)
        Me.bindingNavigatorMovePreviousItem.Text = "previous"
        Me.bindingNavigatorMovePreviousItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorMoveNextItem
        '
        Me.bindingNavigatorMoveNextItem.Image = CType(resources.GetObject("bindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem"
        Me.bindingNavigatorMoveNextItem.Size = New System.Drawing.Size(33, 33)
        Me.bindingNavigatorMoveNextItem.Text = "next"
        Me.bindingNavigatorMoveNextItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorMoveLastItem
        '
        Me.bindingNavigatorMoveLastItem.Image = CType(resources.GetObject("bindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem"
        Me.bindingNavigatorMoveLastItem.Size = New System.Drawing.Size(28, 33)
        Me.bindingNavigatorMoveLastItem.Text = "last"
        Me.bindingNavigatorMoveLastItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorSeparator1
        '
        Me.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1"
        Me.bindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 36)
        '
        'bindingNavigatorSeparator2
        '
        Me.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2"
        Me.bindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 36)
        '
        'bindingNavigatorOKItem
        '
        Me.bindingNavigatorOKItem.Image = CType(resources.GetObject("bindingNavigatorOKItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorOKItem.Name = "bindingNavigatorOKItem"
        Me.bindingNavigatorOKItem.Size = New System.Drawing.Size(23, 33)
        Me.bindingNavigatorOKItem.Text = "ok"
        Me.bindingNavigatorOKItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorSeparator3
        '
        Me.bindingNavigatorSeparator3.Name = "bindingNavigatorSeparator3"
        Me.bindingNavigatorSeparator3.Size = New System.Drawing.Size(6, 36)
        '
        'bindingNavigatorRefreshItem
        '
        Me.bindingNavigatorRefreshItem.Image = CType(resources.GetObject("bindingNavigatorRefreshItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorRefreshItem.Name = "bindingNavigatorRefreshItem"
        Me.bindingNavigatorRefreshItem.Size = New System.Drawing.Size(46, 33)
        Me.bindingNavigatorRefreshItem.Text = "refresh"
        Me.bindingNavigatorRefreshItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorQueryItem
        '
        Me.bindingNavigatorQueryItem.Image = CType(resources.GetObject("bindingNavigatorQueryItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorQueryItem.Name = "bindingNavigatorQueryItem"
        Me.bindingNavigatorQueryItem.Size = New System.Drawing.Size(39, 33)
        Me.bindingNavigatorQueryItem.Text = "query"
        Me.bindingNavigatorQueryItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorPrintItem
        '
        Me.bindingNavigatorPrintItem.Image = CType(resources.GetObject("bindingNavigatorPrintItem.Image"), System.Drawing.Image)
        Me.bindingNavigatorPrintItem.Name = "bindingNavigatorPrintItem"
        Me.bindingNavigatorPrintItem.Size = New System.Drawing.Size(33, 33)
        Me.bindingNavigatorPrintItem.Text = "print"
        Me.bindingNavigatorPrintItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'bindingNavigatorSeparator4
        '
        Me.bindingNavigatorSeparator4.Name = "bindingNavigatorSeparator4"
        Me.bindingNavigatorSeparator4.Size = New System.Drawing.Size(6, 36)
        '
        'bindingNavigatorPositionItem
        '
        Me.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem"
        Me.bindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 36)
        Me.bindingNavigatorPositionItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.bindingNavigatorPositionItem.ToolTipText = "position"
        '
        'bindingNavigatorCountItem
        '
        Me.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem"
        Me.bindingNavigatorCountItem.Size = New System.Drawing.Size(36, 13)
        Me.bindingNavigatorCountItem.Text = "of {0}"
        Me.bindingNavigatorCountItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.bindingNavigatorCountItem.ToolTipText = "count"
        '
        'InfoStatusStrip1
        '
        Me.InfoStatusStrip1.Location = New System.Drawing.Point(0, 354)
        Me.InfoStatusStrip1.Name = "InfoStatusStrip1"
        Me.InfoStatusStrip1.ShowCompany = True
        Me.InfoStatusStrip1.ShowDate = True
        Me.InfoStatusStrip1.ShowEEPAlias = True
        Me.InfoStatusStrip1.ShowNavigatorStatus = True
        Me.InfoStatusStrip1.ShowSolution = True
        Me.InfoStatusStrip1.ShowUserID = True
        Me.InfoStatusStrip1.ShowUserName = True
        Me.InfoStatusStrip1.Size = New System.Drawing.Size(634, 22)
        Me.InfoStatusStrip1.TabIndex = 1
        Me.InfoStatusStrip1.Text = "InfoStatusStrip1"
        '
        'scMaster
        '
        Me.scMaster.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMaster.Location = New System.Drawing.Point(0, 36)
        Me.scMaster.Name = "scMaster"
        Me.scMaster.Size = New System.Drawing.Size(634, 318)
        Me.scMaster.SplitterDistance = 211
        Me.scMaster.TabIndex = 5
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.ClientSize = New System.Drawing.Size(634, 376)
        Me.Controls.Add(Me.scMaster)
        Me.Controls.Add(Me.InfoStatusStrip1)
        Me.Controls.Add(Me.InfoNavigator1)
        Me.Name = "Form1"
        CType(Me.InfoNavigator1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InfoNavigator1.ResumeLayout(False)
        Me.InfoNavigator1.PerformLayout()
        Me.scMaster.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private components As System.ComponentModel.IContainer
    Friend WithEvents InfoNavigator1 As Srvtools.InfoNavigator
    Friend WithEvents bindingNavigatorAbortItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorAddNewItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorApplyItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorCancelItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorEditItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bindingNavigatorOKItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bindingNavigatorRefreshItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorQueryItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorPrintItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents bindingNavigatorSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents bindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents InfoStatusStrip1 As Srvtools.InfoStatusStrip
    Private WithEvents scMaster As System.Windows.Forms.SplitContainer
    Friend WithEvents bindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
End Class
