Imports System.Windows.Forms
Imports Midi
Imports System.Runtime.InteropServices

Module main
    ''' <summary>Used as a 'reference' to the UI thread for opening forms from an event</summary>
    Friend _threadcontext As System.Threading.SynchronizationContext

    Dim WithEvents trayIcon As New NotifyIcon
    Dim trayMenu As New ContextMenu
    Dim WithEvents menuExit As New MenuItem
    Dim WithEvents menuConfigProgram As New MenuItem
    Dim WithEvents menuConfigConnections As New MenuItem
    Dim WithEvents menuConfigEvents As New MenuItem
    Dim menuAdvancedTasks As New MenuItem
    Dim WithEvents menuRefreshWindowHandle As New MenuItem
    Dim WithEvents menuSaveConfiguration As New MenuItem
    Dim WithEvents menuUpdateAvailableDevices As New MenuItem
    Dim WithEvents menuUpdateAvailablePlugins As New MenuItem
    Dim WithEvents menuAbout As New MenuItem

    ''' <summary>Container for all program configuration</summary>
    ''' <remarks>The last opened configuration file is not included here (it is in Application.Settings).</remarks>
    Public FeelConfig As clsConfig

    ''' <summary>Collections of loaded Action plugins</summary>
    Friend actionModules As Collections.Generic.Dictionary(Of Guid, ActionInterface.IAction) = New Collections.Generic.Dictionary(Of Guid, ActionInterface.IAction)
    Public serviceHost As ActionInterface.IServices = New Interfaces.ServiceHost

    ''' <summary>Collections of MIDI input and output devices</summary>
    Public midiIn As System.Collections.Generic.Dictionary(Of String, InputDevice) = New Collections.Generic.Dictionary(Of String, InputDevice)
    Public midiOut As System.Collections.Generic.Dictionary(Of String, OutputDevice) = New Collections.Generic.Dictionary(Of String, OutputDevice)

    Friend configForm As frmConfiguration
    Friend connectForm As frmConnections
    Friend eventForm As frmEvents
    Friend aboutForm As frmAbout

    ''' <summary>ConfigMode overrides normal operation when user is modifying program configuration</summary>
    Private _configMode As Boolean = False
    Public Property configMode(Optional ByVal reConnect As Boolean = False) As Boolean
        <Diagnostics.DebuggerStepThrough()> _
        Get
            Return _configMode
        End Get
        <Diagnostics.DebuggerStepThrough()> _
        Set(ByVal value As Boolean)
            If Not (configForm Is Nothing) Then
                If (configForm.Visible) Then
                    _configMode = True
                End If
            ElseIf Not (connectForm Is Nothing) Then
                If (connectForm.Visible) Then
                    _configMode = True
                End If
            ElseIf Not (eventForm Is Nothing) Then
                If (eventForm.Visible) Then
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
            MenuConfigModeSwitch()
        End Set
    End Property

    Private _connecting As Boolean = False
    ''' <summary>A Boolean representing whether or not connections are actively being setup or destroyed.</summary>
    ''' <returns>True while connections are being established, otherwise False</returns>
    ''' <remarks>This is used for a user-configuratble setting to ignore any events raised by connections while being connected to (for example, any "handshake" type messages)</remarks>
    Public Property Connecting() As Boolean
        <Diagnostics.DebuggerStepThrough()> _
        Get
            Return (_connecting And FeelConfig.IgnoreEvents)
        End Get
        <Diagnostics.DebuggerStepThrough()> _
        Set(ByVal value As Boolean)
            _connecting = value
        End Set
    End Property

    Public Sub Main()
        ''See: <project directory>\feel_commandlineargs.txt
        'For Each arg As String In Environment.GetCommandLineArgs()
        For Each arg As String In My.Application.CommandLineArgs
            'TODO:
            System.Diagnostics.Debug.WriteLine("Command Line Arg: " & arg)
        Next

        ''Aparently this is required to show Groups in ListView controls
        System.Windows.Forms.Application.EnableVisualStyles()

        ''System Tray menu
        menuExit.Text = "E&xit"
        menuConfigProgram.Text = "&Program Configuration"
        menuConfigConnections.Text = "Configure &Connections"
        menuConfigEvents.Text = "Configure &Events"
        menuAdvancedTasks.Text = "Advanced &Tasks"
        menuSaveConfiguration.Text = "&Save Configuration"
        menuRefreshWindowHandle.Text = "&Refresh LJ Handle"
        menuUpdateAvailableDevices.Text = "Update Available &Devices"
        menuUpdateAvailablePlugins.Text = "Update Available &Plugins"
        menuAbout.Text = "About"
        trayMenu.MenuItems.Add(menuExit)
        trayMenu.MenuItems.Add("-")
        trayMenu.MenuItems.Add(menuAbout)
        trayMenu.MenuItems.Add("-")
        menuAdvancedTasks.MenuItems.Add(menuSaveConfiguration)
        menuAdvancedTasks.MenuItems.Add(menuRefreshWindowHandle)
        menuAdvancedTasks.MenuItems.Add(menuUpdateAvailableDevices)
        menuAdvancedTasks.MenuItems.Add(menuUpdateAvailablePlugins)
        trayMenu.MenuItems.Add(menuAdvancedTasks)
        'trayMenu.MenuItems.Add("-")
        trayMenu.MenuItems.Add(menuConfigProgram)
        trayMenu.MenuItems.Add(menuConfigConnections)
        trayMenu.MenuItems.Add(menuConfigEvents)
        trayIcon.ContextMenu = trayMenu
        'trayIcon.Icon = New System.Drawing.Icon(System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceStream("Feel.feel.ico"))
        trayIcon.Icon = My.Resources.Feel.feel
        trayIcon.Visible = True

        LoadModules()

        LoadConfiguration()
        If Not _configMode Then ConnectDevices()

        ''Store a reference to the UI thread (for opening forms from events)
        '' Source: http://www.codeproject.com/Articles/31971/Understanding-SynchronizationContext-Part-I
        Using _dummyControl As New System.Windows.Forms.Control
            ''This Is Nothing if we don't initialize at least one object (or form) in the declarations above
            '' or, in this case, here in the Using
            _threadcontext = System.Threading.SynchronizationContext.Current
            '_threadcontext = System.Windows.Forms.WindowsFormsSynchronizationContext.Current

            ''The following do not work to set context without initializing a control, but are kept here for reference
            'System.Threading.SynchronizationContext.SetSynchronizationContext(_threadcontext)
            'System.Windows.Forms.WindowsFormsSynchronizationContext.SetSynchronizationContext(_threadcontext)
        End Using
        'If _threadcontext Is Nothing Then Diagnostics.Debug.WriteLine("Thread Context is Nothing!")

        Application.Run()
    End Sub

#Region "Windows & Menus"
    <Diagnostics.DebuggerStepThrough()> _
    Private Sub menuExit_Click() Handles menuExit.Click
        If _configMode Then
            Dim quitNoSave As System.Windows.Forms.DialogResult = MessageBox.Show("One or more configuration windows are open; any changes made have not been saved. Do you want to exit without saving?", "Feel: Configuration Not Saved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If (quitNoSave = DialogResult.No) Then Exit Sub
        End If

        DisconnectDevices()

        trayIcon.Visible = False
        Application.Exit()
    End Sub
    <Diagnostics.DebuggerStepThrough()> _
    Friend Sub ExitProgram(ByVal state As Object)
        'TODO: handle Restart = True
        If Not CType(state, Boolean) Then
            menuExit_Click()
        Else
            MessageBox.Show("Restarting not yet implemented!")
        End If
    End Sub

    <Diagnostics.DebuggerStepThrough()>
    Private Sub OpenProgramWindow() Handles menuConfigProgram.Click
        If Not _configMode Then
            ''Singleton: See designer code.
            '' http://www.codeproject.com/Articles/5000/Simple-Singleton-Forms
            '' (dated link) http://www.codeproject.com/KB/vb/Simple_Singleton_Forms.aspx
            If configForm Is Nothing Then
                configForm = New frmConfiguration
            End If
            configForm.Show()
            'configForm.Focus()
        Else
            HighlightActiveConfig()
        End If
    End Sub
    Friend Sub OpenConfigWindow_Ext(ByVal state As Object)
        OpenProgramWindow()
    End Sub

    <Diagnostics.DebuggerStepThrough()>
    Private Sub OpenConnectWindow() Handles menuConfigConnections.Click
        If Not _configMode Then
            ''The Connections Configuration form makes its own connections to
            '' test devices, so we must disconnect existing connections first.
            DisconnectDevices()

            If connectForm Is Nothing Then
                connectForm = New frmConnections
            End If
            connectForm.Show()
        Else
            HighlightActiveConfig()
        End If
    End Sub
    <Diagnostics.DebuggerStepThrough()> _
    Friend Sub OpenConnectWindow_Ext(ByVal state As Object)
        OpenConnectWindow()
    End Sub

    <Diagnostics.DebuggerStepThrough()>
    Private Sub OpenEventWindow() Handles menuConfigEvents.Click, trayIcon.DoubleClick
        If Not _configMode Then
            If eventForm Is Nothing Then
                eventForm = New frmEvents
            End If
            eventForm.Show()
        Else
            HighlightActiveConfig()
        End If
    End Sub
    <Diagnostics.DebuggerStepThrough()>
    Friend Sub OpenEventWindow_Ext(ByVal state As Object)
        OpenEventWindow()
    End Sub

    <Diagnostics.DebuggerStepThrough()> _
    Private Sub OpenAbout() Handles menuAbout.Click
        If aboutForm Is Nothing Then
            aboutForm = New frmAbout
        End If
        aboutForm.Show()
    End Sub

    ''' <summary>Sets the _ljHandle variable to a blank pointer.</summary>
    ''' <remarks>'Resetting' _ljHandle triggers a new search for LightJockey's window handle
    '''  upon the next use. Useful, for instance, if LightJockey was restarted.</remarks>
    <Diagnostics.DebuggerStepThrough()> _
    Friend Sub RefreshHandle() Handles menuRefreshWindowHandle.Click
        _ljHandle = IntPtr.Zero
    End Sub

    ''See: #Region "File I/O", SaveConfiguration()
    'Private Sub SaveConfiguration() Handles menuSaveConfiguration.Click

    <Diagnostics.DebuggerStepThrough()>
    Friend Sub UpdateAvailableDevices() Handles menuUpdateAvailableDevices.Click
        Midi.InputDevice.UpdateInstalledDevices()
        Midi.OutputDevice.UpdateInstalledDevices()
        'TODO: update lists in Connection Config, if in ConfigMode
    End Sub

    ''' <summary>Enables/Disables menu items based on <see cref="configMode(Boolean)">configMode(Boolean)</see>.</summary>
    Private Sub MenuConfigModeSwitch()
        menuConfigConnections.Enabled = Not _configMode
        menuConfigEvents.Enabled = Not _configMode
        menuConfigProgram.Enabled = Not _configMode
    End Sub

    ''' <summary>Brings open config window into Focus if there is an attempt to open another config window.</summary>
    Private Sub HighlightActiveConfig()
        If (_configMode) Then
            Select Case True
                Case eventForm IsNot Nothing
                    eventForm.Focus()
                Case connectForm IsNot Nothing
                    connectForm.Focus()
                Case configForm IsNot Nothing
                    configForm.Focus()
            End Select
        End If
    End Sub
