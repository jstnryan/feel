Imports System.ComponentModel

Public Class ActionInterface
#Region "Constants & Structures"
    Public Const WM_USER As Int32 = &H400 ''1024
    Public Const WM_COPYDATA As Integer = &H4A ''74
    Public Structure CopyData
        Public dwData As Integer
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure
#End Region

#Region "Interfaces"
    ''' <summary>
    ''' Interface utilized by Action plugins
    ''' </summary>
    Public Interface IAction
        ReadOnly Property UniqueID() As Guid
        ReadOnly Property Group() As String
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
        Property Data() As Object
        ''Type: 128=NoteOff, 144=NoteOn, 176=Control Change
        Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean
        Function Initialize(ByRef Host As IServices) As Boolean
        'MustInherit Class ActionData : End Class
    End Interface


    ''' <summary>
    ''' Interface implemented by Feel, utilized by Action plugins to perform actions
    ''' </summary>
    Public MustInherit Class IServices
        ''Feel Information
        Overridable ReadOnly Property ServicesVersion() As Integer
            Get
                Return 1
            End Get
        End Property
        MustOverride Sub OpenWindowConfig()
        MustOverride Sub OpenWindowConnections()
        MustOverride Sub OpenWindowActions()
        MustOverride Sub SaveConfiguration()
        MustOverride Sub ExitProgram(Optional ByVal Restart As Boolean = False)
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
        MustOverride Function GetMIDIDeviceList() As String()
        MustOverride Function GetMIDIDeviceINList() As String()
        MustOverride Function GetMIDIDeviceOUTList() As String()
        ''MIDI Functions
        MustOverride Sub SendMIDI(ByVal Device As Integer, ByVal Message As String)
        Overridable Sub SendMIDI(ByVal Device As Integer, ByVal Message As String())
            For Each _msg As String In Message
                SendMIDI(Device, _msg)
            Next
        End Sub
        MustOverride Sub SendMIDI(ByVal Device As String, ByVal Message As String)
        Overridable Sub SendMIDI(ByVal Device As String, ByVal Message As String())
            For Each _msg As String In Message
                SendMIDI(Device, _msg)
            Next
        End Sub
        Overridable Sub SendMIDI(ByVal Device As String, ByVal ControlType As Byte, ByVal Channel As Byte, ByVal NotCon As Byte, ByVal VelVal As Byte)
            If (Device = "ALL DEVICES") Then
                SendMIDI(Device, (ControlType + Channel).ToString("X2") & NotCon.ToString("X2") & VelVal.ToString("X2"))
            Else
                SendMIDI(FindDeviceIndex(Device), (ControlType + Channel).ToString("X2") & NotCon.ToString("X2") & VelVal.ToString("X2"))
            End If
        End Sub
        Overridable Sub SendMIDI(ByVal Device As Integer, ByVal ControlType As Byte, ByVal Channel As Byte, ByVal Notcon As Byte, ByVal VelVal As Byte)
            SendMIDI(Device, (ControlType + Channel).ToString("X2") & Notcon.ToString("X2") & VelVal.ToString("X2"))
        End Sub

        ''Windows Message Functions
        MustOverride Sub ResetLJWindowHandle()
        MustOverride Function SendLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        MustOverride Function PostLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        MustOverride Function SendWMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        MustOverride Function PostWMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
        MustOverride Function SendLJCopyData(ByVal lParam As ActionInterface.CopyData) As Integer

        ''Software protection functions (licensing)
        Overridable Function Licensing_CodeMeter_Validate(ByVal firmCode As UInteger, Optional ByVal productCode As UInteger = Nothing, Optional ByVal featureCode As UInteger = Nothing) As Boolean
            Return False
        End Function
    End Class
#End Region
End Class

''' <summary>
''' EnumConverter supporting System.ComponentModel.DescriptionAttribute
''' </summary>
''' <remarks>http://www.codeproject.com/Articles/6294/Description-Enum-TypeConverter</remarks>
Public Class EnumDescriptionConverter
    Inherits ComponentModel.EnumConverter
    Protected myVal As System.Type

    ''' <summary>
    ''' Gets Enum Value's Description Attribute
    ''' </summary>
    ''' <param name="value">The value you want the description attribute for</param>
    ''' <returns>The description, if any, else it's .ToString()</returns>
    Public Shared Function GetEnumDescription(ByVal value As [Enum]) As String
        Dim fi As Reflection.FieldInfo = value.[GetType]().GetField(value.ToString())
        Dim attributes As DescriptionAttribute() = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
        Return If((attributes.Length > 0), attributes(0).Description, value.ToString())
    End Function

    ''' <summary>
    ''' Gets the description for certaing named value in an Enumeration
    ''' </summary>
    ''' <param name="value">The type of the Enumeration</param>
    ''' <param name="name">The name of the Enumeration value</param>
    ''' <returns>The description, if any, else the passed name</returns>
    Public Shared Function GetEnumDescription(ByVal value As System.Type, ByVal name As String) As String
        Dim fi As Reflection.FieldInfo = value.GetField(name)
        Dim attributes As DescriptionAttribute() = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
        Return If((attributes.Length > 0), attributes(0).Description, name)
    End Function

    ''' <summary>
    ''' Gets the value of an Enum, based on it's Description Attribute or named value
    ''' </summary>
    ''' <param name="value">The Enum type</param>
    ''' <param name="description">The description or name of the element</param>
    ''' <returns>The value, or the passed in description, if it was not found</returns>
    Public Shared Function GetEnumValue(ByVal value As System.Type, ByVal description As String) As Object
        Dim fis As Reflection.FieldInfo() = value.GetFields()
        For Each fi As Reflection.FieldInfo In fis
            Dim attributes As DescriptionAttribute() = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            If attributes.Length > 0 Then
                If attributes(0).Description = description Then
                    Return fi.GetValue(fi.Name)
                End If
            End If
            If fi.Name = description Then
                Return fi.GetValue(fi.Name)
            End If
        Next
        Return description
    End Function

    Public Sub New(ByVal type As System.Type)
        MyBase.New(type.[GetType]())
        myVal = type
    End Sub

    Public Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As Type) As Object
        If TypeOf value Is [Enum] AndAlso (destinationType Is GetType(String)) Then
            Return GetEnumDescription(DirectCast(value, [Enum]))
        End If
        If TypeOf value Is String AndAlso (destinationType Is GetType(String)) Then
            Return GetEnumDescription(myVal, DirectCast(value, String))
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

    Public Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object
        If TypeOf value Is String Then
            Return GetEnumValue(myVal, DirectCast(value, String))
        End If
        If TypeOf value Is [Enum] Then
            Return GetEnumDescription(DirectCast(value, [Enum]))
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    ''The following two Overrides added by Bernd Wißler: http://www.codeproject.com/Articles/6294/Description-Enum-TypeConverter?msg=1802196#xx1802196xx
    Public Overrides Function GetPropertiesSupported(ByVal context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As TypeConverter.StandardValuesCollection
        Dim values As New ArrayList()
        Dim fis As Reflection.FieldInfo() = myVal.GetFields()
        For Each fi As Reflection.FieldInfo In fis
            Dim attributes As DescriptionAttribute() = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            If attributes.Length > 0 Then
                values.Add(fi.GetValue(fi.Name))
            End If
        Next
        Return New TypeConverter.StandardValuesCollection(values)
    End Function
End Class