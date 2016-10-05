Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("41D5F122-CFF8-44BC-AE2F-B085E4B5C9CC")>
Public Class FaderControl
    Implements IAction

    Private Shared _host As IServices
    Private _actionData As FaderControl.ActionData

    ''' <summary>
    ''' FaderControl.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()>
    Public Class ActionData
        Friend _dev As String
        Friend _page As Integer
        Friend _fader As Integer
        Friend _invert As Boolean

        <TypeConverter(GetType(FaderControl.OutDeviceList)),
            DisplayName("Target Device"),
            Description("Which MIDI output device to send to."),
            DefaultValue("ALL DEVICES")>
        Public Property Device() As String
            Get
                Return _dev
            End Get
            Set(ByVal value As String)
                _dev = value
            End Set
        End Property

        <DisplayName("Page"),
            Description("The page number of the executor fader to control.'"),
            DefaultValue(1)>
        Public Property Page() As Integer
            Get
                Return _page
            End Get
            Set(ByVal value As Integer)
                _page = If(value < 1, 1, value)
            End Set
        End Property

        <DisplayName("Fader"),
            Description("The number of the executor fader to control.'"),
            DefaultValue(1)>
        Public Property Fader() As Integer
            Get
                Return _fader
            End Get
            Set(ByVal value As Integer)
                _fader = If(value < 1, 1, value)
            End Set
        End Property

        <DisplayName("Invert"),
            Description("Inverts the value of the input control.'"),
            DefaultValue(False)>
        Public Property Invert() As Boolean
            Get
                Return _invert
            End Get
            Set(ByVal value As Boolean)
                _invert = value
            End Set
        End Property

        Public Sub New()
            _dev = "ALL DEVICES"
            _page = 1
            _fader = 1
            _invert = False
        End Sub
    End Class

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _actionData
        End Get
        Set(ByVal value As Object)
            _actionData = CType(value, FaderControl.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Allows translation of MIDI CC control to Executor Fader value." & vbCrLf & vbCrLf & "Supports 7-bit (standard resolution) input fader values."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim value As Double = (VelVal / 127) * 100
        If _actionData._invert Then value = 100 - value
        value = value * 1.28
        Dim coarse As Integer = CInt(Fix(value))
        Dim fine As Integer = CInt(Fix((value - coarse) * 128))
        'Diagnostics.Debug.WriteLine("VelVal: " & VelVal & ", Value: " & value & ", Coarse: " & coarse.ToString & "(" & coarse.ToString("X2") & "), Fine: " & fine.ToString & "(" & fine.ToString("X2") & ")")
        _host.SendMIDI(_actionData._dev, "F0 7F 7F 02 7F 06 " & (_actionData._fader - 1).ToString("X2") & " " & _actionData._page.ToString("X2") & " " & fine.ToString("X2") & " " & coarse.ToString("X2") & " F7")
        Debug.WriteLine("Page: " & _actionData._page.ToString & "(" & _actionData._page.ToString("X2") & "), " & "Fader: " & _actionData._fader.ToString & "(" & _actionData._fader.ToString("X2") & "), HEX: " & (_actionData._fader - 1).ToString("X2") & " " & _actionData._page.ToString("X2"))
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Grand MA2 Show Control"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _host = Host
        _actionData = New FaderControl.ActionData
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Executor Fader Control"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As Guid Implements IAction.UniqueID
        Get
            Return New Guid("41D5F122-CFF8-44BC-AE2F-B085E4B5C9CC")
        End Get
    End Property

    Public Class OutDeviceList
        Inherits ComponentModel.StringConverter

        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
            Dim devArr As String() = New String() {}
            For Each device As String In _host.GetMIDIDeviceOUTList
                Array.Resize(devArr, devArr.Length + 1)
                devArr(devArr.Length - 1) = device
            Next
            Return New StandardValuesCollection(devArr)
        End Function
    End Class
End Class