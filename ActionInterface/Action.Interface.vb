#Region "PropertyGrid Helpers"
Public Class DeviceList
    Inherits ComponentModel.StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
        Dim devArr As Collections.Generic.List(Of String) = New Collections.Generic.List(Of String)
        devArr.Add("ALL DEVICES")

        'TODO:
        'For Each device As Integer In Configuration.Connections.Keys
        '   devArr.Add(Configuration.Connections(device).Name)
        'Next

        Return New StandardValuesCollection(devArr.ToArray)
        'Return New StandardValuesCollection(New String() {"ALL DEVICES", "Device 1", "Device 2", "Device 3"})
    End Function
End Class

Public Class OutDeviceList
    Inherits ComponentModel.StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
        Dim devArr As Collections.Generic.List(Of String) = New Collections.Generic.List(Of String)
        devArr.Add("ALL DEVICES")
        'TODO:
        'For Each device As Integer In Configuration.Connections.Keys
        '    If Configuration.Connections(device).OutputEnable Then devArr.Add(Configuration.Connections(device).Name)
        'Next
        Return New StandardValuesCollection(devArr.ToArray)
    End Function
End Class
#End Region

Public Class ActionInterface
    Private m_addonInstances As New List(Of IAction)

    Private Sub LoadAdditionalModules()
        Dim currentApplicationDirectory As String = My.Application.Info.DirectoryPath
        Dim addonsRootDirectory As String = currentApplicationDirectory & "\actions\"
        Dim addonsLoaded As New List(Of System.Type)

        If My.Computer.FileSystem.DirectoryExists(addonsRootDirectory) Then
            Dim dllFilesFound As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(addonsRootDirectory, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*.dll")
            For Each dllFile As String In dllFilesFound
                Dim modulesFound As System.Collections.Generic.List(Of System.Type) = TryLoadAssemblyReference(dllFile)
                addonsLoaded.AddRange(modulesFound)
            Next
        End If

        If addonsLoaded.Count > 0 Then
            For Each addonToInstantiate As System.Type In addonsLoaded
                Dim thisInstance As IAction = CType(Activator.CreateInstance(addonToInstantiate), IAction)
                'TODO: need this??
                'Dim thisTypedInstance As iAction = CType(thisInstance, iAction)
                'thisTypedInstance.Initialise()
                m_addonInstances.Add(thisInstance)
            Next
        End If
    End Sub

    Private Function TryLoadAssemblyReference(ByVal dllFilePath As String) As List(Of System.Type)
        Dim loadedAssembly As Reflection.Assembly = Nothing
        Dim listOfModules As New List(Of System.Type)
        Try
            loadedAssembly = Reflection.Assembly.LoadFile(dllFilePath)
        Catch ex As Exception
            Diagnostics.Debug.WriteLine("Reflection.Assembly exception")
        End Try
        If loadedAssembly IsNot Nothing Then
            For Each assemblyModule As System.Reflection.Module In loadedAssembly.GetModules
                For Each moduleType As System.Type In assemblyModule.GetTypes()
                    For Each interfaceImplemented As System.Type In moduleType.GetInterfaces()
                        Diagnostics.Debug.WriteLine("moduletype.Name: " & moduleType.Name & " | interfaceImplemented.Name: " & interfaceImplemented.Name & " | interfaceImplemented.FullName: " & interfaceImplemented.FullName)
                        'If interfaceImplemented.FullName = "ActionInterface.iAction" Then
                        If (interfaceImplemented.Name = "IAction") Then
                            listOfModules.Add(moduleType)
                        End If
                    Next
                Next
            Next
        End If
        Diagnostics.Debug.WriteLine("loadedmodules.count: " & listOfModules.Count.ToString)
        Return listOfModules
    End Function

    Public Sub New()
        LoadAdditionalModules()
    End Sub

    Public Function GetThingy() As String
        Dim thing As String = ""
        For Each inst As IAction In m_addonInstances
            thing &= inst.Name.ToString & vbCrLf
        Next
        Return thing
    End Function

#Region "Interfaces"
    Public Interface IAction
        ReadOnly Property UniqueID() As Guid
        ReadOnly Property Group() As String
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
        Property Data() As Object
        'Type: 128=NoteOff, 144=NoteOn, 176=Control Change
        Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean
        Sub Initialize(ByRef Host As IServices)
    End Interface

    Public Interface IServices
        ''Feel Information
        'ReadOnly Property Version() As String

        ''LightJockey Information
        Function GetLJHandle() As IntPtr
        Function GetCurrentSequence() As Integer
        Function GetCurrentCue() As Integer
        Function GetCurrentCueList() As Integer

        ''MIDI Information
        Function GetMIDIDeviceOUTList() As String()
        Function GetMIDIDeviceINList() As String()
        ''MIDI Functions
        Sub SendMIDI(ByVal Device As String, ByVal Message As String)

        ''Windows Message Functions
        Function SendMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        Function SendMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
        Function PostMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        Function PostMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
        Function SendLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Interface
#End Region
End Class