using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlSerializer
{

    class Program
    {

        static async Task Main(string[] args)
        {
            var html = await Load("https://learn.malkabruk.co.il/");

            var cleanHtml = new Regex("\\s").Replace(html, "");

            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);



            HtmlElement rootElement = null;

            HtmlElement currentElement = null;

            List<string> lines = htmlLines.ToList();

            foreach (var line in lines)
            {
                var firstWord = line.Split(' ')[0];

                if (firstWord == "/html")
                {
                    break;
                }

                if (firstWord.StartsWith("/"))
                {

                    if(currentElement.Parent!=null)

                        currentElement = currentElement.Parent;

                }

                else if (HtmlHelper.Instance.AllTags.Contains(firstWord))
                {
                    // make a new element

                    HtmlElement newElement = new HtmlElement(firstWord);

                    var attributeMatch = Regex.Match(line, "([a-zA-Z]+)=\"([^\"]+)\"");

                    while (attributeMatch.Success)
                    {
                        var attributeName = attributeMatch.Groups[1].Value;

                        var attributeValue = attributeMatch.Groups[2].Value;

                        // update the classes list

                        if (attributeName.ToLower() == "class")
                        {

                            newElement.classes.AddRange(attributeValue.Split(' '));

                            attributeMatch = attributeMatch.NextMatch();

                        }

                        // set name and id

                        newElement.Name = firstWord;

                        newElement.Id = newElement.Attributes.Find(attr => attr.Name.ToLower() == "id") ?.Value;

                        // update the children and parent

                        if (currentElement != null)
                        {

                            currentElement.Children.Add(newElement);

                            newElement.Parent = currentElement;

                        }

                        else
                        {

                            rootElement = newElement;

                        }

                        if (line.EndsWith("/") || HtmlHelper.Instance.SelfClosingTags.Contains(firstWord))
                        {
                            
                            if (currentElement.Parent != null)

                                currentElement = currentElement.Parent;

                        }
                    }
                }

                else
                {

                    if (currentElement != null)
                    {
                        currentElement.InnerHtml += line;
                    }

                }


            }

            
            Selector root = Selector.convert("div div a.home-hero-button1 button");
            
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();

            result = rootElement.FindElements(root);

            await Console.Out.WriteLineAsync("res:");

            Console.WriteLine(result.Count());

            result.ToList().ForEach(r => Console.WriteLine(r.ToString()));

            Console.ReadLine();

        }





        async static Task<string> Load(string url)
        {

            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);

            var html = await response.Content.ReadAsStringAsync();

            return html;

        }

    }

}


