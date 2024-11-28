using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Este script controla la lógica de las casillas en un tablero de ajedrez dentro de Unity, incluyendo el manejo de interacciones y su posición.

- Referencia al controlador de juego: La variable `GameController` permite la comunicación con el controlador principal del juego.
- Variables de posición: Define los límites `HighestRankY` y `LowestRankY` para el rango de posiciones verticales en el tablero, que se utilizan para calcular nombres algebraicos.
- Inicialización: En el método `Start`, si `GameController` no ha sido asignado, se busca automáticamente un objeto de tipo `GameController`. Luego, se calcula el nombre algebraico de la casilla actual basándose en su posición y lo asigna al nombre del padre del objeto.
- Manejo de clics: El método `OnMouseDown` se activa cuando se hace clic en una casilla. Si hay una pieza seleccionada y esta está en movimiento, se bloquea la interacción para evitar conflictos. Si no hay movimiento, la pieza seleccionada se mueve a la posición de la casilla actual.
*/


public class BoxController : MonoBehaviour
{
    public GameController GameController;

    public float HighestRankY = 3.5f;
    public float LowestRankY = -3.5f;

    // Use this for initialization
    
    void Start()
    {
        if (GameController == null) GameController = FindObjectOfType<GameController>();

        string algebraicName = "";
        algebraicName += (char)(this.transform.position.x - LowestRankY + 'A');
        algebraicName += this.transform.position.y - LowestRankY + 1;
        this.transform.parent.name = algebraicName;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (GameController.SelectedPiece != null && GameController.SelectedPiece.GetComponent<PieceController>().IsMoving() == true)
        {
            // Prevent clicks during movement
            return;
        }

        if (GameController.SelectedPiece != null)
        {
            GameController.SelectedPiece.GetComponent<PieceController>().MovePiece(this.transform.position);
        }
    }
}
