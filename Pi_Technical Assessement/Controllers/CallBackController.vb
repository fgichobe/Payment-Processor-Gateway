Imports System.Net
Imports System.Web.Http
Imports Newtonsoft.Json
Imports RequestPublisher.RequestPublisher

Namespace Controllers
    Public Class CallBackController
        Inherits ApiController

        ' POST api/callback
        Public Function PostValue(<FromBody()> ByVal value As CallBackResponse) As String
            Dim Req As New PublishRequest
            Dim TranRequest As New RequestPublisher.TransactionRequestModel

            TranRequest.QueueName = "callBackResponse"
            TranRequest.RawRequest = JsonConvert.SerializeObject(value)
            TranRequest.RawResponse = "Response Received successfully for processing"

            Req.QueueRequest(TranRequest)

            Return "Response Received successfully for processing"
        End Function
    End Class
End Namespace