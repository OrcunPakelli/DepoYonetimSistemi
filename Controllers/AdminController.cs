using Microsoft.AspNetCore.Mvc;
using DepoYonetimSistemi.Models;

namespace DepoYonetimSistemi.Controllers
{
    public class AdminController: Controller
    {   
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");//kullanıcının sessiondaki rolu role değişkenine atanıyor

            //Kullanıcı rolü kontrol ediliyor
            if (role != UserRole.SystemAdmin.ToString())//Kullanıcı systemadmin rolüne sahip değilse 
            {
                return RedirectToAction("Index", "Home");//anasayfaya geri döndürülüyor
            }
            return View();
        }
    }
}