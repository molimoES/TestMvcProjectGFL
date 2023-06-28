using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;
using TestMvcProjectGFL.Models;

namespace TestMvcProjectGFL.Controllers
{
    public class HomeController : Controller
    {
        CatalogContext db = new CatalogContext();

        [HttpGet]
        public ActionResult Index()
        {
            int idNum;

            try {
                idNum = int.Parse(HttpContext.Request.Url.Segments.Last());
            }
            catch (Exception) {
                idNum = db.Catalogs.Where(cat => cat.ParentId == 0).FirstOrDefault().Id;
            }

            ViewBag.Folder   = db.Catalogs.Where(cat => cat.Id == idNum).FirstOrDefault().Name;
            ViewBag.Catalogs = db.Catalogs.Where(cat => cat.ParentId == idNum).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult Export()
        {
            List<Catalog> catalogs = db.Catalogs.ToList();
            var ser = new XmlSerializer(typeof(List<Catalog>));
            var stream = new MemoryStream();
            ser.Serialize(stream, catalogs);
            stream.Position = 0;

            return File(stream, "application/xml", "Catalogs.xml");
        }

        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase xmlFile)
        {
            if(xmlFile.ContentType.Equals("application/xml") || xmlFile.ContentType.Equals("text/xml")) {
                try {
                    var xmlPath = Server.MapPath("~/Content/" + xmlFile.FileName);
                    xmlFile.SaveAs(xmlPath);
                    XDocument xDoc = XDocument.Load(xmlPath);
                    List<Catalog> catalogs = xDoc.Descendants("Catalog")
                        .Select(catalog =>
                            new Catalog {
                                Id = Convert.ToInt32(catalog.Element("Id").Value),
                                Name = catalog.Element("Name").Value,
                                ParentId = Convert.ToInt32(catalog.Element("ParentId").Value)
                            }).ToList();

                    if (catalogs.Count > 0) {
                        db.Database.Delete();
                        db.Database.CreateIfNotExists();

                        foreach (var item in db.Catalogs) {
                            db.Catalogs.Remove(item);
                        }
                        db.SaveChanges();

                        foreach (Catalog item in catalogs) {
                            db.Catalogs.Add(item);
                        }
                        db.SaveChanges();
                    }
                }
                catch (Exception) {
                    ViewBag.Error = "Can't import XML file";

                    return View();
                }

                return View();
            }
            else {
                ViewBag.Error = "Can't import XML file";

                return View();
            }
        }
    }
}