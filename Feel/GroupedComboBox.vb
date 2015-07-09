' A ComboBox Control With Grouping
' Bradley Smith - 2010/06/23

' From: http://www.brad-smith.info/blog/projects/dropdown-controls
' Blog: http://www.brad-smith.info/blog/archives/104
' Blog: http://www.brad-smith.info/blog/archives/477

Imports System.Collections
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel

''' <summary>
''' Represents a Windows combo box control that, when bound to a data source, is capable of 
''' displaying items in groups/categories.
''' </summary>
Public Class GroupedComboBox
	Inherits ComboBox
	Implements IComparer

	Private mBindingSource As BindingSource
	' used for change detection and grouping
	Private mGroupFont As Font
	' for painting
    Private mGroupMember As String = "Group"
	' name of group-by property
	Private mGroupProperty As PropertyDescriptor
	' used to get group-by values
	Private mInternalItems As ArrayList
	' internal sorted collection of items
	Private mTextFormatFlags As TextFormatFlags
	' used in measuring/painting
	''' <summary>
	''' Gets or sets the data source for this GroupedComboBox.
	''' </summary>
	Public Shadows Property DataSource() As Object
		Get
			' binding source should be transparent to the user
			Return If((mBindingSource IsNot Nothing), mBindingSource.DataSource, Nothing)
		End Get
		Set
			If value IsNot Nothing Then
				' wrap the object in a binding source and listen for changes
				mBindingSource = New BindingSource(value, [String].Empty)
				AddHandler mBindingSource.ListChanged, New ListChangedEventHandler(AddressOf mBindingSource_ListChanged)
				SyncInternalItems()
			Else
				' remove binding
				MyBase.DataSource = InlineAssignHelper(mBindingSource, Nothing)
			End If
		End Set
	End Property

	''' <summary>
	''' Gets a value indicating whether the drawing of elements in the list will be handled by user code. 
	''' </summary>
	<Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
	Public Shadows ReadOnly Property DrawMode() As DrawMode
		Get
			Return MyBase.DrawMode
		End Get
	End Property

	''' <summary>
	''' Gets or sets the property to use when grouping items in the list.
	''' </summary>
	Public Property GroupMember() As String
		Get
			Return mGroupMember
		End Get
		Set
			mGroupMember = value
			If mBindingSource IsNot Nothing Then
				SyncInternalItems()
			End If
		End Set
	End Property

	''' <summary>
	''' Initialises a new instance of the GroupedComboBox class.
	''' </summary>
	Public Sub New()
		MyBase.DrawMode = DrawMode.OwnerDrawVariable
		mGroupMember = [String].Empty
		mInternalItems = New ArrayList()
		mTextFormatFlags = TextFormatFlags.EndEllipsis Or TextFormatFlags.NoPrefix Or TextFormatFlags.SingleLine Or TextFormatFlags.VerticalCenter
	End Sub

	''' <summary>
	''' Explicit interface implementation for the IComparer.Compare method. Performs a two-tier comparison 
	''' on two list items so that the list can be sorted by group, then by display value.
	''' </summary>
	''' <param name="x"></param>
	''' <param name="y"></param>
	''' <returns></returns>
	Private Function IComparer_Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
		' compare the display values (and return the result if there is no grouping)
		Dim secondLevelSort As Integer = Comparer.[Default].Compare(GetItemText(x), GetItemText(y))
		If mGroupProperty Is Nothing Then
			Return secondLevelSort
		End If

		' compare the group values - if equal, return the earlier comparison
		Dim firstLevelSort As Integer = Comparer.[Default].Compare(Convert.ToString(mGroupProperty.GetValue(x)), Convert.ToString(mGroupProperty.GetValue(y)))

		If firstLevelSort = 0 Then
			Return secondLevelSort
		Else
			Return firstLevelSort
		End If
	End Function

	''' <summary>
	''' Determines whether the list item at the specified index is the start of a new group. In all 
	''' cases, populates the string respresentation of the group that the item belongs to.
	''' </summary>
	''' <param name="index"></param>
	''' <param name="groupText"></param>
	''' <returns></returns>
	Private Function IsGroupStart(index As Integer, ByRef groupText As String) As Boolean
		Dim isGroupStart__1 As Boolean = False
		groupText = [String].Empty

		If (mGroupProperty IsNot Nothing) AndAlso (index >= 0) AndAlso (index < Items.Count) Then
			' get the group value using the property descriptor
			groupText = Convert.ToString(mGroupProperty.GetValue(Items(index)))

			' this item is the start of a group if it is the first item with a group -or- if
			' the previous item has a different group
			If (index = 0) AndAlso (groupText <> [String].Empty) Then
				isGroupStart__1 = True
			ElseIf (index - 1) >= 0 Then
				Dim previousGroupText As String = Convert.ToString(mGroupProperty.GetValue(Items(index - 1)))
				If previousGroupText <> groupText Then
					isGroupStart__1 = True
				End If
			End If
		End If

		Return isGroupStart__1
	End Function

	''' <summary>
	''' Re-synchronises the internal sorted collection when the data source changes.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	Private Sub mBindingSource_ListChanged(sender As Object, e As ListChangedEventArgs)
		SyncInternalItems()
	End Sub

	''' <summary>
	''' When the control font changes, updates the font used to render group names.
	''' </summary>
	''' <param name="e"></param>
	Protected Overrides Sub OnFontChanged(e As EventArgs)
		MyBase.OnFontChanged(e)
		mGroupFont = New Font(Font, FontStyle.Bold)
	End Sub

	''' <summary>
	''' When the parent control changes, updates the font used to render group names.
	''' </summary>
	''' <param name="e"></param>
	Protected Overrides Sub OnParentChanged(e As EventArgs)
		MyBase.OnParentChanged(e)
		mGroupFont = New Font(Font, FontStyle.Bold)
	End Sub

	''' <summary>
	''' Performs custom painting for a list item.
	''' </summary>
	''' <param name="e"></param>
	Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
		MyBase.OnDrawItem(e)

		If (e.Index >= 0) AndAlso (e.Index < Items.Count) Then
			' get noteworthy states
			Dim comboBoxEdit As Boolean = (e.State And DrawItemState.ComboBoxEdit) = DrawItemState.ComboBoxEdit
			Dim selected As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected
			Dim noAccelerator As Boolean = (e.State And DrawItemState.NoAccelerator) = DrawItemState.NoAccelerator
			Dim disabled As Boolean = (e.State And DrawItemState.Disabled) = DrawItemState.Disabled
			Dim focus As Boolean = (e.State And DrawItemState.Focus) = DrawItemState.Focus

			' determine grouping
            Dim groupText As String = ""
			Dim isGroupStart__1 As Boolean = IsGroupStart(e.Index, groupText) AndAlso Not comboBoxEdit
			Dim hasGroup As Boolean = (groupText <> [String].Empty) AndAlso Not comboBoxEdit

			' the item text will appear in a different colour, depending on its state
			Dim textColor As Color
			If disabled Then
				textColor = SystemColors.GrayText
			ElseIf (comboBoxEdit AndAlso Focused AndAlso Not DroppedDown) OrElse selected Then
				textColor = SystemColors.HighlightText
			Else
				textColor = ForeColor
			End If

			' items will be indented if they belong to a group
            Dim itemBounds As Rectangle = Rectangle.FromLTRB(e.Bounds.X + (If(hasGroup, 12, 0)), e.Bounds.Y + (If(isGroupStart__1, (e.Bounds.Height \ 2), 0)), e.Bounds.Right, e.Bounds.Bottom)
            Dim groupBounds As New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, (e.Bounds.Height \ 2))

			If isGroupStart__1 AndAlso selected Then
				' ensure that the group header is never highlighted
				e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds)
				e.Graphics.FillRectangle(New SolidBrush(BackColor), groupBounds)
			Else
				' use the default background-painting logic
				e.DrawBackground()
			End If

			' render group header text
			If isGroupStart__1 Then
				TextRenderer.DrawText(e.Graphics, groupText, mGroupFont, groupBounds, ForeColor, mTextFormatFlags)
			End If

			' render item text
			TextRenderer.DrawText(e.Graphics, GetItemText(Items(e.Index)), Font, itemBounds, textColor, mTextFormatFlags)

			' paint the focus rectangle if required
			If focus AndAlso Not noAccelerator Then
				If isGroupStart__1 AndAlso selected Then
					' don't draw the focus rectangle around the group header
					ControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.FromLTRB(groupBounds.X, itemBounds.Y, itemBounds.Right, itemBounds.Bottom))
				Else
					' use default focus rectangle painting logic
					e.DrawFocusRectangle()
				End If
			End If
		End If
	End Sub

	''' <summary>
	''' Determines the size of a list item.
	''' </summary>
	''' <param name="e"></param>
	Protected Overrides Sub OnMeasureItem(e As MeasureItemEventArgs)
		MyBase.OnMeasureItem(e)

		e.ItemHeight = Font.Height

        Dim groupText As String = ""
		If IsGroupStart(e.Index, groupText) Then
			' the first item in each group will be twice as tall in order to accommodate the group header
			e.ItemHeight *= 2
			e.ItemWidth = Math.Max(e.ItemWidth, TextRenderer.MeasureText(e.Graphics, groupText, mGroupFont, New Size(e.ItemWidth, e.ItemHeight), mTextFormatFlags).Width)
		End If
	End Sub

	''' <summary>
	''' Rebuilds the internal sorted collection.
	''' </summary>
	Private Sub SyncInternalItems()
		' locate the property descriptor that corresponds to the value of GroupMember
		mGroupProperty = Nothing
		For Each descriptor As PropertyDescriptor In mBindingSource.GetItemProperties(Nothing)
			If descriptor.Name.Equals(mGroupMember) Then
				mGroupProperty = descriptor
				Exit For
			End If
		Next

		' rebuild the collection and sort using custom logic
		mInternalItems.Clear()
		For Each item As Object In mBindingSource
			mInternalItems.Add(item)
		Next
		mInternalItems.Sort(Me)

		' bind the underlying ComboBox to the sorted collection
		MyBase.DataSource = mInternalItems
	End Sub
	Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
		target = value
		Return value
	End Function
End Class
