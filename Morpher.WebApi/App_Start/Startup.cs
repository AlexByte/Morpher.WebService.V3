﻿using Microsoft.Owin;
using Morpher.WebService.V3;

[assembly: OwinStartup(typeof(Startup))]

namespace Morpher.WebService.V3
{
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAutofacMiddleware(AutofacInit.Container);
        }
    }
}
