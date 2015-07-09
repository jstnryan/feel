Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("15ABCE5A-D91E-4d65-946B-58394B9FE400")> _
Public Class SendMidiString
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Sends a MIDI message (Note On, Note Off, CC, ..) formatted as a hexadecimal string." & vbCrLf & vbCrLf & "To explicitly set each MIDI component (Channel, Note, Velocity, etc..) use the 'Send MIDI (Components)' action instead."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        If (_myData._dev = "ALL DEVICES") Then
            _host.SendMIDI(_myData._dev, _myData._str)
            Return True
        Else
            Dim _device As Integer = _host.FindDeviceIndexByName(_myData._dev)
            If Not (_device = -1) Then
                _host.SendMIDI(_device, _myData._str)
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "MIDI Functions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
    End Sub

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Send MIDI (String)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946B-58394B9FE400")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, SendMidiString.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' SendMidiString.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _dev As String
        Friend _str As String

        <TypeConverter(GetType(SendMidiString.OutDeviceList)), _
            DisplayName("Target Device"), _
            Description("Which MIDI output device to send to."), _
            DefaultValue("ALL DEVICES")> _
        Public Property Device() As String
            Get
                Return _dev
            End Get
            Set(ByVal value As String)
                _dev = value
            End Set
        End Property

        <DisplayName("MIDI String"), _
            Description("The MIDI command to send, in hexadecimal string format." & vbCrLf & vbCrLf & "Example: '9F 00 01' is Note 0 Off, Channel 16, Velocity 1"), _
            DefaultValue("")> _
        Public Property str() As String
            Get
                Return _str
            End Get
            Set(ByVal value As String)
                _str = value
            End Set
        End Property

        Public Sub New()
            _dev = "ALL DEVICES"
            _str = ""
        End Sub
    End Class

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

<Guid("15ABCE5A-D91E-4d65-946B-58394B9FE401")> _
Public Class SendMidiComponents
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Sends a MIDI message (Note On, Note Off, CC, ..) formatted built from individual message components." & vbCrLf & vbCrLf & "To send a message based on a hexadecimal encoded string, use the 'Send MIDI (String)' action instead."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        'If (_myData._dev = "ALL DEVICES") Then
        '    _host.SendMIDI(_myData._dev, _myData._type, _myData._chan, _myData._note, _myData._vel)
        '    Return True
        'Else
        '    Dim _device As Integer = _host.FindDeviceIndexByName(_myData._dev)
        '    If Not (_device = -1) Then
        _host.SendMIDI(_myData._dev, If(_myData._type = MessageType.Passthrough, Type, _myData._type), If(_myData._chan = MidiChannel.Passthrough, Channel, _myData._chan), If(_myData._note = Pitch.Passthrough, NoteCon, _myData._note), If(_myData._vel = 255, VelVal, _myData._vel))
        Return True
        '    Else
        '        Return False
        '    End If
        'End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "MIDI Functions"
        End Get
    End Property

    Public Sub Initialize(ByRef Host As IServices) Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
    End Sub

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Send MIDI (Components)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("15ABCE5A-D91E-4d65-946B-58394B9FE401")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, SendMidiComponents.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' SendMidiString.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _dev As String
        Friend _type As MessageType
        Friend _chan As MidiChannel
        Friend _note As Pitch
        Friend _vel As Byte

        <TypeConverter(GetType(SendMidiString.OutDeviceList)), _
            DisplayName("Target Device"), _
            Description("Which MIDI output device to send to."), _
            DefaultValue("ALL DEVICES")> _
        Public Property Device() As String
            Get
                Return _dev
            End Get
            Set(ByVal value As String)
                _dev = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Message Type"), _
            Description("The MIDI message type."), _
            DefaultValue(MessageType.Passthrough)> _
        Public Property Type() As MessageType
            Get
                Return _type
            End Get
            Set(ByVal value As MessageType)
                _type = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Channel"), _
            Description("The MIDI message type."), _
            DefaultValue(MidiChannel.Passthrough)> _
        Public Property Channel() As MidiChannel
            Get
                Return _chan
            End Get
            Set(ByVal value As MidiChannel)
                _chan = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Data Byte 1"), _
            Description("The first data byte of the message to send. Displayed as: '[HEX, DEC] Note'." & vbCrLf & vbCrLf & "For Note On/Off message types this is th Pitch or Note value, for the Control Change message type, this is the control number."), _
            DefaultValue(Pitch.Passthrough)> _
        Public Property Note() As Pitch
            Get
                Return _note
            End Get
            Set(ByVal value As Pitch)
                _note = value
            End Set
        End Property

        <DisplayName("Data Byte 2"), _
            Description("The second data byte of the message to send. For Note On/Off messages this is the velocity; for Control Change messages, this is the value of the control." & vbCrLf & vbCrLf & "To use the incoming MIDI data (passthrough), enter a value of 255."), _
            DefaultValue(127)> _
        Public Property Value() As Byte
            Get
                Return _vel
            End Get
            Set(ByVal value As Byte)
                If (value = 255) Then
                    _vel = 255
                Else
                    If (value > 127) Then
                        _vel = 127
                    Else
                        _vel = value
                    End If
                End If
            End Set
        End Property

        Public Sub New()
            _dev = "ALL DEVICES"
            _type = MessageType.NoteOn
            _chan = MidiChannel.Passthrough
            _note = Pitch.Passthrough
            _vel = 127
        End Sub
    End Class

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

