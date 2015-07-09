Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Feel.ActionInterface

<Guid("287BC6EB-94D9-481f-891C-11D63FEC4200")> _
Public Class WindowShowHide
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, WindowShowHide.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Shows or hides the selected fixture control window. (One or more fixtures must be selected.)"
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(104, If(_myData._state = State.Hide, If(_myData._window = Window.All, 18, _myData._window + 1), _myData._window), _myData._state)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ: Fixture Controls"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Fixture Control Show/Hide"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("287BC6EB-94D9-481f-891C-11D63FEC4200")
        End Get
    End Property

    ''' <summary>
    ''' WindowShowHide.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _window As WindowShowHide.Window
        Friend _state As WindowShowHide.State

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Window"), _
            Description("Which fixture control window to hide or show."), _
            DefaultValue(WindowShowHide.Window.Intensity)> _
        Public Property Window() As WindowShowHide.Window
            Get
                Return _window
            End Get
            Set(ByVal value As WindowShowHide.Window)
                _window = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("Hide or show window." & vbCrLf & vbCrLf & "Exclusive hides all other controls."), _
            DefaultValue(WindowShowHide.State.Show)> _
        Public Property State() As WindowShowHide.State
            Get
                Return _state
            End Get
            Set(ByVal value As WindowShowHide.State)
                _state = value
            End Set
        End Property

        Sub New()
            _window = WindowShowHide.Window.Intensity
            _state = WindowShowHide.State.Show
        End Sub
    End Class

    Public Enum State As Byte
        <Description("Hide")> Hide = 0
        <Description("Show")> Show = 1
        <Description("Show Exclusive")> Exclusive = 2
    End Enum

    Public Enum Window As Integer
        <Description("ALL FIXTURE CONTROLS")> All = 30 '18
        <Description("Intensity")> Intensity = 1 '2
        <Description("Movement")> Movement = 3 '4
        <Description("Color")> Color = 5 '6
        <Description("Gobo")> Gobo = 7 '8
        <Description("Beam")> Beam = 9 '9
        <Description("Effects")> Effects = 11 '12
        <Description("Lamp")> Lamp = 13 '14
        <Description("Levels/Extended")> Levels = 15 '16
        <Description("Movement Macro")> MMacro = 19 '20
        ''The following two do not use the same Show/Exclusive pattern, but should work here
        'TODO: confirm
        <Description("Select Followspot Setting")> SelFollowspot = 40 '41, no exclusive
        <Description("Edit FollowSpot Settings")> EditFollowspot = 45 'No known hide equivalent
    End Enum
End Class

