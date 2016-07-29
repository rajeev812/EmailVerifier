using FirebaseAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirebaseAngular.Controllers
{
    public class EmailVerifierController : ApiController
    {
        
        // POST: api/EmailVerifier
        public MXServer PostEmail(MXServer Server)
        {
            MXServer newObj = new MXServer();
            if (Server.Email != null)
            {
                using (EmailVerifier obj = new EmailVerifier())
                {
                    // bool result = verifier.CheckExists(EmailText.Text);
                    newObj = obj.IsEmailVerified(Server.Email);

                }
            }

            return newObj;
        }

    
       
    }
}
