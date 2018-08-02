using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading;

namespace MVCPractice.Controllers
{
    public class RepoManagerController : Controller
    {
        //
        // GET: /RepoManager/

        public Models.RepoManagerServices services = new Models.RepoManagerServices();
        public Models.FolderModel folderModel = new Models.FolderModel();

        public ActionResult Index(Models.FolderModel model)
        {

            if (model.folderPath != null) {
                return View(model);
            }

            Session["fPath"] = folderModel.folderPath = Server.MapPath(@"~\wwroot\");

            return View(folderModel);

            
        }
        [HttpPost]

        public ActionResult Index(Models.FileModel model, Models.FolderModel fmodel)
        {
            //services = new Models.RepoManagerServices();

            if (fmodel.folderName != null){

                Thread t1 = new Thread(() => { Directory.CreateDirectory(Session["fPath"].ToString() + fmodel.folderName + @"\"); });

                t1.Start();
            }

            //var path = Server.MapPath("~/wwroot/");

            if (model.file != null || model.filePath != null)
            {
                Thread t = new Thread(() => { services.UploadFile(model.file, Session["fPath"].ToString()); });

                t.Start();
            }

            folderModel.folderPath = Session["fPath"].ToString();

            return View(folderModel);
        }

        public ActionResult EnterDir(Models.FolderModel fObject) {
            Session["fPath"] = folderModel.folderPath = fObject.folderPath;

            return RedirectToAction("Index", folderModel);
        }

        //public delegate FileContentResult func(byte[] data, string fileName);

        public FileResult DownloadFile(Models.FileModel model) {

            return File(System.IO.File.ReadAllBytes(model.filePath), System.Net.Mime.MediaTypeNames.Application.Octet, model.fileName);
        }

        public ActionResult Encrypt(string fileName) {
            services = new Models.RepoManagerServices();

            StreamReader reader = new StreamReader(Server.MapPath(@"~\wwroot\" + fileName));

            string data = reader.ReadToEnd();

            reader.Close();

            string encrStr = services.EncryptString(data);

            StreamWriter writer = new StreamWriter(Server.MapPath(@"~\wwroot\" + fileName));

            writer.Write(encrStr);

            writer.Close();

            return RedirectToAction("Index");
        }

        public ActionResult Decrypt(string fileName) {
            services = new Models.RepoManagerServices();

            StreamReader reader = new StreamReader(Server.MapPath(@"~\wwroot\" + fileName));

            string encrData = reader.ReadToEnd();

            string data = services.DecryptString(encrData);

            reader.Close();

            StreamWriter writer = new StreamWriter(Server.MapPath(@"~\wwroot\" + fileName));

            writer.Write(data);

            writer.Close();

            return RedirectToAction("Index");
            
        }

    }
}
