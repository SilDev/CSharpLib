#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: IniHelper.cs
// Version:  2020-02-03 20:22
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Ini
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Investment;

    internal static class IniHelper
    {
        internal static IDictionary<string, IDictionary<string, IList<string>>> SortHelper(IDictionary<string, IDictionary<string, IList<string>>> source, IEnumerable<string> topSections)
        {
            var comparer = CacheInvestor.GetDefault<AlphaNumericComparer<string>>();
            if (topSections != null)
                return source.OrderBy(p => !string.IsNullOrEmpty(p.Key)) // non-section is string.Empty and should be always on top
                             .ThenBy(p => !IsImportantSection(p.Key)) // important sections should also always be on top
                             .ThenBy(p => !topSections.Contains(p.Key)) // sequence of sections to keep on top (optional)
                             .ThenBy(p => p.Key, comparer) // sort sections alphabetical and numerical
                             .ToDictionary(p => p.Key, p => p.Value); // finally, create the sorted dictionary
            return source.OrderBy(p => !string.IsNullOrEmpty(p.Key))
                         .ThenBy(p => !IsImportantSection(p.Key))
                         .ThenBy(p => p.Key, comparer)
                         .ToDictionary(p => p.Key, p => p.Value);
        }

        internal static bool HasHexPrefix(string str)
        {
            /*
                hex:    REG_BINARY
                hex(0): REG_NONE
                hex(1): REG_SZ
                hex(2): REG_EXPAND_SZ
                hex(3): REG_BINARY
                hex(4): REG_DWORD_LITTLE_ENDIAN
                hex(5): REG_DWORD_BIG_ENDIAN
                hex(6): REG_LINK
                hex(7): REG_MULTI_SZ
                hex(8): REG_RESOURCE_LIST
                hex(9): REG_FULL_RESOURCE_DESCRIPTOR
                hex(a): REG_RESOURCE_REQUIREMENTS_LIST
                hex(b): REG_QWORD_LITTLE_ENDIAN
            */
            if (string.IsNullOrEmpty(str) || str.Length < 7 || !str.StartsWith("hex", StringComparison.Ordinal))
                return false;
            if (str[3] == ':')
                return true;
            if (str[3] == '(' && str[5] == ')' && str[6] == ':')
                return "0123456789ab".ContainsItem(str[4]);
            return false;
        }

        internal static bool IsHex(string str)
        {
            const string hexa = "0123456789abcdef";
            if (HasHexPrefix(str))
                str = str.Substring(str.IndexOf(':') + 1);
            if (str.EndsWith("\\", StringComparison.Ordinal))
                str = str.Substring(0, str.Length - 1);
            if (str.EndsWith(",", StringComparison.Ordinal))
                str = str.Substring(0, str.Length - 1);
            if (!str.Contains(","))
                return str.Length == 2 && hexa.ContainsItem(str[0]) && hexa.ContainsItem(str[1]);
            return str.Split(',').All(s => s.Length == 2 && hexa.ContainsItem(s[0]) && hexa.ContainsItem(s[1]));
        }

        internal static bool SectionIsInvalid(string str) =>
            !(string.IsNullOrEmpty(str) || HasValidStart(str) && str.All(IsValidSectionChar));

        internal static bool KeyIsInvalid(string str) =>
            string.IsNullOrEmpty(str) || !HasValidStart(str) || !str.All(IsValidKeyChar);

        internal static bool IsImportantSection(string str) =>
            !string.IsNullOrEmpty(str) && str.StartsWith("-HKEY_", StringComparison.Ordinal) && str.Contains("\\");

        private static bool HasValidStart(string str) =>
            !string.IsNullOrEmpty(str) && !str.StartsWith("#", StringComparison.Ordinal) && !str.StartsWith(";", StringComparison.Ordinal);

        private static bool IsValidSectionChar(char ch)
        {
            if (ch == '[' || ch == ']')
                return false;
            return !char.IsControl(ch);
        }

        private static bool IsValidKeyChar(char ch)
        {
            if (ch == '=')
                return false;
            return !char.IsControl(ch);
        }
    }
}
