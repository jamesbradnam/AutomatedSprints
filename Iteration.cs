namespace HttpClientStatus;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

class ConnectApi
{
    private string url;
    private static HttpClient client = new HttpClient();
    public ConnectApi(string organisation, string project, string parentPath, string token)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

        this.url = "https://dev.azure.com/"+organisation+"/"+project+"/_apis/wit/classificationnodes/1"+parentPath+"?api-version=6.0";
    }
    public StringContent CreateJson(string name, DateTime startDate, int sprintLengthDays)
    {
        string startDateStr = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

        DateTime finishDate = startDate.AddDays(sprintLengthDays-1);
        string finishDateStr = finishDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

        Sprint sprint = new Sprint(name, startDateStr, finishDateStr);
        string sprintJson = JsonConvert.SerializeObject(sprint);

        return new StringContent(sprintJson, Encoding.UTF8, "application/json");
    }
    public void Post(StringContent data)
    {
        var task = Task.Run(() => client.PostAsync(this.url, data)); 
        task.Wait();
        //var response = task.Result;
    }
}