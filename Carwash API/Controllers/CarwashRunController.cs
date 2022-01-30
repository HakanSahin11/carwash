using Carwash_API.CRUD;
using Carwash_API.Help_Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Carwash_API.Models.UserModels;

namespace Carwash_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarwashRunController : ControllerBase
    {

        private readonly CarwashRunCrud _CarwashRun;
        

        public CarwashRunController(CarwashRunCrud CarwashRun)
        {
            _CarwashRun = CarwashRun;
        }

        [HttpGet]
        public ActionResult<List<CarwashRunModel>> Get() =>
                _CarwashRun.Get().Result;

        [HttpPost]
        public ActionResult Carwash([FromBody] JsonElement json)
        {

            try
            {
                var EncContent = JsonConvert.DeserializeObject<ApiModel>(json.GetRawText());
                if (EncContent.TokenId != "1666723Dx")
                    return Ok("Wrong Token");

                Crypt crypt = new Crypt();
                var DecContent = Task.Run(() => crypt.Decrypter(EncContent.Json, "13334448853")).Result;
                var content = JsonConvert.DeserializeObject<CarwashRunModel>(DecContent);
                var savedCarwash = _CarwashRun.GetCarwashByNumber(content.Carwash).Result;
                var editedContent = new CarwashRunModel();

                switch (EncContent.Task)
                { 
                    case "Change":
                        //get db info
                        if (content.Status == savedCarwash.Status)
                            return Ok($"Error - Already {content.Status}");
                        else
                        {
                            editedContent = new CarwashRunModel
                            {
                                UserName = content.UserName,
                                Status = content.Status,
                                Numberplate = content.Numberplate,
                                StartTime = content.StartTime,
                                Carwash = savedCarwash.Carwash,
                                Id = savedCarwash.Id
                            };
                            _CarwashRun.Update(content.Carwash, editedContent).Wait();
                        }
                            break;
                    case "Create":
                        _CarwashRun.Create(content);
                        break;
                }
                return Ok(new ApiModel(content.Status, "1666723Dx", "Success"));  
            }
            catch (Exception e)
            {
                throw new Exception($"Error Code 1.2 - Error at HTTPPOST - {e.Message}");
            }
        }
    }
}
