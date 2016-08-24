using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Day25.Data;
using Day25.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Day25.API
{
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        private ApplicationDbContext _db;
        public MovieController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var movies = _db.Movies.Select(m => new {
                Title = m.Title,
                Actors = m.MovieActors.Select(ma => ma.Actor).ToList()
            }).ToList();

            return Ok(movies);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id) // id of the movie
        {
            var movie = _db.Movies.Where(m => m.Id == id).Select(m => new {
                Title = m.Title,
                Actors = m.MovieActors.Select(ma => ma.Actor).ToList()
            }).ToList();
            return Ok(movie);
        }

        // POST api/values
        [HttpPost("{id}")]
        public IActionResult Post(int id, [FromBody]Actor actor) // id is the movieId
        {
            _db.Actors.Add(actor);
            _db.SaveChanges();

            var movieActorToCreate = new MovieActor
            {
                MovieId = id,
                ActorId = actor.Id
            };
            _db.MovieActors.Add(movieActorToCreate);
            _db.SaveChanges();

            return Ok();
        }

        // Delete api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromBody]int actorId) // id is the movieId
        {
            var movieActorToDelete = _db.MovieActors.Where(ma => ma.MovieId == id && ma.ActorId == actorId).FirstOrDefault();

            _db.MovieActors.Remove(movieActorToDelete);
            _db.SaveChanges();

            return Ok();
        }

        
    }
}
