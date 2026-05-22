Imports MongoDB.Bson

Public Class Incrementable
    Implements IIncrementable

    Public Property name As String Implements IIncrementable.name
    Public Property amount As Double Implements IIncrementable.amount
    Public Property currencyid As ObjectId Implements IIncrementable.currencyid
    Public Property currencykey As String Implements IIncrementable.currencykey

End Class
