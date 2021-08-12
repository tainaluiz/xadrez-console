namespace XadrezConsole.Jogo {
    class Posicao {

        public int Linha { get; private set; }
        public int Coluna { get; private set; }

        public Posicao(int linha, int coluna) {
            Linha = linha;
            Coluna = coluna;
        }

        public void DefinirPosicao(int linha, int coluna) {
            Linha = linha;
            Coluna = coluna;
        }

        public override string ToString() {
            return $"Posicao {Linha}, {Coluna}";
        }
    }
}
