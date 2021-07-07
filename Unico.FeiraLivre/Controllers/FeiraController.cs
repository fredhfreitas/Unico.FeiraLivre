﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Unico.FeiraLivre.Service.Features.FeiraFeatures.Commands;
using Unico.FeiraLivre.Service.Features.FeiraFeatures.Queries;

namespace Unico.FeiraLivre.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/Feira")]
    [ApiVersion("1.0")]
    public class FeiraController : ControllerBase
    {
        public ILogger<FeiraController> _logger { get; }
        private IMediator _mediator;        
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public FeiraController(ILogger<FeiraController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFeiraCommand command)
        {
            _logger.LogTrace("Create Feira Iniciando");
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogTrace("GetAll Iniciando");
            return Ok(await Mediator.Send(new GetAllFeiraQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogTrace("GetById Iniciando");
            return Ok(await Mediator.Send(new GetFeiraByIdQuery { Id = id }));
        }

        [HttpGet("distrito/{id}")]
        public async Task<IActionResult> GetByDistrito(string distrito)
        {
            _logger.LogDebug("GetByDistrito Iniciando");
            return Ok(await Mediator.Send(new GetFeiraByDistritoQuery { Distrito = distrito }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogDebug("Delete Iniciando");
            return Ok(await Mediator.Send(new DeleteFeiraByIdCommand { Id = id }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFeiraCommand command)
        {
            if (id != command.Id)
            {
                _logger.LogWarning(string.Format("Erro ao atualizar por id:{0}", id.ToString()));
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

    }
}
