Imports MongoDB.Bson

Public Interface IIncrementable
    Property name As String
    Property amount As Double
    Property currencyid As ObjectId
    Property currencykey As String

End Interface
