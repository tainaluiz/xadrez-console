using System.Collections.Generic;
using System.Linq;
using XadrezConsole.Pecas;

namespace XadrezConsole.Jogo {
    class PartidaXadrez {

        public Tabuleiro Tabuleiro { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }
        private HashSet<Peca> _pecas;
        private HashSet<Peca> _capturadas;

        public PartidaXadrez() {
            Tabuleiro = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            _pecas = new HashSet<Peca>();
            _capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        private Peca ExecutaMovimento(Posicao origem, Posicao destino) {
            Peca peca = Tabuleiro.RetirarPeca(origem);
            peca.IncrementarMovimentos();
            Peca pecaCapturada = Tabuleiro.RetirarPeca(destino);
            Tabuleiro.ColocarPeca(peca, destino);
            if (pecaCapturada != null) {
                _capturadas.Add(pecaCapturada);
            }

            /** Jogadas especiais **/
            if (peca is Rei) {
                // roque pequeno
                if (destino.Coluna == origem.Coluna + 2) {
                    Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                    Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                    Peca torre = Tabuleiro.RetirarPeca(origemTorre);
                    torre.IncrementarMovimentos();
                    Tabuleiro.ColocarPeca(torre, destinoTorre);
                }

                // roque grande
                if (destino.Coluna == origem.Coluna - 2) {
                    Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                    Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                    Peca torre = Tabuleiro.RetirarPeca(origemTorre);
                    torre.IncrementarMovimentos();
                    Tabuleiro.ColocarPeca(torre, destinoTorre);
                }
            }

            if (peca is Peao) {
                // em passant
                if (origem.Coluna != destino.Coluna && pecaCapturada == null) {
                    int linhaAux = peca.Cor == Cor.Branca ? 1 : -1;
                    Posicao posicaoAux = new Posicao(destino.Linha + linhaAux, destino.Coluna);
                    pecaCapturada = Tabuleiro.RetirarPeca(posicaoAux);
                    _capturadas.Add(pecaCapturada);
                }
            }
            /** Fim jogadas especiais **/

            return pecaCapturada;
        }

        private void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca peca = Tabuleiro.RetirarPeca(destino);
            peca.DecrementarMovimentos();
            if (pecaCapturada != null) {
                Tabuleiro.ColocarPeca(pecaCapturada, destino);
                _capturadas.Remove(pecaCapturada);
            }
            Tabuleiro.ColocarPeca(peca, origem);

            /** Jogadas especiais **/
            if (peca is Rei) {
                // roque pequeno
                if (destino.Coluna == origem.Coluna + 2) {
                    Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                    Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                    Peca torre = Tabuleiro.RetirarPeca(destinoTorre);
                    torre.IncrementarMovimentos();
                    Tabuleiro.ColocarPeca(torre, origemTorre);
                }

                // roque grande
                if (destino.Coluna == origem.Coluna - 2) {
                    Posicao origemTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                    Posicao destinoTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                    Peca torre = Tabuleiro.RetirarPeca(destinoTorre);
                    torre.IncrementarMovimentos();
                    Tabuleiro.ColocarPeca(torre, origemTorre);
                }
            }

            if (peca is Peao) {
                // em passant
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant) {
                    Peca peao = Tabuleiro.RetirarPeca(destino);
                    int linhaAux = peca.Cor == Cor.Branca ? 3 : 4;
                    Posicao posicaoAux = new Posicao(linhaAux, destino.Coluna);
                    Tabuleiro.ColocarPeca(peao, posicaoAux);
                }
            }
            /** Fim jogadas especiais **/
        }

