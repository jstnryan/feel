Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D66")> _
Public Class ClearNewSequence
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
            Return "Clears the currently running Sequence (load new)." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 1, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Clear/New Sequence"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D66")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D67")> _
Public Class AddScene
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
            Return "Adds a scene to the end of the current Sequence." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 2, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Add Scene"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D67")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D68")> _
Public Class DeleteSequencesWindow
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
            Return "Show the 'Delete Sequences' window. This window appears as a modal dialog (you must close it before performing other interactions)." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 3, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Show Delete Sequences Window"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D68")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D69")> _
Public Class DeleteSequence
    Implements IAction

    Private _myData As DeleteSequence.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, DeleteSequence.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Delete the selected sequence."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, If(_myData._updateLoadList, 13, 4), _myData._sequence)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
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
            Return "Delete Sequence"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D69")
        End Get
    End Property

    ''' <summary>
    ''' DeleteSequence.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _sequence As Integer
        Friend _updateLoadList As Boolean

        <DisplayName("Sequence"), _
            Description("The Sequence number to be deleted."), _
            DefaultValue(0)> _
        Public Property Group() As Integer
            Get
                Return _sequence
            End Get
            Set(ByVal value As Integer)
                _sequence = value
            End Set
        End Property

        <DisplayName("Update LoadList"), _
            Description("Update the list of Sequences, on screen, after deleting. In most circumstances, this should be set to True."), _
            DefaultValue(True)> _
        Public Property vDouble() As Boolean
            Get
                Return _updateLoadList
            End Get
            Set(ByVal value As Boolean)
                _updateLoadList = value
            End Set
        End Property

        Public Sub New()
            _sequence = 0
            _updateLoadList = True
        End Sub
    End Class
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6A")> _
Public Class PreviousScene
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
            Return "Jump to the previous scene in the Sequence." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 5, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Previous Scene (Back)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6A")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6B")> _
Public Class NextScene
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
            Return "Jump to the next scene in the Sequence." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 6, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Next Scene (Forward)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6B")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6C")> _
Public Class SaveSequenceDialog
    Implements IAction

    Private _myData As SaveSequenceDialog.ActionData
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
            Return "Show, or Hide the 'Save Sequence' dialog window."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, _myData._state, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Save Sequence Dialog"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6C")
        End Get
    End Property

    ''' <summary>
    ''' SaveSequenceDialog.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _state As SaveSequenceDialog.State

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Whether to Show, or Hide the dialog window."), _
            DefaultValue(SaveSequenceDialog.State.Show)> _
        Public Property Group() As SaveSequenceDialog.State
            Get
                Return _state
            End Get
            Set(ByVal value As SaveSequenceDialog.State)
                _state = value
            End Set
        End Property

        Public Sub New()
            _state = 0
        End Sub
    End Class

    Public Enum State As Byte
        <Description("Show")> Show = 8
        <Description("Hide")> Hide = 9
    End Enum
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6D")> _
Public Class SaveSequenceQuick
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
            Return "Quick save the current Sequence. This will fall back to opening the Save Sequence dialog if the current Sequence was not previously saved." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 11, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Save Sequence (Quick)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6D")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6E")> _
Public Class SelectSequenceWindow
    Implements IAction

    Private _myData As SelectSequenceWindow.ActionData
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
            Return "Show, Hide, or Toggle the 'Select Sequence' window."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, If(_myData._state = 3, 16, 15), _myData._state)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Select Sequence Window"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6E")
        End Get
    End Property

    ''' <summary>
    ''' SelectSequenceWindow.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _state As SelectSequenceWindow.State

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Whether to Show, Hide, or Toggle the window."), _
            DefaultValue(SaveSequenceDialog.State.Show)> _
        Public Property Group() As SelectSequenceWindow.State
            Get
                Return _state
            End Get
            Set(ByVal value As SelectSequenceWindow.State)
                _state = value
            End Set
        End Property

        Public Sub New()
            _state = 0
        End Sub
    End Class

    Public Enum State As Byte
        <Description("Show")> Show = 0
        <Description("Toggle")> Toggle = 1
        <Description("Hide")> Hide = 2
    End Enum
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6F")> _
Public Class EditPlaybackForward
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
            Return "Set Sequence playback to Forward." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 29, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Edit Playback (Forward)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D6F")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D70")> _
Public Class EditPlaybackReverse
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
            Return "Set Sequence playback to Reverse." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 30, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Edit Playback (Reverse)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D70")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D71")> _
Public Class EditPlaybackStop
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
            Return "Set Sequence playback to Stop." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        'TODO: This action has additional lParam config which is currently unkown
        _host.PostLJMessage(116, 31, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Edit Playback (Stop)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D71")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D72")> _
Public Class DeleteScenesWindow
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
            Return "Shows the 'Delete Scene(s)' window as a modal dialog (other interactions with LightJockey are prevented until the window is dismissed)." & vbCrLf & vbCrLf & "This action has no configurable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        'TODO: This action has additional lParam config which is currently unkown
        _host.PostLJMessage(116, 34, 0)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Delete Scene(s) Window"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D72")
        End Get
    End Property
End Class

<Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D73")> _
Public Class DeleteScenes
    Implements IAction

    Private _myData As DeleteScenes.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, DeleteScenes.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Delete one or more Scenes from the current Sequence within a range."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(116, 35, (_myData._end << 16) + _myData._start)
        Return True ''No error check
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ Sequence Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New DeleteScenes.ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Delete Scenes"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("20E33D32-7502-4e6c-B2BC-C6F7CF482D73")
        End Get
    End Property

    ''' <summary>
    ''' DeleteScenes.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _start As Integer
        Friend _end As Integer

        <DisplayName("Start Scene"), _
            Description("The first Scene in the range to delete."), _
            DefaultValue(1)> _
        Public Property StartScene() As Integer
            Get
                Return _start
            End Get
            Set(ByVal value As Integer)
                _start = If(value > 65535, 65535, If(value < 1, 1, value))
            End Set
        End Property

        <DisplayName("End Scene"), _
            Description("The last Scene in the range to delete."), _
            DefaultValue(1)> _
        Public Property EndScene() As Integer
            Get
                Return _end
            End Get
            Set(ByVal value As Integer)
                _end = If(value > 65535, 65535, If(value < 1, 1, value))
            End Set
        End Property

        Public Sub New()
            _start = 1
            _end = 1
        End Sub
    End Class
End Class