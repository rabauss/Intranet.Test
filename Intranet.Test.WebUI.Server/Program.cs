using Intranet.Test.WebUI.Server.Components;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureEndpoints(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
  services.AddHealthChecks();
  services.AddHttpContextAccessor();

  services.AddRazorComponents()
      .AddInteractiveServerComponents()
      .AddInteractiveWebAssemblyComponents();

  services.AddServerSideBlazor();
  services.AddBootstrapBlazor(options =>
  {
    options.DefaultCultureInfo = "de";
    options.ToastPlacement = BootstrapBlazor.Components.Placement.TopEnd;
  },
  locationOptions =>
  {
    locationOptions.AdditionalJsonFiles = ["Locales/de.json"];
  });
}

void ConfigureEndpoints(WebApplication app)
{
  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
  }
  else
  {
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

  app.UseStatusCodePagesWithReExecute("/error/{0}");

  app.UseHttpsRedirection();

  app.MapStaticAssets();

  app.UseAntiforgery();

  app.MapRazorComponents<App>()
      .AddInteractiveServerRenderMode()
      .AddInteractiveWebAssemblyRenderMode();

  app.MapHealthChecks("/health");
}
