using FlatFinder.Contracts;
using FlatFinder.Contracts.Services;
using FlatFinder.Model;
using FlatFinder.Web.ActionFilters;
using FlatFinder.Web.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlatFinder.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [CustomExceptionFilter]
    [ValidateModel]
    public class FlatController : Controller
    {
        private readonly IFlatService _flatService;
        private readonly IImageService _imageService;

        public FlatController(IFlatService flatService, IImageService imageService)
        {
            _flatService = flatService;
            _imageService = imageService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _flatService.Remove(id);
            return Ok();
        }

        [HttpPost("generate-report")]
        public async Task<IActionResult> GenerateReport([FromBody]GenerateReportRequest request)
        {
            return Json(await _flatService.GenerateReport(request.From, request.To));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await _flatService.Get());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Json(await _flatService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddOrUpdateFlatRequest request)
        {
            return Json(await _flatService.Add(ConvertToFlat(request)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]AddOrUpdateFlatRequest request)
        {
            await _flatService.Update(ConvertToFlat(request, id));
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody]FlatSearchQuery query)
        {
            return Json(await _flatService.Search(query));
        }

        [HttpPost("set-as-sold/{id}")]
        public async Task<IActionResult> SetAsSold(int id)
        {
            await _flatService.SetAsSold(id);
            return Ok();
        }

        private static Flat ConvertToFlat(AddOrUpdateFlatRequest request, int? id = null)
        {
            Flat flat = new Flat
            {
                Address = request.Address,
                Area = request.Area,
                City = request.City,
                Description = request.Description,
                Floor = request.Floor,
                HasBalcony = request.HasBalcony,
                Name = request.Name,
                NumberOfRooms = request.NumberOfRooms,
                PostCode = request.PostCode,
                Price = request.Price
            };

            if (id.HasValue)
                flat.Id = id.Value;

            return flat;
        }
    }
}