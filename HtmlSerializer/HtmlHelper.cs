using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace HtmlSerializer
{
    public class HtmlHelper
    {

        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] AllTags { get; private set; }
        public string[] SelfClosingTags { get; private set; }


        private HtmlHelper()
        {
            try
            {
                // קריאת תוכן קבצי JSON
                string allTagsJson = File.ReadAllText("AllTags.json");

                string selfClosingTagsJson = File.ReadAllText("SelfClosingTags.json");

                // המרת הנתונים למערכים באמצעות Deserialize
                AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);

                SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
            }

            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");

            }



        }
    }
}
