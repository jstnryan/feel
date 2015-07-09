Public Class Interfaces
    Friend Class ServiceHost
        Inherits ActionInterface.IServices

#Region "Feel Functions"
        Public Overrides Sub OpenWindowActions()
            'Source: http://www.codeproject.com/Articles/31971/Understanding-SynchronizationContext-Part-I
            'main._threadcontext.Post(AddressOf main.OpenActionWindow_Ext, main._threadcontext)
            main._threadcontext.Post(AddressOf main.OpenEventWindow_Ext, Nothing)
        End Sub

        Public Overrides Sub OpenWindowConnections()
            If (_threadcontext IsNot Nothing) Then main._threadcontext.Post(AddressOf main.OpenConnectWindow_Ext, _threadcontext)
        End Sub

        Public Overrides Sub OpenWindowConfig()
            If (_threadcontext IsNot Nothing) Then main._threadcontext.Post(AddressOf main.OpenConfigWindow_Ext, _threadcontext)
        End Sub

        Public Overrides Sub SetPage(ByVal Device As String, ByVal Page As Byte)
            'main.SetPage(Device, Page)
            If (Device = "ALL DEVICES") Then
                For Each dev As clsConnection In FeelConfig.Connections.Values
                    If (dev.Enabled = True) Then
                        dev.PageCurrent = Page
                    End If
                Next
            Else
                Dim _device As Integer = FindDeviceIndex(Device)
                If (FeelConfig.Connections.ContainsKey(_device)) Then
                    FeelConfig.Connections(_device).PageCurrent = Page
                End If
            End If
            RedrawControls(Device)
        End Sub
        Public Overrides Sub SetPage(ByVal Device As Integer, ByVal Page As Byte)
            'main.SetPage(Device, Page)
            If (Device = -1) Or (Not FeelConfig.Connections.ContainsKey(Device)) Then
                Exit Sub
            Else
                FeelConfig.Connections(Device).PageCurrent = Page
                RedrawControls(Device)
            End If
        End Sub

        Public Overrides Sub RedrawControls(ByVal Device As Integer)
            If (Device = -1) Or (Not FeelConfig.Connections.ContainsKey(Device)) Then
                Exit Sub
            Else
                With FeelConfig.Connections(Device)
                    If (.Enabled And .OutputEnable) Then
                        For Each cont As clsControl In .Control.Values
                            If (cont.Page.ContainsKey(.PageCurrent)) Then
                                ''If this is the first time setting state, also set current state
                                '' otherwise use only CurrentState
                                If (cont.Page(.PageCurrent).CurrentState = "") Then
                                    SendMIDI(Device, cont.Page(.PageCurrent).InitialState)
                                    cont.Page(.PageCurrent).CurrentState = cont.Page(.PageCurrent).InitialState
                                Else
                                    SendMIDI(Device, cont.Page(.PageCurrent).CurrentState)
                                End If
                            End If
                        Next
                    End If
                End With
            End If
        End Sub
        Public Overrides Sub RedrawControls(Optional ByVal Device As String = "ALL DEVICES")
            If (Device = "ALL DEVICES") Then
                For Each _device As Integer In FeelConfig.Connections.Keys
                    RedrawControls(_device)
                Next
            Else
                RedrawControls(FindDeviceIndex(Device))
            End If
        End Sub
#End Region

