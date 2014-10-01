﻿/*
 * Copyright 2008 (c) Sizing Servers Lab
 * University College of West-Flanders, Department GKG
 * 
 * Author(s):
 *    Dieter Vandroemme
 */

using System;
using System.Text;
using System.Threading;

namespace vApus.Util {
    /// <summary></summary>
    public static class StringUtil {
        /// <summary>
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNumeric(object o) {
            if (o == null)
                throw new ArgumentNullException("o");

            return o is short
                   || o is int
                   || o is long
                   || o is ushort
                   || o is uint
                   || o is ulong
                   || o is float
                   || o is double
                   || o is decimal;
        }

        /// <summary>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNumericType(Type t) {
            if (t == null)
                throw new ArgumentNullException("t");

            return t == typeof(short)
                   || t == typeof(int)
                   || t == typeof(long)
                   || t == typeof(ushort)
                   || t == typeof(uint)
                   || t == typeof(ulong)
                   || t == typeof(float)
                   || t == typeof(double)
                   || t == typeof(decimal);
        }

        /// <summary>
        ///     Generates a random name.
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string GenerateRandomName(int len) {
            var sb = new StringBuilder();
            char c;
            for (int i = 0; i < len; i++) {
                var random = new Random(Guid.NewGuid().GetHashCode());
                c = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Generates a random name with a random length.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max">exclusive</param>
        /// <returns></returns>
        public static string GenerateRandomName(int min, int max) {
            return GenerateRandomName((new Random(Guid.NewGuid().GetHashCode())).Next(min, max));
        }

        /// <summary>
        ///     generates a random word via the given pattern.
        ///     0 = 1 numeric char (obligatory)
        ///     9 = 1 numeric char (optional)
        ///     A = 1 capital (obligatory)
        ///     a = 1 non-capital (obligatory)
        ///     B = 1 capital (optional)
        ///     b = 1 non)capital (optional)
        ///     # = 1 random char (obligatory)
        ///     ? = 1 random char (optional)
        ///     all other chars are fixed.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string GenerateRandomPattern(string pattern) {
            char[] chars = pattern.ToCharArray();
            var returnPattern = new StringBuilder();

            for (int j = 0; j < chars.Length; j++) {
                char c = chars[j];
                var random = new Random(Guid.NewGuid().GetHashCode());
                int i = 0;

                switch (c) {
                    case '0':
                        returnPattern.Append(Convert.ToChar(random.Next(48, 58)));
                        break;
                    case '9':
                        i = random.Next(48, 59);
                        if (i != 58) returnPattern.Append(Convert.ToChar(i));
                        break;
                    case 'A':
                        returnPattern.Append(Convert.ToChar(random.Next(65, 91)));
                        break;
                    case 'a':
                        returnPattern.Append(Convert.ToChar(random.Next(97, 123)));
                        break;
                    case 'B':
                        i = random.Next(65, 92);
                        if (i != 92) returnPattern.Append(Convert.ToChar(i));
                        break;
                    case 'b':
                        i = random.Next(97, 124);
                        if (i != 123) returnPattern.Append(Convert.ToChar(i));
                        break;
                    case '#':
                        returnPattern.Append(Convert.ToChar(random.Next(32, 127)));
                        break;
                    case '?':
                        i = random.Next(32, 128);
                        if (i != 127) returnPattern.Append(Convert.ToChar(i));
                        break;
                    default:
                        returnPattern.Append(c);
                        break;
                }
            }
            return returnPattern.ToString();
        }

        /// <summary>
        ///     No Scientific notation ToString().
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static string FloatToLongString(float f, bool thousandSeparator = false) {
            if (Single.IsNaN(f) || Single.IsPositiveInfinity(f) || Single.IsNegativeInfinity(f))
                return f.ToString();

            return NumberToLongString(f, thousandSeparator && f > 999);
        }

        /// <summary>
        ///     No Scientific notation ToString().
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static string DoubleToLongString(double d, bool thousandSeparator = false) {
            if (Double.IsNaN(d) || Double.IsPositiveInfinity(d) || Double.IsNegativeInfinity(d))
                return d.ToString();

            return NumberToLongString(d, thousandSeparator && d > 999);
        }

        /// <summary>
        ///     No Scientific notation ToString().
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string NumberToLongString(object o, bool thousandSeparator) {
            string s = o.ToString().ToUpper();

            //if string representation was collapsed from scientific notation, just return it
            if (!s.Contains("E")) return s;

            char separator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

            string[] exponentParts = s.Split('E');
            string[] decimalParts = exponentParts[0].Split(separator);

            //fix missing decimal point
            if (decimalParts.Length == 1) decimalParts = new[] { exponentParts[0], "0" };
            string newNumber = decimalParts[0] + decimalParts[1];

            int exponentValue = int.Parse(exponentParts[1]);
            //positive exponent
            if (exponentValue > 0)
                s = newNumber + GetZeros(exponentValue - decimalParts[1].Length);
            else
                //negative exponent
                s = ("0" + separator + GetZeros(exponentValue + decimalParts[0].Length) + newNumber).TrimEnd('0');

            if (thousandSeparator)
                s = SeparateThousands(s);

            return s;
        }

        private static string GetZeros(int zeroCount) {
            if (zeroCount < 0)
                zeroCount = Math.Abs(zeroCount);

            return new string('0', zeroCount);
        }

        private static string SeparateThousands(string s) {
            string separator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string[] split = s.Split(separator[0]);

            var sb = new StringBuilder(split[0].Length + 1);

            int j = 0;
            for (int i = split[0].Length - 1; i != -1; i--) {
                sb.Append(split[0][i]);
                if (++j == 3) {
                    j = 0;
                    sb.Append(" ");
                }
            }

            s = sb.ToString().TrimEnd().Reverse();
            if (split.Length == 2 && !string.IsNullOrEmpty(split[1]))
                s += separator + split[1];

            return s;
        }
    }
}