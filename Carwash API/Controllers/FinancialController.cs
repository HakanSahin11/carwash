using Carwash_API.CRUD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Carwash_API.Models.UserModels;

namespace Carwash_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialController : ControllerBase
    {
        private readonly FinancialCrud _financial;
        public FinancialController(FinancialCrud financial)
        {
            _financial = financial;
        }

        //Get list of all users
        [HttpGet]
        public ActionResult<List<EncFinancialModel>> Get() =>
                _financial.Get().Result;
    }
}
