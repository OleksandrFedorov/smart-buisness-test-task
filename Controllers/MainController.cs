using Microsoft.AspNetCore.Mvc;
using TestTask.DAL.Repositories;
using TestTask.DTO;
using TestTask.DTO.Exceptions;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api")]
    public class MainController : ControllerBase
    {
        private ContractRepository ContractRepo => HttpContext.RequestServices.GetService<ContractRepository>();

        /// <summary>
        /// Returns list of all Contracts.
        /// </summary>
        /// <response code="200">Contracts successfully retrieved.</response>
        /// <response code="401">Request denied due to API key was not provided or invalid.</response>
        /// <response code="500">Contracts not retrieved due to internal service error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ContractDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllContractsAsync()
        {
            var contracts = await ContractRepo.GetAllAsync();
            return Ok(contracts);
        }

        /// <summary>
        /// Add or update contract entity. 
        /// Entities without an Id will be added.
        /// </summary>
        /// <response code="200">Contract successfully added.</response>
        /// <response code="400">Request mosel is invalid.</response>
        /// <response code="401">Request denied due to API key was not provided or invalid.</response>
        /// <response code="404">Entity was not found.</response>
        /// <response code="500">Contract not added due to internal service error.</response>
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateContractAsync(ContractDto dto)
        {
            try
            {
                var result = await ContractRepo.AddOrUpdateAsync(dto);
                return Ok(result);
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (SaveDataException e)
            {
                return Problem(title: e.Message);
            }
        }
    }
}