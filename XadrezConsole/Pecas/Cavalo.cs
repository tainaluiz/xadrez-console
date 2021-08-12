using XadrezConsole.Jogo;
using XadrezConsole.Pecas;

namespace XadrezConsole.Pecas {
    class Cavalo : Peca {

        public Cavalo(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) {
        }

        public override bool[,] MovimentosPossiveis() {
            bool[,] mat = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

            Posicao posicao = new Posicao(0, 0);
            int[,] posicoes = {
                { -1, -2 },
                { -2, -1 },
                { -2, +1 },
                { -1, +2 },
                { +1, +2 },
                { +2, +1 },
                { +2, -1 },
                { +1, -2 }
            };

            for (int i = 0; i < posicoes.GetLength(0); i++) {
                posicao.DefinirPosicao(Posicao.Linha + posicoes[i, 0], Posicao.Coluna + posicoes[i, 1]);
                if (Tabuleiro.PosicaoValida(posicao) && PodeMover(posicao)) {
                    mat[posicao.Linha, posicao.Coluna] = true;
                }
            }

            return mat;
        }

        public override string ToString() {
            return "C";
        }
    }
}
