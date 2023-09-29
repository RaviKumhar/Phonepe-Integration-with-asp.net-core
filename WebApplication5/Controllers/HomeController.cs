using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //int amount = Convert.ToInt32(1) * 100;
            //var event_payload = new Dictionary<string, object>
            //{
            //    { "merchantId", "MERCHANTUAT" },
            //    { "merchantTransactionId", Guid.NewGuid().ToString() },
            //    { "merchantUserId", "MUID12fdsf3" },
            //    { "amount", amount },
            //    { "redirectUrl", "https://findsolution.info/response.php" },
            //    { "redirectMode", "POST" },
            //    { "callbackUrl", "https://findsolution.info/response.php" },
            //    { "mobileNumber", "9999999999" },
            //    { "paymentInstrument", new Dictionary<string, string> { { "type", "PAY_PAGE" } } }
            //};
            //var encoded_payload = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\r\n  \"merchantId\": \"MERCHANTUAT\",\r\n  \"merchantTransactionId\": \"MT7850590068188104\",\r\n  \"merchantUserId\": \"MUID123\",\r\n  \"amount\": 10000,\r\n  \"redirectUrl\": \"https://webhook.site/redirect-url\",\r\n  \"redirectMode\": \"REDIRECT\",\r\n  \"callbackUrl\": \"https://webhook.site/callback-url\",\r\n  \"mobileNumber\": \"9999999999\",\r\n  \"paymentInstrument\": {\r\n    \"type\": \"PAY_PAGE\"\r\n  }\r\n}"));
            //var saltKey = "099eb0cd-02cf-4e2a-8aca-3e6c6aff0399";
            //var saltIndex = 1;
            //var encode = event_payload;
            //var string1 = encoded_payload + "/pg/v1/pay" + saltKey;
            //var sha256 = sha256_hash(string1);
            //var sha2561 = BitConverter.ToString(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(string1)));
            //var finalXHeader = sha256 + "###" + saltIndex;
            //var header = new Dictionary<string, string>
            //{
            //    { "Content-Type", "application/json" },
            //    { "X-VERIFY", finalXHeader }
            //};
            //var headers = new List<string>
            //{
            //    "Content-Type: application/json",
            //    "X-VERIFY:" + finalXHeader
            //};
            //var phone_pay_url = "https://api-preprod.phonepe.com/apis/pg-sandbox/pg/v1/pay";
            //using (var client = new HttpClient())
            //{
            //    var content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string> { { "request", encoded_payload } }), Encoding.UTF8, "application/json");
            //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //   // client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            //    client.DefaultRequestHeaders.Add("X-VERIFY", finalXHeader);
            //    var response = client.PostAsync(phone_pay_url, content).Result;
            //    var result = response.Content.ReadAsStringAsync().Result;
            //    Console.WriteLine(result);
            //}

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public async Task<ActionResult> PhonePeAsync()
        {
            var data = new Dictionary<string, object>
            {
                { "merchantId", "MERCHANTUAT" },
                { "merchantTransactionId", Guid.NewGuid().ToString() },
                { "merchantUserId", "MUID12323232" },
                { "amount", 10000 },
                { "redirectUrl", Url.Action("Response") },
                { "redirectMode", "POST" },
                { "callbackUrl",  Url.Action("Response")  },
                { "mobileNumber", "9999999999" },
                { "paymentInstrument", new Dictionary<string, string> { { "type", "PAY_PAGE" } } }
            };
            var encode = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\r\n  \"merchantId\": \"MERCHANTUAT\",\r\n  \"merchantTransactionId\": \"MT7850590068188104\",\r\n  \"merchantUserId\": \"MUID123\",\r\n  \"amount\": 10000,\r\n  \"redirectUrl\": \"https://webhook.site/redirect-url\",\r\n  \"redirectMode\": \"REDIRECT\",\r\n  \"callbackUrl\": \"https://webhook.site/callback-url\",\r\n  \"mobileNumber\": \"9999999999\",\r\n  \"paymentInstrument\": {\r\n    \"type\": \"PAY_PAGE\"\r\n  }\r\n}"));
            var saltKey = "099eb0cd-02cf-4e2a-8aca-3e6c6aff0399";
            var saltIndex = 1;
            var stringToHash = encode + "/pg/v1/pay" + saltKey;
            var sha256 = sha256_hash(stringToHash);
            //var sha256 = BitConverter.ToString(new System.Security.Cryptography.SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(stringToHash))).Replace("-", "");
            var finalXHeader = sha256 + "###" + saltIndex;

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
              //  client.DefaultRequestHeaders.TryAddWithoutValidation("X-VERIFY", finalXHeader);
                //              client.DefaultRequestHeaders
                //.Accept
                //.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-VERIFY", finalXHeader);
                //ACCEPT header

                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "relativeAddress");
                //request.Content = new StringContent("{\"name\":\"John Doe\",\"age\":33}",
                //                                    Encoding.UTF8,
                //                                    "application/json");//CONTENT-TYPE header
                var requestData = new Dictionary<string, string> { { "request", encode } };
                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api-preprod.phonepe.com/apis/merchant-simulator/pg/v1/pay", content);
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var rData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                return Redirect(rData.data.instrumentResponse.redirectInfo.url.ToString());
            }
        }
        [HttpPost]
        //public ActionResult Response(HttpRequestMessage request)
        //{
        //    var input = request.Content.ReadAsStringAsync().Result;
        //    var saltKey = "099eb0cd-02cf-4e2a-8aca-3e";
        //    // Rest of the code
        //    return View(input, saltKey);
        //}
        public async Task<IActionResult> Response(HttpRequestMessage request)
        {
            var input = await request.Content.ReadAsStringAsync();
            var saltKey = "099eb0cd-02cf-4e2a-8aca-3e6c6aff0399";
            var saltIndex = 1;
            //var finalXHeader = HashString("/pg/v1/status/" + input["merchantId"] + "/" + input["transactionId"] + saltKey) + "###" + saltIndex;

            //using (var client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            //    client.DefaultRequestHeaders.Add("accept", "application/json");
            //    client.DefaultRequestHeaders.Add("X-VERIFY", finalXHeader);
            //    client.DefaultRequestHeaders.Add("X-MERCHANT-ID", input["transactionId"]);

            //    var response = await client.GetAsync("https://api-preprod.phonepe.com/apis/merchant-simulator/pg/v1/status/" + input["merchantId"] + "/" + input["transactionId"]);
            //    var responseContent = await response.Content.ReadAsStringAsync();
            //    var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
            //    Console.WriteLine(jsonResponse);
            //}
            return View(input);
        }

        private string HashString(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(bytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString;
            }
        }
        [HttpGet("InitiatePayment")]
        public IActionResult InitiatePayment()
        {
            //var options = new RestClientOptions("https://mercury-uat.phonepe.com/v4/debit/");
            //var client = new RestClient(options);
            //var request = new RestRequest("");
            //request.AddHeader("accept", "application/json");
            //request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("X-CALLBACK-URL", "https://www.demoMerchant.com/callback");
            //var response = client.Post(request);
            // Set your PhonePe credentials
            string merchantId = "M2306160483220675579140 ";
            string merchantSecretKey = "8289e078-be0b-484d-ae60-052f117f8deb\r\n";

            // Define payment parameters
            string orderId = "1";
            string amount = "1.00"; // e.g., "100.00"
            string returnUrl = "Callback"; // URL to redirect after successful payment

            // Construct the payment URL
            string baseUrl = "https://mercury-uat.phonepe.com/v4";
            string redirectUrl = $"{baseUrl}?merchant_id={merchantId}&order_id={orderId}&amount={amount}&return_url={returnUrl}";

            // Redirect the user to PhonePe for payment
            return Redirect(redirectUrl);
        }

        // Handle the callback from PhonePe
        [HttpPost("Callback")]
        public IActionResult Callback()
        {
            // You will receive callback parameters from PhonePe here
            string orderId = Request.Form["order_id"];
            string txnId = Request.Form["txn_id"];
            string status = Request.Form["status"];
            string checksum = Request.Form["checksum"];

            // Verify the payment status and checksum here

            // Process the payment and update your database

            // Redirect to a thank-you page or appropriate action
            return View("PaymentCallback");
        }
    }
}