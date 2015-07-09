Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("15ABCE5A-D91E-4d65-946B-58394B9FE400")> _
Public Class SendMidiString
    Implements IAction

    Private _myData As ActionData
    Private _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Sends an arbitrary MIDI command formatted as a hexadecimal string." & vbCrLf & vbCrLf & "To explicitly set each MIDI component (Channel, Note, Velocity, etc..) use the 'Send MIDI' action instead."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim _device As Integer = _host.FindDeviceIndexByName(_myData._dev)
        If Not (_device = -1) Then
            _host.SendMIDI(_myData._dev, _myData._str)
            Return True
        Else
            Return False
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
    End Sub

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Send MIDI (String)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946B-58394B9FE400")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, SendMidiString.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' SendMidiString.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _dev As String
        Friend _str As String

        <TypeConverter(GetType(IServices.OutDeviceList)), _
            DisplayName("Target Device"), _
            Description("Which MIDI output device to send to."), _
            DefaultValue("ALL DEVICES")> _
        Public Property Device() As String
            Get
                Return _dev
            End Get
            Set(ByVal value As String)
                _dev = value
            End Set
        End Property

        <DisplayName("MIDI String"), _
            Description("The MIDI command to send, in hexadecimal string format." & vbCrLf & vbCrLf & "Example: '9F 00 01' is Note 0 Off, Channel 16, Velocity 1"), _
            DefaultValue("")> _
        Public Property str() As String
            Get
                Return _str
            End Get
            Set(ByVal value As String)
                _str = value
            End Set
        End Property

        Public Sub New()
            _dev = "ALL DEVICES"
            _str = ""
        End Sub
    End Class
End Class