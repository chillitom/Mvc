// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNet.Http;
#if NET45
using Moq;
#endif
using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding.Test
{
    public class QueryStringValueProviderFactoryTest
    {
        private readonly QueryStringValueProviderFactory _factory = new QueryStringValueProviderFactory();

#if NET45
        [Fact]
        public void GetValueProvider_ReturnsQueryStringValueProviderInstaceWithInvariantCulture()
        {
            // Arrange
            var request = new Mock<HttpRequest>();
            request.SetupGet(f => f.Query).Returns(Mock.Of<IReadableStringCollection>());
            var context = new Mock<HttpContext>();
            context.SetupGet(c => c.Items).Returns(new Dictionary<object, object>());
            context.SetupGet(c => c.Request).Returns(request.Object);
            var requestContext = new RequestContext(context.Object, new Dictionary<string, object>());

            // Act
            var result = _factory.GetValueProvider(requestContext);

            // Assert
            var valueProvider = Assert.IsType<ReadableStringCollectionValueProvider>(result);
            Assert.Equal(CultureInfo.InvariantCulture, valueProvider.Culture);
        }
#endif
    }
}
