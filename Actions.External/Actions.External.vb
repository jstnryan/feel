Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("D5644026-BD84-482b-AF02-BB844852BF00")> _
Public Class ExternalSystem
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "General LightJockey functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "System Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF00")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalSystem.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalSystem.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalSystem.Hotkey.CloseWindow)> _
        Public Property Funct() As ExternalSystem.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalSystem.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalSystem.Hotkey.CloseWindow
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("Close Window")> CloseWindow = 54
        <Description("Close All Windows")> CloseAllWindows = 55
        <Description("*Freeze Output")> FreezeOutput = 127
        <Description("Freeze Output Toggle")> FreezeOutputToggle = 128
        <Description("*Freeze+ B/O")> FreezeAndBlackout = 494
        <Description("Freeze+ B/O Toggle")> FreezeAndBlackoutToggle = 495
        <Description("DMX Offline")> DmxOffline = 442
        <Description("DMX Online")> DmxOnline = 443
        <Description("DMX OnLine/OffLine toggle")> DmxOnlineOfflineToggle = 444
        <Description("Toggle Virtual Fingers")> ToggleVirtualFingers = 490
        <Description("Show Virtual Fingers")> ShowVirtualFingers = 493
        <Description("Toggle Fingers Status")> ToggleFingersStatus = 491
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF01")> _
Public Class ExternalGlobalIntensity
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Global Intensity functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Global Intensity Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF01")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalGlobalIntensity.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalGlobalIntensity.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalGlobalIntensity.Hotkey.ToggleMasterIntensityControl)> _
        Public Property Funct() As ExternalGlobalIntensity.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalGlobalIntensity.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate/deactivate function), the names of which are generally preceeded by an asterisk (*). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalGlobalIntensity.Hotkey.ToggleMasterIntensityControl
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey As Integer
        <Description("Toggle Master Intensity Control")> ToggleMasterIntensityControl = 266
        <Description("Blackout/Restore")> BlackoutRestore = 52
        <Description("Black Out")> BlackOut = 240
        <Description("Restore")> Restore = 241
        <Description("FadeOut/FadeIn")> FadeOutFadeIn = 53
        <Description("Fade Out")> FadeOut = 242
        <Description("Fade In")> FadeIn = 243
        <Description("Fade Out 1 Sec")> FadeOut1Sec = 244
        <Description("Fade Out 2 Sec")> FadeOut2Sec = 245
        <Description("Fade Out 3 Sec")> FadeOut3Sec = 246
        <Description("Fade Out 4 Sec")> FadeOut4Sec = 247
        <Description("Fade Out 5 Sec")> FadeOut5Sec = 248
        <Description("Fade Out 6 Sec")> FadeOut6Sec = 249
        <Description("Fade Out 7 Sec")> FadeOut7Sec = 250
        <Description("Fade Out 8 Sec")> FadeOut8Sec = 251
        <Description("Fade Out 9 Sec")> FadeOut9Sec = 252
        <Description("Fade Out 10 Sec")> FadeOut10Sec = 253
        <Description("Fade In 1 Sec")> FadeIn1Sec = 254
        <Description("Fade In 2 Sec")> FadeIn2Sec = 255
        <Description("Fade In 3 Sec")> FadeIn3Sec = 256
        <Description("Fade In 4 Sec")> FadeIn4Sec = 257
        <Description("Fade In 5 Sec")> FadeIn5Sec = 258
        <Description("Fade In 6 Sec")> FadeIn6Sec = 259
        <Description("Fade In 7 Sec")> FadeIn7Sec = 260
        <Description("Fade In 8 Sec")> FadeIn8Sec = 261
        <Description("Fade In 9 Sec")> FadeIn9Sec = 262
        <Description("Fade In 10 Sec")> FadeIn10Sec = 263
        <Description("Master Intensity Off")> MasterIntensityOff = 484
        <Description("Master Intensity Full")> MasterIntensityFull = 485
        <Description("Master Intensity +1")> MasterIntensityPlus1 = 486
        <Description("Master Intensity +10")> MasterIntenstityPlus10 = 487
        <Description("Master Intensity -1")> MasterIntensityMinus1 = 488
        <Description("Master Intensity -10")> MasterIntensityMinus10 = 489
        <Description("*Blackout")> _Blackout = 129
        <Description("*FadeOut")> _FadeOut = 130
        <Description("*Bump Master")> _BumpMaster = 86
        <Description("*Bump Sub 1")> _BumpSub1 = 87
        <Description("*Bump Sub 2")> _BumpSub2 = 88
        <Description("*Bump Sub 3")> _BumpSub3 = 89
        <Description("*Bump Sub 4")> _BumpSub4 = 90
        <Description("*Bump Sub 5")> _BumpSub5 = 91
        <Description("*Bump Sub 6")> _BumpSub6 = 92
        <Description("*Bump Sub 7")> _BumpSub7 = 93
        <Description("*Bump Sub 8")> _BumpSub8 = 94
        <Description("Toggle Bump Master")> ToggleBumpMaster = 100
        <Description("Toggle Bump Sub 1")> ToggleBumpSub1 = 101
        <Description("Toggle Bump Sub 2")> ToggleBumpSub2 = 102
        <Description("Toggle Bump Sub 3")> ToggleBumpSub3 = 103
        <Description("Toggle Bump Sub 4")> ToggleBumpSub4 = 104
        <Description("Toggle Bump Sub 5")> ToggleBumpSub5 = 105
        <Description("Toggle Bump Sub 6")> ToggleBumpSub6 = 106
        <Description("Toggle Bump Sub 7")> ToggleBumpSub7 = 107
        <Description("Toggle Bump Sub 8")> ToggleBumpSub8 = 108
        'HTP Groups
        <Description("*HTP Group 1 - Bump")> _HtpGroup1Bump = 410
        <Description("*HTP Group 2 - Bump")> _HtpGroup2Bump = 411
        <Description("*HTP Group 3 - Bump")> _HtpGroup3Bump = 412
        <Description("*HTP Group 4 - Bump")> _HtpGroup4Bump = 413
        <Description("*HTP Group 5 - Bump")> _HtpGroup5Bump = 414
        <Description("*HTP Group 6 - Bump")> _HtpGroup6Bump = 415
        <Description("*HTP Group 7 - Bump")> _HtpGroup7Bump = 416
        <Description("*HTP Group 8 - Bump")> _HtpGroup8Bump = 417
        <Description("*HTP Group 9 - Bump")> _HtpGroup9Bump = 418
        <Description("*HTP Group 10 - Bump")> _HtpGroup10Bump = 419
        <Description("*HTP Group 11 - Bump")> _HtpGroup11Bump = 420
        <Description("*HTP Group 12 - Bump")> _HtpGroup12Bump = 421
        <Description("HTP Group 1 - Toggle")> HtpGroup1Toggle = 422
        <Description("HTP Group 2 - Toggle")> HtpGroup2Toggle = 423
        <Description("HTP Group 3 - Toggle")> HtpGroup3Toggle = 424
        <Description("HTP Group 4 - Toggle")> HtpGroup4Toggle = 425
        <Description("HTP Group 5 - Toggle")> HtpGroup5Toggle = 426
        <Description("HTP Group 6 - Toggle")> HtpGroup6Toggle = 427
        <Description("HTP Group 7 - Toggle")> HtpGroup7Toggle = 428
        <Description("HTP Group 8 - Toggle")> HtpGroup8Toggle = 429
        <Description("HTP Group 9 - Toggle")> HtpGroup9Toggle = 430
        <Description("HTP Group 10 - Toggle")> HtpGroup10Toggle = 431
        <Description("HTP Group 11 - Toggle")> HtpGroup11Toggle = 432
        <Description("HTP Group 12 - Toggle")> HtpGroup12Toggle = 433
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF02")> _
Public Class ExternalControls
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Fixture Controls, and Generic Controls functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Controls Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF02")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalControls.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalControls.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalControls.Hotkey.Intensity)> _
        Public Property Funct() As ExternalControls.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalControls.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalControls.Hotkey.Intensity
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        'Fixture Controls
        <Description("Intensity")> Intensity = 57
        <Description("Position")> Position = 58
        <Description("Color")> Color = 59
        <Description("Gobo")> Gobo = 60
        <Description("Beam")> Beam = 61
        <Description("Effects")> Effects = 62
        <Description("Level/Special")> LevelSpecial = 63
        <Description("P/T Macro")> PTMacro = 118
        <Description("Position Options")> PositionOptions = 119
        <Description("Macro Autodelay Toggle")> MacroAutodelayToggle = 292
        <Description("Toggle Intensity Palette")> ToggleIntensityPalette = 267
        <Description("Toggle Position Preset")> TogglePositionPreset = 268
        <Description("Toggle Color Palette")> ToggleColorPalette = 269
        <Description("Toggle Gobo Palette")> ToggleGoboPalette = 270
        <Description("Toggle Beam Palette")> ToggleBeamPalette = 271
        <Description("Toggle Effects Palette")> ToggleEffectsPalette = 272
        <Description("Toggle Level/Special Palette")> ToggleLevelSpecialPalette = 273
        <Description("Toggle All Palettes/Presets")> ToggleAllPalettesPresets = 274
        <Description("Mirror Pan")> MirrorPan = 169
        <Description("Mirror Tilt")> MirrorTilt = 170
        <Description("Swap")> Swap = 171
        <Description("360 deg")> deg360 = 172
        <Description("Pan Relative +")> PanRelativePlus = 236
        <Description("Pan Relative -")> PanRelativeMinus = 237
        <Description("Tilt Relative +")> TiltRelativePlus = 238
        <Description("Tilt Relative -")> TiltRelativeMinus = 239
        <Description("Home Selected Fixtures")> HomeSelectedFixtures = 265
        <Description("All Controls")> AllControls = 64
        'Other Controls
        <Description("Generic Control")> GenericControl = 264
        <Description("Smoke Control")> SmokeControl = 71
        <Description("*Smoke Button")> _SmokeButton = 72
        <Description("Toggle Smoke Timer")> ToggleSmokeTimer = 173
        <Description("CD Control")> CDControl = 70
        <Description("2532Control")> Control2532 = 228
        <Description("Set Followspot 1")> SetFollowspot1 = 95
        <Description("Set Followspot 2")> SetFollowspot2 = 318
        <Description("Set Followspot 3")> SetFollowspot3 = 319
        <Description("Set Followspot 4")> SetFollowspot4 = 320
        <Description("Set Followspot 5")> SetFollowspot5 = 321
        <Description("Set Followspot 6")> SetFollowspot6 = 322
        <Description("Set Followspot 7")> SetFollowspot7 = 323
        <Description("Set Followspot 8")> SetFollowspot8 = 324
        <Description("Set Followspot 9")> SetFollowspot9 = 325
        <Description("Set Followspot 10")> SetFollowspot10 = 326
        <Description("Clear Followspot")> ClearFollowspot = 327
        <Description("View DMX Output")> ViewDMXOutput = 141
        <Description("Toggle Notes")> ToggleNotes = 275
        <Description("Autostrike Lamps Toggle")> AutostrikeLampsToggle = 285
        <Description("Assign Preset#1")> AssignPreset1 = 342
        <Description("Assign Preset#2")> AssignPreset2 = 343
        <Description("Assign Preset#3")> AssignPreset3 = 344
        <Description("Assign Preset#4")> AssignPreset4 = 345
        <Description("Assign Preset#5")> AssignPreset5 = 346
        <Description("Assign Preset#6")> AssignPreset6 = 347
        <Description("Assign Preset#7")> AssignPreset7 = 348
        <Description("Assign Preset#8")> AssignPreset8 = 349
        <Description("Assign Preset#9")> AssignPreset9 = 350
        <Description("Assign Preset#10")> AssignPreset10 = 351
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF03")> _
Public Class ExternalSequenceFunctions
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Sequence Functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Sequence Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF03")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalSequenceFunctions.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalSequenceFunctions.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalSequenceFunctions.Hotkey.SequenceControl)> _
        Public Property Funct() As ExternalSequenceFunctions.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalSequenceFunctions.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalSequenceFunctions.Hotkey.SequenceControl
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("Sequence Control")> SequenceControl = 67
        <Description("Toggle List of Sequences")> ToggleListOfSequences = 139
        <Description("Next Scene")> NextScene = 49
        <Description("Previous Scene")> PreviousScene = 50
        <Description("Add Scene")> AddScene = 51
        <Description("First Scene")> FirstScene = 65
        <Description("Last Scene")> LastScene = 66
        <Description("Quick Save Sequence")> QuickSaveSequence = 73
        <Description("New Sequence")> NewSequence = 138
        <Description("Set Sequence Blind")> SetSequenceBlind = 134
        <Description("*Sequence Blind")> _SequenceBlind = 135
        <Description("Clear Sequence Blind")> ClearSequenceBlind = 136
        <Description("Sequence Blind Toggle")> SequenceBlindToggle = 137
        <Description("Playback Fwd")> PlaybackFwd = 496
        <Description("Playback Rev")> PlaybackRev = 497
        <Description("Playback Stop")> PlaybackStop = 498
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF04")> _
Public Class ExternalCue
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Cue Functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Cue Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF04")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalCue.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalCue.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalCue.Hotkey.ToggleListOfCues)> _
        Public Property Funct() As ExternalCue.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalCue.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalCue.Hotkey.ToggleListOfCues
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        'List of Cues - Functions
        <Description("Toggle List of Cues")> ToggleListOfCues = 140
        <Description("Load Selected Cue (Global List)")> LoadSelectedCueGlobalList = 156
        <Description("Load Selected Cue (Cue Page)")> LoadSelectedCueCuePage = 276
        <Description("Focus Cue# Selection")> FocusCueSelection = 277
        <Description("Previous Cue Page")> PreviousCuePage = 278
        <Description("Next Cue Page")> NextCuePage = 279
        <Description("Cue Page Latch Previous Cue")> CuePageLatchPreviousCue = 280
        <Description("Cue Page Latch Next Cue")> CuePageLatchNextCue = 281
        <Description("Cue Page Select Previous Cue")> CuePageSelectPreviousCue = 282
        <Description("Cue Page Select Next Cue")> CuePageSelectNextCue = 283
        'Cue Control Functions
        <Description("Toggle Cue Control")> ToggleCueControl = 68
        <Description("Toggle Chase Manual")> ToggleChaseManual = 352
        <Description("Toggle Chase Fade")> ToggleChaseFade = 117
        <Description("*Flash Selected Sequence")> _FlashSelectedSequence = 157
        <Description("Release Latched Flash")> ReleaseLatchedFlash = 284
        <Description("Quick Save Cue")> QuickSaveCue = 74
        <Description("New/Clear Cue")> NewClearCue = 227
        <Description("Toggle Select All")> ToggleSelectAll = 85
        <Description("Cue Time Control")> CueTimeControl = 120
        <Description("Cue Loop Control")> CueLoopControl = 121
        'Cue Macro Control Functions
        <Description("Cue Macro Control Toggle")> CueMacroControlToggle = 122
        <Description("Pan/Tilt Macro, Fade Out")> PanTiltMacroFadeOut = 123
        <Description("Pan/Tilt Macro, Fade In")> PanTiltMacroFadeIn = 124
        <Description("Pan/Tilt Macro, Macro Off")> PanTiltMacroMacroOff = 125
        <Description("Pan/Tilt Macro, Macro On")> PanTiltMacroMacroOn = 126
        <Description("Pan/Tilt Macro, Reset Amplitude")> PanTiltMacroResetAmplitude = 303
        <Description("Pan/Tilt Macro, Reset Speed")> PanTiltMacroResetSpeed = 304
        <Description("Pan/Tilt Macro, Freeze")> PanTiltMacroFreeze = 393
        <Description("Pan/Tilt Macro, Run")> PanTiltMacroRun = 394
        <Description("Pan/Tilt Macro, Toggle Freeze")> PanTiltMacroToggleFreeze = 395
        <Description("*Pan/Tilt Macro, Freeze")> _PanTiltMacroFreeze = 396
        'Toggle Cue Slot Selection
        <Description("Toggle slot#1")> Toggleslot1 = 397
        <Description("Toggle slot#2")> Toggleslot2 = 398
        <Description("Toggle slot#3")> Toggleslot3 = 399
        <Description("Toggle slot#4")> Toggleslot4 = 400
        <Description("Toggle slot#5")> Toggleslot5 = 401
        <Description("Toggle slot#6")> Toggleslot6 = 402
        <Description("Toggle slot#7")> Toggleslot7 = 403
        <Description("Toggle slot#8")> Toggleslot8 = 404
        <Description("Toggle slot#9")> Toggleslot9 = 405
        <Description("Toggle slot#10")> Toggleslot10 = 406
        <Description("Toggle slot#11")> Toggleslot11 = 407
        <Description("Toggle slot#12")> Toggleslot12 = 408
        'Toggle Seq On/Off
        <Description("All Slots Off")> AllSlotsOff = 301
        <Description("All Slots On")> AllSlotsOn = 302
        <Description("Toggle Seq #1")> ToggleSeq1 = 1
        <Description("Toggle Seq #2")> ToggleSeq2 = 2
        <Description("Toggle Seq #3")> ToggleSeq3 = 3
        <Description("Toggle Seq #4")> ToggleSeq4 = 4
        <Description("Toggle Seq #5")> ToggleSeq5 = 5
        <Description("Toggle Seq #6")> ToggleSeq6 = 6
        <Description("Toggle Seq #7")> ToggleSeq7 = 7
        <Description("Toggle Seq #8")> ToggleSeq8 = 8
        <Description("Toggle Seq #9")> ToggleSeq9 = 9
        <Description("Toggle Seq #10")> ToggleSeq10 = 10
        <Description("Toggle Seq #11")> ToggleSeq11 = 11
        <Description("Toggle Seq #12")> ToggleSeq12 = 12
        'Clear Cue Slot
        <Description("Clear Sequence #1")> ClearSequence1 = 215
        <Description("Clear Sequence #2")> ClearSequence2 = 216
        <Description("Clear Sequence #3")> ClearSequence3 = 217
        <Description("Clear Sequence #4")> ClearSequence4 = 218
        <Description("Clear Sequence #5")> ClearSequence5 = 219
        <Description("Clear Sequence #6")> ClearSequence6 = 220
        <Description("Clear Sequence #7")> ClearSequence7 = 221
        <Description("Clear Sequence #8")> ClearSequence8 = 222
        <Description("Clear Sequence #9")> ClearSequence9 = 223
        <Description("Clear Sequence #10")> ClearSequence10 = 224
        <Description("Clear Sequence #11")> ClearSequence11 = 225
        <Description("Clear Sequence #12")> ClearSequence12 = 226
        'Sequences Stop/Go
        <Description("Seq #1 Stop")> Seq1Stop = 354
        <Description("Seq #2 Stop")> Seq2Stop = 355
        <Description("Seq #3 Stop")> Seq3Stop = 356
        <Description("Seq #4 Stop")> Seq4Stop = 357
        <Description("Seq #5 Stop")> Seq5Stop = 358
        <Description("Seq #6 Stop")> Seq6Stop = 359
        <Description("Seq #7 Stop")> Seq7Stop = 360
        <Description("Seq #8 Stop")> Seq8Stop = 361
        <Description("Seq #9 Stop")> Seq9Stop = 362
        <Description("Seq #10 Stop")> Seq10Stop = 363
        <Description("Seq #11 Stop")> Seq11Stop = 364
        <Description("Seq #12 Stop")> Seq12Stop = 365
        <Description("All Sequences Stop")> AllSequencesStop = 366
        <Description("Seq #1 Go")> Seq1Go = 367
        <Description("Seq #2 Go")> Seq2Go = 368
        <Description("Seq #3 Go")> Seq3Go = 369
        <Description("Seq #4 Go")> Seq4Go = 370
        <Description("Seq #5 Go")> Seq5Go = 371
        <Description("Seq #6 Go")> Seq6Go = 372
        <Description("Seq #7 Go")> Seq7Go = 373
        <Description("Seq #8 Go")> Seq8Go = 374
        <Description("Seq #9 Go")> Seq9Go = 375
        <Description("Seq #10 Go")> Seq10Go = 376
        <Description("Seq #11 Go")> Seq11Go = 377
        <Description("Seq #12 Go")> Seq12Go = 378
        <Description("All Sequences Go")> AllSequencesGo = 379
        <Description("Seq #1 Stop/Go Toggle")> Seq1StopGoToggle = 380
        <Description("Seq #2 Stop/Go Toggle")> Seq2StopGoToggle = 381
        <Description("Seq #3 Stop/Go Toggle")> Seq3StopGoToggle = 382
        <Description("Seq #4 Stop/Go Toggle")> Seq4StopGoToggle = 383
        <Description("Seq #5 Stop/Go Toggle")> Seq5StopGoToggle = 384
        <Description("Seq #6 Stop/Go Toggle")> Seq6StopGoToggle = 385
        <Description("Seq #7 Stop/Go Toggle")> Seq7StopGoToggle = 386
        <Description("Seq #8 Stop/Go Toggle")> Seq8StopGoToggle = 387
        <Description("Seq #9 Stop/Go Toggle")> Seq9StopGoToggle = 388
        <Description("Seq #10 Stop/Go Toggle")> Seq10StopGoToggle = 389
        <Description("Seq #11 Stop/Go Toggle")> Seq11StopGoToggle = 390
        <Description("Seq #12 Stop/Go Toggle")> Seq12StopGoToggle = 391
        <Description("All Sequences Stop/Go Toggle")> AllSequencesStopGoToggle = 392
        <Description("Emulate Audio Trig")> EmulateAudioTrig = 434
        'Trig Sequences
        <Description("Trig Selected Manual Default")> TrigSelectedManualDefault = 183
        <Description("Trig Selected Manual Fwd")> TrigSelectedManualFwd = 76
        <Description("Trig Selected Manual Rev")> TrigSelectedManualRev = 77
        <Description("Trig Selected Manual Rnd")> TrigSelectedManualRnd = 78
        <Description("Trig Selected Manual Bounce")> TrigSelectedManualBounce = 187
        <Description("Trig Selected Auto Default")> TrigSelectedAutoDefault = 184
        <Description("Trig Selected Auto Fwd")> TrigSelectedAutoFwd = 79
        <Description("Trig Selected Auto Rev")> TrigSelectedAutoRev = 80
        <Description("Trig Selected Auto Rnd")> TrigSelectedAutoRnd = 81
        <Description("Trig Selected Auto Bounce")> TrigSelectedAutoBounce = 188
        <Description("Trig Selected Default")> TrigSelectedDefault = 185
        <Description("Trig Selected Fwd")> TrigSelectedFwd = 82
        <Description("Trig Selected Rev")> TrigSelectedRev = 83
        <Description("Trig Selected Rnd")> TrigSelectedRnd = 84
        <Description("Trig Selected Bounce")> TrigSelectedBounce = 189
        <Description("Trig All Manual Default")> TrigAllManualDefault = 186
        <Description("Trig All Manual Fwd")> TrigAllManualFwd = 131
        <Description("Trig All Manual Rev")> TrigAllManualRev = 132
        <Description("Trig All Manual Rnd")> TrigAllManualRnd = 133
        <Description("Trig All Manual Bounce")> TrigAllManualBounce = 190
        'Trig Seq Default
        <Description("Trig Seq #1 Default")> TrigSeq1Default = 191
        <Description("Trig Seq #2 Default")> TrigSeq2Default = 192
        <Description("Trig Seq #3 Default")> TrigSeq3Default = 193
        <Description("Trig Seq #4 Default")> TrigSeq4Default = 194
        <Description("Trig Seq #5 Default")> TrigSeq5Default = 195
        <Description("Trig Seq #6 Default")> TrigSeq6Default = 196
        <Description("Trig Seq #7 Default")> TrigSeq7Default = 197
        <Description("Trig Seq #8 Default")> TrigSeq8Default = 198
        <Description("Trig Seq #9 Default")> TrigSeq9Default = 199
        <Description("Trig Seq #10 Default")> TrigSeq10Default = 200
        <Description("Trig Seq #11 Default")> TrigSeq11Default = 201
        <Description("Trig Seq #12 Default")> TrigSeq12Default = 202
        'Trig Seq Fwd
        <Description("Trig Seq #1 Fwd")> TrigSeq1Fwd = 13
        <Description("Trig Seq #2 Fwd")> TrigSeq2Fwd = 14
        <Description("Trig Seq #3 Fwd")> TrigSeq3Fwd = 15
        <Description("Trig Seq #4 Fwd")> TrigSeq4Fwd = 16
        <Description("Trig Seq #5 Fwd")> TrigSeq5Fwd = 17
        <Description("Trig Seq #6 Fwd")> TrigSeq6Fwd = 18
        <Description("Trig Seq #7 Fwd")> TrigSeq7Fwd = 19
        <Description("Trig Seq #8 Fwd")> TrigSeq8Fwd = 20
        <Description("Trig Seq #9 Fwd")> TrigSeq9Fwd = 21
        <Description("Trig Seq #10 Fwd")> TrigSeq10Fwd = 22
        <Description("Trig Seq #11 Fwd")> TrigSeq11Fwd = 23
        <Description("Trig Seq #12 Fwd")> TrigSeq12Fwd = 24
        'Trig Seq Rev
        <Description("Trig Seq #1 Rev")> TrigSeq1Rev = 25
        <Description("Trig Seq #2 Rev")> TrigSeq2Rev = 26
        <Description("Trig Seq #3 Rev")> TrigSeq3Rev = 27
        <Description("Trig Seq #4 Rev")> TrigSeq4Rev = 28
        <Description("Trig Seq #5 Rev")> TrigSeq5Rev = 29
        <Description("Trig Seq #6 Rev")> TrigSeq6Rev = 30
        <Description("Trig Seq #7 Rev")> TrigSeq7Rev = 31
        <Description("Trig Seq #8 Rev")> TrigSeq8Rev = 32
        <Description("Trig Seq #9 Rev")> TrigSeq9Rev = 33
        <Description("Trig Seq #10 Rev")> TrigSeq10Rev = 34
        <Description("Trig Seq #11 Rev")> TrigSeq11Rev = 35
        <Description("Trig Seq #12 Rev")> TrigSeq12Rev = 36
        'Trig Seq Random
        <Description("Trig Seq #1 Rnd")> TrigSeq1Rnd = 37
        <Description("Trig Seq #2 Rnd")> TrigSeq2Rnd = 38
        <Description("Trig Seq #3 Rnd")> TrigSeq3Rnd = 39
        <Description("Trig Seq #4 Rnd")> TrigSeq4Rnd = 40
        <Description("Trig Seq #5 Rnd")> TrigSeq5Rnd = 41
        <Description("Trig Seq #6 Rnd")> TrigSeq6Rnd = 42
        <Description("Trig Seq #7 Rnd")> TrigSeq7Rnd = 43
        <Description("Trig Seq #8 Rnd")> TrigSeq8Rnd = 44
        <Description("Trig Seq #9 Rnd")> TrigSeq9Rnd = 45
        <Description("Trig Seq #10 Rnd")> TrigSeq10Rnd = 46
        <Description("Trig Seq #11 Rnd")> TrigSeq11Rnd = 47
        <Description("Trig Seq #12 Rnd")> TrigSeq12Rnd = 48
        'Trig Seq Bounce
        <Description("Trig Seq #1 Bounce")> TrigSeq1Bounce = 203
        <Description("Trig Seq #2 Bounce")> TrigSeq2Bounce = 204
        <Description("Trig Seq #3 Bounce")> TrigSeq3Bounce = 205
        <Description("Trig Seq #4 Bounce")> TrigSeq4Bounce = 206
        <Description("Trig Seq #5 Bounce")> TrigSeq5Bounce = 207
        <Description("Trig Seq #6 Bounce")> TrigSeq6Bounce = 208
        <Description("Trig Seq #7 Bounce")> TrigSeq7Bounce = 209
        <Description("Trig Seq #8 Bounce")> TrigSeq8Bounce = 210
        <Description("Trig Seq #9 Bounce")> TrigSeq9Bounce = 211
        <Description("Trig Seq #10 Bounce")> TrigSeq10Bounce = 212
        <Description("Trig Seq #11 Bounce")> TrigSeq11Bounce = 213
        <Description("Trig Seq #12 Bounce")> TrigSeq12Bounce = 214
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF05")> _
Public Class ExternalCueBuilder
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Cue Builder Functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Cue Builder Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF05")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalCueBuilder.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalCueBuilder.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalCueBuilder.Hotkey.ToggleCueBuilder)> _
        Public Property Funct() As ExternalCueBuilder.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalCueBuilder.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalCueBuilder.Hotkey.ToggleCueBuilder
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("Toggle Cue Builder")> ToggleCueBuilder = 286
        <Description("Toggle Cue Capture Activate")> ToggleCueCaptureActivate = 287
        <Description("Toggle Seq Capture Activate")> ToggleSeqCaptureActivate = 290
        <Description("CueStack Go")> CueStackGo = 288
        <Description("Cue Preset Go")> CuePresetGo = 291
        <Description("Get Cue")> GetCue = 315
        <Description("Clear Cue Stack")> ClearCueStack = 440
        <Description("Clear All Seq")> ClearAllSeq = 441
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF06")> _
Public Class ExternalGenericMacro
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Generic Macro Functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Generic Macro Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF06")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalGenericMacro.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalGenericMacro.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalGenericMacro.Hotkey.GenericMacroEngineOff)> _
        Public Property Funct() As ExternalGenericMacro.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalGenericMacro.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalGenericMacro.Hotkey.GenericMacroEngineOff
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("Generic Macro Engine Off")> GenericMacroEngineOff = 481
        <Description("Generic Macro Engine On")> GenericMacroEngineOn = 482
        <Description("Generic Macro Engine Toggle")> GenericMacroEngineToggle = 483
        <Description("Toggle Generic Macro Editor")> ToggleGenericMacroEditor = 338
        <Description("Clear Generic Macro Editor")> ClearGenericMacroEditor = 353
        <Description("Generic Macro Slot On")> GenericMacroSlotOn = 457
        <Description("Generic Macro Slot Off")> GenericMacroSlotOff = 469
        <Description("Toggle Generic Macro Slot")> ToggleGenericMacroSlot = 445
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF07")> _
Public Class ExternalCuelist
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Cuelist Functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Cuelist Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF07")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalCuelist.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalCuelist.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalCuelist.Hotkey.CueListControl)> _
        Public Property Funct() As ExternalCuelist.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalCuelist.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalCuelist.Hotkey.CueListControl
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("CueList Control")> CueListControl = 69
        <Description("Toggle List of Cuelists")> ToggleListofCuelists = 439
        <Description("Quick Save Cuelist")> QuickSaveCuelist = 75
        <Description("Cuelist Start Top")> CuelistStartTop = 313
        <Description("Cuelist Start Selected")> CuelistStartSelected = 314
        <Description("Cuelist Run to Line")> CuelistRuntoLine = 438
        <Description("Cuelist Go")> CuelistGo = 179
        <Description("Cuelist Back")> CuelistBack = 180
        <Description("Cuelist Stop")> CuelistStop = 181
        <Description("Cuelist Stop And Clear")> CuelistStopAndClear = 182
        <Description("Cuelist Goto Mark 1")> CuelistGotoMark1 = 305
        <Description("Cuelist Goto Mark 2")> CuelistGotoMark2 = 306
        <Description("Cuelist Goto Mark 3")> CuelistGotoMark3 = 307
        <Description("Cuelist Goto Mark 4")> CuelistGotoMark4 = 308
        <Description("Cuelist Run From Mark 1")> CuelistRunFromMark1 = 309
        <Description("Cuelist Run From Mark 2")> CuelistRunFromMark2 = 310
        <Description("Cuelist Run From Mark 3")> CuelistRunFromMark3 = 311
        <Description("Cuelist Run From Mark 4")> CuelistRunFromMark4 = 312
        <Description("Log Timecode")> LogTimecode = 317
        <Description("Select Cuelist, Prev")> SelectCuelistPrev = 510
        <Description("Select Cuelist, Next")> SelectCuelistNext = 511
        <Description("Select Cuelist, Start")> SelectCuelistStart = 512
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF08")> _
Public Class ExternalBackgroundCues
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Background Cues functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Background Cues Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF08")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalBackgroundCues.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalBackgroundCues.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalBackgroundCues.Hotkey.ToggleBGCueControl)> _
        Public Property Funct() As ExternalBackgroundCues.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalBackgroundCues.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalBackgroundCues.Hotkey.ToggleBGCueControl
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("Toggle BGCue Control")> ToggleBGCueControl = 289
        <Description("Clear BGCue")> ClearBGCue = 232
        <Description("Toggle BGCue List")> ToggleBGCueList = 293
        <Description("BGCue - Toggle Slot 1")> BGCueToggleSlot1 = 294
        <Description("BGCue - Toggle Slot 2")> BGCueToggleSlot2 = 295
        <Description("BGCue - Toggle Slot 3")> BGCueToggleSlot3 = 296
        <Description("BGCue - Toggle Slot 4")> BGCueToggleSlot4 = 297
        <Description("BGCue - Toggle Slot 5")> BGCueToggleSlot5 = 298
        <Description("BGCue - All Slots Off")> BGCueAllSlotsOff = 299
        <Description("BGCue - All Slots On")> BGCueAllSlotsOn = 300
        <Description("Load Selected BGCue")> LoadSelectedBGCue = 409
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF09")> _
Public Class ExternalStaticsControl
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Statics Control functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Statics Control"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF09")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalStaticsControl.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalStaticsControl.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalStaticsControl.Hotkey.StaticsControl)> _
        Public Property Funct() As ExternalStaticsControl.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalStaticsControl.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalStaticsControl.Hotkey.StaticsControl
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("Statics Control")> StaticsControl = 158
        <Description("Static 01")> Static01 = 159
        <Description("Static 02")> Static02 = 160
        <Description("Static 03")> Static03 = 161
        <Description("Static 04")> Static04 = 162
        <Description("Static 05")> Static05 = 163
        <Description("Static 06")> Static06 = 164
        <Description("Static 07")> Static07 = 165
        <Description("Static 08")> Static08 = 166
        <Description("Static 09")> Static09 = 167
        <Description("Static 10")> Static10 = 168
        <Description("Static 11")> Static11 = 328
        <Description("Static 12")> Static12 = 329
        <Description("Static 13")> Static13 = 330
        <Description("Static 14")> Static14 = 331
        <Description("Static 15")> Static15 = 332
        <Description("Static 16")> Static16 = 333
        <Description("Static 17")> Static17 = 334
        <Description("Static 18")> Static18 = 335
        <Description("Static 19")> Static19 = 336
        <Description("Static 20")> Static20 = 337
        <Description("Release Statics")> ReleaseStatics = 230
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF10")> _
Public Class ExternalFixtureSelection
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Fixture Selection functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Fixture Selection Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF10")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalFixtureSelection.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalFixtureSelection.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalFixtureSelection.Hotkey.ToggleInclusiveExclusive)> _
        Public Property Funct() As ExternalFixtureSelection.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalFixtureSelection.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalFixtureSelection.Hotkey.ToggleInclusiveExclusive
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("Toggle Inclusive/Exclusive")> ToggleInclusiveExclusive = 152
        <Description("Next Fixture")> NextFixture = 96
        <Description("Previous Fixture")> PreviousFixture = 97
        <Description("De-select all fixtures")> Deselectallfixtures = 229
        <Description("Toggle Fixture Solo")> ToggleFixtureSolo = 98
        <Description("*Fixture Solo")> _FixtureSolo = 99
        'Fixture Groups
        <Description("Toggle Group View")> ToggleGroupView = 153
        <Description("Fixture Group 1")> FixtureGroup1 = 142
        <Description("Fixture Group 2")> FixtureGroup2 = 143
        <Description("Fixture Group 3")> FixtureGroup3 = 144
        <Description("Fixture Group 4")> FixtureGroup4 = 145
        <Description("Fixture Group 5")> FixtureGroup5 = 146
        <Description("Fixture Group 6")> FixtureGroup6 = 147
        <Description("Fixture Group 7")> FixtureGroup7 = 148
        <Description("Fixture Group 8")> FixtureGroup8 = 149
        <Description("Fixture Group 9")> FixtureGroup9 = 150
        <Description("Fixture Group 10")> FixtureGroup10 = 151
        <Description("Fixture Group 11")> FixtureGroup11 = 499
        <Description("Fixture Group 12")> FixtureGroup12 = 500
        <Description("Fixture Group 13")> FixtureGroup13 = 501
        <Description("Fixture Group 14")> FixtureGroup14 = 502
        <Description("Fixture Group 15")> FixtureGroup15 = 503
        <Description("Fixture Group 16")> FixtureGroup16 = 504
        <Description("Fixture Group 17")> FixtureGroup17 = 505
        <Description("Fixture Group 18")> FixtureGroup18 = 506
        <Description("Fixture Group 19")> FixtureGroup19 = 507
        <Description("Fixture Group 20")> FixtureGroup20 = 508
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF11")> _
Public Class ExternalOfflineVisualizer
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "Offline Visualizer functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Offline Visualizer Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF11")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalOfflineVisualizer.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalOfflineVisualizer.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalOfflineVisualizer.Hotkey.ToggleOffLineVisualizer)> _
        Public Property Funct() As ExternalOfflineVisualizer.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalOfflineVisualizer.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalOfflineVisualizer.Hotkey.ToggleOffLineVisualizer
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("Toggle OffLine Visualizer")> ToggleOffLineVisualizer = 492
        <Description("User Camera 1")> UserCamera1 = 109
        <Description("User Camera 2")> UserCamera2 = 110
        <Description("User Camera 3")> UserCamera3 = 111
        <Description("User Camera 4")> UserCamera4 = 112
        <Description("User Camera 5")> UserCamera5 = 113
        <Description("User Camera 6")> UserCamera6 = 114
        <Description("User Camera 7")> UserCamera7 = 115
        <Description("User Camera 8")> UserCamera8 = 116
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF12")> _
Public Class ExternalEmulate2532
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "2532 emulation functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "Emulate 2532"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF12")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalEmulate2532.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalEmulate2532.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalEmulate2532.Hotkey.EmulateLatch2532)> _
        Public Property Funct() As ExternalEmulate2532.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalEmulate2532.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalEmulate2532.Hotkey.EmulateLatch2532
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("2532 Emulate Latch")> EmulateLatch2532 = 154
        <Description("2532 Emulate Flash")> EmulateFlash2532 = 155
        <Description("2532 Emulate UserKey")> EmulateUserKey2532 = 233
        <Description("Release Flash")> ReleaseFlash = 231
        <Description("2532 Next Page")> NextPage2532 = 234
        <Description("2532 Previous Page")> PreviousPage2532 = 235
        <Description("Toggle Latch/Flash")> ToggleLatchFlash = 316
    End Enum
