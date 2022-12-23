using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.Extensions.Configuration;

namespace Business.Concrete
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new()
            {
                IsBodyHtml = isBodyHtml,
                Subject = subject,
                Body = body,
                From = new(_configuration["Mail:Username"], "SauBlog", System.Text.Encoding.UTF8)
            };
            foreach (var to in tos)
                mail.To.Add(to);

            SmtpClient smtp = new()
            {
                Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]),
                Port = int.Parse(_configuration["Mail:Port"]),
                EnableSsl = true,
                Host = _configuration["Mail:Host"]
            };
            await smtp.SendMailAsync(mail);
        }

        public async Task SendRegistrationCompletedMailAsync(string to, string newUserName)
        {
            string mailBody = "";
            #region mailBody
            mailBody =
              $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"> <head> <!--[if gte mso 9]> <xml> <o:OfficeDocumentSettings> <o:AllowPNG /> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml> <![endif]--> <meta http-equiv=\"Content-type\" content=\"text/html; charset=utf-8\" /> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1\" /> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /> <meta name=\"format-detection\" content=\"date=no\" /> <meta name=\"format-detection\" content=\"address=no\" /> <meta name=\"format-detection\" content=\"telephone=no\" /> <meta name=\"x-apple-disable-message-reformatting\" /> <!--[if !mso]><!--> <link href=\"https://fonts.googleapis.com/css?family=Muli:400,400i,700,700i\" rel=\"stylesheet\" /> <!--<![endif]--> <title>Welcome</title> <!--[if gte mso 9]> <style type=\"text/css\" media=\"all\"> sup {{ font-size: 100% !important; }} </style> <![endif]--> <style type=\"text/css\" media=\"screen\"> /* Linked Styles */\r\n        body\r\n{{ padding: 0!important; margin: 0!important; display: block!important; min - width: 100 % !important; width: 100 % !important; background: white; -webkit - text - size - adjust: none; }}\r\n    a\r\n{{\r\ncolor: #66c7ff; text-decoration: none; }} p {{ padding: 0 !important; margin: 0 !important; }} img {{ -ms-interpolation-mode: bicubic; /* Allow smoother rendering of resized image in Internet Explorer */ }} .mcnPreviewText {{ display: none !important; }} /* Mobile styles */ @media only screen and (max-device-width: 480px), only screen and (max-width: 480px) {{ .mobile-shell {{ width: 100% !important; min-width: 100% !important; }} .bg {{ background-size: 100% auto !important; -webkit-background-size: 100% auto !important; }} .text-header, .m-center {{ text-align: center !important; }} .center {{ margin: 0 auto !important; }} .container {{ padding: 20px 10px !important; }} .td {{ width: 100% !important; min-width: 100% !important; }} .m-br-15 {{ height: 15px !important; }} .p30-15 {{ padding: 30px 15px !important; }} .m-td, .m-hide {{ display: none !important; width: 0 !important; height: 0 !important; font-size: 0 !important; line-height: 0 !important; min-height: 0 !important; }} .m-block {{ display: block !important; }} .fluid-img img {{ width: 100% !important; max-width: 100% !important; height: auto !important; }} .column, .column-top, .column-empty, .column-empty2, .column-dir-top {{ float: left !important; width: 100% !important; display: block !important; }} .column-empty {{ padding-bottom: 10px !important; }} .column-empty2 {{ padding-bottom: 30px !important; }} .content-spacing {{ width: 15px !important; }} }} </style> </head> <body class=\"body\" style=\" padding: 0 !important; margin: 0 !important; display: block !important; min-width: 100% !important; width: 100% !important; background: white; -webkit-text-size-adjust: none; \"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"background-color: white;\"> <tr> <td align=\"center\" valign=\"top\"> <table width=\"650\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"mobile-shell\"> <tr> <td class=\"td container\" style=\" width: 650px; min-width: 650px; font-size: 0pt; line-height: 0pt; margin: 0; font-weight: normal; padding: 55px 0px; \"> <!-- Header --> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"p30-15\" style=\"padding: 0px 30px 30px 30px;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <th class=\"column-top\" width=\"145\" style=\" font-size: 0pt; line-height: 0pt; padding: 0; margin: 0; font-weight: normal; vertical-align: top; \"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"img m-center\" style=\" font-size: 0pt; line-height: 0pt; text-align: center; \"> <img src=\"https://www.codewithmukesh.com/wp-content/uploads/2020/05/codewithmukesh_logo_wordpress.png\" width=\"150\" height=\"50\" border=\"0\" alt=\"\" /> </td> </tr> </table> </th> <th class=\"column-empty\" width=\"1\" style=\" font-size: 0pt; line-height: 0pt; padding: 0; margin: 0; font-weight: normal; vertical-align: top; \"></th> <th class=\"column\" style=\" font-size: 0pt; line-height: 0pt; padding: 0; margin: 0; font-weight: normal; \"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"text-header\" style=\" color: #475c77; font-family: 'Muli', Arial, sans-serif; font-size: 12px; line-height: 16px; text-align: right; \"> \r\n\r\n        <a href=\"https://github.com/emreaknci/SauBlog\" target=\"_blank\" class=\"link2\" style=\" color: #475c77; text-decoration: none; \"> <span class=\"link2\" style=\" color: #475c77; text-decoration: none; \">You can view the source code here</span> </a>\r\n        \r\n        </td> </tr> </table> </th> </tr> </table> </td> </tr> </table> <!-- END Header --> <!-- Intro --> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td style=\"padding-bottom: 10px;\"> <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"tbrr p30-15\" style=\" padding: 60px 30px; border-radius: 26px 26px 0px 0px; \" bgcolor=\"#12325c\"> \r\n        <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"> <tr> <td class=\"h1 pb25\" style=\" color: #ffffff; font-family: 'Muli', Arial, sans-serif; font-size: 40px; line-height: 46px; text-align: center; padding-bottom: 25px; \"> Welcome, {newUserName}  </td> </tr> <tr> <td class=\"text-center pb25\" style=\" color: #c1cddc; font-family: 'Muli', Arial, sans-serif; font-size: 16px; line-height: 30px; text-align: center; padding-bottom: 25px; \"> You are currently registered using <span class=\"m-hide\"><br /></span> {to} </td> </tr> <!-- Button --> <tr> <td align=\"center\"> <table class=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"text-align: center;\"> </table>\r\n        </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </td> </tr> </table> </body> </html>";
            #endregion
            await SendMailAsync(to, "Thanks For Registration!", mailBody);
        }

        public async Task SendResetPasswordMailAsync(string to, string userName, string resetPasswordToken)
        {
            string mailBody = "";
            #region mailBody
            mailBody =
              $"<!-- <!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html> -->\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html charset=UTF-8\" />\r\n<link href=\"https://fonts.googleapis.com/css2?family=Roboto&display=swap\" rel=\"stylesheet\">\r\n<style>a,a:link,a:visited{{text-decoration:none;color:#00788a}}a:hover{{text-decoration:underline}}h2,h2 a,h2 a:visited,h3,h3 a,h3 a:visited,h4,h5,h6,.t_cht{{color:#000!important}}.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td{{line-height:100%}}.ExternalClass{{width:100%}}</style>\r\n</head>\r\n<body style=\"font-size:1.25rem;font-family:'Roboto',sans-serif;padding-left:20px;padding-right:20px;padding-top:20px;padding-bottom:20px;background-color:#fafafa;width:75%;max-width:1280px;min-width:600px;margin-right:auto;margin-left:auto\">\r\n<table cellpadding=\"12\" cellspacing=\"0\" width=\"100%\" bgcolor=\"#FAFAFA\" style=\"border-collapse:collapse;margin:auto\">\r\n<thead>\r\n<tr>\r\n<td style=\"padding-left:0;padding-right:0\">\r\n<img src=\"https://uploads-ssl.webflow.com/5e96c040bda7162df0a5646d/5f91d2a4d4d57838829dcef4_image-blue%20waves%402x.png\" style=\"width:80%;max-width:750px\" />\r\n</td>\r\n</tr>\r\n<tr>\r\n<td style=\"text-align:center;padding-bottom:20px\">\r\n<img src=\"https://uploads-ssl.webflow.com/5e96c040bda7162df0a5646d/5f8ebf5c6621845cd7ca4201_Logo-color.svg\" style=\"max-width:250px;width:30%\" />\r\n</td>\r\n</tr>\r\n</thead>\r\n<tbody>\r\n<tr>\r\n<td style=\"padding:50px;background-color:#fff;max-width:660px\">\r\n<table width=\"100%\" style=\"\">\r\n<tr>\r\n<td style=\"text-align:center\">\r\n<h1 style=\"font-size:30px;color:#202225;margin-top:0\">Hello {userName}</h1>\r\n<p style=\"font-size:18px;margin-bottom:30px;color:#202225;max-width:60ch;margin-left:auto;margin-right:auto\">A request has been received to change the password for your account</p>\r\n<a href=\"https://localhost:7144/Auth/ResetPassword/{resetPasswordToken}\" style=\"background-color:#1755f5;color:#fff;padding:8px 24px;border-radius:4px;border-style:solid;border-color:#1755f5;font-size:14px;text-decoration:none;cursor:pointer\">Reset Password </a>\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n<tfoot>\r\n<tr>\r\n<td style=\"text-align:center;padding-top:30px\">\r\n<table>\r\n<tr>\r\n<td><img src=\"https://uploads-ssl.webflow.com/5e96c040bda7162df0a5646d/5f91d2a4d80f5ebbf2ec0119_image-hand%20with%20wrench%402x.png\" style=\"width:100px\" /></td>\r\n<td>\r\n<td style=\"text-align:left;color:#b6b6b6;font-size:18px;padding-left:12px\">If you didn’t request this, you can ignore this email or let us know. Your password won’t change until you create a new password.</td>\r\n</td>\r\n</tr>\r\n</table>\r\n</td>\r\n</tr>\r\n</tfoot>\r\n</table>\r\n</body>";
            #endregion
            await SendMailAsync(to, "Password Reset Request", mailBody);

        }
    }
}
