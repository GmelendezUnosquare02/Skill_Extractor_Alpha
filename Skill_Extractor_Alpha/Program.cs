using Skill_Extractor_Alpha.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

// Add HttpClientFactory
builder.Services.AddHttpClient();

// Add Controllers
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
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

// Map Controllers
app.MapControllers();

app.Run();
