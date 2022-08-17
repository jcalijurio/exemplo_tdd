namespace LojaOnline
{
    public class Cliente
    {
        public Cliente(int id, string nome, bool prime)
        {
            Id = id;
            Nome = nome;
            Prime = prime;
        }

        public int Id { get; }
        public string Nome { get; }
        public bool Prime { get; }
    }
}
