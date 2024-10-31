using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.RequestModel;

namespace PE_PRN231_FA24_TrialTest_KieuQuangMinh_FE.Pages.PersonManagement
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public PersonRequestModel Person { get; set; } = new PersonRequestModel
        {
            Viruses = new List<VirusRequestModel?> { new VirusRequestModel() }
        };

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
                var response = await Common.SendRequestAsync<HttpResponseMessage>($"{Common.BaseURL}/api/person/{Id}", HttpMethod.Delete, null, token);

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    TempData["err"] = "You have no authorization.";
                    return RedirectToPage("./Index");
                }
                var o = await Common.ReadT<SuccessResponse>(response);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["err"] = o.Message;
                    await GetPerson(token, Person.PersonID);
                    return Page();
                }

                TempData["info"] = o.Message;
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                TempData["err"] = ex.Message;
                return Page();
                throw;
            }
        }
    }
}
