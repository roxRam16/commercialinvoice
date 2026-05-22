Imports System.Xml.Serialization
Imports System.Xml
Imports System.Collections.Generic
Imports System.IO

' ======================================================
' COMPROBANTE HIBRIDO (SOPORTA 3.3 Y 4.0)
' ======================================================
<XmlRoot("Comprobante")>
Public Class CFDIFacturaComercial

    <XmlAttribute("Version")> Public Property Version As String
    <XmlAttribute("Folio")> Public Property Folio As String
    <XmlAttribute("Fecha")> Public Property Fecha As DateTime
    <XmlAttribute("SubTotal")> Public Property SubTotal As Decimal
    <XmlAttribute("Total")> Public Property Total As Decimal
    <XmlAttribute("Moneda")> Public Property Moneda As String
    <XmlAttribute("TipoCambio")> Public Property TipoCambio As Decimal
    <XmlAttribute("Exportacion")> Public Property Exportacion As String ' Nuevo 4.0

    <XmlElement("Emisor")> Public Property Emisor As CfdiEmisor
    <XmlElement("Receptor")> Public Property Receptor As CfdiReceptor

    <XmlArray("Conceptos")>
    <XmlArrayItem("Concepto")>
    Public Property Conceptos As List(Of CfdiConcepto)

    <XmlElement("Complemento")> Public Property Complemento As CfdiComplemento

    ' ======================
    ' ATAJOS DE NEGOCIO (RECUPERADOS)
    ' ======================
    <XmlIgnore>
    Public ReadOnly Property UUID As String
        Get
            Return Complemento?.Timbre?.UUID
        End Get
    End Property

    <XmlIgnore>
    Public ReadOnly Property EmisorDomicilio As CceDomicilio
        Get
            Return Complemento?.ComercioExterior?.Emisor?.Domicilio
        End Get
    End Property

    <XmlIgnore>
    Public ReadOnly Property ReceptorDomicilio As CceDomicilio
        Get
            Return Complemento?.ComercioExterior?.Receptor?.Domicilio
        End Get
    End Property

    <XmlIgnore>
    Public ReadOnly Property DestinatarioDomicilio As CceDomicilio
        Get
            Return Complemento?.ComercioExterior?.Destinatario?.Domicilio
        End Get
    End Property

    <XmlIgnore>
    Public ReadOnly Property Items As List(Of CommercialInvoiceItem)
        Get
            If Conceptos Is Nothing OrElse Complemento?.ComercioExterior?.Mercancias Is Nothing Then
                Return New List(Of CommercialInvoiceItem)
            End If

            Return (
                From c In Conceptos
                Join m In Complemento.ComercioExterior.Mercancias
                    On c.NoIdentificacion Equals m.NoIdentificacion
                Select New CommercialInvoiceItem With {
                    .Concepto = c,
                    .Mercancia = m
                }
            ).ToList()
        End Get
    End Property
End Class

' ======================================================
' EMISOR / RECEPTOR
' ======================================================
Public Class CfdiEmisor
    <XmlAttribute("Rfc")> Public Property Rfc As String
    <XmlAttribute("Nombre")> Public Property Nombre As String
    <XmlAttribute("RegimenFiscal")> Public Property RegimenFiscal As String
End Class

Public Class CfdiReceptor
    <XmlAttribute("Rfc")> Public Property Rfc As String
    <XmlAttribute("Nombre")> Public Property Nombre As String
    <XmlAttribute("ResidenciaFiscal")> Public Property ResidenciaFiscal As String
    <XmlAttribute("NumRegIdTrib")> Public Property NumRegIdTrib As String
    <XmlAttribute("UsoCFDI")> Public Property UsoCFDI As String
    <XmlAttribute("RegimenFiscalReceptor")> Public Property RegimenFiscalReceptor As String ' Nuevo 4.0
End Class

' ======================================================
' CONCEPTOS
' ======================================================
Public Class CfdiConcepto
    <XmlAttribute("ClaveProdServ")> Public Property ClaveProdServ As String
    <XmlAttribute("NoIdentificacion")> Public Property NoIdentificacion As String
    <XmlAttribute("Cantidad")> Public Property Cantidad As Decimal
    <XmlAttribute("ClaveUnidad")> Public Property ClaveUnidad As String
    <XmlAttribute("Unidad")> Public Property Unidad As String
    <XmlAttribute("Descripcion")> Public Property Descripcion As String
    <XmlAttribute("ValorUnitario")> Public Property ValorUnitario As Decimal
    <XmlAttribute("Importe")> Public Property Importe As Decimal
    <XmlAttribute("ObjetoImp")> Public Property ObjetoImp As String ' Nuevo 4.0
