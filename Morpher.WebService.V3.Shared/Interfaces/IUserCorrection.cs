﻿namespace Morpher.WebService.V3.Shared.Interfaces
{
    using System;

    using Morpher.WebService.V3.Shared.Models;

    public interface IUserCorrection
    {
        void SetUserDeclensions(RussianDeclensionForms paradigm, string lemma, bool plural, Guid? token);

        void SetUserDeclensions(UkrainianDeclensionForms paradigm, string lemma, bool plural, Guid? token);

        void NewCorrection(UserCorrectionEntity entity, Guid? token);

        bool RemoveCorrection(string lemma, string language, Guid? token);
    }
}
