using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Common.Interface;
using Common.Model;
using System.Text;
using WebService.Helpers;
using WebService.InterfaceWebService;

namespace WebService
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance.
    /// </summary>
    internal sealed class WebService : StatelessService
    {
        public WebService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        var builder = WebApplication.CreateBuilder();
                        //token
                         var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
                        var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
                        builder.Services.AddTransient<IEmail, Email>();
                        builder.Services.AddTransient<ValidationServices>();
                        builder.Services.AddScoped<IUserService, UserService>();
                        builder.Services.AddScoped<IAdminService, AdminService>();
                        builder.Services.AddScoped<IDriverService, DriverService>();
                        builder.Services.AddScoped<IRiderService, RiderService>();
                        //builder.Services.AddScoped<IEmailSender>();


                        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                         .AddJwtBearer(options =>
                         {
                             options.TokenValidationParameters = new TokenValidationParameters
                             {
                                 ValidateIssuer = true,
                                 ValidateAudience = true,
                                 ValidateLifetime = true,
                                 ValidateIssuerSigningKey = true,
                                 ValidIssuer = jwtIssuer,
                                 ValidAudience = jwtIssuer,
                                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                             };
                         });

                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);
                        builder.WebHost
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url);
                        builder.Services.AddControllers();
                        builder.Services.AddEndpointsApiExplorer();
                        builder.Services.AddSwaggerGen();
                        builder.Services.AddSignalR();
                        builder.Services.AddAuthorization(options =>
                        {
                               options.AddPolicy("Admin", policy => policy.RequireClaim("MyCustomClaim", "Admin"));
                               options.AddPolicy("Rider", policy => policy.RequireClaim("MyCustomClaim", "Rider"));
                               options.AddPolicy("Driver", policy => policy.RequireClaim("MyCustomClaim", "Driver"));
                        });

                          builder.Services.AddCors(options =>
                        {
                            options.AddPolicy(name: "cors", builder => {
                                builder.WithOrigins("http://localhost:3000")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();

                                });
                            });
                        var app = builder.Build();
                        if (app.Environment.IsDevelopment())
                        {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                        }
                         app.UseCors("cors");
                        app.UseRouting();
                        app.UseHttpsRedirection();

                        app.UseAuthentication();
                        app.UseAuthorization();

                        app.MapControllers();
                        app.UseStaticFiles();
                        app.UseFileServer();
                        app.UseDefaultFiles();
                        return app;

                    }))
            };
        }
    }
}
