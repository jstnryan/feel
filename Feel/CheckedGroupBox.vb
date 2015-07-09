﻿Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles


''' <summary>
''' CheckedGroupBox is a GroupBox with an embeded CheckBox.
''' </summary>
<ToolboxBitmap(GetType(CheckedGroupBox), "CheckedGroupBox.bmp")> _
Partial Public Class CheckedGroupBox
    Inherits GroupBox
    ' Constants
    Private Const CHECKBOX_X_OFFSET As Integer = 14 '10
    Private Const CHECKBOX_Y_OFFSET As Integer = -2

    ' Members
    Private m_bDisableChildrenIfUnchecked As Boolean

    ''' <summary>
    ''' Public constructor.
    ''' </summary>
    Public Sub New()
        Me.InitializeComponent()
        Me.m_bDisableChildrenIfUnchecked = False
        Me.m_checkBox.Parent = Me
        Me.m_checkBox.Location = New System.Drawing.Point(CHECKBOX_X_OFFSET, CHECKBOX_Y_OFFSET)
        Me.Checked = True

        If VisualStyleInformation.IsEnabledByUser Then
            ' Set the color of the CheckBox's text to the color of the label in a standard groupbox control.
            Dim vsr As New VisualStyleRenderer(VisualStyleElement.Button.GroupBox.Normal)
            Dim groupBoxTextColor As Color = vsr.GetColor(ColorProperty.TextColor)
            Me.m_checkBox.ForeColor = groupBoxTextColor
        End If
    End Sub

#Region "Properties"
    ''' <summary>
    ''' The text associated with the control.
    ''' </summary>
    Public Overrides Property Text() As String
        Get
            If Me.Site IsNot Nothing AndAlso Me.Site.DesignMode = True Then
                ' Design-time mode
                Return Me.m_checkBox.Text
            Else
                ' Run-time
                'return " "; // Set the text of the GroupBox to a space, so the gap appears before the CheckBox.
                Return "      " + Me.m_checkBox.Text
            End If
        End Get
        Set(ByVal value As String)
            MyBase.Text = " "
            ' Set the text of the GroupBox to a space, so the gap appears before the CheckBox.
            Me.m_checkBox.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the component is checked or not.
    ''' </summary>
    <Description("Indicates whether the component is checked or not.")> _
    <Category("Appearance")> _
    <DefaultValue(True)> _
    Public Property Checked() As Boolean
        Get
            Return Me.m_checkBox.Checked
        End Get
        Set(ByVal value As Boolean)
            'If Me.m_checkBox.Checked <> value Then
            Me.m_checkBox.Checked = value
            'End If
        End Set
    End Property

    ''' <summary>
    ''' Indicates the state of the component.
    ''' </summary>
    <Description("Indicates the state of the component.")> _
    <Category("Appearance")> _
    <DefaultValue(CheckState.Checked)> _
    Public Property CheckState() As CheckState
        Get
            Return Me.m_checkBox.CheckState
        End Get
        Set(ByVal value As CheckState)
            'If Me.m_checkBox.CheckState <> value Then
            Me.m_checkBox.CheckState = value
            'End If
        End Set
    End Property

    ''' <summary>
    ''' Determines if child controls of the GroupBox are disabled when the CheckBox is unchecked.
    ''' </summary>
    <Description("Determines if child controls of the GroupBox are disabled when the CheckBox is unchecked.")> _
    <Category("Appearance")> _
    <DefaultValue(True)> _
    Public Property DisableChildrenIfUnchecked() As Boolean
        Get
            Return Me.m_bDisableChildrenIfUnchecked
        End Get
        Set(ByVal value As Boolean)
            'If Not (Me.m_bDisableChildrenIfUnchecked = value) Then
            Me.m_bDisableChildrenIfUnchecked = value
            'End If
        End Set
    End Property

    ''' <summary>
    ''' ToolTip to be shown when user hovers mouse over the CheckBox.
    ''' </summary>
    <Description("Sets the ToolTip text to be shown for CheckBox.")> _
    <DefaultValue("")> _
    Public Property CheckBoxToolTip() As String
        Get
            Return Me.m_checkBox.ToolTip
        End Get
        Set(ByVal value As String)
            Me.m_checkBox.ToolTip = value
        End Set
    End Property
#End Region

#Region "Event Handlers"
    ''' <summary>
    ''' Occurs whenever the Checked property of the CheckBox is changed.
    ''' </summary>
    <Description("Occurs whenever the Checked property of the CheckBox is changed.")> _
    Public Event CheckedChanged As EventHandler

    ''' <summary>
    ''' Occurs whenever the CheckState property of the CheckBox is changed.
    ''' </summary>
    <Description("Occurs whenever the CheckState property of the CheckBox is changed.")> _
    Public Event CheckStateChanged As EventHandler

    ''' <summary>
    ''' Raises the System.Windows.Forms.CheckBox.checkBox_CheckedChanged event.
    ''' </summary>
    ''' <param name="e">An System.EventArgs that contains the event data.</param>
    Protected Overridable Sub OnCheckedChanged(ByVal e As EventArgs)
    End Sub

    ''' <summary>
    ''' Raises the System.Windows.Forms.CheckBox.CheckStateChanged event.
    ''' </summary>
    ''' <param name="e">An System.EventArgs that contains the event data.</param>
    Protected Overridable Sub OnCheckStateChanged(ByVal e As EventArgs)
    End Sub
#End Region

#Region "Events"
    ''When uncommented, these lines cause unexpected disabling of child controls upon form creation
    Private Sub checkBox_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        'If Not (Me.m_bDisableChildrenIfUnchecked) Then
        '    Dim bEnabled As Boolean = Me.m_checkBox.Checked
        '    For Each control As Control In Me.Controls
        '        If (control IsNot Me.m_checkBox) Then
        '            control.Enabled = bEnabled
        '        End If
        '    Next
        'End If

        RaiseEvent CheckedChanged(sender, e)
    End Sub

    Private Sub checkBox_CheckStateChanged(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent CheckStateChanged(sender, e)
    End Sub

    Private Sub CheckedGroupBox_ControlAdded(ByVal sender As Object, ByVal e As ControlEventArgs)
        'If Not (Me.m_bDisableChildrenIfUnchecked) Then
        '    e.Control.Enabled = Me.Checked
        'End If
    End Sub
#End Region
End Class