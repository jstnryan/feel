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
            'main.DisposeAboutWindow()
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
        Dim lblTitle As System.Windows.Forms.Label
        Me.lblVersion = New System.Windows.Forms.Label
        PictureBox1 = New System.Windows.Forms.PictureBox
        txtAcknowledgements = New System.Windows.Forms.TextBox
        lblTitle = New System.Windows.Forms.Label
        CType(PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        PictureBox1.Image = Global.Feel.My.Resources.Feel.feel_large
        PictureBox1.Location = New System.Drawing.Point(3, 3)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New System.Drawing.Size(128, 128)
        PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        '
        'txtAcknowledgements
        '
        txtAcknowledgements.Location = New System.Drawing.Point(82, 59)
        txtAcknowledgements.Multiline = True
        txtAcknowledgements.Name = "txtAcknowledgements"
        txtAcknowledgements.ReadOnly = True
        txtAcknowledgements.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        txtAcknowledgements.Size = New System.Drawing.Size(298, 202)
        txtAcknowledgements.TabIndex = 1
        txtAcknowledgements.TabStop = False
        txtAcknowledgements.Text = resources.GetString("txtAcknowledgements.Text")
        '
        'lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New System.Drawing.Font("UltraCondensedSansSerif", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblTitle.Location = New System.Drawing.Point(137, -2)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New System.Drawing.Size(88, 58)
        lblTitle.TabIndex = 2
        lblTitle.Text = "Feel"
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(253, 36)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(127, 13)
        Me.lblVersion.TabIndex = 5
        Me.lblVersion.Text = "Version: 2.0.0 (Apr. 2015)"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 273)
        Me.Controls.Add(txtAcknowledgements)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(lblTitle)
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
