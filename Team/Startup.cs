using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Team.AuthHelper.OverWrite;
using Team.Infrastructure.DbContext;
using Team.Infrastructure.IRepositories;
using Team.Infrastructure.Repositories;
using Team.Model.AutoMappers;
using Team.Model.AutoMappers.TeamMapper;
using Team.Model.AutoMappers.UserMapper;
using Team.Validator;
using Team.Validator.RunValidator;
using Team.Validator.TeamValidator;
using Team.Validator.UserValidator;

namespace Team
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Serilog

            /*这里配置的意思是：全局最低记录日志级别是Debug，但是针对以Microsoft开头的命名空间的最低级别是Information。

                使用Enruch.FromLogContext()可以让程序在执行上下文时动态添加或移除属性（这个需要看文档）。

                按日生成记录文件，日志文件名后会带着日期，并放到./logs目录下*/

            Log.Logger=new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft",LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs",@"log.txt"),rollingInterval:RollingInterval.Day)
                .CreateLogger();

            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "约",
                    Version = "v1",
                    Description = "海贼王",
                    Contact = new Contact
                    {
                        Name = "醉人",
                        Email = "1045683477@qq.com",
                        Url= "https://www.sanfengyun.com"
                    }
                });

                #region API 文档说明

                var basePath = Path.GetDirectoryName(AppContext.BaseDirectory);
                var apiPath = Path.Combine(basePath, "Team.xml");
                c.IncludeXmlComments(apiPath,true);
                var modelPath = Path.Combine(basePath, "Team.Model.xml");
                c.IncludeXmlComments(modelPath,true);

                #endregion

                #region Token 绑定的 ConfigureService

                var security = new Dictionary<string, IEnumerable<string>> {{"Team", new string[] { }},};
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("Team",new ApiKeyScheme
                {
                    Description = "JWT授权 (数据在请求中进行传输) 直接在下框中输入Bearer {token} (注意两者之间是一个空格)\"",
                    Name = "Authorization",//jwt默认授权的参数名称
                    In = "header",//jwt默认存放在authorization的信息的位置(请求头)
                    Type = "apiKey"
                });

                #endregion

            });

            #endregion

            #region SQL链接

            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            #endregion

            #region Policy

            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("SuperAdministrator", policy => policy.RequireRole("SuperAdministrator").Build());
            });

            #endregion

            #region 认证

            //上边用到的 tokenValidationParameters
            //读取配置文件
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var KeyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(KeyByteArray);

            //令牌验证参数
            var tokenValidationParameters=new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,//还是从 appsettings.json 拿到的
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//发行人
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//订阅人
                ValidateLifetime = true,
                //其实和之前的方法是一样的，只不过请注意 ClockSkew 属性，默认是5分钟缓冲。
                //总的Token有效时间 = JwtRegisteredClaimNames.Exp + ClockSkew ；
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = tokenValidationParameters;

                });


            #endregion

            #region FluentValidation

            services.AddTransient<IValidator<UserUpdateMap>, UserUpdateValidator>();
            services.AddTransient<IValidator<UserRegisteredMap>, UserRegisteredValidator>();
            services.AddTransient<IValidator<UserLoginMap>, UserLoginValidator>();
            services.AddTransient<IValidator<TeamCreateMap>, TeamCreateValidator>();
            services.AddTransient<IValidator<RunRecordMap>, RunRecordValidator>();

            #endregion

            #region 接口生命周期

            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamBall, TeamBall>();
            services.AddScoped<IRunRepository, RunRepository>();
            services.AddScoped<IImagesResource, ImagesResource>();
            services.AddScoped<IRunTeamResource, RunTeamResource>();

            #endregion

            

            services.AddAutoMapper();
            services.AddTimedJob();

            #region 将Json.NET配置为忽略它在对象图中找到的循环

            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            #endregion
            


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddFluentValidation().AddJsonOptions(
                options=>options.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1版本"));

            #endregion

            app.UseTimedJob();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
