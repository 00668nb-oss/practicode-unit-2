using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlSerializer
{
    public static class HtmlElementExtensions
    {
        public static bool IsMatches(HtmlElement htmlElement, Selector selector)
        {
            if (selector.Id != null
                && htmlElement.Id != selector.Id)
                return false;

            if (selector.TagName != null
                && htmlElement.Name != selector.TagName)
                return false;

            if (!selector.Classes.All(c => htmlElement.Classes.Contains(c)))
                return false;

            return true;
        }

        private static void FindElementsRecursive(HtmlElement root, Selector selector, HashSet<HtmlElement> results)
        {
            var descendants = root.Descendants();

            foreach (var item in descendants)
            {
                if (IsMatches(item, selector))
                {
                    if (selector.Child == null)
                        results.Add(item);
                    else
                        FindElementsRecursive(item,selector.Child, results);
                }
            }
        }

        public static IEnumerable<HtmlElement> FindElements(
            this HtmlElement root, Selector selector)
        {
            HashSet<HtmlElement> results = new();
            FindElementsRecursive(root, selector, results);

            return results;
        }
    }
}
