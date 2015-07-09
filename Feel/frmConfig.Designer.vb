<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfig
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
            main.configForm = Nothing
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Lighting APC40")
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Lighting APC20")
        Dim ListViewItem3 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("LightJockey")
        Dim ListViewItem4 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Eric's iPad")
        Dim ListViewItem5 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Justin's iPhone")
        Dim ListViewItem6 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Chris' iPad")
        Dim ListViewItem7 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Video APC20")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfig))
        Me.btnAddConnection = New System.Windows.Forms.Button
        Me.btnRemoveConnection = New System.Windows.Forms.Button
        Me.lvConnections = New System.Windows.Forms.ListView
        Me.grpConnectionDetails = New System.Windows.Forms.GroupBox
        Me.chkConnectionNoteOff = New System.Windows.Forms.CheckBox
        Me.lblConnectionBehavior = New System.Windows.Forms.Label
        Me.txtConnectionInitialization = New System.Windows.Forms.TextBox
        Me.lblConnectionInitialization = New System.Windows.Forms.Label
        Me.cboConnectionOutput = New System.Windows.Forms.ComboBox
        Me.chkConnectionOutputEnabled = New System.Windows.Forms.CheckBox
        Me.lblConnectionOutput = New System.Windows.Forms.Label
        Me.cboConnectionInput = New System.Windows.Forms.ComboBox
        Me.chkConnectionInputEnabled = New System.Windows.Forms.CheckBox
        Me.lblConnectionInput = New System.Windows.Forms.Label
        Me.txtConnectionName = New System.Windows.Forms.TextBox
        Me.lblConnectionName = New System.Windows.Forms.Label
        Me.grpNonMIDI = New System.Windows.Forms.GroupBox
        Me.cboFingersPort = New System.Windows.Forms.ComboBox
        Me.chkDmxover = New System.Windows.Forms.CheckBox
        Me.chkDmxin = New System.Windows.Forms.CheckBox
        Me.chkFingers = New System.Windows.Forms.CheckBox
        Me.chkWindowsMessages = New System.Windows.Forms.CheckBox
        Me.grpConnectionDetails.SuspendLayout()
        Me.grpNonMIDI.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAddConnection
        '
        Me.btnAddConnection.Location = New System.Drawing.Point(12, 12)
        Me.btnAddConnection.Name = "btnAddConnection"
        Me.btnAddConnection.Size = New System.Drawing.Size(116, 23)
        Me.btnAddConnection.TabIndex = 0
        Me.btnAddConnection.Text = "Add Connection"
        Me.btnAddConnection.UseVisualStyleBackColor = True
        '
        'btnRemoveConnection
        '
        Me.btnRemoveConnection.Location = New System.Drawing.Point(134, 12)
        Me.btnRemoveConnection.Name = "btnRemoveConnection"
        Me.btnRemoveConnection.Size = New System.Drawing.Size(116, 23)
        Me.btnRemoveConnection.TabIndex = 1
        Me.btnRemoveConnection.Text = "Remove Connection"
        Me.btnRemoveConnection.UseVisualStyleBackColor = True
        '
        'lvConnections
        '
        Me.lvConnections.CheckBoxes = True
        ListViewItem1.StateImageIndex = 0
        ListViewItem2.StateImageIndex = 0
        ListViewItem3.StateImageIndex = 0
        ListViewItem4.StateImageIndex = 0
        ListViewItem5.StateImageIndex = 0
        ListViewItem6.StateImageIndex = 0
        ListViewItem7.StateImageIndex = 0
        Me.lvConnections.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2, ListViewItem3, ListViewItem4, ListViewItem5, ListViewItem6, ListViewItem7})
        Me.lvConnections.Location = New System.Drawing.Point(12, 41)
        Me.lvConnections.MultiSelect = False
        Me.lvConnections.Name = "lvConnections"
        Me.lvConnections.Size = New System.Drawing.Size(238, 234)
        Me.lvConnections.TabIndex = 2
        Me.lvConnections.UseCompatibleStateImageBehavior = False
        Me.lvConnections.View = System.Windows.Forms.View.List
        '
        'grpConnectionDetails
        '
        Me.grpConnectionDetails.Controls.Add(Me.chkConnectionNoteOff)
        Me.grpConnectionDetails.Controls.Add(Me.lblConnectionBehavior)
        Me.grpConnectionDetails.Controls.Add(Me.txtConnectionInitialization)
        Me.grpConnectionDetails.Controls.Add(Me.lblConnectionInitialization)
        Me.grpConnectionDetails.Controls.Add(Me.cboConnectionOutput)
        Me.grpConnectionDetails.Controls.Add(Me.chkConnectionOutputEnabled)
        Me.grpConnectionDetails.Controls.Add(Me.lblConnectionOutput)
        Me.grpConnectionDetails.Controls.Add(Me.cboConnectionInput)
        Me.grpConnectionDetails.Controls.Add(Me.chkConnectionInputEnabled)
        Me.grpConnectionDetails.Controls.Add(Me.lblConnectionInput)
        Me.grpConnectionDetails.Controls.Add(Me.txtConnectionName)
        Me.grpConnectionDetails.Controls.Add(Me.lblConnectionName)
        Me.grpConnectionDetails.Location = New System.Drawing.Point(256, 12)
        Me.grpConnectionDetails.Name = "grpConnectionDetails"
        Me.grpConnectionDetails.Size = New System.Drawing.Size(295, 263)
        Me.grpConnectionDetails.TabIndex = 4
        Me.grpConnectionDetails.TabStop = False
        Me.grpConnectionDetails.Text = "Connection Details"
        '
        'chkConnectionNoteOff
        '
        Me.chkConnectionNoteOff.AutoSize = True
        Me.chkConnectionNoteOff.Location = New System.Drawing.Point(85, 237)
        Me.chkConnectionNoteOff.Name = "chkConnectionNoteOff"
        Me.chkConnectionNoteOff.Size = New System.Drawing.Size(196, 17)
        Me.chkConnectionNoteOff.TabIndex = 9
        Me.chkConnectionNoteOff.Text = """Note On, Velocity 0"" == ""Note Off"""
        Me.chkConnectionNoteOff.UseVisualStyleBackColor = True
        '
        'lblConnectionBehavior
        '
        Me.lblConnectionBehavior.AutoSize = True
        Me.lblConnectionBehavior.Location = New System.Drawing.Point(15, 238)
        Me.lblConnectionBehavior.Name = "lblConnectionBehavior"
        Me.lblConnectionBehavior.Size = New System.Drawing.Size(52, 13)
        Me.lblConnectionBehavior.TabIndex = 10
        Me.lblConnectionBehavior.Text = "Behavior:"
        '
        'txtConnectionInitialization
        '
        Me.txtConnectionInitialization.AcceptsReturn = True
        Me.txtConnectionInitialization.Location = New System.Drawing.Point(85, 152)
        Me.txtConnectionInitialization.Multiline = True
        Me.txtConnectionInitialization.Name = "txtConnectionInitialization"
        Me.txtConnectionInitialization.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtConnectionInitialization.Size = New System.Drawing.Size(193, 79)
        Me.txtConnectionInitialization.TabIndex = 8
        Me.txtConnectionInitialization.Text = resources.GetString("txtConnectionInitialization.Text")
        Me.txtConnectionInitialization.WordWrap = False
        '
        'lblConnectionInitialization
        '
        Me.lblConnectionInitialization.AutoSize = True
        Me.lblConnectionInitialization.Location = New System.Drawing.Point(15, 155)
        Me.lblConnectionInitialization.Name = "lblConnectionInitialization"
        Me.lblConnectionInitialization.Size = New System.Drawing.Size(64, 13)
        Me.lblConnectionInitialization.TabIndex = 8
        Me.lblConnectionInitialization.Text = "Initialization:"
        '
        'cboConnectionOutput
        '
        Me.cboConnectionOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboConnectionOutput.FormattingEnabled = True
        Me.cboConnectionOutput.Location = New System.Drawing.Point(85, 125)
        Me.cboConnectionOutput.Name = "cboConnectionOutput"
        Me.cboConnectionOutput.Size = New System.Drawing.Size(193, 21)
        Me.cboConnectionOutput.TabIndex = 7
        '
        'chkConnectionOutputEnabled
        '
        Me.chkConnectionOutputEnabled.AutoSize = True
        Me.chkConnectionOutputEnabled.Location = New System.Drawing.Point(85, 102)
        Me.chkConnectionOutputEnabled.Name = "chkConnectionOutputEnabled"
        Me.chkConnectionOutputEnabled.Size = New System.Drawing.Size(65, 17)
        Me.chkConnectionOutputEnabled.TabIndex = 6
        Me.chkConnectionOutputEnabled.Text = "Enabled"
        Me.chkConnectionOutputEnabled.UseVisualStyleBackColor = True
        '
        'lblConnectionOutput
        '
        Me.lblConnectionOutput.AutoSize = True
        Me.lblConnectionOutput.Location = New System.Drawing.Point(15, 103)
        Me.lblConnectionOutput.Name = "lblConnectionOutput"
        Me.lblConnectionOutput.Size = New System.Drawing.Size(42, 13)
        Me.lblConnectionOutput.TabIndex = 5
        Me.lblConnectionOutput.Text = "Output:"
        '
        'cboConnectionInput
        '
        Me.cboConnectionInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboConnectionInput.FormattingEnabled = True
        Me.cboConnectionInput.Location = New System.Drawing.Point(85, 75)
        Me.cboConnectionInput.Name = "cboConnectionInput"
        Me.cboConnectionInput.Size = New System.Drawing.Size(193, 21)
        Me.cboConnectionInput.TabIndex = 5
        '
        'chkConnectionInputEnabled
        '
        Me.chkConnectionInputEnabled.AutoSize = True
        Me.chkConnectionInputEnabled.Location = New System.Drawing.Point(85, 52)
        Me.chkConnectionInputEnabled.Name = "chkConnectionInputEnabled"
        Me.chkConnectionInputEnabled.Size = New System.Drawing.Size(65, 17)
        Me.chkConnectionInputEnabled.TabIndex = 4
        Me.chkConnectionInputEnabled.Text = "Enabled"
        Me.chkConnectionInputEnabled.UseVisualStyleBackColor = True
        '
        'lblConnectionInput
        '
        Me.lblConnectionInput.AutoSize = True
        Me.lblConnectionInput.Location = New System.Drawing.Point(15, 53)
        Me.lblConnectionInput.Name = "lblConnectionInput"
        Me.lblConnectionInput.Size = New System.Drawing.Size(34, 13)
        Me.lblConnectionInput.TabIndex = 2
        Me.lblConnectionInput.Text = "Input:"
        '
        'txtConnectionName
        '
        Me.txtConnectionName.Location = New System.Drawing.Point(85, 26)
        Me.txtConnectionName.Name = "txtConnectionName"
        Me.txtConnectionName.Size = New System.Drawing.Size(193, 20)
        Me.txtConnectionName.TabIndex = 3
        Me.txtConnectionName.Text = "Lighting APC40"
        '
        'lblConnectionName
        '
        Me.lblConnectionName.AutoSize = True
        Me.lblConnectionName.Location = New System.Drawing.Point(15, 29)
        Me.lblConnectionName.Name = "lblConnectionName"
        Me.lblConnectionName.Size = New System.Drawing.Size(38, 13)
        Me.lblConnectionName.TabIndex = 0
        Me.lblConnectionName.Text = "Name:"
        '
        'grpNonMIDI
        '
        Me.grpNonMIDI.Controls.Add(Me.cboFingersPort)
        Me.grpNonMIDI.Controls.Add(Me.chkDmxover)
        Me.grpNonMIDI.Controls.Add(Me.chkDmxin)
        Me.grpNonMIDI.Controls.Add(Me.chkFingers)
        Me.grpNonMIDI.Controls.Add(Me.chkWindowsMessages)
        Me.grpNonMIDI.Location = New System.Drawing.Point(12, 281)
        Me.grpNonMIDI.Name = "grpNonMIDI"
        Me.grpNonMIDI.Size = New System.Drawing.Size(539, 69)
        Me.grpNonMIDI.TabIndex = 5
        Me.grpNonMIDI.TabStop = False
        Me.grpNonMIDI.Text = "Non-MIDI Connections"
        '
        'cboFingersPort
        '
        Me.cboFingersPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFingersPort.FormattingEnabled = True
        Me.cboFingersPort.Items.AddRange(New Object() {"None", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9"})
        Me.cboFingersPort.Location = New System.Drawing.Point(450, 17)
        Me.cboFingersPort.Name = "cboFingersPort"
        Me.cboFingersPort.Size = New System.Drawing.Size(72, 21)
        Me.cboFingersPort.TabIndex = 13
        '
        'chkDmxover
        '
        Me.chkDmxover.AutoSize = True
        Me.chkDmxover.Enabled = False
        Me.chkDmxover.Location = New System.Drawing.Point(262, 42)
        Me.chkDmxover.Name = "chkDmxover"
        Me.chkDmxover.Size = New System.Drawing.Size(129, 17)
        Me.chkDmxover.TabIndex = 14
        Me.chkDmxover.Text = "Enable DMX-Override"
        Me.chkDmxover.UseVisualStyleBackColor = True
        '
        'chkDmxin
        '
        Me.chkDmxin.AutoSize = True
        Me.chkDmxin.Enabled = False
        Me.chkDmxin.Location = New System.Drawing.Point(15, 42)
        Me.chkDmxin.Name = "chkDmxin"
        Me.chkDmxin.Size = New System.Drawing.Size(176, 17)
        Me.chkDmxin.TabIndex = 11
        Me.chkDmxin.Text = "Enable DMX-In (to LightJockey)"
        Me.chkDmxin.UseVisualStyleBackColor = True
        '
        'chkFingers
        '
        Me.chkFingers.AutoSize = True
        Me.chkFingers.Enabled = False
        Me.chkFingers.Location = New System.Drawing.Point(262, 19)
        Me.chkFingers.Name = "chkFingers"
        Me.chkFingers.Size = New System.Drawing.Size(145, 17)
        Me.chkFingers.TabIndex = 12
        Me.chkFingers.Text = "Enable Fingers Emulation"
        Me.chkFingers.UseVisualStyleBackColor = True
        '
        'chkWindowsMessages
        '
        Me.chkWindowsMessages.AutoSize = True
        Me.chkWindowsMessages.Location = New System.Drawing.Point(15, 19)
        Me.chkWindowsMessages.Name = "chkWindowsMessages"
        Me.chkWindowsMessages.Size = New System.Drawing.Size(229, 17)
        Me.chkWindowsMessages.TabIndex = 10
        Me.chkWindowsMessages.Text = "Enable Windows Messages to LightJockey"
        Me.chkWindowsMessages.UseVisualStyleBackColor = True
        '
        'frmConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(563, 362)
        Me.Controls.Add(Me.grpNonMIDI)
        Me.Controls.Add(Me.grpConnectionDetails)
        Me.Controls.Add(Me.lvConnections)
        Me.Controls.Add(Me.btnRemoveConnection)
        Me.Controls.Add(Me.btnAddConnection)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmConfig"
        Me.Text = "Feel: Connections"
        Me.grpConnectionDetails.ResumeLayout(False)
        Me.grpConnectionDetails.PerformLayout()
        Me.grpNonMIDI.ResumeLayout(False)
        Me.grpNonMIDI.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAddConnection As System.Windows.Forms.Button
    Friend WithEvents btnRemoveConnection As System.Windows.Forms.Button
    Friend WithEvents lvConnections As System.Windows.Forms.ListView
    Friend WithEvents grpConnectionDetails As System.Windows.Forms.GroupBox
    Friend WithEvents cboConnectionOutput As System.Windows.Forms.ComboBox
    Friend WithEvents chkConnectionOutputEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents lblConnectionOutput As System.Windows.Forms.Label
    Friend WithEvents cboConnectionInput As System.Windows.Forms.ComboBox
    Friend WithEvents chkConnectionInputEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents lblConnectionInput As System.Windows.Forms.Label
    Friend WithEvents txtConnectionName As System.Windows.Forms.TextBox
    Friend WithEvents lblConnectionName As System.Windows.Forms.Label
    Friend WithEvents txtConnectionInitialization As System.Windows.Forms.TextBox
    Friend WithEvents lblConnectionInitialization As System.Windows.Forms.Label
    Friend WithEvents grpNonMIDI As System.Windows.Forms.GroupBox
    Friend WithEvents chkDmxover As System.Windows.Forms.CheckBox
    Friend WithEvents chkDmxin As System.Windows.Forms.CheckBox
    Friend WithEvents chkFingers As System.Windows.Forms.CheckBox
    Friend WithEvents chkWindowsMessages As System.Windows.Forms.CheckBox
    Friend WithEvents cboFingersPort As System.Windows.Forms.ComboBox
    Friend WithEvents chkConnectionNoteOff As System.Windows.Forms.CheckBox
    Friend WithEvents lblConnectionBehavior As System.Windows.Forms.Label

End Class
