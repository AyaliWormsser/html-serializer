using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Child { get; set; }
        public Selector Parent { get; set; }


        public static Selector convert(string selectorString)
        {
            string[] strings = selectorString.Split(' ');

            Selector root = new Selector();

            Selector current = root;

            foreach (string s in strings)
            {

                string[] parts = s.Split('#', '.');

                string temp = s;

                foreach (string p in parts)
                {

                    if (p.StartsWith("#"))
                    {
                        current.Id = p;

                        temp = temp.Substring(p.Length + 1);
                    }

                    else if(p.EndsWith(".")) 
                    {
                        current.Classes.Add(p);

                        temp = temp.Substring(p.Length + 1);
                    }

                    else
                    {
                        if(HtmlHelper.Instance.AllTags.Contains(p))
                        {
                            temp = temp.Substring(p.Length);

                            current.TagName = p;
                        }

                        else
                        {
                            Console.WriteLine("Error");
                        }

                    }

                }

                Selector newSelector = new Selector();

                newSelector.Parent = current;

                current.Child = newSelector;

                current = newSelector;

            }

            current.Parent.Child = null;

            return root;

        }

    }
}
