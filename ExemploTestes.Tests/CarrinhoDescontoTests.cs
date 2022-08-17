using Moq;

namespace ExemploTestes.Tests
{
    public class CarrinhoDescontoTests
    {
        private readonly Cliente _cliente;
        private readonly IConsultaRH _consultaRH;

        public CarrinhoDescontoTests()
        {
            _cliente = new Cliente { Id = 1, Nome = "José", Prime = false };
            var mock = new Mock<IConsultaRH>();
            mock.Setup(x => x.EstaRegistrado(It.IsAny<Cliente>())).Returns(false);
            _consultaRH = mock.Object;
        }

        [Fact]
        public void DescontoCarrinho_PagamentoPix_Recebe5Porcento()
        {
            // Arrange
            var carrinho = new Carrinho(FormaPagamento.Pix, _cliente, _consultaRH);
            var item1 = new Item(1, "Camiseta", 29.90m, 1, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 2, true);
            var item3 = new Item(3, "Meia", 12m, 3, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);
            carrinho.Incluir(item3);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.Equal(145.7m * .95m, carrinho.Total);
        }

        [Fact]
        public void DescontoCarrinho_ClientePrime_Recebe10Porcento()
        {
            // Arrange
            var cliente = new Cliente { Id = 2, Nome = "Maria", Prime = true };
            var carrinho = new Carrinho(FormaPagamento.Boleto, cliente, _consultaRH);
            var item1 = new Item(1, "Camiseta", 29.90m, 1, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 2, true);
            var item3 = new Item(3, "Meia", 12m, 3, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);
            carrinho.Incluir(item3);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.Equal(145.7m * .9m, carrinho.Total);
        }

        [Fact]
        public void DescontoCarrinho_ClienteEhFuncionario_Recebe8Porcento()
        {
            // Arrange
            var mock = new Mock<IConsultaRH>();
            mock.Setup(x => x.EstaRegistrado(It.IsAny<Cliente>())).Returns(true);
            var carrinho = new Carrinho(FormaPagamento.Boleto, _cliente, mock.Object);
            var item1 = new Item(1, "Camiseta", 29.90m, 1, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 2, true);
            var item3 = new Item(3, "Meia", 12m, 3, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);
            carrinho.Incluir(item3);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            Assert.Equal(145.7m * .92m, carrinho.Total);
            mock.Verify(x => x.EstaRegistrado(It.IsAny<Cliente>()), Times.AtLeastOnce());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DescontoCarrinho_ClienteComVariosDescontos_RecebeApenasMaiorDesconto(bool prime)
        {
            // Arrange
            var cliente = new Cliente { Id = 2, Nome = "Maria", Prime = prime };
            var mock = new Mock<IConsultaRH>();
            mock.Setup(x => x.EstaRegistrado(It.IsAny<Cliente>())).Returns(true);
            var carrinho = new Carrinho(FormaPagamento.Pix, cliente, mock.Object);
            var item1 = new Item(1, "Camiseta", 29.90m, 1, true);
            var item2 = new Item(2, "Bermuda", 39.90m, 2, true);
            var item3 = new Item(3, "Meia", 12m, 3, true);

            // Act
            carrinho.Incluir(item1);
            carrinho.Incluir(item2);
            carrinho.Incluir(item3);

            // Assert
            Assert.NotEmpty(carrinho.Itens);
            var total = 145.7m;
            if (prime)
            {
                Assert.Equal(total * .9m, carrinho.Total);
                mock.Verify(x => x.EstaRegistrado(It.IsAny<Cliente>()), Times.Never());
            }
            else
            {
                Assert.Equal(total * .92m, carrinho.Total);
                mock.Verify(x => x.EstaRegistrado(It.IsAny<Cliente>()), Times.AtLeastOnce());
            }
        }
    }
}
