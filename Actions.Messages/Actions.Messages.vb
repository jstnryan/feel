Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("4CDE2694-8BA6-415d-94EA-EDA045149BCF")> _
Public Class SendLJWMessage
    Implements IAction

    Private _actionData As ActionData
    Private _host As IServices

    ''' <summary>
    ''' SendWMessage.ActionData
    ''' </summary>
    ''' <remarks>Stores the custom message to display in the messagebox.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _msg As Integer
        Friend _lParam As Integer
        Friend _wParam As Integer

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

        Public Sub New()
            _msg = 0
            _lParam = 0
            _wParam = 0
        End Sub
    End Class

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _actionData
        End Get
        Set(ByVal value As Object)
            _actionData = DirectCast(value, ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Sends a Windows Message to LightJockey." & vbCrLf & vbCrLf & "This differs from the 'Post Windows Message' function in that it waits for a response from LightJockey before continuing. The advantage being that you can ensure the message was received (and executed before other messages in the queue), but at the expense of execution speed. Post Windows Message is preferred in most circumstances."
        End Get
    End Property

    'Function Execute(ByVal Device As String, ByVal EventData As Object, ByVal ActionData As Object) As Boolean Implements IAction.Execute
    Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        ''SendLJMessage returns an Integer (the same return from LightJockey), but there's not much that can be done with it here..
        _host.SendLJMessage(_actionData._msg, _actionData._lParam, _actionData._wParam)
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Windows Messages"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _host = Host
        _actionData = New ActionData
    End Sub

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Send Windows Message (LJ)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("4CDE2694-8BA6-415d-94EA-EDA045149BCF")
        End Get
    End Property
End Class

<Guid("4CDE2694-8BA6-415d-94EA-EDA045149BCE")> _
Public Class PostLJWMessage
    Implements IAction

    Private _actionData As ActionData
    Private _host As IServices

    ''' <summary>
    ''' SendWMessage.ActionData
    ''' </summary>
    ''' <remarks>Stores the custom message to display in the messagebox.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _msg As Integer
        Friend _lParam As Integer
        Friend _wParam As Integer

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

        Public Sub New()
            _msg = 0
            _lParam = 0
            _wParam = 0
        End Sub
    End Class

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _actionData
        End Get
        Set(ByVal value As Object)
            _actionData = DirectCast(value, ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Sends an arbitrary Windows Message to LightJockey." & vbCrLf & vbCrLf & "This differs from the 'Send Windows Message' function in that it doesn't wait for a response from LightJockey after sending the message. The advantage being speed over a guarantee that the message was received."
        End Get
    End Property

    'Function Execute(ByVal Device As String, ByVal EventData As Object, ByVal ActionData As Object) As Boolean Implements IAction.Execute
    Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        ''SendLJMessage returns an Integer (the same return from LightJockey), but there's not much that can be done with it here..
        _host.PostLJMessage(_actionData._msg, _actionData._lParam, _actionData._wParam)
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Windows Messages"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _host = Host
        _actionData = New ActionData
    End Sub

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Post Windows Message (LJ)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("4CDE2694-8BA6-415d-94EA-EDA045149BCE")
        End Get
    End Property
End Class

<Guid("4CDE2694-8BA6-415d-94EA-EDA045149BCD")> _
Public Class ResetLJWindowHandle
    Implements IAction

    Private _host As IServices

    Public Property Data() As Object Implements IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            'do nothing
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Resets the stored window 'handle' value for LightJockey, and forces it to be retrieved again." & vbCrLf & vbCrLf & "This is necessary if LightJockey was restarted."
        End Get
    End Property

    'Function Execute(ByVal Device As String, ByVal EventData As Object, ByVal ActionData As Object) As Boolean Implements IAction.Execute
    Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.ResetLJWindowHandle()
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Windows Messages"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _host = Host
    End Sub

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Reset LJ Window Handle"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("4CDE2694-8BA6-415d-94EA-EDA045149BCD")
        End Get
    End Property
End Class