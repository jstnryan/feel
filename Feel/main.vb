Imports System.Windows.Forms
Imports Midi
Imports System.Runtime.InteropServices

Module main
    ''Components for System Tray icon
    Dim WithEvents trayIcon As New NotifyIcon
    Dim trayMenu As New ContextMenu
    Dim WithEvents menuExit As New MenuItem
    Dim WithEvents menuConfigConnections As New MenuItem
    Dim WithEvents menuConfigActions As New MenuItem
    Dim WithEvents menuAbout As New MenuItem

    ''Container for program configuration
    Public Configuration As clsConfig

    ''Collections of MIDI input and output devices
    Public midiIn As System.Collections.Generic.Dictionary(Of String, InputDevice) = New Collections.Generic.Dictionary(Of String, InputDevice)
    Public midiOut As System.Collections.Generic.Dictionary(Of String, OutputDevice) = New Collections.Generic.Dictionary(Of String, OutputDevice)

    Private configM As Boolean = False    ''when configuring actions, override output to LJ
    Public Property configMode(Optional ByVal reConnect As Boolean = False) As Boolean
        Get
            Return configM
        End Get
        Set(ByVal value As Boolean)
            If Not (configForm Is Nothing) Then
                If (configForm.Visible) Then
                    configM = True
                End If
            ElseIf Not (actionForm Is Nothing) Then
                If (actionForm.Visible) Then
                    configM = True
                End If
            End If
            If configM <> value Then
                configM = value
                'RedrawControls()
                If reConnect Then ConnectDevices()
            End If
        End Set
    End Property

    ''Returns a boolean indicating whether it is safe to handle an event's actions.
    '' True = do not handle event's actions
    Private _connecting As Boolean = False
    Public Property Connecting() As Boolean
        Get
            Return (_connecting And Configuration.IgnoreEvents)
        End Get
        Set(ByVal value As Boolean)
            _connecting = value
        End Set
    End Property

    Public configForm As frmConfig
    Public actionForm As frmActions
    Public aboutForm As frmAbout

    Public Sub Main()
        For Each arg As String In My.Application.CommandLineArgs
            System.Diagnostics.Debug.WriteLine("Command Line Arg: " & arg)
        Next

        ''Aparently this is required to show Groups in ListView controls
        System.Windows.Forms.Application.EnableVisualStyles()

        ''create system tray icon and context menu items
        menuExit.Text = "E&xit"
        menuConfigConnections.Text = "Configure &Connections"
        menuConfigActions.Text = "Configure &Actions"
        menuAbout.Text = "About"
        trayMenu.MenuItems.Add(menuExit)
        trayMenu.MenuItems.Add("-")
        trayMenu.MenuItems.Add(menuConfigActions)
        trayMenu.MenuItems.Add(menuConfigConnections)
        trayMenu.MenuItems.Add("-")
        trayMenu.MenuItems.Add(menuAbout)
        trayIcon.ContextMenu = trayMenu
        'trayIcon.Icon = New System.Drawing.Icon(System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream("Feel.feel.ico"))
        trayIcon.Icon = My.Resources.Feel.feel
        trayIcon.Visible = True

        LoadConfiguration() ''Read serialzed config data from file
        If Not configM Then ConnectDevices() ''Make MIDI connections

        Application.Run()
    End Sub

