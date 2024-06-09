using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI_DotNet8.Data;
using SuperHeroAPI_DotNet8.Entities;


namespace SuperHeroAPI_DotNet8.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {   
        //criando uma variável para injetar o DataContext no Controller
        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]//método read
        public async Task<ActionResult<List<SuperHero>>> GetAllHeroes()
        {
            var heroes = await _context.SuperHeroes.ToListAsync();

            return Ok(heroes);
        }

        [HttpGet("{id}")]//método read com id
       
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero is null)
                return NotFound("Hero not found.");//NotFound é o erro 404 e Badrequestion é o erro 400 


            return Ok(hero);
        }

        [HttpPost]// método create
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {

            //vamos fazer isso aqui, mas ele não salva a alteração
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();//isso aqui salva a alteração

            return Ok(await _context.SuperHeroes.ToListAsync());//isso Vai fazer o retorno da lista, só para mostrar que foi atualizado

        }

        [HttpPut]// método update

        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero updatedHero)
        {
            //primeiro queremos que ele encontre o heroi pelo id
            var dbHero = await _context.SuperHeroes.FindAsync(updatedHero.Id);

            if (dbHero is null)//se o Heroi é vazia significa que não achou
            {
                return NotFound("Heroi não encontrado");
            }
            else//caso a id tenha valor
            {
                //aqui eu altero todas as caracteristicas do antigo com o novo que eu recebi no parâmetro
                dbHero.Name = updatedHero.Name;
                dbHero.FirstName = updatedHero.FirstName;
                dbHero.LastName = updatedHero.LastName;
                dbHero.Place = updatedHero.Place;

                //salvar a alteração no banco de dados
                await _context.SaveChangesAsync();

                return Ok(await _context.SuperHeroes.ToListAsync());//vai retorna a lista com todos os herois
            }    
        }

        [HttpDelete]//método delete

        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {   
            //context vai no banco
            //SuperHeroes vai na tabela
            //FindAsync, atráves do id eu retorno todos os valores referente aquele id
            var dbHero = await _context.SuperHeroes.FindAsync(id);

            if (dbHero is null)
                return NotFound("Heroi nao encontrado.");

            _context.SuperHeroes.Remove(dbHero);//deletamos isso
            await _context.SaveChangesAsync();// salvamos a mudança

            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}
