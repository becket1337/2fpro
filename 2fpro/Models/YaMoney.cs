using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2fpro.Models
{
    public class YaMoney
    {
        public static string ClientId = "B7965577BBE645E6D51D32752FB577C8B2F030BD5ADDA35A6BB2DACD0C9AC364";
        public static string SecretId = "0DBC53633919EB95AFD3E1512E6914998F4B35C1765DDDA1A8311590F78DEC8C546425E365E273FA3C3BF6E20F8407D2A5615928FDF5BB2D720B9D969D25FAB6";
        public static string RedirectUri = "http://localhost/Checkout/Finished?transactionOk=true";
        public static string Scope = "payment-p2p";
        public static string patternId = "21144282";
        public static string message = "%D0%9D%D0%B0%D0%B7%D0%B";
        public static string to = "41001911846796";

        private string _accessToken;
        public string AccessToken
        {
            get
            {
                return _accessToken;
            }
            set
            {
                _accessToken = value;
            }
        }

        public string GetTokenRequestURL()
        {
            return "https://sp-money.yandex.ru/oauth/authorize?client_id=" + ClientId +
                                                     "&redirect_uri=" + RedirectUri + "&response_type=code&scope=" + Scope;

        }
        public string GetProcessPaymentURL()
        {
            return "https://sp-money.yandex.ru/request-payment?patternId=" + patternId;

        }
        public string RequestPayment()
        {
            string resultTxt = "";


            WebRequest request = (HttpWebRequest)WebRequest.Create("https://money.yandex.ru/api/request-payment");
            request.Method = "POST";
            request.Timeout = 120000;

            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Headers.Add("Authorization", "Bearer " + _accessToken);
            System.Net.ServicePointManager.Expect100Continue = false;
            string data = "pattern_id=p2p&to=" + to + "&amount_due=5.00&message=" + message + "&comment=" + message;
            //string content = "pattern_id=" + patternId + "&to=41001458233575&amount=5.00&message=" + message;
            /*  request.ContentLength = Encoding.UTF8.GetByteCount(content);

              Stream reqData = request.GetRequestStream();

              reqData.Write(Encoding.UTF8.GetBytes(content), 0, (int)request.ContentLength);
              reqData.Close();
              */
            byte[] sentData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = sentData.Length;

            try
            {
                Stream sendStream = request.GetRequestStream();

                // Выполняем запрос
                sendStream.Write(sentData, 0, (int)sentData.Length);
                sendStream.Close();
            }
            catch (WebException ex)
            {
                resultTxt = "Ошибка: " + ex;
            }

            var result = request.GetResponse();

            if (result is HttpWebResponse)
            {
                var httpWebResponse = (HttpWebResponse)result;

                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("HTTP protocol error", null,
                        WebExceptionStatus.ProtocolError, httpWebResponse);
                }
            }

            using (StreamReader reader = new StreamReader(result.GetResponseStream()))
            {
                resultTxt = reader.ReadToEnd();
            }


            dynamic jsonData = JObject.Parse(resultTxt);

            return (string)jsonData.request_id;
        }
        public string ProcessPayment()
        {
            string resultTxt = "";
            var requestId = RequestPayment();

            WebRequest request = (HttpWebRequest)WebRequest.Create("https://money.yandex.ru/api/process-payment");
            request.Method = "POST";
            request.Timeout = 120000;

            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Headers.Add("Authorization", "Bearer " + _accessToken);
            System.Net.ServicePointManager.Expect100Continue = false;
            string data = "request_id=" + requestId;

            byte[] sentData = Encoding.UTF8.GetBytes(data);
            request.ContentLength = sentData.Length;

            try
            {
                Stream sendStream = request.GetRequestStream();

                // Выполняем запрос
                sendStream.Write(sentData, 0, (int)sentData.Length);
                sendStream.Close();
            }
            catch (WebException ex)
            {
                resultTxt = "Ошибка: " + ex;
            }

            var result = request.GetResponse();

            if (result is HttpWebResponse)
            {
                var httpWebResponse = (HttpWebResponse)result;

                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException("HTTP protocol error", null,
                        WebExceptionStatus.ProtocolError, httpWebResponse);
                }
            }

            using (StreamReader reader = new StreamReader(result.GetResponseStream()))
            {
                resultTxt = reader.ReadToEnd();
            }


            return resultTxt;
        }
        public string GetAccountInfo()
        {
            string resultText = "";

            try
            {

                System.Net.WebRequest request = System.Net.WebRequest.Create("https://money.yandex.ru/api/account-info");
                request.Method = "POST"; // Устанавливаем метод передачи данных в POST
                request.Timeout = 120000; // Устанавливаем таймаут соединения
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.Headers.Add("Authorization", "Bearer " + _accessToken);

                System.Net.WebResponse result = request.GetResponse();
                request.ContentLength = 0;

                using (StreamReader reader = new StreamReader(result.GetResponseStream()))
                {
                    resultText = reader.ReadToEnd();
                }
            }
            catch (System.Net.WebException ex)
            {
                resultText = "Ошибка: " + ex.Message;

            }
            return resultText;
        }

        public string GetAccessTokenFromTemporaryToken(string temporaryToken)
        {

            System.Net.WebRequest reqPost = System.Net.WebRequest.Create("https://sp-money.yandex.ru/oauth/token");
            reqPost.Method = "POST"; // Устанавливаем метод передачи данных в POST
            reqPost.Timeout = 120000; // Устанавливаем таймаут соединения
            reqPost.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            System.Net.ServicePointManager.Expect100Continue = false;
            // Формируем параметры запроса
            string data = "code=" + temporaryToken + "&client_id=" + ClientId + "&client_secret=" +
                                                               SecretId + "&grant_type=authorization_code&redirect_uri="
                                                               + System.Web.HttpUtility.UrlEncode(RedirectUri, Encoding.UTF8);

            byte[] sentData = System.Text.Encoding.UTF8.GetBytes(data);
            reqPost.ContentLength = sentData.Length;
            Stream sendStream = reqPost.GetRequestStream();

            // Выполняем запрос
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();

            // Получаем результат
            System.Net.WebResponse result = reqPost.GetResponse();

            string resultString = "";

            using (StreamReader reader = new StreamReader(result.GetResponseStream()))
            {
                resultString = reader.ReadToEnd();
            }

            // Пытаемся разобрать
            JObject o = JObject.Parse(resultString);
            // и сохранить токен
            _accessToken = (string)o.SelectToken("access_token");

            // Если есть ошибка - возвращаем ее
            return (string)o.SelectToken("error");

        }

    }
}