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
        public IQueryable<login_users> Getlogin_userss()
        {
            return db.login_users;
        }

        // GET: api/loginuser/5
        [ResponseType(typeof(login_users))]
        public IHttpActionResult Getlogin_userss(int id)
        {
            login_users login_users = db.login_users.Find(id);
            if (login_users == null)
            {
                return NotFound();
            }

            return Ok(login_users);
        }

        // PUT: api/loginuser/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putlogin_userss(int id, login_users login_users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != login_users.sn)
            {
                return BadRequest();
            }

            db.Entry(login_users).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!login_usersExists(id))
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
        [ResponseType(typeof(login_users))]
        public IHttpActionResult Postlogin_users(login_users login_users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.login_users.Add(login_users);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = login_users.sn }, login_users);
        }

        // DELETE: api/loginuser/5
        [ResponseType(typeof(login_users))]
        public IHttpActionResult Deletelogin_users(int id)
        {
            login_users login_users = db.login_users.Find(id);
            if (login_users == null)
            {
                return NotFound();
            }

            db.login_users.Remove(login_users);
            db.SaveChanges();

            return Ok(login_users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool login_usersExists(int id)
        {
            return db.login_users.Count(e => e.sn == id) > 0;
        }
    }
}