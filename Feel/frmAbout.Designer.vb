<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)

            ''Singleon Pattern: http://www.codeproject.com/KB/vb/Simple_Singleton_Forms.aspx
            main.aboutForm = Nothing
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim PictureBox1 As System.Windows.Forms.PictureBox
        Dim txtAcknowledgements As System.Windows.Forms.TextBox
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Dim Label1 As System.Windows.Forms.Label
        Dim Label2 As System.Windows.Forms.Label
        Dim LinkLabel1 As System.Windows.Forms.LinkLabel
        Me.lblVersion = New System.Windows.Forms.Label
        PictureBox1 = New System.Windows.Forms.PictureBox
        txtAcknowledgements = New System.Windows.Forms.TextBox
        Label1 = New System.Windows.Forms.Label
        Label2 = New System.Windows.Forms.Label
        LinkLabel1 = New System.Windows.Forms.LinkLabel
        CType(PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        PictureBox1.Image = Global.Feel.My.Resources.Feel.feel_large
        PictureBox1.Location = New System.Drawing.Point(-11, 12)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New System.Drawing.Size(128, 128)
        PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        '
        'txtAcknowledgements
        '
        txtAcknowledgements.Location = New System.Drawing.Point(101, 89)
        txtAcknowledgements.Multiline = True
        txtAcknowledgements.Name = "txtAcknowledgements"
        txtAcknowledgements.ReadOnly = True
        txtAcknowledgements.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        txtAcknowledgements.Size = New System.Drawing.Size(279, 172)
        txtAcknowledgements.TabIndex = 1
        txtAcknowledgements.TabStop = False
        txtAcknowledgements.Text = resources.GetString("txtAcknowledgements.Text")
        '
        'Label1
        '
        Label1.AutoSize = True
        Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Label1.Location = New System.Drawing.Point(98, 12)
        Label1.Name = "Label1"
        Label1.Size = New System.Drawing.Size(63, 16)
        Label1.TabIndex = 2
        Label1.Text = "Feel 2.0"
        '
        'Label2
        '
        Label2.AutoSize = True
        Label2.Location = New System.Drawing.Point(98, 37)
        Label2.Name = "Label2"
        Label2.Size = New System.Drawing.Size(62, 13)
        Label2.TabIndex = 3
        Label2.Text = "Justin Ryan"
        '
        'LinkLabel1
        '
        LinkLabel1.AutoSize = True
        LinkLabel1.Location = New System.Drawing.Point(166, 37)
        LinkLabel1.Name = "LinkLabel1"
        LinkLabel1.Size = New System.Drawing.Size(134, 13)
        LinkLabel1.TabIndex = 4
        LinkLabel1.TabStop = True
        LinkLabel1.Text = "justin@lb1productions.com"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(98, 59)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(129, 13)
        Me.lblVersion.TabIndex = 5
        Me.lblVersion.Text = "Version: 2.0.0 (Feb. 2014)"
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 273)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(LinkLabel1)
        Me.Controls.Add(Label2)
        Me.Controls.Add(Label1)
        Me.Controls.Add(txtAcknowledgements)
        Me.Controls.Add(PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmAbout"
        Me.Text = "Feel: About"
        CType(PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblVersion As System.Windows.Forms.Label
End Class
