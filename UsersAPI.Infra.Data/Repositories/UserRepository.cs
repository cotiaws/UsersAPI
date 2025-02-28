using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersAPI.Domain.Entities;
using UsersAPI.Infra.Data.Contexts;
using UsersAPI.Infra.Data.Enums;

namespace UsersAPI.Infra.Data.Repositories
{
    /// <summary>
    /// Repositório de dados para a entidade 'Usuário (User)'
    /// </summary>
    public class UserRepository
    {
        //Método para gravar, alterar ou excluir usuário no banco de dados
        public void Execute(User user, OperationType operation)
        {
            using (var dataContext = new DataContext())
            {
                switch(operation)
                {
                    case OperationType.Add:
                        dataContext.Add(user);
                        break;

                    case OperationType.Update:
                        dataContext.Update(user);
                        break;

                    case OperationType.Delete:
                        dataContext.Remove(user);
                        break;
                }

                dataContext.SaveChanges();
            }
        }

        //Método para consultar 1 usuário através do email e da senha
        public User? GetByEmailAndPassword(string email, string password)
        {
            using (var dataContext = new DataContext())
            {
                //LAMBDA
                //return dataContext.Set<User>()
                //        .Include(u => u.Role)
                //        .Where(u => u.Email.Equals(email) && u.Password.Equals(password))
                //        .FirstOrDefault();

                //LINQ
                var query = from u in dataContext.Set<User>().Include(u => u.Role)
                            where u.Email.Equals(email) && u.Password.Equals(password)
                            select u;

                return query.FirstOrDefault();
            }
        }

        //Método para verificar se o email está cadastrado no banco de dados
        public bool HasEmail(string email)
        {
            using (var dataContext = new DataContext())
            {
                //LAMBDA
                //return dataContext.Set<User>().Where(u => u.Email.Equals(email)).Any();

                //LINQ
                var query = from u in dataContext.Set<User>()
                            where u.Email.Equals(email)
                            select u;

                return query.Any();
            }
        }
    }
}
