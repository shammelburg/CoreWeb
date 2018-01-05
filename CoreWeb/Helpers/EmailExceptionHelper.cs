using CoreWeb.Settings;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Helpers
{
    public class EmailExceptionHelper
    {
        private static Tuple<MailMessage, SmtpClient> Setup(EmailSettings Settings, string Subject)
        {
            using (var client = new SmtpClient(Settings.SMTPServer, Settings.Port))
            using (var mailMessage = new MailMessage())
            {
                if (!Settings.DefaultCredentials)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(Settings.UserName, Settings.Password);
                }

                mailMessage.From = new MailAddress(Settings.ExFrom, "Email Name");
                mailMessage.To.Add(Settings.ExTo);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = Subject;

                return Tuple.Create(mailMessage, client);
            }
        }

        public static void SendException(Exception ex, string UserName, EmailSettings settings, IHostingEnvironment env)
        {
            if (settings.ExEnabled)
            {
                var tuple = Setup(settings, $"({env.EnvironmentName}) EXCEPTION");
                tuple.Item1.Body = GetReport(ex, UserName);
                tuple.Item2.Send(tuple.Item1);
            }
        }

        public static void SendSqlException(SqlException ex, string UserName, EmailSettings settings, IHostingEnvironment env)
        {
            if (settings.ExEnabled)
            {
                var tuple = Setup(settings, $"({env.EnvironmentName}) SQL EXCEPTION");
                tuple.Item1.Body = GetSqlReport(ex, UserName);
                tuple.Item2.Send(tuple.Item1);
            }
        }

        public static string GetReport(Exception ex, string UserName = "")
        {
            //Get the current date and time
            string dateTime = DateTime.Now.ToLongDateString() + ", at " + DateTime.Now.ToShortTimeString();

            var sb = new StringBuilder();

            sb.Append("<table style='width: 700px;' border='1'>");
            sb.AppendLine("<tr><th colspan='2'>********** Exception Information **********</th></tr>");
            sb.AppendFormat("<tr><td>Exception Date</td><td>{0}</td></tr>", dateTime);
            sb.AppendFormat("<tr><td>User</td><td>{0}</td></tr>", UserName);

            sb.AppendLine("<tr><th colspan='2'>********** Exception Details **********</th></tr>");
            sb.AppendFormat("<tr><td>Message</td><td>{0}</td></tr>", ex.Message);
            sb.AppendFormat("<tr><td>Inner Exception</td><td>{0}</td></tr>", ex.InnerException);
            sb.AppendFormat("<tr><td>Source</td><td>{0}</td></tr>", ex.Source);
            sb.AppendFormat("<tr><td>Method</td><td>{0}</td></tr>", ex.TargetSite);
            sb.AppendFormat("<tr><td>Stack Trace</td><td>{0}</td></tr>", ex.StackTrace);
            sb.Append("<table>");

            return sb.ToString();
        }

        public static string GetSqlReport(SqlException ex, string UserName = "")
        {
            //Get the current date and time
            string dateTime = DateTime.Now.ToLongDateString() + ", at " + DateTime.Now.ToShortTimeString();

            var sb = new StringBuilder();

            sb.Append("<table style='width: 700px;' border='1'>");
            sb.AppendLine("<tr><th colspan='2'>********** SQL Exception Information **********</th></tr>");
            sb.AppendFormat("<tr><td>Exception Date</td><td>{0}</td></tr>", dateTime);
            sb.AppendFormat("<tr><td>User</td><td>{0}</td></tr>", UserName);
            sb.AppendFormat("<tr><td>Message</td><td>{0}</td></tr>", ex.Message);
            sb.AppendFormat("<tr><td>Inner</td><td>{0}</td></tr>", ex.InnerException);
            int count = ex.Errors.Count;

            for (int i = 0; i < count; i++)
            {
                sb.AppendLine($"<tr><th colspan='2'>********** SQL Exception Details ({i + 1}) **********</th></tr>");
                sb.AppendFormat("<tr><td>Message</td><td>{0}</td></tr>", ex.Errors[i].Message);
                sb.AppendFormat("<tr><td>Procedure</td><td>{0} (Line Number: {1})</td></tr>", ex.Errors[i].Procedure, ex.Errors[i].LineNumber);
                sb.AppendFormat("<tr><td>Server</td><td>{0}</td></tr>", ex.Errors[i].Server);
            }
            sb.Append("<table>");

            return sb.ToString();
        }
    }
}
