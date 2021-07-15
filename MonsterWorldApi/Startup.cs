using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MonsterWorldApi.Data;
using MonsterWorldApi.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace MonsterWorldApi
{
    /// <summary>
    /// Classe que inicia a aplicação.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Construtor da classe startup.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configurações
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Método que recebe os serviços da applicação.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var ApiInfoV1 = new OpenApiInfo
            {
                Title = "Blue Monster World API V1",
                Description = "API criada para o gerenciamento dos monstros mais bolados dos games",
                Contact = new OpenApiContact 
                { 
                    Name = "Artur Nascimento",
                    Email = "artur.nascimento@outlook.com",
                    Url = new System.Uri("https://www.blueedtech.com.br")      
                },
                License = new OpenApiLicense
                {
                    Name = "Licença Blue Monster World API",
                    Url = new System.Uri("https://www.blueedtech.com.br")
                },
                Version = "v1"
            };

            var ApiInfoV2 = new OpenApiInfo
            {
                Title = "Blue Monster World API V2",
                Description = "API criada para o gerenciamento dos monstros mais bolados dos games",
                Contact = new OpenApiContact
                {
                    Name = "Artur Nascimento",
                    Email = "artur.nascimento@outlook.com",
                    Url = new System.Uri("https://www.blueedtech.com.br")
                },
                License = new OpenApiLicense
                {
                    Name = "Licença Blue Monster World API",
                    Url = new System.Uri("https://www.blueedtech.com.br")
                },
                Version = "v2"
            };

            //services.AddApiVersioning();

            services.AddControllers(option =>
               option.Conventions.Add(new GroupingByNamespaceConvention())
            ).AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            //services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", ApiInfoV1);
                c.SwaggerDoc("v2", ApiInfoV2);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = $"{Path.Combine(AppContext.BaseDirectory, xmlFile)}";

                c.IncludeXmlComments(xmlPath);


              //  c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
              //  {
              //      Name = "Authorization",
              //      Type = SecuritySchemeType.Http,
               //     Scheme = "basic",
              //      In = ParameterLocation.Header,
               //     Description = "Basic Authorization Header"
              //  });

             //   c.AddSecurityRequirement(new OpenApiSecurityRequirement
             //   {
               //     {
               //         new OpenApiSecurityScheme
              //          {
              //              Reference = new OpenApiReference
              //              {
              //                  Type = ReferenceType.SecurityScheme,
              //                  Id = "basic"
              //              }
               //         },
               //         new string [] { }
                //    }
               // });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "Autenticação por Token"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            #region Database Context
            services.AddDbContext<MonsterWorldApiContext>(
               options => options.UseSqlServer(Configuration.GetConnectionString("MonsterWorldApiConnectionString"))
             );

            #endregion

            services.AddScoped<IMonsterService, SqlMonsterService>();

            //services.AddScoped<IMonsterService, StaticMonsterService>();

            services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<MonsterWorldApiContext>();
            services.AddScoped<AuthService>();
            //services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

            var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Método usado para configurar as requisições HTTP
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MonsterWorldApi v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "MonsterWorldApi v2");
                });
                
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
