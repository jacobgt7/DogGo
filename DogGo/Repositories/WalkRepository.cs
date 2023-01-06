using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection 
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Date, Duration, DogId
                                        FROM Walks
                                        WHERE WalkerId = @walkerId";
                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                WalkerId = walkerId
                            };
                            walks.Add(walk);
                        }
                        return walks;
                    }
                }
            }
        }

        public List<Walk> GetRecentWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT TOP 5 w.Id, Date, Duration, DogId, o.Name
                                        FROM Walks w
                                        JOIN Dog d ON d.Id = w.DogId
                                        JOIN Owner o ON o.Id = d.OwnerId
                                        WHERE w.WalkerId = @walkerId
                                        ORDER BY Date DESC";
                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                WalkerId = walkerId,
                                ClientName = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            walks.Add(walk);
                        }
                        return walks;
                    }
                }
            }
        }
    }
}