End Class

' ======================================================
' COMPLEMENTOS
' ======================================================
'Public Class CfdiComplemento
'    <XmlElement("ComercioExterior", Namespace:="http://www.sat.gob.mx/ComercioExterior11")>
'    Public Property ComercioExterior As ComercioExterior

'    <XmlElement("TimbreFiscalDigital", Namespace:="http://www.sat.gob.mx/TimbreFiscalDigital")>
'    Public Property Timbre As TimbreFiscalDigital
'End Class

'<XmlRoot("TimbreFiscalDigital", Namespace:="http://www.sat.gob.mx/TimbreFiscalDigital")>
'Public Class TimbreFiscalDigital
'    <XmlAttribute("UUID")> Public Property UUID As String
'    <XmlAttribute("FechaTimbrado")> Public Property FechaTimbrado As DateTime
'End Class


Public Class CfdiComplemento
    <XmlElement("ComercioExterior")>
    Public Property ComercioExterior As ComercioExterior

    ' ⚠️ QUITA EL NAMESPACE AQUÍ - El TimbreFiscalDigital ya lo maneja en su propia clase
    <XmlElement("TimbreFiscalDigital")>
    Public Property Timbre As TimbreFiscalDigital
End Class

' ✅ El namespace debe estar solo en el XmlRoot de la clase
<XmlRoot("TimbreFiscalDigital", Namespace:="http://www.sat.gob.mx/TimbreFiscalDigital")>
Public Class TimbreFiscalDigital
    <XmlAttribute("UUID")> Public Property UUID As String
    <XmlAttribute("FechaTimbrado")> Public Property FechaTimbrado As DateTime
End Class

' ======================================================
' COMERCIO EXTERIOR 1.1
' ======================================================
'<XmlRoot("ComercioExterior", Namespace:="http://www.sat.gob.mx/ComercioExterior11")>
<XmlRoot("ComercioExterior")>
Public Class ComercioExterior
    <XmlAttribute("Incoterm")> Public Property Incoterm As String
    <XmlAttribute("TipoCambioUSD")> Public Property TipoCambioUSD As Decimal
    <XmlAttribute("TotalUSD")> Public Property TotalUSD As Decimal

    <XmlElement("Emisor")> Public Property Emisor As CceParte
    <XmlElement("Receptor")> Public Property Receptor As CceParte
    <XmlElement("Destinatario")> Public Property Destinatario As CceParte

    <XmlArray("Mercancias")>
    <XmlArrayItem("Mercancia")>
    Public Property Mercancias As List(Of CceMercancia)
End Class

Public Class CceParte
    <XmlElement("NumRegIdTrib")> Public Property NumRegIdTrib As String
    <XmlElement("Nombre")> Public Property Nombre As String
    <XmlElement("Domicilio")> Public Property Domicilio As CceDomicilio
End Class

Public Class CceDomicilio
    <XmlAttribute("Calle")> Public Property Calle As String
    <XmlAttribute("NumeroExterior")> Public Property NumeroExterior As String
    <XmlAttribute("NumeroInterior")> Public Property NumeroInterior As String
    <XmlAttribute("Ciudad")> Public Property Ciudad As String
    <XmlAttribute("Colonia")> Public Property Colonia As String
    <XmlAttribute("Localidad")> Public Property Localidad As String
    <XmlAttribute("Municipio")> Public Property Municipio As String
    <XmlAttribute("Estado")> Public Property Estado As String
    <XmlAttribute("Pais")> Public Property Pais As String
    <XmlAttribute("CodigoPostal")> Public Property CodigoPostal As String
End Class

' ======================================================
' MERCANCIAS (ATRIBUTOS COMPLETOS)
' ======================================================
Public Class CceMercancia
    <XmlAttribute("NoIdentificacion")> Public Property NoIdentificacion As String
    <XmlAttribute("FraccionArancelaria")> Public Property FraccionArancelaria As String
    <XmlAttribute("CantidadAduana")> Public Property CantidadAduana As Decimal
    <XmlAttribute("UnidadAduana")> Public Property UnidadAduana As String
    <XmlAttribute("ValorUnitarioAduana")> Public Property ValorUnitarioAduana As Decimal
    <XmlAttribute("ValorDolares")> Public Property ValorDolares As Decimal
End Class

' ======================================================
' MODELO DE UNION
' ======================================================
Public Class CommercialInvoiceItem
    Public Property Concepto As CfdiConcepto
    Public Property Mercancia As CceMercancia

    Public ReadOnly Property FraccionArancelaria As String
        Get
            Return Mercancia?.FraccionArancelaria
        End Get
    End Property
End Class
