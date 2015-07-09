Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE470")> _
Public Class ExitProgram
    Implements IAction

    Private _host As IServices
    Private _actionData As ExitProgram.ActionData

    ''' <summary>
    ''' ExitProgram.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _restart As Boolean

        <DisplayName("Restart"), _
            Description("Restart program after exiting.'"), _
            DefaultValue(False)> _
        Public Property Group() As Boolean
            Get
                Return _restart
            End Get
            Set(ByVal value As Boolean)
                _restart = value
            End Set
        End Property

        Public Sub New()
            _restart = False
        End Sub
    End Class

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _actionData
        End Get
        Set(ByVal value As Object)
            _actionData = CType(value, ExitProgram.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Exits Feel, and optionally restarts."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.ExitProgram(_actionData._restart)
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _host = Host
        _actionData = New ExitProgram.ActionData
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Exit Program"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE470")
        End Get
    End Property
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE471")> _
Public Class SaveConfiguration
    Implements IAction

    Private _host As IServices

    Public Property Data() As Object Implements IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            ''Nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Saves the current Feel configuration to file, overwriting the currently used filename."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.SaveConfiguration()
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Save Configuration"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE471")
        End Get
    End Property
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE478")> _
Public Class ConfigureProgram
    Implements IAction

    Private _host As IServices

    Public Property Data() As Object Implements IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            ''Nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Opens the 'Configure Program' window." & vbCrLf & vbCrLf & "This action has no editable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.OpenWindowConfig()
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Configure Program"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE478")
        End Get
    End Property
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE472")> _
Public Class ConfigureConnections
    Implements IAction

    Private _host As IServices
    'Public Class ActionData
    '    'Public Function Clone() As ShowMessageBox.ActionData
    '    '    Return DirectCast(Me.MemberwiseClone(), ShowMessageBox.ActionData)
    '    'End Function
    'End Class

    Public Property Data() As Object Implements IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            ''Nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Opens the 'Configure Connections' window." & vbCrLf & vbCrLf & "This action has no editable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.OpenWindowConnections()
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Configure Connections"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE472")
            'Return "15ABCE5A-D91E-4d65-946A-58394B9FE472"
        End Get
    End Property

    'Public Function ShallowClone() As ChangePageData
    '    Return DirectCast(Me.MemberwiseClone(), ChangePageData)
    'End Function

    'Public Function DeepClone() As ChangePageData
    '    Dim newClone As ChangePageData = DirectCast(Me.MemberwiseClone(), ChangePageData)
    '    newClone._device = String.Copy(_device)
    '    Return newClone
    'End Function
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE473")> _
Public Class ConfigureActions
    Implements IAction

    Private _host As IServices

    Public Property Data() As Object Implements IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            ''Nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Opens the 'Configure Events' window." & vbCrLf & vbCrLf & "This action has no editable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.OpenWindowActions()
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Configure Events"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE473")
        End Get
    End Property
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE474")> _
Public Class ChangePage
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Triggers a page change on one, or all, control surface(s) to a specific page." & vbCrLf & vbCrLf & "• To change multiple controllers to different pages, use multiple actions." & vbCrLf & vbCrLf & "• To change page by an increment, use 'Skip Pages' instead." & vbCrLf & vbCrLf & "It is HIGHLY RECOMMENDED to disable the 'Paged Control' option when defining page change actions."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        '_myData.Page = CByte(_host.GetCurrentCue)
        'System.Diagnostics.Debug.WriteLine("ChangePage Execute: " & _myData.Page)

        _host.SetPage(_myData.Device, _myData.Page)
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Change Page"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE474")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ActionData)
        End Set
    End Property

    <Serializable()> _
    Public Class ActionData
        Private _device As String
        Private _page As Byte

        <TypeConverter(GetType(ChangePage.DeviceList)), _
            DisplayName("Target Device"), _
            Description("Which connection (or device) to update."), _
            DefaultValue("ALL DEVICES")> _
        Public Property Device() As String
            Get
                Return _device
            End Get
            Set(ByVal value As String)
                _device = value
            End Set
        End Property

        <DisplayName("Page"), _
            Description("The target page number."), _
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

        Public Sub New()
            _device = "ALL DEVICES"
            _page = 0
        End Sub
    End Class

    Public Class DeviceList
        Inherits ComponentModel.StringConverter

        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
            Dim devArr As String() = New String() {}
            For Each device As String In ChangePage._host.GetMIDIDeviceList
                Array.Resize(devArr, devArr.Length + 1)
                devArr(devArr.Length - 1) = device
            Next
            Return New StandardValuesCollection(devArr)
        End Function
    End Class
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE475")> _
Public Class ShowMessageBox
    Implements IAction

    Private _actionData As ActionData
    Private _host As ActionInterface.IServices

    ''' <summary>
    ''' ShowMessageBox.ActionData
    ''' </summary>
    ''' <remarks>Stores the custom message to display in the messagebox.</remarks>
    <Serializable()> _
    Public Class ActionData
        Private _message As String

        <DisplayName("Message"), _
            Description("The message to display in the message dialog box."), _
            DefaultValue("My message!")> _
        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = value
            End Set
        End Property

        Public Sub New()
            _message = "My message!"
        End Sub
    End Class

    Public Property Data() As Object Implements ActionInterface.IAction.Data
        Get
            Return _actionData
        End Get
        Set(ByVal value As Object)
            _actionData = DirectCast(value, ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ActionInterface.IAction.Description
        Get
            Return "Displays a message box with a custom message."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements ActionInterface.IAction.Execute
        MsgBox(_actionData.Message)
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements ActionInterface.IAction.Group
        Get
            Return "Other"
        End Get
    End Property

    Public Function Initialize(ByRef Host As ActionInterface.IServices) As Boolean Implements ActionInterface.IAction.Initialize
        Return False ''Switch to enable

        _host = Host
        _actionData = New ActionData
    End Function

    Public ReadOnly Property Name() As String Implements ActionInterface.IAction.Name
        Get
            Return "Display Message Box"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements ActionInterface.IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE475")
        End Get
    End Property
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE476")> _
Public Class ChangeControlState
    Implements IAction

    Private _actionData As ChangeControlState.ActionData
    Private _host As ActionInterface.IServices

    ''' <summary>
    ''' ChangeControlState.ActionData
    ''' </summary>
    ''' <remarks>Stores the MIDI state string to send to the device.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _state As String

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

        Public Sub New()
            _state = ""
        End Sub
    End Class

    Public Property Data() As Object Implements ActionInterface.IAction.Data
        Get
            Return _actionData
        End Get
        Set(ByVal value As Object)
            _actionData = CType(value, ChangeControlState.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements ActionInterface.IAction.Description
        Get
            Return "Sends MIDI to a device, with the intention of changing the status of a control, for example button illumination, and preserves this state until modified." & vbCrLf & vbCrLf & "• This action is identical to the 'Send MIDI' action, with the exception that 'Change Control State' preserves the control's state for repeated use, for example when changing pages."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements ActionInterface.IAction.Execute
        Dim _device As Integer = _host.FindDeviceIndexByInput(Device)
        Dim ContStr As String = If(Type = 144 Or Type = 128, "9", "B") & Channel.ToString & NoteCon.ToString("X2")
        If (_host.ControlExists(_device, ContStr)) Then
            _host.CurrentState(_device, ContStr, _host.CurrentPage(_device)) = _actionData._state
            _host.SendMIDI(_device, _actionData._state)
            Return True
        Else
            Return False
        End If
        ''OLD WAY:
        'If Not (_host.ConnectionExists(_device)) Then
        '    Return False
        'Else
        '    Dim ContStr As String = If(Type = 144 Or Type = 128, "9", "B") & Channel.ToString & NoteCon.ToString("X2")
        '    If Not (_host.ControlExists(_device, ContStr)) Then
        '        Return False
        '    Else
        '        'TODO: Using ".CurrentPage" is a hacky shortcut, and could lead to trouble down the line
        '        _host.CurrentState(_device, ContStr, _host.CurrentPage(_device)) = _state
        '        _host.SendMIDI(_device, _state)
        '        Return True
        '    End If
        'End If
    End Function

    Public ReadOnly Property Group() As String Implements ActionInterface.IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _host = Host
        _actionData = New ChangeControlState.ActionData
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements ActionInterface.IAction.Name
        Get
            Return "Change Control State"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements ActionInterface.IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE476")
        End Get
    End Property
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE477")> _
Public Class ResetControlGroup
    Implements ActionInterface.IAction

    Private _actionData As ActionData
    Private _host As ActionInterface.IServices

    ''' <summary>
    ''' ResetControlGroup.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _group As Byte
        Friend _redraw As Boolean

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

        Public Sub New()
            _group = 0
            _redraw = True
        End Sub
    End Class

    Public Property Data() As Object Implements ActionInterface.IAction.Data
        Get
            Return _actionData
        End Get
        Set(ByVal value As Object)
            _actionData = DirectCast(value, ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Sets all controls in group to 'inactive' (momentary/latch button status), and resets control state."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements ActionInterface.IAction.Execute
        _host.ResetControlsByGroup(_actionData._group)
        If (_actionData._redraw) Then _host.RedrawControls()
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements ActionInterface.IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _host = Host
        _actionData = New ActionData
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements ActionInterface.IAction.Name
        Get
            Return "Reset Controls by Group"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements ActionInterface.IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE477")
        End Get
    End Property
End Class