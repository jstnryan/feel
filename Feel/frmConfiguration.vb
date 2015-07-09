Public Class frmConfiguration

    Private _ConfigFileUpdated As Boolean = False
    Private Property ConfigFileUpdated() As Boolean
        Get
            Return _ConfigFileUpdated
        End Get
        Set(ByVal value As Boolean)
            _ConfigFileUpdated = value
            SetControls()
        End Set
    End Property

    Private Sub frmConfiguration_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If _ConfigFileUpdated Then
            main.configMode(True) = False
        Else
            main.configMode = False
        End If
    End Sub

    Private Sub frmConfiguration_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        main.SaveConfiguration()
    End Sub

    Private Sub frmConfiguration_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        main.configMode = True
        SetControls()
    End Sub

    Private Sub SetControls()
        ''Read Configuration and properly populate/set controls
        ''Configuration File settings:
        txtConfigFile.Text = CompactString(My.Settings.ConfigFile, txtConfigFile.Width, txtConfigFile.Font, Windows.Forms.TextFormatFlags.PathEllipsis)
        ''Connection Behavior settings:
        chkIgnoreWhileConnecting.Checked = FeelConfig.IgnoreEvents
        'chkFeedbackDetection.Checked = FeelConfig.FeedbackDetection
        chkReconnectDevices.Checked = FeelConfig.RestoreConnections
        ''LightJockey Functions settings:
        chkWindowsMessages.Checked = FeelConfig.WmEnable
        chkDmxin.Checked = FeelConfig.DmxinEnable
        chkDmxover.Checked = FeelConfig.DmxoverEnable
        ''MIDI Display Preferences settings:
        'TODO: Better way to do this?
        Select Case FeelConfig.MidiNotation
            Case 0
                rdoNotationNot.Checked = True
            Case 1
                rdoNotationDec.Checked = True
            Case 2
                rdoNotationDecS.Checked = True
            Case 3
                rdoNotationHexP.Checked = True
            Case 4
                rdoNotationHexS.Checked = True
        End Select
        Select Case FeelConfig.MidiNumbering
            Case 0
                rdoNumbering0.Checked = True
            Case 1
                rdoNumbering1.Checked = True
        End Select
        Select Case FeelConfig.MidiTranspose
            Case 0
                rdoTransposeC5.Checked = True
            Case 1
                rdoTransposeC4.Checked = True
            Case 2
                rdoTransposeC3.Checked = True
        End Select
    End Sub

    Private Sub chkWindowsMessages_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWindowsMessages.CheckedChanged
        FeelConfig.WmEnable = chkWindowsMessages.Checked
    End Sub

    Private Sub chkDmxin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDmxin.CheckedChanged
        FeelConfig.DmxinEnable = chkDmxin.Checked
    End Sub

    Private Sub chkDmxover_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDmxover.CheckedChanged
        FeelConfig.DmxoverEnable = chkDmxover.Checked
    End Sub

    Private Sub chkIgnoreWhileConnecting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIgnoreWhileConnecting.CheckedChanged
        FeelConfig.IgnoreEvents = chkIgnoreWhileConnecting.Checked
    End Sub

    'Private Sub chkFeedbackDetection_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    FeelConfig.FeedbackDetection = chkFeedbackDetection.Checked
    'End Sub

    Private Sub chkReconnectDevices_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkReconnectDevices.CheckedChanged
        FeelConfig.RestoreConnections = chkReconnectDevices.Checked
    End Sub

    Private Sub rdoNotationNot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNotationNot.CheckedChanged
        If rdoNotationNot.Checked Then FeelConfig.MidiNotation = 0
    End Sub

    Private Sub rdoNotationDec_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNotationDec.CheckedChanged
        If rdoNotationDec.Checked Then FeelConfig.MidiNotation = 1
    End Sub

    Private Sub rdoNotationDecS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNotationDecS.CheckedChanged
        If rdoNotationDecS.Checked Then FeelConfig.MidiNotation = 2
    End Sub

    Private Sub rdoNotationHexP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNotationHexP.CheckedChanged
        If rdoNotationHexP.Checked Then FeelConfig.MidiNotation = 3
    End Sub

    Private Sub rdoNotationHexS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNotationHexS.CheckedChanged
        If rdoNotationHexS.Checked Then FeelConfig.MidiNotation = 4
    End Sub

    Private Sub rdoNumbering0_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNumbering0.CheckedChanged
        If rdoNumbering0.Checked Then FeelConfig.MidiNumbering = 0
    End Sub

    Private Sub rdoNumbering1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNumbering1.CheckedChanged
        If rdoNumbering1.Checked Then FeelConfig.MidiNumbering = 1
    End Sub

    Private Sub rdoTransposeC5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoTransposeC5.CheckedChanged
        If rdoTransposeC5.Checked Then FeelConfig.MidiTranspose = 0
    End Sub

    Private Sub rdoTransposeC4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoTransposeC4.CheckedChanged
        If rdoTransposeC4.Checked Then FeelConfig.MidiTranspose = 1
    End Sub

    Private Sub rdoTransposeC3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoTransposeC3.CheckedChanged
        If rdoTransposeC3.Checked Then FeelConfig.MidiTranspose = 2
    End Sub

    ''Source: http://www.codeproject.com/Articles/18319/Use-the-NET-framework-to-shorten-a-path-string-wit
    Function CompactString(ByVal MyString As String, ByVal Width As Integer, ByVal Font As Drawing.Font, ByVal FormatFlags As Windows.Forms.TextFormatFlags) As String
        Dim Result As String = String.Copy(MyString)
        Windows.Forms.TextRenderer.MeasureText(Result, Font, New Drawing.Size(Width, 0), FormatFlags Or Windows.Forms.TextFormatFlags.ModifyString)
        Return Result
    End Function

    Private Sub cmdFileOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFileOpen.Click
        Dim openDialog As New Windows.Forms.OpenFileDialog()
        'openDialog.InitialDirectory = Windows.Forms.Application.StartupPath & "\user\"
        openDialog.InitialDirectory = System.IO.Path.GetDirectoryName(My.Settings.ConfigFile)
        openDialog.Filter = "Feel configurations (*.feel)|*.feel|All files (*.*)|*.*"
        If (openDialog.ShowDialog = Windows.Forms.DialogResult.OK) Then
            My.Settings.ConfigFile = openDialog.FileName
            My.Settings.Save()
            main.LoadConfiguration()
            ConfigFileUpdated = True
        End If
        openDialog.Dispose()
        'openDialog = Nothing
    End Sub

    Private Sub cmdFileNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFileNew.Click
        Dim saveDialog As New Windows.Forms.SaveFileDialog
        'saveDialog.InitialDirectory = Windows.Forms.Application.StartupPath & "\user\"
        saveDialog.InitialDirectory = System.IO.Path.GetDirectoryName(My.Settings.ConfigFile)
        saveDialog.Filter = "Feel configuration (*.feel)|*.feel|All files (*.*)|*.*"
        If (saveDialog.ShowDialog = Windows.Forms.DialogResult.OK) Then
            main.CreateNewConfiguration()
            My.Settings.ConfigFile = saveDialog.FileName
            My.Settings.Save()
            main.SaveConfiguration()
            main.LoadConfiguration()
            ConfigFileUpdated = True
        End If
        saveDialog.Dispose()
        'saveDialog = Nothing
    End Sub

    Private Sub cmdFileSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFileSaveAs.Click
        Dim saveDialog As New Windows.Forms.SaveFileDialog
        'saveDialog.InitialDirectory = Windows.Forms.Application.StartupPath & "\user\"
        saveDialog.InitialDirectory = System.IO.Path.GetDirectoryName(My.Settings.ConfigFile)
        saveDialog.Filter = "Feel configuration (*.feel)|*.feel|All files (*.*)|*.*"
        If (saveDialog.ShowDialog = Windows.Forms.DialogResult.OK) Then
            My.Settings.ConfigFile = saveDialog.FileName
            My.Settings.Save()
            main.SaveConfiguration()
            'main.LoadConfiguration() ''Not neccessary, because it's an exact copy of old config
            ConfigFileUpdated = True
        End If
        saveDialog.Dispose()
        'saveDialog = Nothing
    End Sub
End Class