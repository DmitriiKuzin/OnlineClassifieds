using Classifieds.Auth;
using DAL;
using MQ;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClassifiedsDbContext>();
builder.Services.AddRabbitMq();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


await MqExtension.WaitForRabbitReady();
app.Run();