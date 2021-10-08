using com.microsoft.dx.officewopi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using com.microsoft.dx.officewopi.Utils;
using System.Web;

namespace Application.Api.App.modules.wopi
{
    public class filesController : ApiController
    {
        [WopiTokenValidationFilter]
        [HttpGet]
        [Route("wopi/files/{id}")]
        public async Task<HttpResponseMessage> Get(string id)
        {
            //Handles CheckFileInfo
            return await HttpContext.Current.ProcessWopiRequest();
        }

        [WopiTokenValidationFilter]
        [HttpGet]
        [Route("wopi/files/{id}/contents")]
        public async Task<HttpResponseMessage> Contents(string id)
        {
            //Handles GetFile
            return await HttpContext.Current.ProcessWopiRequest();
        }

        [WopiTokenValidationFilter]
        [HttpPost]
        [Route("wopi/files/{id}")]
        public async Task<HttpResponseMessage> Post(string id)
        {
            //Handles Lock, GetLock, RefreshLock, Unlock, UnlockAndRelock, PutRelativeFile, RenameFile, PutUserInfo
            return await HttpContext.Current.ProcessWopiRequest();
        }

        [WopiTokenValidationFilter]
        [HttpPost]
        [Route("wopi/files/{id}/contents")]
        public async Task<HttpResponseMessage> PostContents(string id)
        {
            //Handles PutFile
            return await HttpContext.Current.ProcessWopiRequest();
        }
}
}
