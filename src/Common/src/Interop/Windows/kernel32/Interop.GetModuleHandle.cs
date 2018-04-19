// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Text;

internal partial class Interop
{
    internal partial class Kernel32
    {
#pragma warning disable BCL0015 // Invalid Pinvoke call
        [DllImport(Libraries.Kernel32, EntryPoint = "GetModuleHandle", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string moduleName);
#pragma warning restore BCL0015 // Invalid Pinvoke call
    }
}
