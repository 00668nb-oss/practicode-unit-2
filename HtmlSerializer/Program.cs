using HtmlSerializer;

HtmlSerializerClass html = new HtmlSerializerClass();
string strs = await html.Load("https://example.com");

var htmlLines = html.ParseHtml(strs);
HtmlElement root = html.BuildTree(htmlLines);

Selector selector = Selector.Parse("html body p"); 
var results = root.FindElements(selector);
Console.WriteLine($"Results count: {results.Count()}");

foreach (var line in results)
{
    Console.WriteLine(line.Name);
}