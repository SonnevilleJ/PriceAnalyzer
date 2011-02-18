
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/17/2011 21:01:36
-- Generated from EDMX file: C:\Dev\PriceAnalyzer\PriceTools\Entities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PriceAnalyzer];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CashTransaction_inherits_Transaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_CashTransaction] DROP CONSTRAINT [FK_CashTransaction_inherits_Transaction];
GO
IF OBJECT_ID(N'[dbo].[FK_Deposit_inherits_CashTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_Deposit] DROP CONSTRAINT [FK_Deposit_inherits_CashTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_Withdrawal_inherits_CashTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_Withdrawal] DROP CONSTRAINT [FK_Withdrawal_inherits_CashTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_ShareTransaction_inherits_Transaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_ShareTransaction] DROP CONSTRAINT [FK_ShareTransaction_inherits_Transaction];
GO
IF OBJECT_ID(N'[dbo].[FK_SellShort_inherits_ShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_SellShort] DROP CONSTRAINT [FK_SellShort_inherits_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_BuyToCover_inherits_ShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_BuyToCover] DROP CONSTRAINT [FK_BuyToCover_inherits_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_Sell_inherits_ShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_Sell] DROP CONSTRAINT [FK_Sell_inherits_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_DividendReceipt_inherits_ShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_DividendReceipt] DROP CONSTRAINT [FK_DividendReceipt_inherits_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_DividendReinvestment_inherits_ShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_DividendReinvestment] DROP CONSTRAINT [FK_DividendReinvestment_inherits_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_Buy_inherits_ShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_Buy] DROP CONSTRAINT [FK_Buy_inherits_ShareTransaction];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Transactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions];
GO
IF OBJECT_ID(N'[dbo].[Transactions_CashTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_CashTransaction];
GO
IF OBJECT_ID(N'[dbo].[Transactions_Deposit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_Deposit];
GO
IF OBJECT_ID(N'[dbo].[Transactions_Withdrawal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_Withdrawal];
GO
IF OBJECT_ID(N'[dbo].[Transactions_ShareTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[Transactions_SellShort]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_SellShort];
GO
IF OBJECT_ID(N'[dbo].[Transactions_BuyToCover]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_BuyToCover];
GO
IF OBJECT_ID(N'[dbo].[Transactions_Sell]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_Sell];
GO
IF OBJECT_ID(N'[dbo].[Transactions_DividendReceipt]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_DividendReceipt];
GO
IF OBJECT_ID(N'[dbo].[Transactions_DividendReinvestment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_DividendReinvestment];
GO
IF OBJECT_ID(N'[dbo].[Transactions_Buy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_Buy];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Transactions'
CREATE TABLE [dbo].[Transactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SettlementDate] datetime  NOT NULL
);
GO

-- Creating table 'Transactions_CashTransaction'
CREATE TABLE [dbo].[Transactions_CashTransaction] (
    [Amount] decimal(18,0)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_Deposit'
CREATE TABLE [dbo].[Transactions_Deposit] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_Withdrawal'
CREATE TABLE [dbo].[Transactions_Withdrawal] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_ShareTransaction'
CREATE TABLE [dbo].[Transactions_ShareTransaction] (
    [Shares] float  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Commission] decimal(18,0)  NOT NULL,
    [Ticker] nvarchar(max)  NOT NULL,
    [TransactionType] int  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_SellShort'
CREATE TABLE [dbo].[Transactions_SellShort] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_BuyToCover'
CREATE TABLE [dbo].[Transactions_BuyToCover] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_Sell'
CREATE TABLE [dbo].[Transactions_Sell] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_DividendReceipt'
CREATE TABLE [dbo].[Transactions_DividendReceipt] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_DividendReinvestment'
CREATE TABLE [dbo].[Transactions_DividendReinvestment] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_Buy'
CREATE TABLE [dbo].[Transactions_Buy] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Transactions'
ALTER TABLE [dbo].[Transactions]
ADD CONSTRAINT [PK_Transactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_CashTransaction'
ALTER TABLE [dbo].[Transactions_CashTransaction]
ADD CONSTRAINT [PK_Transactions_CashTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_Deposit'
ALTER TABLE [dbo].[Transactions_Deposit]
ADD CONSTRAINT [PK_Transactions_Deposit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_Withdrawal'
ALTER TABLE [dbo].[Transactions_Withdrawal]
ADD CONSTRAINT [PK_Transactions_Withdrawal]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_ShareTransaction'
ALTER TABLE [dbo].[Transactions_ShareTransaction]
ADD CONSTRAINT [PK_Transactions_ShareTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_SellShort'
ALTER TABLE [dbo].[Transactions_SellShort]
ADD CONSTRAINT [PK_Transactions_SellShort]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_BuyToCover'
ALTER TABLE [dbo].[Transactions_BuyToCover]
ADD CONSTRAINT [PK_Transactions_BuyToCover]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_Sell'
ALTER TABLE [dbo].[Transactions_Sell]
ADD CONSTRAINT [PK_Transactions_Sell]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_DividendReceipt'
ALTER TABLE [dbo].[Transactions_DividendReceipt]
ADD CONSTRAINT [PK_Transactions_DividendReceipt]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_DividendReinvestment'
ALTER TABLE [dbo].[Transactions_DividendReinvestment]
ADD CONSTRAINT [PK_Transactions_DividendReinvestment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_Buy'
ALTER TABLE [dbo].[Transactions_Buy]
ADD CONSTRAINT [PK_Transactions_Buy]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Id] in table 'Transactions_CashTransaction'
ALTER TABLE [dbo].[Transactions_CashTransaction]
ADD CONSTRAINT [FK_CashTransaction_inherits_Transaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_Deposit'
ALTER TABLE [dbo].[Transactions_Deposit]
ADD CONSTRAINT [FK_Deposit_inherits_CashTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_CashTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_Withdrawal'
ALTER TABLE [dbo].[Transactions_Withdrawal]
ADD CONSTRAINT [FK_Withdrawal_inherits_CashTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_CashTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_ShareTransaction'
ALTER TABLE [dbo].[Transactions_ShareTransaction]
ADD CONSTRAINT [FK_ShareTransaction_inherits_Transaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_SellShort'
ALTER TABLE [dbo].[Transactions_SellShort]
ADD CONSTRAINT [FK_SellShort_inherits_ShareTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_ShareTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_BuyToCover'
ALTER TABLE [dbo].[Transactions_BuyToCover]
ADD CONSTRAINT [FK_BuyToCover_inherits_ShareTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_ShareTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_Sell'
ALTER TABLE [dbo].[Transactions_Sell]
ADD CONSTRAINT [FK_Sell_inherits_ShareTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_ShareTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_DividendReceipt'
ALTER TABLE [dbo].[Transactions_DividendReceipt]
ADD CONSTRAINT [FK_DividendReceipt_inherits_ShareTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_ShareTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_DividendReinvestment'
ALTER TABLE [dbo].[Transactions_DividendReinvestment]
ADD CONSTRAINT [FK_DividendReinvestment_inherits_ShareTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_ShareTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_Buy'
ALTER TABLE [dbo].[Transactions_Buy]
ADD CONSTRAINT [FK_Buy_inherits_ShareTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_ShareTransaction]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------