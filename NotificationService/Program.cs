using Classifieds.Auth;
using DAL;
using MQ;
using NotificationService.Consumers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClassifiedsDbContext>();
builder.Services.AddRabbitMq(x =>
{
    x.AddConsumer<ListingPublishRequestedConsumer>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


await MqExtension.WaitForRabbitReady();
app.Run();