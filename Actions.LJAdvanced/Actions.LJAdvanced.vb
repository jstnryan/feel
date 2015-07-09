Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A0")> _
Public Class IntensityGroupValue
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Control intensity values by Intensity Group."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        '_host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
    End Sub

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Intensity Group Value"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A0")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, IntensityGroupValue.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' IntensityGroup.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        'Friend _function As ExternalSystem.Hotkey
        'Friend _state As FunctionState

        '<TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
        '    DisplayName("Function"), _
        '    Description("Which function/hotkey to perform."), _
        '    DefaultValue(ExternalSystem.Hotkey.CloseWindow)> _
        'Public Property Funct() As ExternalSystem.Hotkey
        '    Get
        '        Return _function
        '    End Get
        '    Set(ByVal value As ExternalSystem.Hotkey)
        '        _function = value
        '    End Set
        'End Property

        '<TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
        '    DisplayName("State"), _
        '    Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
        '    DefaultValue(FunctionState.FunctionOn)> _
        'Public Property State() As FunctionState
        '    Get
        '        Return _state
        '    End Get
        '    Set(ByVal value As FunctionState)
        '        _state = value
        '    End Set
        'End Property

        'Public Sub New()
        '    _function = ExternalSystem.Hotkey.CloseWindow
        '    _state = FunctionState.FunctionOn
        'End Sub
    End Class
End Class
