using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlSerializer
{
    public class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; } = new();
        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }


        public static Selector Parse(string query)
        {
            string[] parts = query.Split(' ',
                StringSplitOptions.RemoveEmptyEntries);

            Selector? root = null;
            Selector? current = null;

            foreach (string part in parts)
            {
                Selector selector = new Selector();

                string[] tokens = Regex.Split(part, @"(?=[.#])");

                foreach (string token in tokens)
                {
                    if (token.StartsWith("."))
                        selector.Classes.Add(token.Substring(1));

                    else if (token.StartsWith("#"))
                        selector.Id = token.Substring(1);

                    else if (!string.IsNullOrEmpty(token) && HtmlHelper.Instance.Tags.Contains(token))
                        selector.TagName = token;
                }

                if (root == null)
                    root = selector;

                if (current != null)
                {
                    current.Child = selector;
                    selector.Parent = current;
                }

                current = selector;

            }

            return root ?? new Selector();
        }
    }
}
