#region File info

// *********************************************************************************************************
// Funcular.IdGenerators>Funcular.IdGenerators>Base36Converter.cs
// Created: 2015-06-26 2:42 PM
//  Ported: 2017-05-31 11:20 AM
// Updated: 2017-05-31 11:25 AM
// By: Paul Smith 
// 
// *********************************************************************************************************
// LICENSE: The MIT License (MIT)
// *********************************************************************************************************
// Copyright (c) 2010-2015 <copyright holders>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// *********************************************************************************************************

#endregion



#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion


 
/*  TODO: Create instance converter the constructor of which accepts
    a char list, and implicitly sets the base to its length. */

namespace Funcular.DotNetCore.IdGenerators.BaseConversion
{
    internal static class Base36Converter
    {
        private const int BITS_IN_LONG = 64;
        private const int BASE = 36;
        private static readonly char[] _digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] _buffer = new char[BITS_IN_LONG];
        private static readonly object _lock = new object();

        static Base36Converter()
        {
            BaseConverter.CharList = CharList;
        }

        /// <summary>
        ///     The character set for encoding. Defaults to upper-case alphanumerics 0-9, A-Z.
        /// </summary>
        public static string CharList { get; set; } = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string FromHex(string hex)
        {
            return BaseConverter.Convert(number: hex.ToUpper().Replace(oldValue: "-",newValue: ""), fromBase: 16, toBase: 36);
        }

        public static string FromGuid(Guid guid)
        {
            return BaseConverter.Convert(number: guid.ToString(format: "N"), fromBase: 16, toBase: 36);
        }

        public static string FromInt32(int int32)
        {
            return BaseConverter.Convert(number: int32.ToString(provider: CultureInfo.InvariantCulture), fromBase: 10, toBase: 36);
        }

        public static string FromInt64(long int64)
        {
            return BaseConverter.Convert(number: int64.ToString(provider: CultureInfo.InvariantCulture), fromBase: 10, toBase: 36);
        }

        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <param name="decimalNumber">The number to convert.</param>
        /// <returns></returns>
        public static string FromLong(long decimalNumber)
        {
            unchecked
            {
                int index = BITS_IN_LONG - 1;

                if (decimalNumber == 0)
                    return "0";

                long currentNumber = Math.Abs(value: decimalNumber);

                lock (_lock)
                {
                    while (currentNumber != 0)
                    {
                        int remainder = (int)(currentNumber % BASE);
                        _buffer[index--] = _digits[remainder];
                        currentNumber = currentNumber / BASE;
                    }
                    return new string(value: _buffer, startIndex: index + 1, length: BITS_IN_LONG - index - 1);
                }
            }
        }

        /// <summary>
        ///     Encode the given number into a Base36 string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Encode(long input)
        {
            unchecked
            {
                var result = new Stack<char>();
                while (input != 0)
                {
                    result.Push(item: _digits[input % 36]);
                    input /= 36;
                }
                return new string(value: result.ToArray());
            }
        }

        /// <summary>
        ///     Decode the Base36 Encoded string into a number
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int64 Decode(string input)
        {
            unchecked
            {
                IEnumerable<char> reversed = input.ToUpper().Reverse();
                long result = 0;
                int pos = 0;
                foreach (var c in reversed)
                {
                    result += CharList.IndexOf(value: c) * (long)Math.Pow(x: 36, y: pos);
                    pos++;
                }
                return result;
            }
        }
    }
}