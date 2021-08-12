using XadrezConsole.Pecas;

namespace XadrezConsole.Jogo {
    class Tabuleiro {

        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] Pecas;

        public Tabuleiro(int linhas, int colunas) {
            this.Linhas = linhas;
            this.Colunas = colunas;
            Pecas = new Peca[linhas, colunas];
        }

        private Peca GetPeca(int linha, int coluna) {
            return Pecas[linha, coluna];
        }

        public Peca GetPeca(Posicao posicao) {
            ValidarPosicao(posicao);
            return GetPeca(posicao.Linha, posicao.Coluna);
        }

        public bool ExistePeca(Posicao posicao) {
            return GetPeca(posicao) != null;
        }

        public void ColocarPeca(Peca peca, Posicao posicao) {
            ValidarPosicaoOcupada(posicao);
            Pecas[posicao.Linha, posicao.Coluna] = peca;
            peca.Posicao = posicao;
        }

        public void ColocarPeca(Peca peca, PosicaoXadrez posicaoXadrez) {
            if (posicaoXadrez == null) {
                throw new TabuleiroException("Posição inválida!");
            }
            ColocarPeca(peca, posicaoXadrez.ToPosicao());
        }

        public Peca RetirarPeca(Posicao posicao) {
            Peca peca = GetPeca(posicao);
            if (peca != null) {
                peca.Posicao = null;
                Pecas[posicao.Linha, posicao.Coluna] = null;
            }
            return peca;
        }

        public bool PosicaoValida(Posicao posicao) {
            return posicao != null && posicao.Linha >= 0 && posicao.Linha < Linhas && posicao.Coluna >= 0 && posicao.Coluna < Colunas;
        }

        public void ValidarPosicao(Posicao posicao) {
            if (!PosicaoValida(posicao)) {
                throw new TabuleiroException("Posição inválida!");
            }
        }

        public void ValidarPosicaoOcupada(Posicao posicao) {
            ValidarPosicao(posicao);
            if (ExistePeca(posicao)) {
                throw new TabuleiroException("Já existe uma peça nessa posição!");
            }
        }
    }
}
