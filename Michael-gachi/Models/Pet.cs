using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dojodachi.Models
{
    public class Pet
    {
        [Key]
        public int id {get;set;}
        public int Happiness {get;set;}
        public int Fullness {get;set;}
        public int Energy {get;set;}
        public int Meals {get;set;}

    }
}