/*
 * @bot-written
 *
 * WARNING AND NOTICE
 * Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
 * Full Software Licence as accepted by you before being granted access to this source code and other materials,
 * the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
 * commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
 * licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
 * including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
 * access, download, storage, and/or use of this source code.
 *
 * BOT WARNING
 * This file is bot-written.
 * Any changes out side of "protected regions" will be lost next time the bot makes any changes.
 */
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.DataProtection;

using GraphQL;
using GraphQL.DataLoader;
using GraphQL.EntityFramework;
using GraphQL.Server;
using GraphQL.Types;
using Hangfire;
using Hangfire.EntityFrameworkCore;
using Hangfire.Redis;
using StackExchange.Redis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RazorLight;

using Firstapp2257.Configuration;
using Firstapp2257.Models;
using Firstapp2257.Models.Internal;
using Firstapp2257.Services;
using Firstapp2257.Helpers;
using Firstapp2257.Utility;
using Firstapp2257.Graphql;
using Firstapp2257.Graphql.Types;
using Firstapp2257.Enums;
using Firstapp2257.Services.CertificateProvider;
using Firstapp2257.Services.Interfaces;
using Firstapp2257.Services.Files;
using Firstapp2257.Services.Files.Providers;
using Firstapp2257.Services.TwoFactor;
using Firstapp2257.Services.TwoFactor.Methods;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Firstapp2257
{
	public class Startup
	{
		public Startup(IWebHostEnvironment env, IConfiguration configuration)
		{
			Configuration = configuration;
			CurrentEnvironment = env;
		}

		private IWebHostEnvironment CurrentEnvironment { get; set; }

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// % protected region % [Configure initialization hosted service here] off begin
			services.AddHostedService<InitializationHostedService>();
			// % protected region % [Configure initialization hosted service here] end

			// % protected region % [Configure caching here] off begin
			ConfigureCaching(services);
			// % protected region % [Configure caching here] end

			// % protected region % [Configure MVC here] off begin
			AddMvc(services);
			// % protected region % [Configure MVC here] end

			// % protected region % [Configure database connection here] off begin
			ConfigureDatabaseConnection(services);
			// % protected region % [Configure database connection here] end

			// % protected region % [Configure Auth services here] off begin
			ConfigureAuthServices(services);
			// % protected region % [Configure Auth services here] end

			// % protected region % [Configure scoped services here] off begin
			ConfigureScopedServices(services);
			// % protected region % [Configure scoped services here] end

			// % protected region % [Configure graphql services here] off begin
			ConfigureGraphql(services);
			// % protected region % [Configure graphql services here] end

			// % protected region % [Configure swagger services here] off begin
			AddSwaggerService(services);
			// % protected region % [Configure swagger services here] end

			// % protected region % [Configure configuration services here] off begin
			AddApplicationConfigurations(services);
			// % protected region % [Configure configuration services here] end

			// % protected region % [Add extra startup methods here] off begin
			// % protected region % [Add extra startup methods here] end

			// % protected region % [Configure ApiBehaviorOptions service here] off begin
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = ctx => new Firstapp2257ActionResult();
			});
			// % protected region % [Configure ApiBehaviorOptions service here] end

			// % protected region % [Configure SPA files here] off begin
			// In production, the React files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "Client";
			});
			// % protected region % [Configure SPA files here] end

			// Add scheduled tasks & scheduler
			LoadScheduledTasks(services);
		}

		private void AddMvc(IServiceCollection services)
		{
			services.AddMvc(options =>
				{
					// % protected region % [Configure MVC options here] off begin
					options.Filters.Add(new AntiforgeryFilterAttribute());
					// % protected region % [Configure MVC options here] end
				})
				.AddControllersAsServices()
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
				.AddNewtonsoftJson(options =>
				{
					// % protected region % [Configure JSON options here] off begin
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					options.SerializerSettings.Converters.Add(new StringEnumConverter());
					// % protected region % [Configure JSON options here] end
				})
				.AddMvcOptions(options =>
				{
					// Add extra output formatters after JSON to ensure JSON is the default
					// % protected region % [Configure output formatters here] off begin
					options.OutputFormatters.Add(new CsvOutputFormatter());
					// % protected region % [Configure output formatters here] end
				});

			// % protected region % [Configure health check registration here] off begin
			services.AddHealthChecks();
			// % protected region % [Configure health check registration here] end

			// % protected region % [Configure rate limiting services here] off begin
			// Configure rate limiting stores and processing strategy
			services.TryAddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
			services.TryAddSingleton<IClientPolicyStore, DistributedCacheClientPolicyStore>();
			services.TryAddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
			services.TryAddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
			services.TryAddSingleton<IProcessingStrategy>(sp =>
			{
				// Use Redis strategy if there is a Redis connection, otherwise use the distributed cache
				if (sp.GetRequiredService<IRedisConnectionService>().Enabled)
				{
					return ActivatorUtilities.CreateInstance<CustomRedisProcessingStrategy>(sp);
				}

				return ActivatorUtilities.CreateInstance<AsyncKeyLockProcessingStrategy>(sp);
			});
			// % protected region % [Configure rate limiting services here] end
		}

		// % protected region % [Customise ConfigureDatabaseConnection method here] off begin
		/// <summary>
		/// Set up the database connection
		/// </summary>
		/// <param name="services"></param>
		private void ConfigureDatabaseConnection(IServiceCollection services)
		{
			// Configure the connection to the application database
			var dbConnectionString = Configuration.GetConnectionString("DbConnectionString");
			services.AddDbContextFactory<Firstapp2257DBContext>(options =>
			{
				options.UseNpgsql(dbConnectionString);
				options.UseOpenIddict<Guid>();
				options.ReplaceService<IQueryCompiler, CrudQueryCompiler>();
			});
			services.AddDbContext<Firstapp2257DBContext>(options =>
			{
				options.UseNpgsql(dbConnectionString);
				options.UseOpenIddict<Guid>();
				options.ReplaceService<IQueryCompiler, CrudQueryCompiler>();
			});

			// Configure Redis connection
			services.TryAddSingleton<IRedisConnectionService, RedisConnectionService>();
			services.TryAddSingleton<IConnectionMultiplexer>(sp => sp
				.GetRequiredService<IRedisConnectionService>()
				.Connection);
		}
		// % protected region % [Customise ConfigureDatabaseConnection method here] end

		// % protected region % [Customise ConfigureCaching method here] off begin
		/// <summary>
		/// Configures caching services for the application
		/// </summary>
		/// <param name="services"></param>
		private void ConfigureCaching(IServiceCollection services)
		{
			services.TryAddSingleton<IDistributedCache>(sp =>
			{
				// Back distributed cache by Redis if there is a connection, otherwise use an in memory store
				if (sp.GetRequiredService<IRedisConnectionService>().Enabled)
				{
					return ActivatorUtilities.CreateInstance<RedisCache>(sp);
				}

				return ActivatorUtilities.CreateInstance<MemoryDistributedCache>(sp);
			});
		}
		// % protected region % [Customise ConfigureCaching method here] end

		private void AddSwaggerService(IServiceCollection services)
		{
			// % protected region % [Customise Swagger configuration here] off begin
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("json", new OpenApiInfo {Title = "Firstapp2257", Version = "v1"});
				options.ResolveConflictingActions(a => a.First());

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});
			// % protected region % [Customise Swagger configuration here] end
		}

		private void ConfigureAuthServices(IServiceCollection services)
		{
			// % protected region % [Configure XSRF here] off begin
			services.Configure<DataProtectionOptions>(options =>
			{
				options.ApplicationDiscriminator = "Firstapp2257";
			});
			services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
			// % protected region % [Configure XSRF here] end

			// % protected region % [Configure data protection here] off begin
			services.AddDataProtection()
				.PersistKeysToDbContext<Firstapp2257DBContext>();
			// % protected region % [Configure data protection here] end

			// % protected region % [Configure password requirements here] off begin
			// Register Identity Services
			services.TryAddScoped<IUserClaimsPrincipalFactory<User>, ClaimsPrincipalFactory>();
			services.AddIdentity<User, Group>(options =>
				{
					options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
					options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
					options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;

					options.User.AllowedUserNameCharacters += @"\*";

					if (CurrentEnvironment.IsDevelopment())
					{
						options.Password.RequiredLength = 6;
						options.Password.RequiredUniqueChars = 0;
						options.Password.RequireNonAlphanumeric = false;
						options.Password.RequireLowercase = false;
						options.Password.RequireUppercase = false;
						options.Password.RequireDigit = false;
					}
					else
					{
						options.Password.RequiredLength = 12;
						options.Password.RequiredUniqueChars = 0;
						options.Password.RequireNonAlphanumeric = false;
						options.Password.RequireLowercase = false;
						options.Password.RequireUppercase = false;
						options.Password.RequireDigit = false;
					}

					// Mandate that all users must have a confirmed account
					// By default this is a requirement that the EmailConfirmed field is set to true
					// This can be changed by providing a new implementation of the IUserConfirmation<User> service
					options.SignIn.RequireConfirmedAccount = true;
				})
				.AddEntityFrameworkStores<Firstapp2257DBContext>()
				.AddDefaultTokenProviders();
			// % protected region % [Configure password requirements here] end

			// % protected region % [Customize your OIDC/oAuth2 library] off begin
			ConfigureAuthorizationLibrary(services);
			// % protected region % [Customize your OIDC/oAuth2 library] end

			var certSetting = Configuration.GetSection("CertificateSetting").Get<CertificateSetting>();
			// % protected region % [add any configuration after the cretificate] off begin
			// % protected region % [add any configuration after the cretificate] end

			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

			services.AddAuthentication(options =>
				{
					// % protected region % [Change authentication defaults here] off begin
					options.DefaultScheme = IdentityConstants.ApplicationScheme;
					options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					// % protected region % [Change authentication defaults here] end
				})
				.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
				{
					// % protected region % [Change AddJwtBearer logic here] off begin
					options.Authority = certSetting.JwtBearerAuthority;
					options.Audience = certSetting.JwtBearerAudience;
					options.RequireHttpsMetadata = false;
					options.IncludeErrorDetails = true;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						NameClaimType = OpenIdConnectConstants.Claims.Name,
						RoleClaimType = OpenIdConnectConstants.Claims.Role
					};
					// % protected region % [Change AddJwtBearer logic here] end
				})
				// % protected region % [Add additional authentication chain methods here] off begin
				// % protected region % [Add additional authentication chain methods here] end
				;

			services.ConfigureApplicationCookie(options =>
			{
				// % protected region % [Change AddCookie logic here] off begin
				var serverSettings = Configuration.GetSection(ServerSettings.SectionName).Get<ServerSettings>();
				var isHttps = serverSettings.IsHttps;

				options.LoginPath = "/api/authorization/login";
				options.LogoutPath = "/api/authorization/logout";
				options.SlidingExpiration = true;
				options.ExpireTimeSpan = TimeSpan.FromDays(7);
				options.Cookie.Name = isHttps 
					? $"__Host-.AspNetCore.{IdentityConstants.ApplicationScheme}"
					: $".AspNetCore.{IdentityConstants.ApplicationScheme}";
				options.Cookie.SameSite = SameSiteMode.Strict;
				options.Cookie.SecurePolicy = isHttps ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;
				options.EventsType = typeof(CustomCookieAuthenticationEvents);
				// % protected region % [Change AddCookie logic here] end
			});

			// % protected region % [Modify two factor method registrations here] off begin
			services.AddTwoFactorAuthenticationMethod<EmailTwoFactorMethodEvents>(
				TokenOptions.DefaultEmailProvider,
				10);
			services.AddTwoFactorAuthenticationMethod<AuthenticatorAppTwoFactorMethodEvents>(
				TokenOptions.DefaultAuthenticatorProvider,
				20);
			// % protected region % [Modify two factor method registrations here] end

			// % protected region % [Add any additional two factor method registrations here] off begin
			// % protected region % [Add any additional two factor method registrations here] end

			var cookieConfiguration = new CookieConfiguration();
			Configuration.GetSection("CookieConfiguration").Bind(cookieConfiguration);
			services.Configure<SecurityStampValidatorOptions>(options =>
			{
				options.ValidationInterval = cookieConfiguration.CookieSecurityStampValidationInterval;
				options.OnRefreshingPrincipal = context =>
				{
					// When the security stamp validator replaces the claims principal, give the new principal the
					// identities of the old principal.
					context.NewPrincipal = new ClaimsPrincipal(context.CurrentPrincipal.Identities);
					return Task.CompletedTask;
				};
			});

			// % protected region % [Add additional authentication types here] off begin
			// % protected region % [Add additional authentication types here] end

			services.AddAuthorization(options =>
			{
				// % protected region % [Change authorization logic here] off begin
				options.DefaultPolicy = new AuthorizationPolicyBuilder(
						JwtBearerDefaults.AuthenticationScheme,
						IdentityConstants.ApplicationScheme)
					.RequireAuthenticatedUser()
					.Build();
				// % protected region % [Change authorization logic here] end

				// % protected region % [Change visitor policy here] off begin
				options.AddPolicy(
					"AllowVisitorPolicy",
					new AuthorizationPolicyBuilder(
							JwtBearerDefaults.AuthenticationScheme,
							IdentityConstants.ApplicationScheme)
						.RequireAssertion(_ => true)
						.Build());
				// % protected region % [Change visitor policy here] end

				// % protected region % [Change hangfire dashboard policy here] off begin
				options.AddPolicy(
					"HangfireDashboardPolicy",
					new AuthorizationPolicyBuilder(
							JwtBearerDefaults.AuthenticationScheme,
							IdentityConstants.ApplicationScheme)
						.RequireRole("Staff", "Super Administrators")
						.Build());
				// % protected region % [Change hangfire dashboard policy here] end

				// % protected region % [Change two factor enabled policy here] off begin
				options.AddPolicy(
					"TwoFactorEnabled",
					new AuthorizationPolicyBuilder(
							JwtBearerDefaults.AuthenticationScheme,
							IdentityConstants.ApplicationScheme)
						.RequireClaim("amr", "mfa")
						.Build());
				// % protected region % [Change two factor enabled policy here] end

				// % protected region % [Configure any additional authorization options here] off begin
				// % protected region % [Configure any additional authorization options here] end
			});
		}

		private void ConfigureAuthorizationLibrary(IServiceCollection services)
		{
			// % protected region % [Configure authorization library here] off begin
			var certSetting = Configuration.GetSection("CertificateSetting").Get<CertificateSetting>();

			services.AddOpenIddict()
				.AddCore(options =>
				{
					options.UseEntityFrameworkCore()
						.UseDbContext<Firstapp2257DBContext>()
						.ReplaceDefaultEntities<Guid>();
				})
				.AddServer(options =>
				{
					options.UseMvc();
					options.EnableTokenEndpoint("/api/authorization/connect/token");

					X509Certificate2 cert = null;
					if (CurrentEnvironment.IsDevelopment())
					{
						cert = new InRootFolderCertificateProvider(certSetting).ReadX509SigningCert();
					}
					else
					{
						// not for production, currently using the same as development testing.
						// todo: Create another Certificate Provider Inheriting BaseCertificateProvider, and override ReadX509SigningCert
						// to read cerficicate from another more secure place, e.g cerficate store, aws server...
						cert = new InRootFolderCertificateProvider(certSetting).ReadX509SigningCert();
					}

					if (cert == null)
					{
						// not for production, use x509 certificate and .AddSigningCertificate()
						options.AddEphemeralSigningKey();
					}
					else
					{
						options.AddSigningCertificate(cert);
					}

					// use jwt
					options.UseJsonWebTokens();
					options.AllowPasswordFlow();
					options.AllowRefreshTokenFlow();
					options.AcceptAnonymousClients();
					options.DisableHttpsRequirement();
				});
			// % protected region % [Configure authorization library here] end
		}

		private void ConfigureScopedServices(IServiceCollection services) {
			// Register service to seed test data
			services.TryAddScoped<DataSeedHelper>();

			// Register core scoped services
			services.TryAddScoped<IUserService, UserService>();
			services.TryAddScoped<IGraphQlService, GraphQlService>();
			services.TryAddScoped<ICrudService, CrudService>();
			services.TryAddScoped<ISecurityService, SecurityService>();
			services.TryAddScoped<IIdentityService, IdentityService>();
			services.TryAddScoped<IEmailService, EmailService>();
			services.TryAddScoped<IBackgroundJobService, BackgroundJobService>();
			services.TryAddScoped<IAuditService, AuditService>();
			services.TryAddScoped<IXsrfService, XsrfService>();
			services.TryAddScoped<IPerformContextAccessor, PerformContextAccessor>();
			services.TryAddScoped<ISignInService, SignInService>();
			services.TryAddScoped<ICookieStore, DistributedCacheCookieStore>();
			services.TryAddScoped<ITwoFactorMethodEventFactory, TwoFactorMethodEventFactory>();
			services.TryAddScoped<CustomCookieAuthenticationEvents>();

			// Register context filters
			services.TryAddScoped<AntiforgeryFilter>();
			services.TryAddScoped<XsrfActionFilter>();

			// % protected region % [Configure razor light engine here] off begin
			// Register razor light templating engine
			services.TryAddSingleton<IRazorLightEngine>(_ => new RazorLightEngineBuilder()
				.UseEmbeddedResourcesProject(typeof(Startup).Assembly)
				.UseMemoryCachingProvider()
				.Build());
			// % protected region % [Configure razor light engine here] end

			// % protected region % [Configure storage provider services here] off begin
			// Configure the file system provider to use
			var storageOptions = new StorageProviderConfiguration();
			Configuration.GetSection("StorageProvider").Bind(storageOptions);
			switch (storageOptions.Provider)
			{
				case StorageProviders.S3:
					services.TryAddScoped<IUploadStorageProvider, S3StorageProvider>();
					break;
				case StorageProviders.FILE_SYSTEM:
				default:
					services.TryAddScoped<IUploadStorageProvider, FileSystemStorageProvider>();
					break;
			}
			// % protected region % [Configure storage provider services here] end

			// % protected region % [Add extra core scoped services here] off begin
			// % protected region % [Add extra core scoped services here] end
		}

		private void ConfigureGraphql(IServiceCollection services)
		{
			// GraphQL types must be registered as singleton services. This is since building the underlying graph is
			// expensive and should only be done once.
			services.TryAddSingleton<AchievementsEntityType>();
			services.TryAddSingleton<AchievementsEntityInputType>();
			services.TryAddSingleton<AchievementsEntityFormVersionType>();
			services.TryAddSingleton<AchievementsEntityFormVersionInputType>();
			services.TryAddSingleton<AddressEntityType>();
			services.TryAddSingleton<AddressEntityInputType>();
			services.TryAddSingleton<AddressEntityFormVersionType>();
			services.TryAddSingleton<AddressEntityFormVersionInputType>();
			services.TryAddSingleton<StudentsEntityType>();
			services.TryAddSingleton<StudentsEntityInputType>();
			services.TryAddSingleton<StudentsEntityFormVersionType>();
			services.TryAddSingleton<StudentsEntityFormVersionInputType>();
			services.TryAddSingleton<AssessmentNotesEntityType>();
			services.TryAddSingleton<AssessmentNotesEntityInputType>();
			services.TryAddSingleton<AssessmentsEntityType>();
			services.TryAddSingleton<AssessmentsEntityInputType>();
			services.TryAddSingleton<CommentsEntityType>();
			services.TryAddSingleton<CommentsEntityInputType>();
			services.TryAddSingleton<StaffEntityType>();
			services.TryAddSingleton<StaffEntityInputType>();
			services.TryAddSingleton<StaffEntityCreateInputType>();
			services.TryAddSingleton<AchievementsSubmissionEntityType>();
			services.TryAddSingleton<AchievementsSubmissionEntityInputType>();
			services.TryAddSingleton<AddressSubmissionEntityType>();
			services.TryAddSingleton<AddressSubmissionEntityInputType>();
			services.TryAddSingleton<StudentsSubmissionEntityType>();
			services.TryAddSingleton<StudentsSubmissionEntityInputType>();
			services.TryAddSingleton<AchievementsEntityFormTileEntityType>();
			services.TryAddSingleton<AchievementsEntityFormTileEntityInputType>();
			services.TryAddSingleton<AddressEntityFormTileEntityType>();
			services.TryAddSingleton<AddressEntityFormTileEntityInputType>();
			services.TryAddSingleton<StudentsEntityFormTileEntityType>();
			services.TryAddSingleton<StudentsEntityFormTileEntityInputType>();
			// % protected region % [Register additional graphql types here] off begin
			// % protected region % [Register additional graphql types here] end

			// Register enum GraphQl types
			services.TryAddSingleton<EnumerationGraphType<AchievementTypes>>();
			services.TryAddSingleton<EnumerationGraphType<StaffRoles>>();

			// Connect the database type to the GraphQL type
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AchievementsEntity, AchievementsEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AchievementsEntityFormVersion, AchievementsEntityFormVersionType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AddressEntity, AddressEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AddressEntityFormVersion, AddressEntityFormVersionType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<StudentsEntity, StudentsEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<StudentsEntityFormVersion, StudentsEntityFormVersionType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AssessmentNotesEntity, AssessmentNotesEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AssessmentsEntity, AssessmentsEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<CommentsEntity, CommentsEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<StaffEntity, StaffEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AchievementsSubmissionEntity, AchievementsSubmissionEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AddressSubmissionEntity, AddressSubmissionEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<StudentsSubmissionEntity, StudentsSubmissionEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AchievementsEntityFormTileEntity, AchievementsEntityFormTileEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<AddressEntityFormTileEntity, AddressEntityFormTileEntityType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<StudentsEntityFormTileEntity, StudentsEntityFormTileEntityType>();

			GraphQL.Utilities.GraphTypeTypeRegistry.Register<WhereExpression, WhereType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<Comparison, ComparisonGraphType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<StringComparison, StringComparisonGraphType>();
			GraphQL.Utilities.GraphTypeTypeRegistry.Register<HasCondition, HasConditionType>();

			// % protected region % [Add custom GraphQL Types for custom models here] off begin
			// % protected region % [Add custom GraphQL Types for custom models here] end

			// Add GraphQL core services and executors
			services.TryAddSingleton<IDocumentWriter, GraphQL.SystemTextJson.DocumentWriter>();
			services.TryAddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
			services.TryAddSingleton<IDocumentExecuter, GraphQlDocumentExecuter>();
			services.TryAddSingleton<DataLoaderDocumentListener>();
			services.AddGraphQL();

			// Add the schema and query for graphql
			services.TryAddSingleton<ISchema, Firstapp2257Schema>();
			services.TryAddSingleton<Firstapp2257Query>();
			services.TryAddSingleton<Firstapp2257Mutation>();

			services.TryAddSingleton<WhereType>();
			services.TryAddSingleton<ComparisonGraphType>();
			services.TryAddSingleton<StringComparisonGraphType>();
			services.TryAddSingleton<IdObjectType>();
			services.TryAddSingleton<NumberObjectType>();
			services.TryAddSingleton<OrderGraph>();
			services.TryAddSingleton<BooleanObjectType>();
			services.TryAddSingleton<HasConditionType>();
			// % protected region % [Add extra GraphQL types here] off begin
			// % protected region % [Add extra GraphQL types here] end
		}

		/// <summary>
		/// Read in configuration key value tuples from the appsettings.xxx files.
		/// </summary>
		/// <param name="services"></param>
		private void AddApplicationConfigurations(IServiceCollection services)
		{
			services.Configure<EmailAccount>(Configuration.GetSection("EmailAccount"));
			services.Configure<StorageProviderConfiguration>(Configuration.GetSection("StorageProvider"));
			services.Configure<FileSystemStorageProviderConfiguration>(Configuration.GetSection("FileSystemStorageProvider"));
			services.Configure<S3StorageProviderConfiguration>(Configuration.GetSection("S3StorageProvider"));
			services.Configure<ClientServerConfiguration>(Configuration.GetSection("ClientServerSettings"));
			services.Configure<SchedulerConfiguration>(Configuration.GetSection("Scheduler"));
			services.Configure<ServerSettings>(Configuration.GetSection("ServerSettings"));
			services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
			services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
			services.Configure<CookieConfiguration>(Configuration.GetSection("CookieConfiguration"));
			services.Configure<RedisCacheOptions>(Configuration.GetSection("Redis"));
			services.Configure<RedisConfiguration>(Configuration.GetSection("Redis"));
			// % protected region % [Add more configuration sections here] off begin
			// % protected region % [Add more configuration sections here] end
		}

		private void LoadScheduledTasks(IServiceCollection services)
		{
			// % protected region % [Configure scheduled task library here] off begin
			services.AddHangfire((serviceProvider, configuration) =>
			{
				configuration.UseActivator(
					new ServiceProviderActivator(serviceProvider.GetRequiredService<IServiceScopeFactory>()));

				var redisConnectionService = serviceProvider.GetRequiredService<IRedisConnectionService>();
				var redisConfiguration = serviceProvider.GetRequiredService<IOptions<RedisConfiguration>>().Value;

				if (redisConnectionService.Enabled)
				{
					configuration.UseRedisStorage(
						redisConnectionService.Connection,
						new RedisStorageOptions
						{
							Prefix = redisConfiguration.InstanceName,
						});
				}
				else
				{
					configuration.UseEFCoreStorage(
						() => serviceProvider
							.GetRequiredService<IDbContextFactory<Firstapp2257DBContext>>()
							.CreateDbContext(),
						new EFCoreStorageOptions
						{
							CountersAggregationInterval = new TimeSpan(0, 5, 0),
							DistributedLockTimeout = new TimeSpan(0, 10, 0),
							JobExpirationCheckInterval = new TimeSpan(0, 30, 0),
							QueuePollInterval = new TimeSpan(0, 1, 0),
							Schema = string.Empty,
							SlidingInvisibilityTimeout = new TimeSpan(0, 5, 0),
						});
				}
			});
			// % protected region % [Configure scheduled task library here] end
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			// % protected region % [Add Configure arguments here] off begin
			// % protected region % [Add Configure arguments here] end
			IApplicationBuilder app,
			IWebHostEnvironment env)
		{
			// % protected region % [Add methods at the beginning of the request pipeline here] off begin
			// % protected region % [Add methods at the beginning of the request pipeline here] end

			// % protected region % [Configure request logging here] off begin
			app.UseRequestLogging();
			// % protected region % [Configure request logging here] end

			// % protected region % [Configure rate limiting middleware here] off begin
			app.UseIpRateLimiting();
			// % protected region % [Configure rate limiting middleware here] end

			// % protected region % [Configure security headers here] off begin
			app.UseSecurityHeaders();
			// % protected region % [Configure security headers here] end

			// % protected region % [Change scheduled task server here] off begin
			app.UseHangfireTaskServer();
			// % protected region % [Change scheduled task server here] end

			if (env.IsDevelopment())
			{
				// % protected region % [Configure development exception page here] off begin
				app.UseDeveloperExceptionPage();
				// % protected region % [Configure development exception page here] end

				// % protected region % [Add dev environment settings here] off begin
				// % protected region % [Add dev environment settings here] end
			}
			else
			{
				// % protected region % [Configure production exception page here] off begin
				app.UseExceptionHandler("/Error");
				app.UseHsts();
				// % protected region % [Configure production exception page here] end

				// % protected region % [Add prod environment settings here] off begin
				// % protected region % [Add prod environment settings here] end

			}

			// % protected region % [Add methods before logging config here] off begin
			// % protected region % [Add methods before logging config here] end

			app.UseStaticFiles();
			app.UseSpaStaticFiles();

			app.UseMiddleware<AuditMiddleware>();

			// % protected region % [Alter swagger configuration here] off begin
			// Add Swagger json and ui
			app.UseSwagger(options =>
			{
				options.RouteTemplate = "api/swagger/{documentName}/openapi.json";
			});
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/api/swagger/json/openapi.json", "Firstapp2257");
				options.RoutePrefix = "api/swagger";
			});
			// % protected region % [Alter swagger configuration here] end

			app.UseRouting();
			// % protected region % [add configuration after routing] off begin
			// % protected region % [add configuration after routing] end

			app.UseAuthentication();
			app.UseAuthorization();
			// % protected region % [Add cors settings here] off begin
			// % protected region % [Add cors settings here] end

			// % protected region % [Configure XSRF token here] off begin
			app.UseXsrfToken();
			// % protected region % [Configure XSRF token here] end

			// % protected region % [Configure database user auditing here] off begin
			app.UseDatabaseUserAuditing();
			// % protected region % [Configure database user auditing here] end

			// % protected region % [Configure endpoints here] off begin
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
				endpoints.MapHealthChecks("/api/health");
			});
			// % protected region % [Configure endpoints here] end

			// % protected region % [add extra configuration settings here] off begin
			// % protected region % [add extra configuration settings here] end

			// % protected region % [Configure Hangfire dashboard here] off begin
			app.UseAdminHangfireDashboard();
			// % protected region % [Configure Hangfire dashboard here] end

			// % protected region % [Alter SPA configuration here] off begin
			app.UseReactbot();
			// % protected region % [Alter SPA configuration here] end
		}

		// % protected region % [Add any custom startup methods here] off begin
		// % protected region % [Add any custom startup methods here] end
	}
}
