using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;


namespace RpgApi.Models
{

        public class Usuario
        {
            public int Id { get; set;}//Atalho para propriedade (PROP + TAB)
            public String? Username { get; set; }
            public byte[]? PasswordHash { get; set; }
            public byte[]? PasswordSalt { get; set; }
            public byte[]? Foto { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public DateTime? DataAcesso { get; set; } //using System;

            [NotMapped] //using System.ComponentModel.DataAnnotations.Schema
            public string? PasswordString { get; set; }
            public List<Personagem>? Personagens { get;set; } //using System.Collections.Generic;
            public string? Perfil { get; set; }
            public string? Email { get; set; } 

        }
}