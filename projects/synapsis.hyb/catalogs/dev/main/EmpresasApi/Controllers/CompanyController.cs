using CompaniesApi.Models;
using CompaniesApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CompaniesApi.Controllers
{
    //MODEL BINDING
    //[FromBody]
    //[FromRoute]
    //[FromHeader]
    //[FromQuery] ?nombre=Felipe&apellido=Gavilan

    //Agregamos el decorador que le dice al controlador que se va a comportar como una API
    [ApiController]
    [Route("api/[controller]")] //Controlar a que url dirigirse //Placeholder [controller] sirve para sustituir por el prefijo de nuestro contorlador
    public class CompanyController : ControllerBase //Podemos heredar de ControllerBase y Controller pero la difenrencia es que ContorllerBase es para apis puras, y controller es para soluciones que ademas devuelven vistas, por lo tanto el primero es mas ligero
    {
        //Configuramos nuestro servicio
        //Es decir, la conexion a mongodb
        public CompanyService _companyService;

        //Configuramos el constructor de la clase
        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        //CREACIÓN DE LOS ENDPOINTS

        //Obtener todas las empresas disponibles
        [HttpGet("companies/all")]
        public async Task<ActionResult<List<Company>>> GetAll() //<ActionResult<List<Company>>> Retorna un tipo especifico de dato ActionResult<T>
        {
            var companies = await _companyService.GetAllAsync();

            return Ok(companies);
        }

        //Obtener una empresa por ID
        [HttpGet("company/{id}")]
        public async Task<ActionResult<Company>> GetById(string id)
        {
            if(id == null)
            {
                return BadRequest(new { error = $"{id} is required" });
            }

            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest(new { message = $"{id} proporcionado no es válido." });
            }

            var company = await _companyService.GetByIdAsync(id);

            if(company == null)
            {
                //return NotFound(new {Message = "Not found register"});

                return NotFound(new ApiResponse<Company>
                {
                    Success = false,
                    Message = "Compañía no encontrada",
                    Data = null
                });
            }
            return Ok(company);
        }

        //Crear nueva empresa
        [HttpPost("company/create")]
        public async Task<ActionResult> Create([FromBody] Company company)
        {
            await _companyService.CreateAsync(company);

            return Ok();
        }

        //Actualizar una empresa
        [HttpPut("company/update/{id}")]
        public async Task<ActionResult>Update(string id, [FromBody] Company company)
        {
            var companyId = await _companyService.GetByIdAsync(id);

            if(companyId == null)
            {
                return NotFound(new { Message = "Not found register" });
            }

            company.Id = id;

            await _companyService.UpdateAsync(id, company);

            return NoContent();
        }

        //Desactivar una empresa
        [HttpPut("company/deactive/{id}")]
        public async Task<ActionResult>DeactiveCompany(string id)
        {
            await _companyService.DeactiveCompany(id);

            return NoContent();
        }

        //Activar una empresa
        [HttpPut("company/active/{id}")]
        public async Task<ActionResult>ActiveCompany(string id)
        {
            await _companyService.ActiveCompany(id);

            return NoContent();
        }


    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
