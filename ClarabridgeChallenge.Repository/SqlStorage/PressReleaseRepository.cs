using ClarabridgeChallenge.Models;
using ClarabridgeChallenge.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ClarabridgeChallenge.Repository.SqlStorage
{
    public class PressReleaseRepository : RepositoryBase, IPressReleaseRepository
    {
        public override ModelBase Get(Guid id)
        {
            PressRelease pressRelease = null;
            using(var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClarabridgeConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand()
                {
                    CommandText = "pressRelease_Get",
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection,
                    Parameters =
                    {
                        new SqlParameter("Id", id)
                    }
                })
                {
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        pressRelease = new PressRelease()
                        {
                            Id = (Guid)sqlDataReader["Id"],
                            Title = sqlDataReader["Title"].ToString(),
                            DescriptionHtml = sqlDataReader["DescriptionHtml"].ToString(),
                            DatePublished = Convert.ToDateTime(sqlDataReader["DatePublished"]),
                            DateCreated = Convert.ToDateTime(sqlDataReader["DateCreated"]),
                            DateUpdated = (sqlDataReader["DateUpdated"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(sqlDataReader["DateUpdated"]))
                        };
                    }
                }
            }
            return pressRelease;
        }

        public override IList<ModelBase> GetAll()
        {
            var pressReleases = new List<ModelBase>();
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClarabridgeConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand()
                {
                    CommandText = "pressRelease_GetAll",
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection
                })
                {
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        pressReleases.Add(new PressRelease()
                        {
                            Id = (Guid)sqlDataReader["Id"],
                            Title = sqlDataReader["Title"].ToString(),
                            DescriptionHtml = sqlDataReader["DescriptionHtml"].ToString(),
                            DatePublished = Convert.ToDateTime(sqlDataReader["DatePublished"]),
                            DateCreated = Convert.ToDateTime(sqlDataReader["DateCreated"]),
                            DateUpdated = (sqlDataReader["DateUpdated"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(sqlDataReader["DateUpdated"]))
                        });
                    }
                }
            }
            return pressReleases;
        }

        public override void Add(ModelBase model)
        {
            var pressRelease = (PressRelease) model;
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClarabridgeConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand()
                {
                    CommandText = "pressRelease_Add",
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection,
                    Parameters =
                    {
                        new SqlParameter("Id", pressRelease.Id),
                        new SqlParameter("Title", pressRelease.Title),
                        new SqlParameter("DescriptionHtml", pressRelease.DescriptionHtml),
                        new SqlParameter("DatePublished", pressRelease.DatePublished),
                        new SqlParameter("DateCreated", pressRelease.DateCreated)
                    }
                })
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public override void Update(ModelBase model)
        {
            var pressRelease = (PressRelease)model;
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClarabridgeConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand()
                {
                    CommandText = "pressRelease_Update",
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection,
                    Parameters =
                    {
                        new SqlParameter("Id", pressRelease.Id),
                        new SqlParameter("Title", pressRelease.Title),
                        new SqlParameter("DescriptionHtml", pressRelease.DescriptionHtml),
                        new SqlParameter("DatePublished", pressRelease.DatePublished),
                        new SqlParameter("DateUpdated", pressRelease.DateUpdated)
                    }
                })
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public override void Delete(Guid id)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClarabridgeConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand()
                {
                    CommandText = "pressRelease_Delete",
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection,
                    Parameters =
                    {
                        new SqlParameter("Id", id)
                    }
                })
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public override void DeleteAll()
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClarabridgeConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand()
                {
                    CommandText = "pressRelease_DeleteAll",
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection
                })
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
