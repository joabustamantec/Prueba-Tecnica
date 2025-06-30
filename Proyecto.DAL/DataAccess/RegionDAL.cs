using Proyecto.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Proyecto.DAL.DataAccess
{
    public class RegionDAL
    {
        private readonly DbConnectionFactory _factory;

        public RegionDAL(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        // Obtener todas las regiones
        public List<Region> ListarRegiones()
        {
            var regiones = new List<Region>();

            using (var conn = _factory.CrearConexion())
            using (var cmd = new SqlCommand("sp_ListarRegiones", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        regiones.Add(new Region
                        {
                            IdRegion = Convert.ToInt32(reader["IdRegion"]),
                            NombreRegion = Convert.ToString(reader["NombreRegion"]) ?? string.Empty
                        });
                    }
                }
            }

            return regiones;
        }

        // Obtener una región por ID
        public Region? ObtenerRegion(int id)
        {
            using (var conn = _factory.CrearConexion())
            using (var cmd = new SqlCommand("sp_ObtenerRegion", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRegion", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Region
                        {
                            IdRegion = Convert.ToInt32(reader["IdRegion"]),
                            NombreRegion = Convert.ToString(reader["NombreRegion"]) ?? string.Empty
                        };
                    }
                }
            }

            return null;
        }

        // Insertar o actualizar una región usando SP con MERGE
        public void GuardarOActualizar(Region region)
        {
            using (var conn = _factory.CrearConexion())
            using (var cmd = new SqlCommand("sp_MergeRegion", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdRegion", region.IdRegion > 0 ? (object)region.IdRegion : DBNull.Value);
                cmd.Parameters.AddWithValue("@NombreRegion", region.NombreRegion);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

   
    }
}
