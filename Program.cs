using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Net;

namespace ConsoleApplication1
{
    public class SortedItem
    {
        public string gender { get; set; }
        public string name { get; set; }
    }

    class Program
    {
        static void Main()
        {
            //Use this to process offline data,see below code.
            /*string jsonOfflineData = @"
               [  
                   {  
                      'name':'Bob',
                      'gender':'Male',
                      'age':23,
                      'pets':[
                         {  
                            'name':'Garfield',
                            'type':'Cat'
                         },
                         {  
                            'name':'Fido',
                            'type':'Dog'
                         }
                      ]
                   },
                   {  
                      'name':'Jennifer',
                      'gender':'Female',
                      'age':18,
                      'pets':[
                         {  
                            'name':'Garfield',
                            'type':'Cat'
                         }
                      ]
                   },
                   {  
                      'name':'Steve',
                      'gender':'Male',
                      'age':45,
                      'pets': null
                   },
                   {  
                      'name':'Fred',
                      'gender':'Male',
                      'age':40,
                      'pets':[
                         {  
                            'name':'Tom',
                            'type':'Cat'
                         },
                         {  
                            'name':'Max',
                            'type':'Cat'
                         },
                         {  
                            'name':'Sam',
                            'type':'Dog'
                         },
                         {  
                            'name':'Jim',
                            'type':'Cat'
                         }
                      ]
                   },
                   {  
                      'name':'Samantha',
                      'gender':'Female',
                      'age':40,
                      'pets':[
                         {  
                            'name':'Tabby',
                            'type':'Cat'
                         }
                      ]
                   },
                   {  
                      'name':'Alice',
                      'gender':'Female',
                      'age':64,
                      'pets':[
                         {  
                            'name':'Simba',
                            'type':'Cat'
                         },
                         {  
                            'name':'Nemo',
                            'type':'Fish'
                         }
                      ]
                   }
                ]";*/
                
            using (var client = new WebClient())
            {
                //Option 1: Using webservice to retrive the json data
                var json = client.DownloadString("http://agl-developer-test.azurewebsites.net/people.json");
                JArray arr = JArray.Parse(json);

                //Option 2: Using offline data as provided above
                //JArray arr = JArray.Parse(jsonOfflineData);

                IList<SortedItem> SortedList = arr.SelectMany(obj => fnGetPets(obj)).OrderByDescending(obj => obj.gender).ThenBy(obj => obj.name).ToList();

                Console.WriteLine("Male");
                Console.WriteLine("-----");
                SortedList.Where(obj => obj.gender == "Male").Select(obj => obj.name).ToList().ForEach(Console.WriteLine);
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                Console.WriteLine("Female");
                Console.WriteLine("-----");
                SortedList.Where(obj => obj.gender == "Female").Select(obj => obj.name).ToList().ForEach(Console.WriteLine);

                Console.WriteLine(" ");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
        static private IList<SortedItem> fnGetPets(JToken obj)
        {
            return obj["pets"].Where(pet => (String)pet["type"] == "Cat").Select(pet => new SortedItem { gender = (string)obj["gender"], name= (string)pet["name"] }).ToList();
        }
    }
}
