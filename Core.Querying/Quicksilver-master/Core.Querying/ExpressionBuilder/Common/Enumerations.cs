using Core.Querying.ExpressionBuilder.Attributes;
using EPiServer.Find;

namespace Core.Querying.ExpressionBuilder.Common
{
    /// <summary>
    /// Defines how the filter statements will be connected to each other.
    /// </summary>
    public enum FilterStatementConnector
    {
        /// <summary>
        /// Determines that both the current AND the next filter statement needs to be satisfied.
        /// </summary>
        And,
        /// <summary>
        /// Determines that the current OR the next filter statement needs to be satisfied.
        /// </summary>
        Or
    }

    /// <summary>
    /// Defines the operations supported by the <seealso cref="FilterBuilder{T}" />.
    /// </summary>
    public enum FilterOperation
    {
        /// <summary>
        /// Targets an object in which the property's value is after the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        After,

        /// <summary>
        /// Targets an object in which the property's value is after the provided value from now.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(0)]
        AfterNow,

        /// <summary>
        /// Targets an object in which the property's value is begin with any word of the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        AnyWordBeginsWith,

        /// <summary>
        /// Targets an object in which the property's value is before the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        Before,

        /// <summary>
        /// Targets an object in which the property's value is before the provided value from now.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(0)]
        BeforeNow,

        /// <summary>
        /// Targets an object in which the provided value is count in the property's value (as a list).
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        Count,

        /// <summary>
        /// Targets an object in which the provided value is exist in the property's value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(0)]
        Exists,

        /// <summary>
        /// Targets an object in which the property's value is greater than the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        GreaterThan,

        /// <summary>
        /// Targets an object in which the property's value is greater than the provided value from now.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        GreaterThanNow,

        /// <summary>
        /// Targets an object in which the provided value is presented in the property's value (as a list).
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(2)]
        In,

        /// <summary>
        /// Targets an object in which the provided value is presented in the property's value (as a list).
        /// </summary>
        /// <remarks>Accepts fours value.</remarks>
        [NumberOfValues(4)]
        InRange,

        /// <summary>
        /// Targets an object in which the property's value is less than the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        LessThan,

        /// <summary>
        /// Targets an object in which the property's value is less than the provided value from now.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        LessThanNow,

        /// <summary>
        /// Targets an object in which the property's value is match to the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        Match,

        /// <summary>
        /// Targets an object in which the property's value is match case in sensitive to the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        MatchCaseInsensitive,

        /// <summary>
        /// Targets an object in which the property's value is match contained to the provided value.
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(2)]
        MatchContained,

        /// <summary>
        /// Targets an object in which the property's value is match contained case in sensitive the provided value.
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(2)]
        MatchContainedCaseInsensitive,

        /// <summary>
        /// Targets an object in which the property's value is match day the provided value.
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(2)]
        MatchDay,

        /// <summary>
        /// Targets an object in which the property's value is match month the provided value.
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(2)]
        MatchMonth,

        /// <summary>
        /// Targets an object in which the property's value is match type the provided value.
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(1)]
        MatchType,

        /// <summary>
        /// Targets an object in which the property's value is match type hierarchy the provided value.
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(1)]
        MatchTypeHierarchy,
        

        /// <summary>
        /// Targets an object in which the property's value is match type the provided value.
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(1)]
        MatchYear,

        /// <summary>
        /// Targets an object in which the property's value is within type the provided value (as a list).
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(1)]
        Within,

        /// <summary>
        /// Targets an object in which the property's value is Within distance from type the provided value (as a list).
        /// </summary>
        /// <remarks>Accepts two values.</remarks>
        [NumberOfValues(2)]
        WithinDistanceFrom,

        /// <summary>
        /// Targets an object in which the property's value is within type the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        Prefix,

        /// <summary>
        /// Targets an object in which the property's value is prefix case in sensitive type the provided value.
        /// </summary>
        /// <remarks>Accepts one value.</remarks>
        [NumberOfValues(1)]
        PrefixCaseInsensitive,
    }


    public enum FacetOperation
    {
        TermsFacetFor,
        TermsFacetForWordsIn,
        RangeFacetFor,
        HistogramFacetFor,
        DateHistogramFacetFor,
        GeoDistanceFacetFor,
        StatisticalFacetFor
    }

    public enum SortingOperation
    {

        OrderBy,
        OrderByDescending,
        ThenBy,
        ThenByDescending
    }
}
