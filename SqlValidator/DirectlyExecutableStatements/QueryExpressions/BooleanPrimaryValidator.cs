
namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;

public static class BooleanPrimaryValidator
{
    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (CheckCommonValueExpression(input, out remainder)) return true;
        if (ExistsPredicateValidator.Validate(input, out remainder)) return true;

        return XmlQueryValidator.Validate(input, out remainder);
    }

    private static bool CheckCommonValueExpression(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!CommonValueExpressionValidator.Validate(input, out remainder)) return false;
        return CheckPredicateExpressions(remainder, out remainder);
    }

    private static bool CheckPredicateExpressions(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (BetweenPredicateValidator.Validate(input, out remainder)) return true;
        if (MatchPredicateValidator.Validate(input, out remainder)) return true;
        if (LikeRegexPredicateValidator.Validate(input, out remainder)) return true;
        if (InPredicate.Validate(input, out remainder)) return true;
        if (IsNullPredicate.Validate(input, out remainder)) return true;
        if (QuantifiedComparisonPredicate.Validate(input, out remainder)) return true;
        return ComparisonPredicate.Validate(input, out remainder);
    }
}
