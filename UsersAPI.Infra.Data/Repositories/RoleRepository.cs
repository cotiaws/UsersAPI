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
    /// Repositório de dados para a entidade 'Perfil (Role)'
    /// </summary>
    public class RoleRepository
    {
        //Método para gravar, alterar ou excluir perfis
        public void Execute(Role role, OperationType operation)
        {
            using (var context = new DataContext())
            {
                switch(operation)
                {
                    case OperationType.Add: 
                        context.Add(role); 
                        break;
                    case OperationType.Update: 
                        context.Update(role); 
                        break;
                    case OperationType.Delete: 
                        context.Remove(role); 
                        break;
                };

                context.SaveChanges();
            }
        }

        //método para consultar todos os perfis do banco de dados
        public List<Role> GetAll()
        {
            using (var dataContext = new DataContext())
            {
                //LAMBDA
                //return dataContext.Set<Role>().OrderBy(r => r.Name).ToList();

                //LINQ (Language Integrated Query)
                var query = from r in dataContext.Set<Role>()
                            orderby r.Name ascending
                            select r;

                return query.ToList();
            }
        }

        //método para consultar 1 perfil através do nome
        public Role? GetByName(string name)
        {
            using (var dataContext = new DataContext())
            {
                //LAMBDA
                //return dataContext.Set<Role>().Where(r => r.Name.Equals(name)).FirstOrDefault();

                //LINQ (Language Integrated Query)
                var query = from r in dataContext.Set<Role>()
                            where r.Name.Equals(name)
                            select r;

                return query.FirstOrDefault();
            }
        }
    }
}
