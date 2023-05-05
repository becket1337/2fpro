using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2fpro.Pages
{
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }
        public int? StatusCode { get; set; }
        public string Message { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);


        public void OnGet(int? statusCode = null)
        {
            if (statusCode == null) statusCode = 500;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            if (statusCode == 404) Message = "Ресурс не найден!";
            else if (statusCode == 401) Message = "Запрос, требующий авторизации!";
            else if (statusCode == 400) Message = "Запрос не может быть воспринят сервером!";
            else if (statusCode == 403) Message = "Не доступа к запрашиваемому ресурсу!";
            else if (statusCode == 408 || statusCode == 504) Message = "Превышение интервала ожидания сервера!";
            else if (statusCode == 500) Message = "Внутренняя ошибка сервера!";
            else if (statusCode == 413) Message = "Объем запроса превышает допустимую границу в 4 Mbyte!";
            else if (statusCode == 503) Message = "Сервер временно недоступен, обычно из-за высокой нагрузки или выполняемого обслуживания!";
            else
            {

                Message = "Все плохо !";
            }
            //else if (statusCode == 301) Message = "Указанный ресурс перемещен!";
            StatusCode = statusCode;
        }
    }
}