<Guid("287BC6EB-94D9-481f-891C-11D63FEC4201")> _
Public Class WindowToggle
    Implements IAction

    Private _myData As ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, WindowToggle.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Toggles (hidden/visible) the selected fixture control window. (One or more fixtures must be selected.)"
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(104, _myData._window, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ: Fixture Controls"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Fixture Control Toggle"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("287BC6EB-94D9-481f-891C-11D63FEC4201")
        End Get
    End Property

    ''' <summary>
    ''' WindowToggle.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _window As WindowToggle.Window

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Window"), _
            Description("Which fixture control window to toggle."), _
            DefaultValue(WindowToggle.Window.All)> _
        Public Property Window() As WindowToggle.Window
            Get
                Return _window
            End Get
            Set(ByVal value As WindowToggle.Window)
                _window = value
            End Set
        End Property

        Sub New()
            _window = WindowToggle.Window.All
        End Sub
    End Class

    Public Enum Window As Integer
        <Description("ALL FIXTURE CONTROLS")> All = 39
        <Description("Intensity")> Intensity = 32
        <Description("Movement")> Movement = 33
        <Description("Color")> Color = 34
        <Description("Gobo")> Gobo = 35
        <Description("Beam")> Beam = 36
        <Description("Effects")> Effects = 37
        <Description("Levels/Extended")> Levels = 38
        <Description("Position Options")> PosOptions = 47
        <Description("Movement Macro")> MMacro = 48
    End Enum
End Class

<Guid("287BC6EB-94D9-481f-891C-11D63FEC4202")> _
Public Class DeselectAllFixtures
    Implements IAction

    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            'nothing to save
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Deselects all fixture icons."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(104, 25, 0)
        '_host.PostLJMessage(104, 51, 0)
        '_host.PostLJMessage(104, 53, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ: Fixture Controls"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Deselect All Fixtures"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("287BC6EB-94D9-481f-891C-11D63FEC4202")
        End Get
    End Property
End Class

<Guid("287BC6EB-94D9-481f-891C-11D63FEC4203")> _
Public Class SelectAllFixtures
    Implements IAction

    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return False
        End Get
        Set(ByVal value As Object)
            'nothing to save
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Selects all fixture icons."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(104, 31, 0)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ: Fixture Controls"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Select All Fixtures"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("287BC6EB-94D9-481f-891C-11D63FEC4203")
        End Get
    End Property
End Class

<Guid("287BC6EB-94D9-481f-891C-11D63FEC4204")> _
Public Class SelectAllFixturesOfType
    Implements IAction

    Private _myData As SelectAllFixturesOfType.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, SelectAllFixturesOfType.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Selects all fixtures of the same type, and sets the master fixture."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(104, 49, _myData.Fixture - 1)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ: Fixture Controls"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New SelectAllFixturesOfType.ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Select All Fixtures Of Type"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("287BC6EB-94D9-481f-891C-11D63FEC4204")
        End Get
    End Property

    ''' <summary>
    ''' SelectAllFixturesOfType.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _fixture As Integer

        <DisplayName("Master Fixture"), _
            Description("The fixture number of the fixture type to be selected. This is the master fixture of the selection."), _
            DefaultValue(1)> _
        Public Property Fixture() As Integer
            Get
                Return _fixture
            End Get
            Set(ByVal value As Integer)
                _fixture = value
            End Set
        End Property

        Sub New()
            _fixture = 1
        End Sub
    End Class
End Class

<Guid("287BC6EB-94D9-481f-891C-11D63FEC4205")> _
Public Class DeselectAllFixturesOfType
    Implements IAction

    Private _myData As DeselectAllFixturesOfType.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, DeselectAllFixturesOfType.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Deselects all fixtures of the same type."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(104, 50, _myData.Fixture - 1)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ: Fixture Controls"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New DeselectAllFixturesOfType.ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Select All Fixtures Of Type"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("287BC6EB-94D9-481f-891C-11D63FEC4205")
        End Get
    End Property

    ''' <summary>
    ''' SelectAllFixturesOfType.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _fixture As Integer

        <DisplayName("Fixture"), _
            Description("The fixture number of the fixture type to be deselected."), _
            DefaultValue(1)> _
        Public Property Fixture() As Integer
            Get
                Return _fixture
            End Get
            Set(ByVal value As Integer)
                _fixture = value
            End Set
        End Property

        Sub New()
            _fixture = 1
        End Sub
    End Class
End Class

<Guid("287BC6EB-94D9-481f-891C-11D63FEC4206")> _
Public Class DeselectAllExcept
    Implements IAction

    Private _myData As DeselectAllExcept.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, DeselectAllExcept.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Deselects all fixtures except for the specified fixture number."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(104, 52, _myData.Fixture - 1)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ: Fixture Controls"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New DeselectAllExcept.ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Deselect All Fixtures Except"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("287BC6EB-94D9-481f-891C-11D63FEC4206")
        End Get
    End Property

    ''' <summary>
    ''' DeselectAllExcept.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _fixture As Integer

        <DisplayName("Fixture"), _
            Description("The fixture number to remain selected. All others will be deselected."), _
            DefaultValue(1)> _
        Public Property Fixture() As Integer
            Get
                Return _fixture
            End Get
            Set(ByVal value As Integer)
                _fixture = value
            End Set
        End Property

        Sub New()
            _fixture = 1
        End Sub
    End Class
End Class

<Guid("287BC6EB-94D9-481f-891C-11D63FEC4207")> _
Public Class FixtureAttributeState
    Implements IAction

    Private _myData As FixtureAttributeState.ActionData
    Private Shared _host As IServices

    Public Property Data() As Object Implements Feel.ActionInterface.IAction.Data
        Get
            Return _myData
        End Get
        Set(ByVal value As Object)
            _myData = DirectCast(value, FixtureAttributeState.ActionData)
        End Set
    End Property

    Public ReadOnly Property Description() As String Implements Feel.ActionInterface.IAction.Description
        Get
            Return "Changes the Off/Snap/Fade state of the selected fixture attribute for the currently selected fixtures." & vbCrLf & vbCrLf & "Individual controls may affect other controls if they share the same DMX channel. Not all fixtures support all features."
        End Get
    End Property

    Public Function Execute(ByVal Device As String, ByVal Type As Byte, ByVal Channel As Byte, ByVal NoteCon As Byte, ByVal VelVal As Byte) As Boolean Implements Feel.ActionInterface.IAction.Execute
        _host.PostLJMessage(If(_myData._update, 110, 109), _myData._attribute, _myData._state)
        Return True ''No error checking
    End Function

    Public ReadOnly Property Group() As String Implements Feel.ActionInterface.IAction.Group
        Get
            Return "LJ: Fixture Controls"
        End Get
    End Property

    Public Function Initialize(ByRef Host As Feel.ActionInterface.IServices) As Boolean Implements Feel.ActionInterface.IAction.Initialize
        If Not (Host.Licensing_CodeMeter_Validate(10, 201000, 1)) Then Return False
        _myData = New FixtureAttributeState.ActionData
        _host = Host
        Return True
    End Function

    Public ReadOnly Property Name() As String Implements Feel.ActionInterface.IAction.Name
        Get
            Return "Change Fixture Attribute State"
        End Get
    End Property

    Public ReadOnly Property UniqueID() As System.Guid Implements Feel.ActionInterface.IAction.UniqueID
        Get
            Return New Guid("287BC6EB-94D9-481f-891C-11D63FEC4207")
        End Get
    End Property

    ''' <summary>
    ''' FixtureControlChange.ActionData
    ''' </summary>
    ''' <remarks>Stores the properties for this action.</remarks>
    <Serializable()> _
    Public Class ActionData
        Friend _attribute As FixtureAttributeState.Attribute
        Friend _state As FixtureAttributeState.State
        Friend _update As Boolean

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("Attribute"), _
            Description("The fixture control attribute to change."), _
            DefaultValue(FixtureAttributeState.Attribute.Intensity)> _
        Public Property Attribute() As FixtureAttributeState.Attribute
            Get
                Return _attribute
            End Get
            Set(ByVal value As FixtureAttributeState.Attribute)
                _attribute = value
            End Set
        End Property

        <TypeConverter(GetType(Feel.EnumDescriptionConverter)), _
            DisplayName("State"), _
            Description("The state to change the selected attribute to (Off/Snap/Fade)."), _
            DefaultValue(FixtureAttributeState.State.Off)> _
        Public Property State() As FixtureAttributeState.State
            Get
                Return _state
            End Get
            Set(ByVal value As FixtureAttributeState.State)
                _state = value
            End Set
        End Property

        <DisplayName("Update Controls"), _
            Description("Whether to update the visible controls."), _
            DefaultValue(False)> _
        Public Property Update() As Boolean
            Get
                Return _update
            End Get
            Set(ByVal value As Boolean)
                _update = value
            End Set
        End Property

        Sub New()
            _attribute = FixtureAttributeState.Attribute.Intensity
            _state = FixtureAttributeState.State.Off
            _update = False
        End Sub
    End Class

    Public Enum Attribute
        <Description("Intensity")> Intensity = 1
        <Description("Shutter")> Shutter = 2
        <Description("Strobe")> Strobe = 3
        <Description("Pan")> Pan = 4
        <Description("Tilt")> Tilt = 5
        <Description("Color 1")> Color1 = 6
        <Description("Color 2")> Color2 = 7
        <Description("Color 3")> Color3 = 8
        <Description("Color 4")> Color4 = 9
        <Description("Gobo 1")> Gobo1 = 10
        <Description("Gobo 2")> Gobo2 = 11
        <Description("Lamp")> Lamp = 12
        <Description("Color Strobe")> ColorStrobe = 13
        <Description("Gobo Strobe")> GoboStrobe = 14
        <Description("Effect 1")> Effect1 = 15
        <Description("Iris")> Iris = 16
        <Description("Focus")> Focus = 17
        <Description("Halogen")> Halogen = 18
        <Description("Cyan")> Cyan = 19
        <Description("Magenta")> Magenta = 20
        <Description("Yellow")> Yellow = 21
        <Description("Beam 1")> Beam1 = 22
        <Description("Beam 2")> Beam2 = 23
        <Description("Zoom")> Zoom = 24
        <Description("Mirror 1")> Mirror1 = 25
        <Description("Mirror 2")> Mrror2 = 26
        <Description("Angle")> Angle = 27
        <Description("Level")> Level = 28
        <Description("Shaker 1")> Shaker1 = 29
        <Description("Gobo Rotation")> GoboRotation = 30
        <Description("Pan/Tilt Settings/Speed")> PanTiltSettingsSpeed = 31
        <Description("Effect Settings/Speed")> EffectSettingsSpeed = 32
        <Description("Gobo 3")> Gobo3 = 33
        <Description("Frost")> Frost = 34
        <Description("Reset")> Reset = 35
        <Description("Shutter Rotation")> ShutterRotation = 36
        <Description("Shutter 1A")> Shutter1A = 37
        <Description("Shutter 1B")> Shutter1B = 38
        <Description("Shutter 2A")> Shutter2A = 39
        <Description("Shutter 2B")> Shutter2B = 40
        <Description("Shutter 3A")> Shutter3A = 41
        <Description("Shutter 3B")> Shutter3B = 42
        <Description("Shutter 4A")> Shutter4A = 43
        <Description("Shutter 4B")> Shutter4B = 44
        <Description("Shutter All")> ShutterAll = 45
        <Description("Color Speed 1")> ColorSpeed1 = 46
        <Description("Color Speed 2")> ColorSpeed2 = 47
        <Description("FID Speed")> FIDSpeed = 48
        <Description("Mirror 3 Fade")> Mirror3Fade = 49
        <Description("Mirror 4 Fade")> Mirror4Fade = 50
        <Description("Mirror Rotation")> MirrorRotation = 51
        <Description("SA")> SA = 52
        <Description("Color Scroll")> ColorScroll = 53
        <Description("Smoke")> Smoke = 54
        <Description("Fan")> Fan = 55
        <Description("Lamp On")> LampOn = 56
        <Description("Lamp Off")> LampOff = 57
        <Description("Gobo 4")> Gobo4 = 58
        <Description("Effect 2")> Effect2 = 59
        <Description("Move 1")> Move1 = 60
        <Description("Red")> Red = 61
        <Description("Green")> Green = 62
        <Description("Blue")> Blue = 63
        <Description("Tilt 2")> Tilt2 = 64
        <Description("Effect 3")> Effect3 = 65
        <Description("CTC")> CTC = 66
        <Description("Effect 4")> Effect4 = 67
        <Description("Gobo Param 1")> GoboParam1 = 68
        <Description("Gobo Param 2")> GoboParam2 = 69
        <Description("Param Block")> ParamBlock = 70
        <Description("Random CMY")> RandomCMY = 71
        <Description("Duration")> Duration = 72
        <Description("Strobe Effect")> StrobeEffect = 73
    End Enum

    Public Enum State
        <Description("Off")> Off = 0
        <Description("Snap")> Snap = 1
        <Description("Fade")> Fade = 2
    End Enum
End Class