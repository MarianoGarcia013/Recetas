using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RecetasSLN.dominio;

namespace RecetasSLN.datos
{
    internal class DAO
    {
        private static SqlConnection cnn;

        public DAO()
        {
            cnn = new SqlConnection(Properties.Resources.cnnString);
        }

        public DataTable listarIngredientes()
        {
            DataTable resultado = new DataTable();
            
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = abrirCnn();
            cmd.CommandText = "[dbo].[SP_CONSULTAR_INGREDIENTES]";
            cmd.CommandType = CommandType.StoredProcedure;

            resultado.Load(cmd.ExecuteReader());

            return resultado;
        }

        public void ejecutarInsert(Receta oReceta)
        {
            bool ok = true;
            SqlTransaction t=null;
            SqlCommand cmd = new SqlCommand();

            try
            {
                cnn.Open();
                t = cnn.BeginTransaction();
                cmd.Connection = cnn;
                cmd.CommandType =CommandType.Text;
                cmd.CommandText = "[dbo].[SP_INSERTAR_RECETA]";
                cmd.Parameters.AddWithValue("tipo_receta", oReceta.tipoReceta);
                cmd.Parameters.AddWithValue("nombre", oReceta.tipoReceta);
                cmd.Parameters.AddWithValue("cheff", oReceta.tipoReceta);

                //parámetro de salida:
                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@id";
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pOut);
                cmd.ExecuteNonQuery();

                int recetaNro = (int)pOut.Value;

                SqlCommand cmdDetalle;
                int detalleNro = 1;

                foreach (DetalleReceta item in oReceta.listDetalle)
                {
                    cmdDetalle = new SqlCommand("[dbo].[SP_INSERTAR_DETALLES]", cnn, t);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.CommandType = CommandType.Text;
                    cmdDetalle.Parameters.AddWithValue("@id_receta", recetaNro);
                    cmdDetalle.Parameters.AddWithValue("@id_ingrediente",item.ingrediente.idIngrediente);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", item.cantidad);
                    //cmdDetalle.Parameters.AddWithValue("@nroDetalle", detalleNro);

                    detalleNro++;
                }
                t.Commit();

            }
            catch (Exception ex)
            {
                if (t != null)
                    t.Rollback();
                ok = false;
                
            }
        } 






        private SqlConnection abrirCnn()
        {
            if(cnn.State == ConnectionState.Closed)
            {
              cnn.Open();
            }

            return cnn;
        }

        private SqlConnection cerrarCnn()
        {
            if (cnn.State == ConnectionState.Open)
            {
                cnn.Close();
            }

            return cnn;
        }





    }
}
