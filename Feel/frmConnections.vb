Public Class frmConnections

    Private Sub PopulateConnections()
        lvConnections.Clear()

        For Each thing As Collections.Generic.KeyValuePair(Of Integer, clsConnection) In FeelConfig.Connections
            Dim thingy As clsConnection = thing.Value
            Dim item As Windows.Forms.ListViewItem = New Windows.Forms.ListViewItem
            item.Text = thingy.Name
            item.Checked = thingy.Enabled
            item.Tag = thing.Key
            lvConnections.Items.Add(item)
        Next
    End Sub

    Private Sub frmConfig_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        main.configMode(True) = False
    End Sub

    Private Sub frmConfig_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Save the new configuration
        main.SaveConfiguration()
    End Sub

    Private Sub frmConfig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        main.configMode = True

        ''TODO: Temporary clearing of fields (clear fields in designer; for production, remove these lines)
        btnRemoveConnection.Enabled = False
        txtConnectionName.Text = ""
        txtConnectionName.Enabled = False
        chkConnectionInputEnabled.Checked = False
        chkConnectionInputEnabled.Enabled = False
        cboConnectionInput.SelectedIndex = -1
        cboConnectionInput.Enabled = False
        chkConnectionOutputEnabled.Checked = False
        chkConnectionOutputEnabled.Enabled = False
        cboConnectionOutput.SelectedIndex = -1
        cboConnectionOutput.Enabled = False
        txtConnectionInitialization.Clear()
        txtConnectionInitialization.Enabled = False
        chkConnectionNoteOff.Checked = False
        chkConnectionNoteOff.Enabled = False

        'Populate input and output dropdowns
        Midi.InputDevice.UpdateInstalledDevices()
        For Each dev As Midi.InputDevice In Midi.InputDevice.InstalledDevices
            cboConnectionInput.Items.Add(dev.Name)
        Next
        Midi.OutputDevice.UpdateInstalledDevices()
        For Each dev As Midi.OutputDevice In Midi.OutputDevice.InstalledDevices
            cboConnectionOutput.Items.Add(dev.Name)
        Next

        'Read Configuration and properly populate/set controls
        PopulateConnections()
    End Sub

    Private Sub lvConnections_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvConnections.ItemChecked
        FeelConfig.Connections(CInt(e.Item.Tag)).Enabled = e.Item.Checked
    End Sub

    Private Sub lvConnections_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvConnections.SelectedIndexChanged
        If lvConnections.SelectedItems.Count = 0 Then
            btnRemoveConnection.Enabled = False
            'clear fields and disable
            txtConnectionName.Text = ""
            txtConnectionName.Enabled = False
            chkConnectionInputEnabled.Checked = False
            chkConnectionInputEnabled.Enabled = False
            cboConnectionInput.SelectedIndex = -1
            cboConnectionInput.Enabled = False
            chkConnectionOutputEnabled.Checked = False
            chkConnectionOutputEnabled.Enabled = False
            cboConnectionOutput.SelectedIndex = -1
            cboConnectionOutput.Enabled = False
            txtConnectionInitialization.Clear()
            txtConnectionInitialization.Enabled = False
            chkConnectionNoteOff.Checked = False
            chkConnectionNoteOff.Enabled = False
        Else
            btnRemoveConnection.Enabled = True
            'populate fields and enable
            With FeelConfig.Connections(CInt(lvConnections.SelectedItems(0).Tag))
                txtConnectionName.Text = .Name
                txtConnectionName.Enabled = True


                chkConnectionInputEnabled.Checked = .InputEnable
                chkConnectionInputEnabled.Enabled = True
                If Not (.Input = -1) Then
                    If (cboConnectionInput.Items.Count > .Input) Then
                        If (cboConnectionInput.Items(.Input).ToString = .InputName) Then
                            cboConnectionInput.SelectedIndex = .Input
                        Else
                            chkConnectionInputEnabled.Checked = False
                        End If
                    Else
                        chkConnectionInputEnabled.Checked = False
                    End If
                Else
                    '.InputEnable = False ''This should be taken care of automatically by below
                    chkConnectionInputEnabled.Checked = False
                End If
                cboConnectionInput.Enabled = chkConnectionInputEnabled.Checked


                chkConnectionOutputEnabled.Checked = .OutputEnable
                chkConnectionOutputEnabled.Enabled = True
                If Not (.Output = -1) Then
                    If (cboConnectionOutput.Items.Count > .Output) Then
                        If (cboConnectionOutput.Items(.Output).ToString = .OutputName) Then
                            cboConnectionOutput.SelectedIndex = .Output
                        Else
                            chkConnectionOutputEnabled.Checked = False
                        End If
                    Else
                        chkConnectionOutputEnabled.Checked = False
                    End If
                Else
                    '.OutputEnable = False ''This should be taken care of automatically by below
                    chkConnectionOutputEnabled.Checked = False
                End If
                cboConnectionOutput.Enabled = chkConnectionOutputEnabled.Checked


                txtConnectionInitialization.Text = .Init
                txtConnectionInitialization.Enabled = True
                chkConnectionNoteOff.Enabled = True
                chkConnectionNoteOff.Checked = .NoteOff
            End With
            'txtConnectionName.Focus()
        End If
    End Sub

    Private Sub btnAddConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddConnection.Click
        Dim x As Integer = 0
        Do
            If FeelConfig.Connections.ContainsKey(x) Then
                x += 1
            Else
                Exit Do
            End If
        Loop

        Dim conn As clsConnection = New clsConnection
        If (x > 0) Then
            conn.Name = conn.Name & " (" & x.ToString & ")"
        End If

        Dim item As Windows.Forms.ListViewItem = New Windows.Forms.ListViewItem
        item.Text = conn.Name
        item.Tag = x

        FeelConfig.Connections.Add(x, conn) 'This has to happen first, otherwise below will throw error
        lvConnections.Items.Add(item)
    End Sub

    Private Sub btnRemoveConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveConnection.Click
        FeelConfig.Connections.Remove(CInt(lvConnections.SelectedItems(0).Tag))
        lvConnections.Items.RemoveAt(lvConnections.SelectedItems(0).Index)
    End Sub

    Private Sub txtConnectionName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConnectionName.TextChanged
        If lvConnections.SelectedItems.Count > 0 Then
            lvConnections.SelectedItems(0).Text = txtConnectionName.Text
            FeelConfig.Connections.Item(CInt(lvConnections.SelectedItems(0).Tag)).Name = txtConnectionName.Text
        End If
    End Sub

    Private Sub chkConnectionInputEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConnectionInputEnabled.CheckedChanged
        If lvConnections.SelectedItems.Count > 0 Then
            FeelConfig.Connections(CInt(lvConnections.SelectedItems(0).Tag)).InputEnable = chkConnectionInputEnabled.Checked
            cboConnectionInput.Enabled = chkConnectionInputEnabled.Checked
        End If
    End Sub

    Private Sub cboConnectionInput_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConnectionInput.SelectedIndexChanged
        If lvConnections.SelectedItems.Count > 0 Then
            With FeelConfig.Connections(CInt(lvConnections.SelectedItems(0).Tag))
                .Input = cboConnectionInput.SelectedIndex
                .InputName = cboConnectionInput.Items(cboConnectionInput.SelectedIndex).ToString
            End With
        End If
    End Sub

    Private Sub chkConnectionOutputEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConnectionOutputEnabled.CheckedChanged
        If lvConnections.SelectedItems.Count > 0 Then
            FeelConfig.Connections(CInt(lvConnections.SelectedItems(0).Tag)).OutputEnable = chkConnectionOutputEnabled.Checked
            cboConnectionOutput.Enabled = chkConnectionOutputEnabled.Checked
        End If
    End Sub

    Private Sub cboConnectionOutput_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConnectionOutput.SelectedIndexChanged
        If lvConnections.SelectedItems.Count > 0 Then
            With FeelConfig.Connections(CInt(lvConnections.SelectedItems(0).Tag))
                .Output = cboConnectionOutput.SelectedIndex
                .OutputName = cboConnectionOutput.Items(cboConnectionOutput.SelectedIndex).ToString
            End With
        End If
    End Sub

    Private Sub txtConnectionInitialization_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConnectionInitialization.TextChanged
        If lvConnections.SelectedItems.Count > 0 Then
            FeelConfig.Connections(CInt(lvConnections.SelectedItems(0).Tag)).Init = txtConnectionInitialization.Text
        End If
    End Sub

    Private Sub chkConnectionNoteOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConnectionNoteOff.CheckedChanged
        If lvConnections.SelectedItems.Count > 0 Then
            FeelConfig.Connections(CInt(lvConnections.SelectedItems(0).Tag)).NoteOff = chkConnectionNoteOff.Checked
        End If
    End Sub
End Class
