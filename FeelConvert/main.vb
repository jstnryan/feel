''FEELCONVERT
Module main
    ''Version 1
    Private config1 As clsConfig_1
    Private config2 As Feel.clsConfig

    Public Sub main()
        Dim openDialog As New Windows.Forms.OpenFileDialog()
        openDialog.InitialDirectory = Windows.Forms.Application.StartupPath & "\user\"
        openDialog.Filter = "Feel configuration (v1) (*.config)|*.config|Feel configuration (v2+) (*.feel)|*.feel|All files (*.*)|*.*"
        If (openDialog.ShowDialog = Windows.Forms.DialogResult.OK) Then
            LoadConfiguration1(openDialog.FileName)
            If (config1 Is Nothing) Then
                MessageBox.Show("Unable to open config file!", "Feel: Config File Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("success!")
            End If
        End If
        openDialog.Dispose()
        'openDialog = Nothing
    End Sub

    ''Version 1
    Private Sub LoadConfiguration1(ByVal file As String)
        Dim fs As IO.FileStream
        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        Try
            fs = New IO.FileStream(file, IO.FileMode.Open, IO.FileAccess.Read)
            config1 = CType(bf.Deserialize(fs), clsConfig_1)
            fs.Close()
        Catch fileEx As IO.FileNotFoundException
            MessageBox.Show("File not found!", "Feel: File System Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch directoryEx As IO.DirectoryNotFoundException
            MessageBox.Show("Directory not found!", "Feel: File System Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Catch ex As Exception
            MessageBox.Show("Unexpected error trying to read Feel configuration file." & vbCrLf & vbCrLf & ex.Message, "Feel: Unknown Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            'fs.Dispose()
            fs = Nothing
            bf = Nothing
        End Try
    End Sub
End Module
