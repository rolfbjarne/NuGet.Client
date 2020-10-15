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

            if (string.IsNullOrEmpty(ext) || !string.IsNullOrEmpty(ext) &&
                    !ext.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) &&
                    !ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase) &&
                    !ext.Equals(".png", StringComparison.OrdinalIgnoreCase))
            {
                yield return PackagingLogMessage.CreateWarning(
                    string.Format(CultureInfo.CurrentCulture, MessageFormat, icon),
                    NuGetLogCode.NU5501);
            }

        }
    }
}
