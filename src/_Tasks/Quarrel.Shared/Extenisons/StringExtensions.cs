﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Security.Cryptography;
using JetBrains.Annotations;

namespace System
{
    /// <summary>
    /// A <see langword="class"/> with some extension methods for the <see langword="string"/> type
    /// </summary>
    internal static partial class StringExtensions
    {
        /// <summary>
        /// Converts the input <see langword="string"/> into its hex representation
        /// </summary>
        /// <param name="source">The input <see langword="string"/> to convert</param>
        [Pure, NotNull]
        public static string ToHex([NotNull] this string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            return CryptographicBuffer.EncodeToHexString(bytes.AsBuffer());
        }

        /// <summary>
        /// Truncates the input <see langword="string"/> and adds ellipsis if necessary
        /// </summary>
        /// <param name="source">The input <see langword="string"/> to truncate</param>
        /// <param name="length">The maximum length allowed</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure, NotNull]
        public static string Truncate([NotNull] this string source, int length)
        {
            return source.Length <= length
                ? source
                : $"{source.Substring(0, length - 3)}...";
        }
    }
}
