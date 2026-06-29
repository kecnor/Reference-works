using NUnit.Framework;
using UnityEngine;
public class GameBoardTests
{
    private GameObject go;
    private GameBoard board;

    [SetUp]
    public void SetUp()
    {
        go = new GameObject("TestBoard");
        board = go.AddComponent<GameBoard>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }
    private GameObject MakePiece(string tag)
    {
        var boardpiece = new GameObject(tag);
        boardpiece.tag = tag;
        return boardpiece;
    }

    [Test]
    public void Gameboard_AddBoardPieceTest()
    {
        board.addBoardPiece(Vector3.zero, MakePiece("Tile"));
        Assert.AreEqual(1, board.GameBoardPieces.Count);
    }

    [Test]
    public void Gameboard_AddBoardPiece_StoresObjecTestt()
    {
        var boardpiece = MakePiece("Tile");
        board.addBoardPiece(Vector3.zero, boardpiece);
        Assert.AreSame(boardpiece, board.GameBoardPieces[Vector3.zero]);
    }

    [Test]
    public void Gameboard_AddBoardPiece_MultiplePiecesTest()
    {
        board.addBoardPiece(new Vector3(0, 0, 0), MakePiece("Tile"));
        board.addBoardPiece(new Vector3(1, 0, 0), MakePiece("Tile"));
        board.addBoardPiece(new Vector3(2, 0, 0), MakePiece("Plus"));
        Assert.AreEqual(3, board.GameBoardPieces.Count);
    }

    [Test]
    public void Gameboard_RemoveBoardPieceTest()
    {
        var position = Vector3.zero;
        board.addBoardPiece(position, MakePiece("Tile"));
        board.removeBoardPiece(position);
        Assert.AreEqual(0, board.GameBoardPieces.Count);
    }

    [Test]
    public void Gameboard_RemoveBoardPiece_RemoveKeyTest()
    {
        var position = Vector3.zero;
        board.addBoardPiece(position, MakePiece("Tile"));
        board.removeBoardPiece(position);
        Assert.IsFalse(board.GameBoardPieces.ContainsKey(position));
    }

    [Test]
    public void Gameboard_RemoveBoardPiece_NonExistentKey_DoesNotThrowTest()
    {
        Assert.DoesNotThrow(() => board.removeBoardPiece(Vector3.zero));
    }

    [Test]
    public void Gameboard_OnPositionTest()
    {
        var position = Vector3.zero;
        board.addBoardPiece(position, MakePiece("Tile"));
        Assert.IsTrue(board.onPosition(position));
    }

    [Test]
    public void Gameboard_OnPosition_ForEmptyBoardTest()
    {
        Assert.IsFalse(board.onPosition(Vector3.zero));
    }

    [Test]
    public void Gameboard_OnPosition_ReturnsFalseAfterRemoveTest()
    {
        var position = Vector3.zero;
        board.addBoardPiece(position, MakePiece("Tile"));
        board.removeBoardPiece(position);
        Assert.IsFalse(board.onPosition(position));
    }

    [Test]
    public void Gameboard_GetGameObjectTest()
    {
        var position = Vector3.zero;
        var boardpiece = MakePiece("Tile");
        board.addBoardPiece(position, boardpiece);
        Assert.AreSame(boardpiece, board.GetGameObject(position));
    }

    [Test]
    public void Gameboard_GetGameObject_ReturnsNullForMissingPositionTest()
    {
        Assert.IsNull(board.GetGameObject(new Vector3(99, 99, 99)));
    }

    [Test]
    public void Gameboard_GetRandomTilePositionTest()
    {
        board.addBoardPiece(new Vector3(0, 0, 0), MakePiece("Tile"));
        board.addBoardPiece(new Vector3(1, 0, 0), MakePiece("Plus"));
        for (int i = 0; i < 10; i++)
        {
            Vector3 position = board.GetRandomTilePosition();
            Assert.AreEqual("Tile", board.GetGameObject(position).tag);
        }
    }

    [Test]
    public void Gameboard_GetRandomTilePosition_WithSingleTileTest()
    {
        var pos = Vector3.zero  ;
        board.addBoardPiece(pos, MakePiece("Tile"));

        Assert.AreEqual(pos, board.GetRandomTilePosition());
    }
}