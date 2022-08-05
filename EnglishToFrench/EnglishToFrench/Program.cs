using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Collections;
using System.Web.Script.Serialization;

namespace EnglishToFrench
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //This is intended as a POC, there is a limit to the number of requests you can send to Google Translate API, after which it starts to refuse requests.

            string[] englishSentenceArray = {"Farnborough International Airshow, biennial global aerospace, defence and space trade event which showcases the latest commercial and military aircraft. Manufacturers such as Airbus and Boeing are expected to display their products and announce new orders * 2020 event was held virtually after the physical show was cancelled due to the coronavirus (COVID-19) pandemic",
                "Labour market statistics: integrated national release, including the latest data for employment, economic activity, economic inactivity, unemployment, claimant count, average earnings, productivity, unit wage costs, vacancies & labour disputes",
                "City of London Corporation's Financial and Professional Services dinner. Chancellor Rishi Sunak and Bank of England Governor Andrew Bailey make their annual Mansion House speeches at the event hosted by the Lord Mayor of the City of London Vincent Keaveny" };
            
            foreach (string englishSentence in englishSentenceArray)
            {
                Console.WriteLine("\nEnglish text: " + englishSentence);

                Console.WriteLine("\nFrench translation: " + TranslateEnglishToFrench(englishSentence));
            }
            Console.ReadKey();
        }
        public static string TranslateEnglishToFrench(string englishSentence)
        {
            string googleTranslateUrl = String.Format
                ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                 "en", "fr", Uri.EscapeUriString(englishSentence));

            HttpClient httpClient = new HttpClient();

            string result = httpClient.GetStringAsync(googleTranslateUrl).Result;

            var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);

            var frenchArray = jsonData[0];

            string frenchSentence = "";

            foreach (object item in frenchArray)
            {
                IEnumerable translationLineObject = item as IEnumerable;
                IEnumerator translationLineString = translationLineObject.GetEnumerator();
                translationLineString.MoveNext();
                frenchSentence += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }

           return frenchSentence.Substring(1);
        }
    }
}
