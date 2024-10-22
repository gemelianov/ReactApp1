using ReactApp1.Server.Services;
using ReactApp1.Server.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDocumentsService, DocumentsService>();
builder.Services.AddTransient<IPatternsService, PatternsService>();
builder.Services.AddTransient<ITagsService, TagsService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IPatternsUploader, PatternsUploaderService>();
builder.Services.AddTransient<IDocumentFiller, DocumentFiller>();



var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
