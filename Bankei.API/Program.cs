using Bankei.Application.Commands.CriarInvestimentos;
using Bankei.Application.Commands.SacarInvestimentos;
using Bankei.Application.DTO_s;
using Bankei.Application.Queries.ObterInvestimentos;
using Bankei.Domain.Repositories;
using Bankei.Infrastructure.Data;
using Bankei.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IInvestimentoRepository, InvestimentoRepository>();

builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining(typeof(CriarInvestimentoCommand)));
builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining(typeof(SacarInvestimentoCommand)));

builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining(typeof(ObterInvestimentoQuery)));

builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblyContaining(typeof(InvestimentoDTO)));

builder.Services.AddTransient<IRequestHandler<ObterInvestimentoQuery, InvestimentoDTO>, ObterInvestimentoQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
