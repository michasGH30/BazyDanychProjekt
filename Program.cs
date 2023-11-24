using bazyProjektBlazor.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddSession();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped(sp =>
{
	var httpClient = new HttpClient
	{
		BaseAddress = new Uri("https://localhost:7267/")
	};
	return httpClient;
});

builder.Services.AddControllers();

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

app.UseSession();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
