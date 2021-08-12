using System;
using System.Collections.Generic;
using XadrezConsole.Jogo;
using XadrezConsole.Pecas;

namespace XadrezConsole {
    class Tela {

        public static void ImprimirTabuleiro(Tabuleiro tabuleiro) {
            ImprimirTabuleiro(tabuleiro, null);
        }

        public static void ImprimirTabuleiro(Tabuleiro tabuleiro, bool[,] posicoesPossiveis) {
            if (tabuleiro != null) {
                ConsoleColor corFundoPadrao = Console.BackgroundColor;
                for (int i = 0; i < tabuleiro.Linhas; i++) {
                    Console.Write((8 - i).ToString() + " ");
                    for (int j = 0; j < tabuleiro.Colunas; j++) {
                        if (IsPosicaoPossivel(posicoesPossiveis, i, j)) {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                        }
                        ImprimirPeca(tabuleiro, i, j);
                        Console.BackgroundColor = corFundoPadrao;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("  a b c d e f g h");
            }
        }

        public static void ImprimirPecas(HashSet<Peca> pecas, Cor cor) {
            Console.Write($"{cor}s - ");
            Console.Write("[ ");
            foreach (Peca peca in pecas) {
                if (cor == Cor.Preta) {
                    ConsoleColor corPadrao = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca + " ");
                    Console.ForegroundColor = corPadrao;
                } else {
                    Console.Write(peca + " ");
                }
            }
            Console.Write("]");
        }

        private static bool IsPosicaoPossivel(bool[,] posicoesPossiveis, int linha, int coluna) {
            try {
                return posicoesPossiveis != null && posicoesPossiveis[linha, coluna];
            } catch (Exception) {
                return false;
            }
        }

        private static void ImprimirPeca(Tabuleiro tabuleiro, int linha, int coluna) {
            if (tabuleiro != null) {
                Peca peca = tabuleiro.GetPeca(new Posicao(linha, coluna));
                if (peca == null) {
                    Console.Write("-");
                } else {
                    if (peca.Cor == Cor.Preta) {
                        ConsoleColor colorAux = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(peca);
                        Console.ForegroundColor = colorAux;
                    } else {
                        Console.Write(peca);
                    }
                }
                Console.Write(" ");
            }
        }

        public static PosicaoXadrez LerPosicaoXadrez() {
            string s = Console.ReadLine();
            try {
                char coluna = s[0];
                int linha = int.Parse(s[1].ToString());
                return new PosicaoXadrez(coluna, linha);
            } catch (Exception) {
                throw new TabuleiroException("Posição inválida!");
            }
        }
    }
}
