using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models {
    public class Platform{
        public int Id { get; set; } 
        public int ExternalID { get; set; }        
        public string Name { get; set; }
        public ICollection<Command> Comand {get;set;} = new List<Command>();
    }
}