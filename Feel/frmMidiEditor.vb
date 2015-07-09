Imports System.Linq
Imports System.Collections.Generic

Public Class frmMidiEditor
    ''' <summary>
    ''' Stores the currently editable MIDI string.
    ''' </summary>
    ''' <remarks>
    ''' Converts a MIDI message hexadecimal string into their individual components and stores them separately.
    ''' </remarks>
    Friend Class _midiData
        Dim _stringFormat As String
        Dim _IsComment As Boolean
        Dim _Type As Byte
        Dim _Channel As Byte
        Dim _Data1 As Byte
        Dim _Data2 As Byte
        Dim _Data As Collections.Generic.List(Of Byte)
        Dim _DataLsbMsb As Integer

        Public Property StringFormat() As String
            Get
                Dim retstr As String = ""
                retstr = If(_IsComment, "#", "") _
                    & ((_Type << 4) + _Channel).ToString("X2")
                If (_Type = 12) Then
                    retstr &= _Data1.ToString("X2")
                ElseIf (_Type = 15) Then
                    retstr &= String.Join("", (Enumerable.Range(0, _Data.Count).[Select](Function(i As Integer) _Data(i).ToString("X2"))).ToArray)
                Else
                    retstr &= _Data1.ToString("X2") & _Data2.ToString("X2")
                End If
                Return retstr

                'Return _stringFormat
                'Return _
                '    If(_IsComment, "#", "") _
                '    & ((_Type << 4) + _Channel).ToString("X2") _
                '    & String.Join("", (Enumerable.Range(0, _Data.Length).[Select](Function(i As Integer) _Data(i).ToString("X2"))).ToArray)
            End Get
            Set(ByVal value As String)
                _stringFormat = value
                'set all other values
                If (_stringFormat.IsNullOrEmpty) Then
                    _IsComment = False
                    _Type = 0
                    _Channel = 0
                    _Data1 = 0
                    _Data2 = 0
                    _Data = New Collections.Generic.List(Of Byte)(New Byte() {0, 0})
                Else
                    ''Remove whitespace from commands, and convert to uppercase
                    Dim regex As New Text.RegularExpressions.Regex("\s")
                    _stringFormat = regex.Replace(_stringFormat, String.Empty)
                    _stringFormat = UCase(_stringFormat)

                    If (_stringFormat.Length > 0) Then
                        Dim pos As Integer = 0
                        ''Get first character to determine command type, and/or comment
                        If (_stringFormat.Substring(0, 1) = "#") Then
                            _IsComment = True
                            pos += 1
                        End If
                        _Type = Convert.ToByte(_stringFormat.Substring(pos, 1), 16)

                        If (_stringFormat.Length > pos + 1) Then
                            ''Get channel number
                            _Channel = Convert.ToByte(_stringFormat.Substring(pos + 1, 1), 16)

                            If (_stringFormat.Length > pos + 2) Then
                                DataString = If(_IsComment, _stringFormat.Substring(3), _stringFormat.Substring(2))
                            End If
                        End If
                    End If
                End If
            End Set
        End Property
        Friend ReadOnly Property IsComment() As Boolean
            Get
                Return _IsComment
            End Get
        End Property
        Friend Property Type() As Byte
            Get
                Return _Type
            End Get
            Set(ByVal value As Byte)
                _Type = value
            End Set
        End Property
        Friend Property Channel() As Byte
            Get
                Return _Channel
            End Get
            Set(ByVal value As Byte)
                _Channel = value
            End Set
        End Property
        Friend Property Data1() As Byte
            Get
                Return _Data1
            End Get
            Set(ByVal value As Byte)
                _Data1 = value
                _Data(0) = value
            End Set
        End Property
        Friend Property Data2() As Byte
            Get
                Return _Data2
            End Get
            Set(ByVal value As Byte)
                _Data2 = value
                _Data(1) = value
            End Set
        End Property
        Friend Property Data() As List(Of Byte)
            Get
                Return _Data
            End Get
            Set(ByVal value As List(Of Byte))
                _Data = value
            End Set
        End Property
        Friend Property DataString() As String
            Get
                Dim str As String = ""
                For i As Integer = 0 To _Data.Count - 1
                    str &= _Data(i).ToString("X2") & " "
                Next
                Return str.Substring(0, str.Length - 1)
            End Get
            Set(ByVal value As String)
                'TODO: FAILS IF value is not at least 4 char

                ''Remove whitespace from commands, and convert to uppercase
                Dim regex As New Text.RegularExpressions.Regex("\s")
                value = regex.Replace(value, String.Empty)
                value = UCase(value)

                ''Convert to byte array
                Dim len As Integer = value.Length
                Dim upperBound As Integer = len \ 2
                If ((len Mod 2) = 0) Then
                    upperBound -= 1
                Else
                    value = "0" & value
                End If
                Dim cmdArr(upperBound) As Byte
                For i As Integer = 0 To upperBound
                    cmdArr(i) = Convert.ToByte(value.Substring(i * 2, 2), 16)
                Next
                _Data = New List(Of Byte)(cmdArr)
                'TODO: better way to do this check?
                _Data1 = If(cmdArr(0) > 127, CByte(127), cmdArr(0))
                _Data2 = CByte(If(cmdArr.Length > 1, If(cmdArr(1) > 127, 127, cmdArr(1)), 0))
            End Set
        End Property
        Friend Property DataLsbMsb() As Integer
            Get
                Return (_Data2 * 128) + _Data1
            End Get
            Set(ByVal value As Integer)
                _DataLsbMsb = value

                ''MSB
                '_Data2 = CByte(value \ 128)
                '_Data2 = CByte((value And &HFF80) \ 128)
                '_Data2 = CByte(value >> 7)
                _Data2 = CByte((value And (&H7F << 7)) >> 7)
                _Data(1) = _Data2

                ''LSB
                '_Data1 = CByte(value - ((value \ 128) * 128))
                '_Data1 = CByte(value - (value And &HFF80))
                '_Data1 = CByte(value And &H7F)
                _Data1 = CByte(value And &H7F)
                _Data(0) = _Data1
            End Set
        End Property

        Public Sub New()
            _stringFormat = ""
            _IsComment = False
            _Type = 0
            _Channel = 0
            _Data1 = 0
            _Data2 = 0
            _Data = New List(Of Byte)(New Byte() {0, 0})
            _DataLsbMsb = 0
        End Sub
    End Class
    Friend midiData As New _midiData

    Public Sub New(Optional ByVal value As String = "")
        InitializeComponent()
        midiData.StringFormat = value
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        '265, 102
        Dim grpLoc As System.Drawing.Point = New System.Drawing.Point(15, 33)
        grpNotCon.Location = grpLoc
        grpNotCon.Enabled = False
        grpNotCon.Visible = False
        grpProgramChange.Location = grpLoc
        grpProgramChange.Enabled = False
        grpProgramChange.Visible = False
        grpPitchBend.Location = grpLoc
        grpPitchBend.Enabled = False
        grpPitchBend.Visible = False
        grpSysex.Location = grpLoc
        grpSysex.Enabled = False
        grpSysex.Visible = False
        'cboNotConChannel.Items.Clear()
        'cboNotConNotCon.Items.Clear
        For i As Byte = 0 To 15
            cboNotConChannel.Items.Add("Channel " & frmEvents.DisplayChannel(i))
            cboProgramChangeChannel.Items.Add("Channel " & frmEvents.DisplayChannel(i))
            cboPitchBendChannel.Items.Add("Channel " & frmEvents.DisplayChannel(i))
        Next
        If FeelConfig.MidiNotation = 0 Then
            For i As Byte = 0 To 127
                cboNotConNotCon.Items.Add(frmEvents.DisplayNoteAsString(i))
            Next
        Else
            For i As Byte = 0 To 127
                cboNotConNotCon.Items.Add(frmEvents.DisplayNote(i))
            Next
        End If
        AdjustControls()

        MyBase.OnLoad(e)
    End Sub

    Private Sub frmMidiEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Size = New System.Drawing.Size(302, 226)
    End Sub

    ''' <summary>Clears values from, and hides <see cref="midiData">midiData</see> adjustment controls.</summary>
    ''' <remarks>This is kept separate from <see cref="AdjustControls">AdjustControls</see> so that user events don't change focus.</remarks>
    Private Sub ClearControls()
        grpNotCon.Enabled = False
        grpNotCon.Visible = False
        grpProgramChange.Enabled = False
        grpProgramChange.Visible = False
        grpPitchBend.Enabled = False
        grpPitchBend.Visible = False
        grpSysex.Enabled = False
        grpSysex.Visible = False
    End Sub

    ''' <summary>Sets control values based on current <see cref="midiData">midiData</see>.</summary>
    Private Sub AdjustControls()
        ''Ugh..
        RemoveHandler cboMessageType.SelectedIndexChanged, AddressOf cboMessageType_SelectedIndexChanged
        RemoveHandler cboNotConChannel.SelectedIndexChanged, AddressOf cboChannel_SelectedIndexChanged
        RemoveHandler cboNotConNotCon.SelectedIndexChanged, AddressOf cboNotConNotCon_SelectedIndexChanged
        RemoveHandler nudNotConVelVal.ValueChanged, AddressOf nudNotConVelVal_ValueChanged
        RemoveHandler cboProgramChangeChannel.SelectedIndexChanged, AddressOf cboChannel_SelectedIndexChanged
        RemoveHandler nudProgramInstrument.ValueChanged, AddressOf nudProgramInstrument_ValueChanged
        RemoveHandler cboPitchBendChannel.SelectedIndexChanged, AddressOf cboChannel_SelectedIndexChanged
        RemoveHandler nudPitchBendValue.ValueChanged, AddressOf nudPitchBendValue_ValueChanged
        'RemoveHandler txtSysexData.TextChanged, AddressOf txtSysexData_TextChanged
        'RemoveHandler txtMidiMessage.TextChanged, AddressOf txtMidiMessage_TextChanged

        Select Case midiData.Type
            ''Unsupported:
            'Case "A" 'Polyphonic Aftertouch
            'Case "D" 'Channel Pressure (Aftertouch)
            Case 0 'none
                cboMessageType.SelectedIndex = 0
            Case 8 'MIDI Note Off
                cboMessageType.SelectedIndex = 1
                grpNotCon.Text = "Note Off Properties"
            Case 9 'MIDI Note On
                cboMessageType.SelectedIndex = 2
                grpNotCon.Text = "Note On Properties"
            Case 11 'Control Change
                cboMessageType.SelectedIndex = 3
                grpNotCon.Text = "Control Change Properties"
            Case 12 'Program Change
                cboMessageType.SelectedIndex = 4
                'grpProgramChange.Text = "Program Change Properties"
            Case 14 'Pitch Wheel Change (Pitch Bend)
                cboMessageType.SelectedIndex = 5
                'grpProgramChange.Text = "Pitch Bend Properties"
            Case 15 'MIDI System-Common Message
                cboMessageType.SelectedIndex = 6
        End Select
        Select Case midiData.Type
            Case 0 'none
            Case 8 To 9 'MIDI Note Off, Note On
                grpNotCon.Visible = True
                cboNotConChannel.SelectedIndex = midiData.Channel
                lblNotConNotCon.Text = "Note:"
                cboNotConNotCon.SelectedIndex = midiData.Data1
                lblNotConVelVal.Text = "Velocity:"
                nudNotConVelVal.Value = midiData.Data2
                grpNotCon.Enabled = True
            Case 11 'Control Change
                grpNotCon.Visible = True
                cboNotConChannel.SelectedIndex = midiData.Channel
                lblNotConNotCon.Text = "Control:"
                cboNotConNotCon.SelectedIndex = midiData.Data1
                lblNotConVelVal.Text = "Value:"
                nudNotConVelVal.Value = midiData.Data2
                grpNotCon.Enabled = True
            Case 12 'Program Change
                grpProgramChange.Visible = True
                cboProgramChangeChannel.SelectedIndex = midiData.Channel
                nudProgramInstrument.Value = midiData.Data1
                grpProgramChange.Enabled = True
            Case 14 'Pitch Wheel Change (Pitch Bend)
                grpPitchBend.Visible = True
                cboPitchBendChannel.SelectedIndex = midiData.Channel
                nudPitchBendValue.Value = midiData.DataLsbMsb
                grpPitchBend.Enabled = True
            Case 15 'MIDI System-Common Message
                grpSysex.Visible = True
                txtSysexData.Text = midiData.DataString
                grpSysex.Enabled = True
        End Select
        txtMidiMessage.Text = If(midiData.Type = 0, "", midiData.StringFormat)

        ''Ugh.. AGAIN!
        AddHandler cboMessageType.SelectedIndexChanged, AddressOf cboMessageType_SelectedIndexChanged
        AddHandler cboNotConChannel.SelectedIndexChanged, AddressOf cboChannel_SelectedIndexChanged
        AddHandler cboNotConNotCon.SelectedIndexChanged, AddressOf cboNotConNotCon_SelectedIndexChanged
        AddHandler nudNotConVelVal.ValueChanged, AddressOf nudNotConVelVal_ValueChanged
        AddHandler cboProgramChangeChannel.SelectedIndexChanged, AddressOf cboChannel_SelectedIndexChanged
        AddHandler nudProgramInstrument.ValueChanged, AddressOf nudProgramInstrument_ValueChanged
        AddHandler cboPitchBendChannel.SelectedIndexChanged, AddressOf cboChannel_SelectedIndexChanged
        AddHandler nudPitchBendValue.ValueChanged, AddressOf nudPitchBendValue_ValueChanged
        'AddHandler txtSysexData.TextChanged, AddressOf txtSysexData_TextChanged
        'AddHandler txtMidiMessage.TextChanged, AddressOf txtMidiMessage_TextChanged
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmdInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInsert.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cboMessageType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMessageType.SelectedIndexChanged
        Select Case cboMessageType.SelectedIndex
            Case 0 'none
                midiData.Type = 0
            Case 1 'MIDI Note Off (8)
                midiData.Type = 8
            Case 2 'MIDI Note On (9)
                midiData.Type = 9
            Case 3 'Control Change (11)
                midiData.Type = 11
            Case 4 'Program Change (12)
                midiData.Type = 12
            Case 5 'Pitch Wheel Change (Pitch Bend) (14)
                midiData.Type = 14
            Case 6 'MIDI System-Common Message (15)
                midiData.Type = 15
        End Select
        ClearControls()
        AdjustControls()
    End Sub

    Private Sub cboChannel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboNotConChannel.SelectedIndexChanged, cboProgramChangeChannel.SelectedIndexChanged, cboPitchBendChannel.SelectedIndexChanged
        'TODO: Faster to just have separate Subs where no casting is involved?
        'midiData.Channel = CByte(cboNotConChannel.SelectedIndex)
        midiData.Channel = CByte(DirectCast(sender, Windows.Forms.ComboBox).SelectedIndex)
        AdjustControls()
    End Sub

    Private Sub cboNotConNotCon_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboNotConNotCon.SelectedIndexChanged
        midiData.Data1 = CByte(cboNotConNotCon.SelectedIndex)
        AdjustControls()
    End Sub

    Private Sub nudNotConVelVal_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudNotConVelVal.ValueChanged, nudNotConVelVal.Leave
        midiData.Data2 = CByte(nudNotConVelVal.Value)
        AdjustControls()
    End Sub

    Private Sub nudProgramInstrument_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudProgramInstrument.ValueChanged, nudNotConVelVal.Leave
        midiData.Data1 = CByte(nudProgramInstrument.Value)
        AdjustControls()
    End Sub

    Private Sub nudPitchBendValue_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPitchBendValue.ValueChanged, nudPitchBendValue.Leave
        midiData.DataLsbMsb = CInt(nudPitchBendValue.Value)
        AdjustControls()
    End Sub

    Private Sub txtSysexData_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSysexData.KeyUp
        If (e.KeyCode = Windows.Forms.Keys.Enter) Then
            midiData.DataString = txtSysexData.Text
            AdjustControls()
        End If
    End Sub

    Private Sub txtSysexData_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSysexData.Leave
        midiData.DataString = txtSysexData.Text
        AdjustControls()
    End Sub

    Private Sub txtMidiMessage_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMidiMessage.KeyUp
        If (e.KeyCode = Windows.Forms.Keys.Enter) Then
            midiData.StringFormat = txtMidiMessage.Text
            AdjustControls()
        End If
    End Sub

    Private Sub txtMidiMessage_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMidiMessage.Leave
        midiData.StringFormat = txtMidiMessage.Text
        AdjustControls()
    End Sub
End Class