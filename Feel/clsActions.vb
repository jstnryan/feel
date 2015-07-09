﻿Imports System.ComponentModel

Interface iAction
    ReadOnly Property Name() As String
    ReadOnly Property Description() As String
    Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean 'Type: 128=NoteOff, 144=NoteOn, 176=Control Change
End Interface

#Region "Internal Actions"
'TODO: Figure this cross threading problem out.
<Serializable()> _
Public Class clsActionIntConfigActions
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Configure Actions"
    <NonSerialized()> _
    Public Const _Description As String = "Opens the 'Configure Actions' window." & vbCrLf & vbCrLf & "***NOT CURRENTLY STABLE****"

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        main.OpenActionWindow()
        Return True
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property
End Class

<Serializable()> _
Public Class clsActionIntPage
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Go to Page"
    <NonSerialized()> _
    Public Const _Description As String = "Triggers a page change on one, or all, control surface(s) to a specific page." & vbCrLf & vbCrLf & "• To change multiple controllers to different pages, use multiple actions." & vbCrLf & vbCrLf & "• To change page by an increment, use 'Skip Pages' instead." & vbCrLf & vbCrLf & "It is HIGHLY RECOMMENDED to disable the 'Paged Control' option when defining page change actions."

    Private _device As String
    Private _page As Byte

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        SetPage(_device, _page)
        Return True
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <TypeConverter(GetType(DeviceList)), _
        DisplayName("Target Device"), _
        Description("Which device(s) to change."), _
        DefaultValue("ALL DEVICES")> _
    Public Property Device() As String
        Get
            Return _device
        End Get
        Set(ByVal value As String)
            _device = value
        End Set
    End Property

    <DisplayName("Page Number"), _
        Description("The page to change to."), _
        DefaultValue(0)> _
    Public Property Page() As Byte
        Get
            Return _page
        End Get
        Set(ByVal value As Byte)
            If (value <= 255 And value >= 1) Then
                _page = value
            Else
                _page = 0
            End If
        End Set
    End Property

    Sub New()
        _device = "ALL DEVICES"
        _page = 0
    End Sub
End Class

'TODO: update with "Device" field?
<Serializable()> _
Public Class clsActionIntChangeControlState
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Change Control State"
    <NonSerialized()> _
    Public Const _Description As String = "Sends MIDI to a device, with the intention of changing the status of a control, for example button illumination, and preserves this state until modified." & vbCrLf & vbCrLf & "• This action is identical to the 'Send MIDI' action, with the exception that 'Change Control State' preserves the control's state for repeated use, for example when changing pages."

    Private _state As String

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim _device As Integer = main.FindDeviceByInput(Device)
        If Not (Configuration.Connections.ContainsKey(_device)) Then
            Return False
        Else
            Dim ContStr As String = If(Type = 144 Or Type = 128, "9", "B") & Channel.ToString & NoteCon.ToString("X2")
            If Not (Configuration.Connections(_device).Control.ContainsKey(ContStr)) Then
                Return False
            Else
                'TODO: Usint ".PageCurrent" is a hacky shortcut, and could lead to trouble down the line
                Configuration.Connections(_device).Control(ContStr).Page(Configuration.Connections(_device).PageCurrent).CurrentState = _state
                main.SendMidi(_device, _state)
                ''OR:
                'main.SendMidi(Device, Configuration.Connections(Device).Control(ContStr).Page(Configuration.Connections(Device).PageCurrent).CurrentState)
                Return True
            End If
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("State Change Command"), _
        Description("A string representation of the MIDI signal to send this device in order to change this control's illumination state."), _
        DefaultValue("")> _
    Public Property State() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class clsActionIntToggleControlState
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Toggle Control State"
    <NonSerialized()> _
    Public Const _Description As String = "Sends MIDI to a device, with the intention of changing the status of a control, for example button illumination, looping through a defined list of states." & vbCrLf & vbCrLf & "• This action is identical to the 'Change' action, with the exception that it accepts a list of states, and advances to the next state in the list based on the control's current state."

    <Browsable(False)> _
    Private _states As String()

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim _device As Integer = main.FindDeviceByInput(Device)
        If Not (Configuration.Connections.ContainsKey(_device)) Then
            Return False
        Else
            Dim ContStr As String = If(Type = 144 Or Type = 128, "9", "B") & Channel.ToString & NoteCon.ToString("X2")
            If Not (Configuration.Connections(_device).Control.ContainsKey(ContStr)) Then
                Return False
            Else
                'TODO: Usint ".PageCurrent" is a hacky shortcut, and could lead to trouble down the line
                Dim newState As String = _states(Array.IndexOf(_states, Configuration.Connections(_device).Control(ContStr).Page(Configuration.Connections(_device).PageCurrent).CurrentState) + 1)
                Configuration.Connections(_device).Control(ContStr).Page(Configuration.Connections(_device).PageCurrent).CurrentState = newState
                main.SendMidi(_device, newState)
                ''OR:
                'UpdateControlState(Device, Configuration.Connections(Device).Control(ContStr).Page(Configuration.Connections(Device).PageCurrent).CurrentState)
                Return True
            End If
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("List of State Change Commands"), _
        Description("String representations of the MIDI signal to send this device in order to change this control's illumination state. Place each command on its own line."), _
        DefaultValue("")> _
    Public Property State() As String()
        Get
            Return _states
        End Get
        Set(ByVal value As String())
            _states = value
        End Set
    End Property
