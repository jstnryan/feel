Imports System.Runtime.InteropServices

<Guid("15ABCE5A-D91E-4d65-946A-58394B9FE474")> _
Public Class CloseWindow
    Implements Feel.ActionInterface.iAction

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.iAction.Description
        Get
            Return "Closes the currently highlighted (active) window."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.iAction.Execute

    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.iAction.Group
        Get
            Return "External Functions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As Feel.ActionInterface.iServices) Implements Feel.ActionInterface.iAction.Initialize

    End Sub

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.iAction.Name
        Get
            Return "Close Window"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.iAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946A-58394B9FE474")
        End Get
    End Property
End Class
