﻿namespace Morpher.WebApi.Models.Exceptions
{
    using System;

    public class InvalidTokenFormat : MorpherException
    {
        private static readonly string ErrorMessage = "Неверный формат токена.";

        public InvalidTokenFormat()
            : base(ErrorMessage, 10)
        {
            this.Code = 10;
        }

        public InvalidTokenFormat(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public InvalidTokenFormat(string message, int code, Exception inner)
            : base(message, code, inner)
        {
            this.Code = code;
        }
    }
}