#End Region

#Region "File I/O"
    ''' <summary>Attempts to read serialzed configuration data from disk.</summary>
    ''' <remarks>Creates a blank configuration upon failure.</remarks>
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

    ''' <summary>Completely clears the current configuration object.</summary>
    ''' <remarks>This affects only the configuration loaded into memory. Any saved configurations remain.</remarks>
    <Diagnostics.DebuggerStepThrough()> _
    Friend Sub CreateNewConfiguration()
        FeelConfig = New clsConfig
    End Sub

    <Diagnostics.DebuggerStepThrough()> _
    Private Function CreateUserDirectory() As Boolean
        Try
            Dim dInfo As IO.DirectoryInfo = IO.Directory.CreateDirectory(Application.StartupPath & "\user")
            Return dInfo.Exists
        Catch ex As Exception
            MessageBox.Show("Error creating 'user' directory!", "Feel: I/O Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    <Diagnostics.DebuggerStepThrough()> _
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

    ''' <summary>Serializes Action plug-in .Data Objects independently to prepare Configuration for storage.</summary>
    <Diagnostics.DebuggerStepThrough()> _
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

    ''' <summary>Deserializes Action plug-in .Data Objects and returns a usable Configuration.</summary>
    ''' <remarks>Must be run after plug-ins are loaded. ConfigDeserialzie searches availability of plug-ins before deserialziing the .Data object for each plug-in; unavailable plug-ins' .Data are ignored.</remarks>
    <Diagnostics.DebuggerStepThrough()> _
    Private Function ConfigDeserialize(ByRef Configuration As clsConfig) As clsConfig
        For Each conx As clsConnection In Configuration.Connections.Values
            For Each cont As clsControl In conx.Control.Values
                For Each page As clsControlPage In cont.Page.Values
                    For Each act As clsAction In page.Actions
                        ''TODO (Delete):
                        'If (actionModules.ContainsKey(act.Type)) Then
                        '    act._available = True
                        'Else
                        '    act._available = False
                        'End If
                        act._available = CheckPluginAvailability(act.Type)
                    Next
                    For Each act As clsAction In page.ActionsOff
                        ''TODO (Delete):
                        'If (actionModules.ContainsKey(act.Type)) Then
                        '    act._available = True
                        'Else
                        '    act._available = False
                        'End If
                        act._available = CheckPluginAvailability(act.Type)
                    Next
                Next
            Next
        Next
        Return Configuration
    End Function

    ''' <summary>Checks the (current) availability of a plugin based on its GUID.</summary>
    ''' <param name="PluginGuid">The GUID to find</param>
    ''' <returns>True if plugin currently loaded and ready for use</returns>
    <Diagnostics.DebuggerStepThrough()> _
    Public Function CheckPluginAvailability(ByRef PluginGuid As Guid) As Boolean
        Return actionModules.ContainsKey(PluginGuid)
    End Function
#End Region

#Region "Connections"
    ''' <summary>Establishes MIDI connections to all enabled devices</summary>
    ''' <remarks>Ver:2.1</remarks>
    <Diagnostics.DebuggerStepThrough()> _
    Private Sub ConnectDevices()
        Connecting = True

        For Each device As clsConnection In FeelConfig.Connections.Values
            If (device.Enabled) Then
                ''Check the stored connection index to make sure it is not greater than the number of currently connected devices
                If (device.Input + 1 > InputDevice.InstalledDevices.Count) OrElse (device.Output + 1 > OutputDevice.InstalledDevices.Count) Then
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
                End If

                ''Check that the stored connection names match the system's current names for the stored index
                If Not If(device.InputEnable, device.InputName = InputDevice.InstalledDevices.Item(device.Input).Name, True) OrElse Not If(device.OutputEnable, device.OutputName = OutputDevice.InstalledDevices.Item(device.Output).Name, True) Then
                    device.Enabled = False
                    System.Windows.Forms.MessageBox.Show("Warning: One or both of the names of the input/output ports for the device named " & device.Name & " do not match the name of the system's port at the same address! Usually this is an indication that additional devices have been added or removed from the system recently, or powered on in a different order." & vbCrLf & vbCrLf & "The device has been disabled. Please update this device's connection info by opening the 'Configure Connections' window and adjusting this device's Input and Output settings, then re-enable the device.", "Feel: MIDI Configuration Has Changed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Continue For
                End If

                ''Determine if connections are already in use
                If If(device.InputEnable, InputDevice.InstalledDevices.Item(device.Input).IsOpen, False) OrElse If(device.OutputEnable, OutputDevice.InstalledDevices.Item(device.Output).IsOpen, False) Then
                    device.Enabled = False
                    System.Windows.Forms.MessageBox.Show("Warning: One or both of the input/output ports for the device named " & device.Name & " is already in use! Either the device is already in use elsewhere, or the connection information is incorrectly set." & vbCrLf & vbCrLf & "The device has been disabled. If you receive this error and you believe the device is connected properly, update this device's connection info by opening the 'Configure Connections' window and adjusting this device's Input and Output settings, then re-enable the device.", "Feel: MIDI Port In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Continue For
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

    ''' <summary>Closes and clears all connections, and removes all event handlers</summary>
    ''' <param name="which">(Byte) 1: Close incoming, 2: Close outgoing, 0: Close both</param>
    ''' <remarks>Ver: 1.0
    ''' Called when closing application, or opening Connection Configuration window</remarks>
    <Diagnostics.DebuggerStepThrough()> _
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

#Region "MIDI Event Handlers"
    <Diagnostics.DebuggerStepThrough()> _
    Private Sub NoteOn(ByVal msg As NoteOnMessage)
        If configMode AndAlso Not (eventForm Is Nothing) Then
            ''Override programmed action, redirect action to Configure Actions window
            If (eventForm.Visible) Then
                Dim newCont As frmEvents.curControl = New frmEvents.curControl
                newCont.Device = msg.Device.Name.ToString
                newCont.Channel = CByte(msg.Channel)
                newCont.Type = "Note"
                newCont.NotCon = CByte(msg.Pitch)
                newCont.VelVal = CByte(msg.Velocity)
                eventForm.CurrentControl = newCont
                newCont = Nothing
            End If
        ElseIf Not configMode AndAlso Not Connecting Then
            Dim device As Integer = serviceHost.FindDeviceIndex(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (FeelConfig.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "9" & CByte(msg.Channel).ToString("X") & CByte(msg.Pitch).ToString("X2")
                If (FeelConfig.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(FeelConfig.Connections(device).Control(ContStr).Paged, FeelConfig.Connections(device).PageCurrent, CByte(0))
                    If (FeelConfig.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        ''End Checks
                        With FeelConfig.Connections(device).Control(ContStr).Page(_page)
                            If (FeelConfig.Connections(device).NoteOff) AndAlso (msg.Velocity = 0) Then
                                ''This is actually a "NoteOff", or is to be treated as one according to configuration
                                If ((.Behavior = 1) AndAlso Not (.IsActive)) OrElse (.Behavior = 0) Then
                                    ''This is a 'momentary' button (AND "IsActive" so is waiting to be turned off)
                                    '' OR
                                    ''This is a 'latch' button
                                    For Each actn As clsAction In .ActionsOff
                                        If (actn.Enabled) AndAlso (actn._available) Then
                                            'TODO: actn.Data should never be Nothing, if ._available is set to True
                                            If Not (actn.Data Is Nothing) Then
                                                With actionModules.Item(actn.Type)
                                                    .Data = actn.Data
                                                    If Not (.Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                        'TODO: Something here in release version
                                                        Diagnostics.Debug.WriteLine("ACTION EXECUTE FAILED!")
                                                    End If
                                                End With
                                            End If
                                        End If
                                    Next
                                    If (.Behavior = 0) Then .IsActive = False
                                End If
                            Else
                                If ((.Behavior = 1) AndAlso Not (.IsActive)) OrElse (.Behavior = 0) Then
                                    For Each actn As clsAction In .Actions
                                        If (actn.Enabled) AndAlso (actn._available) Then
                                            If Not (actn.Data Is Nothing) Then
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

    <Diagnostics.DebuggerStepThrough()> _
    Private Sub NoteOff(ByVal msg As NoteOffMessage)
        ''No need to send this to actionForm in configMode, because we'd probably end up associating actions to button up actions by mistake.
        If Not configMode AndAlso Not Connecting Then
            Dim device As Integer = serviceHost.FindDeviceIndex(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (FeelConfig.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "9" & CByte(msg.Channel).ToString("X") & CByte(msg.Pitch).ToString("X2")
                If (FeelConfig.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(FeelConfig.Connections(device).Control(ContStr).Paged, FeelConfig.Connections(device).PageCurrent, CByte(0))
                    If (FeelConfig.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        ''End Checks
                        With FeelConfig.Connections(device).Control(ContStr).Page(_page)
                            If ((.Behavior = 1) AndAlso (Not .IsActive)) OrElse (.Behavior = 0) Then
                                ''This is a 'momentary' button (AND "IsActive" so is waiting to be turned off)
                                '' OR
                                ''This is a 'latch' button
                                For Each actn As clsAction In .ActionsOff
                                    If (actn.Enabled) AndAlso (actn._available) Then
                                        If Not (actn.Data Is Nothing) Then
                                            With actionModules.Item(actn.Type)
                                                .Data = actn.Data
                                                If Not (.Execute(msg.Device.Name, 128, CType(msg.Channel, Byte), CType(msg.Pitch, Byte), CType(msg.Velocity, Byte))) Then
                                                    'TODO: Something here in release version
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

    <Diagnostics.DebuggerStepThrough()> _
    Private Sub ControlChange(ByVal msg As ControlChangeMessage)
        If configMode AndAlso Not (eventForm Is Nothing) Then
            ''override programmed action, redirect action to Configure Actions window
            If (eventForm.Visible) Then
                Dim newCont As frmEvents.curControl = New frmEvents.curControl
                newCont.Device = msg.Device.Name.ToString
                newCont.Channel = CByte(msg.Channel)
                newCont.Type = "Control"
                newCont.NotCon = CByte(msg.Control)
                newCont.VelVal = CByte(msg.Value)
                eventForm.CurrentControl = newCont
                newCont = Nothing
            End If
        ElseIf Not configMode AndAlso Not Connecting Then
            Dim device As Integer = serviceHost.FindDeviceIndex(msg.Device.Name)
            'TODO: Previous line makes next line unneccessary:
            If (FeelConfig.Connections.ContainsKey(device)) Then
                Dim ContStr As String = "B" & CByte(msg.Channel).ToString("X") & CByte(msg.Control).ToString("X2")
                If (FeelConfig.Connections(device).Control.ContainsKey(ContStr)) Then
                    Dim _page As Byte = If(FeelConfig.Connections(device).Control(ContStr).Paged, FeelConfig.Connections(device).PageCurrent, CByte(0))
                    If (FeelConfig.Connections(device).Control(ContStr).Page.ContainsKey(_page)) Then
                        For Each actn As clsAction In FeelConfig.Connections(device).Control(ContStr).Page(_page).Actions
                            If (actn.Enabled) AndAlso (actn._available) Then
                                If Not (actn.Data Is Nothing) Then
                                    With actionModules.Item(actn.Type)
                                        .Data = actn.Data
                                        If Not (.Execute(msg.Device.Name, 176, CType(msg.Channel, Byte), CType(msg.Control, Byte), CType(msg.Value, Byte))) Then
                                            'TODO: Something here in release version
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
        <Diagnostics.DebuggerStepThrough()>
        Get
            If (_ljHandle = IntPtr.Zero) Or (_ljHandle = Nothing) Then
                _ljHandle = GetHandle()
            End If
            Return _ljHandle
        End Get
    End Property
#End Region

#Region "Primary Functions"
    <Diagnostics.DebuggerStepThrough()> _
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
    <Diagnostics.DebuggerStepThrough()> _
    Public Function SendMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        Return If(FeelConfig.WmEnable, CType(SendMessage(LJHandle, ActionInterface.WM_USER + uMsg, New IntPtr(wParam), New IntPtr(lParam)), Integer), -1)
    End Function

    '<Diagnostics.DebuggerStepThrough()> _
    Public Function PostMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        Return If(FeelConfig.WmEnable, CType(PostMessage(LJHandle, ActionInterface.WM_USER + uMsg, wParam, lParam), Integer), -1)
    End Function

    <Diagnostics.DebuggerStepThrough()> _
    Public Function SendCopyData(ByVal lParam As ActionInterface.CopyData) As Integer
        'wParam is supposed to be a pointer to the handle of this process, or an HWND
        Return If(FeelConfig.WmEnable, CType(SendMessage(LJHandle, ActionInterface.WM_COPYDATA, New IntPtr(0), lParam), Integer), -1)
    End Function
#End Region

#Region "Secondary Functions"
    <Diagnostics.DebuggerStepThrough()> _
    Public Function GetReadyState(ByVal handle As IntPtr) As IntPtr
        ''Find out if LightJockey is ready
        ''Returns 1 if LJ is ready to receive messages, else returns 0
        Return SendMessage(handle, ActionInterface.WM_USER + 1502, New IntPtr(0), New IntPtr(0))
    End Function

    <Diagnostics.DebuggerStepThrough()> _
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
    ''' <summary>Populates list of currently available action plug-ins.</summary>
    ''' <remarks>Attempts to load each instance of ActionInterface.IAction; upon success stores refernces by GUID in <see cref="actionModules">actionModules</see>.</remarks>
    <Diagnostics.DebuggerStepThrough()> _
    Friend Sub LoadModules()
        Dim addonsRootDirectory As String = My.Application.Info.DirectoryPath & "\actions\"
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
                'TODO: Clean this up some..
                If (actionModules.ContainsKey(thisInstance.UniqueID)) Then
                    System.Windows.Forms.MessageBox.Show("The plugin '" & thisInstance.Name & "' has an ID which matches another plugin already loaded. Loading of this plugin was skipped, and it will not be available for use.", "Feel: Duplicate Action Plugin ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    If (thisInstance.Initialize(serviceHost)) Then actionModules.Add(thisInstance.UniqueID, thisInstance)
                End If
            Next
        End If
    End Sub

    ''' <summary>Searches through a DLL to find instances of classes which implement IAction.</summary>
    ''' <param name="dllFilePath">Fully qualified path to DLL to search</param>
    ''' <returns>List of all IAction classes in DLL</returns>
    ''' <remarks>For use only within <see cref="LoadModules">LoadModules</see>.</remarks>
    <Diagnostics.DebuggerStepThrough()> _
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

    ''' <summary>Updates the list of available plugins.</summary>
    <Diagnostics.DebuggerStepThrough()> _
    Private Sub ReloadModules() Handles menuUpdateAvailablePlugins.Click
        actionModules.Clear()
        LoadModules()
    End Sub
#End Region
End Module