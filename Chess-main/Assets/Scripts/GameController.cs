using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Este script controla la lógica del flujo del juego de ajedrez en Unity, incluyendo el manejo de turnos, selección de piezas y verificación de condiciones de finalización de la partida.

- Inicialización: Contiene métodos de inicio y actualización, aunque están vacíos.
- Selección de piezas: Permite seleccionar una pieza en función de su color y el turno actual (blancas o negras). Al seleccionar una pieza, se destaca visualmente cambiando su color y se ajusta su posición en el eje z para estar "por encima" de otras piezas.
- Deselección de piezas: Quita el destacado de la pieza seleccionada y la coloca de nuevo al nivel de otras piezas en el tablero.
- Manejo de turnos: Alterna el turno entre jugadores blancos y negros al final de cada movimiento. Además, se verifica si el rey está en jaque, si hay movimientos válidos disponibles para cada bando, y determina si el juego termina en jaque mate o en tablas.
- Validación de movimientos: Comprueba si una pieza seleccionada tiene movimientos válidos disponibles dentro de las reglas del juego, incluyendo la detección de enemigos en las casillas objetivo.
- Condiciones de finalización: Implementa los métodos para manejar las condiciones de tablas y jaque mate, mostrando mensajes de depuración.
*/

public class GameReset : MonoBehaviour
{
    
}



public class GameController : MonoBehaviour
{
    public GameObject Board;
    public GameObject WhitePieces;
    public GameObject BlackPieces;
    public GameObject SelectedPiece;
    public bool WhiteTurn = true;

    // Use this for initialization
    void Start()
    {

    }

    // Método para reiniciar el tablero
    public void ResetBoard()
    {
        // Vuelve a cargar la escena actual para reiniciar el estado del tablero
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectPiece(GameObject piece)
    {
        if (piece.tag == "White" && WhiteTurn == true || piece.tag == "Black" && WhiteTurn == false)
        {
            DeselectPiece();
            SelectedPiece = piece;

            // Highlight
            SelectedPiece.GetComponent<SpriteRenderer>().color = Color.yellow;

            // Put above other pieces
            Vector3 newPosition = SelectedPiece.transform.position;
            newPosition.z = -1;
            SelectedPiece.transform.SetPositionAndRotation(newPosition, SelectedPiece.transform.rotation);
        }
    }

    public void DeselectPiece()
    {
        if (SelectedPiece != null)
        {
            // Remove highlight
            SelectedPiece.GetComponent<SpriteRenderer>().color = Color.white;

            // Put back on the same level as other pieces
            Vector3 newPosition = SelectedPiece.transform.position;
            newPosition.z = 0;
            SelectedPiece.transform.SetPositionAndRotation(newPosition, SelectedPiece.transform.rotation);

            SelectedPiece = null;
        }
    }

    public void EndTurn()
    {
        bool kingIsInCheck = false;
        bool hasValidMoves = false;

        WhiteTurn = !WhiteTurn;

        if (WhiteTurn)
        {
            foreach (Transform piece in WhitePieces.transform)
            {
                if (hasValidMoves == false && HasValidMoves(piece.gameObject))
                {
                    hasValidMoves = true;
                }

                if (piece.name.Contains("Pawn"))
                {
                    piece.GetComponent<PieceController>().DoubleStep = false;
                }
                else if (piece.name.Contains("King"))
                {
                    kingIsInCheck = piece.GetComponent<PieceController>().IsInCheck(piece.position);
                }
            }
        }
        else
        {
            foreach (Transform piece in BlackPieces.transform)
            {
                if (hasValidMoves == false && HasValidMoves(piece.gameObject))
                {
                    hasValidMoves = true;
                }

                if (piece.name.Contains("Pawn"))
                {
                    piece.GetComponent<PieceController>().DoubleStep = false;
                }
                else if (piece.name.Contains("King"))
                {
                    kingIsInCheck = piece.GetComponent<PieceController>().IsInCheck(piece.position);
                }
            }
        }

        if (hasValidMoves == false)
        {
            if (kingIsInCheck == false)
            {
                Stalemate();
            }
            else
            {
                Checkmate();
            }
        }
    }

    bool HasValidMoves(GameObject piece)
    {
        PieceController pieceController = piece.GetComponent<PieceController>();
        GameObject encounteredEnemy;

        foreach (Transform square in Board.transform)
        {
            if (pieceController.ValidateMovement(piece.transform.position, new Vector3(square.position.x, square.position.y, piece.transform.position.z), out encounteredEnemy))
            {
                Debug.Log(piece + " on " + square);
                return true;
            }
        }
        return false;
    }

    void Stalemate()
    {
        Debug.Log("Stalemate!");
    }

    void Checkmate()
    {
        Debug.Log("Checkmate!");
    }
}
