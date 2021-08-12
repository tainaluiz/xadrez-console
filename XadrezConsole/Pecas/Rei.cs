using XadrezConsole.Jogo;
using XadrezConsole.Pecas;

namespace XadrezConsole.Pecas {
    class Rei : Peca {

        public Rei(Tabuleiro tabuleiro, Cor cor, PartidaXadrez partidaXadrez) : base(tabuleiro, cor, partidaXadrez) {
        }

        private bool TesteTorreParaRoque(Posicao posicao) {
            Peca peca = Tabuleiro.GetPeca(posicao);
            return peca is Torre && peca.Cor == Cor && peca.QteMovimentos == 0;
        }

        public override bool[,] MovimentosPossiveis() {
            bool[,] mat = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

            Posicao posicao = new Posicao(0, 0);

            for (int i = 1; i >= -1; i--) {
                for (int j = 1; j >= -1; j--) {
                    posicao.DefinirPosicao(Posicao.Linha + i, Posicao.Coluna + j);
                    if (Tabuleiro.PosicaoValida(posicao) && PodeMover(posicao)) {
                        mat[posicao.Linha, posicao.Coluna] = true;
                    }
                }
            }

            // jogadas especiais
            if (QteMovimentos == 0 && !PartidaXadrez.Xeque) {
                // roque pequeno
                Posicao posicaoTorre = new Posicao(Posicao.Linha, Posicao.Coluna + 3);
                if (TesteTorreParaRoque(posicaoTorre)) {
                    if (!Tabuleiro.ExistePeca(new Posicao(Posicao.Linha, Posicao.Coluna + 1))
                        && !Tabuleiro.ExistePeca(new Posicao(Posicao.Linha, Posicao.Coluna + 2))) {
                        mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                    }
                }

                // roque grande
                Posicao posicaoTorre2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
                if (TesteTorreParaRoque(posicaoTorre2)) {
                    if (!Tabuleiro.ExistePeca(new Posicao(Posicao.Linha, Posicao.Coluna - 1))
                        && !Tabuleiro.ExistePeca(new Posicao(Posicao.Linha, Posicao.Coluna - 2))
                        && !Tabuleiro.ExistePeca(new Posicao(Posicao.Linha, Posicao.Coluna - 3))) {
                        mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                    }
                }
            }

            return mat;
        }

        public override string ToString() {
            return "K";
        }
    }
}
