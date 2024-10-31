using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.RequestModel;
using Repositories.ResponseModel;
using System.Text.Json;

namespace PE_PRN231_FA24_TrialTest_KieuQuangMinh_FE.Pages.PersonManagement
{
    class SuccessResponse
    {
        public int? PersonId { get; set; }
        public string Message { get; set; }
    }
    public class CreateModel : PageModel
    {
        [BindProperty]
        public PersonRequestModel Person { get; set; } = new PersonRequestModel
        {
            Viruses = new List<VirusRequestModel?> { new VirusRequestModel() }
        };

        [BindProperty]
        public List<string>? Viruses { get; set; } = [];
        private async Task GetViruses(string token)
        {
            var resposne = await Common.SendRequestAsync<HttpResponseMessage>($"{Common.BaseURL}/api/viruses", HttpMethod.Get, null, token);
            if (!resposne.IsSuccessStatusCode)
            {
                TempData["err"] = await Common.ReadError(resposne);
            }
            Viruses = await Common.ReadT<List<string>?>(resposne);
        }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                var token = HttpContext.Session.GetString("token");
                if (token == null)
                {
                    return RedirectToPage("../Index");
                }
                await GetViruses(token);
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

                var response = await Common.SendRequestAsync($"{Common.BaseURL}/api/person", HttpMethod.Post, Person, token);
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
                TempData["info"] = $"{o.PersonId} - {o.Message}";
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
