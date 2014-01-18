using CVEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CVEditor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostCV(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileId = Guid.NewGuid().ToString();
                var path = FileIdToPath(fileId);
                file.SaveAs(path);

                return RedirectToAction("ShowCV", new { id = fileId });
            }

            return RedirectToAction("Index");
        }

        private string FileIdToPath(string fileId)
        {
            return Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileId + ".xml");
        }

        public ViewResult ShowCV(string id)
        {
            CV cv = LoadFromXml(id);
            TempData["fileid"] = id;
            return View(cv);
        }

        public ActionResult EditCV(string id)
        {
            CV cv = LoadFromXml(id);
            TempData["fileid"] = id;

            return View(cv);
        }

        [HttpPost]
        public ActionResult EditCV(CV cv, string id)
        {
            if (ModelState.IsValid)
            {
                var path = FileIdToPath(id);
                using (StreamWriter writer = new StreamWriter(path))
                {
                    XmlSerializer ser = new XmlSerializer(cv.GetType());
                    ser.Serialize(writer.BaseStream, cv);
                }
                return RedirectToAction("ShowCV", new { id = id });
            }
            else
            {
                TempData["fileid"] = id;
                return View(cv);
            }
        }

        private CV LoadFromXml(string fileId)
        {
            var path = FileIdToPath(fileId);

            CV cv = new CV();
            using (StreamReader reader = new StreamReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(cv.GetType());
                cv = serializer.Deserialize(reader.BaseStream) as CV;
            }

            return cv;
        }

        public ActionResult AddSlot(string id)
        {
            CV cv = LoadFromXml(id);
            cv.Jobs.Add(new Job
            {
                From = DateTime.Now,
                To = DateTime.Now
            });

            var path = FileIdToPath(id);
            using (StreamWriter writer = new StreamWriter(path))
            {
                XmlSerializer ser = new XmlSerializer(cv.GetType());
                ser.Serialize(writer.BaseStream, cv);
            }

            return RedirectToAction("EditCV", new { id = id });
        }

        public FileResult DownloadCV(string id)
        {
            var path = FileIdToPath(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "CV.xml");
        }
    }
}
