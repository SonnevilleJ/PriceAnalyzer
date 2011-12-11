﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.488
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sonneville.PriceTools {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sonneville.PriceTools.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to Failed to access the Internet to download price data..
        /// </summary>
        internal static string DownloadPricesToCsv_InternetAccessFailed {
            get {
                return ResourceManager.GetString("DownloadPricesToCsv_InternetAccessFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument index must be a date within the span of this Indicator..
        /// </summary>
        internal static string IndicatorError_Argument_index_must_be_a_date_within_the_span_of_this_Indicator {
            get {
                return ResourceManager.GetString("IndicatorError_Argument_index_must_be_a_date_within_the_span_of_this_Indicator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CSV data is corrupt; closing price cannot be null for any period..
        /// </summary>
        internal static string ParseError_CSV_data_is_corrupt__closing_price_cannot_be_null_for_any_period {
            get {
                return ResourceManager.GetString("ParseError_CSV_data_is_corrupt__closing_price_cannot_be_null_for_any_period", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parsed date was returned as null or whitespace..
        /// </summary>
        internal static string ParseError_Parsed_date_was_returned_as_null_or_whitespace {
            get {
                return ResourceManager.GetString("ParseError_Parsed_date_was_returned_as_null_or_whitespace", resourceCulture);
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
        ///   Looks up a localized string similar to Unable to determine PriceSeriesResolution of data periods in CSV data..
        /// </summary>
        internal static string PriceHistoryCsvFile_DetermineResolution_Unable_to_determine_PriceSeriesResolution_of_data_periods_in_CSV_data_ {
            get {
                return ResourceManager.GetString("PriceHistoryCsvFile_DetermineResolution_Unable_to_determine_PriceSeriesResolution" +
                        "_of_data_periods_in_CSV_data_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to get head using Price Series Resolution: {0}.
        /// </summary>
        internal static string PriceHistoryCsvFile_GetHead_Unable_to_get_head_using_Price_Series_Resolution {
            get {
                return ResourceManager.GetString("PriceHistoryCsvFile_GetHead_Unable_to_get_head_using_Price_Series_Resolution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to get tail using Price Series Resolution: {0}.
        /// </summary>
        internal static string PriceHistoryCsvFile_GetTail_Unable_to_get_tail_using_Price_Series_Resolution {
            get {
                return ResourceManager.GetString("PriceHistoryCsvFile_GetTail_Unable_to_get_tail_using_Price_Series_Resolution", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Given duration represents an unknown PriceSeriesResolution..
        /// </summary>
        internal static string PriceHistoryCsvFile_SetResolution_Given_duration_represents_an_unknown_PriceSeriesResolution_ {
            get {
                return ResourceManager.GetString("PriceHistoryCsvFile_SetResolution_Given_duration_represents_an_unknown_PriceSerie" +
                        "sResolution_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current settings prevent connection to the Internet..
        /// </summary>
        internal static string PriceSeriesProvider_DownloadPricesToCsv_Current_settings_prevent_connection_to_the_Internet_ {
            get {
                return ResourceManager.GetString("PriceSeriesProvider_DownloadPricesToCsv_Current_settings_prevent_connection_to_th" +
                        "e_Internet_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Commission for {0} transactions must be 0..
        /// </summary>
        internal static string ShareTransaction_Commission_Commission_for__0__transactions_must_be_0_ {
            get {
                return ResourceManager.GetString("ShareTransaction_Commission_Commission_for__0__transactions_must_be_0_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Commission must be greater than or equal to 0..
        /// </summary>
        internal static string ShareTransaction_Commission_Commission_must_be_greater_than_or_equal_to_0_ {
            get {
                return ResourceManager.GetString("ShareTransaction_Commission_Commission_must_be_greater_than_or_equal_to_0_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shares must be greater than or equal to 0..
        /// </summary>
        internal static string ShareTransaction_OnSharesChanging_Shares_must_be_greater_than_or_equal_to_0_ {
            get {
                return ResourceManager.GetString("ShareTransaction_OnSharesChanging_Shares_must_be_greater_than_or_equal_to_0_", resourceCulture);
            }
        }
    }
}
