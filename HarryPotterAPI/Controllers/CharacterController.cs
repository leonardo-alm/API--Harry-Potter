using HarryPotterAPI.Models;
using HarryPotterAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HarryPotterAPI.Dtos;
using HarryPotterAPI.AuthorizationAndAuthentication;
using Microsoft.AspNetCore.Authorization;

namespace HarryPotterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterRepository _repository;

        public CharacterController(ICharacterRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Character), StatusCodes.Status200OK)]
        public async Task<ActionResult<Character>> Get([FromQuery]int page, int maxResults)
        {
            var characters = _repository.GetCharacters(page, maxResults);

            return Ok(characters);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Character), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Character>> Get([FromRoute]int id)
        {
            var character = _repository.GetCharacterById(id);
            if (character == null) return NotFound("Non-existent Id");

            return Ok(character);
        }

        [HttpGet("student")]
        [ProducesResponseType(typeof(Character), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Character>> Get([FromQuery] string school, int page, int maxResults)
        {
            var character = _repository.GetStudent(school, page, maxResults);
            if (character == null) return BadRequest("No match");

            return Ok(character);
        }

        [HttpPost("query")]
        [ProducesResponseType(typeof(Character), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Character>> PostFilter([FromBody] CharacterDto characterDto, int page, int maxResults)
        {
            var character = _repository.GetCharactersByBody(characterDto, page, maxResults);
            if (character == null) return BadRequest("No match");

            return Created("", character);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Character), StatusCodes.Status201Created)]
        public async Task<ActionResult<Character>> Post([FromBody] CharacterDto newCharacter)
        {        
            var character = _repository.InsertCharacter(newCharacter);

            return Created("", character);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Character), StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Character>> Delete([FromRoute] int id)
        {
            var character = _repository.RemoveCharacter(id);
            if (character == null) return NotFound("Non-existent Id");

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Character), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Character>> Put([FromRoute] int id, [FromBody] CharacterDto updatedCharacter)
        {
          var character = _repository.UpdateFullCharacter(id, updatedCharacter);
          if (character == null) return NotFound("Non-existent Id");

          return Ok(character);
        }

        [HttpPatch("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Character), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Character>> Patch([FromRoute] int id, [FromBody] CharacterProfessionDto updatedCharacter)
        {
            var character = _repository.UpdateCharacterProfession(id, updatedCharacter);
            if (character == null) return NotFound("Non-existent Id");

            return Ok(character);
        }
    }
}