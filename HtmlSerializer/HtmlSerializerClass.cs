using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace HtmlSerializer
{
    public class HtmlSerializerClass
    {
        public async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        public IEnumerable<string> ParseHtml(string html)
        {
            var htmlLines = new Regex("<(.*?)>", RegexOptions.Singleline).Split(html).Select(s => s.Trim()).Where(s => s.Length > 0);

            return htmlLines;
        }

        public MatchCollection AttributesHtml(string htmlElement)
        {
            var attributes = new Regex(@"([^\s=/>]+)(?:\s*=\s*(?:""([^""]*)""|'([^']*)'|([^\s>]+)))?");
            return attributes.Matches(htmlElement);
        }

        public HtmlElement BuildTree(IEnumerable<string> htmlLines)
        {
            HtmlElement root = new HtmlElement();
            HtmlElement currentElement = root;

            foreach (var item in htmlLines)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    string tagName = item.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)[0].TrimEnd('/');
                    
                    if (item.StartsWith("!DOCTYPE", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (item.Trim() == "/html")
                        break;

                    if (item[0] == '/')
                    {
                        if (currentElement.Parent != null)
                            currentElement = currentElement.Parent;
                    }
                    else if (!HtmlHelper.Instance.Tags.Contains(tagName)
                                        && !HtmlHelper.Instance.VoidTags.Contains(tagName))
                        currentElement.InnerHtml += (currentElement.InnerHtml.Length > 0 ? " " : "") + item;
                    else
                    {
                        HtmlElement newElement = new HtmlElement();
                        newElement.Name = tagName;
                        newElement.Parent = currentElement;
                        currentElement.Children.Add(newElement);

                        if (!HtmlHelper.Instance.VoidTags.Contains(tagName)
                                    && !item.EndsWith('/'))
                            currentElement = newElement;

                        string attributesPart = item.Substring(item.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)[0].Length);
                        MatchCollection matches = AttributesHtml(attributesPart);
                        foreach (Match match in matches)
                        {
                            string name = match.Groups[1].Value;
                            string value = match.Groups[2].Success ? match.Groups[2].Value
                                         : match.Groups[3].Success ? match.Groups[3].Value
                                         : match.Groups[4].Value;

                            newElement.Attributes.Add(match.Value);

                            if (name == "id")
                            {
                                newElement.Id = value;
                            }
                            else if (name == "class")
                            {
                                newElement.Classes.AddRange(value.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                            }
                        }

                    }
                }
            }

            return root;
        }
    }
}
