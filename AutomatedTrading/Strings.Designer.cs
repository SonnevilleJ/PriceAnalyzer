﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sonneville.PriceTools.AutomatedTrading {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sonneville.PriceTools.AutomatedTrading.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter transactions cannot be null..
        /// </summary>
        internal static string PortfolioFactory_ConstructPortfolio_Parameter_transactions_cannot_be_null_ {
            get {
                return ResourceManager.GetString("PortfolioFactory_ConstructPortfolio_Parameter_transactions_cannot_be_null_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot add this transaction to the position..
        /// </summary>
        internal static string Position_AddTransaction_Cannot_add_this_transaction_to_the_position_ {
            get {
                return ResourceManager.GetString("Position_AddTransaction_Cannot_add_this_transaction_to_the_position_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ticker must not be null, empty, or whitespace..
        /// </summary>
        internal static string Position_OnTickerChanging_Ticker_must_not_be_null__empty__or_whitespace_ {
            get {
                return ResourceManager.GetString("Position_OnTickerChanging_Ticker_must_not_be_null__empty__or_whitespace_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter basket cannot be null..
        /// </summary>
        internal static string SecurityBasketExtensions_CalculateAnnualGrossReturn_Parameter_basket_cannot_be_null_ {
            get {
                return ResourceManager.GetString("SecurityBasketExtensions_CalculateAnnualGrossReturn_Parameter_basket_cannot_be_nu" +
                        "ll_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot execute this order..
        /// </summary>
        internal static string TradingAccount_Submit_Cannot_execute_this_order_ {
            get {
                return ResourceManager.GetString("TradingAccount_Submit_Cannot_execute_this_order_", resourceCulture);
            }
        }
    }
}
