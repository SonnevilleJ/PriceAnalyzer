﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace Sonneville.PriceTools
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class Container : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new Container object using the connection string found in the 'Container' section of the application configuration file.
        /// </summary>
        public Container() : base("name=Container", "Container")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new Container object.
        /// </summary>
        public Container(string connectionString) : base(connectionString, "Container")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new Container object.
        /// </summary>
        public Container(EntityConnection connection) : base(connection, "Container")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Transaction> Transactions
        {
            get
            {
                if ((_Transactions == null))
                {
                    _Transactions = base.CreateObjectSet<Transaction>("Transactions");
                }
                return _Transactions;
            }
        }
        private ObjectSet<Transaction> _Transactions;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Transactions EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToTransactions(Transaction transaction)
        {
            base.AddObject("Transactions", transaction);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="Buy")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Buy : ShareTransaction
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Buy object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="settlementDate">Initial value of the SettlementDate property.</param>
        /// <param name="shares">Initial value of the Shares property.</param>
        /// <param name="price">Initial value of the Price property.</param>
        /// <param name="commission">Initial value of the Commission property.</param>
        /// <param name="ticker">Initial value of the Ticker property.</param>
        public static Buy CreateBuy(global::System.Int32 id, global::System.DateTime settlementDate, global::System.Double shares, global::System.Decimal price, global::System.Decimal commission, global::System.String ticker)
        {
            Buy buy = new Buy();
            buy.Id = id;
            buy.SettlementDate = settlementDate;
            buy.Shares = shares;
            buy.Price = price;
            buy.Commission = commission;
            buy.Ticker = ticker;
            return buy;
        }

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="BuyToCover")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class BuyToCover : ShareTransaction
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new BuyToCover object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="settlementDate">Initial value of the SettlementDate property.</param>
        /// <param name="shares">Initial value of the Shares property.</param>
        /// <param name="price">Initial value of the Price property.</param>
        /// <param name="commission">Initial value of the Commission property.</param>
        /// <param name="ticker">Initial value of the Ticker property.</param>
        public static BuyToCover CreateBuyToCover(global::System.Int32 id, global::System.DateTime settlementDate, global::System.Double shares, global::System.Decimal price, global::System.Decimal commission, global::System.String ticker)
        {
            BuyToCover buyToCover = new BuyToCover();
            buyToCover.Id = id;
            buyToCover.SettlementDate = settlementDate;
            buyToCover.Shares = shares;
            buyToCover.Price = price;
            buyToCover.Commission = commission;
            buyToCover.Ticker = ticker;
            return buyToCover;
        }

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="CashTransaction")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    [KnownTypeAttribute(typeof(Deposit))]
    [KnownTypeAttribute(typeof(Withdrawal))]
    public abstract partial class CashTransaction : Transaction
    {
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                OnAmountChanging(value);
                ReportPropertyChanging("Amount");
                _Amount = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Amount");
                OnAmountChanged();
            }
        }
        private global::System.Decimal _Amount;
        partial void OnAmountChanging(global::System.Decimal value);
        partial void OnAmountChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="Deposit")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Deposit : CashTransaction
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Deposit object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="settlementDate">Initial value of the SettlementDate property.</param>
        /// <param name="amount">Initial value of the Amount property.</param>
        public static Deposit CreateDeposit(global::System.Int32 id, global::System.DateTime settlementDate, global::System.Decimal amount)
        {
            Deposit deposit = new Deposit();
            deposit.Id = id;
            deposit.SettlementDate = settlementDate;
            deposit.Amount = amount;
            return deposit;
        }

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="DividendReceipt")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class DividendReceipt : ShareTransaction
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new DividendReceipt object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="settlementDate">Initial value of the SettlementDate property.</param>
        /// <param name="shares">Initial value of the Shares property.</param>
        /// <param name="price">Initial value of the Price property.</param>
        /// <param name="commission">Initial value of the Commission property.</param>
        /// <param name="ticker">Initial value of the Ticker property.</param>
        public static DividendReceipt CreateDividendReceipt(global::System.Int32 id, global::System.DateTime settlementDate, global::System.Double shares, global::System.Decimal price, global::System.Decimal commission, global::System.String ticker)
        {
            DividendReceipt dividendReceipt = new DividendReceipt();
            dividendReceipt.Id = id;
            dividendReceipt.SettlementDate = settlementDate;
            dividendReceipt.Shares = shares;
            dividendReceipt.Price = price;
            dividendReceipt.Commission = commission;
            dividendReceipt.Ticker = ticker;
            return dividendReceipt;
        }

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="DividendReinvestment")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class DividendReinvestment : ShareTransaction
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new DividendReinvestment object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="settlementDate">Initial value of the SettlementDate property.</param>
        /// <param name="shares">Initial value of the Shares property.</param>
        /// <param name="price">Initial value of the Price property.</param>
        /// <param name="commission">Initial value of the Commission property.</param>
        /// <param name="ticker">Initial value of the Ticker property.</param>
        public static DividendReinvestment CreateDividendReinvestment(global::System.Int32 id, global::System.DateTime settlementDate, global::System.Double shares, global::System.Decimal price, global::System.Decimal commission, global::System.String ticker)
        {
            DividendReinvestment dividendReinvestment = new DividendReinvestment();
            dividendReinvestment.Id = id;
            dividendReinvestment.SettlementDate = settlementDate;
            dividendReinvestment.Shares = shares;
            dividendReinvestment.Price = price;
            dividendReinvestment.Commission = commission;
            dividendReinvestment.Ticker = ticker;
            return dividendReinvestment;
        }

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="Sell")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Sell : ShareTransaction
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Sell object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="settlementDate">Initial value of the SettlementDate property.</param>
        /// <param name="shares">Initial value of the Shares property.</param>
        /// <param name="price">Initial value of the Price property.</param>
        /// <param name="commission">Initial value of the Commission property.</param>
        /// <param name="ticker">Initial value of the Ticker property.</param>
        public static Sell CreateSell(global::System.Int32 id, global::System.DateTime settlementDate, global::System.Double shares, global::System.Decimal price, global::System.Decimal commission, global::System.String ticker)
        {
            Sell sell = new Sell();
            sell.Id = id;
            sell.SettlementDate = settlementDate;
            sell.Shares = shares;
            sell.Price = price;
            sell.Commission = commission;
            sell.Ticker = ticker;
            return sell;
        }

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="SellShort")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class SellShort : ShareTransaction
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new SellShort object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="settlementDate">Initial value of the SettlementDate property.</param>
        /// <param name="shares">Initial value of the Shares property.</param>
        /// <param name="price">Initial value of the Price property.</param>
        /// <param name="commission">Initial value of the Commission property.</param>
        /// <param name="ticker">Initial value of the Ticker property.</param>
        public static SellShort CreateSellShort(global::System.Int32 id, global::System.DateTime settlementDate, global::System.Double shares, global::System.Decimal price, global::System.Decimal commission, global::System.String ticker)
        {
            SellShort sellShort = new SellShort();
            sellShort.Id = id;
            sellShort.SettlementDate = settlementDate;
            sellShort.Shares = shares;
            sellShort.Price = price;
            sellShort.Commission = commission;
            sellShort.Ticker = ticker;
            return sellShort;
        }

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="ShareTransaction")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    [KnownTypeAttribute(typeof(SellShort))]
    [KnownTypeAttribute(typeof(BuyToCover))]
    [KnownTypeAttribute(typeof(Sell))]
    [KnownTypeAttribute(typeof(DividendReceipt))]
    [KnownTypeAttribute(typeof(DividendReinvestment))]
    [KnownTypeAttribute(typeof(Buy))]
    public abstract partial class ShareTransaction : Transaction
    {
        #region Primitive Properties
    
        /// <summary>
        /// The number of shares traded.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Double Shares
        {
            get
            {
                return _Shares;
            }
            set
            {
                OnSharesChanging(value);
                ReportPropertyChanging("Shares");
                _Shares = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Shares");
                OnSharesChanged();
            }
        }
        private global::System.Double _Shares;
        partial void OnSharesChanging(global::System.Double value);
        partial void OnSharesChanged();
    
        /// <summary>
        /// The price at which the transaction took place.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Decimal Price
        {
            get
            {
                return _Price;
            }
            set
            {
                OnPriceChanging(value);
                ReportPropertyChanging("Price");
                _Price = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Price");
                OnPriceChanged();
            }
        }
        private global::System.Decimal _Price;
        partial void OnPriceChanging(global::System.Decimal value);
        partial void OnPriceChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Decimal Commission
        {
            get
            {
                return _Commission;
            }
            set
            {
                OnCommissionChanging(value);
                ReportPropertyChanging("Commission");
                _Commission = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Commission");
                OnCommissionChanged();
            }
        }
        private global::System.Decimal _Commission;
        partial void OnCommissionChanging(global::System.Decimal value);
        partial void OnCommissionChanged();
    
        /// <summary>
        /// The ticker of the security.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Ticker
        {
            get
            {
                return _Ticker;
            }
            set
            {
                OnTickerChanging(value);
                ReportPropertyChanging("Ticker");
                _Ticker = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Ticker");
                OnTickerChanged();
            }
        }
        private global::System.String _Ticker;
        partial void OnTickerChanging(global::System.String value);
        partial void OnTickerChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        protected global::System.Int32 TransactionType
        {
            get
            {
                return _TransactionType;
            }
            set
            {
                OnTransactionTypeChanging(value);
                ReportPropertyChanging("TransactionType");
                _TransactionType = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("TransactionType");
                OnTransactionTypeChanged();
            }
        }
        private global::System.Int32 _TransactionType;
        partial void OnTransactionTypeChanging(global::System.Int32 value);
        partial void OnTransactionTypeChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="Transaction")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    [KnownTypeAttribute(typeof(CashTransaction))]
    [KnownTypeAttribute(typeof(ShareTransaction))]
    public abstract partial class Transaction : EntityObject
    {
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime SettlementDate
        {
            get
            {
                return _SettlementDate;
            }
            set
            {
                OnSettlementDateChanging(value);
                ReportPropertyChanging("SettlementDate");
                _SettlementDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("SettlementDate");
                OnSettlementDateChanged();
            }
        }
        private global::System.DateTime _SettlementDate;
        partial void OnSettlementDateChanging(global::System.DateTime value);
        partial void OnSettlementDateChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="Withdrawal")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Withdrawal : CashTransaction
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Withdrawal object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="settlementDate">Initial value of the SettlementDate property.</param>
        /// <param name="amount">Initial value of the Amount property.</param>
        public static Withdrawal CreateWithdrawal(global::System.Int32 id, global::System.DateTime settlementDate, global::System.Decimal amount)
        {
            Withdrawal withdrawal = new Withdrawal();
            withdrawal.Id = id;
            withdrawal.SettlementDate = settlementDate;
            withdrawal.Amount = amount;
            return withdrawal;
        }

        #endregion
    
    }

    #endregion
    
}
