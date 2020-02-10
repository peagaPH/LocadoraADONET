using BusinessLogicalLayer.Security;
using DataAccessLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class FuncionarioBLL : IEntityCRUD<Funcionario>, IFuncionarioService
    {

        public DataResponse<Funcionario> Autenticar(string email, string senha)
        {
            //TODO: Validar email e Senha! As implementações não serão feitas 
            //pq a gente já viu isso 

            //Após validar, caso esteja tudo fofinho e pronto pra funcionar, chama o banco!

            senha = HashUtils.HashPassword(senha);
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();

            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                response.Data = db.Funcionarios.Where(f => f.Email == email).Where(f => f.Senha == senha).ToList();
            }
            if (response.Data.Count == 0)
            {
                response.Sucesso = false;
                return response;
            }
            if (response.Sucesso)
            {
                User.FuncionarioLogado = response.Data[0];
            }
            response.Sucesso = true;
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
                    db.Funcionarios.Remove(db.Funcionarios.Find(id));
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

        public DataResponse<Funcionario> GetByID(int id)
        {
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {

                try
                {
                    Funcionario funcionario = db.Funcionarios.FirstOrDefault(x => x.ID == id);
                    response.Data.Add(funcionario);
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

        public DataResponse<Funcionario> GetData()
        {
            DataResponse<Funcionario> response = new DataResponse<Funcionario>();
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    response.Data = db.Funcionarios.ToList();
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

        public Response Insert(Funcionario item)
        {
            Response response = Validate(item);

            if (response.HasErrors())
            {
                response.Sucesso = false;
                return response;
            }

            item.EhAtivo = true;
            item.Senha = HashUtils.HashPassword(item.Senha);
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                try
                {
                    db.Funcionarios.Add(item);
                    db.SaveChanges();
                    response.Sucesso = true;
                }
                catch (Exception ex )
                {
                    response.Sucesso = false;
                    response.Erros.Add("Erros no banco de dados, contate o adm");
                    File.WriteAllText("log.txt", ex.Message);
                }
                
            }
            response.Sucesso = true;

            return response;
        }

        public Response Update(Funcionario item)
        {
            Response response = new Response();

            if (string.IsNullOrWhiteSpace(item.CPF))
            {
                response.Erros.Add("O cpf deve ser informado");
            }
            else
            {
                item.CPF = item.CPF.Trim();
                if (!item.CPF.IsCpf())
                {
                    response.Erros.Add("O cpf informado é inválido.");
                }
            }
            if (response.HasErrors())
            {
                response.Sucesso = false;
                return response;
            }
            using (LocadoraDbContext db = new LocadoraDbContext())
            {
                
                db.Entry<Funcionario>(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            response.Sucesso = true;
            return response;
        }
        private Response Validate(Funcionario item)
        {
            Response response = new Response();

            if (string.IsNullOrWhiteSpace(item.CPF))
            {
                response.Erros.Add("O cpf deve ser informado");
            }
            else
            {
                item.CPF = item.CPF.Trim();
                if (!item.CPF.IsCpf())
                {
                    response.Erros.Add("O cpf informado é inválido.");
                }
            }

            string validacaoSenha = SenhaValidator.ValidateSenha(item.Senha, item.DataNascimento);
            if (validacaoSenha != "")
            {
                response.Erros.Add(validacaoSenha);
            }
            if (response.Erros.Count == 0)
            {
                response.Sucesso = true;
            }
            response.Sucesso = true;

            return response;
        }
    }
}
