using RazorEngineCore;

namespace CloudPhoto.Helpers;

public static class RenderViewHelper
{
    public static async Task<string> GetHtmlFromRazor(string viewPath, object? model = null)
    {
        using (var reader = new StreamReader(viewPath))
        {
            var text = await reader.ReadToEndAsync();
            var razorEngine = new RazorEngine();
            var template = await razorEngine.CompileAsync(text);
            var result = await template.RunAsync(model);
            return result;
        }
    }

    public static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
