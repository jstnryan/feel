Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

''' <summary>
''' Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
''' Provides a method for performing a deep copy of an object.
''' Binary Serialization is used to perform the copy.
''' </summary>
Public NotInheritable Class ObjectCopier
    Private Sub New()
    End Sub
    ''' <summary>
    ''' Perform a deep Copy of the object.
    ''' </summary>
    ''' <typeparam name="T">The type of object being copied.</typeparam>
    ''' <param name="source">The object instance to copy.</param>
    ''' <returns>The copied object.</returns>
    Public Shared Function Clone(Of T)(ByVal source As T) As T
        ' Don't serialize a null object, simply return the default for that object
        If [Object].ReferenceEquals(source, Nothing) Then
            Return Nothing
        End If

        If Not GetType(T).IsSerializable Then
            Throw New ArgumentException("The type must be serializable.", "source")
        End If

        Dim formatter As IFormatter = New BinaryFormatter()
        Dim stream As Stream = New MemoryStream()
        Using stream
            formatter.Serialize(stream, source)
            stream.Seek(0, SeekOrigin.Begin)
            Try
                formatter.Binder = New DeserializationBinder() ''See DeserializationBinder
                Return DirectCast(formatter.Deserialize(stream), T)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine(ex.ToString)
            End Try
        End Using
    End Function
End Class

''' <summary>
''' A fix for the Clone method of ObjectCopier class throwing "Assembly not found" exceptions
''' </summary>
''' <remarks>http://stackoverflow.com/questions/24673376/unable-to-find-assembly-with-binaryformatter-deserialize
'''  and: http://social.msdn.microsoft.com/Forums/vstudio/en-US/e5f0c371-b900-41d8-9a5b-1052739f2521/deserialize-unable-to-find-an-assembly-?forum=netfxbcl</remarks>
Public Class DeserializationBinder
    Inherits SerializationBinder
    Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As Type
        Dim tyType As Type = Nothing

        Dim sShortAssemblyName As String = assemblyName.Split(","c)(0)
        Dim ayAssemblies As Reflection.Assembly() = AppDomain.CurrentDomain.GetAssemblies()

        For Each ayAssembly As Reflection.Assembly In ayAssemblies
            If sShortAssemblyName = ayAssembly.FullName.Split(","c)(0) Then
                tyType = ayAssembly.[GetType](typeName)
                Exit For
            End If
        Next

        Return tyType
    End Function
End Class