
// using System.Collections.Generic;
// using MazeRunners;
// using System;
// public static class PieceAbilities
// {
//     static Dictionary<string, Func<Context, bool>> abilities = new Dictionary<string, Func<Context, bool>>
//     {
//        // { "Forest Spirit", Teletransport }, 
//       //  { "Shape Shift", ChangeShapeAndAvoidTrapsForOneTurn }, 
//         { "Ethereal Shield", ReducesDamageFromDamageTrapsForTwoTurns },
//         { "Swift Wind", AugmentSpeedForOneTurn }, 
//         { "Illusory Echo", CreateCloneAndDistractEnemyOneTurn }
//     };

//     //COPILOT
//     // private static bool Teletransport(Context context)
//     // {
//     //     var random = new Random();
//     //     var randomTile = Board.Instance.Tiles[random.Next(0, Board.Instance.Tiles.Count)];

//     //     context.CurrentTile = randomTile;
//     //     context.CurrentPosition.Add(randomTile);

//     //     return true;
//     // }

//     // private static bool ChangeShapeAndAvoidTrapsForOneTurn(Context context)
//     // {
//     //    // context.CurrentPlayer.Shape = Shape.Bird;
//     //     //context.CurrentPlayer.AvoidTraps = true;

//     //     return true;
//     // }
    
//     private static bool ReducesDamageFromDamageTrapsForTwoTurns(Context context)
//     {
//         context.CurrentPlayer.ReduceDamageFromDamageTraps = true;

//         return true;
//     }

//     private static bool AugmentSpeedForOneTurn(Context context)
//     {
//         context.CurrentPlayer.Speed += 2;

//         return true;
//     }

//     private static bool CreateCloneAndDistractEnemyOneTurn(Context context)
//     {
//         var clone = new Player(context.CurrentPlayer.Name + "Clone", context.CurrentPlayer.Shape, context.CurrentPlayer.Speed, context.CurrentPlayer.AvoidTraps, context.CurrentPlayer.ReduceDamageFromDamageTraps);
//         context.EnemyPlayer = clone;

//         return true;
//     }

//     public static bool ExecuteAbility(string abilityName, Context context)
//     {
//         if (abilities.ContainsKey(abilityName))
//         {
//             return abilities[abilityName](context);
//         }

//         return false;
//     }

//     //GEPETO 1

//     // Habilidad que teletransporta la ficha actual a una posición aleatoria
//     // private static bool Teletransport(Context context)
//     // {
//     //     var randomTile = context.Board.GetRandomAccessibleTile();
//     //     if (randomTile != null)
//     //     {
//     //         context.CurrentFicha.MoveTo(randomTile);  // Mueve la ficha a la nueva posición
//     //         return true;
//     //     }
//     //     return false;
//     // }

//     // // Habilidad que permite evitar trampas en el próximo turno
//     // private static bool ChangeShapeAndAvoidTrapsForOneTurn(Context context)
//     // {
//     //     context.CurrentFicha.IsTrapImmune = true;
//     //     context.CurrentFicha.SetTrapImmunityTurns(1); // Evita las trampas durante el próximo turno
//     //     return true;
//     // }

//     // // Habilidad que reduce el daño recibido de trampas por dos turnos
//     // private static bool ReducesDamageFromDamageTrapsForTwoTurns(Context context)
//     // {
//     //     context.CurrentFicha.IsProtectedFromTraps = true;
//     //     context.CurrentFicha.SetTrapProtectionTurns(2); // Activa la protección durante 2 turnos
//     //     return true;
//     // }

//     // // Aumenta la velocidad de la ficha actual por un turno
//     // private static bool AugmentSpeedForOneTurn(Context context)
//     // {
//     //     context.CurrentFicha.Speed += 2; // Aumenta la velocidad en 2 casillas (o el valor que prefieras)
//     //     context.CurrentFicha.SetSpeedBuffTurns(1); // Aplica el efecto por un turno
//     //     return true;
//     // }

//     // // Crea un clon ilusorio de la ficha actual para distraer al enemigo por un turno
//     // private static bool CreateCloneAndDistractEnemyOneTurn(Context context)
//     // {
//     //     var clone = context.CurrentFicha.CreateClone();
//     //     context.Board.PlaceFicha(clone, context.CurrentFicha.Position); // Coloca el clon en la misma casilla
//     //     context.CurrentFicha.HasCloneActive = true;
//     //     context.CurrentFicha.SetCloneDuration(1); // Duración de un turno
//     //     return true;
//     // }


// //     //GEPETO 2
// //     // Teletransporta la ficha a una casilla aleatoria en el rango del tablero
// //     public static bool Teletransport(Context context)
// //     {
// //         var ficha = context.CurrentFicha;
// //         var board = context.Board;
        
// //         Tile randomTile = board.GetRandomAccessibleTile(); // Método que obtiene una casilla aleatoria accesible
// //         ficha.Position = randomTile; // Actualiza la posición de la ficha
// //         return true;
// //     }

// //     // Cambia de forma para evitar trampas durante un turno
// //     public static bool ChangeShapeAndAvoidTrapsForOneTurn(Context context)
// //     {
// //         var ficha = context.CurrentFicha;
// //         ficha.IsTrapImmune = true; // Activar inmunidad a trampas
// //         ficha.AddStatusEffect(StatusEffect.TrapImmunity, duration: 1); // Añadir efecto temporal de un turno
// //         return true;
// //     }

// //     // Reduce el daño de trampas de daño durante dos turnos
// //     public static bool ReducesDamageFromDamageTrapsForTwoTurns(Context context)
// //     {
// //         var ficha = context.CurrentFicha;
// //         ficha.AddStatusEffect(StatusEffect.DamageResistance, duration: 2); // Efecto de resistencia al daño por 2 turnos
// //         return true;
// //     }

// //     // Aumenta la velocidad de movimiento de la ficha durante un turno
// //     public static bool AugmentSpeedForOneTurn(Context context)
// //     {
// //         var ficha = context.CurrentFicha;
// //         ficha.Speed += 2; // Incremento temporal de velocidad
// //         ficha.AddStatusEffect(StatusEffect.SpeedBoost, duration: 1); // Añadir efecto temporal de velocidad
// //         return true;
// //     }

// //     // Crea un clon de la ficha en una casilla cercana para distraer al enemigo por un turno
// //     public static bool CreateCloneAndDistractEnemyOneTurn(Context context)
// //     {
// //         var board = context.Board;
// //         var ficha = context.CurrentFicha;
        
// //         Tile adjacentTile = board.GetAdjacentTile(ficha.Position); // Método para obtener una casilla adyacente
// //         Ficha clone = ficha.CreateClone(); // Crear un clon de la ficha
// //         clone.Position = adjacentTile; // Colocar el clon en una casilla adyacente
// //         clone.AddStatusEffect(StatusEffect.DistractEnemy, duration: 1); // Duración de un turno para distracción
// //         board.AddTemporaryFicha(clone); // Añadir clon al tablero temporalmente
// //         return true;
// //     }


// }