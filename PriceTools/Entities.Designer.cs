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
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("Entities", "PositionShareTransaction", "Position", System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(Sonneville.PriceTools.Position), "ShareTransaction", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(Sonneville.PriceTools.ShareTransaction))]

#endregion

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
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Position> Positions
        {
            get
            {
                if ((_Positions == null))
                {
                    _Positions = base.CreateObjectSet<Position>("Positions");
                }
                return _Positions;
            }
        }
        private ObjectSet<Position> _Positions;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Transactions EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToTransactions(Transaction transaction)
        {
            base.AddObject("Transactions", transaction);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Positions EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToPositions(Position position)
        {
            base.AddObject("Positions", position);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="CashTransaction")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
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
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        private global::System.Int32 EFTransactionType
        {
            get
            {
                return _EFTransactionType;
            }
            set
            {
                OnEFTransactionTypeChanging(value);
                ReportPropertyChanging("EFTransactionType");
                _EFTransactionType = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("EFTransactionType");
                OnEFTransactionTypeChanged();
            }
        }
        private global::System.Int32 _EFTransactionType;
        partial void OnEFTransactionTypeChanging(global::System.Int32 value);
        partial void OnEFTransactionTypeChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="Position")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Position : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Position object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="ticker">Initial value of the Ticker property.</param>
        public static Position CreatePosition(global::System.Int32 id, global::System.String ticker)
        {
            Position position = new Position();
            position.Id = id;
            position.Ticker = ticker;
            return position;
        }

        #endregion
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
        /// Gets the ticker symbol held by this Position.
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

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// A collection of all &lt;see cref = &quot;IShareTransaction&quot; /&gt;s in this IPosition.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("Entities", "PositionShareTransaction", "ShareTransaction")]
        private EntityCollection<ShareTransaction> EFTransactions
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<ShareTransaction>("Entities.PositionShareTransaction", "ShareTransaction");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<ShareTransaction>("Entities.PositionShareTransaction", "ShareTransaction", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="ShareTransaction")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
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
        private global::System.Int32 EFTransactionType
        {
            get
            {
                return _EFTransactionType;
            }
            set
            {
                OnEFTransactionTypeChanging(value);
                ReportPropertyChanging("EFTransactionType");
                _EFTransactionType = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("EFTransactionType");
                OnEFTransactionTypeChanged();
            }
        }
        private global::System.Int32 _EFTransactionType;
        partial void OnEFTransactionTypeChanging(global::System.Int32 value);
        partial void OnEFTransactionTypeChanged();

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("Entities", "PositionShareTransaction", "Position")]
        public Position Position
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Position>("Entities.PositionShareTransaction", "Position").Value;
            }
            private set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Position>("Entities.PositionShareTransaction", "Position").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Position> PositionReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Position>("Entities.PositionShareTransaction", "Position");
            }
            private set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Position>("Entities.PositionShareTransaction", "Position", value);
                }
            }
        }

        #endregion
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Entities", Name="Transaction")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    [KnownTypeAttribute(typeof(ShareTransaction))]
    [KnownTypeAttribute(typeof(CashTransaction))]
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

    #endregion
    
}
