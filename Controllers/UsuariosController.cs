using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using System.Threading.Tasks;
using RpgApi.Models;
using RpgApi.Utils;
using System.Collections.Generic;


[ApiController]
[Route("[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly DataContext _context;
    public UsuariosController(DataContext context)
    {
        _context = context;
    }
    public async Task<bool> UsuarioExistente(string username)
    {
        if (await _context.Usuarios.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
        {
            return true;
        }
        return false;
    }

    [HttpPost("Registrar")]
    public async Task<IActionResult> RegistrarUsuario(Usuario user)
    {
        try
        {
            if (await UsuarioExistente(user.Username))
                throw new System.Exception("Nome de usuário já existe");

            Criptografia.CriarPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
            user.PasswordString = string.Empty;
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            await _context.Usuarios.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user.Id);

        }
        catch (System.Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }
    [HttpPost("Autenticar")]
    public async Task<IActionResult> AutenticarUsario(Usuario credenciais)
    {
        try
        {
            Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(credenciais.Username.ToLower()));
            if (usuario == null)
            {
                throw new System.Exception("Usuário não encontrado");
            }
            else if (!Criptografia.VerificarPasswordHash(credenciais.PasswordString, usuario.PasswordHash, usuario.PasswordSalt))
            {
                throw new System.Exception("Senha Incorreta");
            }
            else
            {
                //Desafio 3 - OK
                Usuario u = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == credenciais.Username);
                u.DataAcesso = DateTime.Now;
                _context.Usuarios.Update(u);
                int linhasAfetadas = await _context.SaveChangesAsync();


                return Ok(usuario.Id);
            }
        }
        catch (System.Exception ex)
        {

            return BadRequest(ex.Message);
        }

    }
    //Desafio - 1 - OK
    [HttpPut("AlterarSenha")]
    public async Task<IActionResult> Update(Usuario user)
    {
        try
        {
            if (!await UsuarioExistente(user.Username))
            {
                throw new System.Exception("Nome de usuário não existe");
            }

            Usuario? u = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == user.Username);

            Criptografia.CriarPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
            u.PasswordString = string.Empty;
            u.PasswordHash = hash;
            u.PasswordSalt = salt;

            _context.Usuarios.Update(u);

            int linhasAfetadas = await _context.SaveChangesAsync();

            return Ok(linhasAfetadas);

        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //Desafio - 2 - OK
    [HttpGet("TodosUsuarios")]
    public async Task<IActionResult> Get()
    {
        try
        {
            List<Usuario> lista = await _context.Usuarios.ToListAsync();
            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{usuarioId}")]
    public async Task<IActionResult> GetUsuario(int usuarioId)
    {
        try
        {
            //List exigirá o using System.Collections.Generic
            Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == usuarioId);//Busca o usuario no bando através do Id.

            return Ok(usuario);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("GetLogin/{login}")]
    public async Task<IActionResult> GetUsuario(string login)
    {
        try
        {
            Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username.ToLower() == login.ToLower());

            return Ok(usuario);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //Método para alterar a geolocalização.
    [HttpPut("AtualizarLocalizacao")]
    public async Task<IActionResult> AtualizarLocalizacao(Usuario u)
    {
        try
        {
            Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == u.Id);

            usuario.Latitude = u.Latitude;
            usuario.Longitude = u.Longitude;

            var attach = _context.Attach(usuario);
            attach.Property(x => x.Id).IsModified = false;
            attach.Property(x => x.Latitude).IsModified = true;
            attach.Property(x => x.Longitude).IsModified = true;

            int linhasAfetadas = await _context.SaveChangesAsync();//Confirma a alteração no banco.
            return Ok(linhasAfetadas);//Retorna as linhas afetadas (geralmente sempre 1 linha msm).
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //Método para alteração do e-mail.
    [HttpPut("AtualizarEmail")]
    public async Task<IActionResult> AtualizarEmail(Usuario u)
    {
        try
        {
            Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == u.Id);//Busca o usuario no banco atraves do Id.

            usuario.Email = u.Email;

            var attach = _context.Attach(usuario);
            attach.Property(x => x.Id).IsModified = false;
            attach.Property(x => x.Email).IsModified = true;
            int linhasAfetadas = await _context.SaveChangesAsync();//Confirma a alteração no banco.
            return Ok(linhasAfetadas);//Retorna as linhas afetadas (geralmente sempre 1 linha msm).
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //Método para alteração da foto
    [HttpPut("AtualizarFoto")]
    public async Task<IActionResult> AtualizarFoto(Usuario u)
    {
        try
        {
            Usuario usuario = await _context.Usuarios.FirstAsync(x => x.Id == u.Id);

            usuario.Foto = u.Foto;
            var attach = _context.Attach(usuario);
            attach.Property(x => x.Id).IsModified = false;
            attach.Property(x => x.Foto).IsModified = true;

            int linhasAfetadas = await _context.SaveChangesAsync();
            return Ok(linhasAfetadas);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    public async Task<IActionResult> Add(Arma novaArma)
    {
        try
        {
            if (novaArma.Dano == 0)
            {
                throw new System.Exception("O dano da arma não pode ser 0");
            }

            Personagem? personagem = await _context.Personagens.FirstOrDefaultAsync(p => p.Id == novaArma.PersonagemId);

            if (personagem == null) throw new System.Exception("Seu usuario não contém personagens com o Id Personagem informado.");

            Arma? BuscaArma = await _context.Armas.FirstOrDefaultAsync(a => a.PersonagemId == novaArma.PersonagemId);

            if (BuscaArma != null) throw new System.Exception("O Personagem selecionado já contém uma arma atribuida a ele.");

            await _context.Armas.AddAsync(novaArma);
            await _context.SaveChangesAsync();
            return Ok(novaArma.Id);

        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}