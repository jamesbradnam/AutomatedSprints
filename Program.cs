//iteration json format
// {
//   "name": "name",
//   "attributes": {
//   "startDate": "yyyy-MM-ddTHH:mm:ssZ",
//   "finishDate": "yyyy-MM-ddTHH:mm:ssZ"
//   }
// }

namespace HttpClientStatus;
using System.Net.Http;
class Program
{
    public static void Main(string[] args)
    {   
        IDictionary<string, string> variables = new Dictionary<string, string>();

        foreach (string line in System.IO.File.ReadLines(@"ProjectDetails.txt"))
        {
            string[] subStrings = line.Split('=');
            
            subStrings[0] = subStrings[0].Remove(0, 4); //remove "var " 
            subStrings[0] = subStrings[0].Trim();
            subStrings[1] = subStrings[1].Trim(); 

            if (subStrings[0].Equals("accessToken"))
            {
                subStrings[1] = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", subStrings[1])));
            }
            variables.Add(subStrings[0], subStrings[1]);  
            
        }

        ConnectApi apiAccess = new ConnectApi(variables["organisation"], variables["project"], variables["path"], 
                                              variables["accessToken"]);      
        int sprintLengthDays = Int32.Parse(variables["sprintLengthDays"]);

        DateTime startDate = DateTime.ParseExact(variables["startDate"], "dd/MM/yyyy", null);
        DateTime finishDate = startDate.AddDays(sprintLengthDays);

        foreach (string name in System.IO.File.ReadLines(@"SprintNames.txt"))
        {  
            StringContent iterationJson = apiAccess.CreateJson(name, startDate, sprintLengthDays);
            apiAccess.Post(iterationJson);

            //add 1 sprint to startDate
            startDate = startDate.AddDays(sprintLengthDays);
        }
    }
}
