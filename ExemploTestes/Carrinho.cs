namespace ExemploTestes
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

        public FormaPagamento FormaPagamento { get; }
        public IList<Item> Itens { get; }
        public decimal Total { get; private set; }
        public Cliente Cliente { get; }

        public void Incluir(Item item)
        {
            Limitar10Itens(item);

            if (item.Qtd == 0)
                return;

            if (!item.Disponivel)
                return;

            Itens.Add(item);
            SomarTotal();
        }

        private static void Limitar10Itens(Item item)
        {
            if (item.Qtd > 10 && item.Id != 4)
                item.Qtd = 10;
        }

        // Deve-se zerar o total antes de reprocessar.
        public void SomarTotal()
        {
            Total = 0;
            foreach (var item in Itens)
                Total += item.Valor * item.Qtd;

            if (Cliente.Prime)
            {
                Total *= 0.9m;
                return;
            }

            if (_consultaRH.EstaRegistrado(Cliente))
            {
                Total *= 0.92m;
                return;
            }

            if (FormaPagamento == FormaPagamento.Pix)
                Total *= 0.95m;
        }
    }
}
