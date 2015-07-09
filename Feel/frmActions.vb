Imports Feel.My.Resources

Public Class frmActions
    Dim activeProperty As New Object 'Currently highlighted action
    Dim DeviceList As Collections.Generic.Dictionary(Of String, Integer)

    'Holds information about the last touched control
    Private curCont As curControl
    Public WriteOnly Property CurrentControl() As curControl
        Set(ByVal value As curControl)
            'Update the current control marker
            curCont = value
            updateCurrentControl(value)
        End Set
    End Property

    'sub and its deleage to allow cross-threaded updating of form controls
    Delegate Sub updateCurrentControlCallback(ByVal value As curControl)
    Public Sub updateCurrentControl(ByVal value As curControl)
        If Me.InvokeRequired Then
            Dim d As New updateCurrentControlCallback(AddressOf updateCurrentControl)
            Invoke(d, New Object() {value})
        Else
            'Update UI
            setLabels(Configuration.Connections(value.Device).Name, value.Type, "Channel " & value.Channel.ToString, If(value.Type = "Note", CType(value.NotCon, Midi.Pitch).ToString, value.NotCon.ToString), value.VelVal.ToString)
            ''If this is the first device control recieved, enable the form controls.
            If (chkPaged.Enabled = False) Then
                chkPaged.Enabled = True
                nudDevicePage.Enabled = True
                grpConfiguration.Enabled = True
                lvActions.Enabled = True
                cmdActionAdd.Enabled = True
                cmdActionClear.Enabled = True
            End If

            'clear the actions list, and disable appropriate buttons
            lvActions.Items.Clear()
            DeselectAction()

            'TODO: Check to see if active control contains a page change action, if so, update page
            If False Then
                'change page
            Else
                nudDevicePage.Value = curCont.ContPage
                nudActionPage.Value = curCont.ContPage
            End If

            ''Check to see if this control has been programmed, update UI with configuration data
            If SetControl(False) Then
                With Configuration.Connections(value.Device).Control(curCont.ContStr)
                    chkPaged.Checked = .Paged
                    If SetPage(False) Then
                        With .Page(curCont.ContPage)
                            txtInitialState.Text = .InitialState
                            nudControlGroup.Value = .ControlGroup
                            If .Behavior = 0 Then
                                rdoMomentaryAbsolute.Checked = True
                            Else
                                rdoLatchRelative.Checked = True
                            End If
                            PopulateActions()
                        End With
                    Else
                        ''No Page was set, so give things default values
                        txtInitialState.Text = ""
                        nudControlGroup.Value = 0
                        rdoMomentaryAbsolute.Checked = True
                    End If
                End With
            Else
                ''No Control is configured, so give things default values
                chkPaged.Checked = False
            End If
        End If
    End Sub

    Public Sub DeselectAction()
        txtActionName.Text = ""
        cboActionFunction.SelectedIndex = -1
        nudActionPage.Value = 0
        grpAction.Enabled = False

        cmdActionRemove.Enabled = False
        cmdActionUp.Enabled = False
        cmdActionDown.Enabled = False
        cmdActionSwap.Enabled = False
        'cmdActionClear.Enabled = False
        grpAction.Enabled = False
    End Sub

    Delegate Sub SetLblCallback(ByVal Device As String, ByVal Description As String, ByVal Channel As String, ByVal NoteControl As String, ByVal VelocityValue As String)
    Public Sub setLabels(Optional ByVal Device As String = "%", Optional ByVal Description As String = "%", Optional ByVal Channel As String = "%", Optional ByVal NoteControl As String = "%", Optional ByVal VelocityValue As String = "%")
        If lblDevice.InvokeRequired Then
            Dim d As New SetLblCallback(AddressOf setLabels)
            Invoke(d, New Object() {Device, Description, Channel, NoteControl, VelocityValue})
        Else
            lblDevice.Text = If(Device = "%", lblDevice.Text, Device)
            lblDescription.Text = If(Description = "%", lblDevice.Text, Description)
            lblChannel.Text = If(Channel = "%", lblChannel.Text, Channel)
            lblNoteControl.Text = If(NoteControl = "%", lblNoteControl.Text, NoteControl)
            lblVelocityValue.Text = If(VelocityValue = "%", lblVelocityValue.Text, VelocityValue)
            'Change labels, if neccessary
            ttActions.SetToolTip(lblDevice, Configuration.Connections(curCont.Device).InputName)
            ttActions.SetToolTip(lblVelocityValue, "Hex:" & curCont.VelVal.ToString("X2"))
            If (Description = "Note") Then
                lblNotCon.Text = "Note:"
                lblVelVal.Text = "Velocity:"
                rdoMomentaryAbsolute.Text = "Momentary"
                rdoLatchRelative.Text = "Latch"

                ttActions.SetToolTip(lblChannel, "Hex:" & (144 + curCont.Channel).ToString("X2"))
                ttActions.SetToolTip(lblNoteControl, "Dec:" & curCont.NotCon.ToString & ", Hex:" & curCont.NotCon.ToString("X2"))
            ElseIf (Description = "Control") Then
                lblNotCon.Text = "Control:"
                lblVelVal.Text = "Value:"
                rdoMomentaryAbsolute.Text = "Absolute"
                rdoLatchRelative.Text = "Relative"

                ttActions.SetToolTip(lblChannel, "Hex:" & (176 + curCont.Channel).ToString("X2"))
                ttActions.SetToolTip(lblNoteControl, "Hex:" & curCont.NotCon.ToString("X2"))
            End If
        End If
    End Sub

    ''Redraws all actions in Actions List, since sorting is so damned difficult
    <ComponentModel.Description("Redraws the Actions List after individual Actions are sorted."), _
        ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Always)> _
    Private Sub PopulateActions()
        lvActions.Items.Clear()
        If (SetPage(False)) Then
            For Each act As clsAction In Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).Actions
                Dim lvi As Windows.Forms.ListViewItem = New Windows.Forms.ListViewItem
                With lvi
                    .Text = act.Name
                    .Checked = act.Enabled
                    .Group = If(curCont.Type = "Note", lvActions.Groups(0), lvActions.Groups(2))
                End With
                lvActions.Items.Add(lvi)
                lvi = Nothing
            Next
            If (curCont.Type = "Note") Then ''Only need to go through this for Notes, not CC
                For Each act As clsAction In Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).ActionsOff
                    Dim lvi As Windows.Forms.ListViewItem = New Windows.Forms.ListViewItem
                    With lvi
                        .Text = act.Name
                        .Checked = act.Enabled
                        .Group = lvActions.Groups(1)
                    End With
                    lvActions.Items.Add(lvi)
                Next
            End If
        End If
    End Sub
    <ComponentModel.Description("Redraws the Actions List after individual Actions are sorted, and subsequently highlights the desired Action."), _
        ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Always)> _
    Private Sub PopulateActions(ByRef selAction As clsAction)
        PopulateActions()

        ''Reselect the desired item
        With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
            Dim foundIndex As Integer = .Actions.IndexOf(selAction)
            If Not (foundIndex = -1) Then
                ''found in .Actions
                lvActions.Groups(If(curCont.Type = "Note", 0, 2)).Items(foundIndex).Selected = True
                lvActions.Groups(If(curCont.Type = "Note", 0, 2)).Items(foundIndex).EnsureVisible()
            Else
                foundIndex = .ActionsOff.IndexOf(selAction)
                If Not (foundIndex = -1) Then
                    ''found in .ActionsOff
                    lvActions.Groups(1).Items(foundIndex).Selected = True
                    lvActions.Groups(1).Items(foundIndex).EnsureVisible()
                End If
            End If
        End With
        ''Without this lvActions_ItemSelectionChanged() fails on second attempt to modify an Action:
        lvActions.Select()
    End Sub

    Private Sub frmActions_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        main.configMode = False
    End Sub

    Private Sub frmActions_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        main.SaveConfiguration()
    End Sub

    Private Sub frmActions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        main.configMode = True

        'Form size: 765, 460

        'clear the actions list
        lvActions.Items.Clear() 'lvActions.Clear() ''Also clears columns!

        cboActionFunction.ValueMember = "Value"
        cboActionFunction.DisplayMember = "Display"
        cboActionFunction.GroupMember = "Group"
        'TODO: Get rid of temporary placeholder actions
        cboActionFunction.DataSource = New Collections.ArrayList(New Object() { _
            New With {Key .Value = 101, Key .Group = "1. Internal Functions", Key .Display = "Go to Page"}, _
            New With {Key .Value = 102, Key .Group = "1. Internal Functions", Key .Display = "Change Control State"}, _
            New With {Key .Value = 301, Key .Group = "4. LightJockey Functions", Key .Display = "Send Windows Message"}, _
            New With {Key .Value = 302, Key .Group = "4. LightJockey Functions", Key .Display = "Post Windows Message"}, _
            New With {Key .Value = 303, Key .Group = "4. LightJockey Functions", Key .Display = "Load Cue"}, _
            New With {Key .Value = 304, Key .Group = "4. LightJockey Functions", Key .Display = "Load CueList"}, _
            New With {Key .Value = 305, Key .Group = "4. LightJockey Functions", Key .Display = "Macro Amplitude"}, _
            New With {Key .Value = 306, Key .Group = "4. LightJockey Functions", Key .Display = "Macro Speed"}, _
            New With {Key .Value = 307, Key .Group = "4. LightJockey Functions", Key .Display = "Intensity Group"} _
            })

        'New With {Key .Value = 103, Key .Group = "1. Internal Functions", Key .Display = "Shift"}, _
        'New With {Key .Value = 102, Key .Group = "1. Internal Functions", Key .Display = "Skip Pages"}, _
        'New With {Key .Value = 104, Key .Group = "1. Internal Functions", Key .Display = "Redraw Controls"}, _
        'New With {Key .Value = 201, Key .Group = "2. MIDI Functions", Key .Display = "Send MIDI"}, _
        'New With {Key .Value = 202, Key .Group = "2. MIDI Functions", Key .Display = "Send Sysex"}, _
        'New With {Key .Value = 302, Key .Group = "3. Windows Functions", Key .Display = "Execute Command Line"}, _
        'New With {Key .Value = 303, Key .Group = "3. Windows Functions", Key .Display = "Run Program"}, _
        'New With {Key .Value = 304, Key .Group = "3. Windows Functions", Key .Display = "Send Keyboard Keys"}, _
        'New With {Key .Value = 401, Key .Group = "4. LightJockey Functions", Key .Display = String.Empty}, _
        'New With {Key .Value = 501, Key .Group = "5. Fingers Emulation", Key .Display = String.Empty}, _
        'New With {Key .Value = 601, Key .Group = "6. DMX-In", Key .Display = String.Empty}, _
        'New With {Key .Value = 701, Key .Group = "7. DMX-Override", Key .Display = String.Empty} _

        ChangeDescriptionHeight(pgAction, 100)
    End Sub

    Private Sub cboActionFunction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboActionFunction.SelectedIndexChanged
        If (lvActions.SelectedItems.Count > 0) Then
            With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
                Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
                Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
                If (cboActionFunctionQualify(whichGroup, curIndex)) Then
                    Select Case CType(cboActionFunction.SelectedValue, Integer)
                        Case 101
                            'activeProperty = New clsIntPage
                            'txtActionDescription.Text = clsIntPage.Description
                            ''txtActionDescription.Text = CType(activeProperty, clsIntPage).Description
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionIntPage
                            Else
                                .Actions(curIndex).Action = New clsActionIntPage
                            End If
                            txtActionDescription.Text = clsActionIntPage._Description 'CType(.Actions(curIndex).Action, clsIntPage).Description
                        Case 102
                            'activeProperty = New clsIntChangeControlState
                            'txtActionDescription.Text = clsIntChangeControlState.Description
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionIntChangeControlState
                            Else
                                .Actions(curIndex).Action = New clsActionIntChangeControlState
                            End If
                            txtActionDescription.Text = clsActionIntChangeControlState._Description
                        Case 301
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionSendMessage
                            Else
                                .Actions(curIndex).Action = New clsActionSendMessage
                            End If
                            txtActionDescription.Text = clsActionSendMessage._Description 'CType(.Actions(curIndex).Action, clsActionSendMessage).Description
                        Case 301
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionPostMessage
                            Else
                                .Actions(curIndex).Action = New clsActionPostMessage
                            End If
                            txtActionDescription.Text = clsActionPostMessage._Description
                        Case 303
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionLoadCue
                            Else
                                .Actions(curIndex).Action = New clsActionLoadCue
                            End If
                            txtActionDescription.Text = clsActionLoadCue._Description
                        Case 304
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionLoadCueList
                            Else
                                .Actions(curIndex).Action = New clsActionLoadCueList
                            End If
                            txtActionDescription.Text = clsActionLoadCueList._Description
                        Case 305
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionCueMacroAmplitudeRelative
                            Else
                                .Actions(curIndex).Action = New clsActionCueMacroAmplitudeRelative
                            End If
                            txtActionDescription.Text = clsActionCueMacroAmplitudeRelative._Description
                        Case 306
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionCueMacroSpeedRelative
                            Else
                                .Actions(curIndex).Action = New clsActionCueMacroSpeedRelative
                            End If
                            txtActionDescription.Text = clsActionCueMacroSpeedRelative._Description
                        Case 307
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = New clsActionIntensityGroupAbsolute
                            Else
                                .Actions(curIndex).Action = New clsActionIntensityGroupAbsolute
                            End If
                            txtActionDescription.Text = clsActionIntensityGroupAbsolute._Description
                        Case Else
                            'activeProperty = Nothing
                            'txtActionDescription.Text = ""
                            If (whichGroup = 1) Then
                                .ActionsOff(curIndex).Action = Nothing
                            Else
                                .Actions(curIndex).Action = Nothing
                            End If
                            txtActionDescription.Text = "Select a Function to assign to this Action."

                            'Case 102
                            '    activeProperty = New clsIntPageJump
                            '    txtActionDescription.Text = clsIntPageJump.Description
                            'Case 103
                            '    activeProperty = New clsIntShift
                            '    txtActionDescription.Text = clsIntShift.Description
                            'Case 104
                            '    activeProperty = New clsIntRedrawControls
                            '    txtActionDescription.Text = clsIntRedrawControls.Description
                            'Case 201
                            '    activeProperty = New clsMidiSend
                            'Case 202
                            '    activeProperty = New clsMidiSysex
                            'Case 302
                            '    activeProperty = New clsWinCommand
                            'Case 303
                            '    activeProperty = New clsWinRun
                            'Case 304
                            '    activeProperty = New clsWinSendkeys
                    End Select
                Else
                    txtActionDescription.Text = "Edit this Action's properties below:"
                End If
                pgAction.SelectedObject = .Actions(curIndex).Action
            End With
        Else
            activeProperty = Nothing
            txtActionDescription.Text = ""
            cboActionFunction.SelectedIndex = -1
        End If
    End Sub
    Private Function cboActionFunctionQualify(ByVal whichGroup As Integer, ByVal curIndex As Integer) As Boolean
        'This tests to see if an Action has already been assigned to a clsAction
        With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
            If (whichGroup = 1) Then
                If (.ActionsOff(curIndex).Action Is Nothing) Then
                    Return True
                Else
                    Return False
                End If
            Else
                If (.Actions(curIndex).Action Is Nothing) Then
                    Return True
                Else
                    Return False
                End If
            End If
        End With
    End Function

    ''The ItemSelectionChanged event occurs whether the item state changes from selected to deselected or deselected to selected.
    ''The SelectedIndexChanged event occurs in single selection ListView controls, whenever there is a change to the index position of the selected item. In a multiple selection ListView control, this event occurs whenever an item is removed or added to the list of selected items.
    Private Sub lvActions_ItemSelectionChanged() Handles lvActions.ItemSelectionChanged
        'Private Sub lvActions_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvActions.ItemSelectionChanged
        ''TODO: ?? Save the previous action's settings, before updating
        'Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).Actions(0).Action = pgAction.SelectedObject

        If (lvActions.SelectedItems.Count < 1) Then
            DeselectAction()
        Else
            ''Actions List pane:
            cmdActionRemove.Enabled = True
            cmdActionUp.Enabled = True
            cmdActionDown.Enabled = True
            If (curCont.Type = "Note") Then cmdActionSwap.Enabled = True
            cmdActionClear.Enabled = True
            grpAction.Enabled = True

            ''Action pane:
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
                If (whichGroup = 1) Then
                    txtActionName.Text = .ActionsOff(curIndex).Name
                    'TODO: figure out how to set description, without knowing what Object Type the class is
                    'cboActionFunction.SelectedItem = 
                    pgAction.SelectedObject = .ActionsOff(curIndex).Action
                Else
                    txtActionName.Text = .Actions(curIndex).Name
                    pgAction.SelectedObject = .Actions(curIndex).Action
                End If
            End With
        End If
    End Sub
    Private Sub lvActions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvActions.SelectedIndexChanged
        ''Update grpAction fields
    End Sub

    Private Sub lvActions_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvActions.ItemChecked
        If (SetPage(False)) Then
            Dim whichGroup As Integer = lvActions.Groups.IndexOf((e.Item.Group))
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(e.Item)
            With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
                If (whichGroup = 1) Then
                    .ActionsOff(curIndex).Enabled = e.Item.Checked
                    'Diagnostics.Debug.WriteLine(.ActionsOff(curIndex).Name & ": " & .ActionsOff(curIndex).Enabled.ToString)
                Else
                    .Actions(curIndex).Enabled = e.Item.Checked
                    'Diagnostics.Debug.WriteLine(.Actions(curIndex).Name & ": " & .Actions(curIndex).Enabled.ToString)
                End If
            End With
        End If
    End Sub

    Private Sub cmdActionClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionClear.Click
        If Windows.Forms.MessageBox.Show("Are you sure you want to clear all actions associated with this control?", "Feel: Confirm Clear Actions", Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Exclamation, Windows.Forms.MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            lvActions.Items.Clear()
            'grpAction.Enabled = False
            lvActions_ItemSelectionChanged()
            If SetPage(True) Then
                'TODO: (not sure if this is the best way to do this)
                Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).Actions = New Collections.Generic.List(Of clsAction)
                Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).ActionsOff = New Collections.Generic.List(Of clsAction)
            End If
        End If
    End Sub

    Private Sub cmdActionAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionAdd.Click
        Dim groupReleased As Boolean = False
        Dim newIndex As Integer
        If (lvActions.SelectedItems.Count > 0) Then
            If (lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group) = 1) Then
                groupReleased = True
                newIndex = lvActions.Groups(1).Items.Count
            End If
        Else
            newIndex = lvActions.Groups(If(curCont.Type = "Note", 0, 2)).Items.Count
        End If

        If (SetPage(True)) Then
            With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
                Dim tmpAction As clsAction = New clsAction
                ''The next line is not really necessary, but do it anyway to distinguish between multiple new Actions
                '' Also, it's not exactly perfect, as sometimes the "newIndex + 1" number is inaccurate after performing a "Swap"
                tmpAction.Name = "New Action (" & (newIndex + 1).ToString & ")"
                If (groupReleased) Then
                    .ActionsOff.Add(tmpAction)
                Else
                    .Actions.Add(tmpAction)
                End If
                PopulateActions(tmpAction)
            End With
        End If

        'TODO: depreciated code:
        'Dim lvi As Windows.Forms.ListViewItem = New Windows.Forms.ListViewItem 'Dim lvi As New Windows.Forms.ListViewItem
        'With lvi
        '    .Text = "New Action (" & (newIndex + 1).ToString & ")"
        '    .Checked = False
        '    .Group = If(curCont.Type = "Note", lvActions.Groups(0), lvActions.Groups(2))
        'End With
        'lvActions.Items.Add(lvi)

        '''TODO: DOUBLE CHECK THIS
        'If SetPage(True) Then
        '    With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
        '        If .Actions.Count <> newIndex Then
        '            Windows.Forms.MessageBox.Show("Conflicting Actions count.")
        '        Else
        '            Dim newAction As clsAction = New clsAction
        '            '.Actions.Insert(newIndex, newAction)
        '            .Actions.Add(newAction)

        '            'TODO: need this? Probably not, since we added it above
        '            ' otherwise we could eliminate that code and let PopulateActions do it
        '            'PopulateActions(newAction)
        '        End If
        '    End With
        'Else
        '    ''eror creating page on this control
        '    Windows.Forms.MessageBox.Show("OOPS! Can't make page.")
        'End If
    End Sub

    Private Sub cmdActionRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionRemove.Click
        If (lvActions.SelectedItems.Count > 0) Then
            'TODO: need to update this to PopulateActions()?
            ''First, delete from Configuration
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            If (whichGroup = 1) Then
                Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).ActionsOff.RemoveAt(curIndex)
            Else
                Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).Actions.RemoveAt(curIndex)
            End If
            ''Then, remove from lvActions
            lvActions.Items.Remove(lvActions.SelectedItems(0))
        End If
    End Sub

    Private Sub cmdActionUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionUp.Click
        If (lvActions.SelectedItems.Count > 0) Then
            ''Only have to do anything if the item isn't already at index 0 within the group
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            If (curIndex > 0) Then
                With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
                    Dim tmpAction As clsAction
                    If (whichGroup = 1) Then
                        tmpAction = .ActionsOff(curIndex)
                        .ActionsOff.RemoveAt(curIndex)
                        .ActionsOff.Insert(curIndex - 1, tmpAction)
                    Else
                        tmpAction = .Actions(curIndex)
                        .Actions.RemoveAt(curIndex)
                        .Actions.Insert(curIndex - 1, tmpAction)
                    End If
                    PopulateActions(tmpAction)
                End With
        End If
        End If
    End Sub

    Private Sub cmdActionDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionDown.Click
        If (lvActions.SelectedItems.Count > 0) Then
            ''Only have to do anything if the item isn't already at last index within the group
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            If (curIndex <> lvActions.Groups(whichGroup).Items.Count - 1) Then
                With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
                    Dim tmpAction As clsAction
                    If (whichGroup = 1) Then
                        tmpAction = .ActionsOff(curIndex)
                        .ActionsOff.RemoveAt(curIndex)
                        .ActionsOff.Insert(curIndex + 1, tmpAction)
                    Else
                        tmpAction = .Actions(curIndex)
                        .Actions.RemoveAt(curIndex)
                        .Actions.Insert(curIndex + 1, tmpAction)
                    End If

                    PopulateActions(tmpAction)
                End With
            End If
        End If
    End Sub

    Private Sub cmdActionSwap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionSwap.Click
        If (lvActions.SelectedItems.Count > 0) Then
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            Dim tmpAction As New clsAction
            With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
                If (whichGroup = 1) Then
                    tmpAction = .ActionsOff(curIndex)
                    .ActionsOff.RemoveAt(curIndex)
                    .Actions.Add(tmpAction)
                Else
                    tmpAction = .Actions(curIndex)
                    .Actions.RemoveAt(curIndex)
                    .ActionsOff.Add(tmpAction)
                End If
            End With
            PopulateActions(tmpAction)

            ''OLD:
            'lvActions.SelectedItems(0).Group = If(lvActions.SelectedItems(0).Group Is lvActions.Groups(0), lvActions.Groups(1), lvActions.Groups(0))
        End If
    End Sub

    Private Sub txtActionName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtActionName.TextChanged
        If (lvActions.SelectedItems.Count > 0) Then
            lvActions.SelectedItems(0).Text = txtActionName.Text

            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim whichAction As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            With Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage)
                If (whichGroup = 1) Then
                    .ActionsOff(whichAction).Name = txtActionName.Text
                Else
                    .Actions(whichAction).Name = txtActionName.Text
                End If
            End With
        End If
    End Sub

    ''Generates a new Control in the Device Configuration, if not exist
    Private Function SetControl(ByVal createControl As Boolean) As Boolean
        If Not curCont Is Nothing Then
            If Not (Configuration.Connections(curCont.Device).Control.ContainsKey(curCont.ContStr)) Then
                If createControl Then
                    Dim newControl As clsControl = New clsControl
                    Configuration.Connections(curCont.Device).Control.Add(curCont.ContStr, newControl)
                    Return True
                Else
                    Return False
                End If
            Else
                Return True
            End If
        Else : Return False
        End If
    End Function

    ''Generates a new Page in the Control, if not exist
    Private Function SetPage(ByVal createPage As Boolean) As Boolean
        If Not curCont Is Nothing Then
            If SetControl(createPage) Then
                If Not (Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page.ContainsKey(curCont.ContPage)) Then
                    If createPage Then
                        Dim newPage As clsControlPage = New clsControlPage
                        Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page.Add(curCont.ContPage, newPage)
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Else : Return False
        End If
    End Function

    ''Allows modification of the "Description" window height in a PropertyGrid control
    Private Shared Sub ChangeDescriptionHeight(ByVal grid As Windows.Forms.PropertyGrid, ByVal height As Integer)
        If grid Is Nothing Then
            Throw New ArgumentNullException("grid")
        End If

        For Each control As Windows.Forms.Control In grid.Controls
            If control.[GetType]().Name = "DocComment" Then
                Dim fieldInfo As Reflection.FieldInfo = control.[GetType]().BaseType.GetField("userSized", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
                fieldInfo.SetValue(control, True)
                control.Height = height
                Return
            End If
        Next
    End Sub

    Private Sub chkPaged_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPaged.CheckedChanged
        If SetControl(True) Then
            Configuration.Connections(curCont.Device).Control(curCont.ContStr).Paged = chkPaged.Checked
        End If
    End Sub

    Private Sub txtInitialState_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInitialState.TextChanged
        If SetPage(True) Then
            Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).InitialState = txtInitialState.Text
        Else
            'TODO: need this?
            ''error that should never happen
            Windows.Forms.MessageBox.Show("unable to set this value")
        End If
        'Diagnostics.Debug.WriteLine("Did it take? " & Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).InitialState)
    End Sub

    Private Sub nudControlGroup_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudControlGroup.ValueChanged
        If SetPage(True) Then
            Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).ControlGroup = CByte(nudControlGroup.Value)
        End If
    End Sub

    Private Sub rdoMomentaryAbsolute_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoMomentaryAbsolute.CheckedChanged
        If SetPage(True) Then
            If rdoMomentaryAbsolute.Checked Then
                Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).Behavior = 0
            Else
                Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).Behavior = 1
            End If
        End If
    End Sub
    '''Shouldn't need this, because rdoMomentaryAbsolute should take care of it
    'Private Sub rdoLatchRelative_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoLatchRelative.CheckedChanged
    '    If SetPage(True) Then
    '        If rdoLatchRelative.Checked Then
    '            Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).Behavior = 1
    '        Else
    '            Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).Behavior = 0
    '        End If
    '    End If
    'End Sub

    Private Sub pgAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pgAction.Click

    End Sub
