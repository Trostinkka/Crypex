using Npgsql;
using System.Threading.Tasks;

namespace Crypex.Services
{
    public class AuthService
    {
        private readonly string _connectionString;

        public AuthService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> AuthenticateAsync(string login, string password)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT COUNT(*) FROM \"Crypex\".\"user\" WHERE \"Login\" = @login AND \"Password\" = @password";
            await using var command = new NpgsqlCommand(query, connection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);

            var count = (long)(await command.ExecuteScalarAsync())!;

            return count > 0;
        }

        public async Task<bool> RegisterAsync(string login, string firstName, string lastName, string password)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Проверка на существующего пользователя
            var checkQuery = "SELECT COUNT(*) FROM \"Crypex\".\"user\" WHERE \"Login\" = @login";
            await using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@login", login);
                var count = (long)(await checkCommand.ExecuteScalarAsync())!;
                if (count > 0)
                {
                    // Логин уже занят
                    return false;
                }
            }

            // Вставка нового пользователя
            var insertQuery = @"
                INSERT INTO ""Crypex"".""user"" (""Login"", ""FirstName"", ""LastName"", ""Password"")
                VALUES (@login, @firstName, @lastName, @password)";
            await using (var insertCommand = new NpgsqlCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@login", login);
                insertCommand.Parameters.AddWithValue("@firstName", firstName);
                insertCommand.Parameters.AddWithValue("@lastName", lastName);
                insertCommand.Parameters.AddWithValue("@password", password); // Не забудь: в реальных проектах пароль хэшируется!

                var rowsAffected = await insertCommand.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}