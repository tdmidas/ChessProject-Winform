using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ChessSharp;
using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System.Media;
using Stockfish.NET;
using System.Threading.Tasks;
namespace ChessUI;

public partial class SinglePlayer : Form
{
    IStockfish _stockfish = new Stockfish.NET.Stockfish(@"F:\ChessProject2\ChessUI\stockfish\stockfish_20090216_x64.exe");
  
    private readonly Label[] _squareLabels;
    private readonly Dictionary<string, Point> _whiteLocations;
    private readonly Dictionary<string, Point> _blackLocations;
    private Square _selectedSourceSquare;
    private ChessGame _gameBoard = new ChessGame();


    private static string InvertSquare(string sq)
    {
      
        var f = (char)('A' + 'H' - sq[4]);
        var r = '9' - sq[5];
        return "lbl_" + f + r;
    }

    public SinglePlayer()
    {
        InitializeComponent();

        // Initialize ListView columns
        listView1.View = View.Details;
        listView1.Columns.Add("Move #", 50);
        listView1.Columns.Add("White", 150);
        listView1.Columns.Add("Black", 150);

        _squareLabels = Controls.OfType<Label>()
                             .Where(m => Regex.IsMatch(m.Name, "lbl_[A-H][1-8]")).ToArray();

        Array.ForEach(_squareLabels, lbl =>
        {
            lbl.BackgroundImageLayout = ImageLayout.Zoom;
            lbl.Click += SquaresLabels_Click;
        });

        _whiteLocations = _squareLabels.ToDictionary(lbl => lbl.Name,
                                                    lbl => lbl.Location);

        _blackLocations = _squareLabels.ToDictionary(lbl => InvertSquare(lbl.Name),
                                                    lbl => lbl.Location);

        DrawBoard();
    }


    private void FlipUi(Player player)
    {
        if (checkBox1.Checked) return;
        var locationsDictionary = player == Player.White ? _whiteLocations : _blackLocations;
        Array.ForEach(_squareLabels, lbl => lbl.Location = locationsDictionary[lbl.Name]);
    }

    private Player? GetPlayerInCheck()
    {
        if (_gameBoard.GameState == GameState.BlackInCheck || _gameBoard.GameState == GameState.WhiteWinner)
        {
            return Player.Black;
        }
        if (_gameBoard.GameState == GameState.WhiteInCheck || _gameBoard.GameState == GameState.BlackWinner)
        {
            return Player.White;
        }
        return null;
    }

    private void SquaresLabels_Click(object? sender, EventArgs e)
    {
        Label selectedLabel = (Label)sender!;
        if (selectedLabel.BackColor != Color.DarkCyan)
        {
            // Re-draw to remove previously colored labels.
            DrawBoard(GetPlayerInCheck());

            if (selectedLabel.Tag!.ToString() != _gameBoard.WhoseTurn.ToString()) return;
            _selectedSourceSquare = selectedLabel.Name.AsSpan().Slice("lbl_".Length); // implicit conversion
            var validDestinations = ChessUtilities.GetValidMovesOfSourceSquare(_selectedSourceSquare, _gameBoard).Select(m => m.Destination).ToArray();
            if (validDestinations.Length == 0) return;
            selectedLabel.BackColor = Color.Cyan;
            Array.ForEach(validDestinations, square =>
                {
                    _squareLabels.First(lbl => lbl.Name == $"lbl_{square}").BackColor = Color.DarkCyan;
                });
        }
        else
        {
            MakeMove(_selectedSourceSquare, selectedLabel.Name.AsSpan().Slice("lbl_".Length));
        }
    }


