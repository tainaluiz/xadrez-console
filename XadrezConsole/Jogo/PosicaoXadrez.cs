namespace XadrezConsole.Jogo {
    class PosicaoXadrez {

        public int Linha { get; set; }
        public char Coluna { get; set; }

        public PosicaoXadrez(char coluna, int linha) {
            Coluna = char.ToUpper(coluna);
            Linha = linha;
        }

        public Posicao ToPosicao() {
            return new Posicao(8 - Linha, Coluna - 'A');
        }

        public override string ToString() {
            return Coluna.ToString() + Linha.ToString();
        }
    }
}
