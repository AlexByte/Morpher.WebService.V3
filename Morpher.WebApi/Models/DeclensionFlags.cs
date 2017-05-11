﻿namespace Morpher.WebApi.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Flags]
    [SuppressMessage("ReSharper", "StyleCop.SA1602")]
    public enum DeclensionFlags : byte
    {
        FullName = 1 << 0,
        Common = 1 << 1,
        Feminine = 1 << 2,
        Masculine = 1 << 3,
        Animate = 1 << 4,
        Inanimate = 1 << 5
    }
}