// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using MazeRunners;

// public class DoubleClick : MonoBehaviour
// {
//     GameManager gameManager;
//     PieceController pieceController;
//     Camera mainCamera;
//     float firstClickTime = 0;
//     float limitTime = 0.25f;
//     bool check = true;
//     int clickCount = 0;

//     bool active => this.gameObject.name.Contains("NO SE KE COND PONER") ? !Board.Instance.AreEnemiesPlayingOrAboutToPlay : Board.Instance.AreEnemiesPlayingOrAboutToPlay;

//     private void Start()
//     {
//         pieceController = this.gameObject.GetComponent<PieceController>();
//         gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
//         //cambiar camaras
//         mainCamera = this.gameObject.name.Contains("NO SE KE COND PONER") ? gameManager.cameras[1] : gameManager.cameras[2];
//     }

//     private void Update()
//     {
//         //chequear los paneles
//         if (active && Input.GetMouseButtonUp(0) && !(gameManager.IsAnyInfoActive()) && gameManager.IsPlayersPanelActive())
//         {
//             Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//             RaycastHit hit;

//             if (Physics.Raycast(ray, out hit))
//             {
//                 if (hit.transform.gameObject == gameObject)
//                 {
//                     clickCount++;
//                     if (check && clickCount == 1)
//                     {
//                         firstClickTime = Time.time;
//                         StartCoroutine(DoubleClickAction());
//                     }
//                 }
//             }
//         }
//     }

//     IEnumerator DoubleClickAction()
//     {
//         check = false;
//         while (Time.time - firstClickTime < limitTime)
//         {
//             if (clickCount>=2)
//             {
//                 pieceController.OpenInfoPanel();
//                 break;
//             }
//             yield return new WaitForEndOfFrame();
//         }
//         clickCount = 0;
//         check = true;
//     }
// }
