using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;

namespace UserManagementSystem.Controllers
{
    public class BaseController : ControllerBase
    {
        //overrided the ok method with my own implementatiom
        public override OkObjectResult Ok([ActionResultObjectValue] object? value)
        {
            ServiceResponse serviceResponse = new ServiceResponse()
            {
                Data = value,
                Message= "ok",
                Status = Status.Success,
            };
            
            return new OkObjectResult(serviceResponse);
        }

        //overrided the BadRequest method with my own implementatiom
        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object? value)
        {
            ServiceResponse serviceResponse = new ServiceResponse()
            {
                Data = value,
                Message = "Bad request",
                Status = Status.BadRequest,
            };

            return new BadRequestObjectResult(serviceResponse);
        }



        //made a custom method which returns the BadRequestObjectResult objects ,it is for sending custom mesasge from the controller

        //NonAction is added here so that this method doesnt handle http requestss
        [NonAction]
        public BadRequestObjectResult BadRequest(object? value, string message)
        {
            ServiceResponse serviceResponse = new ServiceResponse()
            {
                Data = value,
                Message = message,
                Status = Status.BadRequest,
            };

            return new BadRequestObjectResult(serviceResponse);
        }


    }
}
