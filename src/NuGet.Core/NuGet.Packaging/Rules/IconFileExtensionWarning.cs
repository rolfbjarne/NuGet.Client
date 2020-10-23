// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    public class IconFileExtensionWarning : IPackageRule
    {
        public string MessageFormat { get; }

        public IconFileExtensionWarning(string messageFormat)
        {
            MessageFormat = messageFormat ?? throw new ArgumentNullException(nameof(messageFormat));
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var nuspecReader = builder.NuspecReader;
            var icon = nuspecReader.GetIcon();
            var ext = Path.GetExtension(icon);

            if (!string.IsNullOrEmpty(ext) && IsKnownExtension(ext) && !IsSupportedExtension(ext))
            {
                yield return PackagingLogMessage.CreateWarning(
                    string.Format(CultureInfo.CurrentCulture, MessageFormat, icon),
                    NuGetLogCode.NU5502);
            }
        }

        public static bool IsKnownExtension(string ext)
        {
            return IsSupportedExtension(ext)
                || ext.Equals(".bmp", StringComparison.OrdinalIgnoreCase) // BmpBitmapDecoder
                || ext.Equals(".tiff", StringComparison.OrdinalIgnoreCase) // TiffBitmapDecoder
                || ext.Equals(".gif", StringComparison.OrdinalIgnoreCase) // GifBitmapDecoder
                || ext.Equals(".ico", StringComparison.OrdinalIgnoreCase) // IconBitmapDecoder Only works on BMP-based .ico files
                || ext.Equals(".wmp", StringComparison.OrdinalIgnoreCase); // WmpBitmapDecoder
        }

        public static bool IsSupportedExtension(string ext)
        {
            return ext.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) // JpegBitmapDecoder
                || ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase) // JpegBitmapDecoder
                || ext.Equals(".png", StringComparison.OrdinalIgnoreCase); // PngBitmapDecoder
        }
    }
}
