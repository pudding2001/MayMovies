using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MayMoviesASP.Models
{
    public class Movie
    {
        private DatabaseContext context;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }
        public string Language { get; set; }
        public string Quality { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public string AgeRating { get; set; }
        public string[] Actors { get; set; }
        public int Rating { get; set; }
        public string Image { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
