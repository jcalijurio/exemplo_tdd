namespace LojaOnline
{
    public class Carrinho
    {
        private readonly IConsultaRH _consultaRH;

        public Carrinho(FormaPagamento formaPagamento, Cliente cliente, IConsultaRH consultaRH)
        {
            Itens = new List<Item>();
            FormaPagamento = formaPagamento;
            Cliente = cliente;
            _consultaRH = consultaRH;
        }

        public IList<Item> Itens { get; set; }
        public decimal Total { get; set; }
        public FormaPagamento FormaPagamento { get; }
        public Cliente Cliente { get; }


        public void Incluir(Item item)
        {
            if (item.Qtd == 0)
                return;

            if (!item.Disponivel)
                return;

            Limitar10Unidades(item);

            Itens.Add(item);

            SomarValores();
        }

        private void SomarValores()
        {
            Total = 0m;
            foreach (var it in Itens)
                Total += it.Valor * it.Qtd;

            if (Cliente.Prime)
            {
                Total *= .9m;
                return;
            }

            if (_consultaRH.EstaRegistrado(Cliente))
            {
                Total *= .92m;
                return;
            }

            if (FormaPagamento == FormaPagamento.Pix)
                Total *= .95m;
        }

        private static void Limitar10Unidades(Item item)
        {
            if (item.Qtd > 10 && item.Id != 4)
                item.Qtd = 10;
        }
    }
}
