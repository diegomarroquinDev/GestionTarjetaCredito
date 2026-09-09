USE GestionTarjetaCreditoDb;
GO

CREATE OR ALTER PROCEDURE dbo.sp_CreditCard_GetById
    @CreditCardId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CC.Id,
        CC.CardHolderId,
        CC.CardNumber,
        CC.CreditLimit,
        CC.IsActive,
        CC.CreatedDate,
        CH.FullName AS CardHolderName,
        CH.CreatedDate AS CardHolderCreatedDate
    FROM dbo.CreditCards AS CC
    INNER JOIN dbo.CardHolders AS CH
        ON CH.Id = CC.CardHolderId
    WHERE CC.Id = @CreditCardId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Transaction_GetMonthly
    @CreditCardId INT,
    @Year INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StartDate DATE =
        DATEFROMPARTS(@Year, @Month, 1);

    DECLARE @EndDate DATE =
        DATEADD(MONTH, 1, @StartDate);

    SELECT
        Id,
        CreditCardId,
        TransactionType,
        TransactionDate,
        Description,
        Amount,
        CreatedDate
    FROM dbo.Transactions
    WHERE CreditCardId = @CreditCardId
      AND TransactionDate >= @StartDate
      AND TransactionDate < @EndDate
    ORDER BY TransactionDate DESC, Id DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_FinancialConfiguration_GetByCode
    @Code VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Code,
        Value,
        Description,
        CreatedDate,
        UpdatedDate
    FROM dbo.FinancialConfigurations
    WHERE Code = @Code;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_FinancialConfiguration_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Code,
        Value,
        Description,
        CreatedDate,
        UpdatedDate
    FROM dbo.FinancialConfigurations
    ORDER BY Code;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_FinancialConfiguration_Update
    @Id INT,
    @Value DECIMAL(9,4),
    @UpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Value < 0 OR @Value > 100
    BEGIN
        THROW 50001,
              'El valor de configuración debe estar entre 0 y 100.',
              1;
    END;

    UPDATE dbo.FinancialConfigurations
    SET
        Value = @Value,
        UpdatedDate = ISNULL(@UpdatedDate, SYSDATETIME())
    WHERE Id = @Id;

    IF @@ROWCOUNT = 0
    BEGIN
        THROW 50002,
              'No se encontró la configuración financiera especificada.',
              1;
    END;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Transaction_GetByCreditCardId
    @CreditCardId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        CreditCardId,
        TransactionType,
        TransactionDate,
        Description,
        Amount,
        CreatedDate
    FROM dbo.Transactions
    WHERE CreditCardId = @CreditCardId
    ORDER BY TransactionDate DESC, Id DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Transaction_CreatePurchase
    @CreditCardId INT,
    @TransactionDate DATETIME2,
    @Description NVARCHAR(200),
    @Amount DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Amount <= 0
    BEGIN
        THROW 50003, 'El monto de la compra debe ser mayor que cero.', 1;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.CreditCards
        WHERE Id = @CreditCardId
          AND IsActive = 1
    )
    BEGIN
        THROW 50004, 'La tarjeta no existe o se encuentra inactiva.', 1;
    END;

    INSERT INTO dbo.Transactions
    (
        CreditCardId,
        TransactionType,
        TransactionDate,
        Description,
        Amount
    )
    VALUES
    (
        @CreditCardId,
        1, -- Purchase
        @TransactionDate,
        @Description,
        @Amount
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS TransactionId;
END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Transaction_CreatePayment
    @CreditCardId INT,
    @TransactionDate DATETIME2,
    @Amount DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Amount <= 0
    BEGIN
        THROW 50005, 'El monto del pago debe ser mayor que cero.', 1;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.CreditCards
        WHERE Id = @CreditCardId
          AND IsActive = 1
    )
    BEGIN
        THROW 50006, 'La tarjeta no existe o se encuentra inactiva.', 1;
    END;

    INSERT INTO dbo.Transactions
    (
        CreditCardId,
        TransactionType,
        TransactionDate,
        Description,
        Amount
    )
    VALUES
    (
        @CreditCardId,
        2, -- Payment
        @TransactionDate,
        N'Pago de tarjeta de crédito',
        @Amount
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS TransactionId;
END
GO