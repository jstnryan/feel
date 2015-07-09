Public Class Interfaces
    'Friend m_addonInstances As New List(Of ActionInterface.IAction)

    'Friend Sub LoadModules()
    '    Dim currentApplicationDirectory As String = My.Application.Info.DirectoryPath
    '    Dim addonsRootDirectory As String = currentApplicationDirectory & "\actions\"
    '    Dim addonsLoaded As New List(Of System.Type)

    '    If My.Computer.FileSystem.DirectoryExists(addonsRootDirectory) Then
    '        Dim dllFilesFound As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Computer.FileSystem.GetFiles(addonsRootDirectory, Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories, "*.dll")
    '        For Each dllFile As String In dllFilesFound
    '            Dim modulesFound As System.Collections.Generic.List(Of System.Type) = TryLoadAssemblyReference(dllFile)
    '            addonsLoaded.AddRange(modulesFound)
    '        Next
    '    End If

    '    If addonsLoaded.Count > 0 Then
    '        For Each addonToInstantiate As System.Type In addonsLoaded
    '            Dim thisInstance As ActionInterface.IAction = CType(Activator.CreateInstance(addonToInstantiate), ActionInterface.IAction)
    '            'TODO: need this??
    '            'Dim thisTypedInstance As ActionInterface.IAction = CType(thisInstance, ActionInterface.IAction)
    '            'thisTypedInstance.Initialise()
    '            m_addonInstances.Add(thisInstance)
    '        Next
    '    End If
    'End Sub

    'Private Function TryLoadAssemblyReference(ByVal dllFilePath As String) As List(Of System.Type)
    '    Dim loadedAssembly As Reflection.Assembly = Nothing
    '    Dim listOfModules As New List(Of System.Type)
    '    Try
    '        loadedAssembly = Reflection.Assembly.LoadFile(dllFilePath)
    '    Catch ex As Exception
    '        Diagnostics.Debug.WriteLine("Reflection.Assembly exception")
    '    End Try
    '    If loadedAssembly IsNot Nothing Then
    '        For Each assemblyModule As System.Reflection.Module In loadedAssembly.GetModules
    '            For Each moduleType As System.Type In assemblyModule.GetTypes()
    '                For Each interfaceImplemented As System.Type In moduleType.GetInterfaces()
    '                    'Diagnostics.Debug.WriteLine("moduletype.Name: " & moduleType.Name & " | interfaceImplemented.Name: " & interfaceImplemented.Name & " | interfaceImplemented.FullName: " & interfaceImplemented.FullName)
    '                    'If interfaceImplemented.FullName = "ActionInterface.iAction" Then
    '                    If (interfaceImplemented.Name = "IAction") Then
    '                        listOfModules.Add(moduleType)
    '                    End If
    '                Next
    '            Next
    '        Next
    '    End If
    '    'Diagnostics.Debug.WriteLine("loadedmodules.count: " & listOfModules.Count.ToString)
    '    Return listOfModules
    'End Function

    ''Public Sub New()
    ''    LoadAdditionalModules()
    ''End Sub

    ''Public Function GetThingy() As String
    ''    Dim thing As String = ""
    ''    For Each inst As iAction In m_addonInstances
    ''        thing &= inst.Name.ToString & vbCrLf
    ''    Next
    ''    Return thing
    ''End Function

    Friend Class ServiceHost
        Inherits ActionInterface.IServices

        ''Feel Functions:
        Public Overrides Sub ConfigureActions()
            main.OpenActionWindow()
        End Sub

        Public Overrides Sub ConfigureConnections()
            main.OpenConfigWindow()
        End Sub

        Public Overrides Sub SetPage(ByVal Device As String, ByVal Page As Byte)
            main.SetPage(Device, Page)
        End Sub

        ''LightJockey Information:
        Public Overrides Function GetCurrentCue() As Integer
            Return main.GetCue
        End Function

        Public Overrides Function GetCurrentCueList() As Integer
            Return main.GetCueList
        End Function

        Public Overrides Function GetCurrentSequence() As Integer
            Return main.GetSequence
        End Function

        Public Overrides Function GetCurrentBGCue() As Integer
            Return main.GetBackgroundCue
        End Function

        Public Overrides Function GetLJHandle() As System.IntPtr
            Return main.GetHandle
        End Function

        ''MIDI:
        Public Overrides Function GetMIDIDeviceINList() As String()
            Dim devArr As String() = New String() {"ALL DEVICES"}
            For Each device As Integer In FeelConfig.Connections.Keys
                devArr.Add(FeelConfig.Connections(device).Name)
            Next
            Return devArr
        End Function

        Public Overrides Function GetMIDIDeviceOUTList() As String()
            Dim devArr As String() = New String() {"ALL DEVICES"}
            For Each device As Integer In FeelConfig.Connections.Keys
                If FeelConfig.Connections(device).OutputEnable Then devArr.Add(FeelConfig.Connections(device).Name)
            Next
            Return devArr
        End Function

        Public Overrides Sub SendMIDI(ByVal Device As String, ByVal Message As String)
            main.SendMidi(Device, Message)
        End Sub

        ''Windows Messages:
        Public Overrides Function PostLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Return main.PostMessage(uMsg, wParam, lParam)
        End Function

        Public Overrides Function SendLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Return main.SendMessage(uMsg, wParam, lParam)
        End Function

        Public Overloads Overrides Function PostWMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Return 0
        End Function

        Public Overloads Overrides Function SendWMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
            Return 0
        End Function

        ''DEVICE HELPERS:
        Public Shadows Class DeviceList
            Inherits ComponentModel.StringConverter

            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
                'Dim devArr As Collections.Generic.List(Of String) = New Collections.Generic.List(Of String)
                'devArr.Add("ALL DEVICES")

                ''TODO:
                ''For Each device As Integer In Configuration.Connections.Keys
                ''   devArr.Add(Configuration.Connections(device).Name)
                ''Next

                'Return New StandardValuesCollection(devArr.ToArray)
                ''Return New StandardValuesCollection(New String() {"ALL DEVICES", "Device 1", "Device 2", "Device 3"})

                Dim devArr As String() = New String() {"ALL DEVICES"}
                For Each device As Integer In FeelConfig.Connections.Keys
                    devArr.Add(FeelConfig.Connections(device).Name)
                Next
                Return New StandardValuesCollection(devArr)
            End Function
        End Class

        Public Shadows Class OutDeviceList
            Inherits ComponentModel.StringConverter

            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
                'Dim devArr As Collections.Generic.List(Of String) = New Collections.Generic.List(Of String)
                'devArr.Add("ALL DEVICES")
                ''TODO:
                ''For Each device As Integer In Configuration.Connections.Keys
                ''    If Configuration.Connections(device).OutputEnable Then devArr.Add(Configuration.Connections(device).Name)
                ''Next
                'Return New StandardValuesCollection(devArr.ToArray)

                Dim devArr As String() = New String() {"ALL DEVICES"}
                For Each device As Integer In FeelConfig.Connections.Keys
                    If FeelConfig.Connections(device).OutputEnable Then devArr.Add(FeelConfig.Connections(device).Name)
                Next
                Return New StandardValuesCollection(devArr)
            End Function
        End Class
    End Class
End Class
