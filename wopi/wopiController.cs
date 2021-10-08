using Application.Api.Controllers.BlockChain;
using Application.Api.DMS.Block;
using Application.Api.DMS.ViewModel;
using Application.Dal.DataModel;
using com.microsoft.dx.officewopi.Models;
using com.microsoft.dx.officewopi.Models.Wopi;
using com.microsoft.dx.officewopi.Security;
using com.microsoft.dx.officewopi.Utils;
using DMS.API.Controllers.Block;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Application.Api.App.modules.wopi
{
    public class wopiController : ApiController
    {

        [HttpGet]
        [Route("Home/Detail/{id}")]
        public async Task<IHttpActionResult> Detail(Guid id)
        {
            var Request = HttpContext.Current.Request;

            if (String.IsNullOrEmpty(Request["action"]))
                return Ok("No action provided");

            var fileData = WopiExtensions.Getfiledeatils(id.ToString());
            var file = fileData.DetailedFileModel;
            // Check for null file
            if (file == null)
                return Ok("Files does not exist");

            // Use discovery to determine endpoint to leverage
            List<WopiAction> discoData = await WopiUtil.GetDiscoveryInfo();
            var fileExt = file.BaseFileName.Substring(file.BaseFileName.LastIndexOf('.') + 1).ToLower();
            var action = discoData.FirstOrDefault(i => i.name == Request["action"] && i.ext == fileExt);

            // Make sure the action isn't null
            if (action != null)
            {
                string urlsrc = WopiUtil.GetActionUrl(action, file, Request.Url.Authority);

                // Generate JWT token for the user/document
				//var token  
                returnModel retdata = new returnModel()
                {
                    access_token = token, //new access token 
                    access_token_ttl = token.ValidTo.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
                    wopi_urlsrc = urlsrc
                };
                return Ok(retdata);
            }
            else
            {
                // This will only hit if the extension isn't supported by WOPI
                return Ok("File is not a supported WOPI extension");
            }
        }



              private string getUserContainer(string uname)
        {
            return uname.Replace("@", "-").Replace(".", "-");
        }

        public class details
        {
            public string id { get; set; }
            public string action { get; set; }
        }


        public class returnModel
        {
            public string access_token { get; set; }
            public double access_token_ttl { get; set; }
            public string wopi_urlsrc { get; set; }
        }

    }
}
