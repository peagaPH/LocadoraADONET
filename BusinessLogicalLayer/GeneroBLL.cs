using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    /// <summary>
    /// Classe responsável pelas regras de negócio 
    /// da entidade Gênero.
    /// </summary>
    public class GeneroBLL : IEntityCRUD<Genero>
    {

        public Response Insert(Genero item)
        {
            Response response = Validate(item);
            if (response.Erros.Count > 0)
            {
                response.Sucesso = false;
                return response;
            }

            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                db.Generos.Add(item);
                db.SaveChanges();
            }
            response.Sucesso = true;
            return response;

        }
        public Response Update(Genero item)
        {
            Response response = Validate(item);
            if (response.Erros.Count > 0)
            {
                response.Sucesso = false;
                return response;
            }
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                
                db.Entry<Genero>(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            response.Sucesso = true;
            return response;
        }
        public Response Delete(int id)
        {
            Response response = new Response();
            if (id <= 0)
            {
                response.Erros.Add("ID do cliente não foi informado.");
            }
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }
            using(LocadoraDbContext db = new LocadoraDbContext())
            {
                db.Generos.Remove(db.Generos.Find(id));
                db.SaveChanges();
            }
            response.Sucesso = true;
            return response;
        }

        public DataResponse<Genero> GetData()
        {
            DataResponse<Genero> response = new DataResponse<Genero>();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    response.Data = db.Generos.ToList();
                    response.Sucesso = true;
                }
                catch (Exception ex)
                {
                    response.Sucesso = false;
                    response.Erros.Add("Erros no banco de dados, contate o adm");
                    File.WriteAllText("log.txt", ex.Message);
                }
            }
            response.Sucesso = true;
            return response;
        }

        public DataResponse<Genero> GetByID(int id)
        {
            DataResponse<Genero> response = new DataResponse<Genero>();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {

                try
                {
                    Genero genero = db.Generos.FirstOrDefault(x => x.ID == id);
                    response.Data.Add(genero);

                    response.Sucesso = true;
                }
                catch (Exception ex)
                {
                    response.Sucesso = false;
                    response.Erros.Add("Erros no banco de dados, contate o adm");
                    File.WriteAllText("log.txt", ex.Message);
                }
            }
            response.Sucesso = true;
            return response;
        }
        private Response Validate(Genero item)
        {
            Response response = new Response();
            if (string.IsNullOrWhiteSpace(item.Nome))
            {
                response.Erros.Add("O nome do gênero deve ser informado.");
            }
            else
            {
                //Remove espaços em branco no começo e no final da string.
                item.Nome = item.Nome.Trim();
                //Remove espaços extras entre as palavras, ex: "A      B", ficaria "A B".
                item.Nome = Regex.Replace(item.Nome, @"\s+", " ");
                if (item.Nome.Length < 2 || item.Nome.Length > 50)
                {
                    response.Erros.Add("O nome do gênero deve conter entre 2 e 50 caracteres");
                }
            }
            response.Sucesso = true;
            return response;
        }
    }
}
