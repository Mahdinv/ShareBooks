using Kavenegar;
using Microsoft.AspNetCore.Mvc.Filters;
using ShareBooks.Core.Services.Interfaces;
using ShareBooks.DataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareBooks.Core.Senders
{
    public class MessageSender
    {
        private IUserService _userService;

        AuthorizationFilterContext context;

        public void SMS(string to, string body)
        {
            _userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));

            Setting setting = _userService.GetSetting();

            var sender = setting.SmsSender; /*shomare samane baraye ersal payamak*/
            var receptor = to;
            var message = body;
            var api = new KavenegarApi(setting.SmsApi); /*api ro az khod syte kavenegar migiram*/

            api.Send(sender, receptor, message);
        }/*chon chizio nemikhaim bargardoonim az void estefade mikonim*/
    }
}