#Region "Device Information"
        '''Map MIDI input and output names to device keys
        Public Overrides Function FindDeviceIndex(ByVal Device As String) As Integer
            'TODO: There's got to be a more efficient way of doing this...
            Dim nam As Integer = FindDeviceIndexByName(Device)
            Dim ipt As Integer = FindDeviceIndexByInput(Device)
            Dim opt As Integer = FindDeviceIndexByOutput(Device)

            If Not (nam = -1) Then
                Return nam
            ElseIf Not (ipt = -1) Then
                Return ipt
            ElseIf Not (opt = -1) Then
                Return opt
            End If
            Return -1
        End Function
        Public Overrides Function FindDeviceIndexByName(ByVal Device As String) As Integer
            For Each _index As Integer In FeelConfig.Connections.Keys
                If (FeelConfig.Connections(_index).Name = Device) Then Return _index
            Next
            Return -1
        End Function
        Public Overrides Function FindDeviceIndexByInput(ByVal Device As String) As Integer
            For Each _index As Integer In FeelConfig.Connections.Keys
                If (FeelConfig.Connections(_index).InputName = Device) Then Return _index
            Next
            Return -1
        End Function
        Public Overrides Function FindDeviceIndexByOutput(ByVal Device As String) As Integer
            For Each _index As Integer In FeelConfig.Connections.Keys
                If (FeelConfig.Connections(_index).OutputName = Device) Then Return _index
            Next
            Return -1
        End Function

        Public Overrides Function ConnectionExists(ByVal Connection As Integer) As Boolean
            Return FeelConfig.Connections.ContainsKey(Connection)
        End Function
        Public Overrides Function ControlExists(ByVal Connection As Integer, ByVal Control As String) As Boolean
            If (ConnectionExists(Connection)) Then
                Return FeelConfig.Connections.Item(Connection).Control.ContainsKey(Control)
            Else
                Return False
            End If
        End Function
        Public Overrides Function PageExists(ByVal Connection As Integer, ByVal Control As String, ByVal Page As Byte) As Boolean
            If (ConnectionExists(Connection)) Then
                If (ControlExists(Connection, Control)) Then
                    Return FeelConfig.Connections.Item(Connection).Control(Control).Page.ContainsKey(Page)
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Overrides Property CurrentPage(ByVal Connection As Integer) As Byte
            Get
                Return FeelConfig.Connections(Connection).PageCurrent
            End Get
            Set(ByVal value As Byte)
                FeelConfig.Connections(Connection).PageCurrent = value
            End Set
        End Property
        Public Overrides Property CurrentState(ByVal Connection As Integer, ByVal Control As String, Optional ByVal Page As Byte = 0) As String
            Get
                Return FeelConfig.Connections(Connection).Control(Control).Page(Page).CurrentState
            End Get
            Set(ByVal value As String)
                FeelConfig.Connections(Connection).Control(Control).Page(Page).CurrentState = value
            End Set
        End Property

        Public Overrides Sub ResetControlsByGroup(ByVal Group As Byte)
            For Each dev As Integer In FeelConfig.Connections.Keys
                For Each cont As String In FeelConfig.Connections(dev).Control.Keys
                    For Each pag As Byte In FeelConfig.Connections(dev).Control(cont).Page.Keys
                        If (FeelConfig.Connections(dev).Control(cont).Page(pag).ControlGroup = Group) Then
                            FeelConfig.Connections(dev).Control(cont).Page(pag).IsActive = False
                            FeelConfig.Connections(dev).Control(cont).Page(pag).CurrentState = FeelConfig.Connections(dev).Control(cont).Page(pag).InitialState
                        End If
                    Next
                Next
            Next
        End Sub
#End Region

#Region "LightJockey Information"
        Public Overrides Function GetCurrentCue() As Integer
            Return main.GetCue
        End Function

        Public Overrides Function GetCurrentCueList() As Integer
            Return main.GetCueList
        End Function

        Public Overrides Function GetCurrentSequence() As Integer
            Return main.GetSequence
        End Function

        Public Overrides Function GetCurrentBGCue() As Integer
            Return main.GetBackgroundCue
        End Function

        Public Overrides Function GetLJHandle() As System.IntPtr
            Return main.GetHandle
        End Function
#End Region

#Region "MIDI"
        Public Overrides Function GetMIDIDeviceList() As String()
            Dim devArr As String() = New String() {"ALL DEVICES"}
            For Each device As Integer In FeelConfig.Connections.Keys
                devArr.Add(FeelConfig.Connections(device).Name)
            Next
            Return devArr
        End Function
        Public Overrides Function GetMIDIDeviceINList() As String()
            Dim devArr As String() = New String() {"ALL DEVICES"}
            For Each device As Integer In FeelConfig.Connections.Keys
                If FeelConfig.Connections(device).InputEnable Then devArr.Add(FeelConfig.Connections(device).Name)
            Next
            Return devArr
        End Function
        Public Overrides Function GetMIDIDeviceOUTList() As String()
            Dim devArr As String() = New String() {"ALL DEVICES"}
            For Each device As Integer In FeelConfig.Connections.Keys
                If FeelConfig.Connections(device).OutputEnable Then devArr.Add(FeelConfig.Connections(device).Name)
            Next
            Return devArr
        End Function

        Public Overrides Sub SendMIDI(ByVal Device As Integer, ByVal Message As String)
            'main.SendMidi(Device, Message)
            ''Make sure we're not wasting our time sending something to a device that doesn't exist
            If (Device = -1) Or (Not FeelConfig.Connections.ContainsKey(Device)) Then
                Exit Sub
            Else
                ''Ensure the device is enabled to receive
                If (FeelConfig.Connections(Device).Enabled And FeelConfig.Connections(Device).OutputEnable) Then
                    'TODO: Currently checking this twice, because the regex doesn't like being served an empty string.
                    ' Need to do after because NullOrEmpty does not include spaces.
                    If (String.IsNullOrEmpty(Message)) Then Exit Sub

                    ''Remove whitespace from commands, and convert to uppercase
                    Dim regex As New Text.RegularExpressions.Regex("\s")
                    Message = regex.Replace(Message, String.Empty)
                    Message = UCase(Message)

                    ''Just a failsafe, don't need to do anything with empty States
                    If (String.IsNullOrEmpty(Message)) Then Exit Sub

                    ''Get first character to determine command type (See Select statement below)
                    Dim cmdType As String = Message.Substring(0, 1)
                    If (cmdType = "#") Then Exit Sub 'Comment

                    ''Check to make sure we have all information needed
                    If (Message.Length < 6) Then Exit Sub

                    ''Convert to byte array
                    Dim len As Integer = Message.Length
                    Dim upperBound As Integer = len \ 2
                    If ((len Mod 2) = 0) Then
                        upperBound -= 1
                    Else
                        Message = "0" & Message
                    End If
                    Dim cmdArr(upperBound) As Byte
                    For i As Integer = 0 To upperBound
                        cmdArr(i) = Convert.ToByte(Message.Substring(i * 2, 2), 16)
                    Next

                    'TODO: Checks to make sure values (in Byte, 0-255) do not exceed valid MIDI values (half-Byte, 0-127)
                    ' There's surely a better way to do this...
                    If (Convert.ToByte(Message.Substring(1, 1), 16) > 15) Then Exit Sub
                    If (cmdArr(1) > 127) Then Exit Sub
                    If (cmdArr(2) > 127) Then Exit Sub

                    ''Get the device's MIDI output name
                    Dim _device As String = FeelConfig.Connections(Device).OutputName

                    ''Take appropriate action
                    Select Case cmdType
                        ''Unsupported:
                        'Case "A" 'Polyphonic Aftertouch
                        'Case "D" 'Channel Pressure (Aftertouch)

                        Case "8" 'MIDI Note Off
                            'midiOut.Item(device.OutputName).SendNoteOff(CType(cmd.Substring(1, 1), Midi.Channel), CType(Convert.ToInt16(cmd.Substring(2, 2)), Midi.Pitch), Convert.ToInt16(cmd.Substring(4, 2)))
                            midiOut.Item(_device).SendNoteOff(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), CType(cmdArr(1), Midi.Pitch), cmdArr(2))
                        Case "9" 'MIDI Note On
                            midiOut.Item(_device).SendNoteOn(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), CType(cmdArr(1), Midi.Pitch), cmdArr(2))
                        Case "B" 'Control Change
                            midiOut.Item(_device).SendControlChange(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), CType(cmdArr(1), Midi.Control), cmdArr(2))
                        Case "C" 'Program Change
                            midiOut.Item(_device).SendProgramChange(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), CType(cmdArr(1), Midi.Instrument))
                        Case "E" 'Pitch Wheel Change (Pitch Bend)
                            midiOut.Item(_device).SendPitchBend(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), cmdArr(1))
                        Case "F" 'MIDI System-Common Message
                            If (Message.Substring(1, 1) = "0") Then 'MIDI Sysex Message
                                midiOut.Item(_device).SendSysEx(cmdArr)
                            End If
                    End Select
                End If
            End If
        End Sub
        'Public Overrides Sub SendMIDI(ByVal Device As Integer, ByVal Message() As String)
        '    'main.SendMidi(Device, Message)
        '    For Each _msg As String In Message
        '        SendMIDI(Device, _msg)
        '    Next
        'End Sub
        Public Overrides Sub SendMIDI(ByVal Device As String, ByVal Message As String)
            'main.SendMidi(Device, Message)
            If (Device = "ALL DEVICES") Then
                For Each dev As Integer In FeelConfig.Connections.Keys
                    SendMIDI(dev, Message)
                Next
            Else
                SendMIDI(FindDeviceIndex(Device), Message)
            End If
        End Sub
        'Public Overrides Sub SendMIDI(ByVal Device As String, ByVal Message() As String)
        '    'main.SendMidi(Device, Message)
        '    For Each _msg As String In Message
        '        SendMIDI(Device, _msg)
        '    Next
        'End Sub
        'Public Overloads Sub SendMIDI(ByVal Device As String, ByVal ControlType As Byte, ByVal Channel As Byte, ByVal NotCon As Byte)
        '    Dim ContStr As String = If(ControlType = 144 Or ControlType = 128, "9", "B") & Channel.ToString & NotCon.ToString("X2")
        '    Dim _device As Integer = FindDeviceIndex(Device)
        '    SendMIDI(Device, ContStr)
        'End Sub
        'Public Overloads Sub SendMIDI(ByVal Device As Integer, ByVal ControlType As Byte, ByVal channel As Byte, ByVal notcon As Byte)
        '    Dim ContStr As String = If(ControlType = 144 Or ControlType = 128, "9", "B") & channel.ToString & notcon.ToString("X2")
        '    SendMIDI(Device, ContStr)
        'End Sub
#End Region

#Region "Windows Messages"
        Public Overrides Sub ResetLJWindowHandle()
            main.RefreshHandle()
        End Sub
        Public Overrides Function PostLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Return main.PostMessage(uMsg, wParam, lParam)
        End Function

        Public Overrides Function SendLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Return main.SendMessage(uMsg, wParam, lParam)
        End Function
        'TODO:
        Public Overloads Overrides Function PostWMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Return 0
        End Function
        'TODO:
        Public Overloads Overrides Function SendWMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Return 0
        End Function

        Public Overrides Function SendLJCopyData(ByVal lParam As ActionInterface.CopyData) As Integer
            'wParam is supposed to be a pointer to the handle of this process, or an HWND
            Return main.SendCopyData(lParam)
        End Function
#End Region

#Region "TypeConverters"
        Public Shadows Class DeviceList
            Inherits ComponentModel.StringConverter

            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
                Dim devArr As String() = New String() {"ALL DEVICES"}
                For Each device As Integer In FeelConfig.Connections.Keys
                    devArr.Add(FeelConfig.Connections(device).Name)
                Next
                Return New StandardValuesCollection(devArr)
            End Function
        End Class

        Public Shadows Class OutDeviceList
            Inherits ComponentModel.StringConverter

            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
                Dim devArr As String() = New String() {"ALL DEVICES"}
                For Each device As Integer In FeelConfig.Connections.Keys
                    If FeelConfig.Connections(device).OutputEnable Then devArr.Add(FeelConfig.Connections(device).Name)
                Next
                Return New StandardValuesCollection(devArr)
            End Function
        End Class
#End Region
    End Class
End Class
