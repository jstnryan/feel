Imports System.ComponentModel

Public Class frmEvents
    Dim DeviceList As Collections.Generic.Dictionary(Of String, Integer)

    ''' <summary>Currently active contrrol</summary>
    ''' <remarks>Used by <see cref="frmEvents">frmEvents</see>, this class holds details of the last control
    ''' which was manipulated on the connected devices.</remarks>
    <Diagnostics.DebuggerStepThrough()> _
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
        <Diagnostics.DebuggerStepThrough()> _
        Set(ByVal value As curControl)
            If Not (holdControl) Then
                ''Update last control's state, if available
                updateLastControl()

                ''Update the current control marker
                _curCont = value

                'TODO: Invoke in updateCurrentControl throws IndexOutOfRange exception if multiple items are selected in lvActions
                updateCurrentControl()
            End If
        End Set
    End Property

    '"Lock" last active control
    Dim holdControl As Boolean = False
    ''Container for Copy/Paste
    Dim CopyData As New clsCopiedActions

    <Diagnostics.DebuggerStepThrough()> _
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
                        Else
                            'TODO: not sure if I really need this...
                            If Not (.Control(_curCont.ContStr).DefaultState.IsNullOrEmpty) Then
                                serviceHost.SendMIDI(_device, .Control(_curCont.ContStr).DefaultState)
                            End If
                        End If
                    End If
                End With
            End If
        End If
    End Sub

    Delegate Sub updateCurrentControlCallback()
    <Diagnostics.DebuggerStepThrough()> _
    Public Sub updateCurrentControl()
        If Me.InvokeRequired Then
            Dim d As New updateCurrentControlCallback(AddressOf updateCurrentControl)
            Invoke(d)
        Else
            Dim _device As Integer = serviceHost.FindDeviceIndexByInput(_curCont.Device)

            'Update UI
            setLabels(FeelConfig.Connections(_device).Name, _curCont.Type, "Channel " & DisplayChannel(_curCont.Channel), If(_curCont.Type = "Note", DisplayNote(_curCont.NotCon), _curCont.NotCon.ToString), DisplayVelVal(_curCont.VelVal))

            ''If this is the first device control recieved, enable the form controls.
            If (chkPaged.Enabled = False) Then
                chkPaged.Enabled = True
                nudDevicePage.Enabled = True
                txtDefaultState.Enabled = True
                cmdEditDefaultState.Enabled = True
                grpConfiguration.Enabled = True
                grpActions.Enabled = True
            End If

            'TODO: stupid bugfix
            ' Background: when updating currentControl (touching a control), if multiple actions are selected,
            ' lvActions_ItemSelectionChanged loses track of indexes, and inevitably throws ArgumentOutOfRangeException
            ' which confusingly break at the Invoke above.
            RemoveHandler lvActions.ItemSelectionChanged, AddressOf lvActions_ItemSelectionChanged
            lvActions.Items.Clear()
            AddHandler lvActions.ItemSelectionChanged, AddressOf lvActions_ItemSelectionChanged

            SetControlStates()

            'TODO: Check to see if active control contains a page change action, if so, update page?
            'If False Then
            '    'change page
            'Else
            nudDevicePage.Value = _curCont.ContPage
            'End If

            ''Check to see if this control has been programmed, update UI with configuration data
            If SetControl(False) Then
                With FeelConfig.Connections(_device).Control(_curCont.ContStr)
                    chkPaged.Checked = .Paged
                    txtDefaultState.Text = .DefaultState
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
                txtDefaultState.Text = ""
                txtInitialState.Text = ""
                nudControlGroup.Value = 0
                rdoMomentaryAbsolute.Checked = True
            End If
        End If
    End Sub

    ''' <summary>Sets control properties according to <see cref="lvActions">lvActions</see> SelectedItems.</summary>
    <Diagnostics.DebuggerStepThrough()> _
    Private Sub SetControlStates()
        If (lvActions.SelectedItems.Count = 1) Then
            cmdActionAdd.Enabled = True
            cmdActionRemove.Enabled = True
            cmdActionUp.Enabled = True
            cmdActionDown.Enabled = True
            If (_curCont.Type = "Note") Then cmdActionSwap.Enabled = True
            cmdActionClear.Enabled = True
            grpAction.Enabled = True
        Else
            'TODO: support moving/removing multiple actions at the same time
            'cmdActionAdd.Enabled = True
            cmdActionRemove.Enabled = False
            cmdActionUp.Enabled = False
            cmdActionDown.Enabled = False
            cmdActionSwap.Enabled = False
            'cmdActionClear.Enabled = True
            txtActionName.Text = ""
            cboActionFunction.SelectedIndex = -1
            pgAction.SelectedObject = Nothing
            grpAction.Enabled = False
            'If (lvActions.SelectedItems.Count = 0) Then
            'End If
        End If
    End Sub

    Delegate Sub SetLblCallback(ByVal Device As String, ByVal Description As String, ByVal Channel As String, ByVal NoteControl As String, ByVal VelocityValue As String)
    <Diagnostics.DebuggerStepThrough()> _
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

    ''' <summary>Redraws all actions in Actions List, since sorting is so damned difficult</summary>
    <ComponentModel.Description("Redraws the Actions List after individual Actions are sorted."), _
        ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Always), _
        Diagnostics.DebuggerStepThrough()> _
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
        ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Always), _
        Diagnostics.DebuggerStepThrough()> _
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

    ''' <summary>Populates <see cref="cboActionFunction">cboActionFunction</see> with the currently available Action plugins.</summary>
    Private Sub PopulateActionFunctions()
        cboActionFunction.ValueMember = "Value"
        cboActionFunction.DisplayMember = "Display"
        cboActionFunction.GroupMember = "Group"

        Dim actionlist As New Collections.ArrayList()
        For Each actnMod As Collections.Generic.KeyValuePair(Of Guid, ActionInterface.IAction) In main.actionModules
            actionlist.Add(New With {Key .Value = actnMod.Value.UniqueID, Key .Group = actnMod.Value.Group, Key .Display = actnMod.Value.Name})
        Next
        cboActionFunction.DataSource = actionlist
        actionlist = Nothing
    End Sub

    ''' <summary>Form Load 'event'</summary>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        'TODO: clear the actions list of design-time items in the editor
        lvActions.Items.Clear()

        PopulateActionFunctions()
        ChangeDescriptionHeight(pgAction, 128)
        main.configMode = True

        MyBase.OnLoad(e)
    End Sub
    Private Sub frmActions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Form size: 765, 460
        ' New size: 765, 486
    End Sub

    '''<summary>Changes the height of the Description pane in a PropertyGrid</summary>
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

    Private Sub frmActions_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        main.configMode = False
    End Sub

    Private Sub frmActions_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        updateLastControl()
        'main.SaveConfiguration()
    End Sub

    ''' <summary>Track KeyDown events to control advanced editing inside <see cref="lvActions">lvActions</see>.</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub frmEvents_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        ''If Shift or Control keys are pressed, enable MultiSelect[ion]
        If (e.Shift Or e.Control) Then
            lvActions.MultiSelect = True
        End If

        ''If the X, C, V keys are pressed (along with Control), handle Cut, Copy, Paste operations
        If (e.Control) And (lvActions.Focused) Then
            If (e.KeyCode = Windows.Forms.Keys.X) Then ''Cut
                'TODO: Cut
            ElseIf (e.KeyCode = Windows.Forms.Keys.C) Then ''Copy
                With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                    CopyData = New clsCopiedActions
                    For Each itm As Windows.Forms.ListViewItem In lvActions.SelectedItems
                        'If (itm.Group Is lvActions.Groups(0)) Then
                        If (lvActions.Groups.IndexOf(itm.Group) = 0) Then
                            'Control Pressed / Control Changed (ActionOn)
                            CopyData.Actions.Add(.Actions(lvActions.Groups(lvActions.Groups.IndexOf(itm.Group)).Items.IndexOf(itm)))
                        Else
                            'Control Released (ActionOff)
                            CopyData.ActionsOff.Add(.ActionsOff(lvActions.Groups(lvActions.Groups.IndexOf(itm.Group)).Items.IndexOf(itm)))
                        End If
                    Next
                End With
            ElseIf (e.KeyCode = Windows.Forms.Keys.V) Then ''Paste
                With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                    For Each actn As clsAction In CopyData.Actions
                        .Actions.Add(ObjectCopier.Clone(actn))
                        ''Set the _available property appropriately. It is not retained in the copy because it is NonSerialized, and ObjectCopier.Clone uses serialization.
                        Dim tmpAct As Feel.clsAction = System.Linq.Enumerable.Last(.Actions)
                        tmpAct._available = main.CheckPluginAvailability(tmpAct.Type)
                    Next
                    For Each actn As clsAction In CopyData.ActionsOff
                        .ActionsOff.Add(ObjectCopier.Clone(actn))
                        ''Set the _available property (see above).
                        Dim tmpAct As Feel.clsAction = System.Linq.Enumerable.Last(.Actions)
                        tmpAct._available = main.CheckPluginAvailability(tmpAct.Type)
                    Next
                    PopulateActions()
                End With
            ElseIf (e.KeyCode = Windows.Forms.Keys.D) Then ''Select None
                lvActions.SelectedItems.Clear()
                'NativeMethods.DeselectAllItems(lvActions)
            ElseIf (e.KeyCode = Windows.Forms.Keys.A) Then ''Select All
                NativeMethods.SelectAllItems(lvActions)
            End If
        End If
    End Sub

    Private Sub frmEvents_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        ''If Shift or Control keys are release, disable MultiSelect[ion]
        If Not (e.Shift Or e.Control) Then
            lvActions.MultiSelect = False
        End If
    End Sub

    Private Sub cboActionFunction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboActionFunction.SelectedIndexChanged
        If (lvActions.SelectedItems.Count = 1) Then
            Dim targetGuid As Guid = CType(cboActionFunction.SelectedValue, Guid)
            If Not (targetGuid = Guid.Empty) Then
                txtActionDescription.Text = main.actionModules.Item(targetGuid).Description
                With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                    Dim whichGroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
                    Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(lvActions.SelectedItems(0))
                    ''If this is a new action, assign new .Data
                    If (cboActionFunctionQualify(whichGroup, curIndex)) Then
                        If (whichGroup = 1) Then
                            .ActionsOff(curIndex).Type = targetGuid
                            .ActionsOff(curIndex).Data = ObjectCopier.Clone(main.actionModules.Item(.ActionsOff(curIndex).Type).Data)
                            .ActionsOff(curIndex)._available = True
                        Else
                            .Actions(curIndex).Type = targetGuid
                            .Actions(curIndex).Data = ObjectCopier.Clone(main.actionModules.Item(.Actions(curIndex).Type).Data)
                            .Actions(curIndex)._available = True
                        End If
                    Else
                        ''An action has already been assigned, test to see if targetGuid is different, and reassign
                        If (whichGroup = 1) Then
                            If Not (.ActionsOff(curIndex).Type = targetGuid) Then
                                .ActionsOff(curIndex).Type = targetGuid
                                .ActionsOff(curIndex).Data = ObjectCopier.Clone(main.actionModules.Item(.ActionsOff(curIndex).Type).Data)
                                .ActionsOff(curIndex)._available = True
                            End If
                        Else
                            If Not (.Actions(curIndex).Type = targetGuid) Then
                                .Actions(curIndex).Type = targetGuid
                                .Actions(curIndex).Data = ObjectCopier.Clone(main.actionModules.Item(.Actions(curIndex).Type).Data)
                                .Actions(curIndex)._available = True
                            End If
                        End If
                    End If
                    pgAction.SelectedObject = If(whichGroup = 1, .ActionsOff(curIndex).Data, .Actions(curIndex).Data)
                End With
            End If
        Else
            txtActionDescription.Text = ""
            cboActionFunction.SelectedIndex = -1
        End If
    End Sub
    ''' <summary>Tests to see if an Action needs to be assigned to a clsAction</summary>
    ''' <param name="whichGroup">The index of the group the selected Action belongs to in Actions ListView control.</param>
    ''' <param name="curIndex">The selected Action's current index in the group.</param>
    ''' <returns>True if .Data Is Nothing, otherwise False.</returns>
    <Diagnostics.DebuggerStepThrough()> _
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

    ''The ItemSelectionChanged event occurs whether the item state changes from selected to deselected or deselected to selected.
    ''The SelectedIndexChanged event occurs in single selection ListView controls, whenever there is a change to the index position of the selected item. In a multiple selection ListView control, this event occurs whenever an item is removed or added to the list of selected items.
    Private Sub lvActions_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvActions.ItemSelectionChanged
        If (lvActions.SelectedItems.Count = 1) Then
            Dim whichgroup As Integer = lvActions.Groups.IndexOf(lvActions.SelectedItems(0).Group)
            'TODO: This is a fix for some nasty bug that is seemingly impossible to track
            'If (whichgroup < 0) Then Exit Sub
            Dim curindex As Integer = lvActions.Groups(whichgroup).Items.IndexOf(lvActions.SelectedItems(0))
            With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                If (whichgroup = 1) Then
                    txtActionName.Text = .ActionsOff(curindex).Name
                    Try
                        cboActionFunction.SelectedValue = .ActionsOff(curindex).Type
                    Catch ex As Exception
                        Windows.MessageBox.Show("This action module is not currently loaded.")
                    End Try
                    'pgaction.selectedobject = .actionsoff(curindex).data ''Happens automatically in cboActionFunction_SelectedIndexChanged()
                Else
                    txtActionName.Text = .Actions(curindex).Name
                    Try
                        cboActionFunction.SelectedValue = .Actions(curindex).Type
                    Catch ex As Exception
                        Windows.MessageBox.Show("This action module is not currently loaded.")
                    End Try
                    'pgaction.selectedobject = .actions(curindex).data ''Happens automatically in cboActionFunction_SelectedIndexChanged()
                End If
            End With
            'Else
            ''copy/paste mode or nothing selected
        End If
        SetControlStates()

        'TODO: This isn't quite right, Paste could be used, even though there are no items selected.
        If (lvActions.SelectedItems.Count > 0) Then
            lvActions.ContextMenuStrip = cmsCopyPaste
        Else
            lvActions.ContextMenuStrip = Nothing
        End If
    End Sub
    'Private Sub lvActions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvActions.SelectedIndexChanged
    'End Sub

    Private Sub lvActions_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lvActions.ItemCheck
        ''Workaround for FullRowSelect + MultiSelect errant [un-]checking of lines
        '' See: http://www.vbforums.com/showthread.php?379405-RESOLVED-Listview-with-checkboxes-and-multiselect
        ''
        '' This also causes any (copied then) pasted action to be set to Disabled.
        '' At the moment, the original error seems to have ceased, so this is here only as reference.
        'If (lvActions.MultiSelect) Then
        '    e.NewValue = e.CurrentValue
        'End If
    End Sub

    Private Sub lvActions_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvActions.ItemChecked
        If (SetPage(False)) Then
            Dim whichGroup As Integer = lvActions.Groups.IndexOf((e.Item.Group))
            Dim curIndex As Integer = lvActions.Groups(whichGroup).Items.IndexOf(e.Item)
            With FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage)
                If (whichGroup = 1) Then
                    .ActionsOff(curIndex).Enabled = e.Item.Checked
                Else
                    .Actions(curIndex).Enabled = e.Item.Checked
                End If
            End With
        End If
    End Sub

    Private Sub cmdActionClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdActionClear.Click
        If Windows.Forms.MessageBox.Show("Are you sure you want to clear all actions associated with this control?", "Feel: Confirm Clear Actions", Windows.Forms.MessageBoxButtons.YesNo, Windows.Forms.MessageBoxIcon.Exclamation, Windows.Forms.MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            lvActions.Items.Clear()
            If SetPage(False) Then
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
        lvActions.SelectedItems.Clear()
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

    ''' <summary>Courtesy copying of values between similar fields</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>It is likely that these two fields will have the same value (or at least similar), so if they have not been previously set, they are set to equal values on first input. See also <see cref="txtInitialState_Leave">txtInitialState_Leave</see>.</remarks>
    Private Sub txtDefaultState_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDefaultState.Leave
        If (Not txtDefaultState.Text.IsNullOrEmpty) AndAlso (txtInitialState.Text.IsNullOrEmpty) Then
            txtInitialState.Text = txtDefaultState.Text
        End If
    End Sub

    Private Sub txtDefaultState_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDefaultState.TextChanged
        If SetControl(True) Then
            'TODO: Sanitize this input
            FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).DefaultState = txtDefaultState.Text
        Else
            ''Should never hit this block
            Windows.Forms.MessageBox.Show("unable to set this value")
        End If
    End Sub

    ''' <summary>Courtesy copying of values between similar fields</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>It is likely that these two fields will have the same value (or at least similar), so if they have not been previously set, they are set to equal values on first input. See also <see cref="txtDefaultState_Leave">txtDefaultState_Leave</see>.</remarks>
    Private Sub txtInitialState_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInitialState.Leave
        If (Not txtInitialState.Text.IsNullOrEmpty) AndAlso (txtDefaultState.Text.IsNullOrEmpty) Then
            txtDefaultState.Text = txtInitialState.Text
        End If
    End Sub

    Private Sub txtInitialState_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtInitialState.TextChanged
        If SetPage(True) Then
            'TODO: Sanitize this input
            FeelConfig.Connections(serviceHost.FindDeviceIndexByInput(_curCont.Device)).Control(_curCont.ContStr).Page(_curCont.ContPage).InitialState = txtInitialState.Text
        Else
            'TODO: need this?
            ''error that should never happen
            Windows.Forms.MessageBox.Show("unable to set this value")
        End If
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
    ''Shouldn't need this, because rdoMomentaryAbsolute should take care of it
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

    ''' <summary>Checks whether a Control exists on the currently active Page and Device, optionally creates it if not.</summary>
    ''' <param name="createControl">Create the Control if it doesn't exist (default: False).</param>
    ''' <returns>True if the Control exists, or was created, otherwise False.</returns>
    <Diagnostics.DebuggerStepThrough()> _
    Private Function SetControl(Optional ByVal createControl As Boolean = False) As Boolean
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

    ''' <summary>Checks whether a Page exists on the currently active Device, optionally creates it if not.</summary>
    ''' <param name="createPage">Create the Pontrol if it doesn't exist (default: False).</param>
    ''' <returns>True if the Pontrol exists, or was created, otherwise False.</returns>
    <Diagnostics.DebuggerStepThrough()> _
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

    Private Sub cmdEditDefaultState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditDefaultState.Click
        Using newEditor As frmMidiEditor = New frmMidiEditor(txtDefaultState.Text)
            If (newEditor.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                txtDefaultState.Text = newEditor.midiData.StringFormat
            End If
        End Using
    End Sub

    Private Sub cmdEditInitalState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditInitalState.Click
        Using newEditor As frmMidiEditor = New frmMidiEditor(txtInitialState.Text)
            If (newEditor.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
                txtInitialState.Text = newEditor.midiData.StringFormat
            End If
        End Using
    End Sub

#Region "Display Preference Conversions"
    ''' <summary>Formats a MIDI Channel string, according to user preferences.</summary>
    ''' <param name="channel">The MIDI Channel number to format.</param>
    ''' <returns>Formatted Channel string.</returns>
    <Diagnostics.DebuggerStepThrough()> _
    Friend Function DisplayChannel(ByVal channel As Byte) As String
        Return If(FeelConfig.MidiNumbering = 0, channel.ToString, (channel + 1).ToString)
    End Function

    ''' <summary>Formats a MIDI Note string according to user preferences.</summary>
    ''' <param name="note">The MIDI Note to format.</param>
    ''' <returns>Formatted Note string.</returns>
    <Diagnostics.DebuggerStepThrough()> _
    Friend Function DisplayNote(ByVal note As Byte) As String
        Select Case FeelConfig.MidiNotation
            Case 1, 2 ''Dec
                Return note.ToString & If(FeelConfig.MidiNotation = 2, "d", "")
            Case 3, 4 ''Hex
                Return If(FeelConfig.MidiNotation = 3, "0x" & note.ToString("X2"), note.ToString("X2") & "h")
            Case Else ''Word (0)
                Return DisplayNoteAsString(note)
        End Select
    End Function

    ''' <summary>Converts a numeric MIDI Note into a friendly word string.</summary>
    ''' <param name="note">The MIDI Note to format.</param>
    ''' <returns>Formatted Note string.</returns>
    <Diagnostics.DebuggerStepThrough()> _
    Friend Function DisplayNoteAsString(ByVal note As Byte) As String
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

    ''' <summary>Formats a MIDI Velocity or MIDI Value according to user preferences.</summary>
    ''' <param name="velval">The MIDI Velocity or Value to format.</param>
    ''' <returns>Formatted Velocity or Value string.</returns>
    <Diagnostics.DebuggerStepThrough()> _
    Friend Function DisplayVelVal(ByVal velval As Byte) As String
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

''Keeping this solely because I found a little way to make shit transparent
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