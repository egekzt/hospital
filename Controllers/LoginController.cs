using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using hospital.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace hospital.Controllers
{
    public class LoginController : Controller
    {

        private readonly IhospitalContext _hospitalContext = new hospitalContext();
        

        public LoginController()
        {
            
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string email, string password)
        {
            // Attempt to sign in the user using ASP.NET Core Identity
            

            
                // Check if the user is a Secretary
                var secretary = _hospitalContext.Secretary.FirstOrDefault(s => s.email == email && s.password == password);
                if (secretary != null)
                {
                // Assign Secretary policy
                // Redirect to Secretary area
                HttpContext.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "SecretaryPolicy") }));
                return RedirectToAction("Index", "Appointment", new { area = "Secretary" });
                }

                // Check if the user is a Doctor
                var doctor = _hospitalContext.Doctor.FirstOrDefault(d => d.email == email && d.password == password);
                if (doctor != null)
                {
                // Assign Doctor policy
                // Redirect to Doctor area
                HttpContext.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "DoctorPolicy") }));

                return RedirectToAction("Index", "Appointment", new { area = "Doctor" });
                }


                // Check if the user is a Patient
                var patient = _hospitalContext.Patient.FirstOrDefault(p => p.email == email && p.password == password);
                if (patient != null)
                {
                // Assign Patient policy
                // Redirect to Patient area
                HttpContext.User.AddIdentity(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "PatientPolicy") }));

                return RedirectToAction("Index", "Appointment", new { area = "Patient" });
                }

                // User not found in any table
                ModelState.AddModelError("", "Invalid user.");
                
           

            return View("Login");
        }
    }
}
