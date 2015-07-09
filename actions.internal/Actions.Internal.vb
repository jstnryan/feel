Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE473")> _
Public Class ConfigureActions
    Implements IAction

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
            Return "Opens the 'Configure Actions' window."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Diagnostics.Debug.WriteLine("ConfigureActions")
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Actions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        Return
    End Sub

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Configure Actions"
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

    Private _myData As ChangePageData

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Triggers a page change on one, or all, control surface(s) to a specific page." & vbCrLf & vbCrLf & "• To change multiple controllers to different pages, use multiple actions." & vbCrLf & vbCrLf & "• To change page by an increment, use 'Skip Pages' instead." & vbCrLf & vbCrLf & "It is HIGHLY RECOMMENDED to disable the 'Paged Control' option when defining page change actions."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        System.Diagnostics.Debug.WriteLine("ChangePage Execute: " & _myData.Page)

        'SetPage(_myData.Device, _myData.Page)
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _myData = New ChangePageData
    End Sub

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
            _myData = DirectCast(value, ChangePageData)
        End Set
    End Property

    <Serializable()> _
    Public Class ChangePageData
        Private _device As String
        Private _page As Byte

        <TypeConverter(GetType(DeviceList)), _
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
End Class