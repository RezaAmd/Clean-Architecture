using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common.Models
{
    public class EmailTemplate
    {
        readonly static string button = "font-size: 0.75rem;margin:.25rem .125rem;text-align:center;padding:.375rem .75rem;border-radius:.25rem;text-decoration:none;";
        readonly static string grid = "display:grid;";
        readonly static string greenBg = "background-color:#008466;";
        readonly static string redBg = "background-color:#db0034;";
        readonly static string whiteColor = "color:#fff;";

        public static string PutToBody(string body)
        {
            return $"<div style=\"font-family:IRANSans,iransans,IRANSansX,IRANSansXFaNum,Tahoma;font-size:14px;text-align:right;direction:rtl;max-width:480px;margin:0 auto;\">{body}</div>";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="device"></param>
        /// <param name="dateTime"></param>
        /// <param name="Request">Force logout url.</param>
        /// <returns></returns>
        public static (string Title, string Body) SigninAlert(string ip, string device, string eToken, PersianDateTime dateTime, HttpRequest Request)
        {
            return ("ورود جدید به حساب کاربری شما!",
                PutToBody($"<div><img style=\"width:100%;\" src=\"https://dev.api.vatandar.org/assets/images/Security_On-bro.png\"</div><p>دستگاه جدیدی در تاریخ {dateTime.ToString("dddd d MMMM yyyy HH:mm")} وارد حساب شما شده است.</p><div style=\"text-align:center\"><h6>{ip}</h6><h6>{device}<h6></div><div style=\"{grid}\"><a style=\"{button + redBg + whiteColor}\" href=\"{Request.Scheme}://{Request.Host}/identity/account/forceLogout?eToken={eToken}\">من نبودم</a></div>"));
        }

        public static (string Title, string Body) ResetPassword(string token, string username, HttpRequest request)
        {
            return ("بازنشانی رمز ورود",
                PutToBody($"<p>درخواست بازنشانی رمز عبور داده اید؟ از دکمه زیر اقدام کنید. درغیر اینصورت لطفا این ایمیل را پاک کنید.</p><div style=\"{grid}\"><a style=\"{button + greenBg + whiteColor}\" href=\"{request.Scheme}://{request.Host}/identity/account/resetPassword?token={token}&username={username}\" >بازنشانی رمز</a></div>"));
        }
    }
}