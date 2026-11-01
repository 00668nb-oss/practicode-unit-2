using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlSerializer
{
    public class HtmlElement
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public List<string> Attributes { get; set; } = new();
        public List<string> Classes { get; set; } = new();
        public string InnerHtml { get; set; } = "";
        public HtmlElement? Parent { get; set; } = null;
        public List<HtmlElement> Children { get; set; } = new();
    
    
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue= new Queue<HtmlElement>();

            foreach (var item in Children)
                queue.Enqueue(item);

            while (queue.Count > 0)
            {
                var element=queue.Dequeue();
                yield return element;

                 foreach (var item in element.Children)
                    queue.Enqueue(item);
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement? current = Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
    }
}
