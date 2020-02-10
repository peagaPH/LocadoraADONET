using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DataAccessLayer;
using Entities.ResultSets;
using Entities.Enums;
using System.IO;

namespace BusinessLogicalLayer
{
    public class FilmeBLL : IEntityCRUD<Filme>, IFilmeService
    {

        public DataResponse<Filme> GetByID(int id)
        {
            DataResponse<Filme> response = new DataResponse<Filme>();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {

                try
                {
                    Filme filme = db.Filmes.FirstOrDefault(x => x.ID == id);
                    response.Data.Add(filme);
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

        public DataResponse<Filme> GetData()
        {
            DataResponse<Filme> response = new DataResponse<Filme>();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    response.Data = db.Filmes.ToList();
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

        public DataResponse<FilmeResultSet> GetFilmes()
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    List<FilmeResultSet> filmes = db.Filmes.Select(f => new FilmeResultSet()
                    {
                        Classificacao = f.Classificacao,
                        Nome = f.Nome,
                        ID = f.ID,
                        Genero = f.Genero.Nome
                    }).ToList();
                    response.Data = filmes;
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

        public DataResponse<FilmeResultSet> GetFilmesByClassificacao(Classificacao classificacao)
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    List<FilmeResultSet> classificacaoFilme = db.Filmes
                                                                .Where(f => f.Classificacao == classificacao)
                                                                .Select(f => new FilmeResultSet()
                                                                {
                                                                    Classificacao = f.Classificacao,
                                                                    ID = f.ID,
                                                                    Nome = f.Nome,
                                                                    Genero = f.Genero.Nome
                                                                }).ToList();

                    response.Data = classificacaoFilme;
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

        public DataResponse<FilmeResultSet> GetFilmesByGenero(int genero)
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();
            if (genero <= 0)
            {
                response.Sucesso = false;
                response.Erros.Add("Gênero deve ser informado.");
                return response;
            }
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    List<FilmeResultSet> generoFilme = db.Filmes
                                                                .Where(f => f.GeneroID == genero)
                                                                .Select(f => new FilmeResultSet()
                                                                {
                                                                    Classificacao = f.Classificacao,
                                                                    ID = f.ID,
                                                                    Nome = f.Nome,
                                                                    Genero = f.Genero.Nome
                                                                }).ToList();
                    response.Data = generoFilme;
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

        public DataResponse<FilmeResultSet> GetFilmesByName(string nome)
        {
            DataResponse<FilmeResultSet> response = new DataResponse<FilmeResultSet>();

            if (string.IsNullOrWhiteSpace(nome))
            {
                response.Sucesso = false;
                response.Erros.Add("Nome deve ser informado.");
                return response;
            }
            nome = nome.Trim();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    List<FilmeResultSet> nomeFilme = db.Filmes
                                                         .Where(f => f.Nome == nome)
                                                         .Select(f => new FilmeResultSet()
                                                         {
                                                             Classificacao = f.Classificacao,
                                                             ID = f.ID,
                                                             Nome = f.Nome,
                                                             Genero = f.Genero.Nome
                                                         }).ToList();
                    response.Data = nomeFilme;
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

        public Response Insert(Filme item)
        {
            Response response = Validate(item);
            //TODO: Verificar a existência desse gênero na base de dados
            //generoBLL.LerID(item.GeneroID);

            //Verifica se tem erros!
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    db.Filmes.Add(item);
                    db.SaveChanges();
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
        public Response Update(Filme item)
        {
            Response response = Validate(item);
            //TODO: Verificar a existência desse gênero na base de dados
            //generoBLL.LerID(item.GeneroID);
            //Verifica se tem erros!

            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }

            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    db.Entry<Filme>(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    response.Sucesso = true;
                }
                catch (Exception ex)
                {
                    response.Sucesso = false;
                    response.Erros.Add("Erros no banco de dados, contate o adm");
                    File.WriteAllText("log.txt", ex.Message);
                }

            }
            return response;
        }
        public Response Delete(int id)
        {
            Response response = new Response();
            if (id <= 0)
            {
                response.Erros.Add("ID do filme não foi informado.");
            }
            if (response.Erros.Count != 0)
            {
                response.Sucesso = false;
                return response;
            }
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    db.Filmes.Remove(db.Filmes.Find(id));
                    db.SaveChanges();
                    response.Sucesso = true;
                }
                catch (Exception ex)
                {
                    response.Sucesso = false;
                    response.Erros.Add("Erros no banco de dados, contate o adm");
                    File.WriteAllText("log.txt", ex.Message);
                }

            }

            return response;
        }
        private Response Validate(Filme item)
        {
            Response response = new Response();

            if (item.Duracao <= 10)
            {
                response.Erros.Add("Duração não pode ser menor que 10 minutos.");
            }

            if (item.DataLancamento == DateTime.MinValue
                                    ||
                item.DataLancamento > DateTime.Now)
            {
                response.Erros.Add("Data inválida.");
            }
            response.Sucesso = true;
            return response;
        }
    }
}
