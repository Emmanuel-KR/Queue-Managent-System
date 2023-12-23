using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueSystem.Models;
using QueueSystem.Services;
using System.Reflection.Metadata.Ecma335;

namespace QueueSystem.Controllers
{
    public class QueueController : Controller
    {
        private readonly IQueueRepository _queueRepository;
        public QueueController(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicePointM>>> CheckinPage()
        {
            var services = await _queueRepository.GetServices();
            return View(services);
        }



        [HttpGet]
        public async Task<IActionResult> WaitingPage()
        {
            var calledCustomers = await _queueRepository.GetCalledCustomers();
            return View(calledCustomers);
        }



        [Authorize, HttpGet]
        public async Task<IActionResult> ServicePoint()
        {

            foreach (var claim in User.Claims)
            {
                var userServingPointId = @claim.Value;

                var waitingCustomers = await _queueRepository.GetWaitingCustomers(userServingPointId);

                var currentServingCustomerId = await _queueRepository.MyCurrentServingCustomer(userServingPointId);
                return View(new QueueM());
               
            }
            return NotFound();

        }
       


    }
}
