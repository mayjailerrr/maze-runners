public delegate void TurnActionHandler(Player player, Piece ficha);
//Delegado TurnActionHandler: Para acciones que ocurren en cada turno (ej. movimiento de ficha, activación de habilidad).

// Eventos en Ficha:

//     OnMove: Evento que se dispara cuando una ficha se mueve.
//     OnUseHabilidad: Evento para cuando una ficha usa su habilidad.

// Esto permite manejar efectos, como la activación de trampas al pasar por ellas, o actualizar la interfaz gráfica.

// Eventos en Trap:

//     OnActivate: Se dispara cuando una ficha cae en una trampa.

// Evento GameOver en GameManager: Activa el final del juego cuando se cumple una condición de victoria.