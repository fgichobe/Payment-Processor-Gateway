Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Threading
Imports System.Threading.Channels
Imports LogsClass
Imports Newtonsoft
Imports Newtonsoft.Json
Imports RabbitMQ.Client
Imports RabbitMQ.Client.Events
Imports RabbitMQ.Client.Exceptions
Imports RestSharp

Public Class Worker
    Private Const _queueName As String = "paymentRequest"
    Private _userName As String = String.Empty
    Private _password As String = String.Empty
    Private _url As String = String.Empty
    Private _uri As String = String.Empty
    Private _virtualHost As String = String.Empty
    Private _port As Integer = 15672
    Public LogOrigTxn As New LogClass
    Public LogOrigException As New LogExceptions

    Public Sub New()

        _url = "localhost"
        _port = "15672"
        _userName = "admin"
        _password = "admin"
        _virtualHost = "/"
        _uri = "amqp://admin:admin@localhost:5672/"
    End Sub


    Public Async Function DeQueueRequest() As Task




        Dim factory As ConnectionFactory = New ConnectionFactory() With {
                .HostName = "localhost",
                .Port = _port,
                .UserName = _userName,
                .Password = _password,
                .VirtualHost = _virtualHost,
                .Uri = New Uri(_uri),
                .DispatchConsumersAsync = True
            }
        Try

            factory.HandshakeContinuationTimeout = TimeSpan.FromSeconds(60)
            Dim connection As IConnection = factory.CreateConnection()


            Dim channel As IModel = connection.CreateModel()
            channel.QueueDeclare(queue:=_queueName, durable:=True, exclusive:=False, autoDelete:=False, arguments:=Nothing)
            channel.BasicQos(prefetchSize:=0, prefetchCount:=1, [global]:=False)

            Dim consumer = New AsyncEventingBasicConsumer(channel)
            AddHandler consumer.Received, Function(bc, ea)

                                              Dim message = Encoding.UTF8.GetString(ea.Body.ToArray()).ToString

                                              Try
                                                  Dim transactionRequestDTO = JsonConvert.DeserializeObject(Of TransactionRequestModel)(message)

                                                  If transactionRequestDTO IsNot Nothing Then
                                                      Dim tokenvalue As String = ""
                                                      tokenvalue = FunctionAuthToken()
                                                      tokenvalue.Trim()

                                                      Dim txn As String = ""

                                                      txn = transactionRequestDTO.RawRequest

                                                      Dim reposnesMessage = FunctionSendRequest(txn, tokenvalue)
                                                      'save to db 

                                                      channel.BasicAck(ea.DeliveryTag, False)
                                                  Else

                                                  End If


                                              Catch _unusedJsonException1_ As Exception


                                                  channel.BasicNack(ea.DeliveryTag, False, False)


                                              End Try

                                          End Function
            channel.BasicConsume(queue:=_queueName, autoAck:=False, consumer:=consumer)
            Await Task.CompletedTask
        Catch ex As Exception
            LogOrigException.LogIncomingExceptions(ex.Message)
        End Try

    End Function



    Public Function FunctionAuthToken() As String
        Try


            Dim client = New RestClient("http://54.224.92.175:10001/connect/token")
            client.Timeout = -1
            Dim request = New RestRequest(Method.POST)
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded")
            request.AddParameter("client_id", "PyPay_api")
            request.AddParameter("client_secret", "PyPayApiSecret")
            request.AddParameter("grant_type", "client_credentials")
            request.AddParameter("scope", "PyPay_api")
            request.AddParameter("ContentType", "application/json")
            Dim response As IRestResponse = client.Execute(request)
            Dim jss_token = JsonConvert.DeserializeObject(Of Object)(response.Content)
            Dim tokenval = jss_token("access_token")
            Return tokenval
        Catch ex As Exception
            LogOrigException.LogIncomingExceptions(ex.Message)
        End Try

    End Function

    Public Function FunctionSendRequest(ByVal txn As String, ByVal token As String) As ResponseMessage
        Try
            LogOrigTxn.LogIncomingTxns(txn)

            Dim client = New RestClient("https://sandbox.api.zamupay.com/v1/payment-order/new-order")
            client.Timeout = -1
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim request = New RestRequest(Method.POST)
            request.AddParameter("AUTHORIZATION Bearer", token)
            request.AddParameter("ContentType", "application/json")

            request.AddParameter(ParameterType.RequestBody, txn)
            Dim response As IRestResponse = client.Execute(request)
            'Dim jss_token = JsonConvert.DeserializeObject(Of Object)(response.Content)
            Dim data_Response = JsonConvert.DeserializeObject(Of ResponseMessage)(response.Content)
            Return data_Response
        Catch ex As Exception
            LogOrigException.LogIncomingExceptions(ex.Message)
        End Try
    End Function
End Class
