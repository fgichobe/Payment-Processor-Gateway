Imports System.Text
Imports Newtonsoft.Json
Imports RabbitMQ.Client

Namespace RequestPublisher
    Public Class PublishRequest

        Private Const _queueName As String = "paymentRequest"
        Private _userName As String = String.Empty
        Private _password As String = String.Empty
        Private _url As String = String.Empty
        Private _uri As String = String.Empty
        Private _virtualHost As String = String.Empty
        Private _port As Integer = 15672

        Public Sub New()

            _url = "localhost"
            _port = "15672"
            _userName = "admin"
            _password = "admin"
            _virtualHost = "/"
            _uri = "amqp://admin:admin@localhost:5672/"
        End Sub

        Public Async Function QueueRequest(ByVal transactionRequestDTO As TransactionRequestModel) As Task
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

                Dim connection As IConnection = factory.CreateConnection()


                Dim channel As IModel = connection.CreateModel()


                channel.QueueDeclare(queue:=transactionRequestDTO.QueueName, durable:=True, exclusive:=False, autoDelete:=False, arguments:=Nothing)
                Dim message As String = JsonConvert.SerializeObject(transactionRequestDTO)
                Dim body As Byte() = Encoding.UTF8.GetBytes(message)
                Dim properties As IBasicProperties = channel.CreateBasicProperties()
                properties.Persistent = True
                channel.BasicPublish(exchange:="", routingKey:=transactionRequestDTO.QueueName, basicProperties:=properties, body:=body)
            Catch ex As Exception

            End Try

            Await Task.CompletedTask
        End Function
    End Class
End Namespace
