using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.RequestModel;
using Repositories.ResponseModel;

namespace PE_PRN231_FA24_TrialTest_KieuQuangMinh_FE.Pages.PersonManagement
{
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public PersonRequestModel Person { get; set; } = new PersonRequestModel
        {
            Viruses = new List<VirusRequestModel?> { new VirusRequestModel() }
        };

        [BindProperty]
        public List<string>? Viruses { get; set; } = [];

        [BindProperty]
        public int Id { get; set; }
        private async Task GetViruses(string token)
        {
            var resposne = await Common.SendRequestAsync<HttpResponseMessage>($"{Common.BaseURL}/api/viruses", HttpMethod.Get, null, token);
            if (!resposne.IsSuccessStatusCode)
            {
                TempData["err"] = await Common.ReadError(resposne);
            }
            Viruses = await Common.ReadT<List<string>?>(resposne);
        }
        private async Task GetPerson(string token, int id)
        {
            var response = await Common.SendRequestAsync<HttpResponseMessage>($"{Common.BaseURL}/api/person/{id}", HttpMethod.Get, null, token);
            if (!response.IsSuccessStatusCode)
            {
                TempData["err"] = await Common.ReadError(response);
            }
            Person = await Common.ReadT<PersonRequestModel>(response);
        }
        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                var token = HttpContext.Session.GetString("token");
                if (token == null)
                {
                    return RedirectToPage("../Index");
                }
                await GetViruses(token);
                await GetPerson(token, id);
                Id = id;
                return Page();
            }
            catch (Exception ex)
            {
                TempData["err"] = ex.Message;
                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
        {

            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToPage("../Index");
            }
            try
            {

                if (!ModelState.IsValid)
                {
                    await GetViruses(token);
                }

                var response = await Common.SendRequestAsync($"{Common.BaseURL}/api/person/{Id}", HttpMethod.Put, Person, token);
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    TempData["err"] = "You have no authorization.";
                    return RedirectToPage("./Index");
                }
                if (!response.IsSuccessStatusCode)
                {
                    string err = await Common.ReadError(response);
                    TempData["err"] = err;
                    await GetViruses(token);
                    return Page();
                }
                var o = await Common.ReadT<SuccessResponse>(response);
                TempData["info"] = $"{o.Message}";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["err"] = ex.Message;
                await GetViruses(token);
                return Page();
            }
        }
    }
}
