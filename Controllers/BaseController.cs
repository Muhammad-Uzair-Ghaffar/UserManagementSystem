using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using UserManagementSystem.Dtos.User;
using UserManagementSystem.Models;
using System.Reflection; 
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
                Message= new string[] { "ok"},
                Status = Status.Success,
            };
            
            return new OkObjectResult(serviceResponse);
        }

        //overrided the BadRequest method with my own implementatiom
        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object? value)
        {
            Type messageType = value.GetType();
            ServiceResponse serviceResponse = new ServiceResponse()
            {
                Data = null,
                Status = Status.Failed,
            };
            if (messageType == typeof(string)) 
            {
                serviceResponse.Message = new string[] { value.ToString() };
            }
            if (value is List<string> array)
            {
                serviceResponse.Message = array.ToArray();
            }

            return new BadRequestObjectResult(serviceResponse);
        }


        [NonAction]
        public OkObjectResult Ok(object? value, string message)
        {
            ServiceResponse serviceResponse = new ServiceResponse()
            {
                Data = value,
                Message = new[] { message },
                Status = Status.Success,
            };

            return new OkObjectResult(serviceResponse);
        }

        //made a custom method which returns the BadRequestObjectResult objects ,it is for sending custom mesasge from the controller

        //NonAction is added here so that this method doesnt handle http requestss
        [NonAction]
        public BadRequestObjectResult BadRequest(object? value, string message)
        {
            ServiceResponse serviceResponse = new ServiceResponse()
            {
                Data = value,
                Message = new[] { message },
                Status = Status.Failed,
            };

            return new BadRequestObjectResult(serviceResponse);
        }


    }
}
