using bazyProjektBlazor.Auth;
using bazyProjektBlazor.Components;
using bazyProjektBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ICurrentUser, CurrentUser>();

builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddScoped<IUsersService, UsersService>();

builder.Services.AddScoped<ITeamsService, TeamsService>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IMeetingsService, MeetingsService>();

builder.Services.AddBlazorBootstrap();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStatusCodePagesWithRedirects("/Error");

app.Run();
