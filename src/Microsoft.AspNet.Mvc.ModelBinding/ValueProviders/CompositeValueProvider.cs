// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "It is more fundamentally a value provider than a collection")]
    public class CompositeValueProvider : Collection<IValueProvider>, IEnumerableValueProvider
    {
        public CompositeValueProvider() 
            : base()
        {
        }

        public CompositeValueProvider(IEnumerable<IValueProvider> valueProviders)
            : base(valueProviders.ToList())
        {
        }

        public virtual async Task<bool> ContainsPrefixAsync(string prefix)
        {
            for (var i = 0; i < Count; i++)
            {
                if (await this[i].ContainsPrefixAsync(prefix))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual async Task<ValueProviderResult> GetValueAsync(string key)
        {
            // Performance-sensitive
            // Caching the count is faster for IList<T>
            var itemCount = Items.Count;
            for (var i = 0; i < itemCount; i++)
            {
                var valueProvider = Items[i];
                var result = await valueProvider.GetValueAsync(key);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public virtual async Task<IDictionary<string, string>> GetKeysFromPrefixAsync(string prefix)
        {
            foreach (var valueProvider in this)
            {
                var result = await GetKeysFromPrefixFromProvider(valueProvider, prefix);
                if (result != null && result.Count > 0)
                {
                    return result;
                }
            }
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        private static Task<IDictionary<string, string>> GetKeysFromPrefixFromProvider(IValueProvider provider, 
                                                                                       string prefix)
        {
            var enumeratedProvider = provider as IEnumerableValueProvider;
            return (enumeratedProvider != null) ? enumeratedProvider.GetKeysFromPrefixAsync(prefix) : null;
        }

        protected override void InsertItem(int index, [NotNull] IValueProvider item)
        {
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, [NotNull] IValueProvider item)
        {
            base.SetItem(index, item);
        }
    }
}
