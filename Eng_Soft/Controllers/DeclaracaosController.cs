using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BD;

namespace Eng_Soft.Controllers
{
    public class DeclaracaosController : Controller
    {
        private estp2Entities db = new estp2Entities();

        
        public ActionResult Index()
        {
            int id = int.Parse(Request.Cookies["user"]["id"]);
            var dec = db.Declaracao.Where(d => d.id_user == id);
            return View(dec.ToList());
        }

        // GET: Declaracaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Declaracao declaracao = db.Declaracao.Find(id);
            if (declaracao == null)
            {
                return HttpNotFound();
            }
            return View(declaracao);
        }

        

        // GET: Declaracaos/Create
        public ActionResult Create()
        {
            Declaracao d = new Declaracao();
            d.id_user = int.Parse(Response.Cookies["cookie"]["id"]);
            return View(d);
        }

        [HttpPost]
        public ActionResult Create(Declaracao model, HttpPostedFileBase file1)
        {
            if (file1 != null)
            {
                model.data = DateTime.Now;
                model.ficheiro = new byte[file1.ContentLength];
                file1.InputStream.Read(model.ficheiro, 0, file1.ContentLength);

            }
            db.Declaracao.Add(model);
            db.SaveChanges();
            return View(model);
        }

         
        [HttpGet]
        public FileResult Download(int id)
        {
            var file = (from d in db.Declaracao
                        where d.id == id
                        select new { d.descricao, d.ficheiro }).ToList().FirstOrDefault();
            byte[] fileBytes = file.ficheiro;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.descricao);
        }

    //    [HttpGet]
    //    public FileResult DownLoadFile(int id)
    //    {


    //        List<Declaracao> ObjFiles = GetFileList();

    //        var FileById = (from d in db.Declaracao
    //                        where d.id_user == id
    //                        select new { d.descricao, d.ficheiro }).ToList().FirstOrDefault();
    //                        //select new { FC.FileName, FC.FileContent }).ToList().FirstOrDefault();

    //        return File(FileById.FileContent, "application/pdf", FileById.FileName);

    //    }


    //    #region View Uploaded files  
    //    [HttpGet]
    //    public PartialViewResult FileDetails()
    //    {
    //        List<FileDetailsModel> DetList = GetFileList();

    //        return PartialView("FileDetails", DetList);


    //    }

        

    //    private List<FileDetailsModel> GetFileList()
    //    {
    //        List<FileDetailsModel> DetList = new List<FileDetailsModel>();

    //        DbConnection();
    //        con.Open();
    //        DetList = SqlMapper.Query<FileDetailsModel>(con, "GetFileDetails", commandType: CommandType.StoredProcedure).ToList();
    //        con.Close();
    //        return DetList;
    //    }

    //    #endregion

    //    #region Database related operations  
    //    private void SaveFileDetails(FileDetailsModel objDet)
    //    {

    //        DynamicParameters Parm = new DynamicParameters();
    //        Parm.Add("@FileName", objDet.FileName);
    //        Parm.Add("@FileContent", objDet.FileContent);
    //        DbConnection();
    //        con.Open();
    //        con.Execute("AddFileDetails", Parm, commandType: System.Data.CommandType.StoredProcedure);
    //        con.Close();


    //    }
        

          

    //    private SqlConnection con;
    //    private string constr;
    //    private void DbConnection()
    //    {
    //        constr = ConfigurationManager.ConnectionStrings["dbcon"].ToString();
    //        con = new SqlConnection(constr);

    //    }
        
    //}

    //// POST: Declaracaos/Create
    //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Create([Bind(Include = "id_user,ficheiro,data,descricao,id")] Declaracao declaracao)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        db.Declaracao.Add(declaracao);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }

    //    ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", declaracao.id_user);
    //    return View(declaracao);
    //}

    // GET: Declaracaos/Edit/5
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Declaracao declaracao = db.Declaracao.Find(id);
            if (declaracao == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", declaracao.id_user);
            return View(declaracao);
        }

        // POST: Declaracaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_user,ficheiro,data,descricao,id")] Declaracao declaracao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(declaracao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_user = new SelectList(db.Utilizador, "id", "cod_postal", declaracao.id_user);
            return View(declaracao);
        }

        // GET: Declaracaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Declaracao declaracao = db.Declaracao.Find(id);
            if (declaracao == null)
            {
                return HttpNotFound();
            }
            return View(declaracao);
        }

        // POST: Declaracaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Declaracao declaracao = db.Declaracao.Find(id);
            db.Declaracao.Remove(declaracao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
