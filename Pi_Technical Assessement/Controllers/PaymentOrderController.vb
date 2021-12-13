Imports System.Net
Imports System.Web.Http
Imports Business_Logic
Imports Newtonsoft.Json
Imports Pi_Technical_Assessement.PaymentOrderClass
Imports RequestConsumer
Imports RequestPublisher
Imports RequestPublisher.RequestPublisher

Namespace Controllers
    Public Class PaymentOrderController
        Inherits ApiController

        ' GET api/values

        Public Function GetValues() As IEnumerable(Of String)

            Using context As New PaymentProcessorContext()

            End Using
            Return New String() {"value1", "value2"}
        End Function

        ' POST api/paymentOrder
        Public Function PostValue(<FromBody()> ByVal value As TransactionRequest) As String
            Dim Req As New PublishRequest
            Dim TranRequest As New RequestPublisher.TransactionRequestModel

            TranRequest.QueueName = "paymentRequest"
            TranRequest.RawRequest = JsonConvert.SerializeObject(value)
            TranRequest.RawResponse = "Transaction Received successfully for processing"

            Req.QueueRequest(TranRequest)

            Return "Transaction Received successfully for processing"
        End Function


    End Class
End Namespace