Imports System.Runtime.InteropServices
Imports System.Windows.Forms

''SendMessage(s) to ListView controls, used for "Select All"
'' Source: http://stackoverflow.com/questions/1019388/adding-a-select-all-shortcut-ctrl-a-to-a-net-listview
'' Source: http://stackoverflow.com/a/1118396/242584
<Diagnostics.DebuggerStepThrough()> _
Public Class NativeMethods
    Private Const LVM_FIRST As Integer = &H1000
    Private Const LVM_SETITEMSTATE As Integer = LVM_FIRST + 43

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Public Structure LVITEM
        Public mask As Integer
        Public iItem As Integer
        Public iSubItem As Integer
        Public state As Integer
        Public stateMask As Integer
        <MarshalAs(UnmanagedType.LPTStr)> _
        Public pszText As String
        Public cchTextMax As Integer
        Public iImage As Integer
        Public lParam As IntPtr
        Public iIndent As Integer
        Public iGroupId As Integer
        Public cColumns As Integer
        Public puColumns As IntPtr
    End Structure

    <DllImport("user32.dll", EntryPoint:="SendMessage", CharSet:=CharSet.Auto)> _
    Public Shared Function SendMessageLVItem(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByRef lvi As LVITEM) As IntPtr
    End Function

    ''' <summary>
    ''' Select all rows on the given listview
    ''' </summary>
    ''' <param name="list">The listview whose items are to be selected</param>
    Public Shared Sub SelectAllItems(ByVal list As ListView)
        NativeMethods.SetItemState(list, -1, 2, 2)
    End Sub

    ''' <summary>
    ''' Deselect all rows on the given listview
    ''' </summary>
    ''' <param name="list">The listview whose items are to be deselected</param>
    Public Shared Sub DeselectAllItems(ByVal list As ListView)
        NativeMethods.SetItemState(list, -1, 2, 0)
    End Sub

    ''' <summary>
    ''' Set the item state on the given item
    ''' </summary>
    ''' <param name="list">The listview whose item's state is to be changed</param>
    ''' <param name="itemIndex">The index of the item to be changed</param>
    ''' <param name="mask">Which bits of the value are to be set?</param>
    ''' <param name="value">The value to be set</param>
    Public Shared Sub SetItemState(ByVal list As ListView, ByVal itemIndex As Integer, ByVal mask As Integer, ByVal value As Integer)
        Dim lvItem As New LVITEM()
        lvItem.stateMask = mask
        lvItem.state = value
        SendMessageLVItem(list.Handle, LVM_SETITEMSTATE, itemIndex, lvItem)
    End Sub
End Class
