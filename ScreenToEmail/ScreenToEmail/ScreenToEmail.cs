using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using ScreenShotDemo;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace ScreenEmailSender
{
    public class Sender
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        /// <summary>
        /// Main constructor of the Sender class.
        /// </summary>
        /// <param name="sendermail">The email to the person who sent the email.</param>
        /// <param name="usermail">The email to the person who the email should be sent to.</param>
        /// <param name="smtpclient">The SMTP client URL, required for sending the email. Check http://www.arclab.com/en/amlc/list-of-smtp-and-pop3-servers-mailserver-list.html for a list of URLs.</param>
        public Sender(string sendermail, string usermail, string smtpclient)
        {
            this.usermail = usermail;
            this.sendermail = sendermail;
            this.smtpclient = smtpclient;
        }

        /// <summary>
        /// Captures a screen-shot of the current active window and sends it over to the user's mail.
        /// </summary>
        public void CaptureAndSend()
        {
            ScreenCapture sc = new ScreenCapture();
            Image image = sc.CaptureWindow(GetActiveWindow());
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            string base64repres = Convert.ToBase64String(ms.ToArray());
            ms.Dispose();

            SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpclient);
            smtp.Send(new MailMessage(this.sendermail, this.usermail, "Screen capture", base64repres));
        }

        public string usermail { get; set; }
        public string sendermail { get; set; }
        public string smtpclient { get; set; }
    }
}
