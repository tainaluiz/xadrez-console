using XadrezConsole.Jogo;
using XadrezConsole.Pecas;

namespace XadrezConsole.Pecas {
    class Rainha : Peca {

        public Rainha(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) {
        }

        public override bool[,] MovimentosPossiveis() {
            bool[,] mat = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

            int linhaInicial = Posicao.Linha;
            int colunaInicial = Posicao.Coluna;

            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++) {
                    VerificarLinhaColuna(linhaInicial, colunaInicial, i, j, mat);
                }
            }

            return mat;
        }

        private void VerificarLinhaColuna(int linhaInicial, int colunaInicial, int incrementoLinha, int incrementoColuna, bool[,] mat) {
            if (mat == null) {
                return;
            }

            Posicao posicao = new Posicao(linhaInicial + incrementoLinha, colunaInicial + incrementoColuna);

            while (Tabuleiro.PosicaoValida(posicao) && PodeMover(posicao)) {
                mat[posicao.Linha, posicao.Coluna] = true;
                Peca peca = Tabuleiro.GetPeca(posicao);
                if (peca != null && peca.Cor != Cor) {
                    break;
                }
                posicao.DefinirPosicao(posicao.Linha + incrementoLinha, posicao.Coluna + incrementoColuna);
            }
        }

        public override string ToString() {
            return "Q";
        }
    }
}
