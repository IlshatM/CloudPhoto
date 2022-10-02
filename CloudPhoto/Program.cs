using CloudPhoto;
using CloudPhoto.Handlers;

if (args.Length == 1 && args[0] == Constants.InitCommand)
{
    var initHandler = new InitHandler();
    await initHandler.Handle();
    return;
}

var builder = ConsoleApp.CreateBuilder(args, options => { options.ApplicationName = "cloudphoto"; });
builder.ConfigureServices(Startup.ConfigureServices);
var app = builder.Build();

app.AddCommands<InitHandler>();
app.AddCommands<UploadHandler>();
app.AddCommands<DownloadHandler>();
app.AddCommands<ListHandler>();
app.AddCommands<DeleteHandler>();

app.Run();
