using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;


namespace Roommates.Repositories
{
    /// <summary>
    ///  This class is responsible for interacting with Chore data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    public class ChoreRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoomRepository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public ChoreRepository(string connectionString) : base(connectionString) { }

       
        
        
        
        public List<Chore> GetAll()
        {
            //  We must "use" the database connection.
            //  Because a database is a shared resource (other applications may be using it too) we must
            //  be careful about how we interact with it. Specifically, we Open() connections when we need to
            //  interact with the database and we Close() them when we're finished.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {

                conn.Open();


                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = "SELECT Id, Name FROM Chore";


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        List<Chore> chores = new List<Chore>();


                        while (reader.Read())
                        {


                            int idColumnPosition = reader.GetOrdinal("Id");


                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);





                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };



                            chores.Add(chore);
                        }

                        return chores;
                    }
                }

            }
        }
        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name From Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        if(reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        return chore;
                    }
                }
            }
        }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name)
                                            OUTPUT INSERTED.Id
                                            VALUES(@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        public List<Chore> GetUnassignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT RoommateId, ChoreId, Chore.Id, Chore.Name
                                        FROM Chore
                                        LEFT JOIN RoommateChore on ChoreId = Chore.Id
                                        WHERE RoommateId IS NULL";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();
                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");

                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            chores.Add(chore);
                        }

                        return chores;
                    }
                }
            }
        }

        public void AssignChoreToRoommate(int cId, int rId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateId, ChoreId)
                                        VALUES(@rId, @cId)";
                    cmd.Parameters.AddWithValue("@rId", rId);
                    cmd.Parameters.AddWithValue("@cId", cId);
                    cmd.ExecuteNonQuery();


                }
            }
        }

        public List<Load> GetChoreCounts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT RoommateId, FirstName, Count(ChoreId) AS NumberOfChoresAssigned
                                        FROM Chore
                                        LEFT JOIN RoommateChore on ChoreId = Chore.Id
                                        LEFT JOIN Roommate on RoommateId = Roommate.Id
                                        WHERE RoommateId IS NOT NULL
                                        GROUP BY RoommateId, FirstName";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Load> loads = new List<Load>();
                        while(reader.Read())
                        {
                            Load load = new Load
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoommateId")),
                                RoommateName = reader.GetString(reader.GetOrdinal("FirstName")),
                                ChoreLoad = reader.GetInt32(reader.GetOrdinal("NumberOfChoresAssigned"))
                            };
                            loads.Add(load);
                        }
                        return loads;
                    }
                }
            }
        }
        /// <summary>
        ///  Updates a chore
        /// </summary>
        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                    SET Name = @name
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///  Delete the chore with the given id
        /// </summary>
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // What do you think this code will do if there is a roommate in the room we're deleting???
                    cmd.CommandText = "DELETE FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<AssignedChore> GetAssignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT RoommateId, FirstName, Name, RoommateChore.Id, Chore.Id AS IdOfChore
                                        FROM Chore
                                        LEFT JOIN RoommateChore on ChoreId = Chore.Id
                                        LEFT JOIN Roommate on RoommateId = Roommate.Id
                                        WHERE RoommateId IS NOT NULL";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<AssignedChore> assignedChores = new List<AssignedChore>();
                        while(reader.Read())
                        {
                            AssignedChore chore = new AssignedChore
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ChoreName = reader.GetString(reader.GetOrdinal("Name")),
                                ChoreId=reader.GetInt32(reader.GetOrdinal("IdOfChore")),
                                RoommateName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RoommateId = reader.GetInt32(reader.GetOrdinal("RoommateId"))
                            };
                            assignedChores.Add(chore);
                        }
                        return assignedChores;
                    }
                }
            }
        }

        public void DeleteChoreAssignment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // What do you think this code will do if there is a roommate in the room we're deleting???
                    cmd.CommandText = "DELETE FROM RoommateChore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
