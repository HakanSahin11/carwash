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
    public class FinancialController : ControllerBase
    {
        private readonly FinancialCrud _financial;
        private readonly UserCrud _user;
        private readonly CarwashRunCrud _carwashRun;
        private readonly LoginCrud _login;

        public FinancialController(FinancialCrud financial, UserCrud user, CarwashRunCrud carwash, LoginCrud login)
        {
            _financial = financial;
            _user = user;
            _carwashRun = carwash;
            _login = login;
        }

        //Get list of all users
        [HttpGet]
        public ActionResult<List<EncFinancialModel>> Get() =>
                _financial.Get().Result;

        [HttpPost]
        public ActionResult FinancialSection([FromBody] JsonElement json)
        {

            try
            {
                var EncContent = JsonConvert.DeserializeObject<ApiModel>(json.GetRawText());
                if (EncContent.TokenId != "1666723Dx")
                    return Ok("Wrong Token");

                Crypt crypt = new Crypt();
                var DecContent = Task.Run(() => crypt.Decrypter(EncContent.Json, "13334448853")).Result;
                var content = JsonConvert.DeserializeObject<CarwashRunModel>(DecContent);
                var id = _user.GetUserByEmail(content.UserName).Result.Id;
                var financial = _financial.GetLoginById(id).Result;
                var salt = _login.GetUser(content.UserName).Result.Salt;
                var decSub = crypt.Decrypter(financial.Subscription, salt);

                switch (EncContent.Task)
                {
                    case "GetIncomingRequests":
                        //Checks if running already
                        if (_carwashRun.GetCarwashByUsername(content.UserName) == null)
                            return Ok(new ApiModel("", "1666723Dx", "Already Running"));

                        return Ok(new ApiModel(JsonConvert.SerializeObject(decSub.Result), "1666723Dx", "Success"));
                    case "UserBoughtCarwashTime":

                        //check avalible carwashes
                        var AvalibleCarwashes = _carwashRun.Get().Result.Where(x => x.Status == "Closed").ToList();
                        if (AvalibleCarwashes.Count == 0)
                            return Ok("All carwashes taken");

                        var carwashPrice = 25;
                        if (decSub.Result == "true")
                            carwashPrice = 0;
                        //adds new transaction then encrypts it
                        var updatedFinancial = financial;
                        var DecTrans = JsonConvert.DeserializeObject<List<Transactions>>(crypt.Decrypter(financial.Transactions, salt).Result);
                        if (DecTrans == null)
                            DecTrans = new List<Transactions>();

                        var newTransactionObj = new Transactions { Amount = carwashPrice, Subscription = Convert.ToBoolean(decSub.Result), Transaction_Datetime = DateTime.Now };
                        DecTrans.Add(newTransactionObj);
                        updatedFinancial.Transactions = crypt.Encrypter(JsonConvert.SerializeObject(DecTrans), salt);

                        var savedTotalAmount = crypt.Decrypter(updatedFinancial.TotalAmount, salt);
                        var UpdatedDecTotalAmount = Convert.ToDouble(savedTotalAmount.Result) + carwashPrice;
                        updatedFinancial.TotalAmount = crypt.Encrypter(UpdatedDecTotalAmount.ToString(), salt);

                        var carwash = AvalibleCarwashes[0];
                        carwash.Status = "Running";
                        carwash.StartTime = DateTime.Now.ToString();
                        carwash.UserName = content.UserName;
                        carwash.Numberplate = content.Numberplate;

                        Task.Run(() => _financial.Update(financial.Id, updatedFinancial));
                        Task.Run(() => _carwashRun.Update(carwash.Carwash, carwash));
                        Task.WaitAll();
                        break;
                }
                return Ok();

            }

            catch (Exception e)
            {
                throw new Exception($"Error Code 1.2 - Error at HTTPPOST - {e.Message}");
            }
        }


        /*
        [HttpPost]
        public ActionResult FinancialSection([FromBody] JsonElement json)
        {

            try
            {
                var EncContent = JsonConvert.DeserializeObject<ApiModel>(json.GetRawText());
                if (EncContent.TokenId != "1666723Dx")
                    return Ok("Wrong Token");

                Crypt crypt = new Crypt();
                var DecContent = Task.Run(() => crypt.Decrypter(EncContent.Json, "13334448853")).Result;
                var content = JsonConvert.DeserializeObject<UserModel>(DecContent);
                var id = _user.GetUser(content.UserName).Result.Id;
                var financial = _financial.GetLoginById(id).Result;
                var salt = _login.GetUserByUserName(content.UserName).Result.Salt;
                var decSub = crypt.Decrypter(financial.Subscription, salt);

                switch (EncContent.Task)
                {
                    case "GetIncomingRequests":
                        //Checks if running already
                        if (_carwashRun.GetCarwashByUsername(content.UserName) == null)
                            return Ok(new ApiModel("", "1666723Dx", "Already Running"));

                        return Ok(new ApiModel(JsonConvert.SerializeObject(decSub.Result), "1666723Dx", "Success"));
                    case "UserBoughtCarwashTime":

                        //check avalible carwashes
                        var AvalibleCarwashes = _carwashRun.Get().Result.Where(x => x.Status == "Closed").ToList();
                        if (AvalibleCarwashes.Count == 0)
                            return Ok("All carwashes taken");

                        var carwashPrice = 25;
                        if (decSub.Result == "true")
                            carwashPrice = 0;
                        //adds new transaction then encrypts it
                        var updatedFinancial = financial;
                        var DecTrans = JsonConvert.DeserializeObject<List<Transactions>>(crypt.Decrypter(financial.Transactions, salt).Result);
                        if (DecTrans == null)
                            DecTrans = new List<Transactions>();

                        var newTransactionObj = new Transactions { Amount = carwashPrice, Subscription = Convert.ToBoolean(decSub.Result), Transaction_Datetime = DateTime.Now };
                        DecTrans.Add(newTransactionObj);
                        updatedFinancial.Transactions = crypt.Encrypter(JsonConvert.SerializeObject(DecTrans), salt);

                        var savedTotalAmount = crypt.Decrypter(updatedFinancial.TotalAmount, salt);
                        var UpdatedDecTotalAmount = Convert.ToDouble(savedTotalAmount.Result) + carwashPrice;
                        updatedFinancial.TotalAmount = crypt.Encrypter(UpdatedDecTotalAmount.ToString(), salt);

                        var carwash = AvalibleCarwashes[0];
                        carwash.Status = "Running";
                        carwash.StartTime = DateTime.Now.ToString();
                        carwash.UserName = content.UserName;

                        Task.Run(() => _financial.Update(financial.Id, updatedFinancial));
                        Task.Run(() => _carwashRun.Update(carwash.Carwash, carwash));
                        Task.WaitAll();
                        break;
                }
                return Ok();

            }

            catch (Exception e)
            {
                throw new Exception($"Error Code 1.2 - Error at HTTPPOST - {e.Message}");
            }
        }
        */

    }
}