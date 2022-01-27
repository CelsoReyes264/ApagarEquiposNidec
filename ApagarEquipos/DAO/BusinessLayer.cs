using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApagarEquipos.DAO
{
    public class BusinessLayer: IDisposable
    {
        public String Exception { get; set; }
        public BusinessLayer()
        {
            InputParameters = new List<SqlParameter>();
            OutputParameters = new List<SqlParameter>();
        }
        private SqlConnection _conn { get; set; }
        public enum DBConn
        {
            SIMDB = 1
        }

        private bool Connect(DBConn _DbConnection)
        {
            bool _result = false;
            try
            {
                switch (_DbConnection)
                {
                    case DBConn.SIMDB:
                        _conn = new SqlConnection(Properties.Settings.Default.SIMConnectionString);
                        break;
                }
                _conn.Open();
                _result = true;
            }
            catch { }
            return _result;
        }
        public void Disconnect()
        {
            _conn.Close();
            _conn.Dispose();
        }

        public int ExecuteQuery(DBConn _connection, String _query)
        {
            int _result = 0;
            try
            {
                if (Connect(_connection))
                {
                    _result = new SqlCommand(_query, _conn) { CommandTimeout = 0 }.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Exception = GetException(ex);
            }
            finally
            {
                Disconnect();
            }
            return _result;
        }

        public object ExecuteScalar(DBConn _connection, String _query)
        {
            object _result = null;
            try
            {
                if (Connect(_connection))
                {
                    _result = new SqlCommand(_query, _conn) { CommandTimeout = 0 }.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Exception = GetException(ex);
            }
            finally
            {
                Disconnect();
            }
            return _result;
        }

        public DataTable ExecuteDataTable(DBConn _connection, String _query)
        {
            DataTable _result = new DataTable();
            try
            {
                if (Connect(_connection))
                {
                    new SqlDataAdapter(new SqlCommand(_query, _conn) { CommandTimeout = 0 }).Fill(_result);
                }
            }
            catch (Exception ex)
            {
                Exception = GetException(ex);
            }
            finally
            {
                Disconnect();
            }
            return _result;
        }

        public List<DataTable> ExecuteDataTables(DBConn _connection, String _query)
        {
            List<DataTable> _result = new List<DataTable>();
            DataSet _dataSet = new DataSet();
            try
            {
                if (Connect(_connection))
                {
                    new SqlDataAdapter(new SqlCommand(_query, _conn) { CommandTimeout = 0 }).Fill(_dataSet);
                    foreach (DataTable dt in _dataSet.Tables)
                    {
                        _result.Add(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Exception = GetException(ex);
            }
            finally
            {
                Disconnect();
            }
            return _result;
        }

        #region Stored Procedure
        private List<SqlParameter> InputParameters { get; set; }
        private List<SqlParameter> OutputParameters { get; set; }
        public BusinessLayer AddParam(String _param_name, Object _param_value)
        {
            InputParameters.Add(new SqlParameter()
            {
                ParameterName = _param_name,
                Value = _param_value
            });
            return this;
        }

        public BusinessLayer AddParam(String _param_name, Object _param_value, Boolean _is_output, int _size)
        {
            InputParameters.Add(new SqlParameter()
            {
                ParameterName = _param_name,
                Value = _param_value,
                Direction = _is_output ? ParameterDirection.InputOutput : ParameterDirection.Input,
                Size = _size
            });
            return this;
        }

        public int ProcedureQuery(DBConn _connection, String _procedure_name)
        {
            int _result = 0;
            OutputParameters.Clear();
            try
            {
                if (Connect(_connection))
                {
                    SqlCommand _command = new SqlCommand()
                    {
                        CommandText = _procedure_name,
                        CommandType = CommandType.StoredProcedure,
                        Connection = _conn,
                        CommandTimeout = 0
                    };
                    _command.Parameters.AddRange(InputParameters.ToArray());
                    _result = _command.ExecuteNonQuery();
                    foreach (SqlParameter _param in _command.Parameters)
                    {
                        if (_param.Direction == ParameterDirection.InputOutput)
                        {
                            OutputParameters.Add(_param);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception = GetException(ex);
            }
            finally
            {
                InputParameters.Clear();
                Disconnect();
            }
            return _result;
        }

        public Object ProcedureScalar(DBConn _connection, String _procedure_name)
        {
            Object _result = null;
            OutputParameters.Clear();
            try
            {
                if (Connect(_connection))
                {
                    SqlCommand _command = new SqlCommand()
                    {
                        CommandText = _procedure_name,
                        CommandType = CommandType.StoredProcedure,
                        Connection = _conn,
                        CommandTimeout = 0
                    };
                    _command.Parameters.AddRange(InputParameters.ToArray());
                    _result = _command.ExecuteScalar();
                    foreach (SqlParameter _param in _command.Parameters)
                    {
                        if (_param.Direction == ParameterDirection.InputOutput)
                        {
                            OutputParameters.Add(_param);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception = GetException(ex);
            }
            finally
            {
                InputParameters.Clear();
                Disconnect();
            }
            return _result;
        }

        public DataTable ProcedureDataTable(DBConn _connection, String _procedure_name)
        {
            DataTable _result = new DataTable();
            OutputParameters.Clear();
            try
            {
                if (Connect(_connection))
                {
                    SqlCommand _command = new SqlCommand()
                    {
                        CommandText = _procedure_name,
                        CommandType = CommandType.StoredProcedure,
                        Connection = _conn,
                        CommandTimeout = 0
                    };
                    _command.Parameters.AddRange(InputParameters.ToArray());
                    SqlDataAdapter adapter = new SqlDataAdapter(_command);
                    adapter.Fill(_result);
                    foreach (SqlParameter _param in _command.Parameters)
                    {
                        if (_param.Direction == ParameterDirection.InputOutput)
                        {
                            OutputParameters.Add(_param);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception = GetException(ex);
            }
            finally
            {
                InputParameters.Clear();
                Disconnect();
            }
            return _result;
        }

        public DataSet ProcedureDataSet(DBConn _connection, String _procedure_name)
        {
            DataSet _ds = new DataSet();
            OutputParameters.Clear();
            try
            {
                if (Connect(_connection))
                {
                    SqlCommand _command = new SqlCommand()
                    {
                        CommandText = _procedure_name,
                        CommandType = CommandType.StoredProcedure,
                        Connection = _conn,
                        CommandTimeout = 0
                    };
                    _command.Parameters.AddRange(InputParameters.ToArray());
                    SqlDataAdapter adapter = new SqlDataAdapter(_command);
                    adapter.Fill(_ds);
                    foreach (SqlParameter _param in _command.Parameters)
                    {
                        if (_param.Direction == ParameterDirection.InputOutput)
                        {
                            OutputParameters.Add(_param);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception = GetException(ex);
            }
            finally
            {
                InputParameters.Clear();
                Disconnect();
            }
            return _ds;
        }

        public Object GetParamValue(String _param_name)
        {
            Object _result = null;
            foreach (SqlParameter _param in OutputParameters)
            {
                if (_param.ParameterName == _param_name)
                {
                    _result = _param.Value;
                }
            }
            return _result;
        }
        #endregion

        public String GetException(Exception ex)
        {
            return ex.InnerException == null ? ex.Message : ex.Message + ";Inner Exception: " + GetException(ex.InnerException);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    InputParameters = null;
                    OutputParameters = null;
                    Exception = null;
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BussinesLayer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
