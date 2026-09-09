USE GestionTarjetaCreditoDb;
GO

IF OBJECT_ID('dbo.CardHolders', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CardHolders
    (
        Id INT IDENTITY(1,1) NOT NULL,
        FullName NVARCHAR(150) NOT NULL,
        CreatedDate DATETIME2 NOT NULL
            CONSTRAINT DF_CardHolders_CreatedDate DEFAULT SYSDATETIME(),

        CONSTRAINT PK_CardHolders
            PRIMARY KEY (Id)
    );
END
GO

IF OBJECT_ID('dbo.CreditCards', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CreditCards
    (
        Id INT IDENTITY(1,1) NOT NULL,
        CardHolderId INT NOT NULL,
        CardNumber VARCHAR(19) NOT NULL,
        CreditLimit DECIMAL(18,2) NOT NULL,
        IsActive BIT NOT NULL
            CONSTRAINT DF_CreditCards_IsActive DEFAULT 1,
        CreatedDate DATETIME2 NOT NULL
            CONSTRAINT DF_CreditCards_CreatedDate DEFAULT SYSDATETIME(),

        CONSTRAINT PK_CreditCards
            PRIMARY KEY (Id),

        CONSTRAINT FK_CreditCards_CardHolders
            FOREIGN KEY (CardHolderId)
            REFERENCES dbo.CardHolders(Id),

        CONSTRAINT CK_CreditCards_CreditLimit
            CHECK (CreditLimit > 0),

        CONSTRAINT UQ_CreditCards_CardNumber
            UNIQUE (CardNumber)
    );
END
GO

IF OBJECT_ID('dbo.Transactions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Transactions
    (
        Id INT IDENTITY(1,1) NOT NULL,
        CreditCardId INT NOT NULL,
        TransactionType INT NOT NULL,
        TransactionDate DATETIME2 NOT NULL,
        Description NVARCHAR(200) NOT NULL,
        Amount DECIMAL(18,2) NOT NULL,
        CreatedDate DATETIME2 NOT NULL
            CONSTRAINT DF_Transactions_CreatedDate DEFAULT SYSDATETIME(),

        CONSTRAINT PK_Transactions
            PRIMARY KEY (Id),

        CONSTRAINT FK_Transactions_CreditCards
            FOREIGN KEY (CreditCardId)
            REFERENCES dbo.CreditCards(Id),

        CONSTRAINT CK_Transactions_TransactionType
            CHECK (TransactionType IN (1, 2)),

        CONSTRAINT CK_Transactions_Amount
            CHECK (Amount > 0)
    );
END
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_Transactions_CreditCardId_TransactionDate'
      AND object_id = OBJECT_ID('dbo.Transactions')
)
BEGIN
    CREATE INDEX IX_Transactions_CreditCardId_TransactionDate
        ON dbo.Transactions
        (
            CreditCardId,
            TransactionDate DESC
        );
END
GO


IF OBJECT_ID('dbo.FinancialConfigurations', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.FinancialConfigurations
    (
        Id INT IDENTITY(1,1) NOT NULL,
        Code VARCHAR(100) NOT NULL,
        Value DECIMAL(9,4) NOT NULL,
        Description NVARCHAR(250) NOT NULL,
        CreatedDate DATETIME2 NOT NULL
            CONSTRAINT DF_FinancialConfigurations_CreatedDate DEFAULT SYSDATETIME(),
        UpdatedDate DATETIME2 NULL,

        CONSTRAINT PK_FinancialConfigurations
            PRIMARY KEY (Id),

        CONSTRAINT UQ_FinancialConfigurations_Code
            UNIQUE (Code),

        CONSTRAINT CK_FinancialConfigurations_Value
            CHECK (Value >= 0 AND Value <= 100)
    );
END
GO

