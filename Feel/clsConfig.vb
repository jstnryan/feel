﻿'' For all searchable string indexes:
''  WW = (hex)Page Number [00 = All Pages (Default), 01 = Page 1, FF = Page 255]
''  XX = (hex)Type + Channel [8n = Note On (Not used for storing actions), 9n = Note Off, Bn = Control Change]
''  YY = (hex)Pitch/Control
''  ZZ = (hex)Velocity/Value

<Serializable()> _
Public Class clsConfig
    'MIDI Connections (Virtual ports & MIDI devices)
    Public Connections As Collections.Generic.Dictionary(Of Integer, clsConnection)

    Public WmEnable As Boolean          'Enable Windows Messages to LightJockey
    Public IgnoreEvents As Boolean      'Ignore events raised by devices/connections while still establishing other connections
    Public DmxinEnable As Boolean       'Enable DMX-In to LightJockey
    Public DmxoverEnable As Boolean     'Enable DMX-Override to LightJockey

    Public Sub New()
        WmEnable = False
        IgnoreEvents = False
        DmxinEnable = False
        DmxoverEnable = False
        Connections = New Collections.Generic.Dictionary(Of Integer, clsConnection)
    End Sub
End Class

<Serializable()> _
Public Class clsConnection
    Public Enabled As Boolean
    Public Name As String
    Public Input As Integer
    Public InputName As String
    Public InputEnable As Boolean
    Public Output As Integer
    Public OutputName As String
    Public OutputEnable As Boolean
    Public Init As String
    Public NoteOff As Boolean '"Note On, Velocity 0" emulate "Note Off"

    <NonSerialized()> _
    Private _pageCurrent As Byte
    <NonSerialized()> _
    Private _pagePrevious As Byte

    Public Property PageCurrent() As Byte
        Get
            Return _pageCurrent
        End Get
        Set(ByVal value As Byte)
            _pagePrevious = _pageCurrent
            _pageCurrent = value
            ''TODO: Call RedrawControls() from here, someday..
        End Set
    End Property
    Public ReadOnly Property PagePrevious() As Byte
        Get
            Return _pagePrevious
        End Get
    End Property

    'String is format "XXYY"
    Public Control As Collections.Generic.Dictionary(Of String, clsControl)

    Public Sub New()
        Enabled = False
        Name = "New Connection"
        Input = -1
        InputName = ""
        InputEnable = False
        Output = -1
        OutputName = ""
        OutputEnable = False
        Init = "#Comment (Lines beginning with #)" & vbCrLf & vbCrLf & "## MIDI Message Examples" & vbCrLf & "## One message per line" & vbCrLf & "# Sysex: APC20 Mode 2" & vbCrLf & "#F0 47 00 7B 60 00 04 42 08 02 06 F7" & vbCrLf & "# Sysex: APC40 Mode 0" & vbCrLf & "#F0 47 00 73 60 00 04 40 08 02 06 F7" & vbCrLf & "# Note 74 Off, Channel 4, Velocity 127" & vbCrLf & "#83 4A FF" & vbCrLf & "# Note 0 On, Channel 16, Velocity 1" & vbCrLf & "#9F 00 01"

        _pageCurrent = 0
        _pagePrevious = 0

        Control = New Collections.Generic.Dictionary(Of String, clsControl)
    End Sub

    <Runtime.Serialization.OnDeserialized()> _
    Private Sub SetValues(ByVal context As System.Runtime.Serialization.StreamingContext)
        _pageCurrent = 0
        _pagePrevious = 0
    End Sub
End Class

<Serializable()> _
Public Class clsControl
    Public Paged As Boolean
    'String is format "WW"
    Public Page As Collections.Generic.Dictionary(Of Byte, clsControlPage)

    Public Sub New()
        Paged = False
        Page = New Collections.Generic.Dictionary(Of Byte, clsControlPage)
    End Sub
End Class

<Serializable()> _
Public Class clsControlPage
    Public InitialState As String
    Public ControlGroup As Byte
    Public Behavior As Byte
    <NonSerialized()> Public IsActive As Boolean
    <NonSerialized()> Private _CurrentState As String

    Public Actions As Collections.Generic.List(Of clsAction)
    Public ActionsOff As Collections.Generic.List(Of clsAction)

    Public Property CurrentState() As String
        Get
            If (String.IsNullOrEmpty(_CurrentState)) Then
                Return InitialState
            Else
                Return _CurrentState
            End If
        End Get
        Set(ByVal value As String)
            _CurrentState = value
        End Set
    End Property

    Public Sub New()
        InitialState = ""
        ControlGroup = 0
        Behavior = 0
        IsActive = False
        _CurrentState = ""
        Actions = New Collections.Generic.List(Of clsAction)
        ActionsOff = New Collections.Generic.List(Of clsAction)
    End Sub

    <Runtime.Serialization.OnDeserialized()> _
    Private Sub SetValues(ByVal context As System.Runtime.Serialization.StreamingContext)
        IsActive = False
        _CurrentState = ""
    End Sub
End Class

<Serializable()> _
Public Class clsAction
    Public Name As String
    Public Enabled As Boolean
    'TODO: This is going to have to be temporary, otherwise the list of different
    ' types of actions is going to become HUGE
    Public Type As Integer
    Public Action As Object

    Public Sub New()
        Name = "New Action"
        Enabled = False
        Type = 0
        Action = Nothing
    End Sub
End Class

''A shell class for de-/serializing copy/pasted actions
<Serializable()> _
Public Class clsCopiedActions
    Public Actions As Collections.Generic.List(Of clsAction)
    Public ActionsOff As Collections.Generic.List(Of clsAction)

    Sub New()
        Actions = New Collections.Generic.List(Of clsAction)
        ActionsOff = New Collections.Generic.List(Of clsAction)
    End Sub
End Class

#Region "Previous Class Versions"
''REMOVED:
'' FingersEnable
'' FingersPort
''ADDED:
'' IgnoreEvents
<Serializable()> _
Public Class clsConfig_01
    'MIDI Connections (Virtual ports & MIDI devices)
    Public Connections As Collections.Generic.Dictionary(Of Integer, clsConnection)

    Public WmEnable As Boolean          'Enable Windows Messages to LightJockey
    Public FingersEnable As Boolean     'Enable Serial to LightJockey
    Public FingersPort As Integer       'Serial interface Port #
    Public DmxinEnable As Boolean       'Enable DMX-In to LightJockey
    Public DmxoverEnable As Boolean     'Enable DMX-Override to LightJockey

    Public Sub New()
        WmEnable = False
        FingersEnable = False
        FingersPort = -1
        DmxinEnable = False
        DmxoverEnable = False
        Connections = New Collections.Generic.Dictionary(Of Integer, clsConnection)
    End Sub
End Class
#End Region