        public void RealizaJogada(Posicao origem, Posicao destino) {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (EstaEmXeque(JogadorAtual)) {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Xeque = EstaEmXeque(Adversario(JogadorAtual));

            if (TesteXequemate(Adversario(JogadorAtual))) {
                Terminada = true;
                return;
            }

            Turno++;
            JogadorAtual = JogadorAtual == Cor.Branca ? Cor.Preta : Cor.Branca;

            Peca peca = Tabuleiro.GetPeca(destino);

            // jogadas especiais
            if (peca is Peao) {
                // promocao
                if ((peca.Cor == Cor.Branca && destino.Linha == 0) || (peca.Cor == Cor.Preta && destino.Linha == 7)) {
                    peca = Tabuleiro.RetirarPeca(destino);
                    _pecas.Remove(peca);
                    Peca rainha = new Rainha(Tabuleiro, peca.Cor);
                    Tabuleiro.ColocarPeca(rainha, destino);
                    _pecas.Add(rainha);
                }

                // jogada especial en passant
                if (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2) {
                    VulneravelEnPassant = peca;
                } else {
                    VulneravelEnPassant = null;
                }
            }
        }

        public void ValidarOrigemJogada(Posicao posicao) {
            Peca peca = Tabuleiro.GetPeca(posicao);
            if (peca == null) {
                throw new TabuleiroException("Não existe peça na posição escolhida!");
            } else if (JogadorAtual != peca.Cor) {
                throw new TabuleiroException("A peça escolhida é do outro jogador!");
            } else if (!peca.ExisteMovimentosPossiveis()) {
                throw new TabuleiroException("Não há movimentos possíveis para a peça escolhida!");
            }
        }

        public void ValidarDestinoJogada(Posicao posicaoOrigem, Posicao posicaoDestino) {
            Peca peca = Tabuleiro.GetPeca(posicaoOrigem);
            if (!peca.MovimentoPossivel(posicaoDestino)) {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        public void ColocarPeca(char coluna, int linha, Peca peca) {
            Tabuleiro.ColocarPeca(peca, new PosicaoXadrez(coluna, linha));
            _pecas.Add(peca);
        }

        private void ColocarPecas() {
            ColocarPeca('A', 1, new Torre(Tabuleiro, Cor.Branca));
            ColocarPeca('B', 1, new Cavalo(Tabuleiro, Cor.Branca));
            ColocarPeca('C', 1, new Bispo(Tabuleiro, Cor.Branca));
            ColocarPeca('D', 1, new Rainha(Tabuleiro, Cor.Branca));
            ColocarPeca('E', 1, new Rei(Tabuleiro, Cor.Branca, this));
            ColocarPeca('F', 1, new Bispo(Tabuleiro, Cor.Branca));
            ColocarPeca('G', 1, new Cavalo(Tabuleiro, Cor.Branca));
            ColocarPeca('H', 1, new Torre(Tabuleiro, Cor.Branca));
            ColocarPeca('A', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarPeca('B', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarPeca('C', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarPeca('D', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarPeca('E', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarPeca('F', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarPeca('G', 2, new Peao(Tabuleiro, Cor.Branca, this));
            ColocarPeca('H', 2, new Peao(Tabuleiro, Cor.Branca, this));

            ColocarPeca('A', 8, new Torre(Tabuleiro, Cor.Preta));
            ColocarPeca('B', 8, new Cavalo(Tabuleiro, Cor.Preta));
            ColocarPeca('C', 8, new Bispo(Tabuleiro, Cor.Preta));
            ColocarPeca('D', 8, new Rainha(Tabuleiro, Cor.Preta));
            ColocarPeca('E', 8, new Rei(Tabuleiro, Cor.Preta, this));
            ColocarPeca('F', 8, new Bispo(Tabuleiro, Cor.Preta));
            ColocarPeca('G', 8, new Cavalo(Tabuleiro, Cor.Preta));
            ColocarPeca('H', 8, new Torre(Tabuleiro, Cor.Preta));
            ColocarPeca('A', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarPeca('B', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarPeca('C', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarPeca('D', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarPeca('E', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarPeca('F', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarPeca('G', 7, new Peao(Tabuleiro, Cor.Preta, this));
            ColocarPeca('H', 7, new Peao(Tabuleiro, Cor.Preta, this));
        }

        public HashSet<Peca> Capturadas(Cor cor) {
            return _capturadas.Where(p => p.Cor == cor).ToHashSet();
        }

        public HashSet<Peca> Pecas(Cor cor) {
            return _pecas.Where(p => p.Cor == cor).Except(Capturadas(cor)).ToHashSet();
        }

        private Cor Adversario(Cor cor) {
            return cor == Cor.Branca ? Cor.Preta : Cor.Branca;
        }

        private Peca GetRei(Cor cor) {
            return Pecas(cor).FirstOrDefault(p => p is Rei);
        }

        private bool EstaEmXeque(Cor cor) {
            Peca rei = GetRei(cor);
            foreach (Peca p in Pecas(Adversario(cor))) {
                bool[,] mat = p.MovimentosPossiveis();
                if (mat[rei.Posicao.Linha, rei.Posicao.Coluna]) {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequemate(Cor cor) {
            if (!EstaEmXeque(cor)) {
                return false;
            }
            foreach (Peca peca in Pecas(cor)) {
                bool[,] mat = peca.MovimentosPossiveis();
                for (int i = 0; i < Tabuleiro.Linhas; i++) {
                    for (int j = 0; j < Tabuleiro.Colunas; j++) {
                        if (mat[i, j]) {
                            Posicao origem = peca.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

    }
}
