﻿namespace Morpher.WebApi.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class LogEntity
    {
        public LogEntity(
            string remoteAddress,
            string queryString,
            string querySource,
            DateTime dateTime,
            Guid? webServiceToken,
            string userAgent,
            int errorCode)
        {
            this.RemoteAddress = remoteAddress;
            this.QueryString = queryString;
            this.QuerySource = querySource;
            this.DateTimeUTC = dateTime;
            this.WebServiceToken = webServiceToken;
            this.UserAgent = userAgent;
            this.ErrorCode = errorCode;
        }

        public string RemoteAddress { get; set; }

        public string QueryString { get; set; }

        public string QuerySource { get; set; }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public DateTime DateTimeUTC { get; set; }

        public Guid? WebServiceToken { get; set; }

        public string UserAgent { get; set; }

        public int ErrorCode { get; set; }
    }
}