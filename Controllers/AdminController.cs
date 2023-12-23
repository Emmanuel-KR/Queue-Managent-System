using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueSystem.Models;
using QueueSystem.Services;

namespace QueueSystem.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/ServiceProviders
        public async Task<ActionResult<IEnumerable<ServiceProviderM>>> ViewServiceProviders()
        {
            var serviceProviders = await _adminRepository.GetServiceProviders();
            return View(serviceProviders);
        }

        // GET: Admin/ViewServiceProviderDetails/5
        public async Task<ActionResult<ServiceProviderM>> ViewServiceProviderDetails(int id)
        {
            var ServiceProviderDetails = await _adminRepository.GetServiceProviderDetails(id);
            if (ServiceProviderDetails != null)
                return View(ServiceProviderDetails);
            else
                return NotFound();
        }

        // GET: Admin/CreateServiceProvider
        public async Task<ActionResult> CreateServiceProvider()
        {
            return View();
        }

        // POST: Admin/CreateServiceProvider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateServiceProvider(ServiceProviderM serviceProvider)
        {
            await _adminRepository.CreateServiceProvider(serviceProvider);
            return RedirectToAction(nameof(ViewServiceProviders));
        }

        // GET: Admin/EditServiceProvider/5
        public async Task<ActionResult> EditServiceProvider(int id)
        {
            var serviceProvider = await _adminRepository.GetServiceProviderDetails(id);
            if (serviceProvider != null)
                return View(serviceProvider);
            else
                return NotFound();
        }

        // POST: Admin/EditServiceProvider/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditServiceProvider(int id, ServiceProviderM serviceProvider)
        {
            await _adminRepository.UpdateServiceProvider(id, serviceProvider);
            return RedirectToAction(nameof(ViewServiceProviderDetails), new { id = serviceProvider.Id });
        }


        // POST: Admin/DeleteServiceProvider/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteServiceProvider(int id)
        {
            await _adminRepository.DeleteServiceProvider(id);
            return RedirectToAction(nameof(ViewServiceProviders));
        }

        // GET: Admin/ViewServicePoints
        public async Task<ActionResult<IEnumerable<ServicePointM>>> ViewServicePoints()
        {
            var servicePoints = await _adminRepository.GetServicePoints();
            return View(servicePoints);
        }

        // GET: Admin/ViewServicePointDetails/5
        public async Task<ActionResult<ServiceProviderM>> ViewServicePointDetails(int id)
        {
            var ServiceProviderDetails = await _adminRepository.GetServicePointDetails(id);
            if (ServiceProviderDetails != null)
                return View(ServiceProviderDetails);
            else
                return NotFound();
        }

        // GET: Admin/CreateServicePoint
        public async Task<ActionResult> CreateServicePoint()
        {
            return View();
        }

        // POST: Admin/CreateServicePoint
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateServicePoint(ServicePointM servicePoint)
        {
            await _adminRepository.CreateServicePoint(servicePoint);
            return RedirectToAction(nameof(ViewServicePoints));
        }

        // GET: Admin/EditServicePoint/5
        public async Task<ActionResult> EditServicePoint(int id)
        {
            var servicePoint = await _adminRepository.GetServicePointDetails(id);
            if (servicePoint != null)
                return View(servicePoint);
            else
                return NotFound();
        }

        // POST: Admin/EditServicePoint/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditServicePoint(int id, ServicePointM servicePoint)
        {
            await _adminRepository.UpdateServicePoint(id, servicePoint);
            return RedirectToAction(nameof(ViewServicePointDetails), new { id = servicePoint.Id });
        }

        // POST: Admin/DeleteServicePoint/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteServicePoint(int id)
        {
            await _adminRepository.DeleteServicePoint(id);
            return RedirectToAction(nameof(ViewServicePoints));
        }
        public IActionResult Reports()
        {
            return View();
        }

    }
}
