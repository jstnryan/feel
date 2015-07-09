Partial Class CheckedGroupBox
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Component Designer generated code"
    ''' <summary>
    ''' Required method for Designer support - do not modify 
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        'Me.m_checkBox = New System.Windows.Forms.CheckBox()
        Me.m_checkBox = New ToolTipCheckBox
        Me.SuspendLayout()
        ' 
        ' m_checkBox
        ' 
        Me.m_checkBox.AutoSize = True
        Me.m_checkBox.Checked = True
        Me.m_checkBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.m_checkBox.Location = New System.Drawing.Point(0, 0)
        Me.m_checkBox.Name = "m_checkBox"
        Me.m_checkBox.Size = New System.Drawing.Size(104, 24)
        Me.m_checkBox.TabIndex = 0
        Me.m_checkBox.Text = "checkBox"
        Me.m_checkBox.UseVisualStyleBackColor = True

        'Me.m_checkBox.CheckStateChanged += New System.EventHandler(Me.checkBox_CheckStateChanged)
        AddHandler Me.m_checkBox.CheckStateChanged, New System.EventHandler(AddressOf Me.checkBox_CheckStateChanged)

        'Me.m_checkBox.CheckedChanged += New System.EventHandler(Me.checkBox_CheckedChanged)
        AddHandler Me.m_checkBox.CheckedChanged, New System.EventHandler(AddressOf Me.checkBox_CheckedChanged)
        ' 
        ' XGroupBox
        '
        'Me.ControlAdded += New System.Windows.Forms.ControlEventHandler(Me.XGroupBox_ControlAdded)
        AddHandler Me.ControlAdded, New System.Windows.Forms.ControlEventHandler(AddressOf Me.CheckedGroupBox_ControlAdded)

        Me.ResumeLayout(False)

    End Sub
#End Region

    ''Private
    'Public m_checkBox As System.Windows.Forms.CheckBox
    Private m_checkBox As ToolTipCheckBox

    Private Class ToolTipCheckBox
        Inherits System.Windows.Forms.CheckBox

        Private _tooltip As String
        Private _tt As System.Windows.Forms.ToolTip = New System.Windows.Forms.ToolTip

        ''' <summary>
        ''' ToolTip to be shown when user hovers mouse over the CheckBox.
        ''' </summary>
        Friend Property ToolTip() As String
            Get
                Return Me._tooltip
            End Get
            Set(ByVal value As String)
                Me._tooltip = value
                Me._tt.SetToolTip(Me, Me._tooltip)
            End Set
        End Property
    End Class
End Class