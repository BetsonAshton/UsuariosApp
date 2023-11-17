using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsuariosApp.Application.Models.AtualizarDados;
using UsuariosApp.Application.Models.Auntenticar;
using UsuariosApp.Application.Models.CriarConta;
using UsuariosApp.Application.Models.RecuperarSenhaModel;

namespace UsuariosApp.Application.Interfaces
{
    public interface IUsuarioAppService
    {
        CriarContaResponseModel CriarConta(CriarContaRequestModel model);

        AutenticarResponseModel Autenticar(AutenticarRequestModel model);

        RecuperarSenhaResponseModel RecuperarSenha(RecuperarSenhaRequestModel model);

        AtualizarDadosResponseModel AtualizarDados(AtualizarDadosRequestModel model, string email);

    }
}
