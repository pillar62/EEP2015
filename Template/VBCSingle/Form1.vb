Public Class Form1
    Inherits Srvtools.InfoForm
    Public Sub New()
        InitializeComponent()
    End Sub 'New
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Dim StateItem1 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem2 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem3 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem4 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem5 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem6 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem7 As Srvtools.StateItem = New Srvtools.StateItem
        Dim StateItem8 As Srvtools.StateItem = New Srvtools.StateItem
        Me.Master = New Srvtools.InfoDataSet(Me.components)
        Me.ibsMaster = New Srvtools.InfoBindingSource(Me.components)
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
        CType(Me.Master, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ibsMaster, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InfoNavigator1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InfoNavigator1.SuspendLayout()
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
        Me.ibsMaster.DelayInterval = 300
        Me.ibsMaster.DisableKeyFields = False
        Me.ibsMaster.EnableFlag = False
        Me.ibsMaster.FocusedControl = Nothing
        Me.ibsMaster.OwnerComp = Nothing
        Me.ibsMaster.RelationDelay = False
        Me.ibsMaster.text = "ibsMaster"
        '
        'InfoNavigator1
        '
        Me.InfoNavigator1.AbortItem = Me.bindingNavigatorAbortItem
        Me.InfoNavigator1.AddNewItem = Me.bindingNavigatorAddNewItem
        Me.InfoNavigator1.ApplyItem = Me.bindingNavigatorApplyItem
        Me.InfoNavigator1.BindingSource = Me.ibsMaster
        Me.InfoNavigator1.CancelItem = Me.bindingNavigatorCancelItem
        Me.InfoNavigator1.CountItem = Nothing
        Me.InfoNavigator1.DeleteItem = Me.bindingNavigatorDeleteItem
        Me.InfoNavigator1.DescriptionItem = Nothing
        Me.InfoNavigator1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText
        Me.InfoNavigator1.EditItem = Me.bindingNavigatorEditItem
        Me.InfoNavigator1.ExportItem = Nothing
        Me.InfoNavigator1.ForeColors = System.Drawing.Color.Empty
        Me.InfoNavigator1.GetServerText = True
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
        StateItem1.Description = "Initial"
        StateItem1.EnabledControls = CType(resources.GetObject("StateItem1.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem1.EnabledControlsEdited = False
        StateItem1.StateText = "Initial"
        StateItem2.Description = "Browsed"
        StateItem2.EnabledControls = CType(resources.GetObject("StateItem2.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem2.EnabledControlsEdited = False
        StateItem2.StateText = "Browsed"
        StateItem3.Description = "Inserting"
        StateItem3.EnabledControls = CType(resources.GetObject("StateItem3.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem3.EnabledControlsEdited = False
        StateItem3.StateText = "Inserting"
        StateItem4.Description = "Editing"
        StateItem4.EnabledControls = CType(resources.GetObject("StateItem4.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem4.EnabledControlsEdited = False
        StateItem4.StateText = "Editing"
        StateItem5.Description = "Applying"
        StateItem5.EnabledControls = CType(resources.GetObject("StateItem5.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem5.EnabledControlsEdited = False
        StateItem5.StateText = "Applying"
        StateItem6.Description = "Changing"
        StateItem6.EnabledControls = CType(resources.GetObject("StateItem6.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem6.EnabledControlsEdited = False
        StateItem6.StateText = "Changing"
        StateItem7.Description = "Querying"
        StateItem7.EnabledControls = CType(resources.GetObject("StateItem7.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem7.EnabledControlsEdited = False
        StateItem7.StateText = "Querying"
        StateItem8.Description = "Printing"
        StateItem8.EnabledControls = CType(resources.GetObject("StateItem8.EnabledControls"), System.Collections.Generic.List(Of String))
        StateItem8.EnabledControlsEdited = False
        StateItem8.StateText = "Printing"
        Me.InfoNavigator1.States.Add(StateItem1)
        Me.InfoNavigator1.States.Add(StateItem2)
        Me.InfoNavigator1.States.Add(StateItem3)
        Me.InfoNavigator1.States.Add(StateItem4)
        Me.InfoNavigator1.States.Add(StateItem5)
        Me.InfoNavigator1.States.Add(StateItem6)
        Me.InfoNavigator1.States.Add(StateItem7)
        Me.InfoNavigator1.States.Add(StateItem8)
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
        Me.InfoStatusStrip1.ShowCompany = False
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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.ClientSize = New System.Drawing.Size(634, 376)
        Me.Controls.Add(Me.InfoStatusStrip1)
        Me.Controls.Add(Me.InfoNavigator1)
        Me.Name = "Form1"
        CType(Me.Master, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ibsMaster, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InfoNavigator1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InfoNavigator1.ResumeLayout(False)
        Me.InfoNavigator1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private components As System.ComponentModel.IContainer
    Friend WithEvents Master As Srvtools.InfoDataSet
    Friend WithEvents ibsMaster As Srvtools.InfoBindingSource
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
    Friend WithEvents bindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
End Class
