using FlatFinder.Contracts.Services;
using FlatFinder.Web.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FlatFinder.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [CustomExceptionFilter]
    [ValidateModel]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _offerService.Remove(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var offers = await _offerService.Get();
            var result = new JsonResult(offers);

            await _offerService.SetAsRead(offers.Select(x => x.OfferId).ToArray());

            return result;
        }

        [HttpGet("new-offer-count")]
        public async Task<IActionResult> GetNewOfferCount()
        {
            return Json(new { count = await _offerService.GetNewOfferCount() });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(int id)
        {
            return Json(await _offerService.Add(id));
        }
    }
}