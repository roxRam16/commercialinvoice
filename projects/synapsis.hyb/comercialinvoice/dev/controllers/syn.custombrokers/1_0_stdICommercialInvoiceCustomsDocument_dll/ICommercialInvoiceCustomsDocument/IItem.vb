Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

Public Interface IItem

    <BsonIgnoreIfNull>
    Property sequence As Integer

    Property productid As ObjectId

    Property sku As String

    Property partnumber As String

    Property quantity As Integer

    Property unit As String

    Property description As String

    Property total As Double

    Property currency As String

    Property usdvalue As Double

    Property value As Double

    Property discount As Decimal

    Property unitprice As Double

    Property netweight As Double

    Property purchaseorder As String

    Property destinationcountry As String

    '--------------new-------------- clavedestinationcountry
    Property destinationcountrykey As String

    Property origincountry As String
    '--------------new-------------- claveorigincountry
    Property origincountrykey As String


    ' descriptions
    '--------------new-------------- pedimentodescription
    Property customsdeclarationdescription As String

    '--------------new-------------- acusevalordescription
    Property valuecertificatedescription As String

    '--------------new-------------- descriptionpartnumber
    Property partnumberdescription As String

    '--------------new-------------- cvepartnumber
    Property partnumberkey As String

    ' commercial unit
    '--------------new-------------- quantitycomercial
    Property commercialquantity As Double

    '--------------new--------------
    Property unitquantitycommercial As Integer

    '--------------new--------------descriptionunitquantitycommercial
    Property commercialunitdescription As String

    ' merchandise
    '--------------new--------------
    Property usdcurrency As String

    '--------------new-------------- mercancyvalue
    Property merchandisevalue As Double

    '--------------new-------------- currencymercancy
    Property merchandisecurrency As String

    ' unit values
    '--------------new--------------
    Property valueunitprice As Double

    '--------------new--------------
    Property currencyunitprice As String


    '--------------new-------------- cvemetodovaloracion
    Property valuationmethodkey As Integer

    '--------------new-------------- descriptionmetodovaloracion
    Property valuationmethoddescription As String

    ' tariff
    '--------------new-------------- fraccionarancelaria
    Property tarifffraction As String

    '--------------new-------------- descripcionfraccionarancelaria
    Property tarifffractiondescription As String

    '--------------new-------------- quantitytarifa
    Property tariffquantity As Double

    '--------------new-------------- cveunitquantitytarifa
    Property tariffunitkey As Integer

    '--------------new-------------- descriptionunitquantitytarifa
    Property tariffunitdescription As String

    ' identifiers
    '--------------new--------------
    Property nico As String

    '--------------new-------------- descripcionnico
    Property nicodescription As String

    '--------------new--------------lote
    Property lot As String

    '--------------new-------------- serie
    Property serial As String

    '--------------new-------------- marca
    Property brand As String

    '--------------new-------------- modelo
    Property model As String

    '--------------new-------------- submodelo
    Property submodel As String

    '--------------new-------------- kilometraje
    Property mileage As Integer

    ' state
    '--------------new--------------
    Property status As Short

    '--------------new-------------- archivado
    Property archived As Boolean

    Property valorAgregado As Double

    Property monedaValorAgregado As String

    Property currencyvalueunitprice As String

End Interface
