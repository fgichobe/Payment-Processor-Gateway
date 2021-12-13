Imports System.Data.Entity
Imports System.Data.Entity.ModelConfiguration.Conventions

Public Class PaymentProcessorContext
    Inherits DbContext


    Public Property TransactionRequests() As DbSet(Of TransactionRequest)

    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        ' Configure Code First to ignore PluralizingTableName convention
        ' If you keep this convention then the generated tables
        ' will have pluralized names.
        modelBuilder.Conventions.Remove(Of PluralizingTableNameConvention)()



        ' Specifying the Maximum Length on a Property

        ' In the following example, the Name property
        ' should be no longer than 50 characters.
        ' If you make the value longer than 50 characters,
        ' you will get a DbEntityValidationException exception.
        modelBuilder.Entity(Of TransactionRequest)().Property(Function(t) t.QueueName).
            HasMaxLength(60)


        ' Configuring the Property to be Required

        ' In the following example, the Name property is required.
        ' If you do not specify the Name,
        ' you will get a DbEntityValidationException exception.
        ' The database column used to store this property will be non-nullable.
        modelBuilder.Entity(Of TransactionRequest)().Property(Function(t) t.QueueName).
            IsRequired()

    End Sub
End Class
