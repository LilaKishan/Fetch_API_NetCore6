using Fetch_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fetch_API.Controllers
{
    public class PersonController : Controller
    {
    
        Uri baseaddress = new Uri("http://localhost:5158/api");
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
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Person/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;

                dynamic jsonobject = JsonConvert.DeserializeObject(data);

                var dataOfObject = jsonobject.data;
                var extractDataJson = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);

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
        public IActionResult Edit(int PersonID)
        {
            PersonModel person = new PersonModel();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Person/GetByID/{PersonID}").Result;

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

        #region Delete

        [HttpDelete("{PersonID}")]
        public IActionResult DeleteByID(int PersonID)
        {
            PersonModel person = new PersonModel();
            HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/Person/Delete/{PersonID}").Result;
            Console.WriteLine(PersonID);
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Person Deleted successfully...!!!";
            }
            return RedirectToAction("Get");
        }
        #endregion

        #region ADD/Edit method:Save
        [HttpPost]
        public async Task<IActionResult> Save(PersonModel modelPerson)
        {
            try
            {
                MultipartFormDataContent formdata = new MultipartFormDataContent();

                formdata.Add(new StringContent(modelPerson.Pname), "Pname");
                formdata.Add(new StringContent(modelPerson.Email), "Email");
                formdata.Add(new StringContent(modelPerson.Contact), "Contact");

                if (modelPerson.PersonID == 0)
                {
                    HttpResponseMessage response = await _client.PostAsync($"{_client.BaseAddress}/Person/Post", formdata);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["InsertMessage"] = "Person Inserted successfully...!!!";
                        return RedirectToAction("Get");
                    }
                }
                else
                {
                    HttpResponseMessage response = await _client.PutAsync($"{_client.BaseAddress}/Person/Put/{modelPerson.PersonID}", formdata);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["UpdateMessage"] = "Person Updated successfully...!!!";
                      return  RedirectToAction("Get");
                    }
                }


            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error occure of "+ex.Message;
              
            }
            return RedirectToAction("Get");
        }
        #endregion

    }
}
