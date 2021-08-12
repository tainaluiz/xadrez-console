using XadrezConsole.Jogo;

namespace XadrezConsole.Pecas {
    abstract class Peca {

        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QteMovimentos { get; protected set; }
        public Tabuleiro Tabuleiro { get; protected set; }
        public PartidaXadrez PartidaXadrez { get; private set; }

        public Peca(Tabuleiro tabuleiro, Cor cor) {
            this.Tabuleiro = tabuleiro;
            this.Cor = cor;
        }

        public Peca(Tabuleiro tabuleiro, Cor cor, PartidaXadrez partidaXadrez): this(tabuleiro, cor) {
            this.PartidaXadrez = partidaXadrez;
        }

        public void IncrementarMovimentos() {
            QteMovimentos++;
        }

        public void DecrementarMovimentos() {
            QteMovimentos--;
        }

        public bool ExisteMovimentosPossiveis() {
            bool[,] mat = MovimentosPossiveis();
            for (int i = 0; i < Tabuleiro.Linhas; i++) {
                for (int j = 0; j < Tabuleiro.Colunas; j++) {
                    if (mat[i, j]) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao posicao) {
            return MovimentosPossiveis()[posicao.Linha, posicao.Coluna];
        }
        public bool PodeMover(Posicao posicao) {
            Peca peca = Tabuleiro.GetPeca(posicao);
            return peca == null || peca.Cor != Cor;
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
