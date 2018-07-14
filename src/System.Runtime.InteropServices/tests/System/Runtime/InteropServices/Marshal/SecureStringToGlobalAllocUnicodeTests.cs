// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security;
using Xunit;

namespace System.Runtime.InteropServices.Tests
{
    public class SecureStringToGlobalAllocUnicodeTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("pizza")]
        [InlineData("pepperoni")]
        [InlineData("password")]
        [InlineData("P4ssw0rdAa1")]
        [InlineData("\u1234")]
        [InlineData("\uD800")]
        [InlineData("\uD800\uDC00")]
        [InlineData("\0")]
        [InlineData("abc\0def")]
        public void SecureStringToGlobalAllocUnicode_InvokePtrToStringUni_Roundtrips(string data)
        {
            int nullIndex = data.IndexOf('\0');
            string expected = nullIndex == -1 ? data : data.Substring(0, nullIndex);

            using (SecureString secureString = ToSecureString(data))
            {
                IntPtr ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                try
                {
                    Assert.Equal(expected, Marshal.PtrToStringUni(ptr));
                    Assert.Equal(data, Marshal.PtrToStringUni(ptr, data.Length));
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr);
                }
            }
        }

        [Fact]
        public void SecureStringToGlobalAllocUnicode_NullString_ThrowsArgumentNullException()
        {
            AssertExtensions.Throws<ArgumentNullException>("s", () => Marshal.SecureStringToGlobalAllocUnicode(null));
        }

        [Fact]
        public void SecureStringToGlobalAllocUnicode_DisposedString_ThrowsObjectDisposedException()
        {
            var secureString = new SecureString();
            secureString.Dispose();

            Assert.Throws<ObjectDisposedException>(() => Marshal.SecureStringToGlobalAllocUnicode(secureString));
        }

        private static SecureString ToSecureString(string data)
        {
            var str = new SecureString();
            foreach (char c in data)
            {
                str.AppendChar(c);
            }
            str.MakeReadOnly();
            return str;
        }
    }
}