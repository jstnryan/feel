<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfiguration
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfiguration))
        Me.grpMidiDisplay = New System.Windows.Forms.GroupBox
        Me.grpTranspose = New System.Windows.Forms.GroupBox
        Me.rdoTransposeC3 = New System.Windows.Forms.RadioButton
        Me.rdoTransposeC4 = New System.Windows.Forms.RadioButton
        Me.rdoTransposeC5 = New System.Windows.Forms.RadioButton
        Me.grpNumbering = New System.Windows.Forms.GroupBox
        Me.rdoNumbering1 = New System.Windows.Forms.RadioButton
        Me.rdoNumbering0 = New System.Windows.Forms.RadioButton
        Me.grpNotation = New System.Windows.Forms.GroupBox
        Me.rdoNotationDecS = New System.Windows.Forms.RadioButton
        Me.rdoNotationHexS = New System.Windows.Forms.RadioButton
        Me.rdoNotationDec = New System.Windows.Forms.RadioButton
        Me.rdoNotationHexP = New System.Windows.Forms.RadioButton
        Me.rdoNotationNot = New System.Windows.Forms.RadioButton
        Me.grpLJConfig = New System.Windows.Forms.GroupBox
        Me.chkDmxover = New System.Windows.Forms.CheckBox
        Me.chkDmxin = New System.Windows.Forms.CheckBox
        Me.chkWindowsMessages = New System.Windows.Forms.CheckBox
        Me.grpConnection = New System.Windows.Forms.GroupBox
        Me.chkFeedbackDetection = New System.Windows.Forms.CheckBox
        Me.chkIgnoreWhileConnecting = New System.Windows.Forms.CheckBox
        Me.grpConfigFile = New System.Windows.Forms.GroupBox
        Me.cmdFileSaveAs = New System.Windows.Forms.Button
        Me.cmdFileNew = New System.Windows.Forms.Button
        Me.cmdFileOpen = New System.Windows.Forms.Button
        Me.txtConfigFile = New System.Windows.Forms.TextBox
        Me.grpMidiDisplay.SuspendLayout()
        Me.grpTranspose.SuspendLayout()
        Me.grpNumbering.SuspendLayout()
        Me.grpNotation.SuspendLayout()
        Me.grpLJConfig.SuspendLayout()
        Me.grpConnection.SuspendLayout()
        Me.grpConfigFile.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpMidiDisplay
        '
        Me.grpMidiDisplay.Controls.Add(Me.grpTranspose)
        Me.grpMidiDisplay.Controls.Add(Me.grpNumbering)
        Me.grpMidiDisplay.Controls.Add(Me.grpNotation)
        Me.grpMidiDisplay.Location = New System.Drawing.Point(287, 12)
        Me.grpMidiDisplay.Name = "grpMidiDisplay"
        Me.grpMidiDisplay.Size = New System.Drawing.Size(269, 348)
        Me.grpMidiDisplay.TabIndex = 6
        Me.grpMidiDisplay.TabStop = False
        Me.grpMidiDisplay.Text = "MIDI Display Preferences"
        '
        'grpTranspose
        '
        Me.grpTranspose.Controls.Add(Me.rdoTransposeC3)
        Me.grpTranspose.Controls.Add(Me.rdoTransposeC4)
        Me.grpTranspose.Controls.Add(Me.rdoTransposeC5)
        Me.grpTranspose.Location = New System.Drawing.Point(6, 247)
        Me.grpTranspose.Name = "grpTranspose"
        Me.grpTranspose.Size = New System.Drawing.Size(257, 95)
        Me.grpTranspose.TabIndex = 21
        Me.grpTranspose.TabStop = False
        Me.grpTranspose.Text = "Octave Transpose (""Middle C"")"
        '
        'rdoTransposeC3
        '
        Me.rdoTransposeC3.AutoSize = True
        Me.rdoTransposeC3.Location = New System.Drawing.Point(15, 65)
        Me.rdoTransposeC3.Name = "rdoTransposeC3"
        Me.rdoTransposeC3.Size = New System.Drawing.Size(191, 17)
        Me.rdoTransposeC3.TabIndex = 2
        Me.rdoTransposeC3.TabStop = True
        Me.rdoTransposeC3.Text = "C3 (Note 48, Octaves -2 through 8)"
        Me.rdoTransposeC3.UseVisualStyleBackColor = True
        '
        'rdoTransposeC4
        '
        Me.rdoTransposeC4.AutoSize = True
        Me.rdoTransposeC4.Location = New System.Drawing.Point(15, 42)
        Me.rdoTransposeC4.Name = "rdoTransposeC4"
        Me.rdoTransposeC4.Size = New System.Drawing.Size(191, 17)
        Me.rdoTransposeC4.TabIndex = 1
        Me.rdoTransposeC4.TabStop = True
        Me.rdoTransposeC4.Text = "C4 (Note 60, Octaves -1 through 9)"
        Me.rdoTransposeC4.UseVisualStyleBackColor = True
        '
        'rdoTransposeC5
        '
        Me.rdoTransposeC5.AutoSize = True
        Me.rdoTransposeC5.Location = New System.Drawing.Point(15, 19)
        Me.rdoTransposeC5.Name = "rdoTransposeC5"
        Me.rdoTransposeC5.Size = New System.Drawing.Size(194, 17)
        Me.rdoTransposeC5.TabIndex = 0
        Me.rdoTransposeC5.TabStop = True
        Me.rdoTransposeC5.Text = "C5 (Note 72, Octaves 0 through 10)"
        Me.rdoTransposeC5.UseVisualStyleBackColor = True
        '
        'grpNumbering
        '
        Me.grpNumbering.Controls.Add(Me.rdoNumbering1)
        Me.grpNumbering.Controls.Add(Me.rdoNumbering0)
        Me.grpNumbering.Location = New System.Drawing.Point(6, 169)
        Me.grpNumbering.Name = "grpNumbering"
        Me.grpNumbering.Size = New System.Drawing.Size(257, 72)
        Me.grpNumbering.TabIndex = 20
        Me.grpNumbering.TabStop = False
        Me.grpNumbering.Text = "Channel Numbering"
        '
        'rdoNumbering1
        '
        Me.rdoNumbering1.AutoSize = True
        Me.rdoNumbering1.Location = New System.Drawing.Point(15, 42)
        Me.rdoNumbering1.Name = "rdoNumbering1"
        Me.rdoNumbering1.Size = New System.Drawing.Size(155, 17)
        Me.rdoNumbering1.TabIndex = 1
        Me.rdoNumbering1.TabStop = True
        Me.rdoNumbering1.Text = "One Based (Channels 1-16)"
        Me.rdoNumbering1.UseVisualStyleBackColor = True
        '
        'rdoNumbering0
        '
        Me.rdoNumbering0.AutoSize = True
        Me.rdoNumbering0.Location = New System.Drawing.Point(15, 19)
        Me.rdoNumbering0.Name = "rdoNumbering0"
        Me.rdoNumbering0.Size = New System.Drawing.Size(157, 17)
        Me.rdoNumbering0.TabIndex = 0
        Me.rdoNumbering0.TabStop = True
        Me.rdoNumbering0.Text = "Zero Based (Channels 0-15)"
        Me.rdoNumbering0.UseVisualStyleBackColor = True
        '
        'grpNotation
        '
        Me.grpNotation.Controls.Add(Me.rdoNotationDecS)
        Me.grpNotation.Controls.Add(Me.rdoNotationHexS)
        Me.grpNotation.Controls.Add(Me.rdoNotationDec)
        Me.grpNotation.Controls.Add(Me.rdoNotationHexP)
        Me.grpNotation.Controls.Add(Me.rdoNotationNot)
        Me.grpNotation.Location = New System.Drawing.Point(6, 19)
        Me.grpNotation.Name = "grpNotation"
        Me.grpNotation.Size = New System.Drawing.Size(257, 144)
        Me.grpNotation.TabIndex = 19
        Me.grpNotation.TabStop = False
        Me.grpNotation.Text = "Note Notation"
        '
        'rdoNotationDecS
        '
        Me.rdoNotationDecS.AutoSize = True
        Me.rdoNotationDecS.Location = New System.Drawing.Point(15, 65)
        Me.rdoNotationDecS.Name = "rdoNotationDecS"
        Me.rdoNotationDecS.Size = New System.Drawing.Size(90, 17)
        Me.rdoNotationDecS.TabIndex = 23
        Me.rdoNotationDecS.TabStop = True
        Me.rdoNotationDecS.Text = "Decimal (54d)"
        Me.rdoNotationDecS.UseVisualStyleBackColor = True
        '
        'rdoNotationHexS
        '
        Me.rdoNotationHexS.AutoSize = True
        Me.rdoNotationHexS.Location = New System.Drawing.Point(15, 111)
        Me.rdoNotationHexS.Name = "rdoNotationHexS"
        Me.rdoNotationHexS.Size = New System.Drawing.Size(113, 17)
        Me.rdoNotationHexS.TabIndex = 22
        Me.rdoNotationHexS.TabStop = True
        Me.rdoNotationHexS.Text = "Hexadecimal (36h)"
        Me.rdoNotationHexS.UseVisualStyleBackColor = True
        '
        'rdoNotationDec
        '
        Me.rdoNotationDec.AutoSize = True
        Me.rdoNotationDec.Location = New System.Drawing.Point(15, 42)
        Me.rdoNotationDec.Name = "rdoNotationDec"
        Me.rdoNotationDec.Size = New System.Drawing.Size(84, 17)
        Me.rdoNotationDec.TabIndex = 21
        Me.rdoNotationDec.TabStop = True
        Me.rdoNotationDec.Text = "Decimal (54)"
        Me.rdoNotationDec.UseVisualStyleBackColor = True
        '
        'rdoNotationHexP
        '
        Me.rdoNotationHexP.AutoSize = True
        Me.rdoNotationHexP.Location = New System.Drawing.Point(15, 88)
        Me.rdoNotationHexP.Name = "rdoNotationHexP"
        Me.rdoNotationHexP.Size = New System.Drawing.Size(118, 17)
        Me.rdoNotationHexP.TabIndex = 20
        Me.rdoNotationHexP.TabStop = True
        Me.rdoNotationHexP.Text = "Hexadecimal (0x36)"
        Me.rdoNotationHexP.UseVisualStyleBackColor = True
        '
        'rdoNotationNot
        '
        Me.rdoNotationNot.AutoSize = True
        Me.rdoNotationNot.Location = New System.Drawing.Point(15, 19)
        Me.rdoNotationNot.Name = "rdoNotationNot"
        Me.rdoNotationNot.Size = New System.Drawing.Size(115, 17)
        Me.rdoNotationNot.TabIndex = 19
        Me.rdoNotationNot.TabStop = True
        Me.rdoNotationNot.Text = "Musical Note (F#3)"
        Me.rdoNotationNot.UseVisualStyleBackColor = True
        '
        'grpLJConfig
        '
        Me.grpLJConfig.Controls.Add(Me.chkDmxover)
        Me.grpLJConfig.Controls.Add(Me.chkDmxin)
        Me.grpLJConfig.Controls.Add(Me.chkWindowsMessages)
        Me.grpLJConfig.Location = New System.Drawing.Point(12, 178)
        Me.grpLJConfig.Name = "grpLJConfig"
        Me.grpLJConfig.Size = New System.Drawing.Size(269, 91)
        Me.grpLJConfig.TabIndex = 7
        Me.grpLJConfig.TabStop = False
        Me.grpLJConfig.Text = "LightJockey Functions"
        '
        'chkDmxover
        '
        Me.chkDmxover.AutoSize = True
        Me.chkDmxover.Enabled = False
        Me.chkDmxover.Location = New System.Drawing.Point(16, 65)
        Me.chkDmxover.Name = "chkDmxover"
        Me.chkDmxover.Size = New System.Drawing.Size(129, 17)
        Me.chkDmxover.TabIndex = 17
        Me.chkDmxover.Text = "Enable DMX-Override"
        Me.chkDmxover.UseVisualStyleBackColor = True
        '
        'chkDmxin
        '
        Me.chkDmxin.AutoSize = True
        Me.chkDmxin.Enabled = False
        Me.chkDmxin.Location = New System.Drawing.Point(16, 42)
        Me.chkDmxin.Name = "chkDmxin"
        Me.chkDmxin.Size = New System.Drawing.Size(176, 17)
        Me.chkDmxin.TabIndex = 16
        Me.chkDmxin.Text = "Enable DMX-In (to LightJockey)"
        Me.chkDmxin.UseVisualStyleBackColor = True
        '
        'chkWindowsMessages
        '
        Me.chkWindowsMessages.AutoSize = True
        Me.chkWindowsMessages.Location = New System.Drawing.Point(16, 19)
        Me.chkWindowsMessages.Name = "chkWindowsMessages"
        Me.chkWindowsMessages.Size = New System.Drawing.Size(229, 17)
        Me.chkWindowsMessages.TabIndex = 15
        Me.chkWindowsMessages.Text = "Enable Windows Messages to LightJockey"
        Me.chkWindowsMessages.UseVisualStyleBackColor = True
        '
        'grpConnection
        '
        Me.grpConnection.Controls.Add(Me.chkFeedbackDetection)
        Me.grpConnection.Controls.Add(Me.chkIgnoreWhileConnecting)
        Me.grpConnection.Location = New System.Drawing.Point(12, 102)
        Me.grpConnection.Name = "grpConnection"
        Me.grpConnection.Size = New System.Drawing.Size(269, 70)
        Me.grpConnection.TabIndex = 14
        Me.grpConnection.TabStop = False
        Me.grpConnection.Text = "Connection Behavior"
        '
        'chkFeedbackDetection
        '
        Me.chkFeedbackDetection.AutoSize = True
        Me.chkFeedbackDetection.Enabled = False
        Me.chkFeedbackDetection.Location = New System.Drawing.Point(16, 42)
        Me.chkFeedbackDetection.Name = "chkFeedbackDetection"
        Me.chkFeedbackDetection.Size = New System.Drawing.Size(159, 17)
        Me.chkFeedbackDetection.TabIndex = 15
        Me.chkFeedbackDetection.Text = "Enable Feedback Detection"
        Me.chkFeedbackDetection.UseVisualStyleBackColor = True
        '
        'chkIgnoreWhileConnecting
        '
        Me.chkIgnoreWhileConnecting.AutoSize = True
        Me.chkIgnoreWhileConnecting.Location = New System.Drawing.Point(16, 19)
        Me.chkIgnoreWhileConnecting.Name = "chkIgnoreWhileConnecting"
        Me.chkIgnoreWhileConnecting.Size = New System.Drawing.Size(205, 17)
        Me.chkIgnoreWhileConnecting.TabIndex = 14
        Me.chkIgnoreWhileConnecting.Text = "Ignore MIDI Events While Connecting"
        Me.chkIgnoreWhileConnecting.UseVisualStyleBackColor = True
        '
        'grpConfigFile
        '
        Me.grpConfigFile.Controls.Add(Me.cmdFileSaveAs)
        Me.grpConfigFile.Controls.Add(Me.cmdFileNew)
        Me.grpConfigFile.Controls.Add(Me.cmdFileOpen)
        Me.grpConfigFile.Controls.Add(Me.txtConfigFile)
        Me.grpConfigFile.Location = New System.Drawing.Point(12, 12)
        Me.grpConfigFile.Name = "grpConfigFile"
        Me.grpConfigFile.Size = New System.Drawing.Size(269, 84)
        Me.grpConfigFile.TabIndex = 15
        Me.grpConfigFile.TabStop = False
        Me.grpConfigFile.Text = "Configuration File"
        '
        'cmdFileSaveAs
        '
        Me.cmdFileSaveAs.Location = New System.Drawing.Point(178, 45)
        Me.cmdFileSaveAs.Name = "cmdFileSaveAs"
        Me.cmdFileSaveAs.Size = New System.Drawing.Size(75, 23)
        Me.cmdFileSaveAs.TabIndex = 3
        Me.cmdFileSaveAs.Text = "Save As..."
        Me.cmdFileSaveAs.UseVisualStyleBackColor = True
        '
        'cmdFileNew
        '
        Me.cmdFileNew.Location = New System.Drawing.Point(97, 45)
        Me.cmdFileNew.Name = "cmdFileNew"
        Me.cmdFileNew.Size = New System.Drawing.Size(75, 23)
        Me.cmdFileNew.TabIndex = 2
        Me.cmdFileNew.Text = "New..."
        Me.cmdFileNew.UseVisualStyleBackColor = True
        '
        'cmdFileOpen
        '
        Me.cmdFileOpen.Location = New System.Drawing.Point(16, 45)
        Me.cmdFileOpen.Name = "cmdFileOpen"
        Me.cmdFileOpen.Size = New System.Drawing.Size(75, 23)
        Me.cmdFileOpen.TabIndex = 1
        Me.cmdFileOpen.Text = "Open..."
        Me.cmdFileOpen.UseVisualStyleBackColor = True
        '
        'txtConfigFile
        '
        Me.txtConfigFile.Location = New System.Drawing.Point(16, 19)
        Me.txtConfigFile.Name = "txtConfigFile"
        Me.txtConfigFile.Size = New System.Drawing.Size(237, 20)
        Me.txtConfigFile.TabIndex = 0
        '
        'frmConfiguration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(568, 372)
        Me.Controls.Add(Me.grpConfigFile)
        Me.Controls.Add(Me.grpConnection)
        Me.Controls.Add(Me.grpLJConfig)
        Me.Controls.Add(Me.grpMidiDisplay)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmConfiguration"
        Me.Text = "Feel: Configuration"
        Me.grpMidiDisplay.ResumeLayout(False)
        Me.grpTranspose.ResumeLayout(False)
        Me.grpTranspose.PerformLayout()
        Me.grpNumbering.ResumeLayout(False)
        Me.grpNumbering.PerformLayout()
        Me.grpNotation.ResumeLayout(False)
        Me.grpNotation.PerformLayout()
        Me.grpLJConfig.ResumeLayout(False)
        Me.grpLJConfig.PerformLayout()
        Me.grpConnection.ResumeLayout(False)
        Me.grpConnection.PerformLayout()
        Me.grpConfigFile.ResumeLayout(False)
        Me.grpConfigFile.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpMidiDisplay As System.Windows.Forms.GroupBox
    Friend WithEvents grpLJConfig As System.Windows.Forms.GroupBox
    Friend WithEvents chkDmxover As System.Windows.Forms.CheckBox
    Friend WithEvents chkDmxin As System.Windows.Forms.CheckBox
    Friend WithEvents chkWindowsMessages As System.Windows.Forms.CheckBox
    Friend WithEvents grpNotation As System.Windows.Forms.GroupBox
    Friend WithEvents rdoNotationDecS As System.Windows.Forms.RadioButton
    Friend WithEvents rdoNotationHexS As System.Windows.Forms.RadioButton
    Friend WithEvents rdoNotationDec As System.Windows.Forms.RadioButton
    Friend WithEvents rdoNotationHexP As System.Windows.Forms.RadioButton
    Friend WithEvents rdoNotationNot As System.Windows.Forms.RadioButton
    Friend WithEvents grpNumbering As System.Windows.Forms.GroupBox
    Friend WithEvents rdoNumbering1 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoNumbering0 As System.Windows.Forms.RadioButton
    Friend WithEvents grpTranspose As System.Windows.Forms.GroupBox
    Friend WithEvents rdoTransposeC3 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoTransposeC4 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoTransposeC5 As System.Windows.Forms.RadioButton
    Friend WithEvents grpConnection As System.Windows.Forms.GroupBox
    Friend WithEvents chkIgnoreWhileConnecting As System.Windows.Forms.CheckBox
    Friend WithEvents chkFeedbackDetection As System.Windows.Forms.CheckBox
    Friend WithEvents grpConfigFile As System.Windows.Forms.GroupBox
    Friend WithEvents txtConfigFile As System.Windows.Forms.TextBox
    Friend WithEvents cmdFileSaveAs As System.Windows.Forms.Button
    Friend WithEvents cmdFileNew As System.Windows.Forms.Button
    Friend WithEvents cmdFileOpen As System.Windows.Forms.Button
End Class
