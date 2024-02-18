using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HtmlSerializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<string> classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement(string name)
        {
            Name = name;

            Attributes = new List<Attribute>();

            classes = new List<string>();

            Children = new List<HtmlElement>();

        }

        public IEnumerable<HtmlElement> Descendants()
        {

            Queue<HtmlElement> queue = new Queue<HtmlElement>();

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement currentElement = queue.Dequeue();

                yield return currentElement;

                foreach (HtmlElement child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }

            }

        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement element = this;

            while (element.Parent != null)
            {
                yield return this.Parent;

                element = element.Parent;
            }

        }

        public  HashSet<HtmlElement> FindElements( Selector selector)
        {

            HashSet<HtmlElement> result = new HashSet<HtmlElement>();

            FindElementsRecursively(this, selector, result);

            return result;

        }

        private static void FindElementsRecursively(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
        {

            if (Matches(element, selector))
            {

                result.Add(element);

            }

            foreach (var child in element.Children)
            {

                FindElementsRecursively(child, selector, result);

            }


        }

        public static bool Matches(HtmlElement element, Selector selector)
        {
            
            if (selector.TagName != null && element.Name != selector.TagName)

                return false;

            if (selector.Id != null && element.Id != selector.Id)

                return false;

            if (selector.Classes != null)
            {

                foreach (string cssClass in selector.Classes)
                {

                    if (!element.classes.Contains(cssClass))

                        return false;

                }
            }

            return true;
        }

    }
}
