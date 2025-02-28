using System;
using System.Security.Cryptography;
using System.Text;

namespace UsersAPI.Infra.Data.Helpers
{
    /// <summary>
    /// Classe para criptografia de dados no padrão SHA256
    /// </summary>
    public class SHA256Encrypt
    {
        /// <summary>
        /// Método estático para realizar a criptografia SHA-256
        /// </summary>
        /// <param name="value">Texto a ser criptografado</param>
        /// <returns>Hash SHA-256 em formato hexadecimal</returns>
        public static string Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value), "O valor não pode ser nulo ou vazio.");

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(value);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Converter bytes do hash para string hexadecimal
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
