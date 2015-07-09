Public Class ActionInterface
#Region "Interfaces"
    Public Interface IAction
        ReadOnly Property UniqueID() As Guid
        ReadOnly Property Group() As String
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
        Property Data() As Object
        ''Type: 128=NoteOff, 144=NoteOn, 176=Control Change
        Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean
        Sub Initialize(ByRef Host As IServices)
        'MustInherit Class ActionData : End Class
    End Interface

    Public MustInherit Class IServices
        ''Feel Information
        Overridable ReadOnly Property ServicesVersion() As Integer
            Get
                Return 1
            End Get
        End Property
        MustOverride Sub ConfigureConnections()
        MustOverride Sub ConfigureActions()
        MustOverride Sub SetPage(ByVal Device As String, ByVal Page As Byte)
        MustOverride Sub SetPage(ByVal Device As Integer, ByVal Page As Byte)
        MustOverride Sub RedrawControls(ByVal Device As Integer)
        MustOverride Sub RedrawControls(Optional ByVal Device As String = "ALL DEVICES")

        ''Overridable Function ActionDataSerialize(ByRef ActionData As Object) As String
        ''    Dim _serializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(ActionData.GetType)
        ''    Dim _stream As IO.StringWriter = New IO.StringWriter
        ''    _serializer.Serialize(_stream, ActionData)
        ''    Return _stream.ToString
        ''End Function
        ''Overridable Function ActionDataDeserialize(ByRef ActionData As String, ByRef DataType As System.Type) As Object
        ''    Dim _serializer As Xml.Serialization.XmlSerializer = New Xml.Serialization.XmlSerializer(DataType)
        ''    Dim _reader As System.IO.StringReader = New IO.StringReader(ActionData)
        ''    Return _serializer.Deserialize(_reader)
        ''End Function
        'Overridable Function ActionDataSerialize(ByRef ActionData As Object) As Object
        '    Using st As New IO.MemoryStream
        '        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
        '        bf.Serialize(st, ActionData)
        '        'st = Nothing
        '        bf = Nothing
        '        Return st
        '    End Using
        'End Function
        'Overridable Function ActionDataDeserialize(ByRef ActionData As Object, ByVal DataType As System.Type) As Object
        '    Using st As IO.MemoryStream = New IO.MemoryStream(CType(ActionData, Byte())) 'TODO: not sure about this...
        '        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
        '        Return bf.Deserialize(st)
        '        'st.Close()
        '        'st = Nothing
        '        bf = Nothing
        '    End Using
        'End Function

        MustOverride Function FindDeviceIndex(ByVal Device As String) As Integer
        MustOverride Function FindDeviceIndexByName(ByVal Device As String) As Integer
        MustOverride Function FindDeviceIndexByInput(ByVal Device As String) As Integer
        MustOverride Function FindDeviceIndexByOutput(ByVal Device As String) As Integer
        MustOverride Function ConnectionExists(ByVal Connection As Integer) As Boolean
        MustOverride Function ControlExists(ByVal Connection As Integer, ByVal Control As String) As Boolean
        MustOverride Function PageExists(ByVal Connection As Integer, ByVal Control As String, ByVal Page As Byte) As Boolean
        MustOverride Property CurrentPage(ByVal Connection As Integer) As Byte
        MustOverride Property CurrentState(ByVal Connection As Integer, ByVal Control As String, Optional ByVal Page As Byte = 0) As String
        MustOverride Sub ResetControlsByGroup(ByVal Group As Byte)

        ''LightJockey Information
        MustOverride Function GetLJHandle() As IntPtr
        MustOverride Function GetCurrentSequence() As Integer
        MustOverride Function GetCurrentCue() As Integer
        MustOverride Function GetCurrentCueList() As Integer
        MustOverride Function GetCurrentBGCue() As Integer

        ''MIDI Information
        MustOverride Function GetMIDIDeviceOUTList() As String()
        MustOverride Function GetMIDIDeviceINList() As String()
        ''MIDI Functions
        MustOverride Sub SendMIDI(ByVal Device As Integer, ByVal Message As String)
        MustOverride Sub SendMIDI(ByVal Device As Integer, ByVal Message As String())
        MustOverride Sub SendMIDI(ByVal Device As String, ByVal Message As String)
        MustOverride Sub SendMIDI(ByVal Device As String, ByVal Message As String())

        ''Windows Message Functions
        MustOverride Function SendLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        MustOverride Function PostLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        MustOverride Function SendWMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        MustOverride Function PostWMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer

#Region "PropertyGrid Helpers"
        Public MustInherit Class DeviceList
        End Class

        Public Class OutDeviceList
            Inherits ComponentModel.StringConverter

            Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ComponentModel.ITypeDescriptorContext) As Boolean
                Return True
            End Function

            Public Overloads Overrides Function GetStandardValues(ByVal context As ComponentModel.ITypeDescriptorContext) As StandardValuesCollection
                Dim devArr As String() = New String() {"THIS SHOULD BE OBVIOUS"}
                For Each device As Integer In FeelConfig.Connections.Keys
                    If FeelConfig.Connections(device).OutputEnable Then devArr.Add(FeelConfig.Connections(device).Name)
                Next
                Return New StandardValuesCollection(devArr)
            End Function
        End Class
#End Region
    End Class

    'Public Interface IServices
    '    ''Feel Information
    '    'ReadOnly Property Version() As String
    '    Sub ConfigureConnections()
    '    Sub ConfigureActions()

    '    ''LightJockey Information
    '    Function GetLJHandle() As IntPtr
    '    Function GetCurrentSequence() As Integer
    '    Function GetCurrentCue() As Integer
    '    Function GetCurrentCueList() As Integer

    '    ''MIDI Information
    '    Function GetMIDIDeviceOUTList() As String()
    '    Function GetMIDIDeviceINList() As String()
    '    ''MIDI Functions
    '    Sub SendMIDI(ByVal Device As String, ByVal Message As String)

    '    ''Windows Message Functions
    '    Function SendMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    '    Function SendMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    '    Function PostMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    '    Function PostMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    '    Function SendLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    'End Interface
#End Region
End Class