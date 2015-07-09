Public Interface iAction
    ReadOnly Property UniqueID() As Guid
    ReadOnly Property Group() As String
    ReadOnly Property Name() As String
    ReadOnly Property Description() As String
    'Type: 128=NoteOff, 144=NoteOn, 176=Control Change
    Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean
    Sub Initialize(ByRef Host As iServices)
End Interface

Public Interface iServices
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

    ''Windows Message Functions
    Function SendMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Function SendMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    Function PostMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Function PostMessage(ByVal Handle As Integer, ByVal uMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
    Function SendLJMessage(ByVal uMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
End Interface