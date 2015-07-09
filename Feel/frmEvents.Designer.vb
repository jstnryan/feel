<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEvents
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)

            ''Singleon Pattern: http://www.codeproject.com/KB/vb/Simple_Singleton_Forms.aspx
            main.eventForm = Nothing
            'main.DisposeActionWindow()
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEvents))
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Control Pressed", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Control Released", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Control Changed", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("this one has an even longer name than the other ""longer"" one I added.")
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("This one has a really long name.")
        Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Action on Page 9")
        Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Action on Page 7")
        Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Action on Page 4")
        Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Action on Page 2")
        Dim ListViewItem7 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Action on page 10")
        Dim ListViewItem8 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Action on Page 1")
        Dim ListViewItem9 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Action on Page 0")
        Me.grpConfiguration = New System.Windows.Forms.GroupBox
        Me.cmdEditInitalState = New System.Windows.Forms.Button
        Me.txtInitialState = New System.Windows.Forms.TextBox
        Me.lblInitialState = New System.Windows.Forms.Label
        Me.grpControlBehavior = New System.Windows.Forms.GroupBox
        Me.rdoLatchRelative = New System.Windows.Forms.RadioButton
        Me.rdoMomentaryAbsolute = New System.Windows.Forms.RadioButton
        Me.nudControlGroup = New System.Windows.Forms.NumericUpDown
        Me.lblControlGroup = New System.Windows.Forms.Label
        Me.grpAction = New System.Windows.Forms.GroupBox
        Me.txtActionDescription = New System.Windows.Forms.TextBox
        Me.pgAction = New System.Windows.Forms.PropertyGrid
        Me.lblActionFunction = New System.Windows.Forms.Label
        Me.txtActionName = New System.Windows.Forms.TextBox
        Me.lblActionName = New System.Windows.Forms.Label
        Me.ttActions = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdActionSwap = New System.Windows.Forms.Button
        Me.cmdActionRemove = New System.Windows.Forms.Button
        Me.cmdActionAdd = New System.Windows.Forms.Button
        Me.cmdActionDown = New System.Windows.Forms.Button
        Me.cmdActionUp = New System.Windows.Forms.Button
        Me.cmdActionClear = New System.Windows.Forms.Button
        Me.grpActions = New System.Windows.Forms.GroupBox
        Me.lvActions = New System.Windows.Forms.ListView
        Me.colAction = New System.Windows.Forms.ColumnHeader
        Me.cboActionFunction = New Feel.GroupedComboBox
        Me.grpInput = New Feel.CheckedGroupBox
        Me.txtDefaultState = New System.Windows.Forms.TextBox
        Me.lblDefaultState = New System.Windows.Forms.Label
        Me.grpInputValues = New System.Windows.Forms.GroupBox
        Me.lblVelocityValue = New System.Windows.Forms.Label
        Me.lblNoteControl = New System.Windows.Forms.Label
        Me.lblChannel = New System.Windows.Forms.Label
        Me.lblVelVal = New System.Windows.Forms.Label
        Me.lblNotCon = New System.Windows.Forms.Label
        Me.lblChan = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.cmdEditDefaultState = New System.Windows.Forms.Button
        Me.lblDevice = New System.Windows.Forms.Label
        Me.lblControlDevice = New System.Windows.Forms.Label
        Me.chkPaged = New System.Windows.Forms.CheckBox
        Me.lblControlType = New System.Windows.Forms.Label
        Me.lblControlPage = New System.Windows.Forms.Label
        Me.nudDevicePage = New System.Windows.Forms.NumericUpDown
        Me.grpConfiguration.SuspendLayout()
        Me.grpControlBehavior.SuspendLayout()
        CType(Me.nudControlGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAction.SuspendLayout()
        Me.grpActions.SuspendLayout()
        Me.grpInput.SuspendLayout()
        Me.grpInputValues.SuspendLayout()
        CType(Me.nudDevicePage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpConfiguration
        '
        Me.grpConfiguration.Controls.Add(Me.cmdEditInitalState)
        Me.grpConfiguration.Controls.Add(Me.txtInitialState)
        Me.grpConfiguration.Controls.Add(Me.lblInitialState)
        Me.grpConfiguration.Controls.Add(Me.grpControlBehavior)
        Me.grpConfiguration.Controls.Add(Me.nudControlGroup)
        Me.grpConfiguration.Controls.Add(Me.lblControlGroup)
        Me.grpConfiguration.Enabled = False
        Me.grpConfiguration.Location = New System.Drawing.Point(13, 278)
        Me.grpConfiguration.Name = "grpConfiguration"
        Me.grpConfiguration.Size = New System.Drawing.Size(213, 168)
        Me.grpConfiguration.TabIndex = 3
        Me.grpConfiguration.TabStop = False
        Me.grpConfiguration.Text = "Configuration"
        '
        'cmdEditInitalState
        '
        Me.cmdEditInitalState.Image = Global.Feel.My.Resources.Feel.EditInformation
        Me.cmdEditInitalState.Location = New System.Drawing.Point(173, 22)
        Me.cmdEditInitalState.Name = "cmdEditInitalState"
        Me.cmdEditInitalState.Size = New System.Drawing.Size(22, 20)
        Me.cmdEditInitalState.TabIndex = 6
        Me.ttActions.SetToolTip(Me.cmdEditInitalState, "MIDI String Editor")
        Me.cmdEditInitalState.UseVisualStyleBackColor = True
        '
        'txtInitialState
        '
        Me.txtInitialState.Location = New System.Drawing.Point(86, 22)
        Me.txtInitialState.Name = "txtInitialState"
        Me.txtInitialState.Size = New System.Drawing.Size(87, 20)
        Me.txtInitialState.TabIndex = 5
        '
        'lblInitialState
        '
        Me.lblInitialState.AutoSize = True
        Me.lblInitialState.Location = New System.Drawing.Point(15, 25)
        Me.lblInitialState.Name = "lblInitialState"
        Me.lblInitialState.Size = New System.Drawing.Size(62, 13)
        Me.lblInitialState.TabIndex = 4
        Me.lblInitialState.Text = "Initial State:"
        '
        'grpControlBehavior
        '
        Me.grpControlBehavior.Controls.Add(Me.rdoLatchRelative)
        Me.grpControlBehavior.Controls.Add(Me.rdoMomentaryAbsolute)
        Me.grpControlBehavior.Location = New System.Drawing.Point(18, 79)
        Me.grpControlBehavior.Name = "grpControlBehavior"
        Me.grpControlBehavior.Size = New System.Drawing.Size(177, 74)
        Me.grpControlBehavior.TabIndex = 3
        Me.grpControlBehavior.TabStop = False
        Me.grpControlBehavior.Text = "Behavior"
        '
        'rdoLatchRelative
        '
        Me.rdoLatchRelative.AutoSize = True
        Me.rdoLatchRelative.Location = New System.Drawing.Point(14, 43)
        Me.rdoLatchRelative.Name = "rdoLatchRelative"
        Me.rdoLatchRelative.Size = New System.Drawing.Size(52, 17)
        Me.rdoLatchRelative.TabIndex = 9
        Me.rdoLatchRelative.Text = "Latch"
        Me.rdoLatchRelative.UseVisualStyleBackColor = True
        '
        'rdoMomentaryAbsolute
        '
        Me.rdoMomentaryAbsolute.AutoSize = True
        Me.rdoMomentaryAbsolute.Checked = True
        Me.rdoMomentaryAbsolute.Location = New System.Drawing.Point(14, 20)
        Me.rdoMomentaryAbsolute.Name = "rdoMomentaryAbsolute"
        Me.rdoMomentaryAbsolute.Size = New System.Drawing.Size(77, 17)
        Me.rdoMomentaryAbsolute.TabIndex = 8
        Me.rdoMomentaryAbsolute.TabStop = True
        Me.rdoMomentaryAbsolute.Text = "Momentary"
        Me.rdoMomentaryAbsolute.UseVisualStyleBackColor = True
        '
        'nudControlGroup
        '
        Me.nudControlGroup.Location = New System.Drawing.Point(131, 53)
        Me.nudControlGroup.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudControlGroup.Name = "nudControlGroup"
        Me.nudControlGroup.Size = New System.Drawing.Size(64, 20)
        Me.nudControlGroup.TabIndex = 7
        '
        'lblControlGroup
        '
        Me.lblControlGroup.AutoSize = True
        Me.lblControlGroup.Location = New System.Drawing.Point(15, 55)
        Me.lblControlGroup.Name = "lblControlGroup"
        Me.lblControlGroup.Size = New System.Drawing.Size(75, 13)
        Me.lblControlGroup.TabIndex = 0
        Me.lblControlGroup.Text = "Control Group:"
        '
        'grpAction
        '
        Me.grpAction.Controls.Add(Me.txtActionDescription)
        Me.grpAction.Controls.Add(Me.pgAction)
        Me.grpAction.Controls.Add(Me.cboActionFunction)
        Me.grpAction.Controls.Add(Me.lblActionFunction)
        Me.grpAction.Controls.Add(Me.txtActionName)
        Me.grpAction.Controls.Add(Me.lblActionName)
        Me.grpAction.Enabled = False
        Me.grpAction.Location = New System.Drawing.Point(468, 12)
        Me.grpAction.Name = "grpAction"
        Me.grpAction.Size = New System.Drawing.Size(277, 434)
        Me.grpAction.TabIndex = 12
        Me.grpAction.TabStop = False
        Me.grpAction.Text = "Action"
        '
        'txtActionDescription
        '
        Me.txtActionDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtActionDescription.Location = New System.Drawing.Point(15, 71)
        Me.txtActionDescription.Multiline = True
        Me.txtActionDescription.Name = "txtActionDescription"
        Me.txtActionDescription.ReadOnly = True
        Me.txtActionDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtActionDescription.Size = New System.Drawing.Size(247, 114)
        Me.txtActionDescription.TabIndex = 20
        Me.txtActionDescription.TabStop = False
        '
        'pgAction
        '
        Me.pgAction.Location = New System.Drawing.Point(15, 191)
        Me.pgAction.Name = "pgAction"
        Me.pgAction.PropertySort = System.Windows.Forms.PropertySort.NoSort
        Me.pgAction.Size = New System.Drawing.Size(247, 228)
        Me.pgAction.TabIndex = 19
        Me.pgAction.ToolbarVisible = False
        '
        'lblActionFunction
        '
        Me.lblActionFunction.AutoSize = True
        Me.lblActionFunction.Location = New System.Drawing.Point(12, 47)
        Me.lblActionFunction.Name = "lblActionFunction"
        Me.lblActionFunction.Size = New System.Drawing.Size(51, 13)
        Me.lblActionFunction.TabIndex = 4
        Me.lblActionFunction.Text = "Function:"
        '
        'txtActionName
        '
        Me.txtActionName.Location = New System.Drawing.Point(69, 22)
        Me.txtActionName.MaxLength = 256
        Me.txtActionName.Name = "txtActionName"
        Me.txtActionName.Size = New System.Drawing.Size(193, 20)
        Me.txtActionName.TabIndex = 17
        '
        'lblActionName
        '
        Me.lblActionName.AutoSize = True
        Me.lblActionName.Location = New System.Drawing.Point(12, 25)
        Me.lblActionName.Name = "lblActionName"
        Me.lblActionName.Size = New System.Drawing.Size(38, 13)
        Me.lblActionName.TabIndex = 2
        Me.lblActionName.Text = "Name:"
        '
        'cmdActionSwap
        '
        Me.cmdActionSwap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdActionSwap.Enabled = False
        Me.cmdActionSwap.Image = CType(resources.GetObject("cmdActionSwap.Image"), System.Drawing.Image)
        Me.cmdActionSwap.Location = New System.Drawing.Point(131, 360)
        Me.cmdActionSwap.Name = "cmdActionSwap"
        Me.cmdActionSwap.Size = New System.Drawing.Size(26, 26)
        Me.cmdActionSwap.TabIndex = 15
        Me.ttActions.SetToolTip(Me.cmdActionSwap, "Move selected action between events.")
        Me.cmdActionSwap.UseVisualStyleBackColor = True
        '
        'cmdActionRemove
        '
        Me.cmdActionRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdActionRemove.Enabled = False
        Me.cmdActionRemove.Image = CType(resources.GetObject("cmdActionRemove.Image"), System.Drawing.Image)
        Me.cmdActionRemove.Location = New System.Drawing.Point(15, 393)
        Me.cmdActionRemove.Name = "cmdActionRemove"
        Me.cmdActionRemove.Size = New System.Drawing.Size(26, 26)
        Me.cmdActionRemove.TabIndex = 12
        Me.ttActions.SetToolTip(Me.cmdActionRemove, "Remove selected action.")
        Me.cmdActionRemove.UseVisualStyleBackColor = True
        '
        'cmdActionAdd
        '
        Me.cmdActionAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdActionAdd.Image = CType(resources.GetObject("cmdActionAdd.Image"), System.Drawing.Image)
        Me.cmdActionAdd.Location = New System.Drawing.Point(15, 361)
        Me.cmdActionAdd.Name = "cmdActionAdd"
        Me.cmdActionAdd.Size = New System.Drawing.Size(26, 26)
        Me.cmdActionAdd.TabIndex = 11
        Me.ttActions.SetToolTip(Me.cmdActionAdd, "Add new action.")
        Me.cmdActionAdd.UseVisualStyleBackColor = True
        '
        'cmdActionDown
        '
        Me.cmdActionDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdActionDown.Enabled = False
        Me.cmdActionDown.Image = CType(resources.GetObject("cmdActionDown.Image"), System.Drawing.Image)
        Me.cmdActionDown.Location = New System.Drawing.Point(73, 393)
        Me.cmdActionDown.Name = "cmdActionDown"
        Me.cmdActionDown.Size = New System.Drawing.Size(26, 26)
        Me.cmdActionDown.TabIndex = 14
        Me.ttActions.SetToolTip(Me.cmdActionDown, "Move selected action down.")
        Me.cmdActionDown.UseVisualStyleBackColor = True
        '
        'cmdActionUp
        '
        Me.cmdActionUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdActionUp.Enabled = False
        Me.cmdActionUp.Image = CType(resources.GetObject("cmdActionUp.Image"), System.Drawing.Image)
        Me.cmdActionUp.Location = New System.Drawing.Point(73, 361)
        Me.cmdActionUp.Name = "cmdActionUp"
        Me.cmdActionUp.Size = New System.Drawing.Size(26, 26)
        Me.cmdActionUp.TabIndex = 13
        Me.ttActions.SetToolTip(Me.cmdActionUp, "Move selected action up.")
        Me.cmdActionUp.UseVisualStyleBackColor = True
        '
        'cmdActionClear
        '
        Me.cmdActionClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.cmdActionClear.Image = CType(resources.GetObject("cmdActionClear.Image"), System.Drawing.Image)
        Me.cmdActionClear.Location = New System.Drawing.Point(189, 360)
        Me.cmdActionClear.Name = "cmdActionClear"
        Me.cmdActionClear.Size = New System.Drawing.Size(26, 26)
        Me.cmdActionClear.TabIndex = 16
        Me.ttActions.SetToolTip(Me.cmdActionClear, "Clear all actions.")
        Me.cmdActionClear.UseVisualStyleBackColor = True
        '
        'grpActions
        '
        Me.grpActions.Controls.Add(Me.cmdActionSwap)
        Me.grpActions.Controls.Add(Me.cmdActionRemove)
        Me.grpActions.Controls.Add(Me.cmdActionAdd)
        Me.grpActions.Controls.Add(Me.cmdActionDown)
        Me.grpActions.Controls.Add(Me.cmdActionUp)
        Me.grpActions.Controls.Add(Me.cmdActionClear)
        Me.grpActions.Controls.Add(Me.lvActions)
        Me.grpActions.Enabled = False
        Me.grpActions.Location = New System.Drawing.Point(232, 12)
        Me.grpActions.Name = "grpActions"
        Me.grpActions.Size = New System.Drawing.Size(230, 434)
        Me.grpActions.TabIndex = 13
        Me.grpActions.TabStop = False
        Me.grpActions.Text = "Actions List"
        '
        'lvActions
        '
        Me.lvActions.Alignment = System.Windows.Forms.ListViewAlignment.[Default]
        Me.lvActions.AutoArrange = False
        Me.lvActions.CheckBoxes = True
        Me.lvActions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAction})
        Me.lvActions.FullRowSelect = True
        ListViewGroup1.Header = "Control Pressed"
        ListViewGroup1.Name = "lvgPressed"
        ListViewGroup2.Header = "Control Released"
        ListViewGroup2.Name = "lvgReleased"
        ListViewGroup3.Header = "Control Changed"
        ListViewGroup3.Name = "lvgChanged"
        Me.lvActions.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3})
        Me.lvActions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvActions.HideSelection = False
        ListViewItem1.Group = ListViewGroup2
        ListViewItem1.StateImageIndex = 0
        ListViewItem2.Checked = True
        ListViewItem2.Group = ListViewGroup1
        ListViewItem2.StateImageIndex = 1
        ListViewItem3.Group = ListViewGroup2
        ListViewItem3.StateImageIndex = 0
        ListViewItem4.Group = ListViewGroup2
        ListViewItem4.StateImageIndex = 0
        ListViewItem5.Group = ListViewGroup1
        ListViewItem5.StateImageIndex = 0
        ListViewItem6.Group = ListViewGroup2
        ListViewItem6.StateImageIndex = 0
        ListViewItem7.Group = ListViewGroup1
        ListViewItem7.StateImageIndex = 0
        ListViewItem8.Group = ListViewGroup2
        ListViewItem8.StateImageIndex = 0
        ListViewItem9.Group = ListViewGroup1
        ListViewItem9.StateImageIndex = 0
        Me.lvActions.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6, ListViewItem7, ListViewItem8, ListViewItem9})
        Me.lvActions.LabelWrap = False
        Me.lvActions.Location = New System.Drawing.Point(15, 22)
        Me.lvActions.MultiSelect = False
        Me.lvActions.Name = "lvActions"
        Me.lvActions.ShowItemToolTips = True
        Me.lvActions.Size = New System.Drawing.Size(200, 332)
        Me.lvActions.TabIndex = 10
        Me.lvActions.UseCompatibleStateImageBehavior = False
        Me.lvActions.View = System.Windows.Forms.View.Details
        '
        'colAction
        '
        Me.colAction.Text = "Action"
        Me.colAction.Width = 174
        '
        'cboActionFunction
        '
        Me.cboActionFunction.DropDownHeight = 212
        Me.cboActionFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboActionFunction.FormattingEnabled = True
        Me.cboActionFunction.GroupMember = "Group"
        Me.cboActionFunction.IntegralHeight = False
        Me.cboActionFunction.Location = New System.Drawing.Point(69, 44)
        Me.cboActionFunction.MaxDropDownItems = 20
        Me.cboActionFunction.Name = "cboActionFunction"
        Me.cboActionFunction.Size = New System.Drawing.Size(193, 21)
        Me.cboActionFunction.TabIndex = 18
        '
        'grpInput
        '
        Me.grpInput.CheckBoxToolTip = "When checked, the last input will be retained (for editing) until unchecked."
        Me.grpInput.Checked = False
        Me.grpInput.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.grpInput.Controls.Add(Me.txtDefaultState)
        Me.grpInput.Controls.Add(Me.lblDefaultState)
        Me.grpInput.Controls.Add(Me.grpInputValues)
        Me.grpInput.Controls.Add(Me.lblDescription)
        Me.grpInput.Controls.Add(Me.cmdEditDefaultState)
        Me.grpInput.Controls.Add(Me.lblDevice)
        Me.grpInput.Controls.Add(Me.lblControlDevice)
        Me.grpInput.Controls.Add(Me.chkPaged)
        Me.grpInput.Controls.Add(Me.lblControlType)
        Me.grpInput.Controls.Add(Me.lblControlPage)
        Me.grpInput.Controls.Add(Me.nudDevicePage)
        Me.grpInput.DisableChildrenIfUnchecked = False
        Me.grpInput.Location = New System.Drawing.Point(12, 12)
        Me.grpInput.Name = "grpInput"
        Me.grpInput.Size = New System.Drawing.Size(213, 260)
        Me.grpInput.TabIndex = 0
        Me.grpInput.TabStop = False
        Me.grpInput.Text = "Input"
        '
        'txtDefaultState
        '
        Me.txtDefaultState.Enabled = False
        Me.txtDefaultState.Location = New System.Drawing.Point(87, 226)
        Me.txtDefaultState.Name = "txtDefaultState"
        Me.txtDefaultState.Size = New System.Drawing.Size(87, 20)
        Me.txtDefaultState.TabIndex = 3
        '
        'lblDefaultState
        '
        Me.lblDefaultState.AutoSize = True
        Me.lblDefaultState.Location = New System.Drawing.Point(15, 229)
        Me.lblDefaultState.Name = "lblDefaultState"
        Me.lblDefaultState.Size = New System.Drawing.Size(72, 13)
        Me.lblDefaultState.TabIndex = 14
        Me.lblDefaultState.Text = "Default State:"
        '
        'grpInputValues
        '
        Me.grpInputValues.Controls.Add(Me.lblVelocityValue)
        Me.grpInputValues.Controls.Add(Me.lblNoteControl)
        Me.grpInputValues.Controls.Add(Me.lblChannel)
        Me.grpInputValues.Controls.Add(Me.lblVelVal)
        Me.grpInputValues.Controls.Add(Me.lblNotCon)
        Me.grpInputValues.Controls.Add(Me.lblChan)
        Me.grpInputValues.Location = New System.Drawing.Point(18, 72)
        Me.grpInputValues.Name = "grpInputValues"
        Me.grpInputValues.Size = New System.Drawing.Size(178, 93)
        Me.grpInputValues.TabIndex = 11
        Me.grpInputValues.TabStop = False
        Me.grpInputValues.Text = "MIDI Values:"
        '
        'lblVelocityValue
        '
        Me.lblVelocityValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVelocityValue.Location = New System.Drawing.Point(66, 66)
        Me.lblVelocityValue.Name = "lblVelocityValue"
        Me.lblVelocityValue.Size = New System.Drawing.Size(101, 13)
        Me.lblVelocityValue.TabIndex = 15
        Me.lblVelocityValue.Text = "N/A"
        '
        'lblNoteControl
        '
        Me.lblNoteControl.AutoEllipsis = True
        Me.lblNoteControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoteControl.Location = New System.Drawing.Point(66, 44)
        Me.lblNoteControl.MaximumSize = New System.Drawing.Size(90, 13)
        Me.lblNoteControl.Name = "lblNoteControl"
        Me.lblNoteControl.Size = New System.Drawing.Size(90, 13)
        Me.lblNoteControl.TabIndex = 14
        Me.lblNoteControl.Text = "N/A"
        '
        'lblChannel
        '
        Me.lblChannel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChannel.Location = New System.Drawing.Point(66, 22)
        Me.lblChannel.Name = "lblChannel"
        Me.lblChannel.Size = New System.Drawing.Size(101, 13)
        Me.lblChannel.TabIndex = 13
        Me.lblChannel.Text = "N/A"
        '
        'lblVelVal
        '
        Me.lblVelVal.AutoSize = True
        Me.lblVelVal.Location = New System.Drawing.Point(11, 66)
        Me.lblVelVal.Name = "lblVelVal"
        Me.lblVelVal.Size = New System.Drawing.Size(47, 13)
        Me.lblVelVal.TabIndex = 12
        Me.lblVelVal.Text = "Velocity:"
        '
        'lblNotCon
        '
        Me.lblNotCon.AutoSize = True
        Me.lblNotCon.Location = New System.Drawing.Point(11, 44)
        Me.lblNotCon.Name = "lblNotCon"
        Me.lblNotCon.Size = New System.Drawing.Size(33, 13)
        Me.lblNotCon.TabIndex = 11
        Me.lblNotCon.Text = "Note:"
        '
        'lblChan
        '
        Me.lblChan.AutoSize = True
        Me.lblChan.Location = New System.Drawing.Point(11, 22)
        Me.lblChan.Name = "lblChan"
        Me.lblChan.Size = New System.Drawing.Size(49, 13)
        Me.lblChan.TabIndex = 10
        Me.lblChan.Text = "Channel:"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(84, 47)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(27, 13)
        Me.lblDescription.TabIndex = 10
        Me.lblDescription.Text = "N/A"
        '
        'cmdEditDefaultState
        '
        Me.cmdEditDefaultState.Enabled = False
        Me.cmdEditDefaultState.Image = Global.Feel.My.Resources.Feel.EditInformation
        Me.cmdEditDefaultState.Location = New System.Drawing.Point(174, 226)
        Me.cmdEditDefaultState.Name = "cmdEditDefaultState"
        Me.cmdEditDefaultState.Size = New System.Drawing.Size(22, 20)
        Me.cmdEditDefaultState.TabIndex = 4
        Me.ttActions.SetToolTip(Me.cmdEditDefaultState, "MIDI String Editor")
        Me.cmdEditDefaultState.UseVisualStyleBackColor = True
        '
        'lblDevice
        '
        Me.lblDevice.AutoSize = True
        Me.lblDevice.Location = New System.Drawing.Point(84, 25)
        Me.lblDevice.Name = "lblDevice"
        Me.lblDevice.Size = New System.Drawing.Size(33, 13)
        Me.lblDevice.TabIndex = 6
        Me.lblDevice.Text = "None"
        '
        'lblControlDevice
        '
        Me.lblControlDevice.AutoSize = True
        Me.lblControlDevice.Location = New System.Drawing.Point(15, 25)
        Me.lblControlDevice.Name = "lblControlDevice"
        Me.lblControlDevice.Size = New System.Drawing.Size(47, 13)
        Me.lblControlDevice.TabIndex = 2
        Me.lblControlDevice.Text = "Device::"
        '
        'chkPaged
        '
        Me.chkPaged.AutoSize = True
        Me.chkPaged.Enabled = False
        Me.chkPaged.Location = New System.Drawing.Point(18, 173)
        Me.chkPaged.Name = "chkPaged"
        Me.chkPaged.Size = New System.Drawing.Size(93, 17)
        Me.chkPaged.TabIndex = 1
        Me.chkPaged.Text = "Paged Control"
        Me.chkPaged.UseVisualStyleBackColor = True
        '
        'lblControlType
        '
        Me.lblControlType.AutoSize = True
        Me.lblControlType.Location = New System.Drawing.Point(15, 47)
        Me.lblControlType.Name = "lblControlType"
        Me.lblControlType.Size = New System.Drawing.Size(34, 13)
        Me.lblControlType.TabIndex = 1
        Me.lblControlType.Text = "Type:"
        '
        'lblControlPage
        '
        Me.lblControlPage.AutoSize = True
        Me.lblControlPage.Location = New System.Drawing.Point(15, 199)
        Me.lblControlPage.Name = "lblControlPage"
        Me.lblControlPage.Size = New System.Drawing.Size(111, 13)
        Me.lblControlPage.TabIndex = 12
        Me.lblControlPage.Text = "Active (Current) Page:"
        '
        'nudDevicePage
        '
        Me.nudDevicePage.Enabled = False
        Me.nudDevicePage.Location = New System.Drawing.Point(133, 197)
        Me.nudDevicePage.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.nudDevicePage.Name = "nudDevicePage"
        Me.nudDevicePage.Size = New System.Drawing.Size(63, 20)
        Me.nudDevicePage.TabIndex = 2
        '
        'frmEvents
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(757, 459)
        Me.Controls.Add(Me.grpActions)
        Me.Controls.Add(Me.grpAction)
        Me.Controls.Add(Me.grpConfiguration)
        Me.Controls.Add(Me.grpInput)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmEvents"
        Me.Text = "Feel: Edit Events"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.grpConfiguration.ResumeLayout(False)
        Me.grpConfiguration.PerformLayout()
        Me.grpControlBehavior.ResumeLayout(False)
        Me.grpControlBehavior.PerformLayout()
        CType(Me.nudControlGroup, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAction.ResumeLayout(False)
        Me.grpAction.PerformLayout()
        Me.grpActions.ResumeLayout(False)
        Me.grpInput.ResumeLayout(False)
        Me.grpInput.PerformLayout()
        Me.grpInputValues.ResumeLayout(False)
        Me.grpInputValues.PerformLayout()
        CType(Me.nudDevicePage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblVelocityValue As System.Windows.Forms.Label
    Friend WithEvents lblNoteControl As System.Windows.Forms.Label
    Friend WithEvents lblChannel As System.Windows.Forms.Label
    Friend WithEvents lblVelVal As System.Windows.Forms.Label
    Friend WithEvents lblNotCon As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents lblDevice As System.Windows.Forms.Label
    Friend WithEvents grpInputValues As System.Windows.Forms.GroupBox
    Friend WithEvents lblChan As System.Windows.Forms.Label
    Friend WithEvents lblControlDevice As System.Windows.Forms.Label
    Friend WithEvents lblControlType As System.Windows.Forms.Label
    Friend WithEvents grpConfiguration As System.Windows.Forms.GroupBox
    Friend WithEvents nudControlGroup As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblControlGroup As System.Windows.Forms.Label
    Friend WithEvents grpControlBehavior As System.Windows.Forms.GroupBox
    Friend WithEvents rdoLatchRelative As System.Windows.Forms.RadioButton
    Friend WithEvents rdoMomentaryAbsolute As System.Windows.Forms.RadioButton
    Friend WithEvents lblControlPage As System.Windows.Forms.Label
    Friend WithEvents grpAction As System.Windows.Forms.GroupBox
    Friend WithEvents txtActionName As System.Windows.Forms.TextBox
    Friend WithEvents lblActionName As System.Windows.Forms.Label
    Friend WithEvents nudDevicePage As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblActionFunction As System.Windows.Forms.Label
    Friend WithEvents pgAction As System.Windows.Forms.PropertyGrid
    Friend WithEvents ttActions As System.Windows.Forms.ToolTip
    Friend WithEvents cboActionFunction As Feel.GroupedComboBox
    Friend WithEvents chkPaged As System.Windows.Forms.CheckBox
    Friend WithEvents txtInitialState As System.Windows.Forms.TextBox
    Friend WithEvents lblInitialState As System.Windows.Forms.Label
    Friend WithEvents txtActionDescription As System.Windows.Forms.TextBox
    Friend WithEvents grpActions As System.Windows.Forms.GroupBox
    Friend WithEvents cmdActionSwap As System.Windows.Forms.Button
    Friend WithEvents cmdActionRemove As System.Windows.Forms.Button
    Friend WithEvents cmdActionAdd As System.Windows.Forms.Button
    Friend WithEvents cmdActionDown As System.Windows.Forms.Button
    Friend WithEvents cmdActionUp As System.Windows.Forms.Button
    Friend WithEvents cmdActionClear As System.Windows.Forms.Button
    Friend WithEvents lvActions As System.Windows.Forms.ListView
    Friend WithEvents colAction As System.Windows.Forms.ColumnHeader
    Friend WithEvents grpInput As Feel.CheckedGroupBox
    Friend WithEvents txtDefaultState As System.Windows.Forms.TextBox
    Friend WithEvents lblDefaultState As System.Windows.Forms.Label
    Friend WithEvents cmdEditDefaultState As System.Windows.Forms.Button
    Friend WithEvents cmdEditInitalState As System.Windows.Forms.Button
End Class