#Region "Windows & Menus"
    Private Sub menuExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles menuExit.Click
        If configM Then
            Dim quitNoSave As System.Windows.Forms.DialogResult = MessageBox.Show("One or more configuration windows are open; any changes made have not been saved. Do you want to exit without saving?", "Feel: Configuration Not Saved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If (quitNoSave = DialogResult.No) Then Exit Sub
        End If

        DisconnectDevices()

        trayIcon.Visible = False
        Application.Exit()
    End Sub

    Private Sub OpenConfigWindow() Handles menuConfigConnections.Click
        ''The Connections Configuration form makes its own connections to
        '' test devices, so we must disconnect existing connections first.
        DisconnectDevices()

        ''Singleton: See designer code.
        '' http://www.codeproject.com/Articles/5000/Simple-Singleton-Forms
        '' (dated link) http://www.codeproject.com/KB/vb/Simple_Singleton_Forms.aspx
        If configForm Is Nothing Then
            configForm = New frmConfig
        End If
        configForm.Show()
        'configForm.Focus()
    End Sub

    Friend Sub OpenActionWindow() Handles menuConfigActions.Click
        If actionForm Is Nothing Then
            actionForm = New frmActions
        End If
        actionForm.Show()
    End Sub

    Private Sub OpenAbout() Handles menuAbout.Click
        If aboutForm Is Nothing Then
            aboutForm = New frmAbout
        End If
        aboutForm.Show()
    End Sub
#End Region

#Region "File I/O"
    Private Sub LoadConfiguration()
LoadConfig:
        Dim fs As IO.FileStream
        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        Try
            fs = New IO.FileStream(Application.StartupPath & "\user\feel.config", IO.FileMode.Open, IO.FileAccess.Read)
            Configuration = CType(bf.Deserialize(fs), clsConfig)
            fs.Close()
        Catch fileEx As IO.FileNotFoundException
            MessageBox.Show("No configuration file found. Operating with blank configuration.", "Feel: Configuration Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ''no configuration file was found, so create a new one
            CreateNewConfiguration()
            ''then open the configuration window
            OpenConfigWindow()
        Catch directoryEx As IO.DirectoryNotFoundException
            If CreateUserDirectory() Then
                GoTo LoadConfig
            Else
                MessageBox.Show("Unable to create 'user' directory. Operating with blank configuration file.", "Feel: File System Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                CreateNewConfiguration()
                OpenConfigWindow()
            End If
        Catch ex As Exception
            ''some unexpected error occured
            MessageBox.Show("Unexpected error trying to read Feel configuration file." & vbCrLf & vbCrLf & ex.Message, "Feel: Unknown Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            fs = Nothing
            bf = Nothing
        End Try
    End Sub

    ''TODO: Does this really need to be broken out into its own subroutine?
    Private Sub CreateNewConfiguration()
        Configuration = New clsConfig
    End Sub

    Private Function CreateUserDirectory() As Boolean
        Try
            Dim dInfo As IO.DirectoryInfo = IO.Directory.CreateDirectory(Application.StartupPath & "\user")
            Return dInfo.Exists
        Catch ex As Exception
            MessageBox.Show("Error creating '\user' directory!", "Feel: Unknown Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Public Sub SaveConfiguration()
        Dim fs As IO.FileStream
        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        Try
            fs = New IO.FileStream(Windows.Forms.Application.StartupPath & "\user\feel.config", IO.FileMode.Create)
            bf.Serialize(fs, Configuration)
            fs.Close()
        Catch ex As Exception
            MessageBox.Show("An unexpected error has occured in the 'SaveConnectionConfig' procedure." & vbCrLf & vbCrLf & ex.Message, "Feel: Unexpected Error")
        Finally
            fs = Nothing
            bf = Nothing
        End Try
    End Sub
#End Region

#Region "Connections"
    ''' <summary>
    ''' Makes MIDI connections to all enabled devices
    ''' </summary>
    ''' <remarks>Ver:2.1</remarks>
    Private Sub ConnectDevices()
        Connecting = True

        For Each device As clsConnection In Configuration.Connections.Values
            If (device.Enabled) Then
                ''Check the stored connection index to make sure it is not greater than the number of currently connected devices
                If (device.Input + 1 > InputDevice.InstalledDevices.Count) Or (device.Output + 1 > OutputDevice.InstalledDevices.Count) Then
                    'System.Diagnostics.Debug.WriteLine("Disabled 2: Connection index out of bounds." & vbCrLf & "Device: " & device.Name)
                    device.Enabled = False
                    'With device
                    '    .InputEnable = False
                    '    .Input = -1
                    '    .InputName = ""
                    '    .OutputEnable = False
                    '    .Output = -1
                    '    .OutputName = ""
                    'End With
                    System.Windows.Forms.MessageBox.Show("Warning: One or both of the input/output ports for the device named " & device.Name & " no longer exists! Usually this is an indication that other devices have been removed from the system recently, or have not been powered on." & vbCrLf & vbCrLf & "The device has been disabled. Please update this device's connection info by opening the 'Configure Connections' window and adjusting this device's Input and Output settings, then re-enable the device.", "Feel: MIDI Configuration Has Changed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Continue For
                    'Else
                    '    System.Diagnostics.Debug.WriteLine("ConnectDevices:: Device: " & device.Name & ", Index within range: " & device.Input & ":" & InputDevice.InstalledDevices.Count & "||" & device.Output & ":" & OutputDevice.InstalledDevices.Count)
                End If

                ''Check that the stored connection names match the system's current names for the stored index
                If Not If(device.InputEnable, device.InputName = InputDevice.InstalledDevices.Item(device.Input).Name, True) Or Not If(device.OutputEnable, device.OutputName = OutputDevice.InstalledDevices.Item(device.Output).Name, True) Then
                    'System.Diagnostics.Debug.WriteLine("Disabled 1: Connection name does not match associated index." & vbCrLf & "Device: " & device.Name)
                    device.Enabled = False
                    System.Windows.Forms.MessageBox.Show("Warning: One or both of the names of the input/output ports for the device named " & device.Name & " do not match the name of the system's port at the same address! Usually this is an indication that additional devices have been added or removed from the system recently, or powered on in a different order." & vbCrLf & vbCrLf & "The device has been disabled. Please update this device's connection info by opening the 'Configure Connections' window and adjusting this device's Input and Output settings, then re-enable the device.", "Feel: MIDI Configuration Has Changed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Continue For
                    'Else
                    '    System.Diagnostics.Debug.WriteLine("ConnectDevices:: Device: " & device.Name & ", Names = Indexes:")
                End If

                ''Determine if connections are already in use
                If If(device.InputEnable, InputDevice.InstalledDevices.Item(device.Input).IsOpen, False) Or If(device.OutputEnable, OutputDevice.InstalledDevices.Item(device.Output).IsOpen, False) Then
                    device.Enabled = False
                    System.Windows.Forms.MessageBox.Show("Warning: One or both of the input/output ports for the device named " & device.Name & " is already in use! Either the device is already in use elsewhere, or the connection information is incorrectly set." & vbCrLf & vbCrLf & "The device has been disabled. If you receive this error and you believe the device is connected properly, update this device's connection info by opening the 'Configure Connections' window and adjusting this device's Input and Output settings, then re-enable the device.", "Feel: MIDI Port In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Continue For
                    'Else
                    '    System.Diagnostics.Debug.WriteLine("ConnectDevices:: Device: " & device.Name & ", In/Outputs not in use.")
                End If

                ''If we end up here, we should be good to connect the device
                If device.InputEnable Then
                    Try
                        Dim inDevice As InputDevice
                        inDevice = InputDevice.InstalledDevices.Item(device.Input)
                        midiIn.Add(device.InputName, inDevice)
                        With midiIn(device.InputName)
                            .Open()
                            .StartReceiving(Nothing, True)
                            AddHandler .NoteOn, AddressOf NoteOn
                            AddHandler .NoteOff, AddressOf NoteOff
                            AddHandler .ControlChange, AddressOf ControlChange
                        End With
                        inDevice = Nothing
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine("ConnectDevices:: Try/Catch Exception (Input). Details: " & ex.Message)
                        device.Enabled = False
                        System.Windows.Forms.MessageBox.Show("There has been an unexpected error when trying to connect the input for the device named " & device.Name & "." & vbCrLf & vbCrLf & "The device has been disabled. Please check this device's connection information in the 'Configure Connections' window.", "Feel: Unknown MIDI Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Continue For
                    End Try
                End If
                If device.OutputEnable Then
                    Try
                        Dim outDevice As OutputDevice
                        outDevice = OutputDevice.InstalledDevices.Item(device.Output)
                        midiOut.Add(device.OutputName, outDevice)
                        midiOut.Item(device.OutputName).Open()
                        outDevice = Nothing
                    Catch ex As Exception
                        System.Diagnostics.Debug.WriteLine("ConnectDevices:: Try/Catch Exception (Output). Details: " & ex.Message)
                        device.Enabled = False
                        System.Windows.Forms.MessageBox.Show("There has been an unexpected error when trying to connect the output for the device named " & device.Name & "." & vbCrLf & vbCrLf & "The device has been disabled. Please check this device's connection information in the 'Configure Connections' window.", "Feel: Unknown MIDI Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Continue For
                    End Try

                    ''Device should be connected, so now we take any "initialization" steps listed in the config
                    Dim _device As Integer = FindDeviceByName(device.Name)
                    If Not (String.IsNullOrEmpty(device.Init)) Then
                        Dim initCmds() As String = device.Init.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                        SendMidi(_device, initCmds)
                    End If
                    RedrawControls(_device)
                End If
            End If
        Next

        Connecting = False
    End Sub

    ''' <summary>
    ''' Closes and clears all connections, and removes all event handlers
    ''' </summary>
    ''' <param name="which">(Byte) 1: Close incoming, 2: Close outgoing, 0: Close both</param>
    ''' <remarks>Ver: 1.0
    ''' Called when closing application, or opening Connection Configuration window</remarks>
    Public Sub DisconnectDevices(Optional ByVal which As Byte = 0)
        Connecting = True

        Select Case which
            Case 1 ''Close incoming
                If (midiIn.Count > 0) Then
                    For Each inDev As InputDevice In midiIn.Values
                        If inDev.IsReceiving Then inDev.StopReceiving()
                        If inDev.IsOpen Then
                            inDev.Close()
                            RemoveHandler inDev.NoteOn, AddressOf NoteOn
                            RemoveHandler inDev.NoteOff, AddressOf NoteOff
                            RemoveHandler inDev.ControlChange, AddressOf ControlChange
                            inDev.RemoveAllEventHandlers()
                        End If
                    Next
                    midiIn.Clear()
                End If

            Case 2 ''Close outgoing
                If (midiOut.Count > 0) Then
                    For Each outDev As OutputDevice In midiOut.Values
                        If outDev.IsOpen Then outDev.Close()
                    Next
                    midiOut.Clear()
                End If

            Case Else
                DisconnectDevices(1)
                DisconnectDevices(2)
        End Select

        Connecting = False
    End Sub
#End Region

#Region "Event Handlers"
    Private Sub NoteOn(ByVal msg As NoteOnMessage)
        If configMode And Not (actionForm Is Nothing) Then
            ''override programmed action, redirect action to Configure Actions window
            If (actionForm.Visible) Then
                Dim newCont As curControl = New curControl
                newCont.Device = msg.Device.Name.ToString
                newCont.Channel = CByte(msg.Channel)
                newCont.Type = "Note"
                newCont.NotCon = CByte(msg.Pitch)
                newCont.VelVal = CByte(msg.Velocity)
                actionForm.CurrentControl = newCont
                newCont = Nothing
            End If
        ElseIf Not configMode And Not Connecting Then
            'Diagnostics.Debug.WriteLine("On : " & msg.Device.Name & " (" & Configuration.Connections(msg.Device.Name).Name & "), " & msg.Channel.ToString & ", " & msg.Pitch.ToString & ", " & msg.Velocity.ToString)
            Dim device As Integer = FindDevice(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (Configuration.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "9" & CByte(msg.Channel).ToString("X") & CByte(msg.Pitch).ToString("X2")
                If (Configuration.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(Configuration.Connections(device).Control(ContStr).Paged, Configuration.Connections(device).PageCurrent, CByte(0))
                    If (Configuration.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        ''End Checks
                        With Configuration.Connections(device).Control(ContStr).Page(_page)
                            If (Configuration.Connections(device).NoteOff) And (msg.Velocity = 0) Then
                                ''This is actually a "NoteOff", or is to be treated as one according to configuration
                                If ((.Behavior = 1) And Not (.IsActive)) Or (.Behavior = 0) Then
                                    ''This is a 'momentary' button [AND "IsActive" so is waiting to be turned off]
                                    '' OR
                                    ''This is a 'latch' button
                                    For Each actn As clsAction In .ActionsOff
                                        If (actn.Enabled) Then
                                            If Not (actn.Action Is Nothing) Then
                                                If Not (DirectCast(actn.Action, iAction).Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                    Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                                End If
                                            End If
                                        End If
                                    Next
                                    If (.Behavior = 0) Then .IsActive = False
                                End If
                            Else
                                If ((.Behavior = 1) And Not (.IsActive)) Or (.Behavior = 0) Then
                                    ''This is a 'latch' button [AND "IsActive" = False so is waiting to be turned on]
                                    '' OR
                                    ''This is a 'momentary' button
                                    For Each actn As clsAction In .Actions
                                        If (actn.Enabled) Then
                                            If Not (actn.Action Is Nothing) Then
                                                If Not (DirectCast(actn.Action, iAction).Execute(msg.Device.Name, 144, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                    Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                                .IsActive = Not (.IsActive)
                            End If
                        End With
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub NoteOff(ByVal msg As NoteOffMessage)
        ''Really no need to send this to actionForm in configMode, because we'd probably end up associating actions to button up actions by mistake.
        If Not configMode And Not Connecting Then
            'System.Diagnostics.Debug.WriteLine("Off: " & msg.Device.Name & " (" & Configuration.Connections(msg.Device.Name).Name & "), " & msg.Channel.ToString & ", " & msg.Pitch.ToString & ", " & msg.Velocity.ToString)
            Dim device As Integer = FindDevice(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (Configuration.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "9" & CByte(msg.Channel).ToString("X") & CByte(msg.Pitch).ToString("X2")
                If (Configuration.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(Configuration.Connections(device).Control(ContStr).Paged, Configuration.Connections(device).PageCurrent, CByte(0))
                    If (Configuration.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        ''End Checks
                        With Configuration.Connections(device).Control(ContStr).Page(_page)
                            If ((.Behavior = 1) And (Not .IsActive)) Or (.Behavior = 0) Then
                                ''This is a 'momentary' button [AND "IsActive" so is waiting to be turned off]
                                '' OR
                                ''This is a 'latch' button
                                For Each actn As clsAction In .ActionsOff
                                    If (actn.Enabled) Then
                                        If Not (actn.Action Is Nothing) Then
                                            If Not (DirectCast(actn.Action, iAction).Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                            End If
                                        End If
                                    End If
                                Next
                                If (.Behavior = 0) Then .IsActive = False
                            End If
                        End With
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ControlChange(ByVal msg As ControlChangeMessage)
        If configMode And Not (actionForm Is Nothing) Then
            ''override programmed action, redirect action to Configure Actions window
            If (actionForm.Visible) Then
                Dim newCont As curControl = New curControl
                newCont.Device = msg.Device.Name.ToString
                newCont.Channel = CByte(msg.Channel)
                newCont.Type = "Control"
                newCont.NotCon = CByte(msg.Control)
                newCont.VelVal = CByte(msg.Value)
                actionForm.CurrentControl = newCont
                newCont = Nothing
            End If
        ElseIf Not configMode And Not Connecting Then
            Dim device As Integer = FindDevice(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (Configuration.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "B" & CByte(msg.Channel).ToString("X") & CByte(msg.Control).ToString("X2")
                If (Configuration.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(Configuration.Connections(device).Control(ContStr).Paged, Configuration.Connections(device).PageCurrent, CByte(0))
                    If (Configuration.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        For Each actn As clsAction In Configuration.Connections(device).Control(ContStr).Page(_page).Actions
                            If (actn.Enabled) Then
                                If Not (actn.Action Is Nothing) Then
                                    If Not (DirectCast(actn.Action, iAction).Execute(msg.Device.Name, 176, CType(msg.Channel, Byte), CType(msg.Control, Byte), CType(msg.Value, Byte))) Then
                                        Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        End If
    End Sub
#End Region

#Region "Device Helpers"
    '''Map MIDI input and output names to device keys
    Public Function FindDevice(ByVal Device As String) As Integer
        'TODO: There's got to be a more efficient way of doing this...
        Dim nam As Integer = FindDeviceByName(Device)
        Dim ipt As Integer = FindDeviceByInput(Device)
        Dim opt As Integer = FindDeviceByOutput(Device)

        If Not (nam = -1) Then
            Return nam
        ElseIf Not (ipt = -1) Then
            Return ipt
        ElseIf Not (opt = -1) Then
            Return opt
        End If
        Return -1
    End Function
    Public Function FindDeviceByName(ByVal Name As String) As Integer
        For Each device As Integer In Configuration.Connections.Keys
            If (Configuration.Connections(device).Name = Name) Then Return device
        Next
        Return -1
    End Function
    Public Function FindDeviceByInput(ByVal Input As String) As Integer
        For Each device As Integer In Configuration.Connections.Keys
            If (Configuration.Connections(device).InputName = Input) Then Return device
        Next
        Return -1
    End Function
    Public Function FindDeviceByOutput(ByVal Output As String) As Integer
        For Each device As Integer In Configuration.Connections.Keys
            If (Configuration.Connections(device).OutputName = Output) Then Return device
        Next
        Return -1
    End Function

    Public Sub SendMidi(ByVal Device As Integer, ByVal Message As String)
        ''Make sure we're not wasting our time sending something to a device that doesn't exist
        If (Device = -1) Or (Not Configuration.Connections.ContainsKey(Device)) Then
            Exit Sub
        Else
            ''Ensure the device is enabled to receive
            If (Configuration.Connections(Device).Enabled And Configuration.Connections(Device).OutputEnable) Then
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
                Dim _device As String = Configuration.Connections(Device).OutputName

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
    Public Sub SendMidi(ByVal Device As Integer, ByVal Message As String())
        For Each _msg As String In Message
            SendMidi(Device, _msg)
        Next
    End Sub
    Public Sub SendMidi(ByVal Device As String, ByVal Message As String)
        If (Device = "ALL DEVICES") Then
            For Each dev As Integer In Configuration.Connections.Keys
                SendMidi(dev, Message)
            Next
        Else
            SendMidi(FindDevice(Device), Message)
        End If
    End Sub
    Public Sub SendMidi(ByVal Device As String, ByVal Message As String())
        For Each _msg As String In Message
            SendMidi(Device, _msg)
        Next
    End Sub
    'Public Sub SendMidi(ByVal Device As String, ByVal ControlType As Byte, ByVal Channel As Byte, ByVal NotCon As Byte)
    '    Dim ContStr As String = If(ControlType = 144 Or ControlType = 128, "9", "B") & Channel.ToString & NotCon.ToString("X2")
    '    Dim _device As Integer = FindDevice(Device)
    '    SendMidi(Device, ContStr)
    'End Sub
    'Public Sub SendMidi(ByVal Device As Integer, ByVal ControlType As Byte, ByVal channel As Byte, ByVal notcon As Byte)
    '    Dim ContStr As String = If(ControlType = 144 Or ControlType = 128, "9", "B") & channel.ToString & notcon.ToString("X2")
    '    SendMidi(Device, ContStr)
    'End Sub

    Public Sub SetPage(ByVal Device As Integer, ByVal Page As Byte)
        If (Device = -1) Or (Not Configuration.Connections.ContainsKey(Device)) Then
            Exit Sub
        Else
            Configuration.Connections(Device).PageCurrent = Page
            RedrawControls(Device)
        End If
    End Sub
    Public Sub SetPage(ByVal Device As String, ByVal Page As Byte)
        If (Device = "ALL DEVICES") Then
            For Each dev As clsConnection In Configuration.Connections.Values
                If (dev.Enabled = True) Then
                    dev.PageCurrent = Page
                End If
            Next
        Else
            Dim _device As Integer = FindDevice(Device)
            If (Configuration.Connections.ContainsKey(_device)) Then
                Configuration.Connections(_device).PageCurrent = Page
            End If
        End If
        RedrawControls(Device)
    End Sub

    Public Sub RedrawControls(ByVal Device As Integer)
        If (Device = -1) Or (Not Configuration.Connections.ContainsKey(Device)) Then
            Exit Sub
        Else
            With Configuration.Connections(Device)
                If (.Enabled And .OutputEnable) Then
                    For Each cont As clsControl In .Control.Values
                        If (cont.Page.ContainsKey(.PageCurrent)) Then
                            ''If this is the first time setting state, also set current state
                            '' otherwise use only CurrentState
                            If (cont.Page(.PageCurrent).CurrentState = "") Then
                                SendMidi(Device, cont.Page(.PageCurrent).InitialState)
                                cont.Page(.PageCurrent).CurrentState = cont.Page(.PageCurrent).InitialState
                            Else
                                SendMidi(Device, cont.Page(.PageCurrent).CurrentState)
                            End If
                        End If
                    Next
                End If
            End With
        End If
    End Sub
    Public Function RedrawControls(Optional ByVal Device As String = "ALL DEVICES") As Boolean
        If (Device = "ALL DEVICES") Then
            For Each _device As Integer In Configuration.Connections.Keys
                RedrawControls(_device)
            Next
        Else
            RedrawControls(FindDevice(Device))
        End If
    End Function
#End Region

#Region "Windows Message Functions"
#Region "Declarations"
    <Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True, CharSet:=Runtime.InteropServices.CharSet.Auto)> _
    Private Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    <Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True, CharSet:=Runtime.InteropServices.CharSet.Auto)> _
    Private Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function
    <Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Auto, SetLastError:=True)> _
    Private Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByRef lParam As CopyData) As IntPtr
    End Function

    ''Asynchronous PostMessage alternative
    '' Use insted of SendMessage() when you don't want to wait for the recipient program to finish processing the message
    <Runtime.InteropServices.DllImport("user32.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.StdCall, CharSet:=CharSet.Auto, EntryPoint:="PostMessageA", SetLastError:=True)> _
    Private Function PostMessage(ByVal hwnd As IntPtr, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32
    End Function

    Public Const WM_USER As Int32 = &H400 ''1024
    Public Const WM_COPYDATA As Integer = &H4A ''74
    Public Structure CopyData
        Public dwData As Integer
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure

    Private _ljHandle As IntPtr = IntPtr.Zero
    Private ReadOnly Property LJHandle() As IntPtr
        Get
            If (_ljHandle = IntPtr.Zero) Or (_ljHandle = Nothing) Then
                _ljHandle = GetHandle()
            End If
            Return _ljHandle
        End Get
    End Property
#End Region

#Region "Primary Functions"
    Public Function GetHandle() As IntPtr
        ''Find LightJockey Window
        ''Class name "TLJMainForm", window name "LightJockey"
        Return FindWindow("TLJMainForm", vbNullString)
        'Dim ret As IntPtr = FindWindow("TLJMainForm", vbNullString)
        'Diagnostics.Debug.WriteLine("FindWindow: " & ret.ToString)
        'Return ret
    End Function

    'Public Function SendMessage(ByVal handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    '    'Return SendMessage(handle, WM_USER + uMsg, wParam, lParam)
    '    Return CType(WindowsMessages.SendMessage(New IntPtr(handle), WM_USER + uMsg, New IntPtr(wParam), New IntPtr(lParam)), Integer)
    'End Function
    Public Function SendMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        Return If(Configuration.WmEnable, CType(SendMessage(LJHandle, WM_USER + uMsg, New IntPtr(wParam), New IntPtr(lParam)), Integer), -1)
    End Function

    Public Function PostMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        Return If(Configuration.WmEnable, CType(PostMessage(LJHandle, WM_USER + uMsg, wParam, lParam), Integer), -1)
    End Function

    Public Function SendCopyData(ByVal lParam As CopyData) As Integer
        'wParam is supposed to be a pointer to the handle of this process, or an HWND
        Return If(Configuration.WmEnable, CType(SendMessage(LJHandle, WM_COPYDATA, New IntPtr(0), lParam), Integer), -1)
    End Function
#End Region

#Region "Secondary Functions"
    Public Function GetReadyState(ByVal handle As IntPtr) As IntPtr
        ''Find out if LightJockey is ready
        ''Returns 1 if LJ is ready to receive messages, else returns 0
        Return SendMessage(handle, WM_USER + 1502, New IntPtr(0), New IntPtr(0))
    End Function

    Public Function GetVersion(ByVal handle As IntPtr) As IntPtr
        ''Returns an integer value. To get actual version number in "standard, human readable" form
        '' convert to hex, split by byte, convert to integer
        '' example: "39780608" -> "25F0100" -> "02", "5F", "01", "00" -> 2.95.1.0
        Return SendMessage(handle, WM_USER + 1502, New IntPtr(1), New IntPtr(0))
    End Function
#End Region

#Region "Tertiary Functions"
    Public Function GetSequence(ByVal handle As IntPtr) As Integer
        Return CType(SendMessage(handle, WM_USER + 1600, New IntPtr(256), New IntPtr(0)), Integer)
    End Function

    Public Function GetCue(ByVal handle As IntPtr) As Integer
        Return CType(SendMessage(handle, WM_USER + 1600, New IntPtr(257), New IntPtr(0)), Integer)
    End Function

    Public Function GetCueList(ByVal handle As IntPtr) As Integer
        Return CType(SendMessage(handle, WM_USER + 1600, New IntPtr(258), New IntPtr(0)), Integer)
    End Function

    Public Function GetBackgroundCue(ByVal handle As IntPtr) As Integer
        Return CType(SendMessage(handle, WM_USER + 1600, New IntPtr(262), New IntPtr(0)), Integer)
    End Function
#End Region
#End Region
End Module
