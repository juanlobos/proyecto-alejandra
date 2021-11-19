using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Card
    {
        public Card(string name, string pan)
        {
            Id = System.Guid.NewGuid();
            Name = name;
            Pan = pan;
            Vigente = false;
        }

        public System.Guid Id { get; private set; }
        public string Name { get; set; }
        public string Pan { get; private set; }
        public string Pin { get; set; }
        public decimal Amount { get; set; }
        public bool Vigente { get; set; }
    }
}
