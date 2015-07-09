Imports System.Windows.Forms
Imports Midi
Imports System.Runtime.InteropServices

Module main
    ''Components for System Tray icon
    Dim WithEvents trayIcon As New NotifyIcon
    Dim trayMenu As New ContextMenu
    Dim WithEvents menuExit As New MenuItem
    Dim WithEvents menuConfigProgram As New MenuItem
    Dim WithEvents menuConfigConnections As New MenuItem
    Dim WithEvents menuConfigActions As New MenuItem
    Dim menuAdvancedTasks As New MenuItem
    Dim WithEvents menuRefreshWindowHandle As New MenuItem
    Dim WithEvents menuSaveConfiguration As New MenuItem
    Dim WithEvents menuUpdateAvailableDevices As New MenuItem
    Dim WithEvents menuAbout As New MenuItem

    ''Container for program configuration
    Public FeelConfig As clsConfig

    ''Collections of MIDI input and output devices
    Public midiIn As System.Collections.Generic.Dictionary(Of String, InputDevice) = New Collections.Generic.Dictionary(Of String, InputDevice)
    Public midiOut As System.Collections.Generic.Dictionary(Of String, OutputDevice) = New Collections.Generic.Dictionary(Of String, OutputDevice)

    Private _configMode As Boolean = False    ''when configuring actions, override output to LJ
    Public Property configMode(Optional ByVal reConnect As Boolean = False) As Boolean
        Get
            Return _configMode
        End Get
        Set(ByVal value As Boolean)
            If Not (configForm Is Nothing) Then
                If (configForm.Visible) Then
                    _configMode = True
                End If
            ElseIf Not (connectForm Is Nothing) Then
                If (connectForm.Visible) Then
                    _configMode = True
                End If
            ElseIf Not (actionForm Is Nothing) Then
                If (actionForm.Visible) Then
                    _configMode = True
                End If
            End If
            'TODO: reevaluate the logic of this If
            If _configMode <> value Then
                _configMode = value
                If reConnect Then
                    DisconnectDevices() 'need this?
                    ConnectDevices()
                End If
            End If
        End Set
    End Property

    ''Returns a boolean indicating whether it is safe to handle an event's actions.
    '' True = do not handle event's actions
    Private _connecting As Boolean = False
    Public Property Connecting() As Boolean
        Get
            Return (_connecting And FeelConfig.IgnoreEvents)
        End Get
        Set(ByVal value As Boolean)
            _connecting = value
        End Set
    End Property

    Public configForm As frmConfiguration
    Public connectForm As frmConnections
    Private actionForm As frmActions = New frmActions
    Public aboutForm As frmAbout

    Friend actionModules As Collections.Generic.Dictionary(Of Guid, ActionInterface.IAction) = New Collections.Generic.Dictionary(Of Guid, ActionInterface.IAction)
    'Private actionModules As Collections.Generic.Dictionary(Of String, ActionInterface.IAction)
    Public serviceHost As ActionInterface.IServices = New Interfaces.ServiceHost

    Public Sub Main()
        For Each arg As String In My.Application.CommandLineArgs
            'TODO:
            System.Diagnostics.Debug.WriteLine("Command Line Arg: " & arg)
        Next

        ''Aparently this is required to show Groups in ListView controls
        System.Windows.Forms.Application.EnableVisualStyles()

        ''create system tray icon and context menu items
        menuExit.Text = "E&xit"
        menuConfigProgram.Text = "&Program Configuration"
        menuConfigConnections.Text = "Configure &Connections"
        menuConfigActions.Text = "Configure &Actions"
        ''NEW:
        menuAdvancedTasks.Text = "Advanced &Tasks"
        menuRefreshWindowHandle.Text = "&Refresh LJ Handle"
        menuSaveConfiguration.Text = "&Save Configuration"
        menuUpdateAvailableDevices.Text = "&Update Available Devices"
        '':NEW
        menuAbout.Text = "About"
        trayMenu.MenuItems.Add(menuExit)
        trayMenu.MenuItems.Add("-")
        trayMenu.MenuItems.Add(menuAbout)
        trayMenu.MenuItems.Add("-")
        menuAdvancedTasks.MenuItems.Add(menuSaveConfiguration)
        menuAdvancedTasks.MenuItems.Add(menuRefreshWindowHandle)
        menuAdvancedTasks.MenuItems.Add(menuUpdateAvailableDevices)
        trayMenu.MenuItems.Add(menuAdvancedTasks)
        'trayMenu.MenuItems.Add("-")
        trayMenu.MenuItems.Add(menuConfigProgram)
        trayMenu.MenuItems.Add(menuConfigConnections)
        trayMenu.MenuItems.Add(menuConfigActions)
        trayIcon.ContextMenu = trayMenu
        'trayIcon.Icon = New System.Drawing.Icon(System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream("Feel.feel.ico"))
        trayIcon.Icon = My.Resources.Feel.feel
        trayIcon.Visible = True

        LoadModules() ''Populate our list of currently available action plug-ins

        LoadConfiguration() ''Read serialzed config data from file
        If Not _configMode Then ConnectDevices() ''Make MIDI connections

        Application.Run()
    End Sub

