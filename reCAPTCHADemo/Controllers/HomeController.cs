using Newtonsoft.Json.Linq;
using reCAPTCHADemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace reCAPTCHADemo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //try to pass value to view
            ViewBag.ActionName = "Index";
            ViewData["number"] = "1231456789";

            return View();
        }

        // POST: GetData
        public ActionResult GetDatas()
        {
            CustomRequest myRequests = new CustomRequest();
            //get post from ajax
            System.IO.Stream body = Request.InputStream;
            System.Text.Encoding encoding = Request.ContentEncoding;
            System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
            string json = reader.ReadToEnd();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            myRequests = (CustomRequest)serializer.Deserialize(json, typeof(CustomRequest));
            myRequests.isSuccess = false;

            //check grecaptcha
            string server_SecretKey = System.Configuration.ConfigurationManager.AppSettings["SecretKey"];
            if (myRequests.grecaptchaResponse != null && myRequests.grecaptchaResponse.Length != 0)
            {
                var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}a";
                var requestUri = string.Format(apiUrl, server_SecretKey, myRequests.grecaptchaResponse);
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                using (WebResponse response = request.GetResponse())
                {
                    using (System.IO.StreamReader stream = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        JObject jResponse = JObject.Parse(stream.ReadToEnd());
                        myRequests.isSuccess = jResponse.Value<bool>("success");
                    }
                }  
            }

            //return result
            JsonResult jsonObject = Json(myRequests);
            return jsonObject;
        }

    }
}