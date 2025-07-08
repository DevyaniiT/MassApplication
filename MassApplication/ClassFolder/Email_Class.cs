using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MassApplication.ClassFolder
{
    public class Email_Class
    {
        public static string pdfFile = ConfigurationManager.AppSettings["pdfFile"];
        public static string from_mail = ConfigurationManager.AppSettings["From_mail"];
        public static string to_mail = ConfigurationManager.AppSettings["To_mail"];
        public static string cc_mail = ConfigurationManager.AppSettings["CC_mail"];
        public static string bcc_mail = ConfigurationManager.AppSettings["BCC_mail"];
        public static string path = ConfigurationManager.AppSettings["html_path"];

        public static string m_to, m_cc, m_bcc, m_subject, m_body, m_duration;
        public static string mail_to
        {
            get { return m_to; }
            set { m_to = value; }
        }
        public static string mail_cc
        {
            get { return m_cc; }
            set { m_cc = value; }
        }
        public static string mail_bcc
        {
            get { return m_bcc; }
            set { m_bcc = value; }
        }
        public static string mail_subject
        {
            get { return m_subject; }
            set { m_subject = value; }
        }
        public static string mail_body
        {
            get { return m_body; }
            set { m_body = value; }
        }
        public static string mail_duration
        {
            get { return m_duration; }
            set { m_duration = value; }
        }


        public static string last_runtime;
        
        /// <summary>
        /// This method is used for send mail
        /// </summary>
        public static void SendEmailerData()
        {
            MailMessage msg = new MailMessage();

            //DateTime todayDate;
            //todayDate = Convert.ToDateTime(runtime_date);
            //DateTime yesterdayDate = todayDate.AddDays(-1);
            //last_runtime = yesterdayDate.ToString("yyyy-MM-dd");

            DateTime todaydate = DateTime.Now;
            last_runtime = todaydate.ToString("yyyy-MM-dd");

            //string imagepath = "E:\\My Projects\\LinePack\\Report_schedulerApp\\ConsoleApplication2\\Templates\\Gail.Html";  //get from web.config

            //if (HttpContext.Current != null)
            //{
            //    var request = HttpContext.Current.Request;
            //    duration = request.QueryString["duration"];
            //}

            try
            {
                //Class_Folder.Error.GetErroronNotepad("Emailer");

                msg.From = new MailAddress(from_mail);
                msg.To.Add("devyanithakur281@gmail.com");
                msg.CC.Add("devyani.thakur@alethelabs.co.in");

                //msg.To.Add(to_mail);
                //msg.CC.Add(cc_mail);

                StreamReader reader = new StreamReader(path);
                string body = string.Empty;
                body = reader.ReadToEnd();
                msg.Body = body.ToString();

                msg.Attachments.Add(new Attachment(pdfFile));

                msg.IsBodyHtml = true;
                msg.Subject = "Sub: Daily Reconciliation report for  the gas day : " + last_runtime + "";

                SmtpClient smt = new SmtpClient();

                //SmtpClient smt = new SmtpClient("emailrelay1.gail.co.in");
                //smt.Port = 25;
                //smt.UseDefaultCredentials = false;
                //smt.EnableSsl = false;
                ////   const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                ////const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ////ServicePointManager.SecurityProtocol = Tls12;
                //smt.Credentials = new NetworkCredential("meteringgroup@gail.co.in", "ABC123@abc12");

                //smt.UseDefaultCredentials = true;

                smt.Send(msg);
            }
            catch (Exception ex)
            {
                ClassFolder.Error.GetErroronNotepad(ex.Message.ToString());
            }
            finally
            {
                /*
                    so that the error "The process cannot access the file because it is being used by another process" should not occur when we immediately
                    try to send another mail after one has been sent.
                    w3wp.exe:5800 CREATE D:\\Testcrystal.pdf
                    w3wp.exe:5800 WRITE D:\\Testcrystal.pdf
                    w3wp.exe:5800 CLOSE D:\\Testcrystal.pdf
                    w3wp.exe:5800 OPEN D:\\Testcrystal.pdf
                    w3wp.exe:5800 READ D:\\Testcrystal.pdf
                    As you can see, it created the PDF, it wrote the PDF, it closed the PDF (expected). 
                    Then, there was an unexpected Open, Read, without close immediately after the file was created. So that's why we've to close the file after sending
                 * it through mail because msg will open and read the file but the file is not close automattically after sending it. There will no error while sending the first
                 * mail but when you try to send the next mail one after the other the above error will rise. to avoid this error we are using the Dispose methods to release 
                 * all the resources which are being used.
                */
                msg.Dispose();
            }
        }

        /// <summary>
        /// This method is used for send mail with some parameters
        /// </summary>
        /// <param name="mail_to"></param>
        /// Defines the whom to send the mail
        /// <param name="mail_cc"></param>
        /// Defines the CC of tha mail
        /// <param name="mail_bcc"></param>
        /// Defines BCC of the mail
        /// <param name="mail_subject"></param>
        /// Defines the subject the mail
        /// <param name="mail_body"></param>
        /// Dfines the body of the mail
        /// <param name="mail_duration"></param>
        /// Defines the duration of the mail
        public static void SendEmailerData(string mail_to, string mail_cc, string mail_bcc,string mail_subject, string mail_body, string mail_duration)
        {
            MailMessage msg = new MailMessage();

            DateTime todaydate = DateTime.Now;
            last_runtime = todaydate.ToString("yyyy-MM-dd");
            try
            {
                msg.From = new MailAddress(from_mail);
                msg.To.Add(mail_to);
                
                if (mail_cc != null && mail_cc != "") {
                    msg.CC.Add(mail_cc);
                }
                if (mail_bcc != null && mail_bcc != "")
                {
                    msg.Bcc.Add(mail_bcc);
                }

                StreamReader reader = new StreamReader(path);
                //string body = string.Empty;
                //body = reader.ReadToEnd();
                if (mail_body != null && mail_body != "") {
                    msg.Body = mail_body.ToString();
                }
                msg.Attachments.Add(new Attachment(pdfFile));

                msg.IsBodyHtml = true;
                if (mail_subject != null && mail_subject != "")
                {
                    msg.Subject = mail_subject.ToString();
                }

                SmtpClient smt = new SmtpClient();
                smt.Send(msg);
            }
            catch (Exception ex)
            {
                ClassFolder.Error.GetErroronNotepad(ex.Message.ToString());
                throw new Exception(ex.Message);
            }
            finally
            {
                msg.Dispose();
            }
        }
    }
}