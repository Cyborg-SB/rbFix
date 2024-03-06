
using RbFix.Infrastructure.Configuration.Setings;
using RbFix.Infrastructure.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sessionsConfig = "\r\n# default settings for sessions\r\n[DEFAULT]\r\nFileStorePath=store\r\nFileLogPath=log\r\nReconnectInterval=60\r\n\r\n# session definition\r\n[SESSION]\r\n# inherit FileStorePath, FileLogPath, ConnectionType, \r\n#    ReconnectInterval and SenderCompID from default\r\nBeginString=FIX.4.4\r\nConnectionType=acceptor\r\nSenderCompID=ARCAA\r\nTargetCompID=TW1\r\nStartTime=00:00:01\r\nEndTime=23:59:59\r\nHeartBtInt=20\r\nSocketAcceptPort=9823\r\nSocketAcceptHost=127.0.0.1\r\nDataDictionary= Infrastructure/Dictionaries/FIX44EntrypointGatewayEquities.xml\r\n\r\n\r\n\r\n# session definition\r\n[SESSION]\r\n# inherit FileStorePath, FileLogPath, ConnectionType, \r\n#    ReconnectInterval and SenderCompID from default\r\nBeginString=FIX.4.4\r\nTargetCompID=ARCAA\r\nSenderCompID=TW1\r\nConnectionType=initiator\r\nStartTime=00:00:01\r\nEndTime=23:59:59\r\nHeartBtInt=20\r\nSocketConnectPort=9823\r\nSocketConnectHost=127.0.0.1\r\nDataDictionary= Infrastructure/Dictionaries/FIX44EntrypointGatewayEquities.xml\r\n";

builder.Services.FixEngineInit(new FixEngineSettings(), sessionsConfig);
builder.Services.RegisterFixManagerComponent();

builder.Host.ConfigureAppConfiguration(x =>
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    x.AddJsonFile("appsettings.json");
    x.AddJsonFile($"appsettings.{env}.json");
});

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
