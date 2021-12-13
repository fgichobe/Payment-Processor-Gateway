Imports System.IO
Imports System.Configuration.ConfigurationManager

Public Class LogExceptions
    Public Function LogIncomingExceptions(ByVal ExString As String) As String
        Dim source As String = "PaymentRequestSvr"
        Dim str_Date As String = Format(Now, "dd-MM-yyyy")
        Dim path As String = "D:\Stuffs\Projects\Pi\Logs\Exceptions"
        Dim record As String = Format(Now, "dd/MM/yyyy HH:mm:ss") & ", IncomingExceptions -:" & ExString & ", Source - " & source
        Dim filename As String = Format(Now, "ddMMyyyy")
        path = path + "\" + str_Date + "\"
        Try
            If Directory.Exists(path) = False Then
                Directory.CreateDirectory(path)
            Else
            End If
            path = path & filename & ".txt"
            If File.Exists(path) Then

                File.AppendAllText(path, vbCrLf & record)

            Else
                File.WriteAllText(path, vbCrLf & record)

            End If

        Catch ex As Exception
            Dim err As String = ""
            err = ex.Message

        End Try
    End Function
End Class