#Region "MIDI Enums"
Public Enum MessageType As Byte
    <Description("Passthrough")> Passthrough = 0
    <Description("Note On")> NoteOn = 144 '9x
    <Description("Note Off")> NoteOff = 128 '8x
    <Description("Control Change")> ControlChange = 176 'Bx
    '<Description("Polyphonic Key Pressure")> PolyphonicKeyPressure = 160 'Ax
    <Description("Program Change")> ProgramChange = 192 'Cx
    '<Description("Channel Pressure")> ChannelPressure = 208 'Dx
    <Description("Pitch Bend")> PitchBend = 224 'Ex
End Enum

Public Enum MidiChannel As Byte
    <Description("Passthrough")> Passthrough = 255
    <Description("Channel 1")> Channel1 = 0
    <Description("Channel 2")> Channel2 = 1
    <Description("Channel 3")> Channel3 = 2
    <Description("Channel 4")> Channel4 = 3
    <Description("Channel 5")> Channel5 = 4
    <Description("Channel 6")> Channel6 = 5
    <Description("Channel 7")> Channel7 = 6
    <Description("Channel 8")> Channel8 = 7
    <Description("Channel 9")> Channel9 = 8
    <Description("Channel 10")> Channel10 = 9
    <Description("Channel 11")> Channel11 = 10
    <Description("Channel 12")> Channel12 = 11
    <Description("Channel 13")> Channel13 = 12
    <Description("Channel 14")> Channel14 = 13
    <Description("Channel 15")> Channel15 = 14
    <Description("Channel 16")> Channel16 = 15
End Enum

