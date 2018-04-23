Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI.WebControls
Imports DevExpress.ExpressApp.Web
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Editors
Imports System.Text.RegularExpressions
Imports DevExpress.ExpressApp.Web.Editors.ASPx

Namespace HyperLinkPropertyEditor.Web
	<PropertyEditor(GetType(System.String), "HyperLinkPropertyEditor", False), CancelClickEventPropagation()> _
	Public Class HyperLinkPropertyEditor
		Inherits ASPxStringPropertyEditor
		Public Const UrlEmailMask As String = "(((http|https|ftp)\://)?[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;amp;%\$#\=~])*)|([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})"
		Public Sub New(ByVal objectType As Type, ByVal info As IModelMemberViewItem)
			MyBase.New(objectType, info)
		End Sub
		Protected Overrides Function CreateEditModeControlCore() As WebControl
			Dim textBox As ASPxTextBox = CType(MyBase.CreateEditModeControlCore(), ASPxTextBox)
			textBox.ValidationSettings.RegularExpression.ValidationExpression = UrlEmailMask
			Return textBox
		End Function
		Protected Overrides Function CreateViewModeControlCore() As WebControl
			Return CreateHyperLink()
		End Function
		Protected Overrides Sub ReadViewModeValueCore()
			MyBase.ReadViewModeValueCore()
			SetupHyperLink(PropertyValue)
		End Sub
		Private Shared Function GetResolvedUrl(ByVal value As Object) As String
			Dim url As String = Convert.ToString(value)
			If (Not String.IsNullOrEmpty(url)) Then
				If url.Contains("@") AndAlso IsValidUrl(url) Then
					Return String.Format("mailto:{0}", url)
				End If
				If (Not url.Contains("://")) Then
					url = String.Format("http://{0}", url)
				End If
				If IsValidUrl(url) Then
					Return url
				End If
			End If
			Return String.Empty
		End Function
		Private Shared Function IsValidUrl(ByVal url As String) As Boolean
			Return Regex.IsMatch(url, UrlEmailMask)
		End Function
		Private Function CreateHyperLink() As ASPxHyperLink
			Dim hyperlink As ASPxHyperLink = RenderHelper.CreateASPxHyperLink()
			Return hyperlink
		End Function
		Private Sub SetupHyperLink(ByVal value As Object)
			Dim hyperlink As ASPxHyperLink = CType(InplaceViewModeEditor, ASPxHyperLink)
			Dim url As String = Convert.ToString(value)
			hyperlink.Text = url
			hyperlink.NavigateUrl = GetResolvedUrl(url)
		End Sub
	End Class
End Namespace