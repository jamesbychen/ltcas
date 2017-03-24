using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LTCAS.Models;

namespace LTCAS.WebApi
{
    public class loginuserController : ApiController
    {
        private ltc_dbEntities db = new ltc_dbEntities();

        // GET: api/loginuser
        public IQueryable<login_user> Getlogin_user()
        {
            return db.login_user;
        }

        // GET: api/loginuser/5
        [ResponseType(typeof(login_user))]
        public IHttpActionResult Getlogin_user(int id)
        {
            login_user login_user = db.login_user.Find(id);
            if (login_user == null)
            {
                return NotFound();
            }

            return Ok(login_user);
        }

        // PUT: api/loginuser/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlogin_user(int id, login_user login_user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != login_user.sn)
            {
                return BadRequest();
            }

            db.Entry(login_user).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!login_userExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/loginuser
        [ResponseType(typeof(login_user))]
        public IHttpActionResult Postlogin_user(login_user login_user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.login_user.Add(login_user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = login_user.sn }, login_user);
        }

        // DELETE: api/loginuser/5
        [ResponseType(typeof(login_user))]
        public IHttpActionResult Deletelogin_user(int id)
        {
            login_user login_user = db.login_user.Find(id);
            if (login_user == null)
            {
                return NotFound();
            }

            db.login_user.Remove(login_user);
            db.SaveChanges();

            return Ok(login_user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool login_userExists(int id)
        {
            return db.login_user.Count(e => e.sn == id) > 0;
        }
    }
}