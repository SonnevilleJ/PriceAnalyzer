
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/12/2011 22:27:53
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

IF OBJECT_ID(N'[dbo].[FK_PositionShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_ShareTransaction] DROP CONSTRAINT [FK_PositionShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_PortfolioPosition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Positions] DROP CONSTRAINT [FK_PortfolioPosition];
GO
IF OBJECT_ID(N'[dbo].[FK_CashAccountPortfolio]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Portfolios] DROP CONSTRAINT [FK_CashAccountPortfolio];
GO
IF OBJECT_ID(N'[dbo].[FK_CashAccountCashTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_CashTransaction] DROP CONSTRAINT [FK_CashAccountCashTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_QuotedPricePeriodPriceQuote]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PriceQuotes] DROP CONSTRAINT [FK_QuotedPricePeriodPriceQuote];
GO
IF OBJECT_ID(N'[dbo].[FK_PriceSeriesPricePeriod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PricePeriods] DROP CONSTRAINT [FK_PriceSeriesPricePeriod];
GO
IF OBJECT_ID(N'[dbo].[FK_ShareTransaction_inherits_Transaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_ShareTransaction] DROP CONSTRAINT [FK_ShareTransaction_inherits_Transaction];
GO
IF OBJECT_ID(N'[dbo].[FK_CashTransaction_inherits_Transaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_CashTransaction] DROP CONSTRAINT [FK_CashTransaction_inherits_Transaction];
GO
IF OBJECT_ID(N'[dbo].[FK_QuotedPricePeriod_inherits_PricePeriod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PricePeriods_QuotedPricePeriod] DROP CONSTRAINT [FK_QuotedPricePeriod_inherits_PricePeriod];
GO
IF OBJECT_ID(N'[dbo].[FK_PriceSeries_inherits_PricePeriod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PricePeriods_PriceSeries] DROP CONSTRAINT [FK_PriceSeries_inherits_PricePeriod];
GO
IF OBJECT_ID(N'[dbo].[FK_Deposit_inherits_CashTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_Deposit] DROP CONSTRAINT [FK_Deposit_inherits_CashTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_Withdrawal_inherits_CashTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_Withdrawal] DROP CONSTRAINT [FK_Withdrawal_inherits_CashTransaction];
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
IF OBJECT_ID(N'[dbo].[FK_DividendReceipt_inherits_CashTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_DividendReceipt] DROP CONSTRAINT [FK_DividendReceipt_inherits_CashTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_DividendReinvestment_inherits_ShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_DividendReinvestment] DROP CONSTRAINT [FK_DividendReinvestment_inherits_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_Buy_inherits_ShareTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transactions_Buy] DROP CONSTRAINT [FK_Buy_inherits_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_StaticPricePeriod_inherits_PricePeriod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PricePeriods_StaticPricePeriod] DROP CONSTRAINT [FK_StaticPricePeriod_inherits_PricePeriod];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Transactions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions];
GO
IF OBJECT_ID(N'[dbo].[Positions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Positions];
GO
IF OBJECT_ID(N'[dbo].[Portfolios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Portfolios];
GO
IF OBJECT_ID(N'[dbo].[CashAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CashAccounts];
GO
IF OBJECT_ID(N'[dbo].[PriceQuotes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PriceQuotes];
GO
IF OBJECT_ID(N'[dbo].[PricePeriods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PricePeriods];
GO
IF OBJECT_ID(N'[dbo].[Transactions_ShareTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_ShareTransaction];
GO
IF OBJECT_ID(N'[dbo].[Transactions_CashTransaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_CashTransaction];
GO
IF OBJECT_ID(N'[dbo].[PricePeriods_QuotedPricePeriod]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PricePeriods_QuotedPricePeriod];
GO
IF OBJECT_ID(N'[dbo].[PricePeriods_PriceSeries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PricePeriods_PriceSeries];
GO
IF OBJECT_ID(N'[dbo].[Transactions_Deposit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_Deposit];
GO
IF OBJECT_ID(N'[dbo].[Transactions_Withdrawal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transactions_Withdrawal];
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
IF OBJECT_ID(N'[dbo].[PricePeriods_StaticPricePeriod]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PricePeriods_StaticPricePeriod];
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

-- Creating table 'Positions'
CREATE TABLE [dbo].[Positions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Ticker] nvarchar(max)  NOT NULL,
    [Portfolio_Id] int  NULL
);
GO

-- Creating table 'Portfolios'
CREATE TABLE [dbo].[Portfolios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CashTicker] nvarchar(max)  NOT NULL,
    [CashAccount_Id] int  NOT NULL
);
GO

-- Creating table 'CashAccounts'
CREATE TABLE [dbo].[CashAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'PriceQuotes'
CREATE TABLE [dbo].[PriceQuotes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SettlementDate] datetime  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Volume] bigint  NULL,
    [QuotedPricePeriod_Id] int  NULL
);
GO

-- Creating table 'PricePeriods'
CREATE TABLE [dbo].[PricePeriods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PriceSeriesPricePeriod_PricePeriod_Id] int  NULL
);
GO

-- Creating table 'Transactions_ShareTransaction'
CREATE TABLE [dbo].[Transactions_ShareTransaction] (
    [Shares] float  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Commission] decimal(18,0)  NOT NULL,
    [Ticker] nvarchar(max)  NOT NULL,
    [EFTransactionType] int  NOT NULL,
    [Id] int  NOT NULL,
    [Position_Id] int  NULL
);
GO

-- Creating table 'Transactions_CashTransaction'
CREATE TABLE [dbo].[Transactions_CashTransaction] (
    [Amount] decimal(18,0)  NOT NULL,
    [EFTransactionType] int  NOT NULL,
    [Id] int  NOT NULL,
    [CashAccount_Id] int  NULL
);
GO

-- Creating table 'PricePeriods_QuotedPricePeriod'
CREATE TABLE [dbo].[PricePeriods_QuotedPricePeriod] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'PricePeriods_PriceSeries'
CREATE TABLE [dbo].[PricePeriods_PriceSeries] (
    [Ticker] nvarchar(max)  NOT NULL,
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

-- Creating table 'PricePeriods_StaticPricePeriod'
CREATE TABLE [dbo].[PricePeriods_StaticPricePeriod] (
    [EFOpen] decimal(18,0)  NULL,
    [EFHigh] decimal(18,0)  NULL,
    [EFLow] decimal(18,0)  NULL,
    [EFVolume] bigint  NULL,
    [EFClose] decimal(18,0)  NOT NULL,
    [EFHead] datetime  NOT NULL,
    [EFTail] datetime  NOT NULL,
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

-- Creating primary key on [Id] in table 'Positions'
ALTER TABLE [dbo].[Positions]
ADD CONSTRAINT [PK_Positions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Portfolios'
ALTER TABLE [dbo].[Portfolios]
ADD CONSTRAINT [PK_Portfolios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CashAccounts'
ALTER TABLE [dbo].[CashAccounts]
ADD CONSTRAINT [PK_CashAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PriceQuotes'
ALTER TABLE [dbo].[PriceQuotes]
ADD CONSTRAINT [PK_PriceQuotes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PricePeriods'
ALTER TABLE [dbo].[PricePeriods]
ADD CONSTRAINT [PK_PricePeriods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_ShareTransaction'
ALTER TABLE [dbo].[Transactions_ShareTransaction]
ADD CONSTRAINT [PK_Transactions_ShareTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_CashTransaction'
ALTER TABLE [dbo].[Transactions_CashTransaction]
ADD CONSTRAINT [PK_Transactions_CashTransaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PricePeriods_QuotedPricePeriod'
ALTER TABLE [dbo].[PricePeriods_QuotedPricePeriod]
ADD CONSTRAINT [PK_PricePeriods_QuotedPricePeriod]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PricePeriods_PriceSeries'
ALTER TABLE [dbo].[PricePeriods_PriceSeries]
ADD CONSTRAINT [PK_PricePeriods_PriceSeries]
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

-- Creating primary key on [Id] in table 'PricePeriods_StaticPricePeriod'
ALTER TABLE [dbo].[PricePeriods_StaticPricePeriod]
ADD CONSTRAINT [PK_PricePeriods_StaticPricePeriod]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Position_Id] in table 'Transactions_ShareTransaction'
ALTER TABLE [dbo].[Transactions_ShareTransaction]
ADD CONSTRAINT [FK_PositionShareTransaction]
    FOREIGN KEY ([Position_Id])
    REFERENCES [dbo].[Positions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PositionShareTransaction'
CREATE INDEX [IX_FK_PositionShareTransaction]
ON [dbo].[Transactions_ShareTransaction]
    ([Position_Id]);
GO

-- Creating foreign key on [Portfolio_Id] in table 'Positions'
ALTER TABLE [dbo].[Positions]
ADD CONSTRAINT [FK_PortfolioPosition]
    FOREIGN KEY ([Portfolio_Id])
    REFERENCES [dbo].[Portfolios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PortfolioPosition'
CREATE INDEX [IX_FK_PortfolioPosition]
ON [dbo].[Positions]
    ([Portfolio_Id]);
GO

-- Creating foreign key on [CashAccount_Id] in table 'Portfolios'
ALTER TABLE [dbo].[Portfolios]
ADD CONSTRAINT [FK_CashAccountPortfolio]
    FOREIGN KEY ([CashAccount_Id])
    REFERENCES [dbo].[CashAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CashAccountPortfolio'
CREATE INDEX [IX_FK_CashAccountPortfolio]
ON [dbo].[Portfolios]
    ([CashAccount_Id]);
GO

-- Creating foreign key on [CashAccount_Id] in table 'Transactions_CashTransaction'
ALTER TABLE [dbo].[Transactions_CashTransaction]
ADD CONSTRAINT [FK_CashAccountCashTransaction]
    FOREIGN KEY ([CashAccount_Id])
    REFERENCES [dbo].[CashAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CashAccountCashTransaction'
CREATE INDEX [IX_FK_CashAccountCashTransaction]
ON [dbo].[Transactions_CashTransaction]
    ([CashAccount_Id]);
GO

-- Creating foreign key on [QuotedPricePeriod_Id] in table 'PriceQuotes'
ALTER TABLE [dbo].[PriceQuotes]
ADD CONSTRAINT [FK_QuotedPricePeriodPriceQuote]
    FOREIGN KEY ([QuotedPricePeriod_Id])
    REFERENCES [dbo].[PricePeriods_QuotedPricePeriod]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuotedPricePeriodPriceQuote'
CREATE INDEX [IX_FK_QuotedPricePeriodPriceQuote]
ON [dbo].[PriceQuotes]
    ([QuotedPricePeriod_Id]);
GO

-- Creating foreign key on [PriceSeriesPricePeriod_PricePeriod_Id] in table 'PricePeriods'
ALTER TABLE [dbo].[PricePeriods]
ADD CONSTRAINT [FK_PriceSeriesPricePeriod]
    FOREIGN KEY ([PriceSeriesPricePeriod_PricePeriod_Id])
    REFERENCES [dbo].[PricePeriods_PriceSeries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PriceSeriesPricePeriod'
CREATE INDEX [IX_FK_PriceSeriesPricePeriod]
ON [dbo].[PricePeriods]
    ([PriceSeriesPricePeriod_PricePeriod_Id]);
GO

-- Creating foreign key on [Id] in table 'Transactions_ShareTransaction'
ALTER TABLE [dbo].[Transactions_ShareTransaction]
ADD CONSTRAINT [FK_ShareTransaction_inherits_Transaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_CashTransaction'
ALTER TABLE [dbo].[Transactions_CashTransaction]
ADD CONSTRAINT [FK_CashTransaction_inherits_Transaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PricePeriods_QuotedPricePeriod'
ALTER TABLE [dbo].[PricePeriods_QuotedPricePeriod]
ADD CONSTRAINT [FK_QuotedPricePeriod_inherits_PricePeriod]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PricePeriods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PricePeriods_PriceSeries'
ALTER TABLE [dbo].[PricePeriods_PriceSeries]
ADD CONSTRAINT [FK_PriceSeries_inherits_PricePeriod]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PricePeriods]
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
ADD CONSTRAINT [FK_DividendReceipt_inherits_CashTransaction]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Transactions_CashTransaction]
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

-- Creating foreign key on [Id] in table 'PricePeriods_StaticPricePeriod'
ALTER TABLE [dbo].[PricePeriods_StaticPricePeriod]
ADD CONSTRAINT [FK_StaticPricePeriod_inherits_PricePeriod]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PricePeriods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------