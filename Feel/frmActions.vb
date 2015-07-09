﻿Imports System.ComponentModel

'Imports Feel.My.Resources
Public Class frmActions
    'Dim activeProperty As New Object 'Currently highlighted action
    Dim DeviceList As Collections.Generic.Dictionary(Of String, Integer)

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
                Return FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(Device)).PageCurrent
            End Get
        End Property
    End Class
    'Holds information about the last touched control
    Private _curCont As curControl
    Public WriteOnly Property CurrentControl() As curControl
        Set(ByVal value As curControl)
            If Not (holdControl) Then
                ''Update last control's state, if available
                updateLastControl()

                'Update the current control marker
                _curCont = value
                updateCurrentControl(value)
            End If
        End Set
    End Property

    '"Lock" last active control
    Dim holdControl As Boolean = False

    Private Sub updateLastControl()
        If Not (_curCont Is Nothing) Then
            Dim _device As Integer = serviceHost.FindDeviceIndexByInput(_curCont.Device)
            If (FeelConfig.Connections.ContainsKey(_device)) Then
                With FeelConfig.Connections(_device)
                    If (.Control.ContainsKey(_curCont.ContStr)) Then
                        If (.Control(_curCont.ContStr).Page.ContainsKey(_curCont.ContPage)) Then
                            If Not (.Control(_curCont.ContStr).Page(_curCont.ContPage).CurrentState = "") Then
                                serviceHost.SendMIDI(_device, .Control(_curCont.ContStr).Page(_curCont.ContPage).CurrentState)
                                'ElseIf Not (.InitialState = "") Then
                                '    main.SendMidi(_device, .InitialState)
                            End If
                        End If
                    End If
                End With
            End If
        End If
    End Sub

    Delegate Sub updateCurrentControlCallback(ByVal value As curControl)
    Public Sub updateCurrentControl(ByVal value As curControl)
        If Me.InvokeRequired Then
            Dim d As New updateCurrentControlCallback(AddressOf updateCurrentControl)
            Invoke(d, New Object() {value})
        Else
            Dim _device As Integer = serviceHost.FindDeviceIndexByInput(value.Device)

            'Update UI
            'setLabels(FeelConfig.Connections(_device).Name, value.Type, "Channel " & value.Channel.ToString, If(value.Type = "Note", CType(value.NotCon, Midi.Pitch).ToString, value.NotCon.ToString), value.VelVal.ToString)
            setLabels(FeelConfig.Connections(_device).Name, value.Type, "Channel " & DisplayChannel(value.Channel), If(value.Type = "Note", DisplayNote(value.NotCon), value.NotCon.ToString), DisplayVelVal(value.VelVal))
            ''If this is the first device control recieved, enable the form controls.
            If (chkPaged.Enabled = False) Then
                chkPaged.Enabled = True
                nudDevicePage.Enabled = True
                grpConfiguration.Enabled = True
                grpActions.Enabled = True
                'TODO: Previous makes the following unneccessary?
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
                nudDevicePage.Value = _curCont.ContPage
            End If

            ''Check to see if this control has been programmed, update UI with configuration data
            If SetControl(False) Then
                With FeelConfig.Connections(_device).Control(_curCont.ContStr)
                    chkPaged.Checked = .Paged
                    If SetPage(False) Then
                        With .Page(_curCont.ContPage)
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
                txtInitialState.Text = ""
                nudControlGroup.Value = 0
                rdoMomentaryAbsolute.Checked = True
            End If
        End If
    End Sub

    Public Sub DeselectAction()
        txtActionName.Text = ""
        cboActionFunction.SelectedIndex = -1
        pgAction.SelectedObject = Nothing

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
            Invoke(New SetLblCallback(AddressOf setLabels), New Object() {Device, Description, Channel, NoteControl, VelocityValue})
        Else
            lblDevice.Text = If(Device = "%", lblDevice.Text, Device)
            lblDescription.Text = If(Description = "%", lblDevice.Text, Description)
            lblChannel.Text = If(Channel = "%", lblChannel.Text, Channel)
            lblNoteControl.Text = If(NoteControl = "%", lblNoteControl.Text, NoteControl)
            lblVelocityValue.Text = If(VelocityValue = "%", lblVelocityValue.Text, VelocityValue)
            'Change labels, if neccessary
            ttActions.SetToolTip(lblDevice, FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).InputName)
            ttActions.SetToolTip(lblVelocityValue, "Dec: " & _curCont.VelVal.ToString & ", Hex:" & _curCont.VelVal.ToString("X2"))
            If (Description = "Note") Then
                lblNotCon.Text = "Note:"
                lblVelVal.Text = "Velocity:"
                rdoMomentaryAbsolute.Text = "Momentary"
                rdoLatchRelative.Text = "Latch"

                ttActions.SetToolTip(lblChannel, "Hex:" & (144 + _curCont.Channel).ToString("X2"))
                ttActions.SetToolTip(lblNoteControl, "Note: " & DisplayNoteAsString(_curCont.NotCon) & ", Dec:" & _curCont.NotCon.ToString & ", Hex:" & _curCont.NotCon.ToString("X2"))
            ElseIf (Description = "Control") Then
                lblNotCon.Text = "Control:"
                lblVelVal.Text = "Value:"
                rdoMomentaryAbsolute.Text = "Absolute"
                rdoLatchRelative.Text = "Relative"

                ttActions.SetToolTip(lblChannel, "Hex:" & (176 + _curCont.Channel).ToString("X2"))
                ttActions.SetToolTip(lblNoteControl, "Dec: " & _curCont.NotCon.ToString & ", Hex:" & _curCont.NotCon.ToString("X2"))
            End If
        End If
    End Sub

    ''Redraws all actions in Actions List, since sorting is so damned difficult
    <ComponentModel.Description("Redraws the Actions List after individual Actions are sorted."), _
        ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Always)> _
    Private Sub PopulateActions()
        lvActions.Items.Clear()
        If (SetPage(False)) Then
            Dim _device As Integer = serviceHost.FindDeviceIndexByInput(_curCont.Device)

            For Each act As clsAction In FeelConfig.Connections(_device).Control(_curCont.ContStr).Page(_curCont.ContPage).Actions
                Dim lvi As Windows.Forms.ListViewItem = New Windows.Forms.ListViewItem
                With lvi
                    .Text = act.Name
                    .Checked = act.Enabled
                    .Group = If(_curCont.Type = "Note", lvActions.Groups(0), lvActions.Groups(2))
                End With
                lvActions.Items.Add(lvi)
                lvi = Nothing
            Next
            If (_curCont.Type = "Note") Then ''Only need to go through this for Notes, not CC
                For Each act As clsAction In FeelConfig.Connections(_device).Control(_curCont.ContStr).Page(_curCont.ContPage).ActionsOff
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
        With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
            Dim foundIndex As Integer = .Actions.IndexOf(selAction)
            If Not (foundIndex = -1) Then
                ''found in .Actions
                lvActions.Groups(If(_curCont.Type = "Note", 0, 2)).Items(foundIndex).Selected = True
                lvActions.Groups(If(_curCont.Type = "Note", 0, 2)).Items(foundIndex).EnsureVisible()
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

    Private Sub PopulateActionFunctions()
        cboActionFunction.ValueMember = "Value"
        cboActionFunction.DisplayMember = "Display"
        cboActionFunction.GroupMember = "Group"

        'TODO: Figure out why there are empty lines in cboActionFunction
        Dim actionlist As New Collections.ArrayList()
        For Each actnMod As Collections.Generic.KeyValuePair(Of Guid, ActionInterface.IAction) In main.actionModules
            actionlist.Add(New With {Key .Value = actnMod.Value.UniqueID, Key .Group = actnMod.Value.Group, Key .Display = actnMod.Value.Name})
        Next
        cboActionFunction.DataSource = actionlist
        actionlist = Nothing
    End Sub

    Private Sub frmActions_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        main.configMode = False
    End Sub

    Private Sub frmActions_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        updateLastControl()
        'main.SaveConfiguration()
    End Sub

    Private Sub frmActions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        main.configMode = True
        'Form size: 765, 460

        'clear the actions list
        lvActions.Items.Clear()

        PopulateActionFunctions()

        ChangeDescriptionHeight(pgAction, 128)
    End Sub

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

    Private Sub cboActionFunction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboActionFunction.SelectedIndexChanged
        If (lvActions.SelectedItems.Count = 1) Then
            Dim targetGuid As Guid = CType(cboActionFunction.SelectedValue, Guid)
            If Not (targetGuid = Guid.Empty) Then
                txtActionDescription.Text = main.actionModules.Item(targetGuid).Description
                With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                    Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
                    Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
                    If (cboActionFunctionQualify(whichGroup, curIndex)) Then
                        If (whichGroup = 1) Then
                            .ActionsOff(curIndex).Type = targetGuid
                            .ActionsOff(curIndex).Data = ObjectCopier.Clone(main.actionModules.Item(.ActionsOff(curIndex).Type).Data)
                            .ActionsOff(curIndex)._available = True
                        Else
                            .Actions(curIndex).Type = targetGuid
                            'Diagnostics.Debug.WriteLine(main.actionModules.Item(targetGuid).Data.GetType.ToString)
                            .Actions(curIndex).Data = ObjectCopier.Clone(main.actionModules.Item(.Actions(curIndex).Type).Data)
                            .Actions(curIndex)._available = True
                        End If
                    Else
                        'no need to do anything to Action
                    End If
                    pgAction.SelectedObject = If(whichGroup = 1, .ActionsOff(curIndex).Data, .Actions(curIndex).Data)
                End With
            End If
        Else
            'activeProperty = Nothing
            txtActionDescription.Text = ""
            cboActionFunction.SelectedIndex = -1
        End If
    End Sub
    ''' <summary>
    ''' This tests to see if an Action needs to be assigned to a clsAction
    ''' </summary>
    ''' <param name="whichGroup">The index of the group the selected Action belongs to in Actions ListView control.</param>
    ''' <param name="curIndex">The selected Action's current index in the group.</param>
    ''' <returns>True if .Data Is Nothing, otherwise False.</returns>
    ''' <remarks></remarks>
    Private Function cboActionFunctionQualify(ByVal whichGroup As Integer, ByVal curIndex As Integer) As Boolean
        With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
            If (whichGroup = 1) Then
                If (.ActionsOff(curIndex).Data Is Nothing) Then
                    Return True
                Else
                    Return False
                End If
            Else
                If (.Actions(curIndex).Data Is Nothing) Then
                    Return True
                Else
                    Return False
                End If
            End If
        End With
    End Function

    Private Sub lvActions_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvActions.Click
        'If (My.Computer.Keyboard.ShiftKeyDown) Then
        '    'make multiselect possible
        'End If
    End Sub

    ''The ItemSelectionChanged event occurs whether the item state changes from selected to deselected or deselected to selected.
    ''The SelectedIndexChanged event occurs in single selection ListView controls, whenever there is a change to the index position of the selected item. In a multiple selection ListView control, this event occurs whenever an item is removed or added to the list of selected items.
    Private Sub lvActions_ItemSelectionChanged() Handles lvActions.ItemSelectionChanged
        'Private Sub lvActions_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvActions.ItemSelectionChanged

        If (lvActions.SelectedItems.Count = 1) Then
            ''Actions List pane:
            cmdActionRemove.Enabled = True
            cmdActionUp.Enabled = True
            cmdActionDown.Enabled = True
            If (_curCont.Type = "Note") Then cmdActionSwap.Enabled = True
            cmdActionClear.Enabled = True
            grpAction.Enabled = True

            ''Action pane:
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                If (whichGroup = 1) Then
                    txtActionName.Text = .ActionsOff(curIndex).Name

                    Try
                        cboActionFunction.SelectedValue = .ActionsOff(curIndex).Type
                    Catch ex As Exception
                        Windows.MessageBox.Show("This action is not currently loaded.")
                    End Try

                    'pgAction.SelectedObject = .ActionsOff(curIndex).Data
                Else
                    txtActionName.Text = .Actions(curIndex).Name

                    'cboActionFunction.SelectedItem = -1
                    'cboActionFunction.Select()
                    Try
                        cboActionFunction.SelectedValue = .Actions(curIndex).Type
                    Catch ex As Exception
                        Windows.MessageBox.Show("This action is not currently loaded.")
                    End Try

                    'pgAction.SelectedObject = .Actions(curIndex).Data
                End If
            End With
        Else
            ''Copy/Paste mode or nothing selected
            DeselectAction()
        End If
    End Sub
    'Private Sub lvActions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvActions.SelectedIndexChanged
    '    ''Update grpAction fields
    '    'lvActions_ItemSelectionChanged()
    '    'cboActionFunction.SelectedIndex = -1
    'End Sub

    Private Sub lvActions_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvActions.ItemChecked
        If (SetPage(False)) Then
            Dim whichGroup As Integer = lvActions.Groups.IndexOf((e.Item.Group))
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(e.Item)
            With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
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
                With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                    .Actions = New Collections.Generic.List(Of clsAction)
                    .ActionsOff = New Collections.Generic.List(Of clsAction)
                End With
            End If
        End If
    End Sub

    Private Sub cmdActionAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionAdd.Click
        Dim groupReleased As Boolean = False
        If (lvActions.SelectedItems.Count = 1) Then
            If (lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group) = 1) Then
                groupReleased = True
            End If
        End If
        If (SetPage(True)) Then
            With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                Dim tmpAction As clsAction = New clsAction
                tmpAction.Name = "New Action (" & (lvActions.Items.Count + 1).ToString & ")"
                If (groupReleased) Then
                    .ActionsOff.Add(tmpAction)
                Else
                    .Actions.Add(tmpAction)
                End If
                PopulateActions(tmpAction)
            End With
        End If
    End Sub

    Private Sub cmdActionRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionRemove.Click
        If (lvActions.SelectedItems.Count = 1) Then
            'TODO: need to update this to PopulateActions()?
            ''First, delete from Configuration
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            If (whichGroup = 1) Then
                FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage).ActionsOff.RemoveAt(curIndex)
            Else
                FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage).Actions.RemoveAt(curIndex)
            End If
            ''Then, remove from lvActions
            lvActions.Items.Remove(lvActions.SelectedItems(0))
        End If
    End Sub

    Private Sub cmdActionUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionUp.Click
        If (lvActions.SelectedItems.Count = 1) Then
            ''Only have to do anything if the item isn't already at index 0 within the group
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            If (curIndex > 0) Then
                With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
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
        If (lvActions.SelectedItems.Count = 1) Then
            ''Only have to do anything if the item isn't already at last index within the group
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            If (curIndex <> lvActions.Groups(whichGroup).Items.Count - 1) Then
                With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
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
        If (lvActions.SelectedItems.Count = 1) Then
            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            Dim tmpAction As New clsAction
            With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
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
        If (lvActions.SelectedItems.Count = 1) Then
            lvActions.SelectedItems(0).Text = txtActionName.Text

            Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            Dim whichAction As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
            With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                If (whichGroup = 1) Then
                    .ActionsOff(whichAction).Name = txtActionName.Text
                Else
                    .Actions(whichAction).Name = txtActionName.Text
                End If
            End With
        End If
    End Sub

    Private Sub chkPaged_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPaged.CheckedChanged
        If SetControl(True) Then
            FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Paged = chkPaged.Checked
            'TODO: Set nudDevicePage to 0, and disable?
        End If
    End Sub

    Private Sub txtInitialState_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInitialState.TextChanged
        If SetPage(True) Then
            FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage).InitialState = txtInitialState.Text
        Else
            'TODO: need this?
            ''error that should never happen
            Windows.Forms.MessageBox.Show("unable to set this value")
        End If
        'Diagnostics.Debug.WriteLine("Did it take? " & Configuration.Connections(curCont.Device).Control(curCont.ContStr).Page(curCont.ContPage).InitialState)
    End Sub

    Private Sub nudControlGroup_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudControlGroup.ValueChanged
        If SetPage(True) Then
            FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage).ControlGroup = CByte(nudControlGroup.Value)
        End If
    End Sub

    Private Sub rdoMomentaryAbsolute_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoMomentaryAbsolute.CheckedChanged
        If SetPage(True) Then
            If rdoMomentaryAbsolute.Checked Then
                FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage).Behavior = 0
            Else
                FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage).Behavior = 1
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

    Private Sub nudDevicePage_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDevicePage.ValueChanged
        Dim _device As Integer = serviceHost.FindDeviceIndexByInput(_curCont.Device)
        'TODO: Remove pages that have not had any actions added
        'If (Not curCont.ContPage = 0) And (Configuration.Connections(_device).Control(curCont.ContStr).Page(curCont.ContPage).Actions.Count = 0) And (Configuration.Connections(_device).Control(curCont.ContStr).Page(curCont.ContPage).ActionsOff.Count = 0) Then
        '    ''No Actions on this page, so delete it
        '    Configuration.Connections(_device).Control(curCont.ContStr).Page(curCont.ContPage) = Nothing
        'End If
        FeelConfig.Connections(_device).PageCurrent = CByte(nudDevicePage.Value)
        If Not (FeelConfig.Connections(_device).Control(_curCont.ContStr).Page.ContainsKey(CByte(nudDevicePage.Value))) Then
            SetPage(True)
        End If
        lvActions.SelectedItems.Clear()
        With FeelConfig.Connections(_device).Control(_curCont.ContStr).Page(_curCont.ContPage)
            txtInitialState.Text = .InitialState
            nudControlGroup.Value = .ControlGroup
            If .Behavior = 0 Then
                rdoMomentaryAbsolute.Checked = True
            Else
                rdoLatchRelative.Checked = True
            End If
        End With
        PopulateActions()
    End Sub

    Private Sub grpInput_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grpInput.CheckedChanged
        holdControl = grpInput.Checked
    End Sub

    ''Generates a new Control in the Device Configuration, if not exist
    Private Function SetControl(ByVal createControl As Boolean) As Boolean
        If Not _curCont Is Nothing Then
            If Not (FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control.ContainsKey(_curCont.ContStr)) Then
                If createControl Then
                    Dim newControl As clsControl = New clsControl
                    FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control.Add(_curCont.ContStr, newControl)
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
        If Not _curCont Is Nothing Then
            If SetControl(createPage) Then
                If Not (FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page.ContainsKey(_curCont.ContPage)) Then
                    If createPage Then
                        Dim newPage As clsControlPage = New clsControlPage
                        FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page.Add(_curCont.ContPage, newPage)
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

#Region "Display Preference Conversions"
    Private Function DisplayChannel(ByVal channel As Byte) As String
        Return If(FeelConfig.MidiNumbering = 0, channel.ToString, (channel + 1).ToString)
    End Function

    Private Function DisplayNote(ByVal note As Byte) As String
        Select Case FeelConfig.MidiNotation
            Case 1, 2 ''Dec
                Return note.ToString & If(FeelConfig.MidiNotation = 2, "d", "")
            Case 3, 4 ''Hex
                Return If(FeelConfig.MidiNotation = 3, "0x" & note.ToString("X2"), note.ToString("X2") & "h")
            Case Else ''Word (0)
                Return DisplayNoteAsString(note)
        End Select
    End Function

    Private Function DisplayNoteAsString(ByVal note As Byte) As String
        If (FeelConfig.MidiTranspose = 1) Then
            Return [Enum].GetName(GetType(Midi.Pitch), note).Replace("Sharp", "#")
        Else
            'TODO: there's got to be a better way to do this (look in MIDI.NET's Pitch "Octave" method...)
            Dim _note As String = [Enum].GetName(GetType(Midi.Pitch), note).Replace("Sharp", "#")
            Dim _octave As Integer
            Dim _not As String
            If (note < 12) Then
                _octave = -1
                _not = Strings.Left(_note, _note.Length - 4)
            Else
                _not = Strings.Left(_note, _note.Length - 1)
                _octave = CInt(Strings.Right(_note, 1))
            End If
            Return If(FeelConfig.MidiTranspose = 0, _not & (_octave + 1).ToString, _not & (_octave - 1).ToString)
        End If
    End Function

    Private Function DisplayVelVal(ByVal velval As Byte) As String
        If (FeelConfig.MidiNotation = 3 Or FeelConfig.MidiNotation = 4) Then
            ''Hex
            Return velval.ToString("X2")
        Else
            ''Dec, or Word
            Return velval.ToString
        End If
    End Function
#End Region
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