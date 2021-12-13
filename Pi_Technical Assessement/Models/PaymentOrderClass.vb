Imports Newtonsoft.Json



Public Class Recipient

        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("address")>
        Public Property Address As String

        <JsonProperty("emailAddress")>
        Public Property EmailAddress As String

        <JsonProperty("phoneNumber")>
        Public Property PhoneNumber As String

        <JsonProperty("idType")>
        Public Property IdType As String

        <JsonProperty("idNumber")>
        Public Property IdNumber As String

        <JsonProperty("financialInstitution")>
        Public Property FinancialInstitution As String

        <JsonProperty("primaryAccountNumber")>
        Public Property PrimaryAccountNumber As String

        <JsonProperty("mccmnc")>
        Public Property Mccmnc As String

        <JsonProperty("ccy")>
        Public Property Ccy As Integer

        <JsonProperty("country")>
        Public Property Country As String

        <JsonProperty("purpose")>
        Public Property Purpose As String
    End Class

    Public Class Transaction

        <JsonProperty("routeId")>
        Public Property RouteId As String

        <JsonProperty("ChannelType")>
        Public Property ChannelType As Integer

        <JsonProperty("amount")>
        Public Property Amount As Integer

        <JsonProperty("reference")>
        Public Property Reference As String

        <JsonProperty("systemTraceAuditNumber")>
        Public Property SystemTraceAuditNumber As String
    End Class

    Public Class PaymentOrderLine

        <JsonProperty("recipient")>
        Public Property Recipient As Recipient

        <JsonProperty("transaction")>
        Public Property Transaction As Transaction
    End Class

Public Class TransactionRequest

    <JsonProperty("originatorConversationId")>
    Public Property OriginatorConversationId As String

    <JsonProperty("paymentNotes")>
    Public Property PaymentNotes As String

    <JsonProperty("paymentOrderLines")>
    Public Property PaymentOrderLines As PaymentOrderLine()
End Class


