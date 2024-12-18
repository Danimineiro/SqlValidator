using SqlValidator.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator.DDLStatements.ALTERStatements;
public static class AlterOptionsValidator
{
    private const int ADD_SET_TOKEN_LENGTH = 4;
    private const int DROP_TOKEN_LENGTH = 5;
    private static int lengthCovered = 0;
    private static bool allOptionsCovered = false;
    public static bool Validate(ReadOnlySpan<char> command)
    {
        while(!allOptionsCovered) {
            if (command.StartsWith("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            lengthCovered += AlterValidator.GetNextTokenLength(command);
                if (ParenthesesValidator.StartsAndEndsWithParentheses(command[lengthCovered..]))
                {
                lengthCovered += 1;
                if (command[lengthCovered..].StartsWith("ADD") || command[lengthCovered..].StartsWith("SET"))
                {
                    lengthCovered += ADD_SET_TOKEN_LENGTH;
                        if (IdentifierValidator.Validate(command[lengthCovered..], out _))
                        {
                            lengthCovered += AlterValidator.GetNextTokenLength(command[lengthCovered..]);
                            ReadOnlySpan<char> nextToken = AlterValidator.GetNextToken(command[lengthCovered..].TrimStart());
                            bool tokenIsNumeric = float.TryParse(nextToken, out _);
                            if(!tokenIsNumeric)
                            {
                                lengthCovered += nextToken.Length;
                                if (!command[lengthCovered..].TrimStart().StartsWith(","))
                                {
                                    allOptionsCovered = true;
                                }
                            } else
                            {
                                lengthCovered += nextToken.Length;
                                if (!command[lengthCovered..].TrimStart().StartsWith(","))
                                {
                                    allOptionsCovered = true;
                                }
                            }
                        }

                    }
                    else if (command[lengthCovered..].StartsWith("DROP"))
                {
                    lengthCovered += DROP_TOKEN_LENGTH;
                        if (IdentifierValidator.Validate(command[lengthCovered..], out _))
                        {
                            lengthCovered += AlterValidator.GetNextTokenLength(command[lengthCovered..]);
                            if (!command[lengthCovered..].TrimStart().StartsWith(","))
                            {
                                allOptionsCovered = true;
                            }
                        }
                    } else
                    {
                        return false;
                    }
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        } 
        return true;
    }

    internal static bool ValidateDROPOption(ReadOnlySpan<char> command)
    {
        return false;
    }

    internal static bool ValidateADDSETOption(ReadOnlySpan<char> command)
    {
        return false;
    }
}
