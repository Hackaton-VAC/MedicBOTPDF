using HackatonPDF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HackatonPDF.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrescriptionemailController : ControllerBase
    {
        public static Dictionary<string, string> lastPDFbyUser = new Dictionary<string, string>();

        private readonly ILogger<PrescriptionemailController> _logger;

        public PrescriptionemailController(ILogger<PrescriptionemailController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public object Post([FromBody] Prescription prescription)
        {
            string incomingJson = JsonConvert.SerializeObject(prescription);
            RestClient client = new RestClient("https://app.useanvil.com");
            string api_key = "oYuKff4kGZicoKvqXQFBCBC4bIHpcugO:";
            var request = new RestRequest("api/v1/fill/0KujO3jQFQhLeNi8jnGx.pdf", Method.POST);
            request.AddParameter("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(api_key)), ParameterType.HttpHeader);
            request.AddParameter("application/json", incomingJson, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute<object>(request);
            string pdf_name = "Data/prescription_" + prescription.data.Name.Replace(" ", "_") + "_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + ".pdf";
            System.IO.File.WriteAllBytes(pdf_name, response.RawBytes);
            //sendEmail(prescription, pdf_name, "");
            FileContentResult file = File(response.RawBytes, response.ContentType, pdf_name);
            return file;
        }

        public bool sendEmail(Prescription prescription, string pdf_name, string recipient)
        {
            var fromAddress = new MailAddress("arturoantoniotr@gmail.com", "Medic Bot");
            var toAddress = new MailAddress("arturoantoniotr@gmail.com", "Paciente");
            const string fromPassword = "Aatr12..";
            const string subject = "Prescription Copy";
            const string body = "Aqui ta tu recibo";

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
            message.Subject = subject;
            message.Body = body;
            message.Attachments.Add(new Attachment(pdf_name));
            smtp.Send(message);

            return true;
        }
    }
}
