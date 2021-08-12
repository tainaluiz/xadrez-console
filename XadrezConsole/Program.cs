using System;
using XadrezConsole.Jogo;
using XadrezConsole.Pecas;

namespace XadrezConsole {
    class Program {
        static void Main() {
            PartidaXadrez partidaXadrez = new PartidaXadrez();

            while (!partidaXadrez.Terminada) {
                Console.Clear();

                Console.WriteLine();
                Tela.ImprimirTabuleiro(partidaXadrez.Tabuleiro);
                Console.WriteLine();

                Console.WriteLine("Peças capturadas:");
                Tela.ImprimirPecas(partidaXadrez.Capturadas(Cor.Branca), Cor.Branca);
                Console.WriteLine();
                Tela.ImprimirPecas(partidaXadrez.Capturadas(Cor.Preta), Cor.Preta);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Turno: " + partidaXadrez.Turno);
                Console.WriteLine("Aguardando jogada: " + partidaXadrez.JogadorAtual);

                if (partidaXadrez.Xeque) {
                    Console.WriteLine("XEQUE!");
                }

                Console.WriteLine();
                Console.Write("Origem: ");

                try {
                    Posicao posicaoOrigem = Tela.LerPosicaoXadrez().ToPosicao();
                    partidaXadrez.ValidarOrigemJogada(posicaoOrigem);
                    bool[,] posicoesPossiveis = partidaXadrez.Tabuleiro.GetPeca(posicaoOrigem).MovimentosPossiveis();

                    Console.Clear();
                    Tela.ImprimirTabuleiro(partidaXadrez.Tabuleiro, posicoesPossiveis);

                    Console.WriteLine();
                    Console.Write("Destino: ");
                    Posicao posicaoDestino = Tela.LerPosicaoXadrez().ToPosicao();

                    partidaXadrez.ValidarDestinoJogada(posicaoOrigem, posicaoDestino);

                    partidaXadrez.RealizaJogada(posicaoOrigem, posicaoDestino);
                } catch (TabuleiroException e) {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
            }

            Console.Clear();
            Console.WriteLine("XEQUEMATE!");
            Console.WriteLine("Vencedor: " + partidaXadrez.JogadorAtual);
        }
    }
}
