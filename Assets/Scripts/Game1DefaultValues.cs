public static class Game1DefaultValues
    {
        //Configura el tamaño del tablero
        public static readonly int rows = 12;
        public static readonly int columns = 8;
        
        //Regular la velocidad de la animación 
        public static float moveAnimationMinLength = 0.3f; //0.05f;
        public static readonly float animLength = 0.2f;
        public static readonly float explosionLength = 0.3f;

        //Valores de las pistas para potenciales matches
        public static readonly float waitForHints = 2f;
        public static readonly float opacityFrameDelay = 0.05f;
        
        //valores por defecto para match-3    
        public static readonly int minMatches = 3;
        public static readonly int minMatchesForBonus = 4;
        public static readonly int match3Score = 60;
        public static readonly int subsequentMatchesScore = 100;
    }

   