End Class

''DEPRECIATED by clsActionIntGroupResetControl
'<Serializable()> _
'Public Class clsActionIntGroupResetState
'    Implements iAction

'    <NonSerialized()> _
'    Public Const _Name As String = "Reset Group State"
'    <NonSerialized()> _
'    Public Const _Description As String = "Sets all controls in group to 'Initial State.'"

'    Private _group As Byte
'    Private _device As String

'    <Browsable(False)> _
'    Public ReadOnly Property Description() As String Implements iAction.Description
'        Get
'            Return _Description
'        End Get
'    End Property

'    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
'        'TODO:
'        'If (_device = "ALL DEVICES") Then
'        'Else
'        'End If


'        For Each dev As Integer In Configuration.Connections.Keys
'            For Each cont As String In Configuration.Connections(dev).Control.Keys
'                For Each pag As Byte In Configuration.Connections(dev).Control(cont).Page.Keys
'                    If (Configuration.Connections(dev).Control(cont).Page(pag).ControlGroup = _group) Then
'                        Configuration.Connections(dev).Control(cont).Page(pag).CurrentState = Configuration.Connections(dev).Control(cont).Page(pag).InitialState
'                    End If
'                Next
'            Next
'        Next
'        RedrawControls(_device)
'        Return True
'    End Function

'    <Browsable(False)> _
'    Public ReadOnly Property Name() As String Implements iAction.Name
'        Get
'            Return _Name
'        End Get
'    End Property

'    <DisplayName("Group"), _
'        Description("Which group to reset to 'Initial State.'"), _
'        DefaultValue(0)> _
'    Public Property Group() As Byte
'        Get
'            Return _group
'        End Get
'        Set(ByVal value As Byte)
'            _group = value
'        End Set
'    End Property

'    <TypeConverter(GetType(DeviceList)), _
'        DisplayName("Target Device"), _
'        Description("Which device(s) to change."), _
'        DefaultValue("ALL DEVICES")> _
'    Public Property Device() As String
'        Get
'            Return _device
'        End Get
'        Set(ByVal value As String)
'            _device = value
'        End Set
'    End Property

'    Friend Sub New()
'        _group = 0
'        _device = "ALL DEVICES"
'    End Sub
'End Class
<Serializable()> _
Public Class clsActionIntGroupResetControl
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Reset Controls by Group"
    <NonSerialized()> _
    Public Const _Description As String = "Sets all controls in group to 'inactive' (momentary/latch button status), and resets control state."

    Private _group As Byte
    Private _redraw As Boolean

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        For Each dev As Integer In Configuration.Connections.Keys
            For Each cont As String In Configuration.Connections(dev).Control.Keys
                For Each pag As Byte In Configuration.Connections(dev).Control(cont).Page.Keys
                    If (Configuration.Connections(dev).Control(cont).Page(pag).ControlGroup = _group) Then
                        Configuration.Connections(dev).Control(cont).Page(pag).IsActive = False
                        Configuration.Connections(dev).Control(cont).Page(pag).CurrentState = Configuration.Connections(dev).Control(cont).Page(pag).InitialState
                    End If
                Next
            Next
        Next
        If (_redraw) Then RedrawControls()
        Return True
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("Group"), _
        Description("Which group to reset to 'inactive.'"), _
        DefaultValue(0)> _
    Public Property Group() As Byte
        Get
            Return _group
        End Get
        Set(ByVal value As Byte)
            _group = value
        End Set
    End Property

    <DisplayName("Redraw"), _
        Description("True to redraw control states after reset."), _
        DefaultValue(True)> _
    Public Property Redraw() As Boolean
        Get
            Return _redraw
        End Get
        Set(ByVal value As Boolean)
            _redraw = value
        End Set
    End Property

    Friend Sub New()
        _group = 0
        _redraw = True
    End Sub
