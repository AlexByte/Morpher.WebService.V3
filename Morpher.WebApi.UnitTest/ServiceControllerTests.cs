﻿namespace Morpher.WebApi.UnitTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;

    using Moq;

    using Morpher.WebApi.Controllers;
    using Morpher.WebApi.Models;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

    using NUnit.Framework;

    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ServiceControllerTests
    {
        [Test]
        public void QueriesLeftToday_TokenNotFound()
        {
            Mock<IApiThrottler> apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<Guid>())).Returns((MorpherCacheObject)null);

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            ServiceController serviceController = new ServiceController(apiThrottlerMock.Object) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            ServiceErrorMessage errorMessage;
            responseMessage.TryGetContentValue(out errorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new TokenNotFoundException()), errorMessage);
        }

        [Test]
        public void QueriesLeftToday_TokenFormatException()
        {
            Mock<IApiThrottler> apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<Guid>())).Returns((MorpherCacheObject)null);
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest($"http://localhost:0/foo?token=invalid_token", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            ServiceController serviceController = new ServiceController(apiThrottlerMock.Object) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            ServiceErrorMessage errorMessage;
            responseMessage.TryGetContentValue(out errorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new InvalidTokenFormat()), errorMessage);
        }

        [Test]
        public void QueriesLeftToday_Token_NegativeValue()
        {
            Mock<IApiThrottler> apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<Guid>())).Returns(new MorpherCacheObject() { QueriesLeft = -1 });

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            ServiceController serviceController = new ServiceController(apiThrottlerMock.Object) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            int value;
            responseMessage.TryGetContentValue(out value);

            Assert.AreEqual(0, value);
        }

        [Test]
        public void QueriesLeftToday_Token_PositiveValue()
        {
            Mock<IApiThrottler> apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<Guid>())).Returns(new MorpherCacheObject() { QueriesLeft = 5 });

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest($"http://localhost:0/foo?token={guid}", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            ServiceController serviceController = new ServiceController(apiThrottlerMock.Object) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            int value;
            responseMessage.TryGetContentValue(out value);

            Assert.AreEqual(5, value);
        }

        [Test]
        public void QueriesLeftToday_IpBlocked()
        {
            Mock<IApiThrottler> apiThrottlerMock = new Mock<IApiThrottler>();
            apiThrottlerMock.Setup(throttler => throttler.GetQueryLimit(It.IsAny<string>())).Returns((MorpherCacheObject)null);

            Guid guid = Guid.NewGuid();
            HttpRequestMessage requestMessage =
                RequestCreater.CreateRequest($"http://localhost:0/foo", HttpMethod.Get, "::1");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            ServiceController serviceController = new ServiceController(apiThrottlerMock.Object) { Request = requestMessage };
            HttpResponseMessage responseMessage = serviceController.QueriesLeftToday();

            ServiceErrorMessage errorMessage;
            responseMessage.TryGetContentValue(out errorMessage);

            Assert.AreEqual(new ServiceErrorMessage(new IpBlockedException()), errorMessage);
        }
    }
}
