Module ExtensionMethods
    ''' <summary>Add an item (T) to end of array (of T).</summary>
    <Diagnostics.DebuggerStepThrough(), _
        System.Runtime.CompilerServices.Extension()> _
    Public Sub Add(Of T)(ByRef arr As T(), ByVal item As T)
        Array.Resize(arr, arr.Length + 1)
        arr(arr.Length - 1) = item
    End Sub

    ''' <summary>Extends [String].IsNullOrEmpty, and checks for strings containing only white space.</summary>
    <Diagnostics.DebuggerStepThrough(), _
        System.Runtime.CompilerServices.Extension()> _
    Public Function IsNullOrEmpty(ByVal str As String) As Boolean
        If Not ([String].IsNullOrEmpty(str)) Then
            For i As Integer = 0 To str.Length - 1
                If Not [Char].IsWhiteSpace(str(i)) Then Return False
            Next
        End If
        Return True
    End Function
End Module
