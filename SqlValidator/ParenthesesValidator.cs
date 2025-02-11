using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlValidator;
public static class ParenthesesValidator
{
    public static bool StartsAndEndsWithParentheses(ReadOnlySpan<char> command) {
        if(command.StartsWith("(") && command.EndsWith(")"))
        {
            return true;
        }
        return false;
    }
}
