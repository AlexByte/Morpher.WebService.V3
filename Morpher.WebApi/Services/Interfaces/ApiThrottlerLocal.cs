﻿namespace Morpher.WebApi.Services.Interfaces
{
    using System;
    using System.Net.Http;

    using Morpher.WebApi.ApiThrottler;
    using Morpher.WebApi.Models;

    public class ApiThrottlerLocal : IApiThrottler
    {
        public ApiThrottlingResult Throttle(string ip)
        {
            return ApiThrottlingResult.Success;
        }

        public ApiThrottlingResult Throttle(Guid guid, out bool paidUser)
        {
            paidUser = true;
            return ApiThrottlingResult.Success;
        }

        public ApiThrottlingResult Throttle(HttpRequestMessage httpRequest, out bool paidUser)
        {
            paidUser = true;
            return ApiThrottlingResult.Success;
        }

        public object RemoveFromCache(string key)
        {
            return new object();
        }

        public MorpherCacheObject GetQueryLimit(string ip)
        {
            return new MorpherCacheObject()
                       {
                           PaidUser = true,
                           Unlimited = true,
                           QueriesLeft = 1000
                       };
        }

        public MorpherCacheObject GetQueryLimit(Guid guid)
        {
            return new MorpherCacheObject()
                       {
                           PaidUser = true,
                           Unlimited = true,
                           QueriesLeft = 1000
                       };
        }
    }
}