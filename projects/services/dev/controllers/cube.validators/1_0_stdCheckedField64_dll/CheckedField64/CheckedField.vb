Public Class CheckedField

    Property value As String

    Property found As Boolean

    Property breakonempty As Boolean

    Property runat As IValidationRoute.RunAt

    Property roomname As String

    Property formulafieldname As String

    Property requiredfields As List(Of String)

    Property conditions As List(Of String)

    Property errormessages As List(Of String)

    Property dependencies As List(Of Boolean)

    Property childfields As List(Of CheckedField)

End Class

Public Class MultiKeyItem

    Public fieldpedimento As [Enum]

    Public indexplus As String

End Class
