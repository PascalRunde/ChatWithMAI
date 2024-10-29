using ChatWithMAI.Services;
using Microsoft.AspNetCore.ResponseCompression;
using ChatWithMAI.Components;
using ChatWithMAI.Hubs;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddAIBotService();
builder.Services.AddMessageRoutingService();
builder.Services.AddSessionService();
builder.Services.AddSignUpService();
builder.Services.AddTicketService();
builder.Services.AddUserMatchingService();

builder.Services.AddSignalR();
builder.Services.AddDataProtection()
    .SetApplicationName("chatwithmaiapp")
    .PersistKeysToFileSystem(new DirectoryInfo(@"/app/my_keys/"));

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ChatWithMAI.Client._Imports).Assembly);

app.MapHub<ChatHub>("/chathub");

app.Run();
