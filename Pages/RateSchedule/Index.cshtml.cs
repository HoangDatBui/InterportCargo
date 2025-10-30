using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Interfaces;
using RateScheduleEntity = InterportCargo.BusinessLogic.Entities.RateSchedule;

namespace InterportCargo.Pages.RateSchedule
{
    public class IndexModel : PageModel
    {
        private readonly IRateScheduleRepository _rateScheduleRepository;

        public IndexModel(IRateScheduleRepository rateScheduleRepository)
        {
            _rateScheduleRepository = rateScheduleRepository;
        }

        public List<RateScheduleEntity> RateSchedules { get; set; } = new();

        public IActionResult OnGet()
        {
            // Check if user is authenticated
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                return RedirectToPage("/Account/Login");
            }

            // Allow both employees and customers to access the rate schedule

            RateSchedules = _rateScheduleRepository.GetActiveRates()
                .OrderBy(r => r.Id)
                .ToList();

            return Page();
        }
    }
}
