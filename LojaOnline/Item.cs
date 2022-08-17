namespace LojaOnline
{
    public class Item
    {
        public Item(int id, string descricao, decimal valor, int qtd, bool disponivel)
        {
            Id = id;
            Descricao = descricao;
            Valor = valor;
            Qtd = qtd;
            Disponivel = disponivel;
        }

        public int Id { get; }
        public string Descricao { get; }
        public decimal Valor { get; }
        public int Qtd { get; set; }
        public bool Disponivel { get; }
    }
}
