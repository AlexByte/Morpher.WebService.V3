﻿namespace Morpher.WebApi.Models.Exceptions
{
    using System;

    public class RussianWordsNotFoundException : MorpherException
    {
        private static readonly string ErrorMessage = "Не найдено русских слов.";

        public RussianWordsNotFoundException()
            : base(ErrorMessage, 5)
        {
            this.Code = 5;
        }

        public RussianWordsNotFoundException(string message, int code)
            : base(message, code)
        {
            this.Code = code;
        }

        public RussianWordsNotFoundException(string message, int code, Exception innerException)
            : base(message, code, innerException)
        {
            this.Code = code;
        }
    }
}