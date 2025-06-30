using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using Proyecto.DAL.Models;

namespace Proyecto.DAL.DataAccess
{
    public class ComunaDAL
    {
        private readonly DbConnectionFactory _factory;

        public ComunaDAL(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        // Listar comunas por región
        public List<Comuna> ListarPorRegion(int idRegion)
        {
            var comunas = new List<Comuna>();

            using (var conn = _factory.CrearConexion())
            using (var command = new SqlCommand("sp_ListarComunasPorRegion", conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdRegion", idRegion);
                conn.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comunas.Add(new Comuna
                        {
                            IdComuna = Convert.ToInt32(reader["IdComuna"]),
                            IdRegion = Convert.ToInt32(reader["IdRegion"]),
                            NombreComuna = Convert.ToString(reader["NombreComuna"]) ?? string.Empty,
                            InformacionAdicional = DeserializeInformacion(reader["InformacionAdicional"]?.ToString() ?? string.Empty)
                        });
                    }
                }
            }

            return comunas;
        }

        // Obtener comuna específica
        public Comuna? Obtener(int idRegion, int idComuna)
        {
            using (var conn = _factory.CrearConexion())
            using (var cmd = new SqlCommand("sp_ObtenerComuna", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRegion", idRegion);
                cmd.Parameters.AddWithValue("@IdComuna", idComuna);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Comuna
                        {
                            IdComuna = Convert.ToInt32(reader["IdComuna"]),
                            IdRegion = Convert.ToInt32(reader["IdRegion"]),
                            NombreComuna = Convert.ToString(reader["NombreComuna"]) ?? string.Empty,
                            InformacionAdicional = DeserializeInformacion(reader["InformacionAdicional"]?.ToString() ?? string.Empty)
                        };
                    }
                }
            }

            return null;
        }

        // Guardar o actualizar comuna usando MERGE
        public void GuardarOActualizar(Comuna comuna)
        {

            if (comuna.IdRegion <= 0)
                throw new ArgumentException("IdRegion debe ser mayor que cero.", nameof(comuna.IdRegion));

            using (var conn = _factory.CrearConexion())
            using (var cmd = new SqlCommand("sp_MergeComuna", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdComuna",comuna.IdComuna > 0 ? (object)comuna.IdComuna : DBNull.Value);
                cmd.Parameters.AddWithValue("@IdRegion", comuna.IdRegion);
                cmd.Parameters.AddWithValue("@NombreComuna", comuna.NombreComuna);
                string infoXml = comuna.InformacionAdicional != null ? SerializeInformacion(comuna.InformacionAdicional) : string.Empty;
                cmd.Parameters.AddWithValue("@InformacionAdicional", infoXml);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Deserializar XML a objeto
        private InformacionAdicional DeserializeInformacion(string xml)
        {
            // Si el string es nulo o vacío, devolvemos un objeto vacío
            if (string.IsNullOrWhiteSpace(xml))
                return new InformacionAdicional(); // evita retornar null

            var serializer = new XmlSerializer(typeof(InformacionAdicional));

            using var reader = new StringReader(xml);
            return (InformacionAdicional?)serializer.Deserialize(reader) ?? new InformacionAdicional();
        }

        // Serializar objeto a XML
        private string SerializeInformacion(InformacionAdicional info)
        {
            var serializer = new XmlSerializer(typeof(InformacionAdicional));

            // Aquí eliminamos los namespaces predeterminados
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, info, namespaces);
                return stringWriter.ToString();
            }
        }




    }
}
