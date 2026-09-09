USE GestionTarjetaCreditoDb;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.FinancialConfigurations
    WHERE Code = 'INTEREST_PERCENTAGE'
)
BEGIN
    INSERT INTO dbo.FinancialConfigurations
    (
        Code,
        Value,
        Description
    )
    VALUES
    (
        'INTEREST_PERCENTAGE',
        25.00,
        'Porcentaje de interés bonificable'
    );
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.FinancialConfigurations
    WHERE Code = 'MIN_PAYMENT_PERCENTAGE'
)
BEGIN
    INSERT INTO dbo.FinancialConfigurations
    (
        Code,
        Value,
        Description
    )
    VALUES
    (
        'MIN_PAYMENT_PERCENTAGE',
        5.00,
        'Porcentaje utilizado para calcular la cuota mínima'
    );
END
GO


DECLARE @CardHolderId INT;

SELECT @CardHolderId = Id
FROM dbo.CardHolders
WHERE FullName = 'Cliente Demostración';

IF @CardHolderId IS NULL
BEGIN
    INSERT INTO dbo.CardHolders
    (
        FullName
    )
    VALUES
    (
        'Cliente Demostración'
    );

    SET @CardHolderId = SCOPE_IDENTITY();
END


DECLARE @CreditCardId INT;

SELECT @CreditCardId = Id
FROM dbo.CreditCards
WHERE CardNumber = '4556123412344587';

IF @CreditCardId IS NULL
BEGIN
    INSERT INTO dbo.CreditCards
    (
        CardHolderId,
        CardNumber,
        CreditLimit,
        IsActive
    )
    VALUES
    (
        @CardHolderId,
        '4556123412344587',
        1000.00,
        1
    );

    SET @CreditCardId = SCOPE_IDENTITY();
END

DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);

DECLARE @CurrentMonthStart DATE =
    DATEFROMPARTS(
        YEAR(@CurrentDate),
        MONTH(@CurrentDate),
        1
    );

DECLARE @PreviousMonthStart DATE =
    DATEADD(MONTH, -1, @CurrentMonthStart);

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Transactions
    WHERE CreditCardId = @CreditCardId
)
BEGIN
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
        1,
        DATEADD(DAY, 2, @PreviousMonthStart),
        'Supermercado',
        50.00
    ),
    (
        @CreditCardId,
        1,
        DATEADD(DAY, 8, @PreviousMonthStart),
        'Gasolinera',
        40.00
    ),
    (
        @CreditCardId,
        1,
        DATEADD(DAY, 2, @CurrentMonthStart),
        'Restaurante',
        35.50
    ),
    (
        @CreditCardId,
        1,
        DATEADD(DAY, 4, @CurrentMonthStart),
        'Farmacia',
        42.97
    ),
    (
        @CreditCardId,
        1,
        DATEADD(DAY, 6, @CurrentMonthStart),
        'Supermercado',
        36.00
    );
END
GO


SELECT
    Id,
    FullName,
    CreatedDate
FROM dbo.CardHolders;

SELECT
    Id,
    CardHolderId,
    CardNumber,
    CreditLimit,
    IsActive,
    CreatedDate
FROM dbo.CreditCards;

SELECT
    Id,
    CreditCardId,
    TransactionType,
    TransactionDate,
    Description,
    Amount,
    CreatedDate
FROM dbo.Transactions
ORDER BY TransactionDate DESC;

SELECT
    Id,
    Code,
    Value,
    Description,
    CreatedDate,
    UpdatedDate
FROM dbo.FinancialConfigurations;