End Class

''' <summary>
''' Currently active contrrol
''' </summary>
''' <remarks>Used by frmActions, this class holds details of the last control
''' which was manipulated on the controller.</remarks>
Public Class curControl
    Public Device As String
    Public Type As String
    Public Channel As Byte
    Public NotCon As Byte
    Public VelVal As Byte

    Private _ContStr As String
    Public ReadOnly Property ContStr() As String
        Get
            If (_ContStr Is Nothing) Then
                _ContStr = If(Type = "Note", "9", "B") & Channel.ToString("X") & NotCon.ToString("X2")
            End If
            Return _ContStr
        End Get
    End Property

    ''basically just a shorthand alias
    Public ReadOnly Property ContPage() As Byte
        Get
            Return Configuration.Connections(Device).PageCurrent
        End Get
    End Property
End Class

'''Keeping this solely because I found a little way to make shit transparent
'Private Sub UpdateButtonImages()
'    For Each button As Windows.Forms.Control In Me.Controls
'        If TypeOf button Is Windows.Forms.Button Then
'            Dim btnName As String = button.Name
'            button.BackgroundImage = CType(If(button.Enabled, Feel.My.Resources.Feel.ResourceManager.GetObject(btnName), Feel.My.Resources.Feel.ResourceManager.GetObject(btnName & "_o")), Drawing.Image)
'            Dim g As New System.Drawing.Bitmap(button.BackgroundImage) : g.MakeTransparent(System.Drawing.Color.Magenta) : button.BackgroundImage = g
'        End If
'    Next
'    ''Get rid of the pink background of our button icons
'    'Dim g As New System.Drawing.Bitmap(cmdActionAdd.BackgroundImage) : g.MakeTransparent(System.Drawing.Color.Magenta) : cmdActionAdd.BackgroundImage = g
'    'g = New System.Drawing.Bitmap(cmdActionRemove.BackgroundImage) : g.MakeTransparent(System.Drawing.Color.Magenta) : cmdActionRemove.BackgroundImage = g
'    'g = New System.Drawing.Bitmap(cmdActionUp.BackgroundImage) : g.MakeTransparent(System.Drawing.Color.Magenta) : cmdActionUp.BackgroundImage = g
'    'g = New System.Drawing.Bitmap(cmdActionDown.BackgroundImage) : g.MakeTransparent(System.Drawing.Color.Magenta) : cmdActionDown.BackgroundImage = g
'    'g = New System.Drawing.Bitmap(cmdActionSwap.BackgroundImage) : g.MakeTransparent(System.Drawing.Color.Magenta) : cmdActionSwap.BackgroundImage = g
'    'g = New System.Drawing.Bitmap(cmdActionClear.BackgroundImage) : g.MakeTransparent(System.Drawing.Color.Magenta) : cmdActionClear.BackgroundImage = g
'    '''Make shit transparent?!
'    ''' [also make cmdActionUp color White]
'    ''g = New System.Drawing.Bitmap(cmdActiondUp.BackgroundImage) : g.MakeTransparent(System.Drawing.Color.White) : Me.BackgroundImage = g
'End Sub