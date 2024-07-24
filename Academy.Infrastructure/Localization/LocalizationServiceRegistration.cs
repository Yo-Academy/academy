using Academy.Infrastructure.BackgroundJobs;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Westwind.Globalization;
using Westwind.Globalization.AspnetCore;

namespace Academy.Infrastructure.Localization
{
    internal static class LocalizationServiceRegistration
    {
        internal static IServiceCollection AddLocalization(this IServiceCollection services, IConfiguration config)
        {
            var localizationSettings = config.GetSection(nameof(LocalizationSettings)).Get<LocalizationSettings>();

            if (localizationSettings?.EnableLocalization is true
                && localizationSettings.ResourcesPath is not null)
            {
                services.AddLocalization(options =>
                {
                    options.ResourcesPath = "Localization"; // localizationSettings.ResourcesPath;
                });

                services.AddSingleton(typeof(IStringLocalizerFactory), typeof(DbResStringLocalizerFactory));
                services.AddSingleton(typeof(IHtmlLocalizerFactory), typeof(DbResHtmlLocalizerFactory));

                services.AddWestwindGlobalization(opt =>
                {
                    var dbResourceConfiguration = config.GetSection("LocalizationSettings:DbResourceConfiguration").Get<DbResourceConfiguration>();

                    if (dbResourceConfiguration != null)
                    {
                        // Resource Mode - from Database (or Resx for serving from Resources)
                        opt.ResourceAccessMode = ResourceAccessMode.DbResourceManager;  // .Resx

                        // Make sure the database you connect to exists
                        opt.ConnectionString = dbResourceConfiguration.ConnectionString;

                        // Database provider used - Sql Server is the default
                        opt.DataProvider = DbResourceProviderTypes.PostgreSql;

                        // The table in which resources are stored
                        opt.ResourceTableName = "localizations";

                        opt.AddMissingResources = dbResourceConfiguration.AddMissingResources; // false;
                        opt.ResxBaseFolder = dbResourceConfiguration.ResxBaseFolder; // "~/Properties/";
                        opt.LocalizationFormWebPath = dbResourceConfiguration.LocalizationFormWebPath;
                        opt.StronglyTypedGlobalResource = dbResourceConfiguration.StronglyTypedGlobalResource;
                        opt.ResourceBaseNamespace = dbResourceConfiguration.ResourceBaseNamespace;
                        opt.ResxExportProjectType = GlobalizationResxExportProjectTypes.Project;

                        // Set up security for Localization Administration form
                        opt.ConfigureAuthorizeLocalizationAdministration(actionContext =>
                        {
                            // return true or false whether this request is authorized
                            return true;   //actionContext.HttpContext.User.Identity.IsAuthenticated;
                        });
                    }

                });
            }

            return services;
        }

        internal static IApplicationBuilder UseLocalization(this IApplicationBuilder app, IConfiguration config)
        {
            var localizationSettings = config.GetSection(nameof(LocalizationSettings)).Get<LocalizationSettings>();
            if (localizationSettings?.EnableLocalization is true)
            {
                var supportedCultures = localizationSettings.SupportedCultures?.Select(x => new CultureInfo(x)).ToList();

                app.UseRequestLocalization(new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture(localizationSettings.DefaultRequestCulture),
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                });
            }
            return app;
        }
    }
}