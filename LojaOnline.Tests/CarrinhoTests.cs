using Moq;

namespace LojaOnline.Tests
{
    public class CarrinhoTests
    {
        private readonly Cliente _cliente;
        private readonly IConsultaRH _consultaRH;

        public CarrinhoTests()
        {
            _cliente = new Cliente(1, "José", false);
            var mock = new Mock<IConsultaRH>();
            mock.Setup(x => x.EstaRegistrado(It.IsAny<Cliente>())).Returns(false);
            _consultaRH = mock.Object;
        }

        [Fact]
        public void NovoCarrinho_IncluirItem_AtualizarLista()
        {
            // Arrange
            var carrinho = new Carrinho(FormaPagamento.Boleto, _cliente, _consultaRH);
            var item = new Item(1, "Camiseta", 29.90m, 1, true);

            // Act
            carrinho.Incluir(item);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.Contains(item, carrinho.Itens);
        }

        [Fact]
        public void NovoCarrinho_IncluirItem_SomarValores()
        {
            // Arrange
            var carrinho = new Carrinho(FormaPagamento.Boleto, _cliente, _consultaRH);
            var item1 = new Item(1, "Camiseta", 29.90m, 1, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 2, true);
            var item3 = new Item(3, "Meia", 12m, 3, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);
            carrinho.Incluir(item3);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.Equal(145.7m, carrinho.Total);
        }

        [Fact]
        public void NovoCarrinho_Mais10Unidades_Limitar10UnidadesPorItem()
        {
            // Arrange
            var carrinho = new Carrinho(FormaPagamento.Boleto, _cliente, _consultaRH);
            var item1 = new Item(1, "Camiseta", 29.90m, 11, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 12, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.All(carrinho.Itens, item => Assert.Equal(10, item.Qtd));
        }

        [Fact]
        public void NovoCarrinho_Item0Unidades_RetirarDoCarrinho()
        {
            // Arrange
            var carrinho = new Carrinho(FormaPagamento.Boleto, _cliente, _consultaRH);
            var item1 = new Item(1, "Camiseta", 29.90m, 3, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 0, true);
            var item3 = new Item(3, "Meia", 12m, 2, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);
            carrinho.Incluir(item3);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.Contains(item1, carrinho.Itens);
            Assert.DoesNotContain(item2, carrinho.Itens);
            Assert.Contains(item3, carrinho.Itens);
        }

        [Fact]
        public void NovoCarrinho_ItemIndisponivel_RetirarDoCarrinho()
        {
            // Arrange
            var carrinho = new Carrinho(FormaPagamento.Boleto, _cliente, _consultaRH);
            var item1 = new Item(1, "Camiseta", 29.90m, 3, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 1, false);
            var item3 = new Item(3, "Meia", 12m, 2, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);
            carrinho.Incluir(item3);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.DoesNotContain(item2, carrinho.Itens);
        }

        [Fact]
        public void NovoCarrinho_IncluirItemId4_ItemId4RetirarDoCarrinho()
        {
            // Arrange
            var carrinho = new Carrinho(FormaPagamento.Boleto, _cliente, _consultaRH);
            var item1 = new Item(1, "Camiseta", 29.90m, 13, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 11, false);
            var item3 = new Item(3, "Meia", 12m, 2, true);
            var item4 = new Item(4, "Cueca", 12m, 99, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);
            carrinho.Incluir(item3);
            carrinho.Incluir(item4);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.All(carrinho.Itens, item =>
            {
                if (item.Id == 4)
                    Assert.Equal(99, item.Qtd);
                else
                    Assert.True(item.Qtd <= 10);
            });
        }
    }
}
