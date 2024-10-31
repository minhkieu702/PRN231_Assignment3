using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.RequestModel;
using Repositories.ResponseModel;
using System.Text;
using System.Text.Json;

namespace PE_PRN231_FA24_TrialTest_KieuQuangMinh_FE.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ViroCureUserRequestModel Model { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await Common.SendRequestAsync($"{Common.BaseURL}/api/login", HttpMethod.Post, Model);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var token = JsonDocument.Parse(responseBody).RootElement.GetProperty("token").GetString();
                HttpContext.Session.SetString("token", token);
                return RedirectToPage("./PersonManagement/Index");
            }
            TempData["err"] = "Invalid login attempt.";
            //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
