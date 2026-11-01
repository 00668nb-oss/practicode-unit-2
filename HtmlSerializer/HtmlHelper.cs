using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HtmlSerializer
{
    public class HtmlHelper
    {
        public static HtmlHelper Instance { get; } = new HtmlHelper();

        public string[] Tags { get; set; }
        public string[] VoidTags { get; set; }

        private HtmlHelper()
        {
            string htmlTagsPath = Path.Combine(AppContext.BaseDirectory, "Data", "HtmlTags.json");
            string htmlVoidTagsPath = Path.Combine(AppContext.BaseDirectory, "Data", "HtmlVoidTags.json");
            var tagsContent = File.ReadAllText(htmlTagsPath);
            var voidTagsContent = File.ReadAllText(htmlVoidTagsPath);
            Tags = JsonSerializer.Deserialize<string[]>(tagsContent) ?? [];
            VoidTags = JsonSerializer.Deserialize<string[]>(voidTagsContent) ?? [];
        }
    }
}
