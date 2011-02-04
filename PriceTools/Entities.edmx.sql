
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/03/2011 23:20:38
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------