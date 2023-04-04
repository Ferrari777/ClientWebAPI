using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using ClientWebAPI.Models;
using System.Security.Cryptography.X509Certificates;

namespace ClientWebAPI.Controllers
{
    public class UserController : Controller
    {
        //URI of consumed API
        Uri baseAddress = new Uri("https://localhost:7278/api");

        //Action method that returns collection of all users
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> userList = new List<UserViewModel>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/User");

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    userList = JsonConvert.DeserializeObject<List<UserViewModel>>(data);
                }
            }

            return View(userList);
        }

        //Action method that returns default view for adding new user
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Action method that adds new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Automatically add audit info
                    user.Registered = DateTime.Now;

                    string data = JsonConvert.SerializeObject(user);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = baseAddress;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.PostAsync(baseAddress + "/User", content).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["successMessage"] = "User was created.";
                            return RedirectToAction("Index");
                        }
                    }
                }

                return View(user);
            }
            catch (Exception exc)
            {
                TempData["errorMessage"] = exc.Message;
                return View();
            }
        }

        //Action method that returns parameters of existing user for further modifying
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || id <= 0)
                {
                    return NotFound();
                }

                UserViewModel user = new UserViewModel();

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = baseAddress;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/User/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        user = JsonConvert.DeserializeObject<UserViewModel>(data);
                    }
                }

                return View(user);
            }
            catch (Exception exc)
            {
                TempData["errorMessage"] = exc.Message;
                return View();
            }
        }

        //Action method that updates parameters of existing user
        [HttpPost]
        public IActionResult Edit(UserViewModel user)
        {
            try
            {
                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = baseAddress;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.PutAsync(baseAddress + "/User/" + user.Id, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "User details were updated.";
                        return RedirectToAction("Index");
                    }
                }

                return View(user);
            }
            catch (Exception exc)
            {
                TempData["errorMessage"] = exc.Message;
                return View();
            }
        }

        //Action method that returns parameters of specified user for further deleting
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || id <= 0)
                {
                    return NotFound();
                }

                UserViewModel user = new UserViewModel();

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = baseAddress;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/User/" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        user = JsonConvert.DeserializeObject<UserViewModel>(data);
                    }
                }

                return View(user);
            }
            catch (Exception exc)
            {
                TempData["errorMessage"] = exc.Message;
                return View();
            }
        }

        //Action method that delete parameters of specified user
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = baseAddress;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.DeleteAsync(baseAddress + "/User/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["successMessage"] = "User details were deleted.";
                        return RedirectToAction("Index");
                    }
                }

                return View();
            }
            catch (Exception exc)
            {
                TempData["errorMessage"] = exc.Message;
                return View();
            }
        }

    }
}