End Class
#End Region

#Region "Windows Messages"
<Serializable()> _
Public Class clsActionSendMessage
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Send Windows Message"
    <NonSerialized()> _
    Public Const _Description As String = "Sends an arbitrary Windows Message to LightJockey." & vbCrLf & vbCrLf & "This differs from the 'Post Windows Message' function in that it waits for a response from LightJockey before continuing. The advantage being that you can ensure the message was received, but at the expense of execution speed. Post Windows Message is preferred in most circumstances."

    Private _msg As Integer
    Private _lParam As Integer
    Private _wParam As Integer

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    '<Browsable(False)> _
    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        SendMessage(_msg, _lParam, _wParam)
        Return True
    End Function

    '<CategoryAttribute("Data"), _
    <DisplayName("Message"), _
        Description("The value of the Windows Message to send (WM_USER + X)."), _
        DefaultValue(0)> _
    Public Property Message() As Integer
        Get
            Return _msg
        End Get
        Set(ByVal value As Integer)
            _msg = value
        End Set
    End Property

    <DisplayName("lParam"), _
        Description("The value of the 'lParameter' in the message."), _
        DefaultValue(0)> _
    Public Property lParam() As Integer
        Get
            Return _lParam
        End Get
        Set(ByVal value As Integer)
            _lParam = value
        End Set
    End Property

    <DisplayName("Message"), _
        Description("The value of the 'wParameter' in the message."), _
        DefaultValue(0)> _
    Public Property wParam() As Integer
        Get
            Return _wParam
        End Get
        Set(ByVal value As Integer)
            _wParam = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class clsActionPostMessage
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Send Windows Message"
    <NonSerialized()> _
    Public Const _Description As String = "Sends an arbitrary Windows Message to LightJockey." & vbCrLf & vbCrLf & "This differs from the 'Send Windows Message' function in that it doesn't wait for a response from LightJockey after sending the message. The advantage being speed over a guarantee that the message was received."

    Private _msg As Integer
    Private _lParam As Integer
    Private _wParam As Integer

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        'TODO: I really don't like that I have to instantiate an instance to access functions...
        PostMessage(_msg, _lParam, _wParam)
        Return True
    End Function

    '<CategoryAttribute("Data"), _
    <DisplayName("Message"), _
        Description("The value of the Windows Message to send (WM_USER + X)."), _
        DefaultValue(0)> _
    Public Property Message() As Integer
        Get
            Return _msg
        End Get
        Set(ByVal value As Integer)
            _msg = value
        End Set
    End Property

    <DisplayName("lParam"), _
        Description("The value of the 'lParameter' in the message."), _
        DefaultValue(0)> _
    Public Property lParam() As Integer
        Get
            Return _lParam
        End Get
        Set(ByVal value As Integer)
            _lParam = value
        End Set
    End Property

    <DisplayName("Message"), _
        Description("The value of the 'wParameter' in the message."), _
        DefaultValue(0)> _
    Public Property wParam() As Integer
        Get
            Return _wParam
        End Get
        Set(ByVal value As Integer)
            _wParam = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class clsActionLoadCue
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Load Cue"
    <NonSerialized()> _
    Public Const _Description As String = "Load the selected Cue"

    <Browsable(False)> _
    Private _cue As Integer
    <Browsable(False)> _
    Private _forceCuelistRestart As Boolean

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim ret As Integer = PostMessage(1002, _cue, If(_forceCuelistRestart, 1, 0))
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("Cue"), _
        Description("The Cue number to load." & vbCrLf & vbCrLf & "Set to 0 to clear Cue."), _
        DefaultValue(0)> _
    Public Property Cue() As Integer
        Get
            Return _cue
        End Get
        Set(ByVal value As Integer)
            _cue = value
        End Set
    End Property

    <DisplayName("Force Restart"), _
        Description("Force the Cue to restart."), _
        DefaultValue(False)> _
    Public Property ForceRestart() As Boolean
        Get
            Return _forceCuelistRestart
        End Get
        Set(ByVal value As Boolean)
            _forceCuelistRestart = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class clsActionLoadCueList
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Load CueList"
    <NonSerialized()> _
    Public Const _Description As String = "Load the selected CueList."

    <Browsable(False)> _
    Private _cuelist As Integer
    <Browsable(False)> _
    Private _forceCueListRestart As Boolean

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim ret As Integer = PostMessage(1001, _cuelist, If(_forceCueListRestart, 1, 0))
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("CueList"), _
    Description("The CueList number to load." & vbCrLf & vbCrLf & "Set to -1 to clear CueList."), _
    DefaultValue(0)> _
    Public Property Cue() As Integer
        Get
            Return _cuelist
        End Get
        Set(ByVal value As Integer)
            _cuelist = value
        End Set
    End Property

    <DisplayName("Force Restart"), _
        Description("Force the CueList to restart."), _
        DefaultValue(False)> _
    Public Property ForceRestart() As Boolean
        Get
            Return _forceCueListRestart
        End Get
        Set(ByVal value As Boolean)
            _forceCueListRestart = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class clsActionLoadBackgroundCue
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Load Backround Cue"
    <NonSerialized()> _
    Public Const _Description As String = "Load or merge the selected Background Cue, or clear the Background Cue."

    <Browsable(False)> _
    Private _bgcue As Integer
    Private _merge As Boolean

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim ret As Integer
        If (_bgcue = -1) Then
            ret = PostMessage(114, 9, 0) ''Clear BGCue
        Else
            If _merge Then
                ret = PostMessage(114, 10, _bgcue) ''Merge BGCue ("transparent")
            Else
                ret = PostMessage(114, 7, _bgcue) ''Load BGCue
            End If
        End If

        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("Background Cue"), _
    Description("The number of the Background Cue to load." & vbCrLf & vbCrLf & "Set to -1 to clear the Background Cue."), _
    DefaultValue(0)> _
    Public Property bgcue() As Integer
        Get
            Return _bgcue
        End Get
        Set(ByVal value As Integer)
            _bgcue = value
        End Set
    End Property

    <DisplayName("Merge"), _
        Description("Merge the new Background Cue with the currently running Background Cue. Occupied slots in the new cue will replace those slots in the old cue."), _
        DefaultValue(False)> _
    Public Property merge() As Boolean
        Get
            Return _merge
        End Get
        Set(ByVal value As Boolean)
            _merge = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class clsActionCueMacroAmplitudeRelative
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Macro Amplitude (Relative)"
    <NonSerialized()> _
    Public Const _Description As String = "Adjusts Cue Macro Control Amplitude by relative increments." & vbCrLf & vbCrLf & "Intended for use with relative controls, such as 'endless encoders.'"

    <Browsable(False)> _
    Private _multiplier As Integer
    <Browsable(False)> _
    Private _invert As Boolean

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim amt As Integer = VelVal
        If (VelVal > 63) Then
            amt = CType(Math.Floor((((128 - amt) * -1) * _multiplier) / 100), Integer)
        Else
            amt = CType(Math.Ceiling((amt * _multiplier) / 100), Integer)
        End If
        If (amt = 0) Then
            amt = If(VelVal > 63, amt - 1, amt + 1)
        End If
        If (_invert) Then amt = amt * -1

        Dim ret As Integer = PostMessage(167, 8150796, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("Increment Multiplier"), _
        Description("Typically used only with extremely sensitive or insensitive controls, adjust this value to change the multiplier of the given amount provided by the physical control." & vbCrLf & vbCrLf & "The default value of 100 (meaning '100%') uses the values sent by the controller. Values greater than 100 increase the 'speed' of change by that factor, while values between 0 and 100 decrease the speed of change."), _
        DefaultValue(100)> _
    Public Property Multiplier() As Integer
        Get
            Return _multiplier
        End Get
        Set(ByVal value As Integer)
            _multiplier = value
        End Set
    End Property

    <DisplayName("Invert Direction"), _
        Description("Set to 'True' to swap increment/decrement direction."), _
        DefaultValue(False)> _
    Public Property Invert() As Boolean
        Get
            Return _invert
        End Get
        Set(ByVal value As Boolean)
            _invert = value
        End Set
    End Property

    Public Sub New()
        _multiplier = 100
        _invert = False
    End Sub
End Class

<Serializable()> _
Public Class clsActionCueMacroAmplitudeAbsolute
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Macro Amplitude (Absolute)"
    <NonSerialized()> _
    Public Const _Description As String = "Adjusts Cue Macro Control Amplitude by absolute adjustments." & vbCrLf & vbCrLf & "Intended for use with absolute controls, such as faders and non-endless encoders."

    <Browsable(False)> _
    Private _double As Boolean
    <Browsable(False)> _
    Private _invert As Boolean

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim amt As Integer = VelVal

        If (_double) Then
            amt = amt * 2
            If (amt = 254) Then amt = 255 ''A little hack to avoid a maximum intensity of 254
        End If

        If (_invert) Then amt = amt * -1

        Dim ret As Integer = PostMessage(167, 8150796, -256)
        ret = PostMessage(167, 8150796, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("Double"), _
        Description("This should ALWAYS be set to 'True' unless you have a controller which breaks MIDI standard and has 'high resolution' faders which send values higher than 0-127."), _
        DefaultValue(True)> _
    Public Property vDouble() As Boolean
        Get
            Return _double
        End Get
        Set(ByVal value As Boolean)
            _double = value
        End Set
    End Property

    <DisplayName("Invert Direction"), _
        Description("Set to 'True' to swap increment/decrement direction."), _
        DefaultValue(False)> _
    Public Property Invert() As Boolean
        Get
            Return _invert
        End Get
        Set(ByVal value As Boolean)
            _invert = value
        End Set
    End Property

    Public Sub New()
        _double = True
        _invert = False
    End Sub
End Class

<Serializable()> _
Public Class clsActionCueMacroSpeedRelative
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Macro Speed (Relative)"
    <NonSerialized()> _
    Public Const _Description As String = "Adjusts Cue Macro Control Speed by relative increments." & vbCrLf & vbCrLf & "Intended for use with relative controls, such as 'endless encoders.'"

    <Browsable(False)> _
    Private _multiplier As Integer
    <Browsable(False)> _
    Private _invert As Boolean

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim amt As Integer = VelVal
        If (VelVal > 63) Then
            amt = CType(Math.Floor((((128 - amt) * -1) * _multiplier) / 100), Integer)
        Else
            amt = CType(Math.Ceiling((amt * _multiplier) / 100), Integer)
        End If
        If (amt = 0) Then
            amt = If(VelVal > 63, amt - 1, amt + 1)
        End If
        If (_invert) Then amt = amt * -1

        Dim ret As Integer = PostMessage(167, 8150800, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("Increment Multiplier"), _
        Description("Typically used only with extremely sensitive or insensitive controls, adjust this value to change the multiplier of the given amount provided by the physical control." & vbCrLf & vbCrLf & "The default value of 100 (meaning '100%') uses the values sent by the controller. Values greater than 100 increase the 'speed' of change by that factor, while values between 0 and 100 decrease the speed of change."), _
        DefaultValue(100)> _
    Public Property Multiplier() As Integer
        Get
            Return _multiplier
        End Get
        Set(ByVal value As Integer)
            _multiplier = value
        End Set
    End Property

    <DisplayName("Invert Direction"), _
        Description("Set to 'True' to swap increment/decrement direction."), _
        DefaultValue(False)> _
    Public Property Invert() As Boolean
        Get
            Return _invert
        End Get
        Set(ByVal value As Boolean)
            _invert = value
        End Set
    End Property

    Public Sub New()
        _multiplier = 100
        _invert = False
    End Sub
End Class

<Serializable()> _
Public Class clsActionCueMacroSpeedAbsolute
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Macro Speed (Absolute)"
    <NonSerialized()> _
    Public Const _Description As String = "Adjusts Cue Macro Control Speed by absolute adjustments." & vbCrLf & vbCrLf & "Intended for use with absolute controls, such as faders and non-endless encoders."

    <Browsable(False)> _
    Private _double As Boolean
    <Browsable(False)> _
    Private _invert As Boolean

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim amt As Integer = VelVal

        If (_double) Then
            amt = amt * 2
            If (amt = 254) Then amt = 255 ''A little hack to avoid a maximum intensity of 254
        End If

        If (_invert) Then amt = amt * -1

        Dim ret As Integer = PostMessage(167, 8150800, -256)
        ret = PostMessage(167, 8150800, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("Double"), _
        Description("This should ALWAYS be set to 'True' unless you have a controller which breaks MIDI standard and has 'high resolution' faders which send values higher than 0-127."), _
        DefaultValue(True)> _
    Public Property vDouble() As Boolean
        Get
            Return _double
        End Get
        Set(ByVal value As Boolean)
            _double = value
        End Set
    End Property

    <DisplayName("Invert Direction"), _
        Description("Set to 'True' to swap increment/decrement direction."), _
        DefaultValue(False)> _
    Public Property Invert() As Boolean
        Get
            Return _invert
        End Get
        Set(ByVal value As Boolean)
            _invert = value
        End Set
    End Property

    Public Sub New()
        _double = True
        _invert = False
    End Sub
End Class

<Serializable()> _
Public Class clsActionIntensityGroupAbsolute
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Intensity Group (Absolute)"
    <NonSerialized()> _
    Public Const _Description As String = "Control the value of an Intensity Group, or Master fader." & vbCrLf & vbCrLf & "This action is intended to be used with controls that give absolute values (such as standard faders, or non-continuous encoders)."

    <Browsable(False)> _
    Private _group As Byte
    <Browsable(False)> _
    Private _double As Boolean
    <Browsable(False)> _
    Private _invert As Boolean
    'Private Const WM_COPYDATA As Integer = &H4A ''74
    'Private Structure CopyData
    '    Friend dwData As Integer
    '    Friend cbData As Integer
    '    Friend lpData As IntPtr
    'End Structure
    Private Const _WMCOPY_SetIntensityMasters As Integer = 266
    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, Pack:=1, Size:=18)> _
    Private Structure IntensityState
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValArray, sizeconst:=9)> Public Flags As Byte()  '0: Ignore, <>0: Set value 
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValArray, sizeconst:=9)> Public Values As Byte() 'Value (0-255)
    End Structure

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim Intensities As IntensityState
        Intensities.Flags = New Byte() { _
            CByte(If(_group = 0, 1, 0)), _
            CByte(If(_group = 1, 1, 0)), _
            CByte(If(_group = 2, 1, 0)), _
            CByte(If(_group = 3, 1, 0)), _
            CByte(If(_group = 4, 1, 0)), _
            CByte(If(_group = 5, 1, 0)), _
            CByte(If(_group = 6, 1, 0)), _
            CByte(If(_group = 7, 1, 0)), _
            CByte(If(_group = 8, 1, 0)) _
        }
        Dim val As Byte = CByte(If(_invert, (127 - VelVal) * If(_double, 2, 1), VelVal * If(_double, 2, 1)))

        ''A little hack to avoid a maximum intensity of 254:
        If (_double) And (val = 254) Then val = 255

        Intensities.Values = New Byte() {val, val, val, val, val, val, val, val, val}

        Dim CData As CopyData
        CData.dwData = _WMCOPY_SetIntensityMasters
        CData.cbData = Runtime.InteropServices.Marshal.SizeOf(GetType(IntensityState))
        Dim Pointer As IntPtr
        Pointer = Runtime.InteropServices.Marshal.AllocHGlobal(Runtime.InteropServices.Marshal.SizeOf(Intensities))
        Runtime.InteropServices.Marshal.StructureToPtr(Intensities, Pointer, False)
        CData.lpData = Pointer

        Dim result As Integer = SendCopyData(CData)
        If (result = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <DisplayName("Group"), _
        Description("The index of the group to be controlled. For example, Master is '0', Intensity Group 1 is '1', and so on."), _
        DefaultValue(0)> _
    Public Property Group() As Byte
        Get
            Return _group
        End Get
        Set(ByVal value As Byte)
            _group = value
        End Set
    End Property

    <DisplayName("Double"), _
        Description("This should ALWAYS be set to 'True' unless you have a controller which breaks MIDI standard and has 'high resolution' faders which send values higher than 0-127."), _
        DefaultValue(True)> _
    Public Property vDouble() As Boolean
        Get
            Return _double
        End Get
        Set(ByVal value As Boolean)
            _double = value
        End Set
    End Property

    <DisplayName("Invert"), _
        Description("Set to 'True' to swap increment/decrement direction."), _
        DefaultValue(False)> _
    Public Property Invert() As Boolean
        Get
            Return _invert
        End Get
        Set(ByVal value As Boolean)
            _invert = value
        End Set
    End Property

    Public Sub New()
        _group = 0
        _double = True
        _invert = False
    End Sub
End Class

'TODO:
Public Class clsActionIntensityGroupRelative
    ''See uMsg 135, 53, x
End Class
#End Region

#Region "MIDI Actions"
'TODO:
<Serializable()> _
Public Class clsActionMidiSend
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Send MIDI Note"
    <NonSerialized()> _
    Public Const _Description As String = "Sends an arbitrary MIDI note command to a specified output."

    Private _dev As String
    Private _type As Boolean
    Private _chan As Byte
    Private _note As Byte
    Private _vel As Byte

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        'TODO: Uhhh, this is sorta hacky, but should work...
        'SendMidi(_dev, _str)
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <TypeConverter(GetType(OutDeviceList)), _
        DisplayName("Device"), _
        Description("Which MIDI output device to send to."), _
        DefaultValue("ALL DEVICES")> _
    Public Property dev() As String
        Get
            Return _dev
        End Get
        Set(ByVal value As String)
            _dev = value
        End Set
    End Property

    <DisplayName("Note On"), _
        Description("True for a Note On command. False for a Note Off command."), _
        DefaultValue(True)> _
    Public Property type() As Boolean
        Get
            Return _type
        End Get
        Set(ByVal value As Boolean)
            _type = value
        End Set
    End Property

    <DisplayName("Channel"), _
        Description("MIDI channel (1-16)."), _
        DefaultValue(1)> _
    Public Property chan() As Byte
        Get
            Return _chan
        End Get
        Set(ByVal value As Byte)
            _chan = value
        End Set
    End Property

    <DisplayName("Note"), _
        Description("MIDI note (0-127)."), _
        DefaultValue(0)> _
    Public Property note() As Byte
        Get
            Return _note
        End Get
        Set(ByVal value As Byte)
            _note = value
        End Set
    End Property

    <DisplayName("Velocity"), _
        Description("Note velocity (0-127)."), _
        DefaultValue(127)> _
    Public Property vel() As Byte
        Get
            Return _vel
        End Get
        Set(ByVal value As Byte)
            _vel = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class clsActionMidiSendString
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Send MIDI (String)"
    <NonSerialized()> _
    Public Const _Description As String = "Sends an arbitrary MIDI command formatted as a hexadecimal string." & vbCrLf & vbCrLf & "To explicitly set each MIDI component (Channel, Note, Velocity, etc..) use the 'Send MIDI' action instead."

    Private _str As String
    Private _dev As String

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim _device As Integer = main.FindDeviceByName(_dev)
        If Not (_device = -1) Then
            SendMidi(_dev, _str)
            Return True
        Else
            Return False
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <TypeConverter(GetType(OutDeviceList)), _
        DisplayName("Device"), _
        Description("Which MIDI output device to send to."), _
        DefaultValue("ALL DEVICES")> _
    Public Property dev() As String
        Get
            Return _dev
        End Get
        Set(ByVal value As String)
            _dev = value
        End Set
    End Property

    <DisplayName("MIDI String"), _
        Description("The MIDI command to send, in hexadecimal string format." & vbCrLf & vbCrLf & "Example: '9F 00 01' is Note 0 Off, Channel 16, Velocity 1"), _
        DefaultValue(0)> _
    Public Property str() As String
        Get
            Return _str
        End Get
        Set(ByVal value As String)
            _str = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class clsActionMidiControlChange
    Implements iAction

    <NonSerialized()> _
    Public Const _Name As String = "Send MIDI (Control Change)"
    <NonSerialized()> _
    Public Const _Description As String = "Sends MIDI Control Change messages."

    Private _cont As Byte
    Private _chan As Byte
    Private _dev As String

    <Browsable(False)> _
    Public ReadOnly Property Description() As String Implements iAction.Description
        Get
            Return _Description
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements iAction.Execute
        Dim _device As Integer = main.FindDeviceByName(_dev)
        If Not (_device = -1) Then
            'TODO: checks to make sure _chan is <= 15, _cont <= 127
            Dim cntstr As String = "B" & _chan.ToString("X") & _cont.ToString("X2") & VelVal.ToString("X2")
            'Diagnostics.Debug.WriteLine("Control Change: " & cntstr)
            SendMidi(dev, cntstr)
            Return True
        Else
            Return False
        End If
    End Function

    <Browsable(False)> _
    Public ReadOnly Property Name() As String Implements iAction.Name
        Get
            Return _Name
        End Get
    End Property

    <TypeConverter(GetType(OutDeviceList)), _
        DisplayName("Device"), _
        Description("Which MIDI output device to send to."), _
        DefaultValue("ALL DEVICES")> _
    Public Property dev() As String
        Get
            Return _dev
        End Get
        Set(ByVal value As String)
            _dev = value
        End Set
    End Property

    <DisplayName("Channel"), _
        Description("Which MIDI channel to direct the message. (0-15)"), _
        DefaultValue(0)> _
    Public Property chan() As Byte
        Get
            Return _chan
        End Get
        Set(ByVal value As Byte)
            _chan = value
        End Set
    End Property

    <DisplayName("Control"), _
        Description("The MIDI control from which to send the message."), _
        DefaultValue(0)> _
    Public Property cont() As Byte
        Get
            Return _cont
        End Get
        Set(ByVal value As Byte)
            _cont = value
        End Set
    End Property
End Class
#End Region

#Region "PropertyGrid Helpers"
Public Class DeviceList
    Inherits ComponentModel.StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
        Dim devArr As Collections.Generic.List(Of String) = New Collections.Generic.List(Of String)
        devArr.Add("ALL DEVICES")

        For Each device As Integer In Configuration.Connections.Keys
            devArr.Add(Configuration.Connections(device).Name)
        Next

        Return New StandardValuesCollection(devArr.ToArray)
        'Return New StandardValuesCollection(New String() {"ALL DEVICES", "Device 1", "Device 2", "Device 3"})
    End Function
End Class

Public Class OutDeviceList
    Inherits ComponentModel.StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
        Dim devArr As Collections.Generic.List(Of String) = New Collections.Generic.List(Of String)
        devArr.Add("ALL DEVICES")
        For Each device As Integer In Configuration.Connections.Keys
            If Configuration.Connections(device).OutputEnable Then devArr.Add(Configuration.Connections(device).Name)
        Next
        Return New StandardValuesCollection(devArr.ToArray)
    End Function
End Class
#End Region








''DEPRECIATED CLASSES
'<Serializable()> _
'Public Class clsIntShift
'    Public Const Name As String = "Shift"
'    Public Const Description As String = "Sets or removes a 'Shift' condition. Controls and actions may have additional or alternate behaviors when a Shift state is active."
'    Public Toggle As Boolean
'    Private _device As String

'    <TypeConverter(GetType(DeviceList)), _
'        DisplayName("Target Device"), _
'        Description("Which device to enable Shift functionality."), _
'        DefaultValue("")> _
'    Public Property Device() As String
'        Get
'            Return _device
'        End Get
'        Set(ByVal value As String)
'            _device = value
'        End Set
'    End Property
'End Class
#Region "Windows Actions"
'<Serializable()> _
'Public Class clsWinCommand
'    Private _command As String
'End Class

'<Serializable()> _
'Public Class clsWinRun
'    Private _path As String
'    Private _switches As String
'End Class

'<Serializable()> _
'Public Class clsWinSendkeys
'    Private _handle As Integer
'    Private _window As String
'    Private _keys As String
'End Class
#End Region