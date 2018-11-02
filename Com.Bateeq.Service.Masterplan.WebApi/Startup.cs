using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Com.Bateeq.Service.Masterplan.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using IdentityServer4.AccessTokenValidation;
using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BookingOrderFacade;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.SectionFacade;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Services.ValidateService;
using Com.Bateeq.Service.Masterplan.WebApi.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Logics;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BlockingPlanFacade;
using Com.Bateeq.Service.Masterplan.Lib.Models;

namespace Com.Bateeq.Service.Masterplan.WebApi
{
    public class Startup
    {
        private readonly string[] EXPOSED_HEADERS = new string[] { "Content-Disposition", "api-version", "content-length", "content-md5", "content-type", "date", "request-id", "response-time" };
        private readonly string MASTERPLAN_POLICY = "MasterplanPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void RegisterServices(IServiceCollection services)
        {
            services
                .AddScoped<IIdentityService, IdentityService>()
                .AddScoped<IValidateService, ValidateService>();
        }

        private void RegisterFacades(IServiceCollection services)
        {
            services
                .AddTransient<ISectionFacade, SectionFacade>()
                .AddTransient<IBookingOrderFacade, BookingOrderFacade>()
                .AddTransient<WeeklyPlanFacade>()
                .AddTransient<WeeklyPlanItemFacade>()
                .AddTransient<IBlockingPlanFacade, BlockingPlanFacade>();
        }

        private void RegisterLogics(IServiceCollection services)
        {
            services
                .AddTransient<SectionLogic>()
                .AddTransient<BookingOrderLogic>()
                .AddTransient<BookingOrderDetail>()
                .AddTransient<BookingOrderDetailLogic>()
                .AddTransient<WeeklyPlanLogic>()
                .AddTransient<BlockingPlanLogic>()
                .AddTransient<BlockingPlanWorkScheduleLogic>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString(Constant.DEFAULT_CONNECTION) ?? Configuration[Constant.DEFAULT_CONNECTION];

            #region Register
            services.AddDbContext<MasterplanDbContext>(options => options.UseSqlServer(connectionString));

            RegisterServices(services);

            RegisterFacades(services);

            RegisterLogics(services);

            services.AddAutoMapper();
            #endregion

            #region Authentication
            string Secret = Configuration.GetValue<string>(Constant.SECRET) ?? Configuration[Constant.SECRET];
            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        IssuerSigningKey = Key
                    };
                });
            #endregion

            #region CORS
            services.AddCors(options => options.AddPolicy(MASTERPLAN_POLICY, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders(EXPOSED_HEADERS);
            }));
            #endregion

            #region API
            services
                .AddApiVersioning(options => options.DefaultApiVersion = new ApiVersion(1, 0))
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MasterplanDbContext>();
                context.Database.Migrate();
            }

            app.UseAuthentication();
            app.UseCors(MASTERPLAN_POLICY);
            app.UseMvc();
        }
    }
}
