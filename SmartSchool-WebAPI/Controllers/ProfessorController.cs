using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartSchool_WebAPI.Data;
using SmartSchool_WebAPI.Models;

namespace SmartSchool_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository _repo;
        public ProfessorController(IRepository _repo)
        {
            this._repo = _repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var professores = await _repo.GetAllProfessoresAsync(true);

                return Ok(professores);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("{professorId}")]
        public async Task<IActionResult> GetByProfessorId(int professorId)
        {
            try
            {
                var professor = await _repo.GetProfessorAsyncById(professorId, true);

                return Ok(professor);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpGet("ByAluno/{alunoId}")]
        public async Task<IActionResult> GetByAlunoId(int alunoId)
        {
            try
            {
                var professores = await _repo.GetProfessoresAsyncByAlunoId(alunoId, true);

                return Ok(professores);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Professor professor)
        {
            try
            {
                _repo.Add(professor);

                if(await _repo.SaveChangesAsync()) return Ok(professor);

                return BadRequest("Não foi possível inserir professor.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpPut("{professorId}")]
        public async Task<IActionResult> Put(int professorId, Professor professor)
        {
            try
            {
                if(professorId != professor.Id) return  BadRequest("Ids diferentes");

                var result = await _repo.GetProfessorAsyncById(professorId, false);

                if(result == null) return NotFound("Professor não encontrado.");

                _repo.Update(professor);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok(professor);
                }

                return BadRequest("Erro inseperado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }

        [HttpDelete("{professorId}")]
        public async Task<IActionResult> Delete(int professorId)
        {
            try
            {
                var professor = await _repo.GetProfessorAsyncById(professorId, false);

                if(professor == null) return NotFound("Professor não encontrado");

                _repo.Delete(professor);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok("Professor deletado com sucesso.");
                }

                return BadRequest("Erro inesperado");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex.Message}");
            }
        }
    }
}