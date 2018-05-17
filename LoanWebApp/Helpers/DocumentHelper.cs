using LoanWebApp.Models.DB;
using LoanWebApp.Models.DTO.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoanWebApp.Helpers
{
    public static class DocumentHelper
    {
        //-> SaveUploadFiles
        public static async Task<List<sysDocument>> SaveUploadFiles(LoanEntities db, int tableID, int recordID, HttpRequestBase Request)
        {
            var files = Request.Files;
            List<sysDocument> documents = new List<sysDocument>();
            if (files != null)
            {
                for(int i= 0; i<files.Count; i++)
                {
                    string pathForSavingToDB = "", imageNameForSavingToDB = "";
                    string path = "";
                    string uploadFolderName = "uploads";
                    path = HttpContext.Current.Server.MapPath(@"~\" + uploadFolderName);
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

                    var createImageUniqueName = tableID + "_" + recordID + "_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmssfff") + ".jpg";
                    files[i].SaveAs(path + @"\" + createImageUniqueName);

                    
                    pathForSavingToDB = uploadFolderName + "/" + currentYear + "/" + currentMonth + "/" + createImageUniqueName;
                    
                    var document = new sysDocument();
                    document.name = createImageUniqueName;
                    document.tableID = tableID;
                    document.createdDate = DateTime.Now;
                    document.recordID = recordID;
                    document.path = pathForSavingToDB;
                    db.sysDocuments.Add(document);
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
            IQueryable<sysDocument> documents = from d in db.sysDocuments
                                           where d.deleted == null && d.tableID == tableID && d.recordID == recordID
                                           orderby d.id ascending
                                           select d;
            foreach (var document in documents)
            {
                var docView = MappingHelper.MapDBClassToDTO<sysDocument, DocumentViewDTO>(document);
                docView.fullPath = domain + "/" +  docView.path;
                documentViews.Add(docView);
            }

            return documentViews;
        }
    }
}