    private void DrawBoard(Player? playerInCheck = null)
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var file = (File)i;
                var rank = (Rank)j;
                Label lbl = _squareLabels.First(m => m.Name == "lbl_" + file.ToString() + ((int)rank + 1));
                Piece? piece = _gameBoard[file, rank];
                lbl.BackColor = ((i + j) % 2 == 0) ? Color.FromArgb(181, 136, 99) : Color.FromArgb(240, 217, 181);
                if (piece == null)
                {
                    lbl.BackgroundImage = null;
                    lbl.Tag = "empty";
                    continue;
                }
                lbl.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject($"{piece.Owner}{piece.GetType().Name}")!;
                lbl.Tag = piece.Owner.ToString();
            }

        }

        if (playerInCheck == null) return;

        // Division => Rank             Modulus => File

        Square checkedKingSquare = _gameBoard.Board.SelectMany(x => x)
            .Select((p, i) => new { Piece = p, Square = new Square((File)(i % 8), (Rank)(i / 8)) })
            .First(m => m.Piece is King && m.Piece.Owner == playerInCheck).Square;
        _squareLabels.First(lbl => lbl.Name == "lbl_" + checkedKingSquare).BackColor = Color.Red;
    }

    private async void MakeMove(Square source, Square destination)
    {
        try
        {
            Player player = _gameBoard.WhoseTurn;
            PawnPromotion? pawnPromotion = null;
            if (_gameBoard[source.File, source.Rank] is Pawn)
            {
                if ((player == Player.White && destination.Rank == Rank.Eighth) ||
                    (player == Player.Black && destination.Rank == Rank.First))
                {
                    //var promotion = Interaction.InputBox("Promote to what ?", "Promotion").ToLower();
                    // Interaction.InputBox isn't supported in .NET Core currently.
                    // Consider using it and remove InputBox if it became supported in future release.
                    string? promotion;
                    using (var inputBox = new InputBox())
                    {
                        inputBox.ShowDialog();
                        promotion = inputBox.UserInput;
                    }
                    pawnPromotion = (PawnPromotion)Enum.Parse(typeof(PawnPromotion), promotion!, true);
                }
            }

            var move = new Move(source, destination, player, pawnPromotion);
            if (!_gameBoard.IsValidMove(move))
            {
                MessageBox.Show("Invalid Move!", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            _gameBoard.MakeMove(move, isMoveValidated: true);

            DrawBoard(GetPlayerInCheck());

            // Log move to ListView
            string moveNotation = SquareToNotation(source) + SquareToNotation(destination);
            if (player == Player.White)
            {
                listView1.Items.Add(new ListViewItem(new[] { (listView1.Items.Count + 1).ToString(), moveNotation, "" }));
            }
            else
            {
                listView1.Items[listView1.Items.Count - 1].SubItems[2].Text = moveNotation;
            }

            if (_gameBoard.GameState == GameState.Draw || _gameBoard.GameState == GameState.Stalemate ||
                _gameBoard.GameState == GameState.BlackWinner || _gameBoard.GameState == GameState.WhiteWinner)
            {
                MessageBox.Show(_gameBoard.GameState.ToString());
                return;
            }
            SoundPlayer moveSoundPlayer = new SoundPlayer(Properties.Resources.Move);
            moveSoundPlayer.Play();
            Player whoseTurn = _gameBoard.WhoseTurn;
            lblWhoseTurn.Text = whoseTurn.ToString();
            FlipUi(whoseTurn);
            if (whoseTurn == Player.Black) // Assuming Stockfish plays as Black
            {
                string fen = _gameBoard.GetFen();
                _stockfish.SetFenPosition(fen);
                string bestMove = await Task.Run(() => _stockfish.GetBestMove());
                if (bestMove != null)
                {
                    Square src = ParseSquare(bestMove.Substring(0, 2));
                    Square dest = ParseSquare(bestMove.Substring(2, 2));
                    MakeMove(src, dest);
                }
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Error\n{exception.Message}\n\n{exception.StackTrace}", "Chess", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private string SquareToNotation(Square square)
    {
        return $"{(char)('a' + (int)square.File)}{(int)square.Rank + 1}";
    }
    private string MoveToNotation(Move move)
    {
        return SquareToNotation(move.Source) + SquareToNotation(move.Destination);
    }
    private Square ParseSquare(string squareString)
    {
        File file = (File)(squareString[0] - 'a');
        Rank rank = (Rank)(squareString[1] - '1');
        return new Square(file, rank);
    }
    private void SinglePlayer_Load(object sender, EventArgs e)
    {

    }
}
