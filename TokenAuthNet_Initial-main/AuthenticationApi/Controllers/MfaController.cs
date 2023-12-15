using AuthenticationApi.Helpers;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OtpNet;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using TwoFactor;
using Base32Encoding = Google.Authenticator.Base32Encoding;
using QRCoder;

namespace AuthenticationApi.Controllers
{

   
    [Route("api/twofactor")]
    public class TwoFactorController : ControllerBase
    {


      
        private readonly AppSettings _AppSettings;

        public TwoFactorController(
          
            IOptions<AppSettings> appSettings)
        {
        
            _AppSettings = appSettings.Value;
        }

        static string PrivateKey = "";

        [HttpGet("setup")]
        public ActionResult GoogleAuthSetup(string company, string email)
        {
            // var user = User.Identity.Name; // Retrieve the current user's identity
            var secret = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secret);
            PrivateKey = base32Secret; 
            //var secretkey = _AppSettings.Secret;
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            SetupCode setupInfo = tfa.GenerateSetupCode(company, email, base32Secret, false, 3);

            string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
            string manualEntrySetupCode = setupInfo.ManualEntryKey;

            //var qrCode = new PngByteQRCode(qrCodeImageUrl);
            //var qrCodeImage = qrCode.GetGraphic(20);

            //// Return the QR code image to the user
            //return File(qrCodeImage, "image/png");

            return Ok(setupInfo);
        }



        [HttpPost("verify")]
        public ActionResult VerifyGoogleAuth(string ClientCode, string clientsecretkey)
        {
            
            // var user = User.Identity.Name;
            var secretkey = clientsecretkey;
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            bool isValid = tfa.ValidateTwoFactorPIN(secretkey, ClientCode);
            var key = tfa.GeneratePINAtInterval(secretkey, 10, 6);
            string currentKey = tfa.GetCurrentPIN(secretkey);

            if (currentKey == ClientCode && isValid)
            {
                    // Two-factor authentication succeeded; you can proceed with the protected action.
                 return Ok("Two-factor authentication succeeded");
                
            }
            else
            {
                // Two-factor authentication failed; return an error or redirect to the setup page.
                return BadRequest("Two-factor authentication failed");
            }
        }
        // ...
    }
}
