using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.ResponseModel;
using System.Text.Json;

namespace PE_PRN231_FA24_TrialTest_KieuQuangMinh_FE.Pages.PersonManagement
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<PersonResponseModel> Models { get; set; } = new List<PersonResponseModel> 
        { 
            new PersonResponseModel 
            { 
                Viruses = new List<VirusResponseModel> 
                { 
                    new VirusResponseModel() 
                } 
            } 
        };

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var token = HttpContext.Session.GetString("token");
                if (token == null)
                {
                    return RedirectToPage("../Index");
                }
                var response = await Common.SendRequestAsync<List<PersonResponseModel>>(
                    $"{Common.BaseURL}/api/persons",
                    HttpMethod.Get,
                    null,
                    token);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["err"] = await Common.ReadError(response);
                    return RedirectToPage("../Index");
                }
                var o = await Common.ReadT<List<PersonResponseModel>>(response);
                if (o == null)
                {
                    TempData["err"] = "Something was wrong";
                    return RedirectToPage("./Index");
                }
                Models = o;
                return Page();
            }
            catch (Exception ex)
            {
                TempData["err"] = ex.Message;
                return RedirectToPage("./Index");
            }
        }
    }
}
