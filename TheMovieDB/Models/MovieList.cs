﻿using System.Collections.Generic;

namespace TheMovieDB.Models
{
    public class MovieList
    {
        public List<Movie> Results { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}