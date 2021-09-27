using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ORMDesafio21Dias
{
    public sealed class Service
    {
        private CType cType;


        public Service(CType _cType)
        {
            this.cType = _cType;
        }

        public void Save()
        {
            using (SqlConnection conn = new SqlConnection(this.cType.ConnectionString))
            {
                List<string> cols = new List<string>();
                List<object> values = new List<object>();

                foreach (var p in this.cType.GetType().GetProperties())
                {
                    TableAttribute[] propertyAttributes = (TableAttribute[])p.GetCustomAttributes(typeof(TableAttribute), false);
                    if (propertyAttributes != null && propertyAttributes.Length > 0)
                    {
                        if (!propertyAttributes[0].IsNotOnDataBase && string.IsNullOrEmpty(propertyAttributes[0].PrimaryKey))
                        {
                            cols.Add(p.Name);
                            values.Add(p.GetValue(this.cType));
                        }
                    }

                    else
                    {
                        cols.Add(p.Name);
                        values.Add(p.GetValue(this.cType));
                    }
                }
                string table = $"{this.cType.GetType().Name.ToLower()}s";

                TableAttribute[] tableAttributes = (TableAttribute[])this.cType.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                if (tableAttributes != null && tableAttributes.Length > 0)
                {
                    table = tableAttributes[0].Name;
                }

                string sql = $"insert into {table} (";
                sql += string.Join(',', cols);
                sql += ") values (";
                sql += "@" + string.Join(", @", cols);
                sql += ")";




                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                for (var i = 0; i < cols.Count; i++)
                {
                    var value = values[i];
                    var col = cols[i];
                    if (value.GetType() == typeof(int))
                    {
                        cmd.Parameters.Add("@{col}", SqlDbType.Int);
                    }
                    else if (value.GetType() == typeof(string))
                    {
                        cmd.Parameters.Add("@{col}", SqlDbType.VarChar);
                    }
                    else if (value.GetType() == typeof(double))
                    {
                        cmd.Parameters.Add("@{col}", SqlDbType.Money);
                    }
                    else if (value.GetType() == typeof(DateTime))
                    {
                        cmd.Parameters.Add("@{col}", SqlDbType.DateTime);
                    }

                    cmd.Parameters[$"@{col}"].Value = value;
                }


                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public static T All<T>()
        {
            throw new NotImplementedException();
        }
    }
}
