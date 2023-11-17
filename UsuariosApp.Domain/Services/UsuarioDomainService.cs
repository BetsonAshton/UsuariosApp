using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsuariosApp.Domain.Entities;
using UsuariosApp.Domain.Enums;
using UsuariosApp.Domain.Helpers;
using UsuariosApp.Domain.Interfaces.Messages;
using UsuariosApp.Domain.Interfaces.Repositories;
using UsuariosApp.Domain.Interfaces.Services;

namespace UsuariosApp.Domain.Services
{
    public class UsuarioDomainService:IUsuarioDomainService
    {
        private readonly IUsuarioRepository? _usuarioRepository;
        private readonly IHistoricoAtividadeRepository? _historicoAtividadeRepository;
       
        private readonly IUsuarioMessage? _usuarioMessage;

        public UsuarioDomainService(IUsuarioRepository? usuarioRepository, IHistoricoAtividadeRepository? historicoAtividadeRepository, IUsuarioMessage? usuarioMessage)
        {
            _usuarioRepository = usuarioRepository;
            _historicoAtividadeRepository = historicoAtividadeRepository;
            _usuarioMessage = usuarioMessage;
        }

        public void CriarConta(Usuario usuario)
        {
            if(_usuarioRepository?.Get(usuario.Email) != null)
            {
                throw new ApplicationException("O email informado já está cadastrado. Tente outro.");

            }

            usuario.Senha = MD5Helper.Encrypt(usuario.Senha);

            
            _usuarioRepository?.Create(usuario);

            //preenchendo o histórico desta atividade
            var historicoAtividade = new HistoricoAtividade
            {
                Id = Guid.NewGuid(),
                Tipo = TipoAtividade.CRIAÇÃO_DE_USUARIO,
                DataHora = DateTime.Now,
                Descricao = $"Cadastro do usuário {usuario.Nome} realizado com sucesso.",
                UsuarioId = usuario.Id
            };

            //cadastrar o historico desta atividade
            _historicoAtividadeRepository?.Create(historicoAtividade);

        }

        public Usuario Autenticar(string email, string senha)
        {
            var usuario = _usuarioRepository?.Get(email, MD5Helper.Encrypt(senha));

            if (usuario == null)
            {
                throw new ApplicationException("Acesso negado. Usuário não encontrado.");
            }

            var historicoAtividade = new HistoricoAtividade
            {
                Id = Guid.NewGuid(),
                Tipo = TipoAtividade.AUTENTICAÇÃO,
                DataHora = DateTime.Now,
                Descricao = $"Autenticação do usuário {usuario.Nome} realizado com sucesso.",
                UsuarioId = usuario.Id
            };

            _historicoAtividadeRepository?.Create(historicoAtividade);

         

            return usuario;
        }

        public Usuario RecuperarSenha(string email)
        {
            var usuario = _usuarioRepository?.Get(email);

            if (usuario == null)
            {
                throw new ApplicationException("Usuário não encontrado, verifique o email informado.");
            }

            //gerar uma nova senha para o usuário
            var novaSenha = PasswordHelper.GeneratePassword(true, true, true, true, 8);

            //atualizar a senha no banco de dados
            usuario.Senha = MD5Helper.Encrypt(novaSenha);
            _usuarioRepository?.Update(usuario);

            //enviar uma mensagem por email para o usuário com a nova senha
            var to = usuario.Email;
            var subject = "Recuperação de senha de acesso - COTI Informática";
            var body = $@"
                <div style='padding: 40px; margin: 40px; border: 1px solid #ccc; text-align: center;'>
                    <img src='https://files.cercomp.ufg.br/weby/up/301/o/Fluxo_tempor%C3%A1rio_de_recupera%C3%A7%C3%A3o_de_senha.png'/>
                    <hr/>
                    <h5>Olá {usuario.Nome}</h5>
                    <p>Uma nova senha de acesso foi gerada para você.</p>
                    <p>Acesse o sistema com a senha: {novaSenha}</p>
                    <br/>
                    <p>Att,A equipe </p>
                </div>
            ";

            _usuarioMessage?.SendMessage(to, subject, body);

            //preenchendo o histórico desta atividade
            var historicoAtividade = new HistoricoAtividade
            {
                Id = Guid.NewGuid(),
                Tipo = TipoAtividade.RECUPERAÇÃO_DE_SENHA,
                DataHora = DateTime.Now,
                Descricao = $"Recuperação de senha do usuário {usuario.Nome} realizado com sucesso.",
                UsuarioId = usuario.Id
            };

            _historicoAtividadeRepository?.Create(historicoAtividade);

            return usuario;
        }

        public Usuario AtualizarDados(string? email, string nome, string senha)
        {
            
            var usuario = _usuarioRepository?.Get(email);

            
            if (usuario == null)
            {
                throw new ApplicationException("Usuário não encontrado, verifique o email informado.");
            }

            var dadosAtualizados = false;

            
            if (!string.IsNullOrWhiteSpace(nome))
            {
                usuario.Nome = nome;
                dadosAtualizados = true;
            }

            
            if (!string.IsNullOrWhiteSpace(senha))
            {
                usuario.Senha = MD5Helper.Encrypt(senha);
                dadosAtualizados = true;
            }

           
            if (dadosAtualizados)
            {
                _usuarioRepository?.Update(usuario);
            }
            else
            {
                throw new ApplicationException("Informe pelo menos 1 campo do usuário para ser atualizado.");
            }

            return usuario;
        }
    }
}
