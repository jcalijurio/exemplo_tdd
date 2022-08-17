namespace ExemploTestes
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

        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int Qtd { get; set; }
        public bool Disponivel { get; set; }
    }
}
