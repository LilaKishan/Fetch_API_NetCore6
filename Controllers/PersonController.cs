using Fetch_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fetch_API.Controllers
{
    public class PersonController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        Uri baseaddress = new Uri("http://localhost:5158/api/Person");
        private readonly HttpClient _client;

        public PersonController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseaddress;
        }

        #region GET
        [HttpGet]
        public IActionResult Get()
        {
            List<PersonModel> persons = new List<PersonModel>();
            HttpResponseMessage response = _client.GetAsync(baseaddress).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                dynamic jsonobject = JsonConvert.DeserializeObject(data);

                var dataOfObject = jsonobject.data;
                var extractDataJson = JsonConvert.SerializeObject(dataOfObject);

                persons = JsonConvert.DeserializeObject<List<PersonModel>>(extractDataJson);
            }
            return View("PersonList", persons);
        }
        #endregion


        public IActionResult AddPerson()
        {
            return View();
        }
        #region GetByID

        [HttpGet]
        public IActionResult GetByID(int id)
        {
            PersonModel person = new PersonModel();
            HttpResponseMessage response = _client.GetAsync(baseaddress+"/"+id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                dynamic jsonobject = JsonConvert.DeserializeObject(data);

                var dataOfObject = jsonobject.data;
                var extractDataJson = JsonConvert.SerializeObject(dataOfObject);

                person = JsonConvert.DeserializeObject<PersonModel>(extractDataJson);
            }
            return View("AddPerson", person);
        }
        #endregion

        


    }
}
