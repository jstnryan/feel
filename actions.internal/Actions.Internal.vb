Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE472")> _
Public Class ConfigureConnections
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
            Return "Opens the 'Configure Connections' window." & vbCrLf & vbCrLf & "This action has no editable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.ConfigureConnections()
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _host = Host
        'Return
    End Sub

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
            Return "Opens the 'Configure Actions' window." & vbCrLf & vbCrLf & "This action has no editable properties."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Diagnostics.Debug.WriteLine("ConfigureActions:before")
        _host.ConfigureActions()
        Diagnostics.Debug.WriteLine("ConfigureActions:after")
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "Internal Functions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _host = Host
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

    Private _myData As ActionData
    Private _host As IServices

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

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
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
            _myData = DirectCast(value, ActionData)
        End Set
    End Property

    <Serializable()> _
    Public Class ActionData
        Private _device As String
        Private _page As Byte

        <TypeConverter(GetType(IServices.DeviceList)), _
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

        'Public Function ShallowClone() As ChangePageData
        '    Return DirectCast(Me.MemberwiseClone(), ChangePageData)
        'End Function

        'Public Function DeepClone() As ChangePageData
        '    Dim newClone As ChangePageData = DirectCast(Me.MemberwiseClone(), ChangePageData)
        '    newClone._device = String.Copy(_device)
        '    Return newClone
        'End Function
    End Class
End Class

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE475")> _
Public Class ShowMessageBox
    Implements ActionInterface.IAction

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

        Public Function Clone() As ShowMessageBox.ActionData
            Return DirectCast(Me.MemberwiseClone(), ShowMessageBox.ActionData)
        End Function
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

    Public Sub Initialize(ByRef Host As ActionInterface.IServices) Implements ActionInterface.IAction.Initialize
        _host = Host
        _actionData = New ActionData
    End Sub

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