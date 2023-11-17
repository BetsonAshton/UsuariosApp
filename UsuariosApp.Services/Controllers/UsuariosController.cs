using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsuariosApp.Application.Interfaces;
using UsuariosApp.Application.Models.AtualizarDados;
using UsuariosApp.Application.Models.Auntenticar;
using UsuariosApp.Application.Models.CriarConta;
using UsuariosApp.Application.Models.RecuperarSenhaModel;

namespace UsuariosApp.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioAppService? _usuarioAppService;

        public UsuariosController(IUsuarioAppService? usuarioAppService)
        {
            _usuarioAppService = usuarioAppService;
        }

        [Route("criar-conta")]
        [HttpPost]
        [ProducesResponseType(typeof(CriarContaResponseModel), 201)]
        public IActionResult CriarConta([FromBody] CriarContaRequestModel model)
        {
            try
            {
                
                var response = _usuarioAppService?.CriarConta(model);

                
                return StatusCode(201, response);
            }
            catch (ApplicationException e)
            {
                
                return StatusCode(400, new { e.Message });
            }
            catch (Exception e)
            {
                
                return StatusCode(500, new { e.Message });
            }
        }

        [Route("autenticar")]
        [HttpPost]
        [ProducesResponseType(typeof(AutenticarResponseModel), 200)]
        public IActionResult Autenticar([FromBody] AutenticarRequestModel model)
        {
            try
            {
                
                var response = _usuarioAppService?.Autenticar(model);

                
                return StatusCode(200, response);
            }
            catch (ApplicationException e)
            {
                
                return StatusCode(401, new { e.Message });
            }
            catch (Exception e)
            {
                
                return StatusCode(500, new { e.Message });
            }
        }

        [Route("recuperar-senha")]
        [HttpPost]
        [ProducesResponseType(typeof(RecuperarSenhaResponseModel), 200)]
        public IActionResult RecuperarSenha([FromBody] RecuperarSenhaRequestModel model)
        {
            try
            {
                
                var response = _usuarioAppService?.RecuperarSenha(model);

               
                return StatusCode(200, response);
            }
            catch (ApplicationException e)
            {
                
                return StatusCode(400, new { e.Message });
            }
            catch (Exception e)
            {
                
                return StatusCode(500, new { e.Message });
            }
        }

        [Authorize]
        [Route("atualizar-dados")]
        [HttpPut]
        [ProducesResponseType(typeof(AtualizarDadosResponseModel), 200)]
        public IActionResult AtualizarDados([FromBody] AtualizarDadosRequestModel model)
        {
            try
            {
                //capturar o email do usuário autenticado, através do TOKEN
                var email = User.Identity.Name;

               
                var response = _usuarioAppService?.AtualizarDados(model, email);

                
                return StatusCode(200, response);
            }
            catch (ApplicationException e)
            {
                
                return StatusCode(400, new { e.Message });
            }
            catch (Exception e)
            {
                
                return StatusCode(500, new { e.Message });
            }
        }
    }
}
