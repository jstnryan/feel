Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("7A20357E-9779-44b6-9515-D51DFC1848A4")> _
Public Class BackgroundCueWindow
    Implements IAction

    Private _myData As BackgroundCueWindow.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, BackgroundCueWindow.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Show, Hide, or Toggle the Background Cue window."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(114, _myData._state, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Background Cue Window"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("7A20357E-9779-44b6-9515-D51DFC1848A4")
        End Get
    End Property

    ''' <summary>
    ''' BackgroundCueWindow.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _state As BackgroundCueWindow.State

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Whether to Show, Hide, or Toggle the Background Cue window."), _
            DefaultValue(BackgroundCueWindow.State.Show)> _
        Public Property State() As BackgroundCueWindow.State
            Get
                Return _state
            End Get
            Set(ByVal value As BackgroundCueWindow.State)
                _state = value
            End Set
        End Property

        Sub New()
            _state = BackgroundCueWindow.State.Show
        End Sub
    End Class

    Public Enum State As Byte
        <Description("Show")> Show = 1
        <Description("Hide")> Hide = 2
        <Description("Toggle")> Toggle = 11
    End Enum
End Class

<Guid("7A20357E-9779-44b6-9515-D51DFC1848A5")> _
Public Class BackgroundCueSaveDialog
    Implements IAction

    Private _myData As BackgroundCueSaveDialog.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, BackgroundCueSaveDialog.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Show, or Hide the Background Cue 'Save' dialog window."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(114, _myData._state, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Background Cue Save Dialog"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("7A20357E-9779-44b6-9515-D51DFC1848A5")
        End Get
    End Property

    ''' <summary>
    ''' BackgroundCueWindow.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _state As BackgroundCueSaveDialog.State

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Whether to Show, Hide, or Toggle the Background Cue window."), _
            DefaultValue(BackgroundCueSaveDialog.State.Show)> _
        Public Property State() As BackgroundCueSaveDialog.State
            Get
                Return _state
            End Get
            Set(ByVal value As BackgroundCueSaveDialog.State)
                _state = value
            End Set
        End Property

        Sub New()
            _state = BackgroundCueSaveDialog.State.Show
        End Sub
    End Class

    Public Enum State As Byte
        <Description("Show")> Show = 3
        <Description("Hide")> Hide = 4
    End Enum
End Class

<Guid("7A20357E-9779-44b6-9515-D51DFC1848A6")> _
Public Class SaveBackgroundCue
    Implements IAction

    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            ''Nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Save the currently running Background Cue." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(114, 5, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Save Background Cue"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("7A20357E-9779-44b6-9515-D51DFC1848A6")
        End Get
    End Property
End Class

<Guid("7A20357E-9779-44b6-9515-D51DFC1848A7")> _
Public Class SaveNewBackgroundCue
    Implements IAction

    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            ''Nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Save the currently running Background Cue as a new Background Cue." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(114, 6, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Save New Background Cue"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("7A20357E-9779-44b6-9515-D51DFC1848A7")
        End Get
    End Property
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A8")> _
Public Class LoadBackgroundCue
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Load or merge the selected Background Cue, or clear the Background Cue."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim ret As Integer
        If (_myData._bgcue = -1) Then
            ret = _host.PostLJMessage(114, 9, 0) ''Clear BGCue
        Else
            If _myData._merge Then
                ret = _host.PostLJMessage(114, 10, _myData._bgcue) ''Merge BGCue ("transparent")
            Else
                ret = _host.PostLJMessage(114, 7, _myData._bgcue) ''Load BGCue
            End If
        End If

        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Load Background Cue"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A8")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, LoadBackgroundCue.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' LoadBackgroundCue.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _bgcue As Integer
        Friend _merge As Boolean

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

        Public Sub New()
            _bgcue = 0
            _merge = False
        End Sub
    End Class
End Class

<Guid("7A20357E-9779-44b6-9515-D51DFC1848A8")> _
Public Class ClearNewBackgroundCue
    Implements IAction

    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            ''Nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Clear the (load a new, blank) Background Cue." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(114, 9, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Clear/New Background Cue"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("7A20357E-9779-44b6-9515-D51DFC1848A8")
        End Get
    End Property
End Class

<Guid("7A20357E-9779-44b6-9515-D51DFC1848A9")> _
Public Class SelectBackgroundCueWindow
    Implements IAction

    Private _myData As SelectBackgroundCueWindow.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, SelectBackgroundCueWindow.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Show, Hide, or Toggle the 'Select Background Cue' window."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(114, 12, _myData._state)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Select Background Cue Window"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("7A20357E-9779-44b6-9515-D51DFC1848A9")
        End Get
    End Property

    ''' <summary>
    ''' SelectBackgroundCueWindow.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _state As SelectBackgroundCueWindow.State

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Whether to Show, Hide, or Toggle the 'Select Background Cue' window."), _
            DefaultValue(SelectBackgroundCueWindow.State.Show)> _
        Public Property State() As SelectBackgroundCueWindow.State
            Get
                Return _state
            End Get
            Set(ByVal value As SelectBackgroundCueWindow.State)
                _state = value
            End Set
        End Property

        Sub New()
            _state = SelectBackgroundCueWindow.State.Show
        End Sub
    End Class

    Public Enum State As Byte
        <Description("Show")> Show = 1
        <Description("Hide")> Hide = 0
        <Description("Toggle")> Toggle = 2
    End Enum
End Class

<Guid("7A20357E-9779-44b6-9515-D51DFC1848AA")> _
Public Class SetAllBGCueSlotsState
    Implements IAction

    Private _myData As SetAllBGCueSlotsState.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, SetAllBGCueSlotsState.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Set all Background Cue slots On, or Off."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(114, 13, _myData._state)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Set All BGCue Slots State"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("7A20357E-9779-44b6-9515-D51DFC1848AA")
        End Get
    End Property

    ''' <summary>
    ''' SetAllBGCueSlotsState.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _state As SetAllBGCueSlotsState.State

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Turn On, or Off all Background Cue slots."), _
            DefaultValue(SetAllBGCueSlotsState.State.OnState)> _
        Public Property State() As SetAllBGCueSlotsState.State
            Get
                Return _state
            End Get
            Set(ByVal value As SetAllBGCueSlotsState.State)
                _state = value
            End Set
        End Property

        Sub New()
            _state = SetAllBGCueSlotsState.State.OnState
        End Sub
    End Class

    Public Enum State As Byte
        <Description("On")> OnState = 1
        <Description("Off")> OffState = 0
    End Enum
End Class

<Guid("7A20357E-9779-44b6-9515-D51DFC1848AB")> _
Public Class ToggleBGCueSlotState
    Implements IAction

    Private _myData As ToggleBGCueSlotState.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ToggleBGCueSlotState.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Toggle a single Background Cue slot between On and Off."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(114, 15, _myData._slot - 1)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Background Cue Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Toggle BGCue Slot State"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("7A20357E-9779-44b6-9515-D51DFC1848AB")
        End Get
    End Property

    ''' <summary>
    ''' ToggleBGCueSlotState.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _slot As Integer

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("BGCue Slot Number"), _
            Description("The Background Cue slot number to toggle On or Off."), _
            DefaultValue(0)> _
        Public Property Slot() As Integer
            Get
                Return _slot
            End Get
            Set(ByVal value As Integer)
                _slot = If(value > 5, 5, If(value < 1, 1, value))
            End Set
        End Property

        Sub New()
            _slot = 0
        End Sub
    End Class
End Class