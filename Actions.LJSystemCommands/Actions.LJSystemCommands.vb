Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("1686F2F4-FEFC-4618-8305-B6FAED070024")> _
Public Class CloseActiveWindow
    Implements IAction

    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            'nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Closes the currently active (selected) LightJockey window." & vbCrLf & vbCrLf & "This action has no properties to configure."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(113, 1, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ System Commands"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Close Active Window"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("1686F2F4-FEFC-4618-8305-B6FAED070024")
        End Get
    End Property
End Class

<Guid("1686F2F4-FEFC-4618-8305-B6FAED070025")> _
Public Class CloseAllWindows
    Implements IAction

    Private _myData As CloseAllWindows.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            'nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Closes all open LightJockey windows."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(113, If(_myData._force, 3, 2), 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ System Commands"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Close All Windows"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("1686F2F4-FEFC-4618-8305-B6FAED070025")
        End Get
    End Property

    ''' <summary>
    ''' CloseAllWindows.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _force As Boolean

        <DisplayName("Force"), _
            Description("Force close windows waiting for input."), _
            DefaultValue(False)> _
        Public Property Force() As Boolean
            Get
                Return _force
            End Get
            Set(ByVal value As Boolean)
                _force = value
            End Set
        End Property

        Public Sub New()
            _force = False
        End Sub
    End Class
End Class

<Guid("1686F2F4-FEFC-4618-8305-B6FAED070026")> _
Public Class RestartLightJockey
    Implements IAction

    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            'nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Closes the currently running instance of LightJockey, and restarts the program. WARNING: All 'helper' programs will need to redetect the LightJockey window handle, or be restarted." & vbCrLf & vbCrLf & "This action has no properties to configure."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(113, 1, 0)
        _host.ResetLJWindowHandle()
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ System Commands"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Restart LightJockey"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("1686F2F4-FEFC-4618-8305-B6FAED070026")
        End Get
    End Property
End Class

<Guid("1686F2F4-FEFC-4618-8305-B6FAED070027")> _
Public Class ShutdownLightJockey
    Implements IAction

    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            'nothing to do
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Closes the currently running instance of LightJockey." & vbCrLf & vbCrLf & "This action has no properties to configure."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(113, 1, 0)
        _host.ResetLJWindowHandle()
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ System Commands"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Shutdown LightJockey"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("1686F2F4-FEFC-4618-8305-B6FAED070027")
        End Get
    End Property
End Class