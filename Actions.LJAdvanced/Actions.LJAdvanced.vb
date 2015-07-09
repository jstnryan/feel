Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A0")> _
Public Class IntensityGroupValueAbs
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    'Private Const WM_COPYDATA As Integer = &H4A ''74
    'Private Structure CopyData
    '    Friend dwData As Integer
    '    Friend cbData As Integer
    '    Friend lpData As IntPtr
    'End Structure
    Private Const _WMCOPY_SetIntensityMasters As Integer = 266
    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential, Pack:=1, Size:=18)> _
    Private Structure IntensityState
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValArray, sizeconst:=9)> Public Flags As Byte()  '0: Ignore, <>0: Set value 
        <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValArray, sizeconst:=9)> Public Values As Byte() 'Value (0-255)
    End Structure

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            'Return "Control intensity values by Intensity Group."
            Return "Control the value of an Intensity Group, or Master fader." & vbCrLf & vbCrLf & "This action is intended to be used with controls that give absolute values (such as standard faders, or non-continuous encoders)."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim Intensities As IntensityState
        Intensities.Flags = New Byte() { _
            CByte(If(_myData._group = 0, 1, 0)), _
            CByte(If(_myData._group = 1, 1, 0)), _
            CByte(If(_myData._group = 2, 1, 0)), _
            CByte(If(_myData._group = 3, 1, 0)), _
            CByte(If(_myData._group = 4, 1, 0)), _
            CByte(If(_myData._group = 5, 1, 0)), _
            CByte(If(_myData._group = 6, 1, 0)), _
            CByte(If(_myData._group = 7, 1, 0)), _
            CByte(If(_myData._group = 8, 1, 0)) _
        }
        Dim val As Byte = CByte(If(_myData._invert, (127 - VelVal) * If(_myData._double, 2, 1), VelVal * If(_myData._double, 2, 1)))

        ''A little hack to avoid a maximum intensity of 254:
        If (_myData._double) And (val = 254) Then val = 255

        Intensities.Values = New Byte() {val, val, val, val, val, val, val, val, val}

        Dim CData As Feel.ActionInterface.CopyData
        CData.dwData = _WMCOPY_SetIntensityMasters
        CData.cbData = Runtime.InteropServices.Marshal.SizeOf(GetType(IntensityState))
        Dim Pointer As IntPtr
        Pointer = Runtime.InteropServices.Marshal.AllocHGlobal(Runtime.InteropServices.Marshal.SizeOf(Intensities))
        Runtime.InteropServices.Marshal.StructureToPtr(Intensities, Pointer, False)
        CData.lpData = Pointer

        Dim result As Integer = _host.SendLJCopyData(CData)
        If (result = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Intensity Group Value (abs)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A0")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, IntensityGroupValueAbs.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' IntensityGroupValueAbs.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _group As Byte
        Friend _double As Boolean
        Friend _invert As Boolean

        <DisplayName("Group"), _
            Description("The index of the group to be controlled. For example, Master is '0', Intensity Group 1 is '1', and so on."), _
            DefaultValue(0)> _
        Public Property Group() As Byte
            Get
                Return _group
            End Get
            Set(ByVal value As Byte)
                _group = CByte(If(value > 8, 8, If(value < 0, 0, value)))
            End Set
        End Property

        <DisplayName("Double"), _
            Description("This should ALWAYS be set to 'True' unless you have a controller which breaks MIDI standard and has 'high resolution' faders which send values higher than 0-127."), _
            DefaultValue(True)> _
        Public Property vDouble() As Boolean
            Get
                Return _double
            End Get
            Set(ByVal value As Boolean)
                _double = value
            End Set
        End Property

        <DisplayName("Invert"), _
            Description("Set to 'True' to swap increment/decrement direction."), _
            DefaultValue(False)> _
        Public Property Invert() As Boolean
            Get
                Return _invert
            End Get
            Set(ByVal value As Boolean)
                _invert = value
            End Set
        End Property

        Public Sub New()
            _group = 0
            _double = True
            _invert = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A1")> _
Public Class IntensityGroupValueRel
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            'Return "Control intensity values by Intensity Group."
            Return "Control the value of an Intensity Group, or Master fader." & vbCrLf & vbCrLf & "Intended for use with relative controls, such as 'endless encoders.'"
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim amt As Integer = VelVal
        If (VelVal > 63) Then
            ''decrement
            amt = CType(Math.Ceiling(((128 - VelVal) * _myData._multiplier) / 100), Integer)
            If (amt > 32767) Then amt = 32767
            If Not (_myData._invert) Then
                amt = 65536 - amt
            End If
        Else
            ''increment
            amt = CType(Math.Ceiling((VelVal * _myData._multiplier) / 100), Integer)
            If (amt > 32767) Then amt = 32767
            If (_myData._invert) Then
                amt = 65536 - amt
            End If
        End If

        amt = (65536 * _myData._group) + amt

        Dim ret As Integer = _host.PostLJMessage(135, 53, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Intensity Group Value (rel)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A1")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, IntensityGroupValueRel.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' IntensityGroupValueRel.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _group As Byte
        Friend _multiplier As Integer
        Friend _invert As Boolean

        <DisplayName("Group"), _
            Description("The index of the group to be controlled. For example, Master is '0', Intensity Group 1 is '1', and so on."), _
            DefaultValue(0)> _
        Public Property Group() As Byte
            Get
                Return _group
            End Get
            Set(ByVal value As Byte)
                _group = CByte(If(value > 8, 8, If(value < 0, 0, value)))
            End Set
        End Property

        <DisplayName("Increment Multiplier"), _
            Description("Typically used only with extremely sensitive or insensitive controls, adjust this value to change the multiplier of the given amount provided by the physical control." & vbCrLf & vbCrLf & "The default value of 100 (meaning '100%') uses the values sent by the controller. Values greater than 100 increase the 'speed' of change by that factor, while values between 0 and 100 decrease the speed of change."), _
            DefaultValue(100)> _
        Public Property Multiplier() As Integer
            Get
                Return _multiplier
            End Get
            Set(ByVal value As Integer)
                _multiplier = value
            End Set
        End Property

        <DisplayName("Invert"), _
            Description("Set to 'True' to swap increment/decrement direction."), _
            DefaultValue(False)> _
        Public Property Invert() As Boolean
            Get
                Return _invert
            End Get
            Set(ByVal value As Boolean)
                _invert = value
            End Set
        End Property

        Public Sub New()
            _group = 0
            _multiplier = 100
            _invert = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A2")> _
Public Class CueMacroAmplitudeAbs
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Adjusts Cue Macro Control Amplitude by absolute adjustments." & vbCrLf & vbCrLf & "Intended for use with absolute controls, such as faders and non-endless encoders."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim amt As Integer = VelVal

        If (_myData._double) Then
            amt = amt * 2
            If (amt = 254) Then amt = 255 ''A little hack to avoid a maximum intensity of 254
        End If

        If (_myData._invert) Then amt = amt * -1

        Dim ret As Integer = _host.PostLJMessage(167, 8150796, -256)
        ret = _host.PostLJMessage(167, 8150796, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Cue Macro Amplitude (abs)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A2")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, CueMacroAmplitudeAbs.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' CueMacroAmplitudeAbs.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _double As Boolean
        Friend _invert As Boolean

        <DisplayName("Double"), _
            Description("This should ALWAYS be set to 'True' unless you have a controller which breaks MIDI standard and has 'high resolution' faders which send values higher than 0-127."), _
            DefaultValue(True)> _
        Public Property vDouble() As Boolean
            Get
                Return _double
            End Get
            Set(ByVal value As Boolean)
                _double = value
            End Set
        End Property

        <DisplayName("Invert Direction"), _
            Description("Set to 'True' to swap increment/decrement direction."), _
            DefaultValue(False)> _
        Public Property Invert() As Boolean
            Get
                Return _invert
            End Get
            Set(ByVal value As Boolean)
                _invert = value
            End Set
        End Property

        Public Sub New()
            _double = True
            _invert = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A3")> _
Public Class CueMacroAmplitudeRel
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Adjusts Cue Macro Control Amplitude by relative increments." & vbCrLf & vbCrLf & "Intended for use with relative controls, such as 'endless encoders.'"
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim amt As Integer = VelVal
        If (VelVal > 63) Then
            amt = CType(Math.Floor((((128 - amt) * -1) * _myData._multiplier) / 100), Integer)
        Else
            amt = CType(Math.Ceiling((amt * _myData._multiplier) / 100), Integer)
        End If
        If (amt = 0) Then
            amt = If(VelVal > 63, amt - 1, amt + 1)
        End If
        If (_myData._invert) Then amt = amt * -1

        Dim ret As Integer = _host.PostLJMessage(167, 8150796, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Cue Macro Amplitude (rel)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A3")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, CueMacroAmplitudeRel.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' CueMacroAmplitudeRel.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _multiplier As Integer
        Friend _invert As Boolean

        <DisplayName("Increment Multiplier"), _
            Description("Typically used only with extremely sensitive or insensitive controls, adjust this value to change the multiplier of the given amount provided by the physical control." & vbCrLf & vbCrLf & "The default value of 100 (meaning '100%') uses the values sent by the controller. Values greater than 100 increase the 'speed' of change by that factor, while values between 0 and 100 decrease the speed of change."), _
            DefaultValue(100)> _
        Public Property Multiplier() As Integer
            Get
                Return _multiplier
            End Get
            Set(ByVal value As Integer)
                _multiplier = value
            End Set
        End Property

        <DisplayName("Invert Direction"), _
            Description("Set to 'True' to swap increment/decrement direction."), _
            DefaultValue(False)> _
        Public Property Invert() As Boolean
            Get
                Return _invert
            End Get
            Set(ByVal value As Boolean)
                _invert = value
            End Set
        End Property

        Public Sub New()
            _multiplier = 100
            _invert = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A4")> _
Public Class CueMacroSpeedAbs
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Adjusts Cue Macro Control Speed by absolute adjustments." & vbCrLf & vbCrLf & "Intended for use with absolute controls, such as faders and non-endless encoders."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim amt As Integer = VelVal

        If (_myData._double) Then
            amt = amt * 2
            If (amt = 254) Then amt = 255 ''A little hack to avoid a maximum intensity of 254
        End If

        If (_myData._invert) Then amt = amt * -1

        Dim ret As Integer = _host.PostLJMessage(167, 8150800, -256)
        ret = _host.PostLJMessage(167, 8150800, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Cue Macro Speed (abs)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A4")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, CueMacroSpeedAbs.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' CueMacroSpeedAbs.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _double As Boolean
        Friend _invert As Boolean

        <DisplayName("Double"), _
            Description("This should ALWAYS be set to 'True' unless you have a controller which breaks MIDI standard and has 'high resolution' faders which send values higher than 0-127."), _
            DefaultValue(True)> _
        Public Property vDouble() As Boolean
            Get
                Return _double
            End Get
            Set(ByVal value As Boolean)
                _double = value
            End Set
        End Property

        <DisplayName("Invert Direction"), _
            Description("Set to 'True' to swap increment/decrement direction."), _
            DefaultValue(False)> _
        Public Property Invert() As Boolean
            Get
                Return _invert
            End Get
            Set(ByVal value As Boolean)
                _invert = value
            End Set
        End Property

        Public Sub New()
            _double = True
            _invert = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A5")> _
Public Class CueMacroSpeedRel
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Adjusts Cue Macro Control Speed by relative increments." & vbCrLf & vbCrLf & "Intended for use with relative controls, such as 'endless encoders.'"
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim amt As Integer = VelVal
        If (VelVal > 63) Then
            amt = CType(Math.Floor((((128 - amt) * -1) * _myData._multiplier) / 100), Integer)
        Else
            amt = CType(Math.Ceiling((amt * _myData._multiplier) / 100), Integer)
        End If
        If (amt = 0) Then
            amt = If(VelVal > 63, amt - 1, amt + 1)
        End If
        If (_myData._invert) Then amt = amt * -1

        Dim ret As Integer = _host.PostLJMessage(167, 8150800, amt)
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Cue Macro Speed (rel)"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A5")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, CueMacroSpeedRel.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' CueMacroSpeedRel.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _multiplier As Integer
        Friend _invert As Boolean

        <DisplayName("Increment Multiplier"), _
            Description("Typically used only with extremely sensitive or insensitive controls, adjust this value to change the multiplier of the given amount provided by the physical control." & vbCrLf & vbCrLf & "The default value of 100 (meaning '100%') uses the values sent by the controller. Values greater than 100 increase the 'speed' of change by that factor, while values between 0 and 100 decrease the speed of change."), _
            DefaultValue(100)> _
        Public Property Multiplier() As Integer
            Get
                Return _multiplier
            End Get
            Set(ByVal value As Integer)
                _multiplier = value
            End Set
        End Property

        <DisplayName("Invert Direction"), _
            Description("Set to 'True' to swap increment/decrement direction."), _
            DefaultValue(False)> _
        Public Property Invert() As Boolean
            Get
                Return _invert
            End Get
            Set(ByVal value As Boolean)
                _invert = value
            End Set
        End Property

        Public Sub New()
            _multiplier = 100
            _invert = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A6")> _
Public Class LoadCue
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Loads the selected Cue, or merges the selected Transparent Cue"
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim ret As Integer = _host.PostLJMessage(1002, _myData._cue, If(_myData._forceCuelistRestart, 1, 0))
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Load Cue"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A6")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, LoadCue.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' LoadCue.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _cue As Integer
        Friend _forceCuelistRestart As Boolean

        <DisplayName("Cue"), _
            Description("The Cue number to load." & vbCrLf & vbCrLf & "Set to 0 to clear Cue."), _
            DefaultValue(0)> _
        Public Property Cue() As Integer
            Get
                Return _cue
            End Get
            Set(ByVal value As Integer)
                _cue = value
            End Set
        End Property

        <DisplayName("Force Restart"), _
            Description("Force the Cue to restart."), _
            DefaultValue(False)> _
        Public Property ForceRestart() As Boolean
            Get
                Return _forceCuelistRestart
            End Get
            Set(ByVal value As Boolean)
                _forceCuelistRestart = value
            End Set
        End Property

        Public Sub New()
            _cue = 0
            _forceCuelistRestart = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A7")> _
Public Class LoadCuelist
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Loads the selected CueList"
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim ret As Integer = _host.PostLJMessage(1001, _myData._cuelist, If(_myData._forceCueListRestart, 1, 0))
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Load CueList"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A7")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, LoadCuelist.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' CueMacroSpeedRel.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _cuelist As Integer
        Friend _forceCueListRestart As Boolean

        <DisplayName("CueList"), _
        Description("The CueList number to load." & vbCrLf & vbCrLf & "Set to -1 to clear CueList."), _
        DefaultValue(0)> _
        Public Property Cue() As Integer
            Get
                Return _cuelist
            End Get
            Set(ByVal value As Integer)
                _cuelist = value
            End Set
        End Property

        <DisplayName("Force Restart"), _
            Description("Force the CueList to restart."), _
            DefaultValue(False)> _
        Public Property ForceRestart() As Boolean
            Get
                Return _forceCueListRestart
            End Get
            Set(ByVal value As Boolean)
                _forceCueListRestart = value
            End Set
        End Property

        Public Sub New()
            _cuelist = 0
            _forceCueListRestart = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A8")> _
Public Class LoadBackgroundCue
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Load or merge the selected Background Cue, or clear the Background Cue."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim ret As Integer
        If (_myData._bgcue = -1) Then
            ret = _host.PostLJMessage(114, 9, 0) ''Clear BGCue
        Else
            If _myData._merge Then
                ret = _host.PostLJMessage(114, 10, _myData._bgcue) ''Merge BGCue ("transparent")
            Else
                ret = _host.PostLJMessage(114, 7, _myData._bgcue) ''Load BGCue
            End If
        End If

        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Load Background Cue"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A8")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, LoadBackgroundCue.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' LoadBackgroundCue.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _bgcue As Integer
        Friend _merge As Boolean

        <DisplayName("Background Cue"), _
        Description("The number of the Background Cue to load." & vbCrLf & vbCrLf & "Set to -1 to clear the Background Cue."), _
        DefaultValue(0)> _
        Public Property bgcue() As Integer
            Get
                Return _bgcue
            End Get
            Set(ByVal value As Integer)
                _bgcue = value
            End Set
        End Property

        <DisplayName("Merge"), _
            Description("Merge the new Background Cue with the currently running Background Cue. Occupied slots in the new cue will replace those slots in the old cue."), _
            DefaultValue(False)> _
        Public Property merge() As Boolean
            Get
                Return _merge
            End Get
            Set(ByVal value As Boolean)
                _merge = value
            End Set
        End Property

        Public Sub New()
            _bgcue = 0
            _merge = False
        End Sub
    End Class
End Class

<Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A9")> _
Public Class FlashSequence
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Flash selected Sequence, or clear flashed Sequence"
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        Dim ret As Integer
        If _myData._clear Then
            'TODO:
            ''Strange behaviours here...
            'ret = _host.PostLJMessage(1004, _myData._sequence, -1)
            'ret = _host.PostLJMessage(125, 68, 0)
            'ret = _host.PostLJMessage(125, 88, -1)
            'ret = _host.PostLJMessage(1006, 284, 0)
            ret = _host.PostLJMessage(1007, 284, 0)
        Else
            ret = _host.PostLJMessage(1004, _myData._sequence, 0)
        End If
        If (ret = -1) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ Advanced Functions"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False

        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Flash Sequence"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("88F81E46-596B-4aa3-A5C4-8FB475DA21A9")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, FlashSequence.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' CueMacroSpeedRel.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _sequence As Integer
        Friend _clear As Boolean

        <DisplayName("Sequence"), _
        Description("The number of the Sequence to flash." & vbCrLf & vbCrLf & "When clearing the flashed Sequence, the Sequence number is ignored."), _
        DefaultValue(0)> _
        Public Property sequence() As Integer
            Get
                Return _sequence
            End Get
            Set(ByVal value As Integer)
                _sequence = value
            End Set
        End Property

        <DisplayName("Clear"), _
            Description("Releases (clears) the currently flashed Sequence when set to 'True.'"), _
            DefaultValue(False)> _
        Public Property merge() As Boolean
            Get
                Return _clear
            End Get
            Set(ByVal value As Boolean)
                _clear = value
            End Set
        End Property

        Public Sub New()
            _sequence = 0
            _clear = False
        End Sub
    End Class
End Class