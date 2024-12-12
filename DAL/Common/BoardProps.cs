namespace SeaBattle.Common;

public class BoardProps {
    public int OneDeck { get; }
    public int TwoDeck { get; }
    public int ThreeDeck { get; }
    public int FourDeck { get; }
    public int Size { get; }

    public BoardProps(int oneDeck = 4, int twoDeck = 3, int threeDeck = 2, int fourDeck = 1, int size = 10)
    {
        OneDeck = oneDeck;
        TwoDeck = twoDeck;
        ThreeDeck = threeDeck;
        FourDeck = fourDeck;
        Size = size;
    }
}