using XadrezConsole.Jogo;
using XadrezConsole.Pecas;

namespace XadrezConsole.Pecas {
    class Torre : Peca {

        public Torre(Tabuleiro tabuleiro, Cor cor) : base(tabuleiro, cor) {
        }

        public override bool[,] MovimentosPossiveis() {
            bool[,] mat = new bool[Tabuleiro.Linhas, Tabuleiro.Colunas];

            int linhaInicial = Posicao.Linha;
            int colunaInicial = Posicao.Coluna;

            VerificarLinhaColuna(linhaInicial, colunaInicial, -1, 0, mat);
            VerificarLinhaColuna(linhaInicial, colunaInicial, +1, 0, mat);
            VerificarLinhaColuna(linhaInicial, colunaInicial, 0, +1, mat);
            VerificarLinhaColuna(linhaInicial, colunaInicial, 0, -1, mat);

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
            return "T";
        }
    }
}
