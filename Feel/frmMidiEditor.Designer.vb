<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMidiEditor
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
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblMessageType = New System.Windows.Forms.Label
        Me.cboMessageType = New System.Windows.Forms.ComboBox
        Me.txtMidiMessage = New System.Windows.Forms.TextBox
        Me.lblMidiMessage = New System.Windows.Forms.Label
        Me.cmdInsert = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.grpNotCon = New System.Windows.Forms.GroupBox
        Me.lblNotConNotCon = New System.Windows.Forms.Label
        Me.cboNotConNotCon = New System.Windows.Forms.ComboBox
        Me.nudNotConVelVal = New System.Windows.Forms.NumericUpDown
        Me.lblNotConVelVal = New System.Windows.Forms.Label
        Me.cboNotConChannel = New System.Windows.Forms.ComboBox
        Me.lblNotConChannel = New System.Windows.Forms.Label
        Me.grpProgramChange = New System.Windows.Forms.GroupBox
        Me.nudProgramInstrument = New System.Windows.Forms.NumericUpDown
        Me.lblProgramInstrument = New System.Windows.Forms.Label
        Me.grpSysex = New System.Windows.Forms.GroupBox
        Me.txtSysexData = New System.Windows.Forms.TextBox
        Me.lblSysexData = New System.Windows.Forms.Label
        Me.grpPitchBend = New System.Windows.Forms.GroupBox
        Me.nudPitchBendValue = New System.Windows.Forms.NumericUpDown
        Me.lblPitchBendValue = New System.Windows.Forms.Label
        Me.cboPitchBendChannel = New System.Windows.Forms.ComboBox
        Me.lblPitchBendChannel = New System.Windows.Forms.Label
        Me.cboProgramChangeChannel = New System.Windows.Forms.ComboBox
        Me.lblProgramChangeChannel = New System.Windows.Forms.Label
        Me.grpNotCon.SuspendLayout()
        CType(Me.nudNotConVelVal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpProgramChange.SuspendLayout()
        CType(Me.nudProgramInstrument, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSysex.SuspendLayout()
        Me.grpPitchBend.SuspendLayout()
        CType(Me.nudPitchBendValue, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblMessageType
        '
        Me.lblMessageType.AutoSize = True
        Me.lblMessageType.Location = New System.Drawing.Point(12, 9)
        Me.lblMessageType.Name = "lblMessageType"
        Me.lblMessageType.Size = New System.Drawing.Size(80, 13)
        Me.lblMessageType.TabIndex = 0
        Me.lblMessageType.Text = "Message Type:"
        '
        'cboMessageType
        '
        Me.cboMessageType.FormattingEnabled = True
        Me.cboMessageType.Items.AddRange(New Object() {"None", "Note Off (8X)", "Note On (9X)", "Control Change (BX)", "Program Change (CX)", "Pitch Bend (EX)", "System Exclusive (F0)"})
        Me.cboMessageType.Location = New System.Drawing.Point(98, 6)
        Me.cboMessageType.Name = "cboMessageType"
        Me.cboMessageType.Size = New System.Drawing.Size(182, 21)
        Me.cboMessageType.TabIndex = 0
        '
        'txtMidiMessage
        '
        Me.txtMidiMessage.Location = New System.Drawing.Point(98, 141)
        Me.txtMidiMessage.Name = "txtMidiMessage"
        Me.txtMidiMessage.Size = New System.Drawing.Size(182, 20)
        Me.txtMidiMessage.TabIndex = 7
        '
        'lblMidiMessage
        '
        Me.lblMidiMessage.AutoSize = True
        Me.lblMidiMessage.Location = New System.Drawing.Point(12, 144)
        Me.lblMidiMessage.Name = "lblMidiMessage"
        Me.lblMidiMessage.Size = New System.Drawing.Size(79, 13)
        Me.lblMidiMessage.TabIndex = 3
        Me.lblMidiMessage.Text = "MIDI Message:"
        '
        'cmdInsert
        '
        Me.cmdInsert.Location = New System.Drawing.Point(205, 167)
        Me.cmdInsert.Name = "cmdInsert"
        Me.cmdInsert.Size = New System.Drawing.Size(75, 23)
        Me.cmdInsert.TabIndex = 9
        Me.cmdInsert.Text = "Insert"
        Me.cmdInsert.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Location = New System.Drawing.Point(15, 167)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 23)
        Me.cmdCancel.TabIndex = 8
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'grpNotCon
        '
        Me.grpNotCon.Controls.Add(Me.lblNotConNotCon)
        Me.grpNotCon.Controls.Add(Me.cboNotConNotCon)
        Me.grpNotCon.Controls.Add(Me.nudNotConVelVal)
        Me.grpNotCon.Controls.Add(Me.lblNotConVelVal)
        Me.grpNotCon.Controls.Add(Me.cboNotConChannel)
        Me.grpNotCon.Controls.Add(Me.lblNotConChannel)
        Me.grpNotCon.Location = New System.Drawing.Point(15, 33)
        Me.grpNotCon.Name = "grpNotCon"
        Me.grpNotCon.Size = New System.Drawing.Size(265, 102)
        Me.grpNotCon.TabIndex = 10
        Me.grpNotCon.TabStop = False
        Me.grpNotCon.Text = "Note Off Properties"
        '
        'lblNotConNotCon
        '
        Me.lblNotConNotCon.AutoSize = True
        Me.lblNotConNotCon.Location = New System.Drawing.Point(16, 52)
        Me.lblNotConNotCon.Name = "lblNotConNotCon"
        Me.lblNotConNotCon.Size = New System.Drawing.Size(33, 13)
        Me.lblNotConNotCon.TabIndex = 15
        Me.lblNotConNotCon.Text = "Note:"
        '
        'cboNotConNotCon
        '
        Me.cboNotConNotCon.FormattingEnabled = True
        Me.cboNotConNotCon.Location = New System.Drawing.Point(138, 49)
        Me.cboNotConNotCon.Name = "cboNotConNotCon"
        Me.cboNotConNotCon.Size = New System.Drawing.Size(121, 21)
        Me.cboNotConNotCon.TabIndex = 2
        '
        'nudNotConVelVal
        '
        Me.nudNotConVelVal.Location = New System.Drawing.Point(138, 76)
        Me.nudNotConVelVal.Maximum = New Decimal(New Integer() {127, 0, 0, 0})
        Me.nudNotConVelVal.Name = "nudNotConVelVal"
        Me.nudNotConVelVal.Size = New System.Drawing.Size(121, 20)
        Me.nudNotConVelVal.TabIndex = 3
        '
        'lblNotConVelVal
        '
        Me.lblNotConVelVal.AutoSize = True
        Me.lblNotConVelVal.Location = New System.Drawing.Point(16, 78)
        Me.lblNotConVelVal.Name = "lblNotConVelVal"
        Me.lblNotConVelVal.Size = New System.Drawing.Size(47, 13)
        Me.lblNotConVelVal.TabIndex = 12
        Me.lblNotConVelVal.Text = "Velocity:"
        '
        'cboNotConChannel
        '
        Me.cboNotConChannel.FormattingEnabled = True
        Me.cboNotConChannel.Location = New System.Drawing.Point(138, 22)
        Me.cboNotConChannel.Name = "cboNotConChannel"
        Me.cboNotConChannel.Size = New System.Drawing.Size(121, 21)
        Me.cboNotConChannel.TabIndex = 1
        '
        'lblNotConChannel
        '
        Me.lblNotConChannel.AutoSize = True
        Me.lblNotConChannel.Location = New System.Drawing.Point(16, 25)
        Me.lblNotConChannel.Name = "lblNotConChannel"
        Me.lblNotConChannel.Size = New System.Drawing.Size(49, 13)
        Me.lblNotConChannel.TabIndex = 10
        Me.lblNotConChannel.Text = "Channel:"
        '
        'grpProgramChange
        '
        Me.grpProgramChange.Controls.Add(Me.cboProgramChangeChannel)
        Me.grpProgramChange.Controls.Add(Me.lblProgramChangeChannel)
        Me.grpProgramChange.Controls.Add(Me.nudProgramInstrument)
        Me.grpProgramChange.Controls.Add(Me.lblProgramInstrument)
        Me.grpProgramChange.Location = New System.Drawing.Point(286, 6)
        Me.grpProgramChange.Name = "grpProgramChange"
        Me.grpProgramChange.Size = New System.Drawing.Size(265, 102)
        Me.grpProgramChange.TabIndex = 11
        Me.grpProgramChange.TabStop = False
        Me.grpProgramChange.Text = "Program Change Properties"
        '
        'nudProgramInstrument
        '
        Me.nudProgramInstrument.Location = New System.Drawing.Point(138, 50)
        Me.nudProgramInstrument.Maximum = New Decimal(New Integer() {127, 0, 0, 0})
        Me.nudProgramInstrument.Name = "nudProgramInstrument"
        Me.nudProgramInstrument.Size = New System.Drawing.Size(121, 20)
        Me.nudProgramInstrument.TabIndex = 5
        '
        'lblProgramInstrument
        '
        Me.lblProgramInstrument.AutoSize = True
        Me.lblProgramInstrument.Location = New System.Drawing.Point(16, 52)
        Me.lblProgramInstrument.Name = "lblProgramInstrument"
        Me.lblProgramInstrument.Size = New System.Drawing.Size(103, 13)
        Me.lblProgramInstrument.TabIndex = 12
        Me.lblProgramInstrument.Text = "Program/Instrument:"
        '
        'grpSysex
        '
        Me.grpSysex.Controls.Add(Me.txtSysexData)
        Me.grpSysex.Controls.Add(Me.lblSysexData)
        Me.grpSysex.Location = New System.Drawing.Point(286, 222)
        Me.grpSysex.Name = "grpSysex"
        Me.grpSysex.Size = New System.Drawing.Size(265, 102)
        Me.grpSysex.TabIndex = 14
        Me.grpSysex.TabStop = False
        Me.grpSysex.Text = "System Exclusive Properties"
        '
        'txtSysexData
        '
        Me.txtSysexData.Location = New System.Drawing.Point(55, 22)
        Me.txtSysexData.Multiline = True
        Me.txtSysexData.Name = "txtSysexData"
        Me.txtSysexData.Size = New System.Drawing.Size(204, 74)
        Me.txtSysexData.TabIndex = 6
        '
        'lblSysexData
        '
        Me.lblSysexData.AutoSize = True
        Me.lblSysexData.Location = New System.Drawing.Point(16, 25)
        Me.lblSysexData.Name = "lblSysexData"
        Me.lblSysexData.Size = New System.Drawing.Size(33, 13)
        Me.lblSysexData.TabIndex = 10
        Me.lblSysexData.Text = "Data:"
        '
        'grpPitchBend
        '
        Me.grpPitchBend.Controls.Add(Me.nudPitchBendValue)
        Me.grpPitchBend.Controls.Add(Me.lblPitchBendValue)
        Me.grpPitchBend.Controls.Add(Me.cboPitchBendChannel)
        Me.grpPitchBend.Controls.Add(Me.lblPitchBendChannel)
        Me.grpPitchBend.Location = New System.Drawing.Point(286, 114)
        Me.grpPitchBend.Name = "grpPitchBend"
        Me.grpPitchBend.Size = New System.Drawing.Size(265, 102)
        Me.grpPitchBend.TabIndex = 15
        Me.grpPitchBend.TabStop = False
        Me.grpPitchBend.Text = "Pitch Bend Properties"
        '
        'nudPitchBendValue
        '
        Me.nudPitchBendValue.Location = New System.Drawing.Point(138, 49)
        Me.nudPitchBendValue.Maximum = New Decimal(New Integer() {16383, 0, 0, 0})
        Me.nudPitchBendValue.Name = "nudPitchBendValue"
        Me.nudPitchBendValue.Size = New System.Drawing.Size(121, 20)
        Me.nudPitchBendValue.TabIndex = 5
        Me.nudPitchBendValue.Value = New Decimal(New Integer() {8192, 0, 0, 0})
        '
        'lblPitchBendValue
        '
        Me.lblPitchBendValue.AutoSize = True
        Me.lblPitchBendValue.Location = New System.Drawing.Point(16, 52)
        Me.lblPitchBendValue.Name = "lblPitchBendValue"
        Me.lblPitchBendValue.Size = New System.Drawing.Size(92, 13)
        Me.lblPitchBendValue.TabIndex = 12
        Me.lblPitchBendValue.Text = "Pitch Bend Value:"
        '
        'cboPitchBendChannel
        '
        Me.cboPitchBendChannel.FormattingEnabled = True
        Me.cboPitchBendChannel.Location = New System.Drawing.Point(138, 22)
        Me.cboPitchBendChannel.Name = "cboPitchBendChannel"
        Me.cboPitchBendChannel.Size = New System.Drawing.Size(121, 21)
        Me.cboPitchBendChannel.TabIndex = 4
        '
        'lblPitchBendChannel
        '
        Me.lblPitchBendChannel.AutoSize = True
        Me.lblPitchBendChannel.Location = New System.Drawing.Point(16, 25)
        Me.lblPitchBendChannel.Name = "lblPitchBendChannel"
        Me.lblPitchBendChannel.Size = New System.Drawing.Size(49, 13)
        Me.lblPitchBendChannel.TabIndex = 10
        Me.lblPitchBendChannel.Text = "Channel:"
        '
        'cboProgramChangeChannel
        '
        Me.cboProgramChangeChannel.FormattingEnabled = True
        Me.cboProgramChangeChannel.Location = New System.Drawing.Point(138, 22)
        Me.cboProgramChangeChannel.Name = "cboProgramChangeChannel"
        Me.cboProgramChangeChannel.Size = New System.Drawing.Size(121, 21)
        Me.cboProgramChangeChannel.TabIndex = 13
        '
        'lblProgramChangeChannel
        '
        Me.lblProgramChangeChannel.AutoSize = True
        Me.lblProgramChangeChannel.Location = New System.Drawing.Point(16, 25)
        Me.lblProgramChangeChannel.Name = "lblProgramChangeChannel"
        Me.lblProgramChangeChannel.Size = New System.Drawing.Size(49, 13)
        Me.lblProgramChangeChannel.TabIndex = 14
        Me.lblProgramChangeChannel.Text = "Channel:"
        '
        'frmMidiEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(576, 426)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpPitchBend)
        Me.Controls.Add(Me.grpSysex)
        Me.Controls.Add(Me.grpNotCon)
        Me.Controls.Add(Me.grpProgramChange)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdInsert)
        Me.Controls.Add(Me.lblMidiMessage)
        Me.Controls.Add(Me.txtMidiMessage)
        Me.Controls.Add(Me.cboMessageType)
        Me.Controls.Add(Me.lblMessageType)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmMidiEditor"
        Me.Text = "MIDI Message Editor"
        Me.grpNotCon.ResumeLayout(False)
        Me.grpNotCon.PerformLayout()
        CType(Me.nudNotConVelVal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpProgramChange.ResumeLayout(False)
        Me.grpProgramChange.PerformLayout()
        CType(Me.nudProgramInstrument, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSysex.ResumeLayout(False)
        Me.grpSysex.PerformLayout()
        Me.grpPitchBend.ResumeLayout(False)
        Me.grpPitchBend.PerformLayout()
        CType(Me.nudPitchBendValue, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblMessageType As System.Windows.Forms.Label
    Friend WithEvents cboMessageType As System.Windows.Forms.ComboBox
    Friend WithEvents txtMidiMessage As System.Windows.Forms.TextBox
    Friend WithEvents lblMidiMessage As System.Windows.Forms.Label
    Friend WithEvents cmdInsert As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents grpNotCon As System.Windows.Forms.GroupBox
    Friend WithEvents nudNotConVelVal As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblNotConVelVal As System.Windows.Forms.Label
    Friend WithEvents cboNotConChannel As System.Windows.Forms.ComboBox
    Friend WithEvents lblNotConChannel As System.Windows.Forms.Label
    Friend WithEvents grpProgramChange As System.Windows.Forms.GroupBox
    Friend WithEvents nudProgramInstrument As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblProgramInstrument As System.Windows.Forms.Label
    Friend WithEvents grpSysex As System.Windows.Forms.GroupBox
    Friend WithEvents lblSysexData As System.Windows.Forms.Label
    Friend WithEvents txtSysexData As System.Windows.Forms.TextBox
    Friend WithEvents lblNotConNotCon As System.Windows.Forms.Label
    Friend WithEvents cboNotConNotCon As System.Windows.Forms.ComboBox
    Friend WithEvents grpPitchBend As System.Windows.Forms.GroupBox
    Friend WithEvents nudPitchBendValue As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblPitchBendValue As System.Windows.Forms.Label
    Friend WithEvents cboPitchBendChannel As System.Windows.Forms.ComboBox
    Friend WithEvents lblPitchBendChannel As System.Windows.Forms.Label
    Friend WithEvents cboProgramChangeChannel As System.Windows.Forms.ComboBox
    Friend WithEvents lblProgramChangeChannel As System.Windows.Forms.Label
End Class
