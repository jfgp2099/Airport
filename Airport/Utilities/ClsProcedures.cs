using Nettatica.Models;
using Nettatica.Models.DataModels;
using System;
using System.Data;
using System.Data.Common;

namespace Nettatica.Utilities
{
    public class ClsProcedures : IDisposable
    {
        private bool disposing = false;
        private DataModel db = new DataModel();


        /// <summary>
        /// Traer datos de la vista filtando por una columnaid y un valor especifico
        /// </summary>
        /// <param name="idcolum"></param>
        /// <param name="Value"></param>
        /// idcolum
        /// 1 IdReserva 
        /// 2 IdVuelo
        /// 3 FechaLlegada
        /// 4 AeropuertoOrigen
        /// 5 AeropuertoDestino
        /// 6 Aerolinea
        /// 7 IdCliente
        /// 8 Precio
        /// <returns></returns>
        public DataTable GetReservaView(int idcolum,string Value)
        {
            DataTable Result = new DataTable();

            using (DbConnection con = db.Database.Connection)
            {
                con.Open();
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(con);
                using (DbCommand cmd = dbFactory.CreateCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "[dbo].[FilterDataView]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    DbParameter par = cmd.CreateParameter();
                    par.Direction = ParameterDirection.Input;
                    par.ParameterName = "@Column";
                    par.Value = idcolum;
                    par.DbType = DbType.Int32;
                    cmd.Parameters.Add(par);

                    par = cmd.CreateParameter();
                    par.Direction = ParameterDirection.Input;
                    par.ParameterName = "@Value";
                    par.Value = Value;
                    par.DbType = DbType.String;
                    cmd.Parameters.Add(par);

                    using (DbDataReader DataR = cmd.ExecuteReader())
                    {
                        Result.Load(DataR);
                    }
                }
                con.Close();
            }

            return Result;
        }

        /// <summary>
        /// Liberacion de recursos
        /// </summary>

        public void Dispose()
        {
            if (disposing)
            {
                db.Dispose();
                return;
            }
            disposing = true;
        }






    }
}