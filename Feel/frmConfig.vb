Public Class frmConfig

    Private Sub PopulateConnections()
        lvConnections.Clear()

        For Each thing As Collections.Generic.KeyValuePair(Of String, clsConnection) In Configuration.Connections
            Dim thingy As clsConnection = thing.Value
            Dim item As Windows.Forms.ListViewItem = New Windows.Forms.ListViewItem
            item.Text = thingy.Name
            item.Checked = thingy.Enabled
            item.Tag = CStr(thing.Key)
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

        ''TODO: Temporary clearing of fields (clear fields in designer for production, remove these lines)
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
        For Each dev As Midi.InputDevice In Midi.InputDevice.InstalledDevices
            cboConnectionInput.Items.Add(dev.Name)
        Next
        For Each dev As Midi.OutputDevice In Midi.OutputDevice.InstalledDevices
            cboConnectionOutput.Items.Add(dev.Name)
        Next

        'Read Configuration and properly populate/set controls
        PopulateConnections()
        chkWindowsMessages.Checked = Configuration.WmEnable
        chkFingers.Checked = Configuration.FingersEnable
        cboFingersPort.SelectedIndex = Configuration.FingersPort + 1
        cboFingersPort.Enabled = Configuration.FingersEnable
        chkDmxin.Checked = Configuration.DmxinEnable
        chkDmxover.Checked = Configuration.DmxoverEnable
    End Sub

    Private Sub lvConnections_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvConnections.ItemChecked
        Configuration.Connections(e.Item.Tag.ToString).Enabled = e.Item.Checked
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
            With Configuration.Connections(lvConnections.SelectedItems(0).Tag.ToString)
                txtConnectionName.Text = .Name
                txtConnectionName.Enabled = True
                chkConnectionInputEnabled.Checked = .InputEnable
                chkConnectionInputEnabled.Enabled = True
                If (cboConnectionInput.Items.Count > .Input) Then cboConnectionInput.SelectedIndex = .Input
                cboConnectionInput.Enabled = True
                chkConnectionOutputEnabled.Checked = .OutputEnable
                chkConnectionOutputEnabled.Enabled = True
                'TODO: this line causes an invalid index error if devices have been removed from the system
                If (cboConnectionOutput.Items.Count > .Output) Then cboConnectionOutput.SelectedIndex = .Output
                cboConnectionOutput.Enabled = True
                txtConnectionInitialization.Text = .Init
                txtConnectionInitialization.Enabled = True
                chkConnectionNoteOff.Enabled = True
                chkConnectionNoteOff.Checked = .NoteOff
            End With
            'txtConnectionName.Focus()
        End If
    End Sub

    Private Sub chkWindowsMessages_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWindowsMessages.CheckedChanged
        Configuration.WmEnable = chkWindowsMessages.Checked
    End Sub

    Private Sub chkDmxin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDmxin.CheckedChanged
        Configuration.DmxinEnable = chkDmxin.Checked
    End Sub

    Private Sub chkDmxover_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDmxover.CheckedChanged
        Configuration.DmxoverEnable = chkDmxover.Checked
    End Sub

    Private Sub chkFingers_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFingers.CheckedChanged
        Configuration.FingersEnable = chkFingers.Checked
        cboFingersPort.Enabled = chkFingers.Checked
    End Sub

    Private Sub btnAddConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddConnection.Click
        Dim x As Integer = 0
        Do
            If Configuration.Connections.ContainsKey(x.ToString) Then
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
        item.Tag = x.ToString

        Configuration.Connections.Add(x.ToString, conn) 'This has to happen first, otherwise below will throw error
        lvConnections.Items.Add(item)
    End Sub

    Private Sub btnRemoveConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveConnection.Click
        Configuration.Connections.Remove(CStr(lvConnections.SelectedItems(0).Tag))
        lvConnections.Items.RemoveAt(lvConnections.SelectedItems(0).Index)
    End Sub

    Private Sub txtConnectionName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConnectionName.TextChanged
        If lvConnections.SelectedItems.Count > 0 Then
            lvConnections.SelectedItems(0).Text = txtConnectionName.Text
            Configuration.Connections.Item(lvConnections.SelectedItems(0).Tag.ToString).Name = txtConnectionName.Text
        End If
    End Sub

    Private Sub chkConnectionInputEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConnectionInputEnabled.CheckedChanged
        If lvConnections.SelectedItems.Count > 0 Then
            Configuration.Connections(lvConnections.SelectedItems(0).Tag.ToString).InputEnable = chkConnectionInputEnabled.Checked
            cboConnectionInput.Enabled = chkConnectionInputEnabled.Checked
        End If
    End Sub

    Private Sub cboConnectionInput_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConnectionInput.SelectedIndexChanged
        If lvConnections.SelectedItems.Count > 0 Then
            Dim oldKey As String = lvConnections.SelectedItems(0).Tag.ToString
            Dim newKey As String = cboConnectionInput.Items(cboConnectionInput.SelectedIndex).ToString
            System.Diagnostics.Debug.WriteLine(cboConnectionInput.Items(cboConnectionInput.SelectedIndex).ToString)
            If (newKey = oldKey) Then Exit Sub

            Configuration.Connections(oldKey).Input = cboConnectionInput.SelectedIndex
            Configuration.Connections(oldKey).InputName = newKey

            'Also change the Key of the selected connection
            Configuration.ChangeKey(newKey, oldKey)
            lvConnections.SelectedItems(0).Tag = newKey

            'PopulateConnections()
            'lvConnections.Items(newKey).Selected = True
            'lvConnections.Select()
            ''lvConnections.SelectedItems.Item(0).EnsureVisible()
        End If
    End Sub

    Private Sub chkConnectionOutputEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConnectionOutputEnabled.CheckedChanged
        If lvConnections.SelectedItems.Count > 0 Then
            Configuration.Connections(lvConnections.SelectedItems(0).Tag.ToString).OutputEnable = chkConnectionOutputEnabled.Checked
            cboConnectionOutput.Enabled = chkConnectionOutputEnabled.Checked
        End If
    End Sub

    Private Sub cboConnectionOutput_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConnectionOutput.SelectedIndexChanged
        If lvConnections.SelectedItems.Count > 0 Then
            Configuration.Connections(lvConnections.SelectedItems(0).Tag.ToString).Output = cboConnectionOutput.SelectedIndex
            Configuration.Connections(lvConnections.SelectedItems(0).Tag.ToString).OutputName = cboConnectionOutput.Items(cboConnectionOutput.SelectedIndex).ToString
        End If
    End Sub

    Private Sub txtConnectionInitialization_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConnectionInitialization.TextChanged
        If lvConnections.SelectedItems.Count > 0 Then
            Configuration.Connections(lvConnections.SelectedItems(0).Tag.ToString).Init = txtConnectionInitialization.Text
        End If
    End Sub

    Private Sub chkConnectionNoteOff_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConnectionNoteOff.CheckedChanged
        If lvConnections.SelectedItems.Count > 0 Then
            Configuration.Connections(lvConnections.SelectedItems(0).Tag.ToString).NoteOff = chkConnectionNoteOff.Checked
        End If
    End Sub
End Class
