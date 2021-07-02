using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MayMoviesASP.Models
{
    public class DatabaseContext
    {
        public string ConnectionString { get; set; }
        public DatabaseContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Movie> GetAllMovies()
        {
            List<Movie> list = new List<Movie>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select movies.*, GROUP_CONCAT(DISTINCT movie_actors.actor_name SEPARATOR ', ') as actors from movies LEFT JOIN movie_actors ON movies.id = movie_actors.movie_id GROUP By movies.id", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Movie()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString(),
                            Genre = reader["genre"].ToString(),
                            Language =reader["language"].ToString(),
                            Quality = reader["quality"].ToString(),
                            Country = reader["country"].ToString(),
                            AgeRating = reader["ageRating"].ToString(),
                            Year = Convert.ToInt32(reader["year"]),
                            Image = reader["image"].ToString(),
                            Actors = reader["actors"].ToString().Split(", ")
                        });
                    }
                }
                conn.Close();
            }
            return list;
        }

        public Movie GetMovieById(int id)
        {
            List<Movie> list = new List<Movie>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select movies.*, GROUP_CONCAT(DISTINCT movie_actors.actor_name SEPARATOR ', ') as actors from movies LEFT JOIN movie_actors ON movies.id = movie_actors.movie_id AND movie.id = {id} GROUP By movies.id", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Movie()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString(),
                            Genre = reader["genre"].ToString(),
                            Language = reader["language"].ToString(),
                            Quality = reader["quality"].ToString(),
                            Country = reader["country"].ToString(),
                            AgeRating = reader["ageRating"].ToString(),
                            Year = Convert.ToInt32(reader["year"]),
                            Image = reader["image"].ToString(),
                            Actors = reader["actors"].ToString().Split(", ")
                        });
                    }
                }
                conn.Close();
            }
            return list.Count > 0 ? list[0] : null;
        }

        public List<Movie> GetMoviesByNameOrActor(string searchText)
        {
            List<Movie> list = new List<Movie>();
            using(MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select movies.*, GROUP_CONCAT(DISTINCT movie_actors.actor_name SEPARATOR ', ') as actors from movies LEFT JOIN movie_actors ON movies.id = movie_actors.movie_id WHERE (movies.name LIKE '%{searchText}%' OR movie_actors.actor_name LIKE '%{searchText}%') GROUP By movies.id", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Movie()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString(),
                            Genre = reader["genre"].ToString(),
                            Language = reader["language"].ToString(),
                            Quality = reader["quality"].ToString(),
                            Country = reader["country"].ToString(),
                            AgeRating = reader["ageRating"].ToString(),
                            Year = Convert.ToInt32(reader["year"]),
                            Image = reader["image"].ToString(),
                            Actors = reader["actors"].ToString().Split(", ")
                        });
                    }
                }
                conn.Close();
            }
            return list;
        }

        public void AddMovie(Movie movie)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO `movies`(`name`, `genre`, `language`, `quality`, `year`, `country`, `ageRating`, `image`) VALUES ('{movie.Name}','{movie.Genre}','{movie.Language}','{movie.Quality}','{movie.Year}','{movie.Country}','{movie.AgeRating}','{movie.Image}')", conn);
                cmd.ExecuteNonQuery();
                int id = Convert.ToInt32(cmd.LastInsertedId);
                movie.Actors = movie.Actors[0].Split(",");
                string actors = "";
                for(int i=0; i< movie.Actors.Length; i++)
                {
                    if(i < movie.Actors.Length -1 )
                        actors += $"({id},'{movie.Actors[i]}'),";
                    else
                        actors += $"({id},'{movie.Actors[i]}')";
                }
                cmd = new MySqlCommand($"INSERT INTO `movie_actors`(`movie_id`, `actor_name`) VALUES {actors}", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

    }
}
