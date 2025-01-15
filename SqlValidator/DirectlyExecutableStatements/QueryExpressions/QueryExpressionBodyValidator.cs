namespace SqlValidator.DirectlyExecutableStatements.QueryExpressions;
public static class QueryExpressionBodyValidator
{
    private static readonly string[] UnionOrExcept = ["UNION", "EXCEPT"];
    private static readonly string[] AllOrDistinct = ["ALL", "DISTINCT"];

    public static bool Validate(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        if (!QueryTermValidator.Validate(input, out remainder)) return false;
        if (!CheckOptionalUnionOrExcept(remainder, out remainder)) return false;

        //Optionals
        if (OrderByClauseValidator.Validate(remainder, out ReadOnlySpan<char> tempRemainder))
        {
            remainder = tempRemainder;
        }

        if (LimitClauseValidator.Validate(remainder, out tempRemainder))
        {
            remainder = tempRemainder;
        }

        if (OptionClauseValidator.Validate(remainder, out tempRemainder))
        {
            remainder = tempRemainder;
        }

        return true;
    }

    private static bool CheckOptionalUnionOrExcept(ReadOnlySpan<char> input, out ReadOnlySpan<char> remainder)
    {
        remainder = input;

        while (remainder.HasAnyNextToken(UnionOrExcept, out remainder))
        {
            // Remove Optional All or Distinct
            remainder.HasAnyNextToken(AllOrDistinct, out remainder);

            if (!QueryTermValidator.Validate(remainder, out remainder)) return false;
        }


        return true;
    }
}
