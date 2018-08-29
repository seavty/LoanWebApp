using LoanWebApp.Models.DB;
using LoanWebApp.Models.DTO.Document;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace LoanWebApp.Helpers
{
    public static class DocumentHelper
    {
        //-> SaveUploadFiles
        public static async Task<List<sm_doc>> SaveUploadFiles(LoanEntities db, int tableID, int recordID, HttpRequestBase Request)
        {
            var files = Request.Files;
            List<sm_doc> documents = new List<sm_doc>();
            if (files != null)
            {
                for(int i= 0; i<files.Count; i++)
                {
                    if (files[i].ContentLength == 0)
                        continue;

                    string pathForSavingToDB = "", imageNameForSavingToDB = "";
                    string path = "";
                    string uploadFolderName = "uploads";
                    path = HttpContext.Current.Server.MapPath(@"~\" + uploadFolderName);
                    path = System.Configuration.ConfigurationManager.AppSettings["mediaURL"] +  @"\" + uploadFolderName;
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string currentYear = DateTime.Now.Year.ToString();
                    path += @"\" + currentYear;
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string currentMonth = DateTime.Now.Month > 9 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month;
                    path += @"\" + currentMonth;

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var createImageUniqueName = tableID + "_" + recordID + "_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmssfff") + "_" + i + ".jpg";//in order to avoid redundant image path , use i (variable)
                    files[i].SaveAs(path + @"\" + createImageUniqueName);

                    
                    pathForSavingToDB = uploadFolderName + "/" + currentYear + "/" + currentMonth + "/" + createImageUniqueName;
                    
                    var document = new sm_doc();
                    document.docs_Name = createImageUniqueName;
                    document.docs_TableID = tableID;
                    document.docs_CreatedDate = DateTime.Now;
                    document.docs_Value = recordID.ToString();
                    document.docs_FilePath = pathForSavingToDB;
                    db.sm_doc.Add(document);
                    await db.SaveChangesAsync();

                    documents.Add(document);
                }
            }
            return documents;
        }


        //-> Get Document
        public static List<DocumentViewDTO> GetDocuments(LoanEntities db, int tableID, int recordID)
        {
            string domain = HttpContext.Current.Request.Url.Authority;
            List<DocumentViewDTO> documentViews = new List<DocumentViewDTO>();
            IQueryable<sm_doc> documents = from d in db.sm_doc
                                           where d.docs_Deleted == null && d.docs_TableID == tableID && d.docs_Value == recordID.ToString()
                                           orderby d.docs_docID ascending
                                           select d;
            foreach (var document in documents)
            {
                var docView = MappingHelper.MapDBClassToDTO<sm_doc, DocumentViewDTO>(document);
                docView.fullPath = domain + "/" +  docView.path;
                documentViews.Add(docView);
            }

            return documentViews;
        }

        //-> Send email

        public static string sendEmail(string Subject, string Message, string To, string attachFile = "", string CC = "", bool isHtml = false)
        {
            string smtp = ConfigurationManager.AppSettings["smtp"];
            string smtpPort = ConfigurationManager.AppSettings["smtpport"];
            string isSSL = ConfigurationManager.AppSettings["smtpSSL"];
            string user = ConfigurationManager.AppSettings["smtpuser"];
            string pwd = ConfigurationManager.AppSettings["smtppwd"];
            string from = ConfigurationManager.AppSettings["smtpfrom"];
            string fromName = ConfigurationManager.AppSettings["smtpname"];

            SmtpClient smtpClient = new SmtpClient(smtp, int.Parse(smtpPort));
            smtpClient.UseDefaultCredentials = false;
            if (user != "")
            {
                smtpClient.Credentials = new System.Net.NetworkCredential(user, pwd);
            }
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (isSSL == "Y")
                smtpClient.EnableSsl = true;

            MailMessage mail = new MailMessage();
            mail.Subject = Subject;
            mail.BodyEncoding = new System.Text.UTF8Encoding();
            mail.IsBodyHtml = isHtml;
            mail.Body = Message;
            mail.From = new MailAddress(from, fromName);
            var tos = To.Split(';');
            foreach (string st in tos)
            {
                if (st != "")
                {
                    try
                    {
                        if (IsValidEmail(st))
                        {
                            var addr = new MailAddress(st);
                            mail.To.Add(addr);
                        }
                    }
                    catch (SmtpException e)
                    {
                        
                    }

                }
            }
            var ccs = CC.Split(';');
            foreach (string st in ccs)
            {
                if (st != "")
                {
                    try
                    {
                        if (IsValidEmail(st))
                        {
                            mail.CC.Add(new MailAddress(st));
                        }
                    }
                    catch (SmtpException e)
                    {
                        
                    }

                }
            }
            if (mail.To.Count > 0)
            {
                try
                {
                    if (attachFile != "")
                    {
                        foreach (string att in attachFile.Split(';'))
                        {
                            if (att != "")
                                mail.Attachments.Add(new Attachment(att));
                        }
                    }
                    smtpClient.Send(mail);
                    foreach (Attachment at in mail.Attachments)
                    {
                        at.Dispose();
                    }
                    return "ok";
                }
                catch (SmtpException e)
                {
                    
                    return "no";
                }
            }
            return "no";
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}