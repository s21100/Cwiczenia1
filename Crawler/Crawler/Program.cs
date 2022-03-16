using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler 
{
    class Program 
    {
        static async Task Main(string[] args) 
        {

            if (args.Length > 0) 
            {
                string websiteUrl = args[0];
                bool isValid = Uri.TryCreate(websiteUrl, UriKind.Absolute, out Uri uriResult) 
                    && (uriResult.Scheme == Uri.UriSchemeHttps || uriResult.Scheme == Uri.UriSchemeHttp);
                if (isValid) 
                {
                    HttpClient httpClient = new HttpClient();
                    try 
                    {
                        var body = await httpClient.GetStringAsync(websiteUrl);

                        Regex regex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");
                        MatchCollection matches = regex.Matches(body);
                        
                        if (matches.Count > 0) 
                        {
                            var uniqueMatches = matches.Select(obj => obj.Value).Distinct();
                            foreach (var match in uniqueMatches) 
                            {
                                Console.WriteLine(match);
                            }
                        }
                        else 
                        {
                            Console.WriteLine("Nie znaleziono adresów email");
                        }
                    }
                    catch (HttpRequestException) 
                    {
                        Console.WriteLine("Bład w czasie pobierania strony");
                    } 
                    finally 
                    {
                        httpClient.Dispose();
                    }                 
                } 
                else 
                {
                    throw new ArgumentException();
                }    
            } 
            else 
            {
                throw new ArgumentNullException();
            }
            
        }
    }
}