#Region "Windows & Menus"
    Private Sub menuExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles menuExit.Click
        If _configMode Then
            Dim quitNoSave As System.Windows.Forms.DialogResult = MessageBox.Show("One or more configuration windows are open; any changes made have not been saved. Do you want to exit without saving?", "Feel: Configuration Not Saved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If (quitNoSave = DialogResult.No) Then Exit Sub
        End If

        DisconnectDevices()

        trayIcon.Visible = False
        Application.Exit()
    End Sub

    Friend Sub OpenProgramWindow() Handles menuConfigProgram.Click
        ''Singleton: See designer code.
        '' http://www.codeproject.com/Articles/5000/Simple-Singleton-Forms
        '' (dated link) http://www.codeproject.com/KB/vb/Simple_Singleton_Forms.aspx
        If configForm Is Nothing Then
            configForm = New frmConfiguration
        End If
        configForm.Show()
        'configForm.Focus()
    End Sub

    Friend Sub OpenConnectWindow() Handles menuConfigConnections.Click
        ''The Connections Configuration form makes its own connections to
        '' test devices, so we must disconnect existing connections first.
        DisconnectDevices()

        If connectForm Is Nothing Then
            connectForm = New frmConnections
        End If
        connectForm.Show()
        'configForm.Focus()
    End Sub

    Delegate Sub dlgOpenActionWindow()
    Friend Sub OpenActionWindow() Handles menuConfigActions.Click
        'If actionForm Is Nothing Then
        '    actionForm = New frmActions
        '    actionForm.Show()
        'Else
        '    If actionForm.InvokeRequired Then
        '        actionForm.Invoke(New dlgOpenActionWindow(AddressOf OpenActionWindow))
        '    Else
        '        actionForm.Show()
        '    End If
        'End If
        If actionForm Is Nothing Then
            actionForm = New frmActions
        End If
        actionForm.Show()
    End Sub
    'Delegate Sub dlgDisposeActionWindow()
    Friend Sub DisposeActionWindow()
        'If actionForm.InvokeRequired Then
        '    actionForm.Invoke(New dlgDisposeActionWindow(AddressOf DisposeActionWindow))
        'Else
        '    actionForm = Nothing
        'End If
        actionForm = Nothing
    End Sub

    Private Sub OpenAbout() Handles menuAbout.Click
        If aboutForm Is Nothing Then
            aboutForm = New frmAbout
        End If
        aboutForm.Show()
    End Sub

    ''' <summary>
    ''' Sets the _ljHandle variable to a blank pointer.
    ''' </summary>
    ''' <remarks>'Resetting' _ljHandle triggers a new search for LightJockey's window handle
    '''  upon the next use. Useful, for instance, if LightJockey was restarted.</remarks>
    Friend Sub RefreshHandle() Handles menuRefreshWindowHandle.Click
        _ljHandle = IntPtr.Zero
    End Sub

    ''See: #Region "File I/O", SaveConfiguration()
    'Private Sub SaveConfiguration() Handles menuSaveConfiguration.Click

    Friend Sub UpdateAvailableDevices() Handles menuUpdateAvailableDevices.Click
        Midi.InputDevice.UpdateInstalledDevices()
        Midi.OutputDevice.UpdateInstalledDevices()
        'TODO: update lists in Connection Config, if in ConfigMode
    End Sub
#End Region

#Region "File I/O"
    Friend Sub LoadConfiguration()
        ''Check to make sure a configuration file has been set, if not make one
        If (My.Settings.ConfigFile = "") Then
            My.Settings.ConfigFile = Application.StartupPath & "\user\" & "config.feel"
            My.Settings.Save()
        End If
LoadConfig:
        Dim fs As IO.FileStream
        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        Try
            fs = New IO.FileStream(My.Settings.ConfigFile, IO.FileMode.Open, IO.FileAccess.Read)
            bf.Binder = New DeserializationBinder() ''See DeserializationBinder
            FeelConfig = ConfigDeserialize(CType(bf.Deserialize(fs), clsConfig)) 'FeelConfig = CType(bf.Deserialize(fs), clsConfig)
            fs.Close()
        Catch fileEx As IO.FileNotFoundException
            'MessageBox.Show("No configuration file found. Operating with blank configuration.", "Feel: Configuration Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            trayIcon.ShowBalloonTip(10000, "Feel: Notice", "No configuration file was found. A blank configuration file was created.", ToolTipIcon.Info)
            ''no configuration file was found, so create a new one
            CreateNewConfiguration()
            ''then open the configuration window
            OpenProgramWindow()
        Catch directoryEx As IO.DirectoryNotFoundException
            If CreateUserDirectory() Then
                GoTo LoadConfig
            Else
                'MessageBox.Show("Unable to create 'user' directory. Operating with blank configuration file.", "Feel: File System Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                trayIcon.ShowBalloonTip(10000, "Feel: Warning", "Unable to create 'user' directory. Operation will continue with a blank configuration.", ToolTipIcon.Warning)
                CreateNewConfiguration()
                OpenProgramWindow()
            End If
        Catch ex As Exception
            ''some unexpected error occured
            MessageBox.Show("Unexpected error trying to read Feel configuration file." & vbCrLf & vbCrLf & ex.Message, "Feel: Unknown Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'trayIcon.ShowBalloonTip(5000, "Feel: Warning", "Unexpected error trying to read configuration file.", ToolTipIcon.Error)
        Finally
            fs = Nothing
            bf = Nothing
        End Try
    End Sub

    ''TODO: Does this really need to be broken out into its own subroutine?
    Friend Sub CreateNewConfiguration()
        FeelConfig = New clsConfig
    End Sub

    Private Function CreateUserDirectory() As Boolean
        Try
            Dim dInfo As IO.DirectoryInfo = IO.Directory.CreateDirectory(Application.StartupPath & "\user")
            Return dInfo.Exists
        Catch ex As Exception
            MessageBox.Show("Error creating 'user' directory!", "Feel: I/O Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Public Sub SaveConfiguration() Handles menuSaveConfiguration.Click
        Dim fs As IO.FileStream
        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        Try
            fs = New IO.FileStream(My.Settings.ConfigFile, IO.FileMode.Create)
            bf.Serialize(fs, FeelConfig) 'bf.Serialize(fs, ConfigSerialize(FeelConfig))
            fs.Close()
        Catch ex As Exception
            MessageBox.Show("An unexpected error has occured in the 'SaveConnectionConfig' procedure." & vbCrLf & vbCrLf & ex.Message, "Feel: Unexpected Error")
        Finally
            fs = Nothing
            bf = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Serializes Action plug-in .Data Objects independently to prepare Configuration for storage.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ConfigSerialize(ByRef Configuration As clsConfig) As clsConfig
        Dim _newConfig As clsConfig = ObjectCopier.Clone(Configuration)
        Dim st As New IO.MemoryStream
        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
        For Each conx As clsConnection In _newConfig.Connections.Values
            For Each cont As clsControl In conx.Control.Values
                For Each page As clsControlPage In cont.Page.Values
                    For Each act As clsAction In page.Actions
                        If (act._available) Then
                            'serialzize
                            Try
                                bf.Serialize(st, act.Data)
                                act.Data = st 'act.Data = st.ToArray
                                act._available = False
                            Catch ex As Exception
                                MessageBox.Show("There was an unexpected error trying to serialize (save) an Action. Details:" & vbCrLf & vbCrLf & ex.Message, "Feel: Serialziation Error")
                            Finally
                                st = New IO.MemoryStream
                            End Try
                        End If
                    Next
                    For Each act As clsAction In page.ActionsOff
                        If (act._available) Then
                            Try
                                bf.Serialize(st, act.Data)
                                act.Data = st 'act.Data = st.ToArray
                                act._available = False
                            Catch ex As Exception
                                MessageBox.Show("There was an unexpected error trying to serialize (save) an Action. Details:" & vbCrLf & vbCrLf & ex.Message, "Feel: Serialziation Error")
                            Finally
                                st = New IO.MemoryStream
                            End Try
                        End If
                    Next
                Next
            Next
        Next
        st.Close()
        st = Nothing
        bf = Nothing
        Return _newConfig
    End Function

    ''' <summary>
    ''' Deserializes Action plug-in .Data Objects and returns a usable Configuration.
    ''' </summary>
    ''' <remarks>Must be run after plug-ins are loaded. ConfigDeserialzie searches availability of plug-ins before deserialziing the .Data object for each plug-in; unavailable plug-ins' .Data are ignored.</remarks>
    Private Function ConfigDeserialize(ByRef Configuration As clsConfig) As clsConfig
        For Each conx As clsConnection In Configuration.Connections.Values
            For Each cont As clsControl In conx.Control.Values
                For Each page As clsControlPage In cont.Page.Values
                    For Each act As clsAction In page.Actions
                        If (actionModules.ContainsKey(act.Type)) Then
                            act._available = True
                        Else
                            act._available = False
                        End If
                    Next
                    For Each act As clsAction In page.ActionsOff
                        If (actionModules.ContainsKey(act.Type)) Then
                            act._available = True
                        Else
                            act._available = False
                        End If
                    Next
                Next
            Next
        Next
        Return Configuration
    End Function
    'Private Function ConfigDeserialize_OLD(ByRef Configuration As clsConfig) As clsConfig
    '    Dim st As New IO.MemoryStream
    '    Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
    '    For Each conx As clsConnection In Configuration.Connections.Values
    '        For Each cont As clsControl In conx.Control.Values
    '            For Each page As clsControlPage In cont.Page.Values
    '                For Each act As clsAction In page.Actions
    '                    If (actionModules.ContainsKey(act.Type)) Then
    '                        'deserialize
    '                        st = CType(act.Data, IO.MemoryStream) 'st = New IO.MemoryStream(CType(act.Data, Byte())) 'TODO: not sure about this...
    '                        Try
    '                            bf.Binder = New DeserializationBinder() ''See DeserializationBinder
    '                            act.Data = bf.Deserialize(st)
    '                        Catch ex As Exception
    '                            MessageBox.Show("There was an unexpected error trying to deserialize (load) an Action. Details:" & vbCrLf & vbCrLf & ex.Message, "Feel: Serialziation Error")
    '                        End Try
    '                        act._available = True
    '                    Else
    '                        'don't deserialize
    '                        act._available = False
    '                    End If
    '                Next
    '                For Each act As clsAction In page.ActionsOff
    '                    If (actionModules.ContainsKey(act.Type)) Then
    '                        'deserialize
    '                        st = CType(act.Data, IO.MemoryStream) 'st = New IO.MemoryStream(CType(act.Data, Byte())) 'TODO: not sure about this...
    '                        Try
    '                            bf.Binder = New DeserializationBinder() ''See DeserializationBinder
    '                            act.Data = bf.Deserialize(st)
    '                        Catch ex As Exception
    '                            MessageBox.Show("There was an unexpected error trying to deserialize (load) an Action. Details:" & vbCrLf & vbCrLf & ex.Message, "Feel: Serialziation Error")
    '                        End Try
    '                        act._available = True
    '                    Else
    '                        'don't deserialize
    '                        act._available = False
    '                    End If
    '                Next
    '            Next
    '        Next
    '    Next
    '    st.Close()
    '    st = Nothing
    '    bf = Nothing
    '    Return Configuration
    'End Function
#End Region

#Region "Connections"
    ''' <summary>
    ''' Makes MIDI connections to all enabled devices
    ''' </summary>
    ''' <remarks>Ver:2.1</remarks>
    Private Sub ConnectDevices()
        Connecting = True

        For Each device As clsConnection In FeelConfig.Connections.Values
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
                    'trayIcon.ShowBalloonTip(10000, "Feel: MIDI Configuration has changed", "", ToolTipIcon.Warning)
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
                    Dim _device As Integer = serviceHost.FindDeviceIndexByName(device.Name)
                    If Not (String.IsNullOrEmpty(device.Init)) Then
                        Dim initCmds() As String = device.Init.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                        serviceHost.SendMIDI(_device, initCmds)
                    End If
                    serviceHost.RedrawControls(_device)
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
                Dim newCont As frmActions.curControl = New frmActions.curControl
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
            Dim device As Integer = serviceHost.FindDeviceIndex(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (FeelConfig.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "9" & CByte(msg.Channel).ToString("X") & CByte(msg.Pitch).ToString("X2")
                If (FeelConfig.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(FeelConfig.Connections(device).Control(ContStr).Paged, FeelConfig.Connections(device).PageCurrent, CByte(0))
                    If (FeelConfig.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        ''End Checks
                        With FeelConfig.Connections(device).Control(ContStr).Page(_page)
                            If (FeelConfig.Connections(device).NoteOff) And (msg.Velocity = 0) Then
                                ''This is actually a "NoteOff", or is to be treated as one according to configuration
                                If ((.Behavior = 1) And Not (.IsActive)) Or (.Behavior = 0) Then
                                    ''This is a 'momentary' button [AND "IsActive" so is waiting to be turned off]
                                    '' OR
                                    ''This is a 'latch' button
                                    For Each actn As clsAction In .ActionsOff
                                        If (actn.Enabled) And (actn._available) Then
                                            'TODO: actn.Data should never be Nothing, if ._available is set to True
                                            If Not (actn.Data Is Nothing) Then
                                                'If Not (DirectCast(actn.Data, iAction).Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                '    Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                                'End If
                                                With actionModules.Item(actn.Type)
                                                    .Data = actn.Data
                                                    If Not (.Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                        Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                                    End If
                                                End With
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
                                        If (actn.Enabled) And (actn._available) Then
                                            If Not (actn.Data Is Nothing) Then
                                                'If Not (DirectCast(actn.Data, iAction).Execute(msg.Device.Name, 144, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                '    Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                                'End If
                                                With actionModules.Item(actn.Type)
                                                    .Data = actn.Data
                                                    If Not (.Execute(msg.Device.Name, 144, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                        Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                                    End If
                                                End With
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
            Dim device As Integer = serviceHost.FindDeviceIndex(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (FeelConfig.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "9" & CByte(msg.Channel).ToString("X") & CByte(msg.Pitch).ToString("X2")
                If (FeelConfig.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(FeelConfig.Connections(device).Control(ContStr).Paged, FeelConfig.Connections(device).PageCurrent, CByte(0))
                    If (FeelConfig.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        ''End Checks
                        With FeelConfig.Connections(device).Control(ContStr).Page(_page)
                            If ((.Behavior = 1) And (Not .IsActive)) Or (.Behavior = 0) Then
                                ''This is a 'momentary' button [AND "IsActive" so is waiting to be turned off]
                                '' OR
                                ''This is a 'latch' button
                                For Each actn As clsAction In .ActionsOff
                                    If (actn.Enabled) And (actn._available) Then
                                        If Not (actn.Data Is Nothing) Then
                                            'If Not (DirectCast(actn.Data, iAction).Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                            '    Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                            'End If
                                            With actionModules.Item(actn.Type)
                                                .Data = actn.Data
                                                If Not (.Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                    Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                                End If
                                            End With
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
                Dim newCont As frmActions.curControl = New frmActions.curControl
                newCont.Device = msg.Device.Name.ToString
                newCont.Channel = CByte(msg.Channel)
                newCont.Type = "Control"
                newCont.NotCon = CByte(msg.Control)
                newCont.VelVal = CByte(msg.Value)
                actionForm.CurrentControl = newCont
                newCont = Nothing
            End If
        ElseIf Not configMode And Not Connecting Then
            Dim device As Integer = serviceHost.FindDeviceIndex(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (FeelConfig.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "B" & CByte(msg.Channel).ToString("X") & CByte(msg.Control).ToString("X2")
                If (FeelConfig.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(FeelConfig.Connections(device).Control(ContStr).Paged, FeelConfig.Connections(device).PageCurrent, CByte(0))
                    If (FeelConfig.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        For Each actn As clsAction In FeelConfig.Connections(device).Control(ContStr).Page(_page).Actions
                            If (actn.Enabled) And (actn._available) Then
                                If Not (actn.Data Is Nothing) Then
                                    'If Not (DirectCast(actn.Data, iAction).Execute(msg.Device.Name, 176, CType(msg.Channel, Byte), CType(msg.Control, Byte), CType(msg.Value, Byte))) Then
                                    '    Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                    'End If
                                    With actionModules.Item(actn.Type)
                                        .Data = actn.Data
                                        If Not (.Execute(msg.Device.Name, 176, CType(msg.Channel, Byte), CType(msg.Control, Byte), CType(msg.Value, Byte))) Then
                                            Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                        End If
                                    End With
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
    'Public Function FindDevice(ByVal Device As String) As Integer
    '    'TODO: There's got to be a more efficient way of doing this...
    '    Dim nam As Integer = FindDeviceByName(Device)
    '    Dim ipt As Integer = FindDeviceByInput(Device)
    '    Dim opt As Integer = FindDeviceByOutput(Device)

    '    If Not (nam = -1) Then
    '        Return nam
    '    ElseIf Not (ipt = -1) Then
    '        Return ipt
    '    ElseIf Not (opt = -1) Then
    '        Return opt
    '    End If
    '    Return -1
    'End Function
    'Public Function FindDeviceByName(ByVal Name As String) As Integer
    '    For Each device As Integer In FeelConfig.Connections.Keys
    '        If (FeelConfig.Connections(device).Name = Name) Then Return device
    '    Next
    '    Return -1
    'End Function
    'Public Function FindDeviceByInput(ByVal Input As String) As Integer
    '    For Each device As Integer In FeelConfig.Connections.Keys
    '        If (FeelConfig.Connections(device).InputName = Input) Then Return device
    '    Next
    '    Return -1
    'End Function
    'Public Function FindDeviceByOutput(ByVal Output As String) As Integer
    '    For Each device As Integer In FeelConfig.Connections.Keys
    '        If (FeelConfig.Connections(device).OutputName = Output) Then Return device
    '    Next
    '    Return -1
    'End Function

    'Public Sub SendMidi(ByVal Device As Integer, ByVal Message As String)
    '    ''Make sure we're not wasting our time sending something to a device that doesn't exist
    '    If (Device = -1) Or (Not FeelConfig.Connections.ContainsKey(Device)) Then
    '        Exit Sub
    '    Else
    '        ''Ensure the device is enabled to receive
    '        If (FeelConfig.Connections(Device).Enabled And FeelConfig.Connections(Device).OutputEnable) Then
    '            'TODO: Currently checking this twice, because the regex doesn't like being served an empty string.
    '            ' Need to do after because NullOrEmpty does not include spaces.
    '            If (String.IsNullOrEmpty(Message)) Then Exit Sub

    '            ''Remove whitespace from commands, and convert to uppercase
    '            Dim regex As New Text.RegularExpressions.Regex("\s")
    '            Message = regex.Replace(Message, String.Empty)
    '            Message = UCase(Message)

    '            ''Just a failsafe, don't need to do anything with empty States
    '            If (String.IsNullOrEmpty(Message)) Then Exit Sub

    '            ''Get first character to determine command type (See Select statement below)
    '            Dim cmdType As String = Message.Substring(0, 1)
    '            If (cmdType = "#") Then Exit Sub 'Comment

    '            ''Check to make sure we have all information needed
    '            If (Message.Length < 6) Then Exit Sub

    '            ''Convert to byte array
    '            Dim len As Integer = Message.Length
    '            Dim upperBound As Integer = len \ 2
    '            If ((len Mod 2) = 0) Then
    '                upperBound -= 1
    '            Else
    '                Message = "0" & Message
    '            End If
    '            Dim cmdArr(upperBound) As Byte
    '            For i As Integer = 0 To upperBound
    '                cmdArr(i) = Convert.ToByte(Message.Substring(i * 2, 2), 16)
    '            Next

    '            'TODO: Checks to make sure values (in Byte, 0-255) do not exceed valid MIDI values (half-Byte, 0-127)
    '            ' There's surely a better way to do this...
    '            If (Convert.ToByte(Message.Substring(1, 1), 16) > 15) Then Exit Sub
    '            If (cmdArr(1) > 127) Then Exit Sub
    '            If (cmdArr(2) > 127) Then Exit Sub

    '            ''Get the device's MIDI output name
    '            Dim _device As String = FeelConfig.Connections(Device).OutputName

    '            ''Take appropriate action
    '            Select Case cmdType
    '                ''Unsupported:
    '                'Case "A" 'Polyphonic Aftertouch
    '                'Case "D" 'Channel Pressure (Aftertouch)

    '                Case "8" 'MIDI Note Off
    '                    'midiOut.Item(device.OutputName).SendNoteOff(CType(cmd.Substring(1, 1), Midi.Channel), CType(Convert.ToInt16(cmd.Substring(2, 2)), Midi.Pitch), Convert.ToInt16(cmd.Substring(4, 2)))
    '                    midiOut.Item(_device).SendNoteOff(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), CType(cmdArr(1), Midi.Pitch), cmdArr(2))
    '                Case "9" 'MIDI Note On
    '                    midiOut.Item(_device).SendNoteOn(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), CType(cmdArr(1), Midi.Pitch), cmdArr(2))
    '                Case "B" 'Control Change
    '                    midiOut.Item(_device).SendControlChange(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), CType(cmdArr(1), Midi.Control), cmdArr(2))
    '                Case "C" 'Program Change
    '                    midiOut.Item(_device).SendProgramChange(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), CType(cmdArr(1), Midi.Instrument))
    '                Case "E" 'Pitch Wheel Change (Pitch Bend)
    '                    midiOut.Item(_device).SendPitchBend(CType(Convert.ToByte(Message.Substring(1, 1), 16), Midi.Channel), cmdArr(1))
    '                Case "F" 'MIDI System-Common Message
    '                    If (Message.Substring(1, 1) = "0") Then 'MIDI Sysex Message
    '                        midiOut.Item(_device).SendSysEx(cmdArr)
    '                    End If
    '            End Select
    '        End If
    '    End If
    'End Sub
    'Public Sub SendMidi(ByVal Device As Integer, ByVal Message As String())
    '    For Each _msg As String In Message
    '        SendMidi(Device, _msg)
    '    Next
    'End Sub
    'Public Sub SendMidi(ByVal Device As String, ByVal Message As String)
    '    If (Device = "ALL DEVICES") Then
    '        For Each dev As Integer In FeelConfig.Connections.Keys
    '            SendMidi(dev, Message)
    '        Next
    '    Else
    '        SendMidi(FindDevice(Device), Message)
    '    End If
    'End Sub
    'Public Sub SendMidi(ByVal Device As String, ByVal Message As String())
    '    For Each _msg As String In Message
    '        SendMidi(Device, _msg)
    '    Next
    'End Sub
    'Public Sub SendMidi(ByVal Device As String, ByVal ControlType As Byte, ByVal Channel As Byte, ByVal NotCon As Byte)
    '    Dim ContStr As String = If(ControlType = 144 Or ControlType = 128, "9", "B") & Channel.ToString & NotCon.ToString("X2")
    '    Dim _device As Integer = FindDevice(Device)
    '    SendMidi(Device, ContStr)
    'End Sub
    'Public Sub SendMidi(ByVal Device As Integer, ByVal ControlType As Byte, ByVal channel As Byte, ByVal notcon As Byte)
    '    Dim ContStr As String = If(ControlType = 144 Or ControlType = 128, "9", "B") & channel.ToString & notcon.ToString("X2")
    '    SendMidi(Device, ContStr)
    'End Sub

    'Public Sub SetPage(ByVal Device As Integer, ByVal Page As Byte)
    '    If (Device = -1) Or (Not FeelConfig.Connections.ContainsKey(Device)) Then
    '        Exit Sub
    '    Else
    '        FeelConfig.Connections(Device).PageCurrent = Page
    '        RedrawControls(Device)
    '    End If
    'End Sub
    'Public Sub SetPage(ByVal Device As String, ByVal Page As Byte)
    '    If (Device = "ALL DEVICES") Then
    '        For Each dev As clsConnection In FeelConfig.Connections.Values
    '            If (dev.Enabled = True) Then
    '                dev.PageCurrent = Page
    '            End If
    '        Next
    '    Else
    '        Dim _device As Integer = serviceHost.FindDeviceIndex(Device)
    '        If (FeelConfig.Connections.ContainsKey(_device)) Then
    '            FeelConfig.Connections(_device).PageCurrent = Page
    '        End If
    '    End If
    '    RedrawControls(Device)
    'End Sub

    'Public Sub RedrawControls(ByVal Device As Integer)
    '    If (Device = -1) Or (Not FeelConfig.Connections.ContainsKey(Device)) Then
    '        Exit Sub
    '    Else
    '        With FeelConfig.Connections(Device)
    '            If (.Enabled And .OutputEnable) Then
    '                For Each cont As clsControl In .Control.Values
    '                    If (cont.Page.ContainsKey(.PageCurrent)) Then
    '                        ''If this is the first time setting state, also set current state
    '                        '' otherwise use only CurrentState
    '                        If (cont.Page(.PageCurrent).CurrentState = "") Then
    '                            serviceHost.SendMIDI(Device, cont.Page(.PageCurrent).InitialState)
    '                            cont.Page(.PageCurrent).CurrentState = cont.Page(.PageCurrent).InitialState
    '                        Else
    '                            serviceHost.SendMIDI(Device, cont.Page(.PageCurrent).CurrentState)
    '                        End If
    '                    End If
    '                Next
    '            End If
    '        End With
    '    End If
    'End Sub
    'Public Function RedrawControls(Optional ByVal Device As String = "ALL DEVICES") As Boolean
    '    If (Device = "ALL DEVICES") Then
    '        For Each _device As Integer In FeelConfig.Connections.Keys
    '            RedrawControls(_device)
    '        Next
    '    Else
    '        RedrawControls(serviceHost.FindDeviceIndex(Device))
    '    End If
    'End Function
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
    Private Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByRef lParam As ActionInterface.CopyData) As IntPtr
    End Function

    ''Asynchronous PostMessage alternative
    '' Use insted of SendMessage() when you don't want to wait for the recipient program to finish processing the message
    <Runtime.InteropServices.DllImport("user32.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.StdCall, CharSet:=CharSet.Auto, EntryPoint:="PostMessageA", SetLastError:=True)> _
    Private Function PostMessage(ByVal hwnd As IntPtr, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32
    End Function

    ''These constants are defined in Feel.ActionInterface
    'Public Const WM_USER As Int32 = &H400 ''1024
    'Public Const WM_COPYDATA As Integer = &H4A ''74
    'Public Structure CopyData
    '    Public dwData As Integer
    '    Public cbData As Integer
    '    Public lpData As IntPtr
    'End Structure

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
        Return If(FeelConfig.WmEnable, CType(SendMessage(LJHandle, ActionInterface.WM_USER + uMsg, New IntPtr(wParam), New IntPtr(lParam)), Integer), -1)
    End Function

    Public Function PostMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        Return If(FeelConfig.WmEnable, CType(PostMessage(LJHandle, ActionInterface.WM_USER + uMsg, wParam, lParam), Integer), -1)
    End Function

    Public Function SendCopyData(ByVal lParam As ActionInterface.CopyData) As Integer
        'wParam is supposed to be a pointer to the handle of this process, or an HWND
        Return If(FeelConfig.WmEnable, CType(SendMessage(LJHandle, ActionInterface.WM_COPYDATA, New IntPtr(0), lParam), Integer), -1)
    End Function
#End Region

#Region "Secondary Functions"
    Public Function GetReadyState(ByVal handle As IntPtr) As IntPtr
        ''Find out if LightJockey is ready
        ''Returns 1 if LJ is ready to receive messages, else returns 0
        Return SendMessage(handle, ActionInterface.WM_USER + 1502, New IntPtr(0), New IntPtr(0))
    End Function

    Public Function GetVersion(ByVal handle As IntPtr) As IntPtr
        ''Returns an integer value. To get actual version number in "standard, human readable" form
        '' convert to hex, split by byte, convert to integer
        '' example: "39780608" -> "25F0100" -> "02", "5F", "01", "00" -> 2.95.1.0
        Return SendMessage(handle, ActionInterface.WM_USER + 1502, New IntPtr(1), New IntPtr(0))
    End Function
#End Region

#Region "Tertiary Functions"
    Public Function GetSequence() As Integer
        Return CType(SendMessage(LJHandle, ActionInterface.WM_USER + 1600, New IntPtr(256), New IntPtr(0)), Integer)
    End Function

    Public Function GetCue() As Integer
        Return CType(SendMessage(LJHandle, ActionInterface.WM_USER + 1600, New IntPtr(257), New IntPtr(0)), Integer)
    End Function

    Public Function GetCueList() As Integer
        Return CType(SendMessage(LJHandle, ActionInterface.WM_USER + 1600, New IntPtr(258), New IntPtr(0)), Integer)
    End Function

    Public Function GetBackgroundCue() As Integer
        Return CType(SendMessage(LJHandle, ActionInterface.WM_USER + 1600, New IntPtr(262), New IntPtr(0)), Integer)
    End Function
#End Region
#End Region

#Region "Action Plugins"
    'Private Sub LoadActions()
    '    Dim serviceHost As ActionInterface.IServices = New Interfaces.ServiceHost
    '    Dim actionsList As Interfaces = New Interfaces
    '    actionsList.LoadModules()
    '    For Each actn As ActionInterface.IAction In actionsList.m_addonInstances
    '        actn.Initialize(serviceHost)
    '        Diagnostics.Debug.WriteLine(vbCrLf)
    '        actn.Execute("device", CByte("127"), CByte("0"), CByte("0"), CByte("0"))
    '    Next
    'End Sub

    Friend Sub LoadModules()
        Dim currentApplicationDirectory As String = My.Application.Info.DirectoryPath
        Dim addonsRootDirectory As String = currentApplicationDirectory & "\actions\"
        Dim addonsLoaded As New Collections.Generic.List(Of System.Type)

        If My.Computer.FileSystem.DirectoryExists(addonsRootDirectory) Then
            Dim dllFilesFound As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(addonsRootDirectory, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*.dll")
            For Each dllFile As String In dllFilesFound
                Dim modulesFound As System.Collections.Generic.List(Of System.Type) = TryLoadAssemblyReference(dllFile)
                addonsLoaded.AddRange(modulesFound)
            Next
        End If

        If addonsLoaded.Count > 0 Then
            For Each addonToInstantiate As System.Type In addonsLoaded
                Dim thisInstance As ActionInterface.IAction = CType(Activator.CreateInstance(addonToInstantiate), ActionInterface.IAction)
                ''TODO: need this??
                'Dim thisTypedInstance As ActionInterface.IAction = CType(thisInstance, ActionInterface.IAction)
                'thisTypedInstance.Initialise()
                thisInstance.Initialize(serviceHost)
                actionModules.Add(thisInstance.UniqueID, thisInstance)
            Next
        End If
    End Sub

    Private Function TryLoadAssemblyReference(ByVal dllFilePath As String) As Collections.Generic.List(Of System.Type)
        Dim loadedAssembly As Reflection.Assembly = Nothing
        Dim listOfModules As New Collections.Generic.List(Of System.Type)
        Try
            loadedAssembly = Reflection.Assembly.LoadFile(dllFilePath)
        Catch ex As Exception
            Diagnostics.Debug.WriteLine("Reflection.Assembly exception")
        End Try
        If loadedAssembly IsNot Nothing Then
            For Each assemblyModule As System.Reflection.Module In loadedAssembly.GetModules
                For Each moduleType As System.Type In assemblyModule.GetTypes()
                    For Each interfaceImplemented As System.Type In moduleType.GetInterfaces()
                        'Diagnostics.Debug.WriteLine("moduletype.Name: " & moduleType.Name & " | interfaceImplemented.Name: " & interfaceImplemented.Name & " | interfaceImplemented.FullName: " & interfaceImplemented.FullName)
                        'If interfaceImplemented.FullName = "ActionInterface.IAction" Then
                        If (interfaceImplemented.Name = "IAction") Then
                            listOfModules.Add(moduleType)
                        End If
                    Next
                Next
            Next
        End If
        'Diagnostics.Debug.WriteLine("loadedmodules.count: " & listOfModules.Count.ToString)
        Return listOfModules
    End Function
#End Region

#Region "Random Code Helpers"
    ''' <summary>
    ''' Add an item (T) to end of array (of T).
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="arr"></param>
    ''' <param name="item"></param>
    ''' <remarks></remarks>
    <System.Runtime.CompilerServices.Extension()> _
    Public Sub Add(Of T)(ByRef arr As T(), ByVal item As T)
        Array.Resize(arr, arr.Length + 1)
        arr(arr.Length - 1) = item
    End Sub
#End Region
End Module