End Class

<Guid("D5644026-BD84-482b-AF02-BB844852BF13")> _
Public Class ExternalDMXIN
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public ReadOnly Property Description() As String Implements IAction.Description
        Get
            Return "DMX-In functions available through the External Functions / External Hotkeys interface."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements IAction.Execute
        _host.PostLJMessage(_myData.State, _myData.Funct, 0) 'Returns 0
        Return True
    End Function

    Public ReadOnly Property Group() As String Implements IAction.Group
        Get
            Return "LJ External Hotkeys"
        End Get
    End Property

    Public Function Initialize(ByRef Host As IServices) As Boolean Implements IAction.Initialize
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements IAction.Name
        Get
            Return "DMX-In Functions"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements IAction.UniqueID
        Get
            Return New Guid("D5644026-BD84-482b-AF02-BB844852BF13")
        End Get
    End Property

    Public Property Data() As Object Implements IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, ExternalDMXIN.ActionData)
        End Set
    End Property

    ''' <summary>
    ''' ExternalSystem.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _function As ExternalDMXIN.Hotkey
        Friend _state As FunctionState

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Function"), _
            Description("Which function/hotkey to perform."), _
            DefaultValue(ExternalDMXIN.Hotkey.DMXInFunctionsDisable)> _
        Public Property Funct() As ExternalDMXIN.Hotkey
            Get
                Return _function
            End Get
            Set(ByVal value As ExternalDMXIN.Hotkey)
                _function = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Some External Functions/Hotkeys can be activated in both 'On' and 'Off' states (activate, deactivate function). Others perform the same function for both ('toggle' functions, for example)."), _
            DefaultValue(FunctionState.FunctionOn)> _
        Public Property State() As FunctionState
            Get
                Return _state
            End Get
            Set(ByVal value As FunctionState)
                _state = value
            End Set
        End Property

        Public Sub New()
            _function = ExternalDMXIN.Hotkey.DMXInFunctionsDisable
            _state = FunctionState.FunctionOn
        End Sub
    End Class

    Public Enum Hotkey
        <Description("DMXIn Functions Disable")> DMXInFunctionsDisable = 435
        <Description("DMXIn Functions Enable")> DMXInFunctionsEnable = 436
        <Description("DMXIn Functions Toggle")> DMXInFunctionsToggle = 437
    End Enum
End Class

Public Enum FunctionState As Integer
    <Description("Function On")> FunctionOn = 1006
    <Description("Function Off")> FunctionOff = 1007
End Enum