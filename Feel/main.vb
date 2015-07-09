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
    Public configForm As frmConfig
    Public actionForm As frmActions
    Public aboutForm As frmAbout

    Public Sub Main()
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

    Private Sub OpenActionWindow() Handles menuConfigActions.Click
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
    ''TODO: Rewrite this so workflow is more clear?
    Private Sub ConnectDevices()
        'TODO: Add a Try..Catch block, to determine if Configuration is corrupted/wrong version
        For Each device As clsConnection In Configuration.Connections.Values
            If (device.Enabled) Then
                If (device.Input + 1 <= InputDevice.InstalledDevices.Count) And (device.Output + 1 <= OutputDevice.InstalledDevices.Count) Then 'Is the stored connection index out of bounds?
                    If Not (InputDevice.InstalledDevices.Item(device.Input).IsOpen) And Not (OutputDevice.InstalledDevices.Item(device.Output).IsOpen) Then 'Are the stored ports already in use?
                        If (InputDevice.InstalledDevices.Item(device.Input).Name = device.InputName) And (OutputDevice.InstalledDevices.Item(device.Output).Name = device.OutputName) Then 'Ensure stored indexes match stored names for that port
                            ''We've passed all of our checks at this point, and it should be safe to connect.
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
                            Dim outDevice As OutputDevice
                            outDevice = OutputDevice.InstalledDevices.Item(device.Output)
                            midiOut.Add(device.OutputName, outDevice)
                            midiOut.Item(device.OutputName).Open()
                            outDevice = Nothing

                            ''Device should be connected, so now we take any "initialization" steps listed in the config
                            If Not (String.IsNullOrEmpty(device.Init)) Then
                                Dim initCmds() As String = device.Init.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                                UpdateControlState(device.OutputName, initCmds)
                            End If

                            RedrawControls(device.OutputName)
                        Else
                            ''If we end up in this block, the stored name for a given input or output index does not match the 
                            '' current name of the connection at that index. This is a pretty good indicator that indexes have 
                            '' changed, and we'd try to connect to the wrong device.
                            device.Enabled = False
                            System.Diagnostics.Debug.WriteLine("Disabled 1")
                            '.Input = -1
                            '.InputName = ""
                            '.Output = -1
                            '.OutputName = ""
                        End If
                    Else
                        System.Windows.Forms.MessageBox.Show("Warning: One or both of the input/output ports for the device named " & device.Name & " is already in use! Either the device is already in use elsewhere, or the connection information is incorrectly set." & vbCrLf & vbCrLf & "If you receive this error and you believe the device is connected properly, update this device's connection info by opening the 'Configure Connections' window and adjusting this device's Input and Output settings.", "Feel: MIDI Port In Use")
                    End If
                Else
                    ''If we end up in this block, the stored index for .Input or .Output is greater than
                    '' the number of currently installed input devices. We can then assume the
                    '' connection information is old, and reset it.
                    device.Enabled = False
                    System.Diagnostics.Debug.WriteLine("Disabled 2")
                    '.Input = -1
                    '.InputName = ""
                    '.Output = -1
                    '.OutputName = ""
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Closes and clears all connections, and removes all event handlers
    ''' </summary>
    ''' <param name="which">(Byte) 1: Close incoming, 2: Close outgoing, 0: Close both</param>
    ''' <remarks>Called when closing application, or opening Connection Configuration window</remarks>
    Public Sub DisconnectDevices(Optional ByVal which As Byte = 0)
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
        ElseIf Not configMode Then
            'Diagnostics.Debug.WriteLine("On : " & msg.Device.Name & " (" & Configuration.Connections(msg.Device.Name).Name & "), " & msg.Channel.ToString & ", " & msg.Pitch.ToString & ", " & msg.Velocity.ToString)
            If (Configuration.Connections.ContainsKey(msg.Device.Name)) Then
                Dim ContStr As String = "9" & CByte(msg.Channel).ToString("X") & CByte(msg.Pitch).ToString("X2")
                If (Configuration.Connections(msg.Device.Name).Control.ContainsKey(ContStr)) Then
                    If (Configuration.Connections(msg.Device.Name).Control(ContStr).Page.ContainsKey(Configuration.Connections(msg.Device.Name).PageCurrent)) Then
                        ''End Checks
                        If (Configuration.Connections(msg.Device.Name).NoteOff) And (msg.Velocity = 0) Then
                            'This is actually a "NoteOff"
                            For Each actn As clsAction In Configuration.Connections(msg.Device.Name).Control(ContStr).Page(Configuration.Connections(msg.Device.Name).PageCurrent).ActionsOff
                                If (actn.Enabled) Then
                                    If Not (actn.Action Is Nothing) Then
                                        If Not (DirectCast(actn.Action, iAction).Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                            Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            For Each actn As clsAction In Configuration.Connections(msg.Device.Name).Control(ContStr).Page(Configuration.Connections(msg.Device.Name).PageCurrent).Actions
                                If (actn.Enabled) Then
                                    If Not (actn.Action Is Nothing) Then
                                        If Not (DirectCast(actn.Action, iAction).Execute(msg.Device.Name, 144, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                            Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub NoteOff(ByVal msg As NoteOffMessage)
        ''Really no need to send this to actionForm in configMode, because we'd probably end up associating actions to button up actions by mistake.
        If Not configMode Then
            'System.Diagnostics.Debug.WriteLine("Off: " & msg.Device.Name & " (" & Configuration.Connections(msg.Device.Name).Name & "), " & msg.Channel.ToString & ", " & msg.Pitch.ToString & ", " & msg.Velocity.ToString)
            If (Configuration.Connections.ContainsKey(msg.Device.Name)) Then
                Dim ContStr As String = "9" & CByte(msg.Channel).ToString("X") & CByte(msg.Pitch).ToString("X2")
                If (Configuration.Connections(msg.Device.Name).Control.ContainsKey(ContStr)) Then
                    If (Configuration.Connections(msg.Device.Name).Control(ContStr).Page.ContainsKey(Configuration.Connections(msg.Device.Name).PageCurrent)) Then
                        ''End Checks
                        For Each actn As clsAction In Configuration.Connections(msg.Device.Name).Control(ContStr).Page(Configuration.Connections(msg.Device.Name).PageCurrent).ActionsOff
                            If (actn.Enabled) Then
                                If Not (actn.Action Is Nothing) Then
                                    If Not (DirectCast(actn.Action, iAction).Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
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
        ElseIf Not configMode Then
            If (Configuration.Connections.ContainsKey(msg.Device.Name)) Then
                Dim ContStr As String = "B" & CByte(msg.Channel).ToString("X") & CByte(msg.Control).ToString("X2")
                If (Configuration.Connections(msg.Device.Name).Control.ContainsKey(ContStr)) Then
                    If (Configuration.Connections(msg.Device.Name).Control(ContStr).Page.ContainsKey(Configuration.Connections(msg.Device.Name).PageCurrent)) Then
                        ''End Checks
                        For Each actn As clsAction In Configuration.Connections(msg.Device.Name).Control(ContStr).Page(Configuration.Connections(msg.Device.Name).PageCurrent).Actions
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
    '''Temporary hard-coded stuff for APC units
    ''If (device.Model = "APC40") Or (device.Model = "APC20") Then
    ''    midiOut.Item(device.midiOutName).SendSysEx(New Byte() {&HF0, &H47, &H0, CByte(If(device.Model = "APC40", &H73, &H7B)), &H60, &H0, &H4, CByte(If(device.Mode = 0, &H40, If(device.Mode = 1, &H41, &H42))), &H8, &H2, &H6, &HF7})
    ''    ''TODO: I'm just playing here, but these are examples of how to turn the various LEDs on and off
    ''    If (device.Model = "APC40") Then
    ''        '' The first controls buttons (SendNoteOn)
    ''        'midiOut.Item(device.midiOutName).SendNoteOn(Channel.Channel1, Pitch.F3, colorAPC.Off) '90 53 00
    ''        '' The second controls LED rings (SendControlChange)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(31, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(30, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(29, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(28, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(27, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(26, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(25, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(24, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(63, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(62, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(61, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(60, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(59, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(58, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(57, Midi.Control), 0)
    ''        midiOut.Item(device.midiOutName).SendControlChange(Channel.Channel1, CType(56, Midi.Control), 0)
    ''    End If
    ''End If
    Public Sub UpdateControlState(ByVal Device As String, ByVal State As String)
        ''Remove whitespace from commands, and convert to uppercase
        Dim regex As New Text.RegularExpressions.Regex("\s")
        State = regex.Replace(State, String.Empty)
        State = UCase(State)

        ''Just a failsafe, don't need to do anything with empty States
        If (String.IsNullOrEmpty(State)) Then Exit Sub

        ''Get first character to determine command type (See Select statement below)
        Dim cmdType As String = State.Substring(0, 1)
        If (cmdType = "#") Then Exit Sub 'Comment

        ''Convert to byte array
        Dim len As Integer = State.Length
        Dim upperBound As Integer = len \ 2
        If ((len Mod 2) = 0) Then
            upperBound -= 1
        Else
            State = "0" & State
        End If
        Dim cmdArr(upperBound) As Byte
        For i As Integer = 0 To upperBound
            cmdArr(i) = Convert.ToByte(State.Substring(i * 2, 2), 16)
        Next

        ''Take appropriate action
        Select Case cmdType
            ''Unsupported:
            'Case "A" 'Polyphonic Aftertouch
            'Case "D" 'Channel Pressure (Aftertouch)

            Case "8" 'MIDI Note Off
                'midiOut.Item(device.OutputName).SendNoteOff(CType(cmd.Substring(1, 1), Midi.Channel), CType(Convert.ToInt16(cmd.Substring(2, 2)), Midi.Pitch), Convert.ToInt16(cmd.Substring(4, 2)))
                midiOut.Item(Device).SendNoteOff(CType(State.Substring(1, 1), Midi.Channel), CType(cmdArr(1), Midi.Pitch), cmdArr(2))
            Case "9" 'MIDI Note On
                midiOut.Item(Device).SendNoteOn(CType(State.Substring(1, 1), Midi.Channel), CType(cmdArr(1), Midi.Pitch), cmdArr(2))
            Case "B" 'Control Change
                midiOut.Item(Device).SendControlChange(CType(State.Substring(1, 1), Midi.Channel), CType(cmdArr(1), Midi.Control), cmdArr(2))
            Case "C" 'Program Change
                midiOut.Item(Device).SendProgramChange(CType(State.Substring(1, 1), Midi.Channel), CType(cmdArr(1), Midi.Instrument))
            Case "E" 'Pitch Wheel Change (Pitch Bend)
                midiOut.Item(Device).SendPitchBend(CType(State.Substring(1, 1), Midi.Channel), cmdArr(1))
            Case "F" 'MIDI System-Common Message
                If (State.Substring(1, 1) = "0") Then 'MIDI Sysex Message
                    midiOut.Item(Device).SendSysEx(cmdArr)
                End If
        End Select
    End Sub
    Public Sub UpdateControlState(ByVal Device As String, ByVal State As String())
        For Each _state As String In State
            UpdateControlState(Device, _state)
        Next
    End Sub
    Public Sub UpdateControlState(ByVal Device As String, ByVal ControlType As Byte, ByVal Channel As Byte, ByVal NotCon As Byte)
        Dim ContStr As String = If(ControlType = 144 Or ControlType = 128, "9", "B") & Channel.ToString & NotCon.ToString("X2")
        'Dim CurState As String = Configuration.Connections(Device).Control(ContStr).Page(0).CurrentState
        If (Configuration.Connections.ContainsKey(Device)) Then
            If (Configuration.Connections(Device).Control.ContainsKey(ContStr)) Then
                UpdateControlState(Device, Configuration.Connections(Device).Control(ContStr).Page(0).CurrentState)
            End If
        End If
    End Sub

    Public Function SetPage(ByVal Device As String, ByVal Page As Byte) As Boolean
        If (Device = "ALL DEVICES") Then
            For Each dev As clsConnection In Configuration.Connections.Values
                dev.PageCurrent = Page
            Next
        Else
            If Not (Configuration.Connections.ContainsKey(Device)) Then
                Return False
            Else
                Configuration.Connections(Device).PageCurrent = Page
            End If
        End If
        Return RedrawControls(Device)
    End Function

    Public Function RedrawControls(ByVal Device As String) As Boolean
        If (Device = "ALL DEVICES") Then
            For Each dev As clsConnection In Configuration.Connections.Values
                _RedrawControls(Device)
            Next
        Else
            _RedrawControls(Device)
        End If
    End Function
    Private Function _RedrawControls(ByVal Device As String) As Boolean
        For Each cont As clsControl In Configuration.Connections(Device).Control.Values
            If (cont.Page.ContainsKey(Configuration.Connections(Device).PageCurrent)) Then
                UpdateControlState(Device, cont.Page(Configuration.Connections(Device).PageCurrent).CurrentState)
            End If
        Next
    End Function

    Public Function SendMidiNoteOn(ByVal Device As String, ByVal Channel As Byte, ByVal Note As Byte, ByVal Velocity As Byte) As Boolean
        Diagnostics.Debug.WriteLine("Device.SendMidiNoteOn handled: " & Device)
        Return True
    End Function

    Public Function SendMidiNoteOff(ByVal Device As String, ByVal Channel As Byte, ByVal Note As Byte, ByVal Velocity As Byte) As Boolean
        Diagnostics.Debug.WriteLine("Device.SendMidiNoteOff handled: " & Device)
        Return True
    End Function

    Public Function SendSysex(ByVal Device As String, ByVal Message As Byte()) As Boolean
        Diagnostics.Debug.WriteLine("Device.SendSysex handled: " & Message(0).ToString)
        Return True
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
        Return CType(SendMessage(LJHandle, WM_USER + uMsg, New IntPtr(wParam), New IntPtr(lParam)), Integer)
    End Function

    Public Function SendCopyData(ByVal lParam As CopyData) As Integer
        'wParam is supposed to be a pointer to the handle of this process, or an HWND
        Return CType(SendMessage(LJHandle, WM_COPYDATA, New IntPtr(0), lParam), Integer)
    End Function

    Public Function PostMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        Return CType(PostMessage(LJHandle, WM_USER + uMsg, wParam, lParam), Integer)
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
