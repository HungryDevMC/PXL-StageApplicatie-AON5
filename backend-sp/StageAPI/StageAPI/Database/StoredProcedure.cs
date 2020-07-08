using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace StageAPI.Database
{
    public class StoredProcedure : IDisposable
    {
        private MySqlConnection _connection => DBConnection.Connection;

        private readonly string _name;
        private readonly MySqlCommand _command;
        private readonly Dictionary<string, object> _outputParameters;

        public StoredProcedure(string name)
        {
            _name = name;

            _command = _connection.CreateCommand();
            _command.CommandText = name;
            _command.CommandType = CommandType.StoredProcedure;

            _outputParameters = new Dictionary<string, object>();
        }

        public void AddOutput<T>(string name, int size = -1)
        {
            if (_outputParameters.ContainsKey(name))
            {
                return;
            }

            var param = _command.Parameters.AddWithValue(name, default(T));
            param.Direction = ParameterDirection.Output;

            if (size != -1)
            {
                param.Size = size;
            }

            _outputParameters.Add(name, default(T));
        }

        public T GetOutput<T>(string name)
        {
            if (!_outputParameters.ContainsKey(name) || _outputParameters[name] is DBNull)
            {
                return default;
            }

            return (T) _outputParameters[name];
        }

        public void AddParameter(string name, object value, int size = -1)
        {
            var param = _command.Parameters.AddWithValue($"@{name}", value ?? DBNull.Value);
            param.Direction = ParameterDirection.Input;

            if (size != -1)
            {
                param.Size = size;
            }
        }

        public void FinalizeOutput()
        {
            for (var i = 0; i < _outputParameters.Count; i++)
            {
                var output = _outputParameters.ElementAt(i);

                _outputParameters[output.Key] = _command.Parameters[output.Key].Value;
            }
        }

        public async Task<StoredProcedure> Run()
        {
            await _command.ExecuteNonQueryAsync();
            FinalizeOutput();

            return this;
        }

        public async Task<MySqlDataReader> RunReader()
        {
            var reader = await _command.ExecuteReaderAsync();
            FinalizeOutput();

            return (MySqlDataReader) reader;
        }

        public void Dispose()
        {
            _command.Dispose();
        }
    }
}
