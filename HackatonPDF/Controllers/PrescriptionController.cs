using HackatonPDF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HackatonPDF.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrescriptionController : ControllerBase
    {

        private readonly ILogger<PrescriptionController> _logger;

        public PrescriptionController(ILogger<PrescriptionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public object Post([FromBody] Prescription prescription)
        {
            /*string incomingJson = " {                                                                                                             " +
                "     \"title\": \"Prescription 2\",                                                                            " +
                "   \"fontSize\": 10,                                                                                           " +
                "   \"textColor\": \"#333333\",                                                                                 " +
                "   \"data\": {                                                                                                 " +
                "         \"Name\": \"Name\",                                                                                   " +
                "     \"Age\": \"Age\",                                                                                         " +
                "     \"RecipientName\": \"RecipientName\",                                                                         " +
                "     \"Allowance\": \"Allowance\",                                                                             " +
                "     \"DayCount\": 12345,                                                                                      " +
                "     \"Gender\": \"Gender\",                                                                                   " +
                "     \"Date\": \"2021-02-16\",                                                                                 " +
                "     \"Prescription\": \"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.\",    " +
                "     \"DocSign\": \"https://www.terragalleria.com/images-misc/signature_philip_hyde_small.jpg\", " +
                "     \"Clearance\": \"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.\",       " +
                "     \"DocAddress\": \"DocAddress\",                                                                           " +
                "     \"DocPhone\": \"DocPhone\",                                                                               " +
                "     \"DocEmail\": \"testy@example.com\",                                                                      " +
                "     \"DocName\": \"DocName\",                                                                                 " +
                "     \"DocTitle\": \"DocTitle\"                                                                                " +
                "   }                                                                                                           " +
            " }                                                                                                             ";*/
            //Prescription prescription = JsonConvert.DeserializeObject<Prescription>(incomingJson); // Create Prescription Object
            
            string incomingJson = JsonConvert.SerializeObject(prescription);
            RestClient client = new RestClient("https://app.useanvil.com");
            string api_key = "oYuKff4kGZicoKvqXQFBCBC4bIHpcugO:";
            var request = new RestRequest("api/v1/fill/0KujO3jQFQhLeNi8jnGx.pdf", Method.POST);
            request.AddParameter("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(api_key)), ParameterType.HttpHeader);
            request.AddParameter("application/json", incomingJson, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute<object>(request);
            string pdf_name = "Data/prescription_" + prescription.data.Name.Replace(" ", "_") + "_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + ".pdf";
            //System.IO.File.WriteAllBytes(pdf_name, response.RawBytes);
            //sendEmail(prescription, response.RawBytes);

            /*Send docs email*/
            if (prescription.data.SendToDoc)
            {
                var fromAddress = new MailAddress("medicbot.hackaton@gmail.com", "Medic Bot");
                var toAddress = new MailAddress(prescription.data.DocEmail, prescription.data.DocName);
                const string fromPassword = "h4ck4th0n2021";
                const string subject = "Prescription Copy";
                const string body = "Hackathon sin H";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                var message = new MailMessage(fromAddress, toAddress);

                if (!String.IsNullOrEmpty(prescription.data.PatientEmail))
                {
                    message.To.Add(prescription.data.PatientEmail);
                }
                message.Subject = subject;
                message.Body = body;
                Attachment attach;
                var ms = new MemoryStream(response.RawBytes);
                attach = new Attachment(ms, $@"prescription_{prescription.data.PatientName}.pdf");
                message.Attachments.Add(attach);
                smtp.Send(message);
                if (!String.IsNullOrEmpty(prescription.data.PatientEmail))
                {
                    message.To.Add(prescription.data.PatientEmail);
                }
                return "Ok";
            }

            FileContentResult file = File(response.RawBytes, response.ContentType, pdf_name);
            return file;
        }

        
    }

} 