Public Enum Pitch As Byte
    <Description("Passthrough")> Passthrough = 255
    <Description("[00,    0]   C-1")> CNeg1 = 0
    <Description("[01,    1]   C#-1")> CSharpNeg1 = 1
    <Description("[02,    2]   D-1")> DNeg1 = 2
    <Description("[03,    3]   D#-1")> DSharpNeg1 = 3
    <Description("[04,    4]   E-1")> ENeg1 = 4
    <Description("[05,    5]   F-1")> FNeg1 = 5
    <Description("[06,    6]   F#-1")> FSharpNeg1 = 6
    <Description("[07,    7]   G-1")> GNeg1 = 7
    <Description("[08,    8]   G#-1")> GSharpNeg1 = 8
    <Description("[09,    9]   A-1")> ANeg1 = 9
    <Description("[0A,   10]   A#-1")> ASharpNeg1 = 10
    <Description("[0B,   11]   B-1")> BNeg1 = 11
    <Description("[0C,   12]   C0")> C0 = 12
    <Description("[0D,   13]   C#0")> CSharp0 = 13
    <Description("[0E,   14]   D0")> D0 = 14
    <Description("[0F,   15]   D#0")> DSharp0 = 15
    <Description("[10,   16]   E0")> E0 = 16
    <Description("[11,   17]   F0")> F0 = 17
    <Description("[12,   18]   F#0")> FSharp0 = 18
    <Description("[13,   19]   G0")> G0 = 19
    <Description("[14,   20]   G#0")> GSharp0 = 20
    <Description("[15,   21]   A0")> A0 = 21
    <Description("[16,   22]   A#0")> ASharp0 = 22 'usually the lowest key on an 88-key keyboard
    <Description("[17,   23]   B0")> B0 = 23
    <Description("[18,   24]   C1")> C1 = 24
    <Description("[19,   25]   C#1")> CSharp1 = 25
    <Description("[1A,   26]   D1")> D1 = 26
    <Description("[1B,   27]   D#1")> DSharp1 = 27
    <Description("[1C,   28]   E1")> E1 = 28
    <Description("[1D,   29]   F1")> F1 = 29
    <Description("[1E,   30]   F#1")> FSharp1 = 30
    <Description("[1F,   31]   G1")> G1 = 31
    <Description("[20,   32]   G#1")> GSharp1 = 32
    <Description("[21,   33]   A1")> A1 = 33
    <Description("[22,   34]   A#1")> ASharp1 = 34
    <Description("[23,   35]   B1")> B1 = 35
    <Description("[24,   36]   C2")> C2 = 36
    <Description("[25,   37]   C#2")> CSharp2 = 37
    <Description("[26,   38]   D2")> D2 = 38
    <Description("[27,   39]   D#2")> DSharp2 = 39
    <Description("[28,   40]   E2")> E2 = 40
    <Description("[29,   41]   F2")> F2 = 41
    <Description("[2A,   42]   F#2")> FSharp2 = 42
    <Description("[2B,   43]   G2")> G2 = 43
    <Description("[2C,   44]   G#2")> GSharp2 = 44
    <Description("[2D,   45]   A2")> A2 = 45
    <Description("[2E,   46]   A#2")> ASharp2 = 46
    <Description("[2F,   47]   B2")> B2 = 47
    <Description("[30,   48]   C3")> C3 = 48
    <Description("[31,   49]   C#3")> CSharp3 = 49
    <Description("[32,   50]   D3")> D3 = 50
    <Description("[33,   51]   D#3")> DSharp3 = 51
    <Description("[34,   52]   E3")> E3 = 52
    <Description("[35,   53]   F3")> F3 = 53
    <Description("[36,   54]   F#3")> FSharp3 = 54
    <Description("[37,   55]   G3")> G3 = 55
    <Description("[38,   56]   G#3")> GSharp3 = 56
    <Description("[39,   57]   A3")> A3 = 57
    <Description("[3A,   58]   A#3")> ASharp3 = 58
    <Description("[3B,   59]   B3")> B3 = 59
    <Description("[3C,   60]   C4")> C4 = 60
    <Description("[3D,   61]   C#4")> CSharp4 = 61
    <Description("[3E,   62]   D4")> D4 = 62
    <Description("[3F,   63]   D#4")> DSharp4 = 63
    <Description("[40,   64]   E4")> E4 = 64
    <Description("[41,   65]   F4")> F4 = 65
    <Description("[42,   66]   F#4")> FSharp4 = 66
    <Description("[43,   67]   G4")> G4 = 67
    <Description("[44,   68]   G#4")> GSharp4 = 68
    <Description("[45,   69]   A4")> A4 = 69
    <Description("[46,   70]   A#4")> ASharp4 = 70
    <Description("[47,   71]   B4")> B4 = 71
    <Description("[48,   72]   C5")> C5 = 72
    <Description("[49,   73]   C#5")> CSharp5 = 73
    <Description("[4A,   74]   D5")> D5 = 74
    <Description("[4B,   75]   D#5")> DSharp5 = 75
    <Description("[4C,   76]   E5")> E5 = 76
    <Description("[4D,   77]   F5")> F5 = 77
    <Description("[4E,   78]   F#5")> FSharp5 = 78
    <Description("[4F,   79]   G5")> G5 = 79
    <Description("[50,   80]   G#5")> GSharp5 = 80
    <Description("[51,   81]   A5")> A5 = 81
    <Description("[52,   82]   A#5")> ASharp5 = 82
    <Description("[53,   83]   B5")> B5 = 83
    <Description("[54,   84]   C6")> C6 = 84
    <Description("[55,   85]   C#6")> CSharp6 = 85
    <Description("[56,   86]   D6")> D6 = 86
    <Description("[57,   87]   D#6")> DSharp6 = 87
    <Description("[58,   88]   E6")> E6 = 88
    <Description("[59,   89]   F6")> F6 = 89
    <Description("[5A,   90]   F#6")> FSharp6 = 90
    <Description("[5B,   91]   G6")> G6 = 91
    <Description("[5C,   92]   G#6")> GSharp6 = 92
    <Description("[5D,   93]   A6")> A6 = 93
    <Description("[5E,   94]   A#6")> ASharp6 = 94
    <Description("[5F,   95]   B6")> B6 = 95
    <Description("[60,   96]   C7")> C7 = 96
    <Description("[61,   97]   C#7")> CSharp7 = 97
    <Description("[62,   98]   D7")> D7 = 98
    <Description("[63,   99]   D#7")> DSharp7 = 99
    <Description("[64,  100]   E7")> E7 = 100
    <Description("[65,  101]   F7")> F7 = 101
    <Description("[66,  102]   F#7")> FSharp7 = 102
    <Description("[67,  103]   G7")> G7 = 103
    <Description("[68,  104]   G#7")> GSharp7 = 104
    <Description("[69,  105]   A7")> A7 = 105
    <Description("[6A,  106]   A#7")> ASharp7 = 106
    <Description("[6B,  107]   B7")> B7 = 107
    <Description("[6C,  108]   C8")> C8 = 108 'usually the highest key on an 88-key keyboard
    <Description("[6D,  109]   C#8")> CSharp8 = 109
    <Description("[6E,  110]   D8")> D8 = 110
    <Description("[6F,  111]   D#8")> DSharp8 = 111
    <Description("[70,  112]   E8")> E8 = 112
    <Description("[71,  113]   F8")> F8 = 113
    <Description("[72,  114]   F#8")> FSharp8 = 114
    <Description("[73,  115]   G8")> G8 = 115
    <Description("[74,  116]   G#8")> GSharp8 = 116
    <Description("[75,  117]   A8")> A8 = 117
    <Description("[76,  118]   A#8")> ASharp8 = 118
    <Description("[77,  119]   B8")> B8 = 119
    <Description("[78,  120]   C9")> C9 = 120
    <Description("[79,  121]   C#9")> CSharp9 = 121
    <Description("[7A,  122]   D9")> D9 = 122
    <Description("[7B,  123]   D#9")> DSharp9 = 123
    <Description("[7C,  124]   E9")> E9 = 124
    <Description("[7D,  125]   F9")> F9 = 125
    <Description("[7E,  126]   F#9")> FSharp9 = 126
    <Description("[7F,  127]   G9")> G9 = 127
End